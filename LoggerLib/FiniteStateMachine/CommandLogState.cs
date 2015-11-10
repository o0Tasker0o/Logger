using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLib
{
    public class CommandLogState : CommandState
    {
        public CommandLogState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            AddSubCommand("s", "Search log entries", SearchEntries);
            AddSubCommand("", "Search log entries", SearchEntries);
            AddSubCommand("rs", "(UNAVAILABLE) Search previous results", null);
            AddSubCommand("t", "Enter TODO list", EnterTodoList);

            RegisterState(typeof(CommandLogState), this);
            SetNextState(typeof(ReadState));
        }

        private void EnterTodoList()
        {
            SetNextState(typeof(DisplayTodoListHeaderState));
        }

        private void SearchEntries()
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.Output("Please enter the term you wish to search for: ");
            mConsole.SetColour(ConsoleColor.Gray);

            string[] searchTerms = mConsole.GetInput().ToLower().Split(' ');
            DateTime startDate = GetDate("Please enter the date to start searching from: ", new DateTime(1, 1, 1));
            DateTime endDate = GetDate("Please enter the date to search up to: ", DateTime.Now);

            IEnumerable<LogEntry> searchedEntries = from logEntry in mLog.GetEntries()
                                                    where searchTerms.Any(logEntry.Text.ToLower().Contains) &&
                                                    logEntry.CreatedTime >= startDate &&
                                                    logEntry.CreatedTime <= endDate
                                                    orderby logEntry.CreatedTime
                                                    select logEntry;

            foreach(LogEntry entry in searchedEntries)
            {
                mConsole.SetColour(ConsoleColor.Green);
                mConsole.Output(entry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
                mConsole.SetColour(ConsoleColor.Gray);
                mConsole.OutputLine(entry.Text);
            }

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.OutputLine("");
            mConsole.OutputLine("End of logs. Type to make another entry");
            mConsole.SetColour(ConsoleColor.Gray);
            SetNextState(typeof(ReadState));
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

                if (string.IsNullOrEmpty(inputDate))
                {
                    return defaultDate;
                }

            } while (!DateTime.TryParse(inputDate, out parsedDate));

            return parsedDate;
        }

        protected override void DisplayHelp()
        {
            base.DisplayHelp();
            SetNextState(typeof(ReadState));
        }
    }
}
