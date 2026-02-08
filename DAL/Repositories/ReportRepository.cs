using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        private readonly AppDbContext _context;
        
        public ReportRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Report>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(r => r.BusinessId == businessId);
        }
        
        public async Task<IEnumerable<Report>> GetByReportTypeAsync(string reportType)
        {
            return await FindAsync(r => r.ReportType.ToString() == reportType);
        }
    }
}

