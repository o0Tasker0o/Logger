using System;
using System.Linq;

namespace LoggerLib
{
    public class DisplayLogHeaderState : State
    {
        public DisplayLogHeaderState(IConsole console, ILog log) : base(console, log)
        {
            mNextState = new ReadState(console, log);
        }

        public override void Execute()
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
    }
}
