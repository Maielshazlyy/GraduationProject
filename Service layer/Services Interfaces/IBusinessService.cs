using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_layer.Models;

namespace Service_layer.Services_Interfaces
{
   public interface IBusinessService
    {
        Task<IEnumerable<Business>> GetAllAsync();
        Task<Business?> GetByIdAsync(int id);
        Task<Business> CreateAsync(Business business);
        Task<Business?> UpdateAsync(int id, Business business);
        Task<bool> DeleteAsync(int id);
    }
}
