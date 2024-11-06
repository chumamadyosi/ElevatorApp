using Domain.Elevator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ElevatorFactory
    {
        // Factory method to create appropriate elevator based on ElevatorType
        public static Elevator CreateElevator(ElevatorType elevatorType)
        {
            switch (elevatorType)
            {
                case ElevatorType.Passenger:
                    return new Domain.Elevator.PassengerElevator();
                case ElevatorType.Freight:
                    return new Domain.Elevator.FreightElevator();
                default:
                    throw new ArgumentException("Invalid elevator type");
            }
        }
    }
}


