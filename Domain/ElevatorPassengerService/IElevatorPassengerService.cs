using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public interface IElevatorPassengerService
    {
        Task<ErrorCode?> LoadPassengersAsync(Elevator elevator, int count);
        bool AddPassengers(Elevator elevator, int count);
    }
}
