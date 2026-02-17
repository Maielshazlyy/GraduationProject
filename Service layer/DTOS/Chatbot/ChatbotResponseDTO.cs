namespace Service_layer.DTOS.Chatbot
{
    public class ChatbotResponseDTO
    {
        public string Answer { get; set; } = string.Empty;
        public string? ConversationId { get; set; }
        public List<string>? Suggestions { get; set; } // Optional suggestions for follow-up questions
    }
}

