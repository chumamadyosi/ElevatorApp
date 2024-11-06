﻿using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorDispatch
{
    public class ElevatorDispatchService : IElevatorDispatchService
    {
        private readonly List<Elevator> _elevators;
        private readonly IElevatorMovementService _elevatorMovementService;
        private readonly IElevatorOccupantService _elevatorPassengerService;

        public ElevatorDispatchService(List<Elevator> elevators,
        IElevatorMovementService elevatorMovementService,
        IElevatorOccupantService elevatorPassengerService)
        {
            _elevators = elevators ?? throw new ArgumentNullException(nameof(elevators));
            _elevatorMovementService = elevatorMovementService ?? throw new ArgumentNullException(nameof(elevatorMovementService));
            _elevatorPassengerService = elevatorPassengerService ?? throw new ArgumentNullException(nameof(elevatorPassengerService));
        }

        public (Elevator? elevator, ErrorCode? errorCode) GetNearestElevator(int requestedFloor, Direction requestedDirection, ElevatorType elevatorType)
        {
            if (!_elevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            var candidateElevators = _elevators
                .Where(e => e.Type == elevatorType &&
                            e.Direction == Direction.Stationary ||
                            e.Direction == requestedDirection &&
                             (requestedDirection == Direction.Up && e.CurrentFloor <= requestedFloor ||
                              requestedDirection == Direction.Down && e.CurrentFloor >= requestedFloor))
                .ToList();

            if (!candidateElevators.Any())
                return (null, ErrorCode.NoAvailableElevators);

            var nearestElevator = candidateElevators
                .OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor))
                .FirstOrDefault();

            return (nearestElevator, null);
        }

        // Request elevator to a specific floor and load passengers
        public async Task<ErrorCode?> RequestElevatorToFloor(Elevator elevator, int requestedFloor, ElevatorType elevatorType)
        {
            if (elevator == null)
                return ErrorCode.NullElevator;

            // Validate the requested floor
            if (requestedFloor < 1 || requestedFloor > elevator.MaxFloor)
                return ErrorCode.FloorOutOfRange;

            // Check if the elevator's type matches the requested type
            if (elevator.Type != elevatorType)
                return ErrorCode.InvalidElevatorType;

            // If the elevator is already at the requested floor, no need to move
            if (elevator.CurrentFloor == requestedFloor)
            {
                return null; // No need to move, just return
            }

            // Move the elevator to the requested floor
            var moveError = await _elevatorMovementService.MoveElevatorToDestinationFloor(elevator, requestedFloor);
            if (moveError.HasValue)
                return moveError;

            return null; // Successfully requested the elevator to move
        }



    }
}
