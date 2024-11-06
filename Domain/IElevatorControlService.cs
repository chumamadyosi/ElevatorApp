using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IElevatorControlService
    {
        (Elevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection);
        // Request elevator to a specific floor and load passengers
        Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, int passengers);
        // Move elevator to the destination floor
        Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor);
        // Get elevator status by its ID
        (string status, ErrorCode? errorCode) GetElevatorStatusById(int elevatorId);
        // Elevator operation methods
        Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor);
        Task<ErrorCode?> LoadPassengersAsync(Elevator elevator, int count);
        bool AddPassengers(Elevator elevator, int count);
        // Event for floor change notifications
        event Action<int, Direction> OnFloorChanged;
    }
}
