using Domain_layer.Models;
using Service_layer.DTOS.Setting;

namespace Service_layer.Services_Interfaces
{
    public interface ISettingService
    {
        Task<Setting?> GetByBusinessIdAsync(string businessId);
        Task<Setting?> UpdateAsync(string businessId, SettingUpdateDTO dto);
    }
}

