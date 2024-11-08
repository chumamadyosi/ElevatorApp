using Domain.ElevatorAccessControlService;
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
    public class GlassElevator : IElevator, IElevatorWithUserAccess
    {
        private readonly IElevatorDispatchService _dispatchService;
        private readonly IElevatorMovementService _movementService;
        private readonly IElevatorOccupantService _occupantService;
        private readonly IElevatorAccessControlService _accessControlService;

        public GlassElevator(IElevatorDispatchService dispatchService, IElevatorMovementService movementService, IElevatorOccupantService occupantService, IElevatorAccessControlService accessControlService)
        {
            _dispatchService = dispatchService;
            _movementService = movementService;
            _occupantService = occupantService;
            _accessControlService = accessControlService;
        }

        public async Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType = ElevatorType.Passenger)
        {

            return await _dispatchService.GetNearestElevator(requestedFloor, requestedDirection, elevatorType);
        }

        public async Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, ElevatorType elevatorType = ElevatorType.Passenger)
        {
            return await _dispatchService.RequestElevatorToFloor(elevator, requestedFloor, elevatorType);
        }

        public Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            return _movementService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
        }

        public async Task<ErrorCode?> LoadOccupantsAsync(Elevator elevator, int count, OccupantType occupantType = OccupantType.Passenger)
        {
            return await _occupantService.LoadOccupantsAsync(elevator, count, occupantType);
        }

        public async Task<bool> AddOccupants(Elevator elevator, int count, OccupantType occupantType = OccupantType.Passenger)
        {

            return await _occupantService.AddOccupants(elevator, count, occupantType);
        }

        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor)
        {
            return await _movementService.MoveToFloorAsync(elevator, floor);
        }

        public async Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevatorWithUserAccess(Guid userId, int requestedFloor, Direction requestedDirection, ElevatorType elevatorType = ElevatorType.Passenger)
        {
            var authorizationResult = await _accessControlService.AuthorizeUser(userId);

            if (authorizationResult.errorCode.HasValue || !authorizationResult.hasAccess)
            {
                return (null, authorizationResult.errorCode); // Return access denied if the user is unauthorized
            }

            return await GetNearestElevator(requestedFloor, requestedDirection, elevatorType);
        }
    }

}
