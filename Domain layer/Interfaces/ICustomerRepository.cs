using Domain_layer.Models;

namespace Domain_layer.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetByBusinessIdAsync(string businessId);
        Task<Customer?> GetByEmailAsync(string email);
    }
}

