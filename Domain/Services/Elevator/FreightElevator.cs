using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Threading.Tasks;

namespace Domain
{
    public class FreightElevator : IElevator
    {
        // Method to add occupants to elevator
        public async Task<bool> AddOccupants(Elevator elevator, int count)
        {
            if (count < 0)
            {
                return false;
            }

            if (elevator.CurrentWeight + count <= elevator.MaxWeightCapacity)
            {
                elevator.CurrentWeight += count; // Successfully add cargo weight
                return true;
            }
            return false; // Exceeded weight capacity
        }

        // Async method to load occupants// For cargo (weight in kg)
        public async Task<ErrorCode?> LoadOccupants(Elevator elevator, int count)
        {
            if (count < 0)
            {
                return ErrorCode.ExceedsCapacity; 
            }
            
            if (elevator.CurrentWeight + count > elevator.MaxWeightCapacity)
            {
                return ErrorCode.ExceedsWeightCapacity; 
            }
            return null; 
        }
    }
}
