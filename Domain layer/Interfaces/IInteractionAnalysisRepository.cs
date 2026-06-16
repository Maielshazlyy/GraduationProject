using Domain_layer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain_layer.Interfaces
{
    public interface IInteractionAnalysisRepository : IRepository<InteractionAnalysis>
    {
        Task<InteractionAnalysis?> GetByInteractionIdAsync(string interactionId);
        Task<IEnumerable<InteractionAnalysis>> GetByBusinessIdAsync(string businessId);
        Task<IEnumerable<InteractionAnalysis>> GetByBusinessIdAndDateRangeAsync(string businessId, DateTime from, DateTime to);
    }
}
