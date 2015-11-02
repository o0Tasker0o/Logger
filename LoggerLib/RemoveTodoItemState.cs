using System;

namespace LoggerLib
{
    public class RemoveTodoItemState : State
    {
        public RemoveTodoItemState(IConsole console, ILog log, ITodoList todoList) : base (console, log, todoList)
        {
            mNextState = new ReadTodoState(console, log, todoList);
        }

        public override void Execute()
        {
            UInt32 todoItemId = 0;

            do
            {
                mConsole.SetColour(ConsoleColor.DarkCyan);
                mConsole.Output("Enter the item ID to remove");
                mConsole.SetColour(ConsoleColor.Gray);
            } while(!UInt32.TryParse(mConsole.GetInput(), out todoItemId));

            try
            {
                mTodoList.RemoveEntry(todoItemId);
            }
            catch(ArgumentOutOfRangeException)
            {
                mConsole.SetColour(ConsoleColor.Red);
                mConsole.OutputLine("Item with given ID not found");
                mConsole.SetColour(ConsoleColor.Gray);
            }
        }
    }
}
