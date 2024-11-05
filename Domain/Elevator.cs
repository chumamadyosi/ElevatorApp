using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{

    public class Elevator : IElevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        public int PassengerCount { get; private set; }
        public int MaxPassengerCount { get; set; }
        public int MaxFloor { get; set; }
        public Direction Direction { get; private set; }

        public Elevator(int id, int initialFloor = 1, int maxFloor = 10, int maxPassengerCount = 5)
        {
            Id = id;
            CurrentFloor = initialFloor;
            MaxFloor = maxFloor; // Set from configuration
            MaxPassengerCount = maxPassengerCount; // Set from configuration
            PassengerCount = 0; // Initialize passenger count
        }
        public ErrorCode? MoveToFloor(int floor)
        {
            if (floor < 1 || floor > MaxFloor)
            {
                return ErrorCode.FloorOutOfRange;
            }

            CurrentFloor = floor;
            PassengerCount = 0; // Reset passenger count on floor change
            return null;
        }
        public ErrorCode? LoadPassengers(int count)
        {
            if (count > MaxPassengerCount)
            {
                return ErrorCode.ExceedsPassengerCapacity;
            }

            PassengerCount = count;
            return null;
        }
        public bool AddPassengers(int count)
        {
            if (PassengerCount + count > MaxPassengerCount)
            {
                return false; // Cannot exceed max passenger count
            }

            PassengerCount += count;
            return true;
        }
    }

}

