using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ElevatorRequest
    {
        public int RequestedFloor { get; set; }
        public Direction RequestedDirection { get; set; }
        public ElevatorType ElevatorType { get; set; }
        public int PassengerCountOrLoadCapacity { get; set; }
        public ErrorCode? ErrorCode { get; set; }
    }
}
