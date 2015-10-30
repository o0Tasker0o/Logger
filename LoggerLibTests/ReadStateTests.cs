using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;

namespace LoggerLibTests
{
    [TestClass]
    public class StateTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadStateThrowsExceptionWhenPassedNullConsole()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            State state = new ReadState(null, mockLog, mockTodoList);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadStateThrowsExceptionWhenPassedNullLog()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            State state = new ReadState(mockConsole, null, mockTodoList);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadStateThrowsExceptionWhenPassedNullTodoList()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            State state = new ReadState(mockConsole, mockLog, null);
        }
    }

    [TestClass]
    public class ReadStateTests
    {
        [TestMethod]
        public void ReadStateNextStateIsNull()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadState state = new ReadState(mockConsole, mockLog, mockTodoList);
            Assert.IsNull(state.GetNextState());
        }

        [TestMethod]
        public void ReadStateOutputsPromptCharacter()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadState state = new ReadState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            mockConsole.Received(1).Output(">");
        }

        [TestMethod]
        public void ReadStateReadsConsoleWhenExecuted()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadState state = new ReadState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            mockConsole.Received(1).GetInput();
        }

        [TestMethod]
        public void ReadStateReturnsStoreStateWhenReadingPlainText()
        {
            const string cConsoleInput = "This is plain text";
            IConsole mockConsole = Substitute.For<IConsole>();
            mockConsole.GetInput().Returns(cConsoleInput);

            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadState state = new ReadState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            State nextState = state.GetNextState();
            Assert.IsInstanceOfType(nextState, typeof(StoreState));
            Assert.AreEqual(cConsoleInput, nextState.Input);
        }

        [TestMethod]
        public void ReadStateReturnsCommandStateWhenReadingCommand()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            mockConsole.GetInput().Returns(">COMMANDSTRING");

            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadState state = new ReadState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            State nextState = state.GetNextState();
            Assert.IsInstanceOfType(nextState, typeof(CommandState));
            Assert.AreEqual("COMMANDSTRING", nextState.Input);
        }

        [TestMethod]
        public void ReadStateReturnsNullStateWhenReadingBlankString()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();
            ReadState state = new ReadState(mockConsole, mockLog, mockTodoList);
            state.Execute();

            Assert.IsNull(state.GetNextState());
        }
    }
}
