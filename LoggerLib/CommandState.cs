using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLib
{
    public class CommandState : State
    {
        public CommandState(IConsole console, ILog log) : base(console, log)
        {
            mNextState = new ReadState(console, log);
        }

        public override void Execute()
        {
            switch(Input)
            {
                case "s":
                case "":
                    SearchEntries();
                    break;
                case "t":
                    mNextState = new DisplayTodoListHeaderState(mConsole, mLog);
                    break;
                default:
                    DisplayHelp();
                    break;
            }
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
        }

        private void DisplayHelp()
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.OutputLine("Unrecognised command. Please enter one of the following commands");
            mConsole.OutputLine("s\t- Search log entries");
            mConsole.OutputLine("\t- Search log entries");
            mConsole.OutputLine("rs\t- (UNAVAILABLE) Search previous results");
            mConsole.OutputLine("t\t- Enter TODO list");
            mConsole.OutputLine("?\t- (UNAVAILABLE) Display help");
            mConsole.SetColour(ConsoleColor.Gray);
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
    }
}
