using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service_layer.DTOS.Chat;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    /// <summary>
    /// Intent Detection Service - PLACEHOLDER IMPLEMENTATION
    /// 
    /// TODO: Replace this placeholder implementation with actual AI-based intent detection.
    /// Recommended AI services:
    /// - Azure Language Understanding (LUIS) / Azure Language Service
    /// - Google Dialogflow
    /// - AWS Lex
    /// - OpenAI API (with intent detection prompt)
    /// - Custom trained NLP model
    /// 
    /// Current implementation: Simple keyword-based intent detection (temporary placeholder).
    /// Supports both Arabic and English languages.
    /// This will be replaced with AI integration in the future.
    /// </summary>
    public class IntentDetectionService : IIntentDetectionService
    {
        public Task<DetectedIntentResultDTO> DetectIntentAsync(
            string businessId,
            string interactionId,
            IEnumerable<string> recentMessages)
        {
            // ============================================
            // PLACEHOLDER IMPLEMENTATION - TO BE REPLACED
            // ============================================
            // TODO: Implement AI-based intent detection
            // This is a temporary keyword-based implementation.
            // Replace with actual AI API call when ready.
            // ============================================
            var last = recentMessages.LastOrDefault() ?? string.Empty;
            var lower = last.ToLowerInvariant();

            // Detect language (Arabic or English)
            var detectedLanguage = DetectLanguage(last);
            var detectedDialect = DetectDialect(last, detectedLanguage);

            var result = new DetectedIntentResultDTO
            {
                Confidence = 0.5,
                RequiresAction = false,
                ComplexityLevel = "Low",
                RequiresEscalation = false,
                PriorityLevel = "Normal",
                DetectedLanguage = detectedLanguage,
                DetectedDialect = detectedDialect
            };

            // Intent detection - supports both Arabic and English
            if (IsCreateOrderIntent(lower, detectedLanguage))
            {
                result.Intent = "CreateOrder";
                result.RequiresAction = true;
                result.Confidence = 0.8;
            }
            else if (IsOrderStatusIntent(lower, detectedLanguage))
            {
                result.Intent = "AskAboutOrderStatus";
                result.Confidence = 0.7;
            }
            else if (IsComplaintIntent(lower, detectedLanguage))
            {
                result.Intent = "Complaint";
                result.RequiresAction = true;
                result.ComplexityLevel = "Medium";
                result.Confidence = 0.75;
            }
            else if (IsRequestHumanAgentIntent(lower, detectedLanguage))
            {
                result.Intent = "RequestHumanAgent";
                result.RequiresAction = true;
                result.ComplexityLevel = "High";
                result.RequiresEscalation = true;
                result.PriorityLevel = "High";
                result.Confidence = 0.9;
                result.EscalationReason = detectedLanguage == "ar" 
                    ? "العميل طلب صراحة التحدث مع موظف بشري."
                    : "Customer explicitly requested a human agent.";
            }
            else if (IsAskProductsIntent(lower, detectedLanguage))
            {
                result.Intent = "AskAboutProducts";
                result.Confidence = 0.7;
            }
            else
            {
                result.Intent = "GeneralQuestion";
                result.Confidence = 0.5;
            }

            return Task.FromResult(result);
        }

        private string DetectLanguage(string text)
        {
            // PLACEHOLDER: Simple language detection based on character analysis
            // TODO: Replace with proper language detection library or AI service
            // Recommended: Google Cloud Translation API, Azure Translator, or ML.NET language detection
            var arabicCharCount = text.Count(c => c >= 0x0600 && c <= 0x06FF);
            var englishCharCount = text.Count(c => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'));
            
            return arabicCharCount > englishCharCount ? "ar" : "en";
        }

        private string DetectDialect(string text, string language)
        {
            // PLACEHOLDER: Simple dialect detection
            // TODO: Replace with AI-based dialect detection when available
            if (language == "ar")
            {
                // PLACEHOLDER: Detect Egyptian dialect vs Standard Arabic using keywords
                var egyptianKeywords = new[] { "عايز", "عندك", "عندي", "عشان", "ازاي", "ايه", "مش", "هو", "هي" };
                var lower = text.ToLowerInvariant();
                var egyptianCount = egyptianKeywords.Count(k => lower.Contains(k));
                
                return egyptianCount > 0 ? "Egyptian" : "Standard Arabic";
            }
            
            return "English";
        }

        private bool IsCreateOrderIntent(string lowerText, string language)
        {
            if (language == "ar")
            {
                return lowerText.Contains("اطلب") || 
                       (lowerText.Contains("عايز") && (lowerText.Contains("برجر") || lowerText.Contains("بيتزا") || lowerText.Contains("ساندويتش"))) ||
                       lowerText.Contains("طلب") || lowerText.Contains("اريد");
            }
            else
            {
                return lowerText.Contains("order") || 
                       lowerText.Contains("want") || 
                       lowerText.Contains("buy") ||
                       lowerText.Contains("get") ||
                       (lowerText.Contains("i") && (lowerText.Contains("burger") || lowerText.Contains("pizza")));
            }
        }

        private bool IsOrderStatusIntent(string lowerText, string language)
        {
            if (language == "ar")
            {
                return lowerText.Contains("حالة الطلب") || 
                       lowerText.Contains("وين الطلب") || 
                       lowerText.Contains("الطلب") ||
                       lowerText.Contains("status");
            }
            else
            {
                return lowerText.Contains("order status") || 
                       lowerText.Contains("where is my order") ||
                       lowerText.Contains("order tracking");
            }
        }

        private bool IsComplaintIntent(string lowerText, string language)
        {
            if (language == "ar")
            {
                return lowerText.Contains("مش عاجبني") || 
                       lowerText.Contains("شكوى") || 
                       lowerText.Contains("مشكلة") ||
                       lowerText.Contains("غلط") ||
                       lowerText.Contains("مش راضي");
            }
            else
            {
                return lowerText.Contains("complaint") || 
                       lowerText.Contains("problem") ||
                       lowerText.Contains("wrong") ||
                       lowerText.Contains("not satisfied") ||
                       lowerText.Contains("disappointed");
            }
        }

        private bool IsRequestHumanAgentIntent(string lowerText, string language)
        {
            if (language == "ar")
            {
                return lowerText.Contains("موظف") || 
                       lowerText.Contains("بشري") || 
                       lowerText.Contains("انسان") ||
                       lowerText.Contains("human") ||
                       lowerText.Contains("agent");
            }
            else
            {
                return lowerText.Contains("human") || 
                       lowerText.Contains("agent") ||
                       lowerText.Contains("person") ||
                       lowerText.Contains("representative");
            }
        }

        private bool IsAskProductsIntent(string lowerText, string language)
        {
            if (language == "ar")
            {
                return lowerText.Contains("منيو") || 
                       lowerText.Contains("منتجات") || 
                       lowerText.Contains("قائمة") ||
                       lowerText.Contains("menu") ||
                       lowerText.Contains("products");
            }
            else
            {
                return lowerText.Contains("menu") || 
                       lowerText.Contains("products") ||
                       lowerText.Contains("items") ||
                       lowerText.Contains("what do you have");
            }
        }
    }
}


