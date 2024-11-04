using Application;
using Domain;
using ElevatorConsole;
using Microsoft.Extensions.Configuration;
using System;
namespace ElevatorApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load settings from appsettings.json
            var elevatorSettings = LoadElevatorSettings();

            // Initialize elevators based on configuration
            var elevators = new List<IElevator>();
            for (int i = 1; i <= elevatorSettings.NumberOfElevators; i++)
            {
                elevators.Add(new Elevator(i));
            }

            int _totalFloors = elevatorSettings.NumberOfFloors;

            // Initialize the elevator control service and console manager
            var elevatorControlService = new ElevatorControlService(elevators);
            var elevatorConsoleManager = new ElevatorConsoleManager(elevatorControlService, _totalFloors);

            Console.WriteLine("Commands: 'call <floor> <passengers>', 'status'");
            while (true)
            {
                var input = Console.ReadLine();
                elevatorConsoleManager.HandleCommand(input);
            }
        }

        private static ElevatorSettings LoadElevatorSettings()
        {
            // Setup configuration to read from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            // Bind settings to ElevatorSettings class
            return config.GetSection("ElevatorSettings").Get<ElevatorSettings>()?? new ElevatorSettings(); // Ensure a fallback if null
        }
    }
}