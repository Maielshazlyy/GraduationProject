using DAL.Context;
using Domain_layer.Interfaces;
using Domain_layer.Models;

namespace DAL.Repositories
{
    public class WorkingHoursRepository : Repository<WorkingHours>, IWorkingHoursRepository
    {
        private readonly AppDbContext _context;

        public WorkingHoursRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkingHours>> GetByBusinessIdAsync(string businessId)
        {
            return await FindAsync(wh => wh.BusinessId == businessId);
        }
    }
}

