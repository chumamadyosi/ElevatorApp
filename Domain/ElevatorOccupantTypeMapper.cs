using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class ElevatorOccupantTypeMapper
    {
        public static OccupantType GetOccupantTypeForElevator(ElevatorType elevatorType)
        {
            return elevatorType switch
            {
                ElevatorType.Passenger => OccupantType.Passenger,
                ElevatorType.Freight => OccupantType.Cargo,
                ElevatorType.Glass => OccupantType.Passenger, // Assuming glass elevators are for passengers
                _ => throw new ArgumentException($"Unsupported ElevatorType: {elevatorType}")
            };
        }
    }
}
