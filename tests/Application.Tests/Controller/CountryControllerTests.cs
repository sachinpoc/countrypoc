using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Application.Services;
using MyCleanArchitectureApp.WebAPI.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MyCleanArchitectureApp.Tests.Controllers
{
    public class CountryControllerTests
    {
        private readonly Mock<CountryService> _countryServiceMock;
        private readonly CountryController _countryController;

        public CountryControllerTests()
        {
            // Mock the CountryService
            _countryServiceMock = new Mock<CountryService>();

            // Create the CountryController with the mocked CountryService
            _countryController = new CountryController(_countryServiceMock.Object);

            // Mock User Claims for authorization (if required)
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _countryController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaims }
            };
        }

        [Fact]
        public async Task GetCountries_ShouldReturnOkWithListOfCountries()
        {
            // Arrange
            var countries = new List<CountryDto>
            {
                new CountryDto { Id = 1, Name = "Country1" },
                new CountryDto { Id = 2, Name = "Country2" }
            };

            _countryServiceMock.Setup(s => s.GetCountriesAsync()).ReturnsAsync(countries);

            // Act
            var result = await _countryController.GetCountries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCountries = Assert.IsType<List<CountryDto>>(okResult.Value);
            Assert.Equal(2, returnCountries.Count);
        }

        [Fact]
        public async Task GetCountry_ShouldReturnOkWithCountryById()
        {
            // Arrange
            var country = new CountryDto { Id = 1, Name = "Country1" };
            _countryServiceMock.Setup(s => s.GetCountryByIdAsync(1)).ReturnsAsync(country);

            // Act
            var result = await _countryController.GetCountry(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnCountry = Assert.IsType<CountryDto>(okResult.Value);
            Assert.Equal(1, returnCountry.Id);
            Assert.Equal("Country1", returnCountry.Name);
        }

        [Fact]
        public async Task AddCountry_ShouldReturnOkWhenCountryIsAdded()
        {
            // Arrange
            var newCountry = new CountryDto { Id = 3, Name = "NewCountry" };

            _countryServiceMock.Setup(s => s.AddCountryAsync(It.IsAny<CountryDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _countryController.AddCountry(newCountry);

            // Assert
            Assert.IsType<OkResult>(result);
            _countryServiceMock.Verify(s => s.AddCountryAsync(It.IsAny<CountryDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCountry_ShouldReturnOkWhenCountryIsUpdated()
        {
            // Arrange
            var updateCountry = new CountryDto { Id = 1, Name = "UpdatedCountry" };

            _countryServiceMock.Setup(s => s.UpdateCountryAsync(It.IsAny<CountryDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _countryController.UpdateCountry(updateCountry);

            // Assert
            Assert.IsType<OkResult>(result);
            _countryServiceMock.Verify(s => s.UpdateCountryAsync(It.IsAny<CountryDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCountry_ShouldReturnOkWhenCountryIsDeleted()
        {
            // Arrange
            var countryId = 1;

            _countryServiceMock.Setup(s => s.DeleteCountryAsync(countryId)).Returns(Task.CompletedTask);

            // Act
            var result = await _countryController.DeleteCountry(countryId);

            // Assert
            Assert.IsType<OkResult>(result);
            _countryServiceMock.Verify(s => s.DeleteCountryAsync(countryId), Times.Once);
        }

        [Fact]
        public void Controller_ShouldBeDecoratedWithAuthorizeAttribute()
        {
            // Assert that the controller has [Authorize] attribute
            var authorizeAttribute = (AuthorizeAttribute)_countryController
                .GetType()
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
                .FirstOrDefault();

            Assert.NotNull(authorizeAttribute);
        }
    }
}
