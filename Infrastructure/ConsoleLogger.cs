using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message, ErrorCode errorCode)
        {
            Console.WriteLine($"Error Code: {errorCode} - {message}");
        }
    }
}
