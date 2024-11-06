using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Elevator
{
    public interface IElevator : IElevatorDispatchService, IElevatorMovementService, IElevatorOccupantService
    {
        // You could add common elevator-related properties here if needed.
    }
}
