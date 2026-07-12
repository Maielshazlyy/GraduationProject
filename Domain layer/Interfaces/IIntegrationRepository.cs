using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface IIntegrationRepository : IRepository<Integration>
    {
        Task<IEnumerable<Integration>> GetByBusinessIdAsync(string businessId);
        Task<Integration?> GetByPlatformNameAsync(string businessId, string platformName);
    }
}

