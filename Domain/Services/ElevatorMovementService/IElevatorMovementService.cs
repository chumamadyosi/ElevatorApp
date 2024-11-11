using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorMovementService
{
    public interface IElevatorMovementService
    {
        Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor);
        Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor);
    }
}
