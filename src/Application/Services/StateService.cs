using AutoMapper;
using MyCleanArchitectureApp.Core.Interfaces;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Core.Entities;

namespace MyCleanArchitectureApp.Application.Services
{
    public class StateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StateDto>> GetStatesAsync()
        {
            var states = await _unitOfWork.StateRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StateDto>>(states);
        }

        public async Task<StateDto> GetStateByIdAsync(int id)
        {
            var state = await _unitOfWork.StateRepository.GetByIdAsync(id);
            return _mapper.Map<StateDto>(state);
        }

        public async Task AddStateAsync(StateDto stateDto)
        {
            var state = _mapper.Map<State>(stateDto);
            await _unitOfWork.StateRepository.AddAsync(state);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateStateAsync(StateDto stateDto)
        {
            var state = _mapper.Map<State>(stateDto);
            await _unitOfWork.StateRepository.UpdateAsync(state);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteStateAsync(int id)
        {
            await _unitOfWork.StateRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
