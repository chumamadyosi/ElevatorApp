using System;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public class ElevatorOccupantService : IElevatorOccupantService
    {
        // Method to check if it is safe to load passengers
        public async Task<ErrorCode?> LoadOccupants(Elevator elevator, int count)
        {
            if (count < 0)
                return ErrorCode.ExceedsCapacity; 

 
            if (elevator.PassengerCount + count > elevator.MaxPassengerCount)
                return ErrorCode.ExceedsPassengerCapacity;

            return null; 
        }

        // Method to add passengers after validating that it's safe
        public async Task<bool> AddOccupants(Elevator elevator, int count)
        {
            var validationResult = await LoadOccupants(elevator, count);

            if (validationResult != null)
                return false;

            elevator.PassengerCount += count;
            return true; 
        }
    }
}
