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

        public ElevatorConsoleManager(IElevatorControlService elevatorControlService, int totalFloors)
        {
            _elevatorControlService = elevatorControlService;
            _totalFloors = totalFloors;
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

                var nearestElevator = _elevatorControlService.GetNearestElevator(floor);
                _elevatorControlService.MoveToFloor(nearestElevator, floor, passengers);
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
                Console.WriteLine(_elevatorControlService.GetElevatorStatusById(i));
            }
        }
    }

}
