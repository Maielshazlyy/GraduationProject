using Domain_layer.Models;
using Service_layer.DTOS.Customer;

namespace Service_layer.Services_Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<IEnumerable<Customer>> GetByBusinessIdAsync(string businessId);
        Task<Customer?> GetByIdAsync(string id);
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer> CreateAsync(CustomerCreateDTO dto);
        Task<Customer?> UpdateAsync(string id, CustomerUpdateDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}

