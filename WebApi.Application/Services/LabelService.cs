using WebApi.Application.Models;
using WebApi.Application.Repositories;

namespace WebApi.Application.Services
{
    public class LabelService
    {
        private readonly LabelRepository _labelRepository;
        public LabelService(LabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task<Label> CreateLabel(Label label)
        {
            return await _labelRepository.CreateLabel(label);
        }

        public async Task<Label> GetLabel(string id)
        {
            return await _labelRepository.GetLabel(id);
        }

        public async Task<IEnumerable<Label>> GetAllByUserId(int userId)
        {
            return await _labelRepository.GetAllByUserId(userId);
        }

        public async Task<bool> UpdateLabel(string labelId, Label label)
        {
            return await _labelRepository.UpdateLabel(labelId, label);
        }

        public async Task<bool> DeleteLabel(string labelId)
        {
            return await _labelRepository.DeleteLabel(labelId);
        }
    }
}
