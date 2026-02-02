using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.Setting;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IBusinessRepository _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SettingService(
            ISettingRepository settingRepository,
            IBusinessRepository businessRepository,
            IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Setting?> GetByBusinessIdAsync(string businessId)
        {
            return await _settingRepository.GetByBusinessIdAsync(businessId);
        }

        public async Task<Setting?> UpdateAsync(string businessId, SettingUpdateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(businessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{businessId}' not found.");

            var setting = await _settingRepository.GetByBusinessIdAsync(businessId);
            if (setting == null)
                throw new ArgumentException($"Settings not found for business '{businessId}'.");

            setting.AutoAssignTickets = dto.AutoAssignTickets;
            setting.EnableNotifications = dto.EnableNotifications;
            setting.Language = dto.Language;

            _settingRepository.Update(setting);
            await _unitOfWork.CompleteAsync();
            return setting;
        }
    }
}

