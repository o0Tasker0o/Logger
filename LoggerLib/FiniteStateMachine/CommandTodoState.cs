using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLib
{
    public class CommandTodoState : State
    {
        private Dictionary<string, Tuple<string, Action>> mCommands;

        public CommandTodoState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            mCommands = new Dictionary<string, Tuple<string, Action>>();

            AddSubCommand("r", "Remove todo list entry", DeleteEntry);
            AddSubCommand("?", "Display help", DisplayHelp);

            mNextState = new ReadTodoState(console, log, todoList);
        }

        private void AddSubCommand(string commandString, string helpText, Action command)
        {
            mCommands.Add(commandString, new Tuple<string, Action>(helpText, command));
        }

        public override void Execute()
        {
            try
            {
                mCommands[Input].Item2();
            }
            catch(KeyNotFoundException)
            {
                mConsole.SetColour(ConsoleColor.DarkCyan);
                mConsole.OutputLine("Unrecognised command. Please enter one of the following commands");
                DisplayHelp();
            }
        }
        
        private void DeleteEntry()
        {
            mNextState = new RemoveTodoItemState(mConsole, mLog, mTodoList);
        }

        private void DisplayHelp()
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);

            foreach(String command in mCommands.Keys)
            {
                mConsole.Output(command + "\t- ");
                mConsole.OutputLine(mCommands[command].Item1);
            }

            mConsole.SetColour(ConsoleColor.Gray);
        }
    }
}
