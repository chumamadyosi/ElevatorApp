using Domain.ElevatorPassengerService;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DomainServives.ElevatorPassenger
{
    namespace TestProject.DomainServices.ElevatorPassengerService
    {
        public class ElevatorOccupantServiceTests
        {
            private readonly ElevatorOccupantService _elevatorOccupantService;

            public ElevatorOccupantServiceTests()
            {
                _elevatorOccupantService = new ElevatorOccupantService();
            }

            [Fact]
            public async Task Return_error_if_passenger_count_is_negative_in_LoadOccupants()
            {
                var elevator = new Elevator { PassengerCount = 0, MaxPassengerCount = 5 };
                int count = -1; // Invalid count

                var result = await _elevatorOccupantService.LoadOccupants(elevator, count);

                Assert.Equal(ErrorCode.ExceedsCapacity, result);
            }

            [Fact]
            public async Task Return_error_if_passenger_count_exceeds_max_capacity_in_LoadOccupants()
            {
                var elevator = new Elevator { PassengerCount = 4, MaxPassengerCount = 5 };
                int count = 2; // Exceeds capacity (4 + 2 > 5)

                var result = await _elevatorOccupantService.LoadOccupants(elevator, count);

                Assert.Equal(ErrorCode.ExceedsPassengerCapacity, result);
            }

            [Fact]
            public async Task Successfully_load_passengers_in_LoadOccupants()
            {
                var elevator = new Elevator { PassengerCount = 2, MaxPassengerCount = 5 };
                int count = 2; // Valid count (2 + 2 <= 5)

                var result = await _elevatorOccupantService.LoadOccupants(elevator, count);

                Assert.Null(result);
                Assert.Equal(4, elevator.PassengerCount); // After loading, the count should be 4
            }

            [Fact]
            public async Task Return_false_if_passenger_count_is_negative_in_AddOccupants()
            {
                var elevator = new Elevator { PassengerCount = 0, MaxPassengerCount = 5 };
                int count = -1; // Invalid count

                var result = await _elevatorOccupantService.AddOccupants(elevator, count);

                Assert.False(result);
            }

            [Fact]
            public async Task Return_false_if_passenger_count_exceeds_max_capacity_in_AddOccupants()
            {
                var elevator = new Elevator { PassengerCount = 4, MaxPassengerCount = 5 };
                int count = 2; // Exceeds capacity (4 + 2 > 5)

                var result = await _elevatorOccupantService.AddOccupants(elevator, count);

                Assert.False(result);
            }

            [Fact]
            public async Task Successfully_add_passengers_in_AddOccupants()
            {
                var elevator = new Elevator { PassengerCount = 2, MaxPassengerCount = 5 };
                int count = 2; // Valid count (2 + 2 <= 5)

                var result = await _elevatorOccupantService.AddOccupants(elevator, count);

                Assert.True(result);
                Assert.Equal(4, elevator.PassengerCount); // After adding, the count should be 4
            }
        }
    }
}
