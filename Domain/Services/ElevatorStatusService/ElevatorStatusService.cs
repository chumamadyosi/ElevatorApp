using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class ElevatorStatusService : IElevatorStatusService
    {
        private readonly List<Elevator> _elevators;

        public ElevatorStatusService(List<Elevator> elevators)
        {
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators), "Elevator list cannot be null.");
        }

        // Get elevator status by its ID
        public async Task<(string status, ErrorCode? errorCode)> GetElevatorStatusById(ulong elevatorId)
        {
            var elevator = await Task.FromResult(_elevators.FirstOrDefault(e => e.Id == elevatorId));

            if (elevator == null)
                return ("", ErrorCode.ElevatorNotFound);

            var status = FormatElevatorStatus(elevator);
            return (status, null);
        }

        // Get statuses of all elevators in bulk
        public async Task<List<(ulong elevatorId, string status, ErrorCode? errorCode)>> GetElevatorStatuses()
        {
            return await Task.Run(() =>
                _elevators.Select(elevator =>
                    (elevator.Id, FormatElevatorStatus(elevator), (ErrorCode?)null)).ToList());
        }

        private string FormatElevatorStatus(Elevator elevator)
        {
            return $"Elevator {elevator.Id}: Floor {elevator.CurrentFloor} | Passengers: {elevator.PassengerCount} | Direction: {elevator.Direction}";
        }
    }
}
