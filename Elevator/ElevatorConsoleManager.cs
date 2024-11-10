using Application;
using Domain;
using System;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorConsoleManager
    {
        private readonly IElevatorControlFactory _elevatorFactory;

        private readonly int _totalFloors;
        private readonly List<Elevator> _elevators;
        private readonly IElevatorService _elevatorService;

        public ElevatorConsoleManager(IElevatorControlFactory elevatorFactory, BuildingSettings settings, List<Elevator> elevators, IElevatorService elevatorService)
        {
            _elevatorFactory = elevatorFactory ?? throw new ArgumentNullException(nameof(elevatorFactory));
            _totalFloors = settings.TotalFloors;
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators));
            _elevatorService = elevatorService;
        }

        //public async Task StartAsync()
        //{
        //    // Use _elevators here as needed
        //}

        public async Task StartAsync()
        {
            while (true)
            {
                Console.WriteLine("Welcome to the Elevator Control System!");
                Console.WriteLine("1. Request Elevator");
                Console.WriteLine("2. Get Specific Elevator Status");
                Console.WriteLine("3. Display Real-Time Elevator Status");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RequestElevator();
                        break;
                    case "2":
                        await GetSpecificElevatorStatus();
                        break;
                    case "3":
                        await DisplayRealTimeElevatorStatus();
                        break;
                    case "4":
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
            Console.Write("Enter requested floor: ");//current floor
            if (!int.TryParse(Console.ReadLine(), out int requestedFloor) || requestedFloor < 1 || requestedFloor > _totalFloors)
            {
                Console.WriteLine($"Invalid floor number. Please enter a floor between 1 and {_totalFloors}.");
                return;
            }

            Console.WriteLine("Enter the requested direction (Up/Down):");//this should be a button in real life if you are on floor 3 going up you press up
            var directionInput = Console.ReadLine()?.Trim().ToLower();
            if (directionInput != "up" && directionInput != "down")
            {
                Console.WriteLine("Invalid direction. Please enter 'Up' or 'Down'.");
                return;
            }

            var requestedDirection = directionInput == "up" ? Direction.Up : Direction.Down;

            Console.WriteLine("Please specify the type of elevator (Passenger, Glass, Freight):");
            var elevatorTypeInput = Console.ReadLine()?.Trim();
            if (!Enum.TryParse(elevatorTypeInput, true, out ElevatorType elevatorType))
            {
                Console.WriteLine("Invalid elevator type. Please enter one of the following: Passenger, Glass, Freight.");
                return;
            }

            if (elevatorType == ElevatorType.Freight)
                Console.Write("Enter load in kgs: ");
            else
                Console.Write("Enter number of passengers: ");
            if (!int.TryParse(Console.ReadLine(), out int passengerCountOrLoadCapacity) || passengerCountOrLoadCapacity <= 0)
            {
                Console.WriteLine("Invalid number of passengers. Please enter a positive number.");
                return;
            }

            // Use the factory to create an elevator
            var elevatorFactory = _elevatorFactory.CreateElevatorControlService(ElevatorType.Passenger);  // For example, creating a PassengerElevator

            // Get the nearest elevator

            var (elevator, errorCode) = await _elevatorService.GetNearestElevator(requestedFloor, requestedDirection, ElevatorType.Passenger);
            if (elevator == null)
            {
                Console.WriteLine($"Error: {errorCode}");
                return;
            }

            var ocupantType = ElevatorOccupantTypeMapper.GetOccupantTypeForElevator(elevatorType);


            // Validate and load passengers using the ElevatorOccupantService
            var loadError = await elevatorFactory.LoadOccupants(elevator, passengerCountOrLoadCapacity);
            if (loadError.HasValue)
            {
                //log with error code saying capacity
                return;
            }

            // Request the elevator to move to the requested floor
            var requestError = await _elevatorService.RequestElevatorToFloor(elevator, requestedFloor);
            if (requestError.HasValue)
            {
                Console.WriteLine($"Error: {requestError}");
                return;
            }

            // Wait for the elevator to arrive at the requested floor
            await WaitForElevatorArrival(elevator, requestedFloor);

            Console.WriteLine($"Elevator {elevator.Id} has arrived at floor {requestedFloor}.");


            // Ask for the destination floor
            Console.Write("Enter destination floor: ");
            if (!int.TryParse(Console.ReadLine(), out int destinationFloor) || destinationFloor < 1 || destinationFloor > _totalFloors)
            {
                Console.WriteLine($"Invalid destination floor. Please enter a floor between 1 and {_totalFloors}.");
                return;
            }
            var tryAddOccupants = await elevatorFactory.AddOccupants(elevator, passengerCountOrLoadCapacity);
            if (tryAddOccupants)
            { // Move the elevator to the destination floor
                var moveError = await _elevatorService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
            }
            else
            {
                Console.WriteLine($"Elevator {elevator.Id} is now moving to destination floor {destinationFloor}.");
                return;
            }
        }

        private async Task WaitForElevatorArrival(Elevator elevator, int requestedFloor)
        {
            // Wait until the elevator reaches the requested floor
            while (elevator.CurrentFloor != requestedFloor)
            {
                await Task.Delay(500); // Check every 500ms
            }
        }

        private async Task GetSpecificElevatorStatus()
        {
            Console.Write("Enter elevator ID: ");
            if (!ulong.TryParse(Console.ReadLine(), out ulong elevatorId))
            {
                Console.WriteLine("Invalid elevator ID.");
                return;
            }

            var (status, errorCode) = _elevatorService.GetElevatorStatusById(elevatorId);
            if (errorCode.HasValue)
            {
                Console.WriteLine($"Error: {errorCode}");
            }
            else
            {
                Console.WriteLine(status);
            }
        }
        private async Task DisplayRealTimeElevatorStatus()
        {
            Console.WriteLine("Press 'Q' at any time to stop real-time status display.");
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Real-time status display stopped.");
                    break;
                }

                var elevatorStatuses = await _elevatorService.GetElevatorStatuses();
                Console.Clear();
                Console.WriteLine("Real-Time Elevator Status:");
                foreach (var (elevatorId, status, errorCode) in elevatorStatuses)
                {
                    if (errorCode.HasValue)
                    {
                        Console.WriteLine($"Elevator {elevatorId}: Error - {errorCode}");
                    }
                    else
                    {
                        Console.WriteLine(status);
                    }
                }
                await Task.Delay(1000); // Update status every second
            }
        }
    }
}
