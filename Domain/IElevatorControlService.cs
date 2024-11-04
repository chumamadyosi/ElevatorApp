using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IElevatorControlService
    {
        IElevator GetNearestElevator(int requestedFloor);
        void MoveToFloor(IElevator elevator, int floor, int passengers);
        string GetElevatorStatusById(int elevatorId);
        int ElevatorCount { get; }
    }
}
