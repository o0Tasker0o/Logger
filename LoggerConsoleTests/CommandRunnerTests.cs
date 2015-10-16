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
        private ILog mMockLog = Substitute.For<ILog>();
        private IConsole mMockConsole = Substitute.For<IConsole>();
        private readonly DateTime mYesterday = DateTime.Now.AddDays(-1).Date;
        private readonly DateTime mTomorrow = DateTime.Now.AddDays(+1).Date;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullConsole()
        {
            CommandRunner runner = new CommandRunner(null, mMockLog);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullLog()
        {
            CommandRunner runner = new CommandRunner(mMockConsole, null);
        }

        [TestMethod]
        public void CommandRunnerExitsWhenReadingBlankLine()
        {
            mMockConsole.ReadLine().Returns("");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(1).ReadLine();
            mMockLog.DidNotReceive().AddEntry(Arg.Any<LogEntry>());
            mMockLog.DidNotReceive().GetEntries();
        }

        [TestMethod]
        public void CommandRunnerAddsNewEntryWhenPlainTextEntered()
        {
            const string cEntryText = "This is a new log entry";

            mMockConsole.ReadLine().Returns(cEntryText, "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(2).ReadLine();
            mMockLog.Received(1).AddEntry(Arg.Is<LogEntry>(entry => entry.Text == cEntryText));
            mMockLog.DidNotReceive().GetEntries();
        }

        [TestMethod]
        public void CommandRunnerSearchesForEntriesWhenGivenASearchCommandString()
        {
            const string cEntryText = "search text";
            TestSearchCommand(">s", cEntryText, cEntryText, mYesterday.ToString(), mTomorrow.ToString());
        }

        [TestMethod]
        public void CommandRunnerSearchesForEntriesWhenGivenABlankCommandString()
        {
            const string cEntryText = "search text";
            TestSearchCommand(">", cEntryText, cEntryText, mYesterday.ToString(), mTomorrow.ToString());
        }

        [TestMethod]
        public void CommandRunnerInterpretsBlankStartDateStringAsEarliestDate()
        {
            const string cEntryText = "search text";
            TestSearchCommand(">s", cEntryText, cEntryText, "", mTomorrow.ToString());
        }

        [TestMethod]
        public void CommandRunnerInterpretsBlankEndDateStringAsNow()
        {
            const string cEntryText = "search text";
            TestSearchCommand(">s", cEntryText, cEntryText, mYesterday.ToString(), "");
        }

        [TestMethod]
        public void CommandRunnerSearchIsCaseInsensitiveForSearchTerms()
        {
            string entryText = "search text";
            string searchText = entryText.ToUpper();
            TestSearchCommand(">s", searchText, entryText, mYesterday.ToString(), "");
        }

        [TestMethod]
        public void CommandRunnerSearchIsCaseInsensitiveForEntryText()
        {
            string searchText = "search text";
            string entryText = searchText.ToUpper();
            TestSearchCommand(">s", searchText, entryText, mYesterday.ToString(), "");
        }

        private void TestSearchCommand(string searchCommand, string searchText, string entryText, string startDate, string endDate)
        {
            LogEntry testEntry = new LogEntry(entryText);
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.ReadLine().Returns(searchCommand, searchText, startDate, endDate, "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(5).ReadLine();
            mMockLog.Received(1).GetEntries();

            mMockConsole.Received(1).WriteLine(testEntry.ToString());
        }

        [TestMethod]
        public void CommandRunnerRetrievesNoEntriesIfSearchTextNotFound()
        {
            TestSearchReturnsNothingWhenNoMatchesFound("search text", "no entry contains this string", mYesterday, mTomorrow);
        }

        [TestMethod]
        public void CommandRunnerRetrievesNoEntriesIfEntryIsAfterDateRange()
        {
            TestSearchReturnsNothingWhenNoMatchesFound("search text", "search text", mYesterday, mYesterday);
        }

        [TestMethod]
        public void CommandRunnerRetrievesNoEntriesIfEntryIsBeforeDateRange()
        {
            TestSearchReturnsNothingWhenNoMatchesFound("search text", "search text", mTomorrow, mTomorrow);
        }

        private void TestSearchReturnsNothingWhenNoMatchesFound(string entryText, string searchText, DateTime startDate, DateTime endDate)
        {
            LogEntry testEntry = new LogEntry(entryText);
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.ReadLine().Returns(">s", searchText, startDate.ToString(), endDate.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(5).ReadLine();
            mMockLog.Received(1).GetEntries();

            mMockConsole.Received(0).WriteLine(Arg.Is<string>(str => str.Contains(testEntry.Text)));
        }

        [TestMethod]
        public void CommandRunnerRequestsStartDateUntilEnteredCorrectly()
        {
            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.ReadLine().Returns(">s", testEntry.Text, "INVALID DATE 1", mYesterday.ToString(), mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(6).ReadLine();
            mMockLog.Received(1).GetEntries();

            mMockConsole.Received(1).WriteLine(testEntry.ToString());
        }

        [TestMethod]
        public void CommandRunnerRequestsEndDateUntilEnteredCorrectly()
        {
            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.ReadLine().Returns(">s", testEntry.Text, mYesterday.ToString(), "INVALID DATE 1", mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(6).ReadLine();
            mMockLog.Received(1).GetEntries();

            mMockConsole.Received(1).WriteLine(testEntry.ToString());
        }

        [TestMethod]
        public void CommandRunnerSearchesForEntriesContainingAnySearchElements()
        {
            LogEntry testEntry1 = new LogEntry("entry containing element1");
            LogEntry testEntry2 = new LogEntry("entry containing element2");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry1, testEntry2 };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.ReadLine().Returns(">s", "element1 element2", mYesterday.ToString(), mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(5).ReadLine();
            mMockLog.Received(1).GetEntries();

            mMockConsole.Received(1).WriteLine(testEntry1.ToString());
            mMockConsole.Received(1).WriteLine(testEntry2.ToString());
        }

        [TestMethod]
        public void CommandRunnerCanSearchWithinRetrievedEntries()
        {
            LogEntry testEntry1 = new LogEntry("entry 1");
            LogEntry testEntry2 = new LogEntry("entry 2");
            LogEntry testEntry3 = new LogEntry("other thing 1");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry1, testEntry2, testEntry3 };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.ReadLine().Returns(">s", "entry", mYesterday.ToString(), mTomorrow.ToString(), ">rs", "1", mYesterday.ToString(), mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog);

            mMockConsole.Received(9).ReadLine();
            mMockLog.Received(1).GetEntries();

            mMockConsole.Received(2).WriteLine(testEntry1.ToString());
            mMockConsole.Received(1).WriteLine(testEntry2.ToString());
            mMockConsole.Received(0).WriteLine(testEntry3.ToString());
        }
    }
}
