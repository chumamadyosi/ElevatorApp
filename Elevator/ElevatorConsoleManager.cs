using Application;
using Domain;
using Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorConsoleManager
    {
        private readonly IElevatorControlFactory _elevatorFactory;
        private readonly BuildingSettings _settings;
        private readonly List<Elevator> _elevators;
        private readonly IElevatorService _elevatorService;
        private readonly ErrorHandler _errorHandler;

        public ElevatorConsoleManager(IElevatorControlFactory elevatorFactory, BuildingSettings settings, List<Elevator> elevators, IElevatorService elevatorService, ErrorHandler errorHandler, IOptions<BuildingSettings> options)
        {
            _settings = options.Value;
            _elevatorFactory = elevatorFactory ?? throw new ArgumentNullException(nameof(elevatorFactory));
            _settings.TotalFloors = settings.TotalFloors;
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators));
            _elevatorService = elevatorService;
            _errorHandler = errorHandler ?? throw new ArgumentNullException(nameof(errorHandler));
        }

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
                        var requestError = await RequestElevator();
                        if (requestError.HasValue)
                        {
                            _errorHandler.HandleError(requestError.Value);
                        }
                        break;
                    case "2":
                        var statusError = await GetSpecificElevatorStatus();
                        if (statusError.HasValue)
                        {
                            _errorHandler.HandleError(statusError.Value);
                        }
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

        private async Task<ErrorCode?> RequestElevator()
        {
            var elevatorRequest = GetLiftRequest();
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

            var loadError = await elevatorFactory.LoadOccupants(elevator, elevatorRequest.PassengerCountOrLoadCapacity);
            if (loadError.HasValue)
            {
                return loadError;
            }

            var requestError = await _elevatorService.RequestElevatorToFloor(elevator, elevatorRequest.RequestedFloor);
            if (requestError.HasValue)
            {
                return requestError;
            }

            await WaitForElevatorArrival(elevator, elevatorRequest.RequestedFloor);

            Console.WriteLine($"Elevator {elevator.Id} has arrived at floor {elevatorRequest.RequestedFloor}.");

            Console.Write("Enter destination floor: ");
            if (!int.TryParse(Console.ReadLine(), out int destinationFloor) || destinationFloor < 1 || destinationFloor > _settings.TotalFloors)
            {
                return ErrorCode.InvalidFloorRequest;
            }

            var moveError = await _elevatorService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
            if (moveError.HasValue)
            {
                return moveError;
            }

            Console.WriteLine($"Elevator {elevator.Id} is now moving to destination floor {destinationFloor}.");
            return null;
        }

        private async Task<ErrorCode?> GetSpecificElevatorStatus()
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
                        _errorHandler.HandleError(errorCode.Value);
                    }
                    else
                    {
                        Console.WriteLine(status);
                    }
                }
                await Task.Delay(1000); // Update status every second
            }
        }

        public ElevatorRequest GetLiftRequest() //press button up/down
        {
            var liftRequest = new ElevatorRequest();

            Console.Write("Enter current floor: ");
            if (!int.TryParse(Console.ReadLine(), out int requestedFloor) || requestedFloor < 1 || requestedFloor > _settings.TotalFloors)
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidFloorRequest };
            }
            liftRequest.RequestedFloor = requestedFloor;

            Console.WriteLine("Enter the requested direction (Up/Down):");
            var directionInput = Console.ReadLine()?.Trim().ToLower();
            if (directionInput != "up" && directionInput != "down")
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidDirection };
            }
            liftRequest.RequestedDirection = directionInput == "up" ? Direction.Up : Direction.Down;

            Console.WriteLine("Please specify the type of elevator (Passenger, Glass, Freight):");
            var elevatorTypeInput = Console.ReadLine()?.Trim();
            if (!Enum.TryParse(elevatorTypeInput, true, out ElevatorType elevatorType))
            {
                return new ElevatorRequest { ErrorCode = ErrorCode.InvalidElevatorType };
            }
            liftRequest.ElevatorType = elevatorType;

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

        private async Task WaitForElevatorArrival(Elevator elevator, int requestedFloor)
        {
            while (elevator.CurrentFloor != requestedFloor)
            {
                await Task.Delay(500);
            }
        }
    }
}


