using System;

namespace LoggerLib
{
    public class ReadState : State
    {
        public ReadState(IConsole console, ILog log) : base(console, log)
        {
        }

        public override void Execute()
        {
            mConsole.SetColour(ConsoleColor.Green);
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
                mNextState = null;
            }
        }
    }
}
