using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorConsole
{
    public static class ErrorHandler
    {
        public static void HandleError(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.ElevatorNotFound:
                    Console.WriteLine("Error: The requested elevator was not found.");
                    break;
                case ErrorCode.FloorOutOfRange:
                    Console.WriteLine("Error: The requested floor is out of range.");
                    break;
                case ErrorCode.ExceedsPassengerCapacity:
                    Console.WriteLine("Error: The number of passengers exceeds the maximum capacity.");
                    break;
                case ErrorCode.NullElevator:
                    Console.WriteLine("Error: The elevator cannot be null.");
                    break;
                default:
                    Console.WriteLine("An unknown error occurred.");
                    break;
            }
        }
    }
}
