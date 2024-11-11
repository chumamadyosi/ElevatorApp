using Domain.ElevatorEventService;
using Domain.ElevatorMovementService;
using Domain;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DomainServives.ElevatorMovement
{
    public class ElevatorMovementServiceTests
    {
        private readonly ElevatorMovementService _elevatorMovementService;
        private readonly IElevatorEventService _elevatorEventService;

        public ElevatorMovementServiceTests()
        {
            _elevatorEventService = Substitute.For<IElevatorEventService>();
            _elevatorMovementService = new ElevatorMovementService(_elevatorEventService);
        }

        
        public async Task Return_error_if_floor_is_out_of_range()
        {
            var elevator = new Elevator { CurrentFloor = 1, MaxFloor = 5, SpeedInMillisecondsPerFloor = 100 };
            int invalidDestinationFloor = 10;

            var result = await _elevatorMovementService.MoveElevatorToDestinationFloor(elevator, invalidDestinationFloor);

            Assert.Equal(ErrorCode.FloorOutOfRange, result);
        }

        [Fact]
        public async Task Move_elevator_to_destination_floor_successfully()
        {
            var elevator = new Elevator { CurrentFloor = 1, MaxFloor = 5, SpeedInMillisecondsPerFloor = 100 };
            int destinationFloor = 3;

            var result = await _elevatorMovementService.MoveElevatorToDestinationFloor(elevator, destinationFloor);

            Assert.Null(result);
            Assert.Equal(destinationFloor, elevator.CurrentFloor);
            Assert.Equal(Direction.Stationary, elevator.Direction);
        }

        [Fact]
        public async Task Return_error_if_target_floor_is_out_of_range()
        {
            var elevator = new Elevator { CurrentFloor = 1, MaxFloor = 5, SpeedInMillisecondsPerFloor = 100 };
            int invalidTargetFloor = 10;

            var result = await _elevatorMovementService.MoveToFloorAsync(elevator, invalidTargetFloor);

            Assert.Equal(ErrorCode.FloorOutOfRange, result);
        }

        [Fact]
        public async Task Move_elevator_to_target_floor_successfully()
        {
            var elevator = new Elevator { CurrentFloor = 1, MaxFloor = 5, SpeedInMillisecondsPerFloor = 100 };
            int targetFloor = 3;

            var result = await _elevatorMovementService.MoveToFloorAsync(elevator, targetFloor);

            Assert.Null(result);
            Assert.Equal(targetFloor, elevator.CurrentFloor);
            Assert.Equal(Direction.Stationary, elevator.Direction);
        }
    }
}
