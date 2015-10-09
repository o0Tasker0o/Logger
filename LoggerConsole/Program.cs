using LoggerLib;
using System;
using System.Linq;

namespace LoggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                Console.Write("Enter a log entry: ");
                string entryText = Console.ReadLine();

                LogEntry entry = new LogEntry(entryText);
                loggerDatabase.LogEntries.Add(entry);
                loggerDatabase.SaveChanges();

                var logEntries = from logEntry in loggerDatabase.LogEntries
                                 orderby logEntry.Text
                                 select logEntry;

                Console.WriteLine("All entries in the database:");
                foreach (var item in logEntries)
                {
                    Console.WriteLine(item.Text);
                }

                Console.WriteLine("Press enter to exit...");
                Console.ReadKey();
            }
        }
    }
}
