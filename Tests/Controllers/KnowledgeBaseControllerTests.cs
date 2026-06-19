using digital_employee.Controllers;
using Domain_layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Service_layer.DTOS.KnowledgeBase;
using Service_layer.Services_Interfaces;
using System.Security.Claims;
using Xunit;

namespace Tests.Controllers
{
    public class KnowledgeBaseControllerTests
    {
        private readonly Mock<IKnowledgeBaseService> _kbServiceMock = new();
        private readonly Mock<IAuditLogService> _auditLogMock = new();

        private KnowledgeBaseController CreateController(string userId = "user-1")
        {
            var controller = new KnowledgeBaseController(_kbServiceMock.Object, _auditLogMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(
                        new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "test"))
                }
            };
            return controller;
        }

        private static KnowledgeBase FakeKb(string id, string bizId = "biz-1") =>
            new() { KnowledgeBaseId = id, BusinessId = bizId, Question = "Q?", Answer = "A." };

        // ── GetById ──────────────────────────────────────────────────────────

        [Fact]
        public async Task GetById_Found_Returns200()
        {
            _kbServiceMock.Setup(s => s.GetByIdAsync("kb-1")).ReturnsAsync(FakeKb("kb-1"));

            var result = await CreateController().GetById("kb-1") as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            _kbServiceMock.Setup(s => s.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((KnowledgeBase?)null);

            var result = await CreateController().GetById("missing") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        // ── Create ───────────────────────────────────────────────────────────

        [Fact]
        public async Task Create_ValidDto_Returns201()
        {
            var kb  = FakeKb("kb-new");
            var dto = new KnowledgeBaseCreateDTO { BusinessId = "biz-1", Question = "Q?", Answer = "A." };

            _kbServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(kb);
            _auditLogMock.Setup(a => a.LogKnowledgeBaseActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .Returns(Task.CompletedTask);

            var result = await CreateController().Create(dto) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
        }

        [Fact]
        public async Task Create_ServiceThrowsArgumentException_Returns400()
        {
            var dto = new KnowledgeBaseCreateDTO { BusinessId = "bad", Question = "Q?", Answer = "A." };
            _kbServiceMock.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new ArgumentException("Business not found."));

            var result = await CreateController().Create(dto) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── BulkCreate ───────────────────────────────────────────────────────

        [Fact]
        public async Task BulkCreate_ValidDto_Returns200WithEntries()
        {
            var entries = new List<KnowledgeBase> { FakeKb("kb-1"), FakeKb("kb-2") };
            var dto     = new KnowledgeBaseBulkCreateDTO
            {
                BusinessId = "biz-1",
                Entries    = [new KnowledgeBulkEntryDTO { Question = "Q1", Answer = "A1" }]
            };

            _kbServiceMock.Setup(s => s.BulkCreateAsync(dto)).ReturnsAsync(entries);
            _auditLogMock.Setup(a => a.LogKnowledgeBaseActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .Returns(Task.CompletedTask);

            var result = await CreateController().BulkCreate(dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task BulkCreate_EmptyEntries_Returns400()
        {
            var dto = new KnowledgeBaseBulkCreateDTO { BusinessId = "biz-1", Entries = [] };
            _kbServiceMock.Setup(s => s.BulkCreateAsync(dto)).ThrowsAsync(new ArgumentException("Entries cannot be empty."));

            var result = await CreateController().BulkCreate(dto) as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        // ── Update ───────────────────────────────────────────────────────────

        [Fact]
        public async Task Update_Found_Returns200()
        {
            var dto = new KnowledgeBaseCreateDTO { BusinessId = "biz-1", Question = "Updated?", Answer = "A." };
            _kbServiceMock.Setup(s => s.UpdateAsync("kb-1", dto)).ReturnsAsync(FakeKb("kb-1"));
            _auditLogMock.Setup(a => a.LogKnowledgeBaseActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .Returns(Task.CompletedTask);

            var result = await CreateController().Update("kb-1", dto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Update_NotFound_Returns404()
        {
            var dto = new KnowledgeBaseCreateDTO { BusinessId = "biz-1", Question = "Q?", Answer = "A." };
            _kbServiceMock.Setup(s => s.UpdateAsync("missing", dto)).ReturnsAsync((KnowledgeBase?)null);

            var result = await CreateController().Update("missing", dto) as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        // ── Delete ───────────────────────────────────────────────────────────

        [Fact]
        public async Task Delete_Found_Returns204()
        {
            _kbServiceMock.Setup(s => s.GetByIdAsync("kb-1")).ReturnsAsync(FakeKb("kb-1"));
            _kbServiceMock.Setup(s => s.DeleteAsync("kb-1")).ReturnsAsync(true);
            _auditLogMock.Setup(a => a.LogKnowledgeBaseActionAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string?>()))
                         .Returns(Task.CompletedTask);

            var result = await CreateController().Delete("kb-1") as NoContentResult;

            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task Delete_NotFound_Returns404()
        {
            _kbServiceMock.Setup(s => s.GetByIdAsync("missing")).ReturnsAsync((KnowledgeBase?)null);

            var result = await CreateController().Delete("missing") as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }
    }
}
