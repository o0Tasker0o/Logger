using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;

namespace LoggerLibTests
{
    [TestClass]
    public class StoreTodoStateTests
    {
        [TestMethod]
        public void StoreTodoStateNextStateIsReadTodoState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            StoreTodoState state = new StoreTodoState(mockConsole, mockLog, mockTodoList);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }

        [TestMethod]
        public void StoreTodoStateStoresTextInTodoList()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            StoreTodoState state = new StoreTodoState(mockConsole, mockLog, mockTodoList);

            state.Input = "This is the entry text";
            state.Execute();

            mockTodoList.Received(1).AddEntry(Arg.Is<TodoEntry>(entry => entry.Text == state.Input));
        }
    }
}
