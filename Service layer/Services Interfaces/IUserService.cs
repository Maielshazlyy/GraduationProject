using Domain_layer.Models;
using Service_layer.DTOS.User;

namespace Service_layer.Services_Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetByBusinessIdAsync(string businessId);
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> UpdateAsync(string id, UserUpdateDTO dto);
        Task<bool> AssignRoleAsync(string userId, string newRole);
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Create a new human employee (Agent) under a specific business.
        /// Only Business Owners (or Admins) should be allowed to call this
        /// via the API, and the businessId should come from the owner's token.
        /// </summary>
        Task<User> CreateHumanEmployeeAsync(string businessId, CreateHumanEmployeeDTO dto);
    }
}

