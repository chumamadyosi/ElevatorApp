using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain
{
    public class PassengerElevator : IElevator
    {
        public Task<bool> AddOccupants(Elevator elevator, int count)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorCode?> LoadOccupants(Elevator elevator, int count)
        {
            throw new NotImplementedException();
        }
    }

}
