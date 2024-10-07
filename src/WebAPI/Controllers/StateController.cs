using Microsoft.AspNetCore.Mvc;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Application.Services;

namespace MyCleanArchitectureApp.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly StateService _stateService;

        public StateController(StateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            var states = await _stateService.GetStatesAsync();
            return Ok(states);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetState(int id)
        {
            var state = await _stateService.GetStateByIdAsync(id);
            return Ok(state);
        }

        [HttpPost]
        public async Task<IActionResult> AddState(StateDto stateDto)
        {
            await _stateService.AddStateAsync(stateDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateState(StateDto stateDto)
        {
            await _stateService.UpdateStateAsync(stateDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            await _stateService.DeleteStateAsync(id);
            return Ok();
        }
    }
}
