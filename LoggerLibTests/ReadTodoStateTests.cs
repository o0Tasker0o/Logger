using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;

namespace LoggerLibTests
{
    [TestClass]
    public class ReadTodoStateTests
    {
        [TestMethod]
        public void ReadTodoStateNextStateIsNull()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadTodoState state = new ReadTodoState(mockConsole, mockLog, mockTodoList);
            Assert.IsNull(state.GetNextState());
        }

        [TestMethod]
        public void ReadTodoStateOutputsPromptCharacter()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadTodoState state = new ReadTodoState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            mockConsole.Received(1).Output(">");
        }

        [TestMethod]
        public void ReadTodoStateReadsConsoleWhenExecuted()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadTodoState state = new ReadTodoState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            mockConsole.Received(1).GetInput();
        }

        [TestMethod]
        public void ReadTodoStateReturnsStoreStateWhenReadingPlainText()
        {
            const string cConsoleInput = "This is plain text";
            IConsole mockConsole = Substitute.For<IConsole>();
            mockConsole.GetInput().Returns(cConsoleInput);

            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadTodoState state = new ReadTodoState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            State nextState = state.GetNextState();
            Assert.IsInstanceOfType(nextState, typeof(StoreTodoState));
            Assert.AreEqual(cConsoleInput, nextState.Input);
        }

        [TestMethod]
        public void ReadTodoStateReturnsCommandStateWhenReadingCommand()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            mockConsole.GetInput().Returns(">COMMANDSTRING");

            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadTodoState state = new ReadTodoState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            State nextState = state.GetNextState();
            Assert.IsInstanceOfType(nextState, typeof(CommandState));
            Assert.AreEqual("COMMANDSTRING", nextState.Input);
        }

        [TestMethod]
        public void ReadTodoStateReturnsDisplayLogHeaderStateStateWhenReadingBlankString()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadTodoState state = new ReadTodoState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            Assert.IsInstanceOfType(state.GetNextState(), typeof(DisplayLogHeaderState));
        }
    }
}
