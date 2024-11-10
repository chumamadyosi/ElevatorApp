using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IElevatorService : IElevatorDispatchService, IElevatorMovementService, IElevatorStatusService
    {
        // You could add common elevator-related properties here if needed.
    }
}
