using System;

namespace LoggerLib
{
    public class StoreTodoState : State
    {
        public StoreTodoState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            mNextState = new ReadTodoState(console, log, todoList);
        }

        public override void Execute()
        {
            mTodoList.AddEntry(new TodoEntry(Input));
        }
    }
}
