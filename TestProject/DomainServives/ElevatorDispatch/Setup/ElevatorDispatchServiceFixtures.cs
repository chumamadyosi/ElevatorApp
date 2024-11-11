using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DomainServives.ElevatorDispatch.Setup
{
    public class ElevatorDispatchServiceFixtures
    {
        public readonly IElevatorDispatchService _dispatchService;
        public readonly IElevatorMovementService _movementService;
        public readonly List<Elevator> _elevators;
        public readonly Elevator _elevator;

        public ElevatorDispatchServiceFixtures()
        {
            _movementService = Substitute.For<IElevatorMovementService>();
            _elevator = new Elevator { CurrentFloor = 5, Direction = Direction.Stationary, ElevatorType = ElevatorType.Passenger, MaxFloor = 10 };
            _elevators = new List<Elevator> { _elevator };
            _dispatchService = new ElevatorDispatchService(_elevators, _movementService);
        }

        public void ClearElevators()
        {
            // Clear the elevators list to simulate no available elevators
            _elevators.Clear();
        }

        public void AddElevator(Elevator elevator)
        {
            _elevators.Add(elevator); // Add an elevator to the list
        }
    }
}
