using System.Threading.Tasks;
using Service_layer.DTOS.Chat;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Service for handling customer text chat interactions (WebChat channel).
    /// Handles text messages, intent detection, orders, tickets, and recommendations.
    /// </summary>
    public interface ICustomerChatService
    {
        /// <summary>
        /// Handle a text message from customer (Chat channel).
        /// </summary>
        Task<CustomerChatResponseDTO> HandleMessageAsync(CustomerChatRequestDTO request);

        /// <summary>
        /// Get available communication capabilities for a business.
        /// </summary>
        Task<CustomerCapabilitiesDTO> GetCapabilitiesAsync(string businessId);

        /// <summary>
        /// Get backend-driven recommendations for a main menu item
        /// (e.g., suggest fries and drink with burger) without using chat.
        /// Used by the public ordering page.
        /// </summary>
        Task<System.Collections.Generic.List<RecommendationItemDTO>> GetOrderRecommendationsAsync(
            CustomerOrderRecommendationRequestDTO request);
    }
}
