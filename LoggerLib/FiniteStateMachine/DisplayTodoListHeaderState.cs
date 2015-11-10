using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib
{
    public class DisplayTodoListHeaderState : State
    {
        public DisplayTodoListHeaderState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            SetNextState(typeof(ReadTodoState));

            RegisterState(typeof(DisplayTodoListHeaderState), this);
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

            mConsole.SetColour(ConsoleColor.DarkCyan);
            mConsole.OutputLine("Type to make an entry. Hit enter to return to logger");

            UInt32 entryIndex = 0;

            foreach (TodoEntry entry in mTodoList.GetEntries())
            {
                mConsole.SetColour(ConsoleColor.Magenta);
                mConsole.Output(entryIndex + "> ");
                mConsole.SetColour(ConsoleColor.Gray);
                mConsole.OutputLine(entry.Text);

                ++entryIndex;
            }

            mConsole.SetColour(ConsoleColor.Gray);
        }
    }
}
