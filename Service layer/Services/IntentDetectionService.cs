using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service_layer.DTOS.Chat;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    /// <summary>
    /// Simple placeholder implementation for intent detection.
    /// In production, this should call an AI model and parse its JSON response.
    /// </summary>
    public class IntentDetectionService : IIntentDetectionService
    {
        public Task<DetectedIntentResultDTO> DetectIntentAsync(
            string businessId,
            string interactionId,
            IEnumerable<string> recentMessages)
        {
            // Very naive keyword-based stub, to be replaced with real AI integration.
            var last = recentMessages.LastOrDefault() ?? string.Empty;
            var lower = last.ToLowerInvariant();

            var result = new DetectedIntentResultDTO
            {
                Confidence = 0.5,
                RequiresAction = false
            };

            if (lower.Contains("order") || lower.Contains("اطلب") || lower.Contains("عايز") && lower.Contains("برجر"))
            {
                result.Intent = "CreateOrder";
                result.RequiresAction = true;
            }
            else if (lower.Contains("حالة الطلب") || lower.Contains("order status"))
            {
                result.Intent = "AskAboutOrderStatus";
            }
            else if (lower.Contains("مش عاجبني") || lower.Contains("شكوى") || lower.Contains("complaint"))
            {
                result.Intent = "Complaint";
                result.RequiresAction = true;
            }
            else if (lower.Contains("موظف") || lower.Contains("human") || lower.Contains("agent"))
            {
                result.Intent = "RequestHumanAgent";
                result.RequiresAction = true;
            }
            else if (lower.Contains("منيو") || lower.Contains("menu") || lower.Contains("products"))
            {
                result.Intent = "AskAboutProducts";
            }
            else
            {
                result.Intent = "GeneralQuestion";
            }

            return Task.FromResult(result);
        }
    }
}


