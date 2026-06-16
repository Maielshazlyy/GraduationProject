using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;

namespace DAL.Repositories
{
    public class CallSummaryRepository : Repository<CallSummary>, ICallSummaryRepository
    {
        public CallSummaryRepository(AppDbContext context) : base(context) { }

        public async Task<CallSummary?> GetByCallIdAsync(string callId)
            => await FirstOrDefaultAsync(c => c.CallId == callId);

        public async Task<CallSummary?> GetByInteractionIdAsync(string interactionId)
            => await FirstOrDefaultAsync(c => c.InteractionId == interactionId);

        public async Task<IEnumerable<CallSummary>> GetByBusinessIdAsync(string businessId)
            => await FindAsync(c => c.BusinessId == businessId);
    }
}
