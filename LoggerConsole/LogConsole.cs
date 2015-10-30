using LoggerLib;
using System;

namespace LoggerConsole
{
    public class LogConsole : IConsole
    {
        public String GetInput()
        {
            return Console.ReadLine();
        }

        public void OutputLine(String text)
        {
            Console.WriteLine(text);
        }

        public void Output(String text)
        {
            Console.Write(text);
        }

        public void SetColour(ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
        }

        public void Clear()
        {
            Console.Clear();
        }
    }
}
