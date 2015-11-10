using System;

namespace LoggerLib
{
    public class StoreState : State
    {
        public StoreState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            SetNextState(typeof(ReadState));

            RegisterState(typeof(StoreState), this);
        }

        public override void Execute()
        {
            mLog.AddEntry(new LogEntry(Input));
        }
    }
}
