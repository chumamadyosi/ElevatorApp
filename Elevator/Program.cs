using Application;
using Domain;
using ElevatorConsole;
using System;
namespace ElevatorApp
{
    class Program
    {
        private static IElevatorControlService _elevatorControlService;
        private static int _totalFloors;
        static void Main(string[] args)
        {
            Console.Write("Enter the number of elevators: ");
            int numElevators = int.Parse(Console.ReadLine());

            Console.Write("Enter the number of floors: ");
            int totalFloors = int.Parse(Console.ReadLine());

            var elevators = new List<IElevator>();
            for (int i = 1; i <= numElevators; i++)
            {
                elevators.Add(new Elevator(i));
            }

            var elevatorControlService = new ElevatorControlService(elevators);
            var elevatorConsoleManager = new ElevatorConsoleManager(elevatorControlService, totalFloors);

            Console.WriteLine("Commands: 'call <floor> <passengers>', 'status'");
            while (true)
            {
                var input = Console.ReadLine();
                elevatorConsoleManager.HandleCommand(input);
            }
        }
    }
}