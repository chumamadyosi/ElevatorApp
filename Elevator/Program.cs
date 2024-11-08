using Application;
using Domain;
using ElevatorConsole;
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
            var consoleManager = host.Services.GetRequiredService<ElevatorConsoleManager>();

            await consoleManager.StartAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Configure building and elevator settings
                    var buildingSettings = hostContext.Configuration.GetSection("BuildingSettings").Get<BuildingSettings>();
                    services.AddSingleton(buildingSettings);

                    // Register the elevator control service and console manager
                    services.AddScoped<IElevatorStatusService, ElevatorStatusService>();
                    services.AddSingleton<ElevatorConsoleManager>();

                    // Register and configure elevators based on settings
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
                                MaxPassengerCount = config.MaxPassengerCount
                            };
                            elevators.Add(elevator);

                        }

                        return elevators;
                    });
                });
    }
}
