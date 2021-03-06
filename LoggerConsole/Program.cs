﻿using LoggerLib;
using System;
using System.IO;

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

            State nextState = new DisplayLogHeaderState(console, log, todoList);

            while(null != nextState)
            {
                nextState.Execute();
                nextState = nextState.GetNextState();
            }
        }

    }
}
