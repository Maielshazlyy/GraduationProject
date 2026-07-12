using digital_employee.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service_layer.DTOS.Voice;
using Service_layer.Services_Interfaces;
using Xunit;

namespace Tests.Controllers
{
    public class CustomerVoiceControllerTests
    {
        private readonly Mock<ICustomerVoiceService> _voiceServiceMock = new();

        private CustomerVoiceController CreateStartController() =>
            new(_voiceServiceMock.Object);

        private VoiceWebhookController CreateWebhookController() =>
            new(_voiceServiceMock.Object);

        // ── StartCall ────────────────────────────────────────────────────────

        [Fact]
        public async Task StartCall_ValidRequest_Returns200WithMeetingUrl()
        {
            var response = new StartVoiceCallResponseDTO
            {
                InteractionId = "inter-1",
                MeetingUrl    = "https://meeting.test.com",
                Status        = "connecting"
            };
            _voiceServiceMock.Setup(s => s.StartCallAsync(It.IsAny<StartVoiceCallRequestDTO>()))
                             .ReturnsAsync(response);

            var result = await CreateStartController().StartCall(new StartVoiceCallRequestDTO
            {
                BusinessId = "biz-1",
                CustomerId = "cust-1"
            }) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var body = result.Value as StartVoiceCallResponseDTO;
            Assert.Equal("https://meeting.test.com", body!.MeetingUrl);
        }

        [Fact]
        public async Task StartCall_MissingBusinessId_Returns400()
        {
            var result = await CreateStartController().StartCall(new StartVoiceCallRequestDTO
            {
                BusinessId = "",
                CustomerId = "cust-1"
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            _voiceServiceMock.Verify(s => s.StartCallAsync(It.IsAny<StartVoiceCallRequestDTO>()), Times.Never);
        }

        [Fact]
        public async Task StartCall_MissingCustomerId_Returns400()
        {
            var result = await CreateStartController().StartCall(new StartVoiceCallRequestDTO
            {
                BusinessId = "biz-1",
                CustomerId = ""
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task StartCall_BusinessNotFound_Returns404()
        {
            _voiceServiceMock.Setup(s => s.StartCallAsync(It.IsAny<StartVoiceCallRequestDTO>()))
                             .ThrowsAsync(new ArgumentException("Business not found."));

            var result = await CreateStartController().StartCall(new StartVoiceCallRequestDTO
            {
                BusinessId = "bad",
                CustomerId = "cust-1"
            }) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task StartCall_MeetingUrlNotConfigured_Returns400()
        {
            _voiceServiceMock.Setup(s => s.StartCallAsync(It.IsAny<StartVoiceCallRequestDTO>()))
                             .ThrowsAsync(new InvalidOperationException("MeetingUrl is not configured."));

            var result = await CreateStartController().StartCall(new StartVoiceCallRequestDTO
            {
                BusinessId = "biz-1",
                CustomerId = "cust-1"
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── CallCompleted (VoiceWebhookController) ───────────────────────────

        [Fact]
        public async Task CallCompleted_ValidPayload_Returns200()
        {
            _voiceServiceMock.Setup(s => s.HandleCallCompletedAsync(It.IsAny<VoiceCallCompletedDTO>()))
                             .Returns(Task.CompletedTask);

            var result = await CreateWebhookController().CallCompleted(new VoiceCallCompletedDTO
            {
                BusinessId = "biz-1",
                CallData   = new VoiceCallDataDTO { CallId = "inter-1" },
                Analysis   = new VoiceAnalysisDTO()
            }) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CallCompleted_MissingCallId_Returns400()
        {
            var result = await CreateWebhookController().CallCompleted(new VoiceCallCompletedDTO
            {
                CallData = new VoiceCallDataDTO { CallId = "" },
                Analysis = new VoiceAnalysisDTO()
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            _voiceServiceMock.Verify(s => s.HandleCallCompletedAsync(It.IsAny<VoiceCallCompletedDTO>()), Times.Never);
        }

        [Fact]
        public async Task CallCompleted_BusinessNotFound_Returns400()
        {
            _voiceServiceMock.Setup(s => s.HandleCallCompletedAsync(It.IsAny<VoiceCallCompletedDTO>()))
                             .ThrowsAsync(new ArgumentException("Business not found."));

            var result = await CreateWebhookController().CallCompleted(new VoiceCallCompletedDTO
            {
                CallData = new VoiceCallDataDTO { CallId = "inter-1" },
                Analysis = new VoiceAnalysisDTO()
            }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
