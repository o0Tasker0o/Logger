using System;

namespace LoggerLib
{
    public class ReadState : State
    {
        public ReadState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
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
                mNextState = new CommandState(mConsole, mLog, mTodoList);
                mNextState.Input = input.Remove(0, 1);
            }
            else if(!string.IsNullOrEmpty(input))
            {
                mNextState = new StoreState(mConsole, mLog, mTodoList);
                mNextState.Input = input;
            }
            else
            {
                mNextState = null;
            }
        }
    }
}
