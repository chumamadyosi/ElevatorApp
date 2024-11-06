using Domain.ElevatorEventService;
using System;
using System.Threading.Tasks;

namespace Domain.ElevatorMovementService
{
    public class ElevatorMovementService : IElevatorMovementService
    {
        private readonly IElevatorEventService _elevatorEventService;

        public ElevatorMovementService(IElevatorEventService elevatorEventService)
        {
            _elevatorEventService = elevatorEventService;
        }

        public async Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            if (destinationFloor < 1 || destinationFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            while (elevator.CurrentFloor != destinationFloor)
            {
                elevator.Direction = elevator.CurrentFloor < destinationFloor ? Direction.Up : Direction.Down;
                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                // Notify floor change and simulate movement delay
                await NotifyAndDelay(elevator.CurrentFloor, elevator.Direction);
            }

            elevator.Direction = Direction.Stationary;
            await NotifyAndDelay(elevator.CurrentFloor, elevator.Direction); // Stationary notification

            return null;
        }

        private async Task NotifyAndDelay(int currentFloor, Direction direction)
        {
            _elevatorEventService.RaiseFloorChangedEvent(currentFloor, direction);
            await Task.Delay(1000); // Simulate moving between floors
        }


        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int targetFloor)
        {
            if (targetFloor < 1 || targetFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            while (elevator.CurrentFloor != targetFloor)
            {
                elevator.Direction = elevator.CurrentFloor < targetFloor ? Direction.Up : Direction.Down;
                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                // Raise the event for floor change notifications
                _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction);

                // Simulate delay for moving between floors
                await Task.Delay(1000); // Simulate moving between floors
            }

            elevator.Direction = Direction.Stationary; // Set elevator to stationary once it reaches the target floor
            _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction); // Notify arrival at the target floor

            return null;
        }
    }
}
