using LoggerLib;
using System;
using System.IO;
using System.Linq;

namespace LoggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth, 60);
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"                           __");
            Console.WriteLine(@"                          / /  ___  ___ ____ ____ ____");
            Console.WriteLine(@"                         / /__/ _ \/ _ `/ _ `/ -_) __/");
            Console.WriteLine(@"                        /____/\___/\_, /\_, /\__/_/");
            Console.WriteLine(@"                                  /___//___/");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Type to make an entry. Type '>?' for a list of commands or hit Enter to exit.");

            Console.ForegroundColor = ConsoleColor.Gray;

            Log log = new Log();
            TodoList todoList = new TodoList();
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

            CommandRunner runner = new CommandRunner(console, log, todoList);
        }

    }
}
