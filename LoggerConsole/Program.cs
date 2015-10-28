using LoggerLib;
using System;
using System.IO;
using System.Linq;

namespace LoggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth, 60);
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));

            Log log = new Log();
            TodoList todoList = new TodoList();
            LogConsole console = new LogConsole();

            CommandRunner runner = new CommandRunner(console, log, todoList);
        }

    }
}
