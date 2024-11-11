using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject.DomainServives.ElevatorStatus
{
    public class ElevatorStatusServiceTests
    {
        private readonly ElevatorStatusService _elevatorStatusService;
        private readonly List<Elevator> _elevators;

        public ElevatorStatusServiceTests()
        {
            _elevators = new List<Elevator>
            {
                new Elevator { Id = 1, CurrentFloor = 3, PassengerCount = 5, Direction = Direction.Up },
                new Elevator { Id = 2, CurrentFloor = 1, PassengerCount = 0, Direction = Direction.Stationary },
                new Elevator { Id = 3, CurrentFloor = 5, PassengerCount = 10, Direction = Direction.Down }
            };

            _elevatorStatusService = new ElevatorStatusService(_elevators);
        }

        [Fact]
        public async Task Return_error_if_elevator_not_found_in_GetElevatorStatusById()
        {
            ulong elevatorId = 999; // Non-existing ID

            var result = await _elevatorStatusService.GetElevatorStatusById(elevatorId); // Await the async call

            Assert.Equal(ErrorCode.ElevatorNotFound, result.errorCode);
            Assert.Equal("", result.status); // Status should be empty if not found
        }

        [Fact]
        public async Task Return_status_for_existing_elevator_in_GetElevatorStatusById()
        {
            ulong elevatorId = 1; // Existing ID

            var result = await _elevatorStatusService.GetElevatorStatusById(elevatorId); // Await the async call

            Assert.Null(result.errorCode); // No error for found elevator
            Assert.Equal("Elevator 1: Floor 3 | Passengers: 5 | Direction: Up", result.status); // Expected status format
        }

        [Fact]
        public async Task Return_all_elevator_statuses_in_GetElevatorStatuses()
        {
            var expectedStatuses = new List<(ulong elevatorId, string status, ErrorCode? errorCode)>
            {
                (1, "Elevator 1: Floor 3 | Passengers: 5 | Direction: Up", null),
                (2, "Elevator 2: Floor 1 | Passengers: 0 | Direction: Stationary", null),
                (3, "Elevator 3: Floor 5 | Passengers: 10 | Direction: Down", null)
            };

            var result = await _elevatorStatusService.GetElevatorStatuses(); // Await the async call

            Assert.Equal(expectedStatuses.Count, result.Count); // Ensure the count matches
            for (int i = 0; i < expectedStatuses.Count; i++)
            {
                Assert.Equal(expectedStatuses[i].elevatorId, result[i].elevatorId);
                Assert.Equal(expectedStatuses[i].status, result[i].status);
                Assert.Null(result[i].errorCode); // No error codes should be returned
            }
        }
    }
}
