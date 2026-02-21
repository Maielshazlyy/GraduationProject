using System;
using System.Linq;
using System.Threading.Tasks;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class SentimentService : ISentimentService
    {
        private readonly ISentimentRepository _sentimentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SentimentService(ISentimentRepository sentimentRepository, IUnitOfWork unitOfWork)
        {
            _sentimentRepository = sentimentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Sentiment>> GetAllAsync()
        {
            return await _sentimentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Sentiment>> GetByMessageIdAsync(string messageId)
        {
            return await _sentimentRepository.GetByMessageIdAsync(messageId);
        }

        public async Task<IEnumerable<Sentiment>> GetByBusinessIdAsync(string businessId)
        {
            return await _sentimentRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Sentiment?> GetByIdAsync(string id)
        {
            return await _sentimentRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Analyze sentiment of a message using AI.
        /// 
        /// TODO: Replace this placeholder implementation with actual AI sentiment analysis API.
        /// Recommended AI services:
        /// - Azure Text Analytics (Azure Cognitive Services)
        /// - Google Cloud Natural Language API
        /// - AWS Comprehend
        /// - OpenAI API (with sentiment analysis prompt)
        /// 
        /// Current implementation: Simple keyword-based sentiment analysis (temporary placeholder).
        /// This will be replaced with AI integration in the future.
        /// </summary>
        public async Task<Sentiment> AnalyzeSentimentAsync(string messageId, string messageText, string language = "ar")
        {
            // ============================================
            // PLACEHOLDER IMPLEMENTATION - TO BE REPLACED
            // ============================================
            // TODO: Implement AI-based sentiment analysis
            // This is a temporary keyword-based implementation.
            // Replace with actual AI API call when ready.
            // ============================================
            var lowerText = messageText.ToLowerInvariant();
            var sentiment = new Sentiment
            {
                SentimentId = Guid.NewGuid().ToString(),
                MessageId = messageId,
                SourceText = messageText,
                AnalyzedAt = DateTime.UtcNow
            };

            // PLACEHOLDER: Simple keyword-based sentiment detection
            // This will be replaced with AI-based analysis
            var positiveKeywords = language == "ar" 
                ? new[] { "ممتاز", "رائع", "شكرا", "حلو", "جميل", "مشكور", "تمام", "زوق", "عظيم" }
                : new[] { "excellent", "great", "thanks", "good", "nice", "perfect", "amazing", "wonderful", "love", "happy" };
            
            var negativeKeywords = language == "ar"
                ? new[] { "سيء", "مش عاجبني", "مشكلة", "غلط", "مش راضي", "زعلان", "غضبان", "مش عاجب", "مش حلو" }
                : new[] { "bad", "terrible", "awful", "hate", "angry", "disappointed", "problem", "wrong", "poor", "worst" };

            var positiveCount = positiveKeywords.Count(k => lowerText.Contains(k));
            var negativeCount = negativeKeywords.Count(k => lowerText.Contains(k));

            if (positiveCount > negativeCount)
            {
                sentiment.Label = "Positive";
                sentiment.Score = Math.Min(0.8 + (positiveCount * 0.1), 1.0);
            }
            else if (negativeCount > positiveCount)
            {
                sentiment.Label = "Negative";
                sentiment.Score = Math.Max(-1.0, -0.8 - (negativeCount * 0.1));
            }
            else
            {
                sentiment.Label = "Neutral";
                sentiment.Score = 0.0;
            }

            // Save sentiment to database
            await _sentimentRepository.AddAsync(sentiment);
            await _unitOfWork.CompleteAsync();

            return sentiment;
        }
    }
}

