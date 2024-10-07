using Moq;
using Xunit;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using MyCleanArchitectureApp.Application.Services;
using MyCleanArchitectureApp.Core.Entities;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Core.Interfaces;

namespace MyCleanArchitectureApp.Tests.Services
{
    public class CountryServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly CountryService _countryService;

        public CountryServiceTests()
        {
            // AutoMapper configuration
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Country, CountryDto>().ReverseMap();
            });
            _mapper = mappingConfig.CreateMapper();

            // Setup UnitOfWork mock
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Initialize CountryService
            _countryService = new CountryService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task GetCountriesAsync_ShouldReturnAllCountries()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { Id = 1, Name = "Country1" },
                new Country { Id = 2, Name = "Country2" }
            };

            _unitOfWorkMock.Setup(uow => uow.CountryRepository.GetAllAsync()).ReturnsAsync(countries);

            // Act
            var result = await _countryService.GetCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Country1", result.First().Name);
        }

        [Fact]
        public async Task GetCountryByIdAsync_ShouldReturnCountry()
        {
            // Arrange
            var country = new Country { Id = 1, Name = "Country1" };
            _unitOfWorkMock.Setup(uow => uow.CountryRepository.GetByIdAsync(1)).ReturnsAsync(country);

            // Act
            var result = await _countryService.GetCountryByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Country1", result.Name);
        }

        [Fact]
        public async Task AddCountryAsync_ShouldAddNewCountry()
        {
            // Arrange
            var countryDto = new CountryDto { Id = 3, Name = "NewCountry" };
            var country = _mapper.Map<Country>(countryDto);

            _unitOfWorkMock.Setup(uow => uow.CountryRepository.AddAsync(It.IsAny<Country>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1); // Return a valid int (e.g., 1 for one entry saved)

            // Act
            await _countryService.AddCountryAsync(countryDto);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.CountryRepository.AddAsync(It.IsAny<Country>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCountryAsync_ShouldUpdateExistingCountry()
        {
            // Arrange
            var countryDto = new CountryDto { Id = 1, Name = "UpdatedCountry" };
            var country = _mapper.Map<Country>(countryDto);

            _unitOfWorkMock.Setup(uow => uow.CountryRepository.UpdateAsync(It.IsAny<Country>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            await _countryService.UpdateCountryAsync(countryDto);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.CountryRepository.UpdateAsync(It.IsAny<Country>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCountryAsync_ShouldDeleteCountry()
        {
            // Arrange
            var countryId = 1;

            _unitOfWorkMock.Setup(uow => uow.CountryRepository.DeleteAsync(countryId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            await _countryService.DeleteCountryAsync(countryId);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.CountryRepository.DeleteAsync(countryId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
