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

            // Loop until the elevator reaches the destination floor
            while (elevator.CurrentFloor != destinationFloor)
            {
                // Determine direction based on current and destination floor
                elevator.Direction = elevator.CurrentFloor < destinationFloor ? Direction.Up : Direction.Down;

                // Update the current floor based on the direction
                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                // Notify floor change and simulate movement delay based on elevator speed
                await NotifyAndDelay(elevator.CurrentFloor, elevator.Direction, elevator.SpeedInMillisecondsPerFloor);
            }

            // Once the elevator reaches the destination floor, set direction to stationary
            elevator.Direction = Direction.Stationary;
            elevator.PassengerCount = 0;

            // Notify stationary state after reaching the destination
            await NotifyAndDelay(elevator.CurrentFloor, elevator.Direction, elevator.SpeedInMillisecondsPerFloor);

            return null;
        }


        private async Task NotifyAndDelay(int currentFloor, Direction direction, int speedInMillisecondsPerFloor)
        {
            // Raise the event for floor change notifications
            _elevatorEventService.RaiseFloorChangedEvent(currentFloor, direction);

            // Simulate the movement delay based on the speed configuration
            await Task.Delay(speedInMillisecondsPerFloor);  // Speed-based delay
        }

        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int targetFloor)
        {
            if (targetFloor < 1 || targetFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            // Loop until the elevator reaches the target floor
            while (elevator.CurrentFloor != targetFloor)
            {
                // Determine direction based on current and target floor
                elevator.Direction = elevator.CurrentFloor < targetFloor ? Direction.Up : Direction.Down;

                // Update the current floor based on the direction
                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                // Raise the event for floor change notifications
                _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction);

                // Simulate delay for moving between floors based on elevator speed
                await Task.Delay(elevator.SpeedInMillisecondsPerFloor); // Adjust delay for speed
            }

            // Once the elevator reaches the target floor, set direction to stationary
            elevator.Direction = Direction.Stationary;

            // Notify stationary state after reaching the target floor
            _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction);

            return null;
        }

    }
}
