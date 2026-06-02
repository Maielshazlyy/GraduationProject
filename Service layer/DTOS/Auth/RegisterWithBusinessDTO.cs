namespace Service_layer.DTOS.Auth
{
    /// <summary>
    /// Self-contained owner signup: creates the Business and the Owner account atomically.
    /// No BusinessId is needed — it is generated server-side and returned in the response.
    /// </summary>
    public class RegisterWithBusinessDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        // ── Business details ──────────────────────────────────────────
        /// <summary>Display name of the business (required).</summary>
        public string BusinessName { get; set; } = string.Empty;

        /// <summary>Optional: "Restaurant", "Cafe", "Retail", etc.</summary>
        public string? BusinessType { get; set; }
    }
}
