﻿using LoggerLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerConsole
{
    public class CommandRunner
    {
        private readonly Dictionary<string, Tuple<string, Action>> mSubCommands;
        private IConsole mConsole;
        private ILog mLog;
        private IEnumerable<LogEntry> mSearchResults;

        private bool mRunning;

        public CommandRunner(IConsole console, ILog log)
        {
            mSubCommands = new Dictionary<string, Tuple<string, Action>>();

            AddSubCommand("s", "Search log entries", SearchAndDisplayLogEntries);
            AddSubCommand("", "Search log entries", SearchAndDisplayLogEntries);
            AddSubCommand("rs", "Search previous results", SearchAndDisplayFilteredLogEntries);
            AddSubCommand("?", "Display help", DisplayHelp);

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

        private void AddSubCommand(string commandString, string helpText, Action command)
        {
            mSubCommands.Add(commandString, new Tuple<string, Action>(helpText, command));
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
            try
            {
                mSubCommands[command].Item2();
            }
            catch(KeyNotFoundException)
            {
                mConsole.SetColour(ConsoleColor.Red);
                mConsole.WriteLine("Unrecognised command. Please enter one of the following commands");
                mConsole.SetColour(ConsoleColor.DarkCyan);
                DisplayHelp();
                mConsole.SetColour(ConsoleColor.Gray);
            }
        }

        private void DisplayHelp()
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);

            foreach(String command in mSubCommands.Keys)
            {
                mConsole.Write(">" + command + " - ");
                mConsole.WriteLine(mSubCommands[command].Item1);
            }

            mConsole.SetColour(ConsoleColor.Gray);
        }

        private void StoreLogEntry(String entryText)
        {
            mLog.AddEntry(new LogEntry(entryText));
        }

        private void SearchAndDisplayLogEntries()
        {
            mSearchResults = GetSearchResults(mLog.GetEntries());
            DisplaySearchedLogEntries();
        }

        private void SearchAndDisplayFilteredLogEntries()
        {
            mSearchResults = GetSearchResults(mSearchResults);
            DisplaySearchedLogEntries();
        }

        private void DisplaySearchedLogEntries()
        {
            foreach (var logEntry in mSearchResults)
            {
                mConsole.SetColour(ConsoleColor.Green);
                mConsole.Write(logEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
                mConsole.SetColour(ConsoleColor.Gray);
                mConsole.WriteLine(logEntry.Text);
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
            String[] searchTerms = mConsole.ReadLine().ToLower().Split(' ');

            DateTime startDate = GetDate("Please enter the date to start searching from: ", new DateTime(1, 1, 1));
            DateTime endDate = GetDate("Please enter the date to search up to: ", DateTime.Now);

            return from logEntry in entries
                        where searchTerms.Any(logEntry.Text.ToLower().Contains) &&
                        logEntry.CreatedTime >= startDate &&
                        logEntry.CreatedTime <= endDate
                        orderby logEntry.CreatedTime
                        select logEntry;
        }

        private DateTime GetDate(string instruction, DateTime defaultDate)
        {
            DateTime parsedDate;
            string inputDate;

            do
            {
                mConsole.SetColour(ConsoleColor.DarkCyan);
                mConsole.Write(instruction);
                mConsole.SetColour(ConsoleColor.Gray);
                inputDate = mConsole.ReadLine();

                if(string.IsNullOrEmpty(inputDate))
                {
                    return defaultDate;
                }

            } while (!DateTime.TryParse(inputDate, out parsedDate));

            return parsedDate;
        }
    }
}
