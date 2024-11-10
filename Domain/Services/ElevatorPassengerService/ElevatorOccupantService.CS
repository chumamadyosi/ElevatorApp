using System;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public class ElevatorOccupantService : IElevatorOccupantService
    {
        // Method to load passengers asynchronously
        public async Task<ErrorCode?> LoadOccupants(Elevator elevator, int count)
        {
            if (count < 0)
                return ErrorCode.ExceedsCapacity; // Handle negative count for invalid input

            // Check if adding passengers exceeds the maximum allowed passenger count
            if (elevator.PassengerCount + count > elevator.MaxPassengerCount)
                return ErrorCode.ExceedsPassengerCapacity;

            elevator.PassengerCount += count; // Add passengers to elevator
            return null; // Successfully loaded passengers
        }

        // Non-async method to add passengers
        public async Task<bool> AddOccupants(Elevator elevator, int count)
        {
            if (count < 0)
                return false; // Negative count is invalid

            // Check if adding passengers exceeds the maximum allowed passenger count
            if (elevator.PassengerCount + count <= elevator.MaxPassengerCount)
            {
                elevator.PassengerCount += count;
                return true; // Successfully added passengers
            }
            return false; // Exceeded passenger capacity
        }


    }
}
