using System;

namespace LoggerLib
{
    public class ReadTodoState : State
    {
        public ReadTodoState(IConsole console, ILog log) : base(console, log)
        {
        }

        public override void Execute()
        {
            mConsole.SetColour(ConsoleColor.Magenta);
            mConsole.Output(">");
            mConsole.SetColour(ConsoleColor.Gray);

            string input = mConsole.GetInput();

            if (input.StartsWith(">"))
            {
                mNextState = new CommandState(mConsole, mLog);
                mNextState.Input = input.Remove(0, 1);
            }
            else if(!string.IsNullOrEmpty(input))
            {
                mNextState = new StoreState(mConsole, mLog);
                mNextState.Input = input;
            }
            else
            {
                mNextState = new DisplayLogHeaderState(mConsole, mLog);
            }
        }
    }
}
