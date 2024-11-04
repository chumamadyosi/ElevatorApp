using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ElevatorControlService : IElevatorControlService
    {
        private readonly List<IElevator> _elevators;

        public int ElevatorCount => _elevators.Count;

        public ElevatorControlService(List<IElevator> elevators)
        {
            _elevators = elevators;
        }

        public IElevator GetNearestElevator(int requestedFloor)
        {
            return _elevators.OrderBy(e => Math.Abs(e.CurrentFloor - requestedFloor)).First();
        }

        public void MoveToFloor(IElevator elevator, int floor, int passengers)
        {
            elevator.MoveToFloor(floor);
            elevator.LoadPassengers(passengers);
        }

        public string GetElevatorStatusById(int elevatorId)
        {
            var elevator = _elevators.FirstOrDefault(e => e.Id == elevatorId);
            return elevator != null
                ? $"Elevator {elevator.Id}: Floor {elevator.CurrentFloor} | Passengers: {elevator.PassengerCount}"
                : "Elevator not found.";
        }
    }

}
