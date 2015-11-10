using System;

namespace LoggerLib
{
    public class StoreTodoState : State
    {
        public StoreTodoState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            SetNextState(typeof(ReadTodoState));

            RegisterState(typeof(StoreTodoState), this);
        }

        public override void Execute()
        {
            mTodoList.AddEntry(new TodoEntry(Input));
        }
    }
}
