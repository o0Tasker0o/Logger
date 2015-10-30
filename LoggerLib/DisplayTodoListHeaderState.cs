using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib
{
    public class DisplayTodoListHeaderState : State
    {
        public DisplayTodoListHeaderState(IConsole console, ILog log) : base(console, log)
        {
            mNextState = new ReadTodoState(console, log);
        }

        public override void Execute()
        {
            mConsole.Clear();

            mConsole.SetColour(ConsoleColor.Magenta);
            mConsole.OutputLine(@"                    __________  ___  ____    __   _     __");
            mConsole.OutputLine(@"                   /_  __/ __ \/ _ \/ __ \  / /  (_)__ / /_");
            mConsole.OutputLine(@"                    / / / /_/ / // / /_/ / / /__/ (_-</ __/");
            mConsole.OutputLine(@"                   /_/  \____/____/\____/ /____/_/___/\__/");
            mConsole.OutputLine("");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            mConsole.OutputLine("Type to make an entry. Hit enter to return to logger");

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
