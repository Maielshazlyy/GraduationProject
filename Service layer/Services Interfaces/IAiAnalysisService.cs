using System.Threading.Tasks;
using Service_layer.DTOS.AiAnalysis;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Sends completed chat sessions to the AI analysis pipeline
    /// and returns structured analysis results.
    ///
    /// Called post-session only — not real-time.
    /// Trigger: Interaction closed, ended, or timed out.
    /// </summary>
    public interface IAiAnalysisService
    {
        /// <summary>
        /// Builds the analysis request from the DB for the given interaction,
        /// sends it to POST /api/v1/analysis/chat-batch,
        /// and returns the structured analysis result.
        /// </summary>
        Task<AiSessionAnalysisResultDTO?> AnalyzeSessionAsync(string interactionId);
    }
}
