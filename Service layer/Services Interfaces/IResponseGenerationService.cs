using System.Threading.Tasks;
using Service_layer.DTOS.Chat;

namespace Service_layer.Services_Interfaces
{
    /// <summary>
    /// Generative AI service for response generation.
    /// Backend provides context (intent, action outcome, data), AI generates natural language reply.
    /// </summary>
    public interface IResponseGenerationService
    {
        /// <summary>
        /// Generate a natural, conversational response based on context.
        /// Backend provides the FACTS (what happened), AI generates HOW to say it.
        /// </summary>
        Task<string> GenerateResponseAsync(ResponseGenerationContextDTO context);
    }
}


