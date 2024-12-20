﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorDispatch
{
    public interface IDispatchElevatorWithUserAccess
    {
        Task<(Elevator? elevator, ErrorCode? errorCode)> GetNearestElevatorWithUserAccess(int requestedFloor, Direction requestedDirection, Guid userId, ElevatorType elevatorType = ElevatorType.Passenger);
    }
}
