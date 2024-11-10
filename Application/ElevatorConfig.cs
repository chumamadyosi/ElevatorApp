using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ElevatorConfig
    {
        public ulong Id { get; set; }
        public int MaxFloor { get; set; }
        public int MaxPassengerCount { get; set; }
        public int MaxWeightCapacity { get; set; }
        public int SpeedInMillisecondsPerFloor { get; set; }
        public ElevatorType ElevatorType { get; set; }
    }
}
