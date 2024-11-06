using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public class ElevatorOccupantService : IElevatorOccupantService
    {
        // Load occupants, based on whether they are passengers or cargo (weight)
        public async Task<ErrorCode?> LoadOccupantsAsync(Elevator elevator, int count, OccupantType occupantType)
        {
            if (count < 0)
                return ErrorCode.ExceedsCapacity;  // Handling negative count for invalid input.

            switch (occupantType)
            {
                case OccupantType.Passenger:
                    // For passengers
                    if (elevator.PassengerCount + count > elevator.MaxPassengerCount)
                        return ErrorCode.ExceedsPassengerCapacity;

                    elevator.PassengerCount += count; // Add passengers
                    break;

                case OccupantType.Cargo:
                    // For cargo (weight in kg)
                    if (elevator.TotalWeight + count > elevator.MaxWeightCapacity)
                        return ErrorCode.ExceedsWeightCapacity;

                    elevator.TotalWeight += count; // Add cargo weight
                    break;
            }

            return null; // Successfully loaded passengers or cargo
        }

        // Add occupants (non-async version)
        public bool AddOccupants(Elevator elevator, int count, OccupantType occupantType)
        {
            switch (occupantType)
            {
                case OccupantType.Passenger:
                    // For passengers
                    if (elevator.PassengerCount + count <= elevator.MaxPassengerCount)
                    {
                        elevator.PassengerCount += count;
                        return true; // Successfully added passengers
                    }
                    return false; // Exceeded passenger capacity

                case OccupantType.Cargo:
                    // For cargo (weight in kg)
                    if (elevator.TotalWeight + count <= elevator.MaxWeightCapacity)
                    {
                        elevator.TotalWeight += count;
                        return true; // Successfully added cargo
                    }
                    return false; // Exceeded weight capacity
            }

            return false; // Default case if no valid occupant type
        }

        // Event for floor change notifications
    }

}
