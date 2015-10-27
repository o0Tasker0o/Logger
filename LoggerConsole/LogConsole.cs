using System;

namespace LoggerConsole
{
    public class LogConsole : IConsole
    {
        public String ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(String text)
        {
            Console.WriteLine(text);
        }

        public void Write(String text)
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
