
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
    public class ElevatorFactory
    {
        private readonly ElevatorDispatchService _dispatchService;
        private readonly IElevatorMovementService _movementService;
        private readonly IElevatorOccupantService _occupantService;
        private readonly IElevatorAccessControlService _accessControlService;

        public ElevatorFactory(ElevatorDispatchService dispatchService, IElevatorMovementService movementService, IElevatorOccupantService occupantService, IElevatorAccessControlService accessControlService)
        {
            _dispatchService = dispatchService;
            _movementService = movementService;
            _occupantService = occupantService;
            _accessControlService = accessControlService;
        }
        public IElevator CreateElevator(ElevatorType elevatorType)
        {
            switch (elevatorType)
            {
                case ElevatorType.Passenger:
                    return new PassengerElevator(_dispatchService, _movementService, _occupantService);
                case ElevatorType.Freight:
                    return new FreightElevator(_dispatchService, _movementService, _occupantService);
                case ElevatorType.Glass:
                    return new GlassElevator(_dispatchService, _movementService, _occupantService, _accessControlService);
                default:
                    throw new ArgumentException("Invalid elevator type");
            }
        }
    }
}


