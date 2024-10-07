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
    public class StateControllerTests
    {
        private readonly Mock<StateService> _stateServiceMock;
        private readonly StateController _stateController;

        public StateControllerTests()
        {
            // Mock the StateService
            _stateServiceMock = new Mock<StateService>();

            // Create the StateController with the mocked StateService
            _stateController = new StateController(_stateServiceMock.Object);

            // Mock User Claims for authorization (if required)
            var userClaims = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            _stateController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = userClaims }
            };
        }

        [Fact]
        public async Task GetStates_ShouldReturnOkWithListOfStates()
        {
            // Arrange
            var states = new List<StateDto>
            {
                new StateDto { Id = 1, Name = "State1" },
                new StateDto { Id = 2, Name = "State2" }
            };

            _stateServiceMock.Setup(s => s.GetStatesAsync()).ReturnsAsync(states);

            // Act
            var result = await _stateController.GetStates();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnStates = Assert.IsType<List<StateDto>>(okResult.Value);
            Assert.Equal(2, returnStates.Count);
        }

        [Fact]
        public async Task GetState_ShouldReturnOkWithStateById()
        {
            // Arrange
            var state = new StateDto { Id = 1, Name = "State1" };
            _stateServiceMock.Setup(s => s.GetStateByIdAsync(1)).ReturnsAsync(state);

            // Act
            var result = await _stateController.GetState(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnState = Assert.IsType<StateDto>(okResult.Value);
            Assert.Equal(1, returnState.Id);
            Assert.Equal("State1", returnState.Name);
        }

        [Fact]
        public async Task AddState_ShouldReturnOkWhenStateIsAdded()
        {
            // Arrange
            var newState = new StateDto { Id = 3, Name = "NewState" };

            _stateServiceMock.Setup(s => s.AddStateAsync(It.IsAny<StateDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _stateController.AddState(newState);

            // Assert
            Assert.IsType<OkResult>(result);
            _stateServiceMock.Verify(s => s.AddStateAsync(It.IsAny<StateDto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateState_ShouldReturnOkWhenStateIsUpdated()
        {
            // Arrange
            var updateState = new StateDto { Id = 1, Name = "UpdatedState" };

            _stateServiceMock.Setup(s => s.UpdateStateAsync(It.IsAny<StateDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _stateController.UpdateState(updateState);

            // Assert
            Assert.IsType<OkResult>(result);
            _stateServiceMock.Verify(s => s.UpdateStateAsync(It.IsAny<StateDto>()), Times.Once);
        }

        [Fact]
        public async Task DeleteState_ShouldReturnOkWhenStateIsDeleted()
        {
            // Arrange
            var stateId = 1;

            _stateServiceMock.Setup(s => s.DeleteStateAsync(stateId)).Returns(Task.CompletedTask);

            // Act
            var result = await _stateController.DeleteState(stateId);

            // Assert
            Assert.IsType<OkResult>(result);
            _stateServiceMock.Verify(s => s.DeleteStateAsync(stateId), Times.Once);
        }

        [Fact]
        public void Controller_ShouldBeDecoratedWithAuthorizeAttribute()
        {
            // Assert that the controller has [Authorize] attribute
            var authorizeAttribute = (AuthorizeAttribute)_stateController
                .GetType()
                .GetCustomAttributes(typeof(AuthorizeAttribute), inherit: true)
                .FirstOrDefault();

            Assert.NotNull(authorizeAttribute);
        }
    }
}
