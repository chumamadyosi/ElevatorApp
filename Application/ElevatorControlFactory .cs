
using Domain;
using Domain.ElevatorAccessControlService;
using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ElevatorControlFactory : IElevatorControlFactory
    {

        private readonly IElevatorOccupantService _occupantService;


        public ElevatorControlFactory(IElevatorOccupantService occupantService)
        {
            _occupantService = occupantService;
      
        }
        public IElevator CreateElevatorControlService(ElevatorType elevatorType)
        {
            switch (elevatorType)
            {
                case ElevatorType.Passenger:
                    return new PassengerElevator();
                case ElevatorType.Freight:
                    return new FreightElevator();
                case ElevatorType.Glass:
                    return new GlassElevator(_occupantService);
                default:
                    throw new ArgumentException("Invalid elevator type");
            }
        }
    }
}


