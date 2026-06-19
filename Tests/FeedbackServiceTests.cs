using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.Feedback;
using Service_layer.Services;
using Service_layer.Services_Interfaces;
using Xunit;

namespace Tests
{
    public class FeedbackServiceTests
    {
        private readonly Mock<IFeedbackRepository> _feedbackRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<ITicketRepository> _ticketRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IAuditLogService> _auditLogMock = new();

        private FeedbackService CreateService() =>
            new(_feedbackRepoMock.Object, _customerRepoMock.Object,
                _ticketRepoMock.Object, _uowMock.Object, _auditLogMock.Object);

        private static Customer FakeCustomer(string id, string? bizId = null) =>
            new() { CustomerId = id, BusinessId = bizId };

        private static Ticket FakeTicket(string id) =>
            new() { Id = id };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidFeedback_ReturnsFeedback()
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1", "biz-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _auditLogMock.Setup(a => a.CreateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .ReturnsAsync(new AuditLog { AuditLogId = "log-1", BusinessId = "biz-1", Action = "CreateFeedback", Entity = "Feedback", EntityId = "fb-1" });

            var service = CreateService();
            var result  = await service.CreateAsync(new FeedbackCreateDTO
            {
                CustomerId = "cust-1",
                Rating     = 5,
                Comment    = "Great service!"
            });

            Assert.NotNull(result);
            Assert.Equal(5, result.Rating);
            Assert.Equal("cust-1", result.CustomerId);
            _feedbackRepoMock.Verify(r => r.AddAsync(It.IsAny<Feedback>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithTicketId_LinksFeedbackToTicket()
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _ticketRepoMock.Setup(r => r.GetByIdAsync("ticket-1")).ReturnsAsync(FakeTicket("ticket-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CreateAsync(new FeedbackCreateDTO
            {
                CustomerId = "cust-1",
                TicketId   = "ticket-1",
                Rating     = 4
            });

            Assert.Equal("ticket-1", result.TicketId);
        }

        [Fact]
        public async Task CreateAsync_CustomerNotFound_ThrowsArgumentException()
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new FeedbackCreateDTO { CustomerId = "bad", Rating = 4 }));
        }

        [Fact]
        public async Task CreateAsync_InvalidTicketId_ThrowsArgumentException()
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _ticketRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Ticket?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new FeedbackCreateDTO
                {
                    CustomerId = "cust-1",
                    TicketId   = "ghost-ticket",
                    Rating     = 3
                }));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(-1)]
        public async Task CreateAsync_RatingOutOfRange_ThrowsArgumentException(int rating)
        {
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new FeedbackCreateDTO { CustomerId = "cust-1", Rating = rating }));
        }

        // ── UpdateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_ValidRating_UpdatesFeedback()
        {
            var feedback = new Feedback { FeedbackId = "fb-1", Rating = 3, Comment = "OK" };
            _feedbackRepoMock.Setup(r => r.GetByIdAsync("fb-1")).ReturnsAsync(feedback);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.UpdateAsync("fb-1", new FeedbackUpdateDTO { Rating = 5, Comment = "Excellent!" });

            Assert.Equal(5, result!.Rating);
            Assert.Equal("Excellent!", result.Comment);
        }

        [Fact]
        public async Task UpdateAsync_FeedbackNotFound_ReturnsNull()
        {
            _feedbackRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Feedback?)null);

            var service = CreateService();
            var result  = await service.UpdateAsync("ghost", new FeedbackUpdateDTO { Rating = 4 });

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRating_ThrowsArgumentException()
        {
            var feedback = new Feedback { FeedbackId = "fb-2" };
            _feedbackRepoMock.Setup(r => r.GetByIdAsync("fb-2")).ReturnsAsync(feedback);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.UpdateAsync("fb-2", new FeedbackUpdateDTO { Rating = 10 }));
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingFeedback_ReturnsTrueAndDeletes()
        {
            var feedback = new Feedback { FeedbackId = "fb-3" };
            _feedbackRepoMock.Setup(r => r.GetByIdAsync("fb-3")).ReturnsAsync(feedback);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.DeleteAsync("fb-3");

            Assert.True(result);
            _feedbackRepoMock.Verify(r => r.Delete(feedback), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _feedbackRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Feedback?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("ghost");

            Assert.False(result);
        }
    }
}
