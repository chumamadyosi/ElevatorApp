﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public enum ErrorCode
    {
        None = 0,
        ElevatorNotFound = 1001,
        FloorOutOfRange = 1002,
        ExceedsPassengerCapacity = 1003,
        NullElevator = 1004,
        NoAvailableElevators = 1005,
        PassengerCountExceeded = 1006
        // Add other error codes as needed
    }
}