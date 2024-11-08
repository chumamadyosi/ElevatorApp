namespace Domain
{
    public interface IElevatorStatusService
    {
        // Get status of a specific elevator by ID
        (string status, ErrorCode? errorCode) GetElevatorStatusById(int elevatorId);

        // Get statuses of all elevators in bulk
        Task<List<(int elevatorId, string status, ErrorCode? errorCode)>> GetElevatorStatuses();
    }
}