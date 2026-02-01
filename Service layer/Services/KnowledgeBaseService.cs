using System;
using Domain_layer.Interfaces;
using Domain_layer.Models;
using Service_layer.DTOS.KnowledgeBase;
using Service_layer.Services_Interfaces;

namespace Service_layer.Services
{
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly IRepository<KnowledgeBase> _knowledgeBaseRepository;
        private readonly IRepository<Business> _businessRepository;
        private readonly IUnitOfWork _unitOfWork;

        public KnowledgeBaseService(
            IRepository<KnowledgeBase> knowledgeBaseRepository,
            IRepository<Business> businessRepository,
            IUnitOfWork unitOfWork)
        {
            _knowledgeBaseRepository = knowledgeBaseRepository;
            _businessRepository = businessRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<KnowledgeBase>> GetAllAsync()
        {
            return await _knowledgeBaseRepository.GetAllAsync();
        }

        public async Task<IEnumerable<KnowledgeBase>> GetByBusinessIdAsync(string businessId)
        {
            var allKb = await _knowledgeBaseRepository.GetAllAsync();
            return allKb.Where(kb => kb.BusinessId == businessId);
        }

        public async Task<KnowledgeBase?> GetByIdAsync(string id)
        {
            return await _knowledgeBaseRepository.GetByIdAsync(id);
        }

        public async Task<KnowledgeBase> CreateAsync(KnowledgeBaseCreateDTO dto)
        {
            var business = await _businessRepository.GetByIdAsync(dto.BusinessId);
            if (business == null)
                throw new ArgumentException($"Business with id '{dto.BusinessId}' not found.");

            var kb = new KnowledgeBase
            {
                KnowledgeBaseId = Guid.NewGuid().ToString(),
                Question = dto.Question,
                Answer = dto.Answer,
                BusinessId = dto.BusinessId,
                CreatedAt = DateTime.UtcNow
            };

            await _knowledgeBaseRepository.AddAsync(kb);
            await _unitOfWork.CompleteAsync();
            return kb;
        }

        public async Task<KnowledgeBase?> UpdateAsync(string id, KnowledgeBaseCreateDTO dto)
        {
            var kb = await _knowledgeBaseRepository.GetByIdAsync(id);
            if (kb == null) return null;

            kb.Question = dto.Question;
            kb.Answer = dto.Answer;

            _knowledgeBaseRepository.Update(kb);
            await _unitOfWork.CompleteAsync();
            return kb;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var kb = await _knowledgeBaseRepository.GetByIdAsync(id);
            if (kb == null) return false;

            _knowledgeBaseRepository.Delete(kb);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}

