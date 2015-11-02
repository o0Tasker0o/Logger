using System;

namespace LoggerLib
{
    public class StoreState : State
    {
        public StoreState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            mNextState = new ReadState(console, log, mTodoList);
        }

        public override void Execute()
        {
            mLog.AddEntry(new LogEntry(Input));
        }
    }
}
