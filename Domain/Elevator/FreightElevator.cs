using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
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


        public async Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType = ElevatorType.Freight)
        {
            return await _dispatchService.GetNearestElevator(requestedFloor, requestedDirection, elevatorType);
        }

        public async Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, ElevatorType elevatorType = ElevatorType.Freight)
        {
            return await _dispatchService.RequestElevatorToFloor(elevator, requestedFloor, elevatorType);
        }

        public async Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            return await _movementService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
        }

        public async Task<ErrorCode?> LoadOccupantsAsync(Elevator elevator, int count, OccupantType occupantType = OccupantType.Cargo)
        {
            return await _occupantService.LoadOccupantsAsync(elevator, count, occupantType);
        }

        public async Task<bool> AddOccupants(Elevator elevator, int count, OccupantType occupantType = OccupantType.Cargo)
        {
            return await _occupantService.AddOccupants(elevator, count, occupantType);
        }

        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor)
        {
            return await _movementService.MoveToFloorAsync(elevator, floor);
        }
    }
}
