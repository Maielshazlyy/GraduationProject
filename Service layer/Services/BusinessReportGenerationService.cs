using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Service_layer.DTOS.AiAnalysis;
using Service_layer.DTOS.BusinessReport;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class BusinessReportGenerationService : IBusinessReportGenerationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessRepository _businessRepository;
        private readonly IAiReportService _aiReportService;

        public BusinessReportGenerationService(
            IUnitOfWork unitOfWork,
            IBusinessRepository businessRepository,
            IAiReportService aiReportService)
        {
            _unitOfWork = unitOfWork;
            _businessRepository = businessRepository;
            _aiReportService = aiReportService;
        }

        public async Task<AiReportResponseDTO> GenerateReportAsync(string businessId, ReportGenerateRequestDTO request)
        {
            // 1 — Validate business
            var business = await _businessRepository.GetByIdAsync(businessId);
            if (business == null)
                throw new KeyNotFoundException("Business not found.");

            // 2 — Load analyses in date range
            var analyses = await _unitOfWork.InteractionAnalyses
                .GetByBusinessIdAndDateRangeAsync(businessId, request.From, request.To);
            var list = analyses.ToList();

            if (list.Count == 0)
                throw new InvalidOperationException("NO_DATA");

            // 3 — Aggregate intents, topics, key moments
            var allIntents  = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var allTopics   = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var allMoments  = new List<string>();

            foreach (var a in list)
            {
                if (!string.IsNullOrEmpty(a.IntentsDetectedJson))
                {
                    var intents = JsonSerializer.Deserialize<List<AiIntentResultDTO>>(a.IntentsDetectedJson) ?? new();
                    foreach (var intent in intents)
                    {
                        allIntents.TryGetValue(intent.Name, out var c);
                        allIntents[intent.Name] = c + intent.Count;
                    }
                }

                if (!string.IsNullOrEmpty(a.MainTopicsJson))
                {
                    var topics = JsonSerializer.Deserialize<List<string>>(a.MainTopicsJson) ?? new();
                    foreach (var t in topics)
                    {
                        allTopics.TryGetValue(t, out var c);
                        allTopics[t] = c + 1;
                    }
                }

                if (!string.IsNullOrEmpty(a.KeyMomentsJson))
                {
                    var moments = JsonSerializer.Deserialize<List<string>>(a.KeyMomentsJson) ?? new();
                    allMoments.AddRange(moments);
                }
            }

            var topIntents = allIntents
                .OrderByDescending(kv => kv.Value)
                .Take(10)
                .Select(kv => new ReportIntentCountDTO { Name = kv.Key, Count = kv.Value })
                .ToList();

            var topTopics = allTopics
                .OrderByDescending(kv => kv.Value)
                .Take(10)
                .Select(kv => new ReportTopicCountDTO { Name = kv.Key, Count = kv.Value })
                .ToList();

            // 4 — Cluster common issues from complaint/negative sessions
            var issueTopics = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in list.Where(a =>
                a.MainIntent.Equals("Complaint", StringComparison.OrdinalIgnoreCase) ||
                a.SentimentLabel.Equals("Negative", StringComparison.OrdinalIgnoreCase)))
            {
                if (string.IsNullOrEmpty(a.MainTopicsJson)) continue;
                var topics = JsonSerializer.Deserialize<List<string>>(a.MainTopicsJson) ?? new();
                foreach (var t in topics)
                {
                    if (!issueTopics.ContainsKey(t)) issueTopics[t] = new();
                    if (!string.IsNullOrEmpty(a.Summary))
                        issueTopics[t].Add(a.Summary);
                }
            }

            var commonIssues = issueTopics
                .OrderByDescending(kv => kv.Value.Count)
                .Take(10)
                .Select(kv => new ReportCommonIssueDTO
                {
                    Issue    = kv.Key,
                    Count    = kv.Value.Count,
                    Examples = kv.Value.Take(3).ToList()
                })
                .ToList();

            // 5 — Sample summaries (10 most recent)
            var sampleSummaries = list
                .OrderByDescending(a => a.AnalyzedAt)
                .Take(10)
                .Select(a => new ReportSampleSummaryDTO
                {
                    SessionId      = a.InteractionId,
                    Summary        = a.Summary ?? string.Empty,
                    SummaryAr      = a.SummaryAr ?? string.Empty,
                    MainIntent     = a.MainIntent,
                    SentimentLabel = a.SentimentLabel,
                    SentimentScore = a.SentimentScore
                })
                .ToList();

            // 6 — Sentiment distribution
            var sentDist = list.GroupBy(a => a.SentimentLabel)
                               .ToDictionary(g => g.Key, g => g.Count());

            // 7 — Build AI payload
            var payload = new AiReportRequestDTO
            {
                BusinessId   = businessId,
                BusinessName = business.Name,
                Period       = new ReportPeriodDTO { From = request.From, To = request.To },
                Metrics      = new ReportMetricsDTO
                {
                    TotalSessions         = list.Count,
                    AnalyzedSessions      = list.Count,
                    AverageSentimentScore = Math.Round(list.Average(a => a.SentimentScore), 3),
                    SentimentDistribution = new SentimentDistributionDTO
                    {
                        Positive = sentDist.GetValueOrDefault("Positive", 0),
                        Neutral  = sentDist.GetValueOrDefault("Neutral",  0),
                        Negative = sentDist.GetValueOrDefault("Negative", 0)
                    },
                    TotalComplaints          = list.Count(a => a.MainIntent.Equals("Complaint", StringComparison.OrdinalIgnoreCase)),
                    TotalHumanAgentRequests  = allIntents.GetValueOrDefault("RequestHumanAgent", 0),
                    TotalOrdersDetected      = allIntents.GetValueOrDefault("CreateOrder", 0)
                },
                TopIntents       = topIntents,
                TopTopics        = topTopics,
                CommonIssues     = commonIssues,
                RecentKeyMoments = allMoments.TakeLast(10).ToList(),
                SampleSummaries  = sampleSummaries
            };

            // 8 — Call AI and return
            return await _aiReportService.GenerateReportAsync(payload);
        }
    }
}
