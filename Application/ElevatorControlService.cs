using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application
{
    public class ElevatorControlService : IElevatorControlService
    {
        private readonly List<Elevator> _elevators;

        public ElevatorControlService(List<Elevator> elevators)
        {
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators), "Elevator list cannot be null.");
        }

        // Method to get the nearest available elevator
        public (Elevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection)
        {
            if (!_elevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            var candidateElevators = _elevators
                .Where(e => e.Direction == Direction.Stationary ||
                            (e.Direction == requestedDirection &&
                             ((requestedDirection == Direction.Up && e.CurrentFloor <= requestedFloor) ||
                              (requestedDirection == Direction.Down && e.CurrentFloor >= requestedFloor))))
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

            // Check if elevator is already at the requested floor
            if (elevator.CurrentFloor == requestedFloor)
            {
                // No need to move, just load passengers if needed
                return await LoadPassengersAsync(elevator, passengers);
            }

            // If the elevator is not at the requested floor, move it to the requested floor
            var moveError = await MoveElevatorToDestinationFloor(elevator, requestedFloor);
            if (moveError.HasValue) return moveError;

            // Once the elevator has moved to the requested floor, load the passengers
            return await LoadPassengersAsync(elevator, passengers);
        }

        // Move elevator to the destination floor
        public async Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            if (destinationFloor < 1 || destinationFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            while (elevator.CurrentFloor != destinationFloor)
            {
                elevator.Direction = elevator.CurrentFloor < destinationFloor ? Direction.Up : Direction.Down;
                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                // Notify floor change
                OnFloorChanged?.Invoke(elevator.CurrentFloor, elevator.Direction);

                // Simulate delay for moving between floors
                await Task.Delay(1000); // Simulate moving to the next floor
            }

            elevator.Direction = Direction.Stationary;
            OnFloorChanged?.Invoke(elevator.CurrentFloor, elevator.Direction); // Notify arrival at the target floor

            return null;
        }

        // Get elevator status by its ID
        public (string status, ErrorCode? errorCode) GetElevatorStatusById(int elevatorId)
        {
            var elevator = _elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator == null)
                return ("", ErrorCode.ElevatorNotFound);

            var status = $"Elevator {elevator.Id}: Floor {elevator.CurrentFloor} | Passengers: {elevator.PassengerCount} | Direction: {elevator.Direction}";
            return (status, null);
        }

        // Move elevator to a specific floor asynchronously
        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int targetFloor)
        {
            if (targetFloor < 1 || targetFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            while (elevator.CurrentFloor != targetFloor)
            {
                elevator.Direction = elevator.CurrentFloor < targetFloor ? Direction.Up : Direction.Down;
                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                // Raise the event for floor change notifications
                OnFloorChanged?.Invoke(elevator.CurrentFloor, elevator.Direction);

                // Simulate delay for moving to the next floor
                await Task.Delay(1000); // Simulate moving between floors
            }

            elevator.Direction = Direction.Stationary; // Set elevator to stationary once it reaches the target floor
            OnFloorChanged?.Invoke(elevator.CurrentFloor, elevator.Direction); // Notify that it has arrived

            return null;
        }

        // Load passengers into the elevator
        public async Task<ErrorCode?> LoadPassengersAsync(Elevator elevator, int count)
        {
            if (count < 0)
                return ErrorCode.ExceedsPassengerCapacity;

            if (elevator.PassengerCount + count > elevator.MaxPassengerCount)
                return ErrorCode.ExceedsPassengerCapacity;

            elevator.PassengerCount += count;
            return null; // Successfully loaded passengers
        }

        // Add passengers to the elevator (non-async version)
        public bool AddPassengers(Elevator elevator, int count)
        {
            if (elevator.PassengerCount + count <= elevator.MaxPassengerCount)
            {
                elevator.PassengerCount += count;
                return true; // Successfully added passengers
            }
            return false; // Exceeded capacity
        }

        // Event for floor change notifications
        public event Action<int, Direction> OnFloorChanged;
    }
}
