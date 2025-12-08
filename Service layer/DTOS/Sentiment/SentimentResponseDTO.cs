using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_layer.DTOS.Sentiment
{
    public class SentimentResponseDTO
    {
        public int SentimentId { get; set; }
        public string SourceText { get; set; }
        public double Score { get; set; }
        public string Label { get; set; } // Positive / Neutral / Negative
        public DateTime AnalyzedAt { get; set; }
    }
}
