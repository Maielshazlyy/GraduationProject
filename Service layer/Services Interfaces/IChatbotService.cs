using Service_layer.DTOS.Chatbot;

namespace Service_layer.Services_Interfaces
{
    public interface IChatbotService
    {
        Task<ChatbotResponseDTO> AskQuestionAsync(string businessId, ChatbotRequestDTO request);
    }
}

