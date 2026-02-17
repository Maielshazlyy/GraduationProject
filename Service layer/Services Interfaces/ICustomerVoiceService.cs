using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Chat;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Service for handling customer voice call interactions (Voice channel).
    /// Handles audio messages, speech-to-text, text-to-speech, call sessions,
    /// delivery delays, recovery, and feedback collection.
    /// </summary>
    public interface ICustomerVoiceService
    {
        /// <summary>
        /// Handle a voice message from customer (Voice channel).
        /// Converts audio to text, processes intent, executes business logic, and returns audio response.
        /// </summary>
        Task<CustomerChatResponseDTO> HandleVoiceMessageAsync(CustomerChatRequestDTO request);

        /// <summary>
        /// Initialize a new voice call session.
        /// Creates Interaction with CallSessionId.
        /// </summary>
        Task<Interaction> InitializeVoiceSessionAsync(string businessId, string? customerId, string callSessionId);

        /// <summary>
        /// Mark an interaction as interrupted (call dropped).
        /// Used for recovery and follow-up.
        /// </summary>
        Task MarkInteractionInterruptedAsync(string interactionId);

        /// <summary>
        /// Submit feedback at end of voice call.
        /// </summary>
        Task<Feedback> SubmitFeedbackAsync(VoiceFeedbackDTO feedbackDto);

        /// <summary>
        /// Get voice settings for a business (for TTS configuration).
        /// </summary>
        Task<VoiceSettingsDTO?> GetVoiceSettingsAsync(string businessId);
    }
}

