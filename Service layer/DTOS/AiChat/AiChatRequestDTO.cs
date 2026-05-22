using System.ComponentModel.DataAnnotations;

namespace Service_layer.DTOS.AiChat
{
    public class AiChatRequestDTO
    {
        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public string SessionId { get; set; } = string.Empty;
    }
}
