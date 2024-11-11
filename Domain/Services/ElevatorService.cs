using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ElevatorService : IElevatorService
    {
        private readonly IElevatorDispatchService _elevatorDispatchService;
        private readonly IElevatorMovementService _elevatorMovementService;
        private readonly IElevatorStatusService _elevatorStatusService;

        public ElevatorService(
       IElevatorDispatchService elevatorDispatchService,
       IElevatorMovementService elevatorMovementService,
       IElevatorStatusService elevatorStatusService) // <-- Use the interface here
        {
            _elevatorDispatchService = elevatorDispatchService;
            _elevatorMovementService = elevatorMovementService;
            _elevatorStatusService = elevatorStatusService;
        }
        public async Task<(string status, ErrorCode? errorCode)> GetElevatorStatusById(ulong elevatorId)
        {
            return await _elevatorStatusService.GetElevatorStatusById(elevatorId);
        }

        public async Task<List<(ulong elevatorId, string status, ErrorCode? errorCode)>> GetElevatorStatuses()
        {
            return await _elevatorStatusService.GetElevatorStatuses();
        }

        public async Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType)
        {
            return await _elevatorDispatchService.GetNearestElevator(requestedFloor, requestedDirection, elevatorType);
        }

        public async Task<ErrorCode?> MoveElevatorToDestinationFloor(Elevator elevator, int destinationFloor)
        {
            return await _elevatorMovementService.MoveElevatorToDestinationFloor(elevator, destinationFloor);
        }

        public async Task<ErrorCode?> MoveToFloorAsync(Elevator elevator, int floor)
        {
            return await _elevatorMovementService.MoveToFloorAsync(elevator, floor);
        }

        public async Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor)
        {
            return await _elevatorDispatchService.RequestElevatorToFloor(elevator, requestedFloor);
        }
    }
}
