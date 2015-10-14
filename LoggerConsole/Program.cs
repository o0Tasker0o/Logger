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

            Log log = new Log();
            LogConsole console = new LogConsole();

            var logEntries = from logEntry in log.GetEntries()
                             where logEntry.CreatedTime > DateTime.Now.Date
                             orderby logEntry.CreatedTime
                             select logEntry;

            foreach(LogEntry entry in logEntries)
            {
                console.SetColour(ConsoleColor.Green);
                console.Write("> ");
                console.SetColour(ConsoleColor.Gray);
                console.WriteLine(entry.Text);
            }

            CommandRunner runner = new CommandRunner(console, log);
        }

    }
}
