using Microsoft.AspNetCore.Mvc;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Application.Services;

namespace MyCleanArchitectureApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly CountryService _countryService;

        public CountryController(CountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _countryService.GetCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry(int id)
        {
            var country = await _countryService.GetCountryByIdAsync(id);
            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(CountryDto countryDto)
        {
            await _countryService.AddCountryAsync(countryDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCountry(CountryDto countryDto)
        {
            await _countryService.UpdateCountryAsync(countryDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            await _countryService.DeleteCountryAsync(id);
            return Ok();
        }
    }
}
