using LoggerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

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
            DisplayTodoListHeaderState state = new DisplayTodoListHeaderState(mockConsole, mockLog);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }

        [TestMethod]
        public void DisplayTodoListHeaderStateOutputsHeaderText()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            DisplayTodoListHeaderState state = new DisplayTodoListHeaderState(mockConsole, mockLog);
            state.Execute();

            mockConsole.Received(1).Clear();
            mockConsole.Received().OutputLine(Arg.Any<String>());
        }
    }
}
