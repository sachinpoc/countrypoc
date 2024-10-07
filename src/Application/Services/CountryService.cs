using AutoMapper;
using MyCleanArchitectureApp.Core.Interfaces;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Core.Entities;

namespace MyCleanArchitectureApp.Application.Services
{
    public class CountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
        {
            var countries = await _unitOfWork.CountryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(countries);
        }

        public async Task<CountryDto> GetCountryByIdAsync(int id)
        {
            var country = await _unitOfWork.CountryRepository.GetByIdAsync(id);
            return _mapper.Map<CountryDto>(country);
        }

        public async Task AddCountryAsync(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);
            await _unitOfWork.CountryRepository.AddAsync(country);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCountryAsync(CountryDto countryDto)
        {
            var country = _mapper.Map<Country>(countryDto);
            await _unitOfWork.CountryRepository.UpdateAsync(country);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCountryAsync(int id)
        {
            await _unitOfWork.CountryRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
