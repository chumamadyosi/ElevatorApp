﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public interface IElevatorOccupantService
    {
        Task<ErrorCode?> LoadOccupants(Elevator elevator, int count); //count can be passagers or weight this means we round off weight(up for discussion and decimals might not matter that much)
        Task<bool> AddOccupants(Elevator elevator, int count);
    }
}
