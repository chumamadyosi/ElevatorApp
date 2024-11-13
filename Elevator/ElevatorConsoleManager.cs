using Application;
using Domain;
using Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorConsoleManager : IElevatorConsoleManager
    {
        private readonly IElevatorControlFactory _elevatorFactory;
        private readonly BuildingSettings _settings;
        private readonly IElevatorService _elevatorService;
        private readonly ErrorHandler _errorHandler;

        public ElevatorConsoleManager(IElevatorControlFactory elevatorFactory, BuildingSettings settings, IElevatorService elevatorService, ErrorHandler errorHandler, IOptions<BuildingSettings> options)
        {
            _settings = options.Value;
            _elevatorFactory = elevatorFactory ?? throw new ArgumentNullException(nameof(elevatorFactory));
            _settings.TotalFloors = settings.TotalFloors;
            _elevatorService = elevatorService;
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

        // Main loop to handle elevator control options in a console-based system
        public async Task StartAsync()
        {
            while (true)
            {
                // Display menu options to the user
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
                        // Handle elevator request and report any error through ErrorHandler
                        var requestError = await RequestElevator();
                        if (requestError.HasValue)
                        {
                            _errorHandler.HandleError(requestError.Value);
                        }
                        break;
                    case "2":
                        // Retrieve specific elevator status and handle any error
                        var statusError = await GetSpecificElevatorStatus();
                        if (statusError.HasValue)
                        {
                            _errorHandler.HandleError(statusError.Value);
                        }
                        break;
                    case "3":
                        // Display real-time elevator status updates
                        await DisplayRealTimeElevatorStatus();
                        break;
                    case "4":
                        // Exit the control system
                        Console.WriteLine("Exiting the Elevator Control System. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        // Requests an elevator and manages various steps of the process
        public async Task<ErrorCode?> RequestElevator()
        {
            var elevatorRequest = GetLiftRequest(); // Get lift request details from user
            if (elevatorRequest.ErrorCode != null)
            {
                return elevatorRequest.ErrorCode;
            }

            var elevatorFactory = _elevatorFactory.CreateElevatorControlService(ElevatorType.Passenger);

            var (elevator, errorCode) = await _elevatorService.GetNearestElevator(elevatorRequest.RequestedFloor, elevatorRequest.RequestedDirection, ElevatorType.Passenger);
            if (errorCode.HasValue)
            {
                return errorCode;
            }

            // Load occupants or freight into the elevator, Method to check if it is safe to load passengers before requesting
            var loadError = await elevatorFactory.LoadOccupants(elevator, elevatorRequest.PassengerCountOrLoadCapacity);
            if (loadError.HasValue)
            {
                return loadError;
            }

            // Request elevator to arrive at the specified floor
            var requestError = await _elevatorService.RequestElevatorToFloor(elevator, elevatorRequest.RequestedFloor);
            if (requestError.HasValue)
            {
                return requestError;
            }

            // Wait until the elevator arrives at the requested floor
            await WaitForElevatorArrival(elevator, elevatorRequest.RequestedFloor);

            Console.WriteLine($"Elevator {elevator.Id} has arrived at floor {elevatorRequest.RequestedFloor}.");

            // Prompt user for destination floor and validate input
            Console.Write("Enter destination floor: ");
            if (!int.TryParse(Console.ReadLine(), out int destinationFloor) || destinationFloor < 1 || destinationFloor > _settings.TotalFloors)
            {
                return ErrorCode.InvalidFloorRequest;
            }


            // Add the passengers once the elevator has arrived at the requested floor
            var addOccupants = await elevatorFactory.AddOccupants(elevator, elevatorRequest.PassengerCountOrLoadCapacity);
            if (!addOccupants)
            {
                return ErrorCode.ExceedsCapacity; // Return error that occurs when adding passengers after arrival
            }


            // Move the elevator to the destination floor and report any error
            Console.WriteLine($"Elevator {elevator.Id} is now moving to destination floor {destinationFloor}.");

            var _ = _elevatorService.MoveElevatorToDestinationFloor(elevator, destinationFloor);



            return null;
        }

        // Retrieves the status of a specific elevator by ID
        public async Task<ErrorCode?> GetSpecificElevatorStatus()
        {
            Console.Write("Enter elevator ID: ");
            if (!ulong.TryParse(Console.ReadLine(), out ulong elevatorId))
            {
                return ErrorCode.InvalidElevatorType;
            }


            var (status, errorCode) = await _elevatorService.GetElevatorStatusById(elevatorId);
            if (errorCode.HasValue)
            {
                return errorCode;
            }

            Console.WriteLine(status);
            return null;
        }

        // Continuously displays real-time status of all elevators until stopped by user
        public async Task DisplayRealTimeElevatorStatus()
        {

            while (true)
            {
                Console.WriteLine("Press 'Q' at any time to stop real-time status display.");
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Real-time status display stopped.");
                    break;
                }

                // Retrieve and display real-time statuses of all elevators
                var elevatorStatuses = await _elevatorService.GetElevatorStatuses();
                Console.Clear();
                Console.WriteLine("Real-Time Elevator Status:");
                foreach (var (elevatorId, status, errorCode) in elevatorStatuses)
                {
                    if (errorCode.HasValue)
                    {
                        _errorHandler.HandleError(errorCode.Value);
                    }
                    else
                    {
                        Console.WriteLine(status);
                    }
                }
                await Task.Delay(1000); 
            }
        }

        // Collects elevator request details from user input
        public ElevatorRequest GetLiftRequest()
        {
            var liftRequest = new ElevatorRequest();

            // Prompt user for the current floor and validate input
            Console.Write("Enter current floor: ");
            if (!int.TryParse(Console.ReadLine(), out int requestedFloor) || requestedFloor < 1 || requestedFloor > _settings.TotalFloors)
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidFloorRequest };
            }
            liftRequest.RequestedFloor = requestedFloor;

            // Prompt user for the desired direction and validate input
            Console.WriteLine("Enter the requested direction (Up/Down):");
            var directionInput = Console.ReadLine()?.Trim().ToLower();
            if (directionInput != "up" && directionInput != "down")
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidDirection };
            }
            liftRequest.RequestedDirection = directionInput == "up" ? Direction.Up : Direction.Down;

            // Prompt user for the type of elevator and validate input
            Console.WriteLine("Please specify the type of elevator (Passenger, Glass, Freight):");
            var elevatorTypeInput = Console.ReadLine()?.Trim();
            if (!Enum.TryParse(elevatorTypeInput, true, out ElevatorType elevatorType))
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidElevatorType };
            }
            liftRequest.ElevatorType = elevatorType;

            // Prompt user for passenger count or load capacity and validate input
            if (elevatorType == ElevatorType.Freight)
                Console.Write("Enter load in kgs: ");
            else
                Console.Write("Enter number of passengers: ");
            if (!int.TryParse(Console.ReadLine(), out int passengerCountOrLoadCapacity) || passengerCountOrLoadCapacity <= 0)
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidPassengerCount };
            }
            liftRequest.PassengerCountOrLoadCapacity = passengerCountOrLoadCapacity;

            return liftRequest;
        }

        // Waits until the elevator reaches the requested floor
        private async Task WaitForElevatorArrival(Elevator elevator, int requestedFloor)
        {
            while (elevator.CurrentFloor != requestedFloor)
            {
                await Task.Delay(500);
            }
        }
    }
}
