using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Elevator
{
    public class FreightElevator : IElevator
    {
        private readonly IElevatorDispatchService _dispatchService;
        private readonly IElevatorMovementService _movementService;
        private readonly IElevatorOccupantService _occupantService;
        private readonly OccupantType occupantType = OccupantType.Cargo;

        public FreightElevator(
            IElevatorDispatchService dispatchService,
            IElevatorMovementService movementService,
            IElevatorOccupantService occupantService)
        {
            _dispatchService = dispatchService;
            _movementService = movementService;
            _occupantService = occupantService;
        }

        // Overriding the method from IElevatorDispatchService


        public (Elevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType = ElevatorType.Freight)
        {
            return _dispatchService.GetNearestElevator(requestedFloor, requestedDirection, elevatorType);
        }

        public Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, ElevatorType elevatorType = ElevatorType.Freight)
        {
            return _dispatchService.RequestElevatorToFloor(elevator, requestedFloor, elevatorType);
        }

        public Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            return _movementService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
        }

        public Task<ErrorCode?> LoadOccupantsAsync(Elevator elevator, int count, OccupantType occupantType = OccupantType.Cargo)
        {
            return _occupantService.LoadOccupantsAsync(elevator, count, occupantType);
        }

        public bool AddOccupants(Elevator elevator, int count, OccupantType occupantType = OccupantType.Cargo)
        {
            return _occupantService.AddOccupants(elevator, count, occupantType);
        }

        public Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor)
        {
            return _movementService.MoveToFloorAsync(elevator, floor);
        }
    }
}
