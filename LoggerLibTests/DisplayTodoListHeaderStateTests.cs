using LoggerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace LoggerLibTests
{
    [TestClass]
    public class DisplayTodoListHeaderStateTests
    {
        [TestMethod]
        public void DisplayTodoListHeaderStateReturnsReadState()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            DisplayTodoListHeaderState state = new DisplayTodoListHeaderState(mockConsole, mockLog, mockTodoList);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }

        [TestMethod]
        public void DisplayTodoListHeaderStateOutputsHeaderText()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            DisplayTodoListHeaderState state = new DisplayTodoListHeaderState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            mockConsole.Received(1).Clear();
            mockConsole.Received().OutputLine(Arg.Any<String>());
        }

        [TestMethod]
        public void DisplayTodoHeaderStateOutputsAllTodoItems()
        {
            TodoEntry todoEntry = new TodoEntry("entry");
            List<TodoEntry> todoEntries = new List<TodoEntry>() { todoEntry };

            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            mockTodoList.GetEntries().Returns(todoEntries);

            DisplayTodoListHeaderState state = new DisplayTodoListHeaderState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            mockConsole.Received(1).Output("0> ");
            mockConsole.Received(1).OutputLine("entry");
        }
    }
}
