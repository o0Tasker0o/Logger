using System;

namespace LoggerLib
{
    public class ReadTodoState : State
    {
        public ReadTodoState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            RegisterState(typeof(ReadTodoState), this);
        }

        public override void Execute()
        {
            mConsole.SetColour(ConsoleColor.Magenta);
            mConsole.Output(">");
            mConsole.SetColour(ConsoleColor.Gray);

            string input = mConsole.GetInput();

            if (input.StartsWith(">"))
            {
                SetNextState(typeof(CommandTodoState));
                mNextState.Input = input.Remove(0, 1);
            }
            else if(!string.IsNullOrEmpty(input))
            {
                SetNextState(typeof(StoreTodoState));
                mNextState.Input = input;
            }
            else
            {
                SetNextState(typeof(DisplayLogHeaderState));
            }
        }
    }
}
