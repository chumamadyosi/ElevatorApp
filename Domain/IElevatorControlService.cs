using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IElevatorControlService
    {
        (IElevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection);
        ErrorCode? MoveToFloor(IElevator elevator, int floor, int passengers);
        (string status, ErrorCode? errorCode) GetElevatorStatusById(int elevatorId);
        int ElevatorCount { get; }
    }
}
