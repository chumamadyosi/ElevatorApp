using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Threading.Tasks;

namespace Domain
{
    public class FreightElevator : IElevator
    {
        // Method to add occupants (non-async version)
        public Task<bool> AddOccupants(Elevator elevator, int count)
        {
            if (count < 0)
            {
                // Negative count is invalid for loading occupants
                return Task.FromResult(false);
            }

            if (elevator.CurrentWeight + count <= elevator.MaxWeightCapacity)
            {
                elevator.CurrentWeight += count; // Successfully add cargo weight
                return Task.FromResult(true);
            }
            return Task.FromResult(false); // Exceeded weight capacity
        }

        // Async method to load occupants
        public async Task<ErrorCode?> LoadOccupants(Elevator elevator, int count)
        {
            if (count < 0)
            {
                return ErrorCode.ExceedsCapacity; // Handling negative count for invalid input
            }

            // For cargo (weight in kg)
            if (elevator.CurrentWeight + count > elevator.MaxWeightCapacity)
            {
                return ErrorCode.ExceedsWeightCapacity; // Exceeded weight capacity
            }

            elevator.CurrentWeight += count; // Add cargo weight
            return null; // Successfully loaded occupants
        }
    }
}
