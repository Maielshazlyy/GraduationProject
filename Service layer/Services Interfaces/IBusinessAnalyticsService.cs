using Service_layer.DTOS.Chatbot;

namespace Service_layer.Services_Interfaces
{
    public interface IBusinessAnalyticsService
    {
        Task<BusinessAnalyticsDTO> GetBusinessAnalyticsAsync(string businessId);
    }
}

