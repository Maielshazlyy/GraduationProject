using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.Ticket;
using Service_layer.Services;
using Xunit;

namespace Tests
{
    public class TicketServiceTests
    {
        private readonly Mock<ITicketRepository> _ticketRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<IRepository<User>> _userRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IInteractionRepository> _interactionRepoMock = new();

        private TicketService CreateService()
        {
            _uowMock.Setup(u => u.Interactions).Returns(_interactionRepoMock.Object);
            return new TicketService(
                _ticketRepoMock.Object,
                _businessRepoMock.Object,
                _customerRepoMock.Object,
                _userRepoMock.Object,
                _uowMock.Object);
        }

        private static Business FakeBusiness(string id) => new() { BusinessId = id, Name = "Biz" };
        private static Customer FakeCustomer(string id) => new() { CustomerId = id };
        private static User FakeUser(string id) => new() { Id = id };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidBusinessAndCustomer_ReturnsTicket()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CreateAsync(new TicketCreateDTO
            {
                BusinessId = "biz-1",
                CustomerId = "cust-1",
                Subject    = "My order is late"
            });

            Assert.NotNull(result);
            Assert.Equal("Open", result.Status);
            Assert.False(result.IsEnded);
            _ticketRepoMock.Verify(r => r.AddAsync(It.IsAny<Ticket>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithInteractionId_LinksInteractionToTicket()
        {
            var interaction = new Interaction { InteractionId = "inter-1" };

            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync("cust-1")).ReturnsAsync(FakeCustomer("cust-1"));
            _interactionRepoMock.Setup(r => r.GetByIdAsync("inter-1")).ReturnsAsync(interaction);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CreateAsync(new TicketCreateDTO
            {
                BusinessId    = "biz-1",
                CustomerId    = "cust-1",
                Subject       = "Help",
                InteractionId = "inter-1"
            });

            Assert.Equal("inter-1", result.InteractionId);
            Assert.Equal(result.Id, interaction.RelatedTicketId);
        }

        [Fact]
        public async Task CreateAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Business?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new TicketCreateDTO { BusinessId = "bad", CustomerId = "cust-1" }));
        }

        [Fact]
        public async Task CreateAsync_CustomerNotFound_ThrowsArgumentException()
        {
            _businessRepoMock.Setup(r => r.GetByIdAsync("biz-1")).ReturnsAsync(FakeBusiness("biz-1"));
            _customerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Customer?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new TicketCreateDTO { BusinessId = "biz-1", CustomerId = "bad" }));
        }

        // ── AssignTicketAsync ────────────────────────────────────────────────

        [Fact]
        public async Task AssignTicketAsync_ValidTicketAndUser_SetsStatusInProgress()
        {
            var ticket = new Ticket { Id = "t-1", Status = "Open" };
            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-1")).ReturnsAsync(ticket);
            _userRepoMock.Setup(r => r.GetByIdAsync("user-1")).ReturnsAsync(FakeUser("user-1"));
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.AssignTicketAsync("t-1", new AssignTicketDTO { UserId = "user-1" });

            Assert.Equal("InProgress", result!.Status);
            Assert.Equal("user-1", result.AssignedToUserId);
        }

        [Fact]
        public async Task AssignTicketAsync_UserNotFound_ThrowsArgumentException()
        {
            var ticket = new Ticket { Id = "t-1" };
            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-1")).ReturnsAsync(ticket);
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

            var service = CreateService();
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.AssignTicketAsync("t-1", new AssignTicketDTO { UserId = "ghost" }));
        }

        // ── CloseTicketAsync ─────────────────────────────────────────────────

        [Fact]
        public async Task CloseTicketAsync_ExistingTicket_SetsClosedAndIsEnded()
        {
            var ticket = new Ticket { Id = "t-2", Status = "InProgress" };
            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-2")).ReturnsAsync(ticket);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.CloseTicketAsync("t-2", new CloseTicketDTO());

            Assert.Equal("Closed", result!.Status);
            Assert.True(result.IsEnded);
        }

        [Fact]
        public async Task CloseTicketAsync_WithInteraction_ClosesInteractionToo()
        {
            var interaction = new Interaction { InteractionId = "inter-2", Status = "Open" };
            var ticket      = new Ticket { Id = "t-3", InteractionId = "inter-2" };

            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-3")).ReturnsAsync(ticket);
            _interactionRepoMock.Setup(r => r.GetByIdAsync("inter-2")).ReturnsAsync(interaction);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            await service.CloseTicketAsync("t-3", new CloseTicketDTO());

            Assert.Equal("Closed", interaction.Status);
            Assert.True(interaction.IsEnded);
        }

        // ── UpdateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_ExistingTicket_UpdatesSubjectAndStatus()
        {
            var ticket = new Ticket { Id = "t-4", Subject = "Old", Status = "Open" };
            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-4")).ReturnsAsync(ticket);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.UpdateAsync("t-4", new TicketUpdateDTO
            {
                Subject = "New subject",
                Status  = "InProgress"
            });

            Assert.Equal("New subject", result!.Subject);
            Assert.Equal("InProgress", result.Status);
        }

        [Fact]
        public async Task UpdateAsync_NullResolutionNotes_DoesNotOverwriteExistingNotes()
        {
            var ticket = new Ticket { Id = "t-5", ResolutionNotes = "Existing notes" };
            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-5")).ReturnsAsync(ticket);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            await service.UpdateAsync("t-5", new TicketUpdateDTO
            {
                Subject          = "Sub",
                Status           = "Open",
                ResolutionNotes  = null
            });

            Assert.Equal("Existing notes", ticket.ResolutionNotes);
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingTicket_ReturnsTrueAndDeletes()
        {
            var ticket = new Ticket { Id = "t-6" };
            _ticketRepoMock.Setup(r => r.GetByIdAsync("t-6")).ReturnsAsync(ticket);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var service = CreateService();
            var result  = await service.DeleteAsync("t-6");

            Assert.True(result);
            _ticketRepoMock.Verify(r => r.Delete(ticket), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _ticketRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Ticket?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("ghost");

            Assert.False(result);
        }

        // ── GetOpenEscalationQueueAsync ──────────────────────────────────────

        [Fact]
        public async Task GetOpenEscalationQueueAsync_ReturnsOnlyUnassignedOpenEscalations()
        {
            var tickets = new List<Ticket>
            {
                new() { Id="1", TicketType="HumanEscalation", IsEnded=false, AssignedToUserId=null },
                new() { Id="2", TicketType="HumanEscalation", IsEnded=true,  AssignedToUserId=null },  // ended
                new() { Id="3", TicketType="HumanEscalation", IsEnded=false, AssignedToUserId="u1" },   // assigned
                new() { Id="4", TicketType="General",         IsEnded=false, AssignedToUserId=null }    // wrong type
            };

            _ticketRepoMock.Setup(r => r.GetByBusinessIdAsync("biz-1")).ReturnsAsync(tickets);

            var service = CreateService();
            var result  = (await service.GetOpenEscalationQueueAsync("biz-1")).ToList();

            Assert.Single(result);
            Assert.Equal("1", result[0].Id);
        }
    }
}
