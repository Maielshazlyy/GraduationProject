using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface ICallSummaryRepository : IRepository<CallSummary>
    {
        Task<CallSummary?> GetByCallIdAsync(string callId);
    }
}
