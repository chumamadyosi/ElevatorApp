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
        void MoveToFloor(int floor);
        void LoadPassengers(int count);
    }
}
