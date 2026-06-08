using System.Threading.Tasks;
using Domain_layer.Models;
using Service_layer.DTOS.Chat;
using Service_layer.DTOS.Voice;

namespace Service_layer.Services_Interfaces
{
    public interface ICustomerVoiceService
    {
        /// <summary>
        /// Called when the customer clicks "Call".
        /// Reads the fixed MeetingUrl from Settings, creates an Interaction,
        /// notifies the Voice AI to join, and returns the meeting URL to the frontend.
        /// </summary>
        Task<StartVoiceCallResponseDTO> StartCallAsync(StartVoiceCallRequestDTO request);

        /// <summary>
        /// Called by the Voice AI after a call ends.
        /// Stores the full call data + analysis in CallSummary and closes the Interaction.
        /// </summary>
        Task HandleCallCompletedAsync(VoiceCallCompletedDTO payload);


    }
}

