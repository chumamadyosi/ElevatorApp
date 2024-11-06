using Application;
using Domain;
using System;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorConsoleManager
    {
        private readonly IElevatorControlService _elevatorControlService;
        private readonly int _totalFloors;

        public ElevatorConsoleManager(IElevatorControlService elevatorControlService, BuildingSettings settings)
        {
            _elevatorControlService = elevatorControlService ?? throw new ArgumentNullException(nameof(elevatorControlService));
            _totalFloors = settings.TotalFloors;

            // Subscribe to the OnFloorChanged event
            _elevatorControlService.OnFloorChanged += ElevatorControlService_OnFloorChanged;
        }

        // Event handler for floor changes
        private void ElevatorControlService_OnFloorChanged(int currentFloor, Direction direction)
        {
            // Output the elevator's current floor and direction to the console
            Console.WriteLine($"Elevator is now at floor {currentFloor}, moving {direction}.");
        }

        public async Task StartAsync()
        {
            while (true)
            {
                Console.WriteLine("Welcome to the Elevator Control System!");
                Console.WriteLine("1. Request Elevator");
                Console.WriteLine("2. Get Elevator Status");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RequestElevator();
                        break;
                    case "2":
                        await GetElevatorStatus();
                        break;
                    case "3":
                        Console.WriteLine("Exiting the Elevator Control System. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private async Task RequestElevator()
        {
            Console.Write("Enter requested floor (1 - MaxFloor): ");
            if (!int.TryParse(Console.ReadLine(), out int requestedFloor) || requestedFloor < 1 || requestedFloor > _totalFloors)
            {
                Console.WriteLine($"Invalid floor number. Please enter a floor between 1 and {_totalFloors}.");
                return;
            }

            Console.Write("Enter number of passengers: ");
            if (!int.TryParse(Console.ReadLine(), out int passengers) || passengers <= 0)
            {
                Console.WriteLine("Invalid number of passengers. Please enter a positive number.");
                return;
            }

            Console.Write("Enter requested direction (Up/Down): ");
            var directionInput = Console.ReadLine();
            if (!Enum.TryParse<Direction>(directionInput, true, out Direction requestedDirection))
            {
                Console.WriteLine("Invalid direction. Please enter 'Up' or 'Down'.");
                return;
            }

            // Get the nearest elevator
            var (elevator, errorCode) = _elevatorControlService.GetNearestElevator(requestedFloor, requestedDirection);
            if (elevator == null)
            {
                Console.WriteLine($"Error: {errorCode}");
                return;
            }

            // Request the elevator to move to the requested floor
            var requestError = await _elevatorControlService.RequestElevatorToFloor(elevator, requestedFloor, passengers);
            if (requestError.HasValue)
            {
                Console.WriteLine($"Error: {requestError}");
                return;
            }

            // Wait for the elevator to arrive at the requested floor
            await WaitForElevatorArrival(elevator, requestedFloor, requestedDirection);

            Console.WriteLine($"Elevator {elevator.Id} has arrived at floor {requestedFloor} with {passengers} passengers.");

            // Ask for the destination floor
            Console.Write("Enter destination floor (1 - MaxFloor): ");
            if (!int.TryParse(Console.ReadLine(), out int destinationFloor) || destinationFloor < 1 || destinationFloor > _totalFloors)
            {
                Console.WriteLine($"Invalid destination floor. Please enter a floor between 1 and {_totalFloors}.");
                return;
            }

            // Move the elevator to the destination floor
            var moveError = await _elevatorControlService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
            if (moveError.HasValue)
            {
                Console.WriteLine($"Error: {moveError}");
            }
            else
            {
                Console.WriteLine($"Elevator {elevator.Id} is now moving to destination floor {destinationFloor}.");
            }
        }

        private async Task WaitForElevatorArrival(Elevator elevator, int requestedFloor, Direction requestedDirection)
        {
            // Wait until the elevator reaches the requested floor
            while (elevator.CurrentFloor != requestedFloor)
            {
                await Task.Delay(500); // Check every 500ms
            }
        }

        private async Task GetElevatorStatus()
        {
            Console.Write("Enter elevator ID: ");
            if (!int.TryParse(Console.ReadLine(), out int elevatorId))
            {
                Console.WriteLine("Invalid elevator ID.");
                return;
            }

            var (status, errorCode) = _elevatorControlService.GetElevatorStatusById(elevatorId);
            if (errorCode.HasValue)
            {
                Console.WriteLine($"Error: {errorCode}");
            }
            else
            {
                Console.WriteLine(status);
            }
        }

        // Optionally, unsubscribe from the event when done (e.g., in Dispose method)
        public void Dispose()
        {
            _elevatorControlService.OnFloorChanged -= ElevatorControlService_OnFloorChanged;
        }
    }
}
