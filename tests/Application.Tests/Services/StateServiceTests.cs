using Xunit;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyCleanArchitectureApp.Application.Services;
using MyCleanArchitectureApp.Application.DTOs;
using MyCleanArchitectureApp.Core.Entities;
using MyCleanArchitectureApp.Core.Interfaces;
using System.Linq;

namespace MyCleanArchitectureApp.Tests.Services
{
    public class StateServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMapper _mapper;
        private readonly StateService _stateService;

        public StateServiceTests()
        {
            // AutoMapper configuration for State and StateDto
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<State, StateDto>().ReverseMap();
            });
            _mapper = mappingConfig.CreateMapper();

            // Setup UnitOfWork mock
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            // Initialize StateService
            _stateService = new StateService(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task GetStatesAsync_ShouldReturnAllStates()
        {
            // Arrange
            var states = new List<State>
            {
                new State { Id = 1, Name = "State1" },
                new State { Id = 2, Name = "State2" }
            };

            _unitOfWorkMock.Setup(uow => uow.StateRepository.GetAllAsync()).ReturnsAsync(states);

            // Act
            var result = await _stateService.GetStatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("State1", result.First().Name);
        }

        [Fact]
        public async Task GetStateByIdAsync_ShouldReturnState()
        {
            // Arrange
            var state = new State { Id = 1, Name = "State1" };
            _unitOfWorkMock.Setup(uow => uow.StateRepository.GetByIdAsync(1)).ReturnsAsync(state);

            // Act
            var result = await _stateService.GetStateByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("State1", result.Name);
        }

        [Fact]
        public async Task AddStateAsync_ShouldAddNewState()
        {
            // Arrange
            var stateDto = new StateDto { Id = 3, Name = "NewState" };
            var state = _mapper.Map<State>(stateDto);

            _unitOfWorkMock.Setup(uow => uow.StateRepository.AddAsync(It.IsAny<State>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1); // Return a valid int (e.g., 1 for one entry saved)

            // Act
            await _stateService.AddStateAsync(stateDto);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.StateRepository.AddAsync(It.IsAny<State>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateStateAsync_ShouldUpdateExistingState()
        {
            // Arrange
            var stateDto = new StateDto { Id = 1, Name = "UpdatedState" };
            var state = _mapper.Map<State>(stateDto);

            _unitOfWorkMock.Setup(uow => uow.StateRepository.UpdateAsync(It.IsAny<State>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            await _stateService.UpdateStateAsync(stateDto);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.StateRepository.UpdateAsync(It.IsAny<State>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteStateAsync_ShouldDeleteState()
        {
            // Arrange
            var stateId = 1;

            _unitOfWorkMock.Setup(uow => uow.StateRepository.DeleteAsync(stateId)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync()).ReturnsAsync(1);

            // Act
            await _stateService.DeleteStateAsync(stateId);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.StateRepository.DeleteAsync(stateId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
