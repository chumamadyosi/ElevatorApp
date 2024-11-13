using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorConsoleApp
    {
        private readonly IElevatorConsoleManager _elevatorConsoleService;

        public ElevatorConsoleApp(IElevatorConsoleManager elevatorConsoleService)
        {
            _elevatorConsoleService = elevatorConsoleService ?? throw new ArgumentNullException(nameof(elevatorConsoleService));
        }

        public Task StartAsync() => _elevatorConsoleService.StartAsync();

        public Task<ErrorCode?> RequestElevator() => _elevatorConsoleService.RequestElevator();

        public Task<ErrorCode?> GetSpecificElevatorStatus() => _elevatorConsoleService.GetSpecificElevatorStatus();

        public Task DisplayRealTimeElevatorStatus() => _elevatorConsoleService.DisplayRealTimeElevatorStatus();

        public ElevatorRequest GetLiftRequest() => _elevatorConsoleService.GetLiftRequest();
    }
}
