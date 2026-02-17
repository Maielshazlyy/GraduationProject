using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_layer.DTOS.Chatbot;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly IBusinessAnalyticsService _analyticsService;

        public ChatbotService(IBusinessAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        public async Task<ChatbotResponseDTO> AskQuestionAsync(string businessId, ChatbotRequestDTO request)
        {
            // Get business analytics data
            var analytics = await _analyticsService.GetBusinessAnalyticsAsync(businessId);

            // Analyze the question and generate response
            var answer = GenerateResponse(request.Question, analytics);

            return new ChatbotResponseDTO
            {
                Answer = answer,
                ConversationId = request.ConversationId ?? Guid.NewGuid().ToString(),
                Suggestions = GenerateSuggestions(request.Question)
            };
        }

        private string GenerateResponse(string question, BusinessAnalyticsDTO analytics)
        {
            var lowerQuestion = question.ToLower();

            // Business Analysis Questions
            if (lowerQuestion.Contains("revenue") || lowerQuestion.Contains("sales") || lowerQuestion.Contains("income"))
            {
                return GenerateRevenueAnalysis(analytics);
            }

            if (lowerQuestion.Contains("customer") || lowerQuestion.Contains("clients"))
            {
                return GenerateCustomerAnalysis(analytics);
            }

            if (lowerQuestion.Contains("ticket") || lowerQuestion.Contains("support") || lowerQuestion.Contains("issue"))
            {
                return GenerateTicketAnalysis(analytics);
            }

            if (lowerQuestion.Contains("sentiment") || lowerQuestion.Contains("feeling") || lowerQuestion.Contains("emotion"))
            {
                return GenerateSentimentAnalysis(analytics);
            }

            if (lowerQuestion.Contains("feedback") || lowerQuestion.Contains("rating") || lowerQuestion.Contains("review"))
            {
                return GenerateFeedbackAnalysis(analytics);
            }

            if (lowerQuestion.Contains("order") || lowerQuestion.Contains("purchase"))
            {
                return GenerateOrderAnalysis(analytics);
            }

            if (lowerQuestion.Contains("recommend") || lowerQuestion.Contains("improve") || lowerQuestion.Contains("suggestion"))
            {
                return GenerateRecommendations(analytics);
            }

            if (lowerQuestion.Contains("performance") || lowerQuestion.Contains("overview") || lowerQuestion.Contains("summary"))
            {
                return GeneratePerformanceSummary(analytics);
            }

            // Default response
            return $"I can help you with business analysis, sentiment insights, sales recommendations, and customer satisfaction. " +
                   $"Your business '{analytics.BusinessName}' has {analytics.TotalOrders} orders, {analytics.TotalCustomers} customers, " +
                   $"and an average rating of {analytics.AverageRating:F1}/5.0. " +
                   $"What specific area would you like to know more about?";
        }

        private string GenerateRevenueAnalysis(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"📊 **Revenue Analysis for {analytics.BusinessName}:**\n");
            sb.AppendLine($"• Total Revenue: ${analytics.TotalRevenue:F2}");
            sb.AppendLine($"• Total Orders: {analytics.TotalOrders}");
            sb.AppendLine($"• Average Order Value: ${analytics.AverageOrderValue:F2}");
            sb.AppendLine($"• Completed Orders: {analytics.CompletedOrders}");
            sb.AppendLine($"• Pending Orders: {analytics.PendingOrders}\n");

            if (analytics.TotalOrders > 0)
            {
                var completionRate = (analytics.CompletedOrders / (double)analytics.TotalOrders) * 100;
                sb.AppendLine($"**Insights:**");
                sb.AppendLine($"• Order Completion Rate: {completionRate:F1}%");
                
                if (completionRate < 70)
                {
                    sb.AppendLine($"• ⚠️ Your completion rate is below 70%. Consider improving order processing time.");
                }
            }

            return sb.ToString();
        }

        private string GenerateCustomerAnalysis(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"👥 **Customer Analysis for {analytics.BusinessName}:**\n");
            sb.AppendLine($"• Total Customers: {analytics.TotalCustomers}");
            sb.AppendLine($"• New Customers (Last 30 Days): {analytics.NewCustomersLast30Days}");
            sb.AppendLine($"• Average Rating: {analytics.AverageRating:F1}/5.0");
            sb.AppendLine($"• Total Feedbacks: {analytics.TotalFeedbacks}\n");

            if (analytics.TotalFeedbacks > 0)
            {
                var satisfactionRate = (analytics.PositiveFeedbacks / (double)analytics.TotalFeedbacks) * 100;
                sb.AppendLine($"**Customer Satisfaction:**");
                sb.AppendLine($"• Positive Feedback Rate: {satisfactionRate:F1}%");
                sb.AppendLine($"• Positive Feedbacks: {analytics.PositiveFeedbacks}");
                sb.AppendLine($"• Negative Feedbacks: {analytics.NegativeFeedbacks}");
            }

            return sb.ToString();
        }

        private string GenerateTicketAnalysis(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"🎫 **Support Ticket Analysis for {analytics.BusinessName}:**\n");
            sb.AppendLine($"• Total Tickets: {analytics.TotalTickets}");
            sb.AppendLine($"• Open Tickets: {analytics.OpenTickets}");
            sb.AppendLine($"• In Progress: {analytics.InProgressTickets}");
            sb.AppendLine($"• Closed Tickets: {analytics.ClosedTickets}");
            
            if (analytics.AverageTicketResolutionTime > 0)
            {
                sb.AppendLine($"• Average Resolution Time: {analytics.AverageTicketResolutionTime:F1} hours\n");
            }

            if (analytics.TotalTickets > 0)
            {
                var resolutionRate = (analytics.ClosedTickets / (double)analytics.TotalTickets) * 100;
                sb.AppendLine($"**Insights:**");
                sb.AppendLine($"• Resolution Rate: {resolutionRate:F1}%");
                
                if (analytics.OpenTickets > analytics.ClosedTickets)
                {
                    sb.AppendLine($"• ⚠️ You have more open tickets than closed ones. Consider increasing support capacity.");
                }
            }

            return sb.ToString();
        }

        private string GenerateSentimentAnalysis(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"😊 **Sentiment Analysis for {analytics.BusinessName}:**\n");
            sb.AppendLine($"• Positive Sentiments: {analytics.PositiveSentiments}");
            sb.AppendLine($"• Negative Sentiments: {analytics.NegativeSentiments}");
            sb.AppendLine($"• Neutral Sentiments: {analytics.NeutralSentiments}");
            sb.AppendLine($"• Average Sentiment Score: {analytics.AverageSentimentScore:F2}\n");

            var totalSentiments = analytics.PositiveSentiments + analytics.NegativeSentiments + analytics.NeutralSentiments;
            if (totalSentiments > 0)
            {
                var positiveRate = (analytics.PositiveSentiments / (double)totalSentiments) * 100;
                sb.AppendLine($"**Insights:**");
                sb.AppendLine($"• Positive Sentiment Rate: {positiveRate:F1}%");
                
                if (analytics.NegativeSentiments > analytics.PositiveSentiments)
                {
                    sb.AppendLine($"• ⚠️ Negative sentiments exceed positive ones. Focus on improving customer experience.");
                }
                else if (positiveRate >= 70)
                {
                    sb.AppendLine($"• ✅ Great! Your customers are mostly satisfied.");
                }
            }

            return sb.ToString();
        }

        private string GenerateFeedbackAnalysis(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"⭐ **Feedback Analysis for {analytics.BusinessName}:**\n");
            sb.AppendLine($"• Average Rating: {analytics.AverageRating:F1}/5.0");
            sb.AppendLine($"• Total Feedbacks: {analytics.TotalFeedbacks}");
            sb.AppendLine($"• Positive Feedbacks (4-5 stars): {analytics.PositiveFeedbacks}");
            sb.AppendLine($"• Negative Feedbacks (1-2 stars): {analytics.NegativeFeedbacks}\n");

            if (analytics.TotalFeedbacks > 0)
            {
                var satisfactionRate = (analytics.PositiveFeedbacks / (double)analytics.TotalFeedbacks) * 100;
                sb.AppendLine($"**Insights:**");
                sb.AppendLine($"• Customer Satisfaction Rate: {satisfactionRate:F1}%");
                
                if (analytics.AverageRating < 3.5)
                {
                    sb.AppendLine($"• ⚠️ Your average rating is below 3.5. Focus on addressing customer concerns.");
                }
                else if (analytics.AverageRating >= 4.5)
                {
                    sb.AppendLine($"• ✅ Excellent! You're maintaining high customer satisfaction.");
                }
            }

            return sb.ToString();
        }

        private string GenerateOrderAnalysis(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"🛒 **Order Analysis for {analytics.BusinessName}:**\n");
            sb.AppendLine($"• Total Orders: {analytics.TotalOrders}");
            sb.AppendLine($"• Total Revenue: ${analytics.TotalRevenue:F2}");
            sb.AppendLine($"• Average Order Value: ${analytics.AverageOrderValue:F2}");
            sb.AppendLine($"• Completed Orders: {analytics.CompletedOrders}");
            sb.AppendLine($"• Pending Orders: {analytics.PendingOrders}\n");

            if (analytics.LastOrderDate != DateTime.MinValue)
            {
                var daysSinceLastOrder = (DateTime.UtcNow - analytics.LastOrderDate).Days;
                sb.AppendLine($"• Last Order: {daysSinceLastOrder} days ago");
            }

            return sb.ToString();
        }

        private string GenerateRecommendations(BusinessAnalyticsDTO analytics)
        {
            var recommendations = new List<string>();

            // Revenue recommendations
            if (analytics.AverageOrderValue < 50)
            {
                recommendations.Add("💡 **Increase Average Order Value:** Consider upselling or bundling products to increase revenue per order.");
            }

            // Customer satisfaction recommendations
            if (analytics.AverageRating < 4.0)
            {
                recommendations.Add("💡 **Improve Customer Satisfaction:** Focus on addressing negative feedback and improving service quality.");
            }

            if (analytics.NegativeSentiments > analytics.PositiveSentiments)
            {
                recommendations.Add("💡 **Address Negative Sentiments:** Review customer interactions and identify common pain points.");
            }

            // Ticket recommendations
            if (analytics.OpenTickets > analytics.ClosedTickets)
            {
                recommendations.Add("💡 **Improve Support Response Time:** Consider adding more support staff or implementing automated responses.");
            }

            if (analytics.AverageTicketResolutionTime > 24)
            {
                recommendations.Add("💡 **Reduce Ticket Resolution Time:** Aim to resolve tickets within 24 hours to improve customer satisfaction.");
            }

            // Sales recommendations
            if (analytics.NewCustomersLast30Days < 10 && analytics.TotalCustomers > 0)
            {
                recommendations.Add("💡 **Increase Customer Acquisition:** Consider marketing campaigns or referral programs to attract new customers.");
            }

            // Menu recommendations
            if (analytics.AvailableMenuItems < analytics.TotalMenuItems * 0.8)
            {
                recommendations.Add("💡 **Restock Menu Items:** Ensure most items are available to maximize sales opportunities.");
            }

            var sb = new StringBuilder();
            sb.AppendLine($"💼 **Recommendations for {analytics.BusinessName}:**\n");

            if (recommendations.Any())
            {
                foreach (var rec in recommendations)
                {
                    sb.AppendLine(rec);
                }
            }
            else
            {
                sb.AppendLine("✅ Your business is performing well! Keep up the good work.");
                sb.AppendLine("• Continue monitoring customer feedback");
                sb.AppendLine("• Maintain high service quality");
                sb.AppendLine("• Focus on customer retention");
            }

            return sb.ToString();
        }

        private string GeneratePerformanceSummary(BusinessAnalyticsDTO analytics)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"📈 **Performance Summary for {analytics.BusinessName}:**\n");
            sb.AppendLine($"**Business Overview:**");
            sb.AppendLine($"• Type: {analytics.BusinessType}");
            sb.AppendLine($"• Total Revenue: ${analytics.TotalRevenue:F2}");
            sb.AppendLine($"• Total Orders: {analytics.TotalOrders}");
            sb.AppendLine($"• Total Customers: {analytics.TotalCustomers}\n");

            sb.AppendLine($"**Customer Satisfaction:**");
            sb.AppendLine($"• Average Rating: {analytics.AverageRating:F1}/5.0");
            sb.AppendLine($"• Positive Sentiment Rate: {GetPositiveSentimentRate(analytics):F1}%\n");

            sb.AppendLine($"**Support Performance:**");
            sb.AppendLine($"• Total Tickets: {analytics.TotalTickets}");
            sb.AppendLine($"• Resolution Rate: {GetResolutionRate(analytics):F1}%\n");

            sb.AppendLine($"**Recent Activity:**");
            if (analytics.LastOrderDate != DateTime.MinValue)
            {
                var daysSinceLastOrder = (DateTime.UtcNow - analytics.LastOrderDate).Days;
                sb.AppendLine($"• Last Order: {daysSinceLastOrder} days ago");
            }
            if (analytics.LastTicketDate != DateTime.MinValue)
            {
                var daysSinceLastTicket = (DateTime.UtcNow - analytics.LastTicketDate).Days;
                sb.AppendLine($"• Last Ticket: {daysSinceLastTicket} days ago");
            }

            return sb.ToString();
        }

        private double GetPositiveSentimentRate(BusinessAnalyticsDTO analytics)
        {
            var total = analytics.PositiveSentiments + analytics.NegativeSentiments + analytics.NeutralSentiments;
            if (total == 0) return 0;
            return (analytics.PositiveSentiments / (double)total) * 100;
        }

        private double GetResolutionRate(BusinessAnalyticsDTO analytics)
        {
            if (analytics.TotalTickets == 0) return 0;
            return (analytics.ClosedTickets / (double)analytics.TotalTickets) * 100;
        }

        private List<string> GenerateSuggestions(string question)
        {
            var suggestions = new List<string>();

            if (question.ToLower().Contains("revenue") || question.ToLower().Contains("sales"))
            {
                suggestions.Add("What are my top selling items?");
                suggestions.Add("How can I increase revenue?");
            }
            else if (question.ToLower().Contains("customer"))
            {
                suggestions.Add("What is my customer satisfaction rate?");
                suggestions.Add("How many new customers did I get?");
            }
            else if (question.ToLower().Contains("ticket"))
            {
                suggestions.Add("What is my average ticket resolution time?");
                suggestions.Add("How many open tickets do I have?");
            }
            else
            {
                suggestions.Add("What is my business performance overview?");
                suggestions.Add("What are recommendations to improve sales?");
                suggestions.Add("What is my customer sentiment analysis?");
                suggestions.Add("How can I improve customer satisfaction?");
            }

            return suggestions;
        }
    }
}

