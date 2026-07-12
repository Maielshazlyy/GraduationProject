using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class KnowledgeBaseRepository : Repository<KnowledgeBase>, IKnowledgeBaseRepository
    {
        private readonly AppDbContext _context;
        
        public KnowledgeBaseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<KnowledgeBase>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(kb => kb.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<KnowledgeBase>> SearchAsync(string businessId, string searchTerm)
        {
            return await FindAsync(kb => 
                kb.BusinessId == businessId && 
                (kb.Question.Contains(searchTerm) || kb.Answer.Contains(searchTerm)));
        }
    }
}

