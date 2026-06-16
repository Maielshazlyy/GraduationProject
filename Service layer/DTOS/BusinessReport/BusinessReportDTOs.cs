using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Service_layer.DTOS.BusinessReport
{
    // ── Frontend → Backend ────────────────────────────────────────────────────

    public class ReportGenerateRequestDTO
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Language { get; set; } = "ar";
        public bool Regenerate { get; set; } = false;
    }

    // ── Backend → AI (aggregated payload) ────────────────────────────────────

    public class AiReportRequestDTO
    {
        [JsonPropertyName("businessId")]     public string BusinessId { get; set; } = string.Empty;
        [JsonPropertyName("businessName")]   public string BusinessName { get; set; } = string.Empty;
        [JsonPropertyName("period")]         public ReportPeriodDTO Period { get; set; } = new();
        [JsonPropertyName("metrics")]        public ReportMetricsDTO Metrics { get; set; } = new();
        [JsonPropertyName("topIntents")]     public List<ReportIntentCountDTO> TopIntents { get; set; } = new();
        [JsonPropertyName("topTopics")]      public List<ReportTopicCountDTO> TopTopics { get; set; } = new();
        [JsonPropertyName("commonIssues")]   public List<ReportCommonIssueDTO> CommonIssues { get; set; } = new();
        [JsonPropertyName("recentKeyMoments")] public List<string> RecentKeyMoments { get; set; } = new();
        [JsonPropertyName("sampleSummaries")] public List<ReportSampleSummaryDTO> SampleSummaries { get; set; } = new();
    }

    public class ReportPeriodDTO
    {
        [JsonPropertyName("from")] public DateTime From { get; set; }
        [JsonPropertyName("to")]   public DateTime To { get; set; }
    }

    public class ReportMetricsDTO
    {
        [JsonPropertyName("totalSessions")]          public int TotalSessions { get; set; }
        [JsonPropertyName("analyzedSessions")]       public int AnalyzedSessions { get; set; }
        [JsonPropertyName("averageSentimentScore")]  public double AverageSentimentScore { get; set; }
        [JsonPropertyName("sentimentDistribution")]  public SentimentDistributionDTO SentimentDistribution { get; set; } = new();
        [JsonPropertyName("totalComplaints")]        public int TotalComplaints { get; set; }
        [JsonPropertyName("totalHumanAgentRequests")] public int TotalHumanAgentRequests { get; set; }
        [JsonPropertyName("totalOrdersDetected")]    public int TotalOrdersDetected { get; set; }
    }

    public class SentimentDistributionDTO
    {
        [JsonPropertyName("positive")] public int Positive { get; set; }
        [JsonPropertyName("neutral")]  public int Neutral { get; set; }
        [JsonPropertyName("negative")] public int Negative { get; set; }
    }

    public class ReportIntentCountDTO
    {
        [JsonPropertyName("name")]  public string Name { get; set; } = string.Empty;
        [JsonPropertyName("count")] public int Count { get; set; }
    }

    public class ReportTopicCountDTO
    {
        [JsonPropertyName("name")]  public string Name { get; set; } = string.Empty;
        [JsonPropertyName("count")] public int Count { get; set; }
    }

    public class ReportCommonIssueDTO
    {
        [JsonPropertyName("issue")]    public string Issue { get; set; } = string.Empty;
        [JsonPropertyName("count")]    public int Count { get; set; }
        [JsonPropertyName("examples")] public List<string> Examples { get; set; } = new();
    }

    public class ReportSampleSummaryDTO
    {
        [JsonPropertyName("sessionId")]      public string SessionId { get; set; } = string.Empty;
        [JsonPropertyName("summary")]        public string Summary { get; set; } = string.Empty;
        [JsonPropertyName("summaryAr")]      public string SummaryAr { get; set; } = string.Empty;
        [JsonPropertyName("mainIntent")]     public string MainIntent { get; set; } = string.Empty;
        [JsonPropertyName("sentimentLabel")] public string SentimentLabel { get; set; } = string.Empty;
        [JsonPropertyName("sentimentScore")] public double SentimentScore { get; set; }
    }

    // ── AI → Backend (report response) ───────────────────────────────────────

    public class AiReportResponseDTO
    {
        [JsonPropertyName("businessId")]      public string BusinessId { get; set; } = string.Empty;
        [JsonPropertyName("period")]          public ReportPeriodDTO Period { get; set; } = new();
        [JsonPropertyName("reportTitle")]     public string ReportTitle { get; set; } = string.Empty;
        [JsonPropertyName("summary")]         public string Summary { get; set; } = string.Empty;
        [JsonPropertyName("summaryAr")]       public string SummaryAr { get; set; } = string.Empty;
        [JsonPropertyName("highlights")]      public List<string> Highlights { get; set; } = new();
        [JsonPropertyName("highlightsAr")]    public List<string> HighlightsAr { get; set; } = new();
        [JsonPropertyName("problems")]        public List<ReportProblemDTO> Problems { get; set; } = new();
        [JsonPropertyName("recommendations")] public List<ReportRecommendationDTO> Recommendations { get; set; } = new();
        [JsonPropertyName("suggestedActions")] public List<string> SuggestedActions { get; set; } = new();
        [JsonPropertyName("riskLevel")]       public string RiskLevel { get; set; } = "medium";
    }

    public class ReportProblemDTO
    {
        [JsonPropertyName("title")]       public string Title { get; set; } = string.Empty;
        [JsonPropertyName("description")] public string Description { get; set; } = string.Empty;
        [JsonPropertyName("severity")]    public string Severity { get; set; } = string.Empty;
        [JsonPropertyName("evidence")]    public List<string> Evidence { get; set; } = new();
    }

    public class ReportRecommendationDTO
    {
        [JsonPropertyName("title")]          public string Title { get; set; } = string.Empty;
        [JsonPropertyName("description")]    public string Description { get; set; } = string.Empty;
        [JsonPropertyName("priority")]       public string Priority { get; set; } = string.Empty;
        [JsonPropertyName("expectedImpact")] public string ExpectedImpact { get; set; } = string.Empty;
        [JsonPropertyName("suggestedOwner")] public string SuggestedOwner { get; set; } = string.Empty;
    }
}
