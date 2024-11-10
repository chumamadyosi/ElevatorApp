namespace Domain
{
    public interface IElevatorStatusService
    {
        // Get status of a specific elevator by ID
        (string status, ErrorCode? errorCode) GetElevatorStatusById(ulong elevatorId);

        // Get statuses of all elevators in bulk
        Task<List<(ulong elevatorId, string status, ErrorCode? errorCode)>> GetElevatorStatuses();
    }
}