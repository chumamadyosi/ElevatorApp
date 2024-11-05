using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ElevatorSettings
    {
        public int ElevatorCount { get; set; }
        public int DefaultMaxPassengerCount { get; set; }
        public int DefaultMaxFloor { get; set; }
        public List<ElevatorConfig> Elevators { get; set; }
    }
}
