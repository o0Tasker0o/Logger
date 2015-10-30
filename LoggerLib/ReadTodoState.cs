using System;

namespace LoggerLib
{
    public class ReadTodoState : State
    {
        public ReadTodoState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
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
                mNextState = new CommandState(mConsole, mLog, mTodoList);
                mNextState.Input = input.Remove(0, 1);
            }
            else if(!string.IsNullOrEmpty(input))
            {
                mNextState = new StoreTodoState(mConsole, mLog, mTodoList);
                mNextState.Input = input;
            }
            else
            {
                mNextState = new DisplayLogHeaderState(mConsole, mLog, mTodoList);
            }
        }
    }
}
