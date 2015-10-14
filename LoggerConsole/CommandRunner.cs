using LoggerLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerConsole
{
    public class CommandRunner
    {
        private IConsole mConsole;
        private ILog mLog;
        private IEnumerable<LogEntry> mSearchResults;

        private bool mRunning;

        public CommandRunner(IConsole console, ILog log)
        {
            if(null == console)
            {
                throw new ArgumentNullException("Console must not be null");
            }

            if(null == log)
            {
                throw new ArgumentNullException("Log must not be null");
            }

            mConsole = console;
            mLog = log;
            mRunning = true;

            while (mRunning)
            {
                mConsole.SetColour(ConsoleColor.Green);
                mConsole.Write("> ");
                mConsole.SetColour(ConsoleColor.Gray);
                ExecuteCommand(mConsole.ReadLine());
            }
        }

        private void ExecuteCommand(string command)
        {
            if(command.StartsWith(">"))
            {
                ExecuteSubCommand(command.Replace(">", ""));
            }
            else if(string.IsNullOrEmpty(command))
            {
                mRunning = false;
            }
            else
            {
                StoreLogEntry(command);
            }
        }

        private void ExecuteSubCommand(string command)
        {
            switch(command)
            {
                case "rs":
                    SearchAndDisplayFilteredLogEntries();
                    break;
                default:
                    SearchAndDisplayLogEntries();
                    break;
            }
        }

        private void StoreLogEntry(String entryText)
        {
            mLog.AddEntry(new LogEntry(entryText));
        }

        private void SearchAndDisplayLogEntries()
        {
            mSearchResults = GetSearchResults(mLog.GetEntries());
            DisplayMyLogEntries();
        }

        private void SearchAndDisplayFilteredLogEntries()
        {
            mSearchResults = GetSearchResults(mSearchResults);
            DisplayMyLogEntries();
        }

        private void DisplayMyLogEntries()
        {
            foreach (var logEntry in mSearchResults)
            {
                mConsole.WriteLine(logEntry.ToString());
            }

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.WriteLine("");
            mConsole.WriteLine("End of logs. Type to make another entry");
            mConsole.SetColour(ConsoleColor.Gray);
        }

        private IEnumerable<LogEntry> GetSearchResults(IEnumerable<LogEntry> entries)
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.Write("Please enter the term you wish to search for: ");

            mConsole.SetColour(ConsoleColor.Gray);
            String[] searchTerms = mConsole.ReadLine().Split(' ');

            DateTime startDate = GetDate("Please enter the date to start searching from: ");
            DateTime endDate = GetDate("Please enter the date to search up to: ");

            return from logEntry in entries
                        where searchTerms.Any(logEntry.Text.Contains) &&
                        logEntry.CreatedTime > startDate &&
                        logEntry.CreatedTime < endDate
                        orderby logEntry.CreatedTime
                        select logEntry;
        }

        private DateTime GetDate(string instruction)
        {
            DateTime parsedDate;

            do
            {
                mConsole.SetColour(ConsoleColor.DarkCyan);
                mConsole.Write(instruction);
                mConsole.SetColour(ConsoleColor.Gray);
            } while (!DateTime.TryParse(mConsole.ReadLine(), out parsedDate));

            return parsedDate;
        }
    }
}
