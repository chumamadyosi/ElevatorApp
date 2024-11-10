using Domain;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public class ErrorHandler
    {
        private readonly ILogger _logger;

        public ErrorHandler(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void HandleError(ErrorCode errorCode)
        {
            string errorMessage = errorCode switch
            {
                ErrorCode.ElevatorNotFound => "The requested elevator was not found.",
                ErrorCode.FloorOutOfRange => "The requested floor is out of range.",
                ErrorCode.ExceedsPassengerCapacity => "The number of passengers exceeds the maximum capacity.",
                ErrorCode.NullElevator => "The elevator cannot be null.",
                ErrorCode.NoAvailableElevators => "There are no available elevators.",
                ErrorCode.PassengerCountExceeded => "Passenger count exceeded the limit.",
                ErrorCode.InvalidElevatorType => "The elevator type is invalid.",
                ErrorCode.ElevatorAccessDenied => "Elevator access is denied.",
                ErrorCode.ExceedsWeightCapacity => "The weight capacity has been exceeded.",
                ErrorCode.ExceedsCapacity => "The elevator exceeds its capacity.",
                _ => "An unknown error occurred."
            };

            // Log the error message and error code using the injected logger
            _logger.LogError(errorMessage, errorCode);
        }
    }
}
