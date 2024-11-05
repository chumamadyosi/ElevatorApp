using Domain;
using System;
using Xunit;

namespace TestProject
{
    public class ElevatorTests
    {
        private const int MaxFloor = 5; // Assuming max floor is 5 for tests
        private const int GroundFloor = 0; // Assuming ground floor is represented as 0

        [Fact]
        public void Elevator_Should_Initialize_On_Ground_Floor()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            var currentFloor = elevator.CurrentFloor;

            // Assert
            Assert.Equal(GroundFloor, currentFloor); // Elevator should start at ground floor (0)
        }

        [Fact]
        public void Elevator_Should_Add_Passengers_Within_Limit()
        {
            // Arrange
            var elevator = new Elevator(1) { MaxPassengerCount = 5 }; // Set max passengers for the test

            // Act
            var result = elevator.AddPassengers(3); // Add 3 passengers initially
            result &= elevator.AddPassengers(2); // Try to add 2 more passengers

            // Assert
            Assert.True(result); // Should return true, within limit
            Assert.Equal(5, elevator.PassengerCount); // Total should match max capacity
        }

        [Fact]
        public void Elevator_Should_Not_Exceed_Max_Passengers()
        {
            // Arrange
            var elevator = new Elevator(1) { MaxPassengerCount = 5 }; // Set max passengers for the test

            // Act
            var result1 = elevator.AddPassengers(5); // Fill to max
            var result2 = elevator.AddPassengers(1); // Attempt to exceed max capacity

            // Assert
            Assert.True(result1); // Initial fill within limit should return true
            Assert.False(result2); // Exceeding limit should return false
            Assert.Equal(5, elevator.PassengerCount); // Passenger count should not exceed max
        }

        [Fact]
        public void Elevator_Should_Move_To_Another_Floor_Within_Range()
        {
            // Arrange
            var elevator = new Elevator(1);

            // Act
            elevator.MoveToFloor(3); // Move to a valid floor within range

            // Assert
            Assert.Equal(3, elevator.CurrentFloor); // Current floor should be updated
        }

        [Fact]
        public void Elevator_Should_Not_Move_To_Invalid_Floor()
        {
            // Arrange
            var elevator = new Elevator(1) { MaxFloor = MaxFloor }; // Set max floor limit for the test

            // Act & Assert
            Assert.Throws<ArgumentException>(() => elevator.MoveToFloor(-1)); // Below ground floor
            Assert.Throws<ArgumentException>(() => elevator.MoveToFloor(MaxFloor + 1)); // Above max floor
        }

        [Fact]
        public void Elevator_Should_Have_PassengerCount_Reset_After_Move()
        {
            // Arrange
            var elevator = new Elevator(1) { MaxPassengerCount = 5 };
            elevator.AddPassengers(3); // Add passengers before moving

            // Act
            elevator.MoveToFloor(3); // Move to a different floor
            var passengerCountAfterMove = elevator.PassengerCount;

            // Assert
            Assert.Equal(0, passengerCountAfterMove); // Should reset after moving
        }
    }
}
