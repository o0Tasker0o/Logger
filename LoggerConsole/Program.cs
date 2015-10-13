using LoggerLib;
using System;
using System.Linq;

namespace LoggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Daily Log");
            Console.WriteLine("Type to make an entry. Hit Enter for options");

            CommandRunner runner = new CommandRunner(new LogConsole(), new Log());
        }

    }
}
