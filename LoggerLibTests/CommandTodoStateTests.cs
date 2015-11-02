using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;
using System.Collections.Generic;

namespace LoggerLibTests
{
    [TestClass]
    public class CommandTodoStateTests
    {
        [TestMethod]
        public void CommandTodoStateReturnsReadTodoState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandTodoState state = new CommandTodoState(mockConsole, mockLog, mockTodoList);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }
        
        [TestMethod]
        public void CommandTodoStateHandlesUnknownCommandStrings()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandTodoState state = new CommandTodoState(mockConsole, mockLog, mockTodoList);
            state.Input = "UNKNOWN COMMAND";

            state.Execute();

            mockConsole.Received(1).OutputLine("Unrecognised command. Please enter one of the following commands");
            mockConsole.Received(1).OutputLine("Remove todo list entry");
            mockConsole.Received(1).OutputLine("Display help");

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }

        [TestMethod]
        public void CommandTodoStateDisplaysHelpOnQuestionMark()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandTodoState state = new CommandTodoState(mockConsole, mockLog, mockTodoList);
            state.Input = "?";

            state.Execute();

            mockConsole.Received(0).OutputLine("Unrecognised command. Please enter one of the following commands");
            mockConsole.Received(1).OutputLine("Remove todo list entry");
            mockConsole.Received(1).OutputLine("Display help");

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }

        [TestMethod]
        public void CommandTodoStateReturnsRemoveTodoItemState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandTodoState state = new CommandTodoState(mockConsole, mockLog, mockTodoList);
            state.Input = "r";

            state.Execute();

            Assert.IsInstanceOfType(state.GetNextState(), typeof(RemoveTodoItemState));
        }
    }
}
