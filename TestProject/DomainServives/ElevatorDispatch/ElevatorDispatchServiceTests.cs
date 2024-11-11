using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using TestProject.DomainServives.ElevatorDispatch.Setup;

namespace TestProject.DomainServives.ElevatorDispatch
{
    public class When_getting_nearest_elevator_Then : IClassFixture<ElevatorDispatchServiceFixtures>
    {
        private readonly (Elevator? Elevator, ErrorCode? ErrorCode) _result;
        private readonly ElevatorDispatchServiceFixtures _fixtures;

        public When_getting_nearest_elevator_Then(ElevatorDispatchServiceFixtures fixtures)
        {
            _fixtures = fixtures;

            _result = _fixtures._dispatchService
                .GetNearestElevator(5, Direction.Up, ElevatorType.Passenger)
                .Result;
        }

        [Fact]
        public void Return_nearest_elevator_if_available()
        {
            Assert.NotNull(_result.Elevator);
            Assert.Null(_result.ErrorCode);
        }
    }

    public class When_requesting_elevator_to_floor_Then : IClassFixture<ElevatorDispatchServiceFixtures>
    {
        private readonly ErrorCode? _result;
        private readonly ElevatorDispatchServiceFixtures _fixtures;

        public When_requesting_elevator_to_floor_Then(ElevatorDispatchServiceFixtures fixtures)
        {
            _fixtures = fixtures;

            var elevator = _fixtures._elevator;
            _result = _fixtures._dispatchService.RequestElevatorToFloor(elevator, 5).Result;
        }

        [Fact]
        public void Return_null_if_elevator_is_on_requested_floor()
        {
            var result = _fixtures._dispatchService.RequestElevatorToFloor(_fixtures._elevator, _fixtures._elevator.CurrentFloor).Result;
            Assert.Null(result);
        }

        [Fact]
        public void Return_error_if_requested_floor_out_of_range()
        {
            var result = _fixtures._dispatchService.RequestElevatorToFloor(_fixtures._elevator, 100).Result;
            Assert.Equal(ErrorCode.FloorOutOfRange, result);
        }
    }
    public class When_Elevator_Is_Not_Available_Return_Error : IClassFixture<ElevatorDispatchServiceFixtures>
    {
        private readonly ElevatorDispatchServiceFixtures _fixtures;
        private readonly (Elevator? elevator, ErrorCode? errorCode) _result;

        public When_Elevator_Is_Not_Available_Return_Error(ElevatorDispatchServiceFixtures fixtures)
        {
            _fixtures = fixtures;

            // Set up the scenario with no available elevators
            _fixtures.ClearElevators();  // Ensure the elevators list is empty

            // Run the method to get the result for the test
            _result = _fixtures._dispatchService.GetNearestElevator(5, Direction.Up, ElevatorType.Passenger).Result;
        }

        [Fact]
        public void Return_null_elevator_if_no_elevators_available()
        {
            Assert.Null(_result.elevator);
        }

        [Fact]
        public void Return_no_available_elevators_error_code()
        {
            Assert.Equal(ErrorCode.NoAvailableElevators, _result.errorCode);
        }
    }


}

