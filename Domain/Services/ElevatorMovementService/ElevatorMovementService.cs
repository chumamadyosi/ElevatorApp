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
            try
            {
                if (destinationFloor < 1 || destinationFloor > elevator.MaxFloor)
                    return ErrorCode.FloorOutOfRange;
                elevator.Direction = elevator.CurrentFloor < destinationFloor ? Direction.Up : Direction.Down;

                // Delay once initially to simulate movement starting
                await Task.Delay(elevator.SpeedInMillisecondsPerFloor);

                // Loop until the elevator reaches the destination floor
                while (elevator.CurrentFloor != destinationFloor)
                {

                    elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                    await NotifyAndDelay(elevator, destinationFloor);
                }

                elevator.Direction = Direction.Stationary;

                // Final notification upon reaching the destination floor
                await NotifyAndDelay(elevator, destinationFloor);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving elevator {elevator.Id}: {ex.Message}");
            }
            return null;
        }

        private async Task NotifyAndDelay(Elevator elevator, int destinationFloor)
        {
            bool isAtDestination = elevator.CurrentFloor == destinationFloor;

            // Send notification to event service without blocking the main thread
            _ = Task.Run(() =>
            {
                _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction);
                if (isAtDestination && elevator.Direction == Direction.Stationary)
                {
                    Console.WriteLine($"Elevator {elevator.Id} has reached floor {elevator.CurrentFloor} and is now stationary.");
                }
                else
                {
                    Console.WriteLine($"Elevator {elevator.Id} is at floor {elevator.CurrentFloor}, moving {elevator.Direction}");
                }
            });

            // Delay only if not at destination, simulating elevator movement
            if (!isAtDestination)
            {
                await Task.Delay(elevator.SpeedInMillisecondsPerFloor);
            }
        }



        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int targetFloor)
        {
            if (targetFloor < 1 || targetFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            // Loop until the elevator reaches the target floor
            while (elevator.CurrentFloor != targetFloor)
            {
                elevator.Direction = elevator.CurrentFloor < targetFloor ? Direction.Up : Direction.Down;

                elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;

                _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction);

                // Simulate delay for moving between floors based on elevator speed
                await Task.Delay(elevator.SpeedInMillisecondsPerFloor); // Adjust delay for speed
            }

            elevator.Direction = Direction.Stationary;

            // Notify stationary state after reaching the target floor
            _elevatorEventService.RaiseFloorChangedEvent(elevator.CurrentFloor, elevator.Direction);

            return null;
        }

    }
}
