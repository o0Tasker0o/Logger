using System;
using System.Collections.Generic;

namespace LoggerLib
{
    public abstract class State
    {
        protected State mNextState;
        protected IConsole mConsole;
        protected ILog mLog;
        protected ITodoList mTodoList;

        private static Dictionary<Type, State> mStates;

        public String Input
        {
            get;
            set;
        }
        
        public State(IConsole console, ILog log, ITodoList todoList)
        {
            if (null == console)
            {
                throw new ArgumentNullException("Console must not be null");
            }

            if (null == log)
            {
                throw new ArgumentNullException("Log must not be null");
            }

            if (null == todoList)
            {
                throw new ArgumentNullException("TODO list must not be null");
            }

            mConsole = console;
            mLog = log;
            mTodoList = todoList;

            if(null == mStates)
            {
                mStates = new Dictionary<Type, State>();
            }
        }

        public State GetNextState()
        {
            return mNextState;
        }

        protected void RegisterState(Type stateType, State instance)
        {
            if (!mStates.ContainsKey(stateType))
            {
                mStates.Add(stateType, this);
            }
        }

        protected void SetNextState(Type stateType)
        {
            if (!mStates.ContainsKey(stateType))
            {
                mStates[stateType] = (State) Activator.CreateInstance(stateType, mConsole, mLog, mTodoList);
            }

            mNextState = mStates[stateType];
        }

        public abstract void Execute();
    }
}
