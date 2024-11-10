using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorDispatch
{
    public class ElevatorDispatchService : IElevatorDispatchService
    {
        private readonly List<Elevator> _elevators;
        private readonly IElevatorMovementService _elevatorMovementService;
        private readonly IElevatorOccupantService _elevatorPassengerService;

        public ElevatorDispatchService(List<Elevator> elevators,
        IElevatorMovementService elevatorMovementService,
        IElevatorOccupantService elevatorPassengerService)
        {
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators));
            _elevatorMovementService = elevatorMovementService ?? throw new ArgumentNullException(nameof(elevatorMovementService));
            _elevatorPassengerService = elevatorPassengerService ?? throw new ArgumentNullException(nameof(elevatorPassengerService));
        }

        public async Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType)
        {
            if (!_elevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            // Filter elevators based on type, direction, and current floor asynchronously
            var candidateElevators = await Task.Run(() =>
                _elevators
                    .Where(e => e.ElevatorType == elevatorType &&
                                (e.Direction == Direction.Stationary ||
                                 e.Direction == requestedDirection &&
                                 ((requestedDirection == Direction.Up && e.CurrentFloor <= requestedFloor) ||
                                  (requestedDirection == Direction.Down && e.CurrentFloor >= requestedFloor))))
                    .ToList()
            );

            if (!candidateElevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            // Find the nearest elevator based on the absolute distance to the requested floor
            var nearestElevator = candidateElevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .FirstOrDefault();

            return (nearestElevator, null);
        }

        // Request elevator to a specific floor and load passengers
        public async Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor)
        {
            if (elevator == null)
                return ErrorCode.NullElevator;

            // Validate the requested floor
            if (requestedFloor < 1 || requestedFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            // If the elevator is already at the requested floor, no need to move
            if (elevator.CurrentFloor == requestedFloor)
            {
                return null; // No need to move, just return
            }

            // Move the elevator to the requested floor
            var moveError = await _elevatorMovementService.MoveElevatorToDestinationFloor(elevator, requestedFloor);
            if (moveError.HasValue)
                return moveError;

            return null; // Successfully requested the elevator to move
        }



    }
}
