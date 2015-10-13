﻿using LoggerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerConsole
{
    public class CommandRunner
    {
        private IConsole mConsole;
        private ILog mLog;

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
                DisplayLogEntries();
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
        private void StoreLogEntry(String entryText)
        {
            mLog.AddEntry(new LogEntry(entryText));
        }

        private void DisplayLogEntries()
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.WriteLine("Please enter the term you wish to search for:");

            mConsole.SetColour(ConsoleColor.Gray);
            String searchTerm = mConsole.ReadLine();

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.WriteLine("Please enter the date to start searching from:");

            mConsole.SetColour(ConsoleColor.Gray);
            DateTime startDate = DateTime.Parse(mConsole.ReadLine());

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.WriteLine("Please enter the date to search up to:");

            mConsole.SetColour(ConsoleColor.Gray);
            DateTime endDate = DateTime.Parse(mConsole.ReadLine());

            var logEntries = from logEntry in mLog.GetEntries()
                                where logEntry.Text.Contains(searchTerm) &&
                                logEntry.CreatedTime > startDate &&
                                logEntry.CreatedTime < endDate
                                orderby logEntry.CreatedTime
                                select logEntry;

            foreach (var logEntry in logEntries)
            {
                mConsole.WriteLine(logEntry.ToString());
            }

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.WriteLine("");
            mConsole.WriteLine("End of logs. Type to make another entry");
            mConsole.SetColour(ConsoleColor.Gray);
        }
    }
}
