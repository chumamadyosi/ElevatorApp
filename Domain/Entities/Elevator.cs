using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Elevator
    {
        public ulong Id { get; set; }
        public int CurrentFloor { get; set; } = 1;
        public Direction Direction { get; set; } = Direction.Stationary;
        public int MaxFloor { get; set; }
        public int MaxPassengerCount { get; set; }
        public int MaxWeightCapacity { get; set; }
        public int SpeedInMillisecondsPerFloor { get; set; }  // Speed of the elevator
        public ElevatorType ElevatorType { get; set; }  // New property to distinguish elevator types



        public int PassengerCount { get; set; } = 0;
        public int CurrentWeight { get; set; } = 0;
    }
}
