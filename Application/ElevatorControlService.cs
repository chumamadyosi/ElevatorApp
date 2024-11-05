using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ElevatorControlService : IElevatorControlService
    {
        private readonly List<IElevator> _elevators;

        public int ElevatorCount => _elevators.Count;

        public ElevatorControlService(List<IElevator> elevators)
        {
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators), "Elevator list cannot be null.");
        }

        public (IElevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection)
        {
            if (!_elevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            // Filter elevators by those moving in the requested direction or idle
            var candidateElevators = _elevators
                .Where(e => e.Direction == Direction.Stationary ||
                            (e.Direction == requestedDirection &&
                            ((requestedDirection == Direction.Up && e.CurrentFloor <= requestedFloor) ||
                             (requestedDirection == Direction.Down && e.CurrentFloor >= requestedFloor))))
                .ToList();

            if (!candidateElevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            // Choose the nearest elevator among candidates
            var nearestElevator = candidateElevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .FirstOrDefault();

            return (nearestElevator, null);
        }


        public ErrorCode? MoveToFloor(IElevator elevator, int floor, int passengers)
        {
            if (elevator == null)
                return ErrorCode.NullElevator;

            if (floor < 1 || floor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            if (passengers + elevator.PassengerCount > elevator.MaxPassengerCount)
                return ErrorCode.ExceedsPassengerCapacity;

            elevator.MoveToFloor(floor);
            elevator.LoadPassengers(passengers);

            return null;
        }

        public (string status, ErrorCode? errorCode) GetElevatorStatusById(int elevatorId)
        {
            var elevator = _elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator == null)
                return ("", ErrorCode.ElevatorNotFound);

            var status = $"Elevator {elevator.Id}: Floor {elevator.CurrentFloor} | Passengers: {elevator.PassengerCount}";
            return (status, null);
        }
    }

}
