using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorDispatch
{
    public interface IElevatorDispatchService
    {
        Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType);
        Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, ElevatorType elevatorType); 
    }
}
