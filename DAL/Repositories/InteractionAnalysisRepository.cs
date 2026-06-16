using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class InteractionAnalysisRepository : Repository<InteractionAnalysis>, IInteractionAnalysisRepository
    {
        private readonly AppDbContext _context;

        public InteractionAnalysisRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<InteractionAnalysis?> GetByInteractionIdAsync(string interactionId)
        {
            var results = await FindAsync(a => a.InteractionId == interactionId);
            return results.FirstOrDefault();
        }

        public async Task<IEnumerable<InteractionAnalysis>> GetByBusinessIdAsync(string businessId)
        {
            return await _context.InteractionAnalyses
                .Where(a => a.BusinessId == businessId)
                .Include(a => a.Interaction)
                    .ThenInclude(i => i.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<InteractionAnalysis>> GetByBusinessIdAndDateRangeAsync(
            string businessId, DateTime from, DateTime to)
        {
            return await _context.InteractionAnalyses
                .Where(a => a.BusinessId == businessId &&
                            a.AnalyzedAt >= from &&
                            a.AnalyzedAt <= to)
                .ToListAsync();
        }
    }
}
