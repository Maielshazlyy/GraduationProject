using digital_employee.Controllers;
using Domain_layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service_layer.DTOS.Ticket;
using Service_layer.Services_Interfaces;
using System.Security.Claims;
using Xunit;

namespace Tests.Controllers
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketService> _ticketServiceMock = new();
        private readonly Mock<IAuditLogService> _auditLogMock = new();

        private TicketController CreateController(string userId = "user-1", string? businessId = null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId)
            };
            if (businessId != null)
                claims.Add(new Claim("BusinessId", businessId));

            var controller = new TicketController(_ticketServiceMock.Object, _auditLogMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"))
                }
            };
            return controller;
        }

        private static Ticket FakeTicket(string id) =>
            new() { Id = id, BusinessId = "biz-1", CustomerId = "cust-1", Status = "Open" };

        // ── GetById ──────────────────────────────────────────────────────────

        [Fact]
        public async Task GetById_Found_Returns200()
        {
            _ticketServiceMock.Setup(s => s.GetByIdAsync("t-1")).ReturnsAsync(FakeTicket("t-1"));

            var result = await CreateController().GetById("t-1") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            _ticketServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Ticket?)null);

            var result = await CreateController().GetById("ghost") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        // ── Create ───────────────────────────────────────────────────────────

        [Fact]
        public async Task Create_ValidDto_Returns201()
        {
            var ticket = FakeTicket("t-new");
            var dto    = new TicketCreateDTO { BusinessId = "biz-1", CustomerId = "cust-1", Subject = "Help" };

            _ticketServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(ticket);

            var result = await CreateController().Create(dto) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task Create_BusinessNotFound_Returns400()
        {
            var dto = new TicketCreateDTO { BusinessId = "bad", CustomerId = "cust-1" };
            _ticketServiceMock.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new ArgumentException("Business not found."));

            var result = await CreateController().Create(dto) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── Update ───────────────────────────────────────────────────────────

        [Fact]
        public async Task Update_Found_Returns200()
        {
            var dto = new TicketUpdateDTO { Subject = "Updated", Status = "InProgress" };
            _ticketServiceMock.Setup(s => s.UpdateAsync("t-1", dto)).ReturnsAsync(FakeTicket("t-1"));

            var result = await CreateController().Update("t-1", dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Update_NotFound_Returns404()
        {
            var dto = new TicketUpdateDTO { Subject = "X", Status = "Open" };
            _ticketServiceMock.Setup(s => s.UpdateAsync("ghost", dto)).ReturnsAsync((Ticket?)null);

            var result = await CreateController().Update("ghost", dto) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        // ── AssignTicket ─────────────────────────────────────────────────────

        [Fact]
        public async Task AssignTicket_ValidAssignment_Returns200()
        {
            var ticket = FakeTicket("t-1");
            ticket.AssignedToUserId = "agent-1";
            ticket.Status = "InProgress";

            _ticketServiceMock.Setup(s => s.AssignTicketAsync("t-1", It.IsAny<AssignTicketDTO>())).ReturnsAsync(ticket);
            _auditLogMock.Setup(a => a.LogTicketActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .Returns(Task.CompletedTask);

            var result = await CreateController().AssignTicket("t-1", new AssignTicketDTO { UserId = "agent-1" }) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task AssignTicket_TicketNotFound_Returns404()
        {
            _ticketServiceMock.Setup(s => s.AssignTicketAsync("ghost", It.IsAny<AssignTicketDTO>())).ReturnsAsync((Ticket?)null);

            var result = await CreateController().AssignTicket("ghost", new AssignTicketDTO { UserId = "u-1" }) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task AssignTicket_UserNotFound_Returns400()
        {
            _ticketServiceMock.Setup(s => s.AssignTicketAsync("t-1", It.IsAny<AssignTicketDTO>()))
                              .ThrowsAsync(new ArgumentException("User not found."));

            var result = await CreateController().AssignTicket("t-1", new AssignTicketDTO { UserId = "bad" }) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── CloseTicket ──────────────────────────────────────────────────────

        [Fact]
        public async Task CloseTicket_Found_Returns200()
        {
            var ticket = FakeTicket("t-1");
            ticket.Status  = "Closed";
            ticket.IsEnded = true;

            _ticketServiceMock.Setup(s => s.CloseTicketAsync("t-1", It.IsAny<CloseTicketDTO>())).ReturnsAsync(ticket);
            _auditLogMock.Setup(a => a.LogTicketActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .Returns(Task.CompletedTask);

            var result = await CreateController().CloseTicket("t-1", new CloseTicketDTO()) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task CloseTicket_NotFound_Returns404()
        {
            _ticketServiceMock.Setup(s => s.CloseTicketAsync("ghost", It.IsAny<CloseTicketDTO>())).ReturnsAsync((Ticket?)null);

            var result = await CreateController().CloseTicket("ghost", new CloseTicketDTO()) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        // ── Delete ───────────────────────────────────────────────────────────

        [Fact]
        public async Task Delete_Found_Returns204()
        {
            _ticketServiceMock.Setup(s => s.DeleteAsync("t-1")).ReturnsAsync(true);

            var result = await CreateController().Delete("t-1") as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task Delete_NotFound_Returns404()
        {
            _ticketServiceMock.Setup(s => s.DeleteAsync("ghost")).ReturnsAsync(false);

            var result = await CreateController().Delete("ghost") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        // ── GetEscalationQueue ───────────────────────────────────────────────

        [Fact]
        public async Task GetEscalationQueue_NoBusinessIdInToken_Returns400()
        {
            // Controller created without BusinessId claim
            var result = await CreateController(businessId: null).GetEscalationQueue() as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task GetEscalationQueue_WithBusinessId_Returns200()
        {
            _ticketServiceMock.Setup(s => s.GetOpenEscalationQueueAsync("biz-1")).ReturnsAsync(new List<Ticket>());

            var result = await CreateController(businessId: "biz-1").GetEscalationQueue() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
