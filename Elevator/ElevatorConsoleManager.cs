using Application;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorConsoleManager
    {
        private readonly IElevatorControlService _elevatorControlService;
        private readonly int _totalFloors;

        public ElevatorConsoleManager(IElevatorControlService elevatorControlService, BuildingSettings settings)
        {
            _elevatorControlService = elevatorControlService;
            _totalFloors = settings.TotalFloors;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("Commands: 'call <floor> <passengers>', 'status', 'exit'");

            while (true)
            {
                Console.Write("> ");
                var input = await Console.In.ReadLineAsync();
                if (string.Equals(input?.Trim(), "exit", StringComparison.OrdinalIgnoreCase))
                    break;

                HandleCommand(input);
            }
        }

        public void HandleCommand(string input)
        {
            var argsArray = input.Split(' ');

            if (argsArray[0].ToLower() == "call" && argsArray.Length == 3)
            {
                ProcessCallCommand(argsArray);
            }
            else if (argsArray[0].ToLower() == "status")
            {
                DisplayStatus();
            }
            else
            {
                Console.WriteLine("Invalid command. Use 'call <floor> <passengers>' or 'status'.");
            }
        }

        private void ProcessCallCommand(string[] args)
        {
            if (int.TryParse(args[1], out int floor) && int.TryParse(args[2], out int passengers))
            {
                if (floor < 1 || floor > _totalFloors)
                {
                    Console.WriteLine($"Invalid floor. Please enter a floor between 1 and {_totalFloors}.");
                    return;
                }

                // Determine the direction based on the requested floor
                var requestedDirection = floor > 1 ? Direction.Up : Direction.Down;

                var (nearestElevator, errorCode) = _elevatorControlService.GetNearestElevator(floor, requestedDirection);

                // Check if no elevator is available
                if (errorCode == ErrorCode.NoAvailableElevators)
                {
                    ErrorHandler.HandleError(errorCode.Value);
                    return;
                }

                // Now proceed to move the elevator
                var moveErrorCode = _elevatorControlService.MoveToFloor(nearestElevator, floor, passengers);

                // Handle any potential move errors
                if (moveErrorCode != null)
                {
                    ErrorHandler.HandleError(moveErrorCode.Value);
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Use the format 'call <floor> <passengers>'.");
            }
        }

        private void DisplayStatus()
        {
            for (int i = 1; i <= _elevatorControlService.ElevatorCount; i++)
            {
                var (status, errorCode) = _elevatorControlService.GetElevatorStatusById(i);
                if (errorCode == null)
                {
                    Console.WriteLine(status);
                }
                else
                {
                    ErrorHandler.HandleError(errorCode.Value);
                }
            }
        }
    }
}
