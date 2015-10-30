using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;

namespace LoggerLibTests
{
    [TestClass]
    public class StoreStateTests
    {
        [TestMethod]
        public void StoreStateNextStateIsReadState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            StoreState state = new StoreState(mockConsole, mockLog);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadState));
        }

        [TestMethod]
        public void StoreStateStoresTextInLog()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            StoreState state = new StoreState(mockConsole, mockLog);

            state.Input = "This is the entry text";
            state.Execute();

            mockLog.Received(1).AddEntry(Arg.Is<LogEntry>(entry => entry.Text == state.Input));
        }
    }
}
