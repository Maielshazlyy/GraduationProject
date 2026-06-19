using Domain_layer.Interfaces;
using Domain_layer.Models;
using Moq;
using Service_layer.DTOS.KnowledgeBase;
using Service_layer.Services;
using Service_layer.Services_Interfaces;
using Xunit;

namespace Tests
{
    public class KnowledgeBaseServiceTests
    {
        private readonly Mock<IKnowledgeBaseRepository> _kbRepoMock = new();
        private readonly Mock<IBusinessRepository> _businessRepoMock = new();
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IAiKnowledgeSyncService> _aiSyncMock = new();

        private KnowledgeBaseService CreateService() =>
            new(_kbRepoMock.Object, _businessRepoMock.Object, _uowMock.Object, _aiSyncMock.Object);

        private static Business FakeBusiness(string id) =>
            new() { BusinessId = id, Name = "Test Business" };

        // ── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ValidBusiness_ReturnsKnowledgeBase()
        {
            var businessId = "biz-1";
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new KnowledgeBaseCreateDTO
            {
                BusinessId = businessId,
                Question   = "What are your hours?",
                Answer     = "9am to 5pm",
                IsFAQ      = true
            };

            var service = CreateService();
            var result  = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Question, result.Question);
            Assert.Equal(dto.Answer, result.Answer);
            Assert.Equal(businessId, result.BusinessId);
            _kbRepoMock.Verify(r => r.AddAsync(It.IsAny<KnowledgeBase>()), Times.Once);
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Business?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(new KnowledgeBaseCreateDTO { BusinessId = "invalid" }));
        }

        // ── BulkCreateAsync ──────────────────────────────────────────────────

        [Fact]
        public async Task BulkCreateAsync_ValidEntries_ReturnsAllCreatedItems()
        {
            var businessId = "biz-2";
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(businessId)).Returns(Task.CompletedTask);

            var dto = new KnowledgeBaseBulkCreateDTO
            {
                BusinessId = businessId,
                Entries =
                [
                    new KnowledgeBulkEntryDTO { Question = "Q1", Answer = "A1" },
                    new KnowledgeBulkEntryDTO { Question = "Q2", Answer = "A2", IsFAQ = true }
                ]
            };

            var service = CreateService();
            var results = (await service.BulkCreateAsync(dto)).ToList();

            Assert.Equal(2, results.Count);
            Assert.All(results, r => Assert.Equal(businessId, r.BusinessId));
            _kbRepoMock.Verify(r => r.AddAsync(It.IsAny<KnowledgeBase>()), Times.Exactly(2));
            _uowMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task BulkCreateAsync_EmptyEntries_ThrowsArgumentException()
        {
            var businessId = "biz-3";
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            var service = CreateService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.BulkCreateAsync(new KnowledgeBaseBulkCreateDTO
                {
                    BusinessId = businessId,
                    Entries    = []
                }));
        }

        [Fact]
        public async Task BulkCreateAsync_BusinessNotFound_ThrowsArgumentException()
        {
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Business?)null);

            var service = CreateService();

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.BulkCreateAsync(new KnowledgeBaseBulkCreateDTO
                {
                    BusinessId = "bad-id",
                    Entries    = [new KnowledgeBulkEntryDTO { Question = "Q", Answer = "A" }]
                }));
        }

        // ── DeleteAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task DeleteAsync_ExistingEntry_ReturnsTrueAndDeletesItem()
        {
            var kb = new KnowledgeBase
            {
                KnowledgeBaseId = "kb-1",
                BusinessId      = "biz-4",
                Question        = "Q",
                Answer          = "A"
            };

            _kbRepoMock.Setup(r => r.GetByIdAsync("kb-1")).ReturnsAsync(kb);
            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock.Setup(a => a.SyncBusinessAsync(kb.BusinessId)).Returns(Task.CompletedTask);

            var service = CreateService();
            var result  = await service.DeleteAsync("kb-1");

            Assert.True(result);
            _kbRepoMock.Verify(r => r.Delete(kb), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _kbRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((KnowledgeBase?)null);

            var service = CreateService();
            var result  = await service.DeleteAsync("missing-id");

            Assert.False(result);
            _kbRepoMock.Verify(r => r.Delete(It.IsAny<KnowledgeBase>()), Times.Never);
        }

        // ── GetByBusinessIdAsync ─────────────────────────────────────────────

        [Fact]
        public async Task GetByBusinessIdAsync_ReturnsAllEntriesForBusiness()
        {
            var businessId = "biz-5";
            var fakeData   = new List<KnowledgeBase>
            {
                new() { KnowledgeBaseId = "1", BusinessId = businessId, Question = "Q1", Answer = "A1" },
                new() { KnowledgeBaseId = "2", BusinessId = businessId, Question = "Q2", Answer = "A2" }
            };

            _kbRepoMock
                .Setup(r => r.GetByBusinessIdAsync(businessId))
                .ReturnsAsync(fakeData);

            var service = CreateService();
            var results = (await service.GetByBusinessIdAsync(businessId)).ToList();

            Assert.Equal(2, results.Count);
        }

        // ── AI sync failure is non-fatal ─────────────────────────────────────

        [Fact]
        public async Task CreateAsync_AiSyncFails_DoesNotThrow()
        {
            var businessId = "biz-6";
            _businessRepoMock
                .Setup(r => r.GetByIdAsync(businessId))
                .ReturnsAsync(FakeBusiness(businessId));

            _uowMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);
            _aiSyncMock
                .Setup(a => a.SyncBusinessAsync(businessId))
                .ThrowsAsync(new HttpRequestException("AI unreachable"));

            var service = CreateService();

            var exception = await Record.ExceptionAsync(() =>
                service.CreateAsync(new KnowledgeBaseCreateDTO
                {
                    BusinessId = businessId,
                    Question   = "Q",
                    Answer     = "A"
                }));

            Assert.Null(exception);
        }
    }
}
