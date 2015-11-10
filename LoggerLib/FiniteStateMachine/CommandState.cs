using System;
using System.Collections.Generic;

namespace LoggerLib
{
    public class CommandState : State
    {
        private Dictionary<string, Tuple<string, Action>> mCommands;

        public CommandState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            mCommands = new Dictionary<string, Tuple<string, Action>>();

            AddSubCommand("?", "Display help", DisplayHelp);
        }

        protected void AddSubCommand(string commandString, string helpText, Action command)
        {
            mCommands.Add(commandString, new Tuple<string, Action>(helpText, command));
        }

        public override void Execute()
        {
            try
            {
                mCommands[Input].Item2();
            }
            catch (KeyNotFoundException)
            {
                mConsole.SetColour(ConsoleColor.DarkCyan);
                mConsole.OutputLine("Unrecognised command. Please enter one of the following commands");
                DisplayHelp();
            }
        }

        protected virtual void DisplayHelp()
        {
            mConsole.SetColour(ConsoleColor.DarkCyan);

            foreach (String command in mCommands.Keys)
            {
                mConsole.Output(command + "\t- ");
                mConsole.OutputLine(mCommands[command].Item1);
            }

            mConsole.SetColour(ConsoleColor.Gray);
        }
    }
}
