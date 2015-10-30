using LoggerLib;
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
        private ITodoList mTodoList;
        private IEnumerable<LogEntry> mSearchResults;

        private bool mRunning;

        public CommandRunner(IConsole console, ILog log, ITodoList todoList)
        {
            mSubCommands = new Dictionary<string, Tuple<string, Action>>();

            AddSubCommand("s", "Search log entries", SearchAndDisplayLogEntries);
            AddSubCommand("", "Search log entries", SearchAndDisplayLogEntries);
            AddSubCommand("rs", "Search previous results", SearchAndDisplayFilteredLogEntries);
            AddSubCommand("t", "Enter TODO list", EnterTodoList);
            AddSubCommand("?", "Display help", DisplayHelp);

            if(null == console)
            {
                throw new ArgumentNullException("Console must not be null");
            }

            if(null == log)
            {
                throw new ArgumentNullException("Log must not be null");
            }

            if(null == todoList)
            {
                throw new ArgumentNullException("TODO list must not be null");
            }

            mConsole = console;
            mLog = log;
            mTodoList = todoList;

            EnterLogger();

            mRunning = true;

            while (mRunning)
            {
                mConsole.SetColour(ConsoleColor.Green);
                mConsole.Output("> ");
                mConsole.SetColour(ConsoleColor.Gray);
                ExecuteCommand(mConsole.GetInput());
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
                mConsole.OutputLine("Unrecognised command. Please enter one of the following commands");
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
                mConsole.Output(">" + command + " - ");
                mConsole.OutputLine(mSubCommands[command].Item1);
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
                mConsole.Output(logEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
                mConsole.SetColour(ConsoleColor.Gray);
                mConsole.OutputLine(logEntry.Text);
            }

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.OutputLine("");
            mConsole.OutputLine("End of logs. Type to make another entry");
            mConsole.SetColour(ConsoleColor.Gray);
        }

        private IEnumerable<LogEntry> GetSearchResults(IEnumerable<LogEntry> entries)
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.Output("Please enter the term you wish to search for: ");

            mConsole.SetColour(ConsoleColor.Gray);
            String[] searchTerms = mConsole.GetInput().ToLower().Split(' ');

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
                mConsole.Output(instruction);
                mConsole.SetColour(ConsoleColor.Gray);
                inputDate = mConsole.GetInput();

                if(string.IsNullOrEmpty(inputDate))
                {
                    return defaultDate;
                }

            } while (!DateTime.TryParse(inputDate, out parsedDate));

            return parsedDate;
        }

        private void EnterLogger()
        {
            mConsole.Clear();

            mConsole.SetColour(ConsoleColor.Green);
            mConsole.OutputLine(@"                           __");
            mConsole.OutputLine(@"                          / /  ___  ___ ____ ____ ____");
            mConsole.OutputLine(@"                         / /__/ _ \/ _ `/ _ `/ -_) __/");
            mConsole.OutputLine(@"                        /____/\___/\_, /\_, /\__/_/");
            mConsole.OutputLine(@"                                  /___//___/");
            mConsole.OutputLine("");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            mConsole.OutputLine("Type to make an entry. Type '>?' for a list of commands or hit Enter to exit.");

            Console.ForegroundColor = ConsoleColor.Gray;

            var logEntries = from logEntry in mLog.GetEntries()
                             where logEntry.CreatedTime > DateTime.Now.Date
                             orderby logEntry.CreatedTime
                             select logEntry;

            foreach (LogEntry entry in logEntries)
            {
                mConsole.SetColour(ConsoleColor.Green);
                mConsole.Output("> ");
                mConsole.SetColour(ConsoleColor.Gray);
                mConsole.OutputLine(entry.Text);
            }
        }

        private void EnterTodoList()
        {
            mConsole.Clear();

            DisplayTodoList();

            bool runTodoList = true;

            while (runTodoList)
            {
                mConsole.SetColour(ConsoleColor.Magenta);
                mConsole.Output(">");
                mConsole.SetColour(ConsoleColor.Gray);
                String todoText = mConsole.GetInput();

                if (string.IsNullOrEmpty(todoText))
                {
                    runTodoList = false;
                }
                else if(todoText == ">r")
                {
                    string removalId = mConsole.GetInput();

                    try
                    {
                        mTodoList.RemoveEntry(UInt32.Parse(removalId));
                    }
                    catch(Exception)
                    {
                        mConsole.SetColour(ConsoleColor.Red);
                        mConsole.OutputLine("Incorrect number provided");
                        mConsole.SetColour(ConsoleColor.Gray);
                    }
                }
                else
                {
                    mTodoList.AddEntry(new TodoEntry(todoText));
                }
            }

            EnterLogger();
        }

        private void DisplayTodoList()
        {
            mConsole.SetColour(ConsoleColor.Magenta);
            mConsole.OutputLine(@"                    __________  ___  ____    __   _     __");
            mConsole.OutputLine(@"                   /_  __/ __ \/ _ \/ __ \  / /  (_)__ / /_");
            mConsole.OutputLine(@"                    / / / /_/ / // / /_/ / / /__/ (_-</ __/");
            mConsole.OutputLine(@"                   /_/  \____/____/\____/ /____/_/___/\__/");
            mConsole.OutputLine("");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            mConsole.OutputLine("Type to make an entry. Hit enter to return to logger");

            Console.ForegroundColor = ConsoleColor.Gray;

            UInt32 index = 0;

            foreach(TodoEntry entry in mTodoList.GetEntries())
            {
                mConsole.SetColour(ConsoleColor.Magenta);
                mConsole.Output(index + ">");
                mConsole.SetColour(ConsoleColor.Gray);
                mConsole.OutputLine(entry.Text);

                ++index;
            }
        }
    }
}
