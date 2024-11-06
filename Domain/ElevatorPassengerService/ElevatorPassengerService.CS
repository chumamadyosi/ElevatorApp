using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public class ElevatorPassengerService : IElevatorPassengerService
    {
        public async Task<ErrorCode?> LoadPassengersAsync(Elevator elevator, int count)
        {
            if (count < 0)
                return ErrorCode.ExceedsPassengerCapacity;

            if (elevator.PassengerCount + count > elevator.MaxPassengerCount)
                return ErrorCode.ExceedsPassengerCapacity;

            elevator.PassengerCount += count;
            return null; // Successfully loaded passengers
        }

        // Add passengers to the elevator (non-async version)
        public bool AddPassengers(Elevator elevator, int count)
        {
            if (elevator.PassengerCount + count <= elevator.MaxPassengerCount)
            {
                elevator.PassengerCount += count;
                return true; // Successfully added passengers
            }
            return false; // Exceeded capacity
        }

        // Event for floor change notifications
    }
}
