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

            while(true)
            {
                Console.Write("> ");
                string entryText = Console.ReadLine();

                if(String.IsNullOrEmpty(entryText))
                {
                    DisplayLogEntries();
                }
                else
                {
                    StoreLogEntry(entryText);
                }
            }
        }

        private static void StoreLogEntry(String entryText)
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                LogEntry entry = new LogEntry(entryText);
                loggerDatabase.LogEntries.Add(entry);
                loggerDatabase.SaveChanges();
            }
        }

        private static void DisplayLogEntries()
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                Console.Clear();
                Console.WriteLine("Please enter the term you wish to search for:");

                String searchTerm = Console.ReadLine();

                Console.WriteLine("Please enter the date to start searching from:");

                DateTime startDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Please enter the date to search up to:");

                DateTime endDate = DateTime.Parse(Console.ReadLine());

                var logEntries = from logEntry in loggerDatabase.LogEntries
                                 where logEntry.Text.Contains(searchTerm) &&
                                 logEntry.CreatedTime > startDate &&
                                 logEntry.CreatedTime < endDate
                                 orderby logEntry.CreatedTime
                                 select logEntry;

                foreach (var logEntry in logEntries)
                {
                    Console.WriteLine(logEntry.CreatedTime + "> " + logEntry.Text);
                }
            }

            Console.WriteLine();
            Console.WriteLine("End of logs. Type to make another entry");
        }
    }
}
