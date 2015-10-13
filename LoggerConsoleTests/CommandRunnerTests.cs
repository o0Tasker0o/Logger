using LoggerConsole;
using LoggerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace LoggerConsoleTests
{
    [TestClass]
    public class CommandRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullConsole()
        {
            ILog mockLog = Substitute.For<ILog>();
            CommandRunner runner = new CommandRunner(null, mockLog);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullLog()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            CommandRunner runner = new CommandRunner(mockConsole, null);
        }

        [TestMethod]
        public void CommandRunnerExitsWhenReadingBlankLine()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            mockConsole.ReadLine().Returns("");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(1).ReadLine();
            mockLog.DidNotReceive().AddEntry(Arg.Any<LogEntry>());
            mockLog.DidNotReceive().GetEntries();
        }

        [TestMethod]
        public void CommandRunnerAddsNewEntryWhenPlainTextEntered()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            const string cEntryText = "This is a new log entry";

            mockConsole.ReadLine().Returns(cEntryText, "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(2).ReadLine();
            mockLog.Received(1).AddEntry(Arg.Is<LogEntry>(entry => entry.Text == cEntryText));
            mockLog.DidNotReceive().GetEntries();
        }

        [TestMethod]
        public void CommandRunnerSearchesForEntriesWhenGivenACommandString()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mockLog.GetEntries().Returns(testEntries);

            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            DateTime tomorrow = DateTime.Now.AddDays(+1).Date;
            mockConsole.ReadLine().Returns(">", testEntry.Text, yesterday.ToString(), tomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(5).ReadLine();
            mockLog.Received(1).GetEntries();

            mockConsole.Received(1).WriteLine(testEntry.ToString());
        }

        [TestMethod]
        public void CommandRunnerRetrievesNoEntriesIfSearchTextNotFound()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mockLog.GetEntries().Returns(testEntries);

            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            DateTime tomorrow = DateTime.Now.AddDays(+1).Date;
            mockConsole.ReadLine().Returns(">", "No entry contains this text", yesterday.ToString(), tomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(5).ReadLine();
            mockLog.Received(1).GetEntries();

            mockConsole.Received(0).WriteLine(Arg.Is<string>(str => str.Contains(testEntry.Text)));
        }

        [TestMethod]
        public void CommandRunnerRetrievesNoEntriesIfEntryIsAfterDateRange()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mockLog.GetEntries().Returns(testEntries);

            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            mockConsole.ReadLine().Returns(">", testEntry.Text, yesterday.ToString(), yesterday.ToString(), "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(5).ReadLine();
            mockLog.Received(1).GetEntries();

            mockConsole.Received(0).WriteLine(Arg.Is<string>(str => str.Contains(testEntry.Text)));
        }

        [TestMethod]
        public void CommandRunnerRetrievesNoEntriesIfEntryIsBeforeDateRange()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mockLog.GetEntries().Returns(testEntries);

            DateTime tomorrow = DateTime.Now.AddDays(+1).Date;
            mockConsole.ReadLine().Returns(">", testEntry.Text, tomorrow.ToString(), tomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(5).ReadLine();
            mockLog.Received(1).GetEntries();

            mockConsole.Received(0).WriteLine(Arg.Is<string>(str => str.Contains(testEntry.Text)));
        }

        [TestMethod]
        public void CommandRunnerRequestsStartDateUntilEnteredCorrectly()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mockLog.GetEntries().Returns(testEntries);

            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            DateTime tomorrow = DateTime.Now.AddDays(+1).Date;
            mockConsole.ReadLine().Returns(">", testEntry.Text, "INVALID DATE 1", yesterday.ToString(), tomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(6).ReadLine();
            mockLog.Received(1).GetEntries();

            mockConsole.Received(1).WriteLine(testEntry.ToString());
        }

        [TestMethod]
        public void CommandRunnerRequestsEndDateUntilEnteredCorrectly()
        {
            IConsole mockConsole = Substitute.For<IConsole>();
            ILog mockLog = Substitute.For<ILog>();

            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mockLog.GetEntries().Returns(testEntries);

            DateTime yesterday = DateTime.Now.AddDays(-1).Date;
            DateTime tomorrow = DateTime.Now.AddDays(+1).Date;
            mockConsole.ReadLine().Returns(">", testEntry.Text, yesterday.ToString(), "INVALID DATE 1", tomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mockConsole, mockLog);

            mockConsole.Received(6).ReadLine();
            mockLog.Received(1).GetEntries();

            mockConsole.Received(1).WriteLine(testEntry.ToString());
        }
    }
}
