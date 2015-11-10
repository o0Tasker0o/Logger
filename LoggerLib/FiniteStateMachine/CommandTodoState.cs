
namespace LoggerLib
{
    public class CommandTodoState : CommandState
    {
        public CommandTodoState(IConsole console, ILog log, ITodoList todoList) : base(console, log, todoList)
        {
            AddSubCommand("r", "Remove todo list entry", DeleteEntry);

            SetNextState(typeof(ReadTodoState));

            RegisterState(typeof(CommandTodoState), this);
        }

        private void DeleteEntry()
        {
            SetNextState(typeof(RemoveTodoItemState));
        }

        protected override void DisplayHelp()
        {
            base.DisplayHelp();
            SetNextState(typeof(ReadTodoState));
        }
    }
}
