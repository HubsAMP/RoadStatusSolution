using RoadStatus.Service.Interfaces;
using System;

namespace RoadStatus.Service
{
    public class ConsoleWrapper : IConsoleWrapper
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}