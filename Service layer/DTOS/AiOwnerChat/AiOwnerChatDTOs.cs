using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.AiOwnerChat
{
    public class AiOwnerChatRequestDTO
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class AiOwnerChatResponseDTO
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("reply")]
        public string Reply { get; set; } = string.Empty;

        /// <summary>Analytics sections IRIS used to answer (e.g. "Financial Performance").</summary>
        [JsonPropertyName("data_sources_used")]
        public List<string> DataSourcesUsed { get; set; } = new();

        /// <summary>"high" = data-backed, "medium" = partial, "low" = no data.</summary>
        [JsonPropertyName("confidence")]
        public string Confidence { get; set; } = "high";
    }

    public class AiOwnerReportResponseDTO
    {
        [JsonPropertyName("report_content")]
        public string ReportContent { get; set; } = string.Empty;

        [JsonPropertyName("sections_available")]
        public List<string> SectionsAvailable { get; set; } = new();

        [JsonPropertyName("last_updated")]
        public DateTime? LastUpdated { get; set; }
    }

    public class AiOwnerReloadResponseDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("reports_loaded")]
        public int ReportsLoaded { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
