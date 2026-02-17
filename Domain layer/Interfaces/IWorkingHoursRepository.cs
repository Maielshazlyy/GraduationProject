using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IWorkingHoursRepository : IRepository<WorkingHours>
    {
        Task<IEnumerable<WorkingHours>> GetByBusinessIdAsync(string businessId);
    }
}

