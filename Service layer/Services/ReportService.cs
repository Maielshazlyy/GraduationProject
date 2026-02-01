using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Domain_layer.enums;
using Service_layer.DTOS.Report;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Report> _reportRepository;
        private readonly IRepository<Business> _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(
            IRepository<Report> reportRepository,
            IRepository<Business> businessRepository,
            IUnitOfWork unitOfWork)
        {
            _reportRepository = reportRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _reportRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Report>> GetByBusinessIdAsync(string businessId)
        {
            var allReports = await _reportRepository.GetAllAsync();
            return allReports.Where(r => r.BusinessId == businessId);
        }

        public async Task<Report?> GetByIdAsync(string id)
        {
            return await _reportRepository.GetByIdAsync(id);
        }

        public async Task<Report> CreateAsync(ReportCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var report = new Report
            {
                Id = Guid.NewGuid().ToString(),
                ReportId = Guid.NewGuid().ToString(),
                Title = dto.Title,
                ReportType = ReportType.Performance, // Default type, can be made configurable
                GeneratedAt = DateTime.UtcNow,
                FilePath = string.Empty, // Will be set when report is generated
                BusinessId = dto.BusinessId
            };

            await _reportRepository.AddAsync(report);
            await _unitOfWork.CompleteAsync();
            return report;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null) return false;

            _reportRepository.Delete(report);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

