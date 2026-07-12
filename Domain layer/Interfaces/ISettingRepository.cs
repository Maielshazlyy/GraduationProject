using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>
    {
        Task<Setting?> GetByBusinessIdAsync(string businessId);
    }
}

