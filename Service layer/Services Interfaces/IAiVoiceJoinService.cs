namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Notifies the Voice AI to join an active call.
    /// Called by the backend immediately after creating an Interaction for a new voice call.
    /// </summary>
    public interface IAiVoiceJoinService
    {
        /// <summary>
        /// POSTs a join request to the Voice AI so it can connect to the meeting.
        /// Non-fatal — failure is logged but does not abort the call flow.
        /// </summary>
        Task JoinCallAsync(string interactionId, string businessId, string meetingUrl);
    }
}
