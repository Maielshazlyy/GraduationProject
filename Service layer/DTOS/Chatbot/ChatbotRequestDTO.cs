namespace Service_layer.DTOS.Chatbot
{
    public class ChatbotRequestDTO
    {
        public string Question { get; set; } = string.Empty;
        public string? ConversationId { get; set; } // Optional: for conversation history
    }
}

