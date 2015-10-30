using LoggerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace LoggerLibTests
{
    [TestClass]
    public class DisplayLogHeaderStateTests
    {
        [TestMethod]
        public void DisplayLogHeaderStateReturnsReadState()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            DisplayLogHeaderState state = new DisplayLogHeaderState(mockConsole, mockLog);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadState));
        }

        [TestMethod]
        public void DisplayLogHeaderStateOutputsHeaderText()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            DisplayLogHeaderState state = new DisplayLogHeaderState(mockConsole, mockLog);
            state.Execute();

            mockConsole.Received(1).Clear();
            mockConsole.Received().OutputLine(Arg.Any<String>());
        }

        [TestMethod]
        public void DisplayLogHeaderStateOutputsTodaysLogs()
        {
            LogEntry logEntry = new LogEntry("entry");
            List<LogEntry> logEntries = new List<LogEntry>() { logEntry };

            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            DisplayLogHeaderState state = new DisplayLogHeaderState(mockConsole, mockLog);
            state.Execute();

            mockConsole.Received(1).Output("> ");
            mockConsole.Received(1).OutputLine("entry");
        }
    }
}
