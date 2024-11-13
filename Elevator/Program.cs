using Application;
using Domain;
using Domain.ElevatorAccessControlService;
using Domain.ElevatorDispatch;
using Domain.ElevatorEventService;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using Domain.Services;
using ElevatorConsole;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElevatorApp
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            // Resolve IElevatorConsoleManager instead of ElevatorConsoleManager
            var consoleManager = host.Services.GetRequiredService<IElevatorConsoleManager>();

            await consoleManager.StartAsync();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var buildingSettings = hostContext.Configuration.GetSection("BuildingSettings").Get<BuildingSettings>();
                    services.AddSingleton(buildingSettings);

                    // Register elevators as a singleton list
                    services.AddSingleton(serviceProvider =>
                    {
                        var settings = serviceProvider.GetRequiredService<BuildingSettings>();
                        var elevators = new List<Elevator>();

                        foreach (var config in settings.ElevatorSettings.Elevators)
                        {
                            var elevator = new Elevator
                            {
                                Id = config.Id,
                                MaxFloor = config.MaxFloor,
                                MaxPassengerCount = config.MaxPassengerCount,
                                MaxWeightCapacity = config.MaxWeightCapacity,
                                SpeedInMillisecondsPerFloor = config.SpeedInMillisecondsPerFloor,
                                ElevatorType = config.ElevatorType
                            };
                            elevators.Add(elevator);
                        }

                        return elevators;
                    });

                    services.AddSingleton<IElevator, FreightElevator>();
                    services.AddSingleton<IElevator, GlassElevator>();
                    services.AddSingleton<IElevator, PassengerElevator>();

                    // Register other required services
                    services.AddScoped<IElevatorService, ElevatorService>();
                    services.AddScoped<IElevatorControlFactory, ElevatorControlFactory>();
                    services.AddScoped<IElevatorAccessControlService, ElevatorAccessControlService>();
                    services.AddScoped<IElevatorDispatchService, ElevatorDispatchService>();
                    services.AddScoped<IElevatorEventService, ElevatorEventService>();
                    services.AddScoped<IElevatorStatusService, ElevatorStatusService>();
                    services.AddScoped<IElevatorMovementService, ElevatorMovementService>();
                    services.AddScoped<IElevatorOccupantService, ElevatorOccupantService>();
                    services.AddScoped<ILogger, ConsoleLogger>();
                    services.AddSingleton<ErrorHandler>();
                    services.AddLogging();

                    // Register ElevatorConsoleManager and ElevatorConsole for console interactions
                    services.AddSingleton<IElevatorConsoleManager, ElevatorConsoleManager>();
                    services.AddSingleton<ElevatorConsoleApp>(); // Register ElevatorConsole as the main console manager
                });
    }
}
