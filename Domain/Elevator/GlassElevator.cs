using Domain.ElevatorAccessControlService;
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
    public class GlassElevator : IElevator
    {
        private readonly IElevatorDispatchService _dispatchService;
        private readonly IElevatorMovementService _movementService;
        private readonly IElevatorOccupantService _occupantService;
        private readonly IElevatorAccessControlService _accessControlService;

        public GlassElevator(
            IElevatorDispatchService dispatchService,
            IElevatorMovementService movementService,
            IElevatorOccupantService occupantService, IElevatorAccessControlService accessControlService)
        {
            _dispatchService = dispatchService;
            _movementService = movementService;
            _occupantService = occupantService;
            _accessControlService = accessControlService;
        }

        public (Elevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType = ElevatorType.Passenger)
        {

            return _dispatchService.GetNearestElevator(requestedFloor, requestedDirection, elevatorType);
        }

        public Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, ElevatorType elevatorType = ElevatorType.Passenger)
        {
            return _dispatchService.RequestElevatorToFloor(elevator, requestedFloor, elevatorType);
        }

        public Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            return _movementService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
        }

        public Task<ErrorCode?> LoadOccupantsAsync(Elevator elevator, int count, OccupantType occupantType = OccupantType.Passenger)
        {
            return _occupantService.LoadOccupantsAsync(elevator, count, occupantType);
        }

        public bool AddOccupants(Elevator elevator, int count, OccupantType occupantType = OccupantType.Passenger)
        {
            return _occupantService.AddOccupants(elevator, count, occupantType);
        }

        public Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor)
        {
            return _movementService.MoveToFloorAsync(elevator, floor);
        }
    }

}
