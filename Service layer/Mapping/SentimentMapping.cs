using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_layer.DTOS.Sentiment;

namespace Service_layer.Mapping
{
    public static class SentimentMapping
    {
        public static SentimentResponseDTO ToDto(this Sentiment s)
        {
            if (s == null) return null;

            return new SentimentResponseDTO
            {
                SentimentId = s.SentimentId,
                SourceText = s.SourceText,
                Label = s.Label,
                Score = s.Score,
                AnalyzedAt = s.AnalyzedAt
            };
        }
    }
}
