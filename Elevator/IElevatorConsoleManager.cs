using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public interface IElevatorConsoleManager
    {
        Task StartAsync();
        Task<ErrorCode?> RequestElevator();
        Task<ErrorCode?> GetSpecificElevatorStatus();
        Task DisplayRealTimeElevatorStatus();
        ElevatorRequest GetLiftRequest();
    }
}
