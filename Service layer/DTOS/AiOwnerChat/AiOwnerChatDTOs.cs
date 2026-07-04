using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Service_layer.DTOS.BusinessReport;

namespace Service_layer.DTOS.AiOwnerChat
{
    public class AiOwnerChatRequestDTO
    {
        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

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

    /// <summary>
    /// Pushed to the AI service whenever a business report is generated/updated, so Owner Chat
    /// has the report content to ground its answers in. This is a separate store from the
    /// customer-chat knowledge base (menu/FAQ) -- owner chat must answer from analytics data only.
    /// </summary>
    public class AiOwnerReportSyncRequestDTO
    {
        [JsonPropertyName("business_id")]
        public string BusinessId { get; set; } = string.Empty;

        [JsonPropertyName("business_name")]
        public string BusinessName { get; set; } = string.Empty;

        [JsonPropertyName("period")]
        public ReportPeriodDTO Period { get; set; } = new();

        [JsonPropertyName("report")]
        public AiReportResponseDTO Report { get; set; } = new();
    }

    public class AiOwnerReportSyncResponseDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}
