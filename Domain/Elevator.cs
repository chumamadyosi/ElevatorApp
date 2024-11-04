using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class Elevator : IElevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        public int PassengerCount { get; private set; }

        public Elevator(int id)
        {
            Id = id;
            CurrentFloor = 1;
            PassengerCount = 0;
        }

        public void MoveToFloor(int floor)
        {
            CurrentFloor = floor;
        }

        public void LoadPassengers(int count)
        {
            PassengerCount = count;
        }

    }
}
