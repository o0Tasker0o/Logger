using System;

namespace LoggerLib
{
    public class StoreState : State
    {
        public StoreState(IConsole console, ILog log) : base(console, log)
        {
            mNextState = new ReadState(console, log);
        }

        public override void Execute()
        {
            mLog.AddEntry(new LogEntry(Input));
        }
    }
}
