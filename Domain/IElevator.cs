using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IElevator
    {
        int Id { get; }
        int CurrentFloor { get; }
        int PassengerCount { get; }

        // Add MaxPassengerCount and MaxFloor properties
        int MaxPassengerCount { get; }
        int MaxFloor { get; }
        Direction Direction { get; }

        ErrorCode? MoveToFloor(int floor);
        ErrorCode? LoadPassengers(int count);
        bool AddPassengers(int count);
    }
}
