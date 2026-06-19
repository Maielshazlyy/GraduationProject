using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Service_layer.DTOS.Voice;
using Service_layer.Services;
using Service_layer.Services_Interfaces;
using Xunit;

namespace Tests
{
    public class CustomerVoiceServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IAiVoiceJoinService> _aiJoinMock = new();
        private readonly Mock<IAuditLogService> _auditLogMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<IInteractionRepository> _interactionRepoMock = new();
        private readonly Mock<ICallSummaryRepository> _callSummaryRepoMock = new();

        private CustomerVoiceService CreateService(string meetingUrl = "https://meeting.example.com")
        {
            _uowMock.Setup(u => u.Businesses).Returns(_businessRepoMock.Object);
            _uowMock.Setup(u => u.Customers).Returns(_customerRepoMock.Object);
            _uowMock.Setup(u => u.Interactions).Returns(_interactionRepoMock.Object);
            _uowMock.Setup(u => u.CallSummaries).Returns(_callSummaryRepoMock.Object);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["AiVoiceApi:MeetingUrl"] = meetingUrl
                })
                .Build();

            return new CustomerVoiceService(_uowMock.Object, _aiJoinMock.Object, config, _auditLogMock.Object);
        }

        private static Business FakeBusiness(string id) => new() { BusinessId = id, Name = "Biz" };
        private static Customer FakeCustomer(string id) => new() { CustomerId = id };

        // ── StartCallAsync ───────────────────────────────────────────────────

        [Fact]
        public async Task StartCallAsync_ValidRequest_ReturnsInteractionIdAndMeetingUrl()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiJoinMock.Setup(a => a.JoinCallAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                       .Returns(Task.CompletedTask);
            _auditLogMock.Setup(a => a.LogInteractionActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null))
                         .Returns(Task.CompletedTask);

            var service = CreateService("https://meeting.example.com");
            var result  = await service.StartCallAsync(new StartVoiceCallRequestDTO
            {
                BusinessId = "biz-1",
                CustomerId = "cust-1"
            });

            Assert.NotNull(result);
            Assert.Equal("https://meeting.example.com", result.MeetingUrl);
            Assert.Equal("connecting", result.Status);
            Assert.False(string.IsNullOrWhiteSpace(result.InteractionId));
            _interactionRepoMock.Verify(r => r.AddAsync(It.IsAny<Interaction>()), Times.Once);
        }

        [Fact]
        public async Task StartCallAsync_NotifiesAiWithCustomerIdAsIdentifier()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-42")).ReturnsAsync(FakeCustomer("cust-42"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _auditLogMock.Setup(a => a.LogInteractionActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), null))
                         .Returns(Task.CompletedTask);

            var service = CreateService();
            await service.StartCallAsync(new StartVoiceCallRequestDTO
            {
                BusinessId = "biz-1",
                CustomerId = "cust-42"
            });

            // AI must receive customerId as customer_identifier
            _aiJoinMock.Verify(a => a.JoinCallAsync(
                It.IsAny<string>(),
                "biz-1",
                "cust-42"), Times.Once);
        }

        [Fact]
        public async Task StartCallAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.StartCallAsync(new StartVoiceCallRequestDTO
                {
                    BusinessId = "bad",
                    CustomerId = "cust-1"
                }));
        }

        [Fact]
        public async Task StartCallAsync_CustomerNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.StartCallAsync(new StartVoiceCallRequestDTO
                {
                    BusinessId = "biz-1",
                    CustomerId = "bad"
                }));
        }

        [Fact]
        public async Task StartCallAsync_MeetingUrlNotConfigured_ThrowsInvalidOperationException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));

            // Pass empty string as MeetingUrl
            var service = CreateService(meetingUrl: "");
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.StartCallAsync(new StartVoiceCallRequestDTO
                {
                    BusinessId = "biz-1",
                    CustomerId = "cust-1"
                }));
        }

        // ── HandleCallCompletedAsync ─────────────────────────────────────────

        [Fact]
        public async Task HandleCallCompletedAsync_ValidPayload_SavesCallSummary()
        {
            var interaction = new Interaction
            {
                InteractionId = "inter-1",
                BusinessId    = "biz-1",
                Status        = "Open"
            };

            _interactionRepoMock.Setup(r => r.GetByIdAsync("inter-1")).ReturnsAsync(interaction);
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            await service.HandleCallCompletedAsync(BuildPayload("inter-1", escalate: false));

            _callSummaryRepoMock.Verify(r => r.AddAsync(It.IsAny<CallSummary>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task HandleCallCompletedAsync_NoEscalation_ClosesInteraction()
        {
            var interaction = new Interaction
            {
                InteractionId = "inter-2",
                BusinessId    = "biz-1",
                Status        = "Open"
            };

            _interactionRepoMock.Setup(r => r.GetByIdAsync("inter-2")).ReturnsAsync(interaction);
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            await service.HandleCallCompletedAsync(BuildPayload("inter-2", escalate: false));

            Assert.Equal("Closed", interaction.Status);
            Assert.True(interaction.IsEnded);
        }

        [Fact]
        public async Task HandleCallCompletedAsync_EscalationRequired_SetsInteractionToEscalated()
        {
            var interaction = new Interaction
            {
                InteractionId = "inter-3",
                BusinessId    = "biz-1",
                Status        = "Open"
            };

            _interactionRepoMock.Setup(r => r.GetByIdAsync("inter-3")).ReturnsAsync(interaction);
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            await service.HandleCallCompletedAsync(BuildPayload("inter-3", escalate: true));

            Assert.Equal("Escalated", interaction.Status);
        }

        [Fact]
        public async Task HandleCallCompletedAsync_BusinessNotFound_ThrowsArgumentException()
        {
            // Interaction not found either — business_id comes from payload
            _interactionRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Interaction?)null);
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service  = CreateService();
            var payload  = BuildPayload("inter-x", escalate: false);
            payload.BusinessId = "bad-biz";

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.HandleCallCompletedAsync(payload));
        }

        // ── Helpers ──────────────────────────────────────────────────────────

        private static VoiceCallCompletedDTO BuildPayload(string callId, bool escalate)
        {
            var now = DateTime.UtcNow;
            return new VoiceCallCompletedDTO
            {
                BusinessId = "biz-1",
                CallData   = new VoiceCallDataDTO
                {
                    CallId          = callId,
                    StartTime       = now.AddMinutes(-5),
                    EndTime         = now,
                    DurationSeconds = 300,
                    MessagesCount   = 4
                },
                Analysis = new VoiceAnalysisDTO
                {
                    Summary            = "Call summary",
                    EscalationRequired = escalate,
                    OverallSentiment   = new VoiceSentimentDTO { Score = 0.8, Label = "Positive" }
                }
            };
        }
    }
}
