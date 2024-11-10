using System.Threading.Tasks;

namespace Domain
{
    public interface IElevatorWithUserAccess
    {
        Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevatorWithUserAccess(Guid userId, int requestedFloor, Direction requestedDirection, ElevatorType elevatorType = ElevatorType.Passenger);
    }
}