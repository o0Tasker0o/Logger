using System;

namespace LoggerLib
{
    public abstract class State
    {
        protected State mNextState;
        protected IConsole mConsole;
        protected ILog mLog;

        public String Input
        {
            get;
            set;
        }
        
        public State(IConsole console, ILog log)
        {
            if (null == console)
            {
                throw new ArgumentNullException("Console must not be null");
            }

            if (null == log)
            {
                throw new ArgumentNullException("Log must not be null");
            }

            mConsole = console;
            mLog = log;
        }

        public State GetNextState()
        {
            return mNextState;
        }

        public abstract void Execute();
    }
}
