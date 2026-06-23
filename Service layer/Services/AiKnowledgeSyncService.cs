using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Service_layer.DTOS.AiChat;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class AiKnowledgeSyncService : IAiKnowledgeSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IUnitOfWork _unitOfWork;

        public AiKnowledgeSyncService(HttpClient httpClient, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
        }

        public async Task SyncBusinessAsync(string businessId)
        {
            var business = await _unitOfWork.Businesses.GetByIdAsync(businessId);
            if (business == null) return;

            var menuItems = (await _unitOfWork.MenuItems.GetByBusinessIdAsync(businessId))
                .Select(mi => new AiMenuItemDTO
                {
                    MenuItemId  = mi.MenuItemId,
                    Name        = mi.Name,
                    Description = mi.Description,
                    Price       = mi.Price,
                    Category    = mi.MenuCategory?.Name,
                    IsAvailable = mi.IsAvailable
                })
                .ToList();

            var faqs = (await _unitOfWork.KnowledgeBases.GetByBusinessIdAsync(businessId))
                .Where(k => k.IsActive)
                .Select(k => new AiKnowledgeEntryDTO
                {
                    Question = k.Question,
                    Answer   = k.Answer,
                    IsFaq    = k.IsFAQ
                })
                .ToList();

            var payload = new AiKnowledgeSyncDTO
            {
                BusinessId    = businessId,
                BusinessName  = business.Name,
                KnowledgeBase = new AiKnowledgeBaseDTO
                {
                    MenuItems = menuItems,
                    Faqs      = faqs
                }
            };

            var response = await _httpClient.PostAsJsonAsync("/api/v1/business/knowledge-base/sync", payload);
            response.EnsureSuccessStatusCode();
        }
    }
}
