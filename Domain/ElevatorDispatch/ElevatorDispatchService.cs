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
        private readonly IElevatorPassengerService _elevatorPassengerService;

        public ElevatorDispatchService(List<Elevator> elevators,
        IElevatorMovementService elevatorMovementService,
        IElevatorPassengerService elevatorPassengerService)
        {
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators));
            _elevatorMovementService = elevatorMovementService ?? throw new ArgumentNullException(nameof(elevatorMovementService));
            _elevatorPassengerService = elevatorPassengerService ?? throw new ArgumentNullException(nameof(elevatorPassengerService));
        }

        public (Elevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection)
        {
            if (!_elevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            var candidateElevators = _elevators
                .Where(e => e.Direction == Direction.Stationary ||
                            e.Direction == requestedDirection &&
                             (requestedDirection == Direction.Up && e.CurrentFloor <= requestedFloor ||
                              requestedDirection == Direction.Down && e.CurrentFloor >= requestedFloor))
                .ToList();

            if (!candidateElevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            var nearestElevator = candidateElevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .FirstOrDefault();

            return (nearestElevator, null);
        }

        // Request elevator to a specific floor and load passengers
        public async Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, int passengers)
        {
            if (elevator == null)
                return ErrorCode.NullElevator;

            if (requestedFloor < 1 || requestedFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            if (elevator.CurrentFloor == requestedFloor)
            {
                return await LoadPassengersIfNeeded(elevator, passengers);
            }

            var moveError = await _elevatorMovementService.MoveElevatorToDestinationFloor(elevator, requestedFloor);
            if (moveError.HasValue)
                return moveError;

            return await LoadPassengersIfNeeded(elevator, passengers);
        }

        private async Task<ErrorCode?> LoadPassengersIfNeeded(Elevator elevator, int passengers)
        {
            if (passengers > 0)
                return await _elevatorPassengerService.LoadPassengersAsync(elevator, passengers);
            return null; // No passengers to load
        }

    }
}
