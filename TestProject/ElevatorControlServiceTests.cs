using Xunit;
using System.Collections.Generic;
using Domain;

namespace Application.Tests
{
    public class ElevatorControlServiceTests
    {
        private readonly ElevatorControlService _elevatorControlService;
        private readonly List<IElevator> _elevators;

        public ElevatorControlServiceTests()
        {
            _elevators = new List<IElevator>
            {
                new Elevator(1,1) {  MaxFloor = 10, MaxPassengerCount = 5 },
                new Elevator(2, 3) { MaxFloor = 8, MaxPassengerCount = 4 },
                new Elevator(3, 5) { MaxFloor = 10, MaxPassengerCount = 6 }
            };

            _elevatorControlService = new ElevatorControlService(_elevators);
        }

        [Fact]
        public void GetNearestElevator_Should_Return_Nearest_Elevator()
        {
            // Arrange
            int requestedFloor = 2;

            // Act
            var (elevator, errorCode) = _elevatorControlService.GetNearestElevator(requestedFloor);

            // Assert
            Assert.NotNull(elevator);
            Assert.Null(errorCode);
            Assert.Equal(1, elevator.Id); // Elevator 1 is the nearest
        }

        [Fact]
        public void GetNearestElevator_Should_Return_Error_When_No_Elevators_Available()
        {
            // Arrange
            var elevatorControlService = new ElevatorControlService(new List<IElevator>());

            // Act
            var (elevator, errorCode) = elevatorControlService.GetNearestElevator(1);

            // Assert
            Assert.Null(elevator);
            Assert.Equal(ErrorCode.NoAvailableElevators, errorCode);
        }

        [Fact]
        public void MoveToFloor_Should_Move_Elevator_To_Valid_Floor()
        {
            // Arrange
            var elevator = _elevators[0]; // Elevator 1
            int targetFloor = 3;
            int passengers = 2;

            // Act
            var errorCode = _elevatorControlService.MoveToFloor(elevator, targetFloor, passengers);

            // Assert
            Assert.Null(errorCode);
            Assert.Equal(targetFloor, elevator.CurrentFloor);
            Assert.Equal(passengers, elevator.PassengerCount);
        }

        [Fact]
        public void MoveToFloor_Should_Return_Error_When_Elevator_Is_Null()
        {
            // Arrange
            IElevator elevator = null;
            int targetFloor = 3;
            int passengers = 2;

            // Act
            var errorCode = _elevatorControlService.MoveToFloor(elevator, targetFloor, passengers);

            // Assert
            Assert.Equal(ErrorCode.NullElevator, errorCode);
        }

        [Fact]
        public void MoveToFloor_Should_Return_Error_When_Floor_Out_Of_Range()
        {
            // Arrange
            var elevator = _elevators[0]; // Elevator 1
            int targetFloor = 11; // Invalid floor
            int passengers = 1;

            // Act
            var errorCode = _elevatorControlService.MoveToFloor(elevator, targetFloor, passengers);

            // Assert
            Assert.Equal(ErrorCode.FloorOutOfRange, errorCode);
        }

        [Fact]
        public void MoveToFloor_Should_Return_Error_When_Exceeds_Passenger_Capacity()
        {
            // Arrange
            var elevator = _elevators[0]; // Elevator 1
            elevator.LoadPassengers(5); // Fill to max capacity
            int targetFloor = 2;
            int passengers = 1; // Attempting to load one more

            // Act
            var errorCode = _elevatorControlService.MoveToFloor(elevator, targetFloor, passengers);

            // Assert
            Assert.Equal(ErrorCode.ExceedsPassengerCapacity, errorCode);
        }

        [Fact]
        public void GetElevatorStatusById_Should_Return_Status_When_Elevator_Exists()
        {
            // Arrange
            int elevatorId = 2; // Elevator 2

            // Act
            var (status, errorCode) = _elevatorControlService.GetElevatorStatusById(elevatorId);

            // Assert
            Assert.Null(errorCode);
            Assert.Equal("Elevator 2: Floor 3 | Passengers: 0", status);
        }

        [Fact]
        public void GetElevatorStatusById_Should_Return_Error_When_Elevator_Not_Found()
        {
            // Arrange
            int elevatorId = 999; // Non-existent elevator ID

            // Act
            var (status, errorCode) = _elevatorControlService.GetElevatorStatusById(elevatorId);

            // Assert
            Assert.Equal(ErrorCode.ElevatorNotFound, errorCode);
            Assert.Equal("", status);
        }
    }
}
