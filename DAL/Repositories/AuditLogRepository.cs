using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        private readonly AppDbContext _context;
        
        public AuditLogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<AuditLog>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(a => a.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(string userId)
        {
            return await FindAsync(a => a.UserId == userId);
        }
    }
}

