﻿using LoggerConsole;
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
        private ITodoList mMockTodoList = Substitute.For<ITodoList>();
        private IConsole mMockConsole = Substitute.For<IConsole>();
        private readonly DateTime mYesterday = DateTime.Now.AddDays(-1).Date;
        private readonly DateTime mTomorrow = DateTime.Now.AddDays(+1).Date;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullConsole()
        {
            CommandRunner runner = new CommandRunner(null, mMockLog, mMockTodoList);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullLog()
        {
            CommandRunner runner = new CommandRunner(mMockConsole, null, mMockTodoList);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CommandRunnerThrowsExceptionWhenPassedNullTodoList()
        {
            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, null);
        }

        [TestMethod]
        public void CommandRunnerExitsWhenReadingBlankLine()
        {
            mMockConsole.GetInput().Returns("");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(1).GetInput();
            mMockLog.DidNotReceive().AddEntry(Arg.Any<LogEntry>());
        }

        [TestMethod]
        public void CommandRunnerAddsNewEntryWhenPlainTextEntered()
        {
            const string cEntryText = "This is a new log entry";

            mMockConsole.GetInput().Returns(cEntryText, "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(2).GetInput();
            mMockLog.Received(1).AddEntry(Arg.Is<LogEntry>(entry => entry.Text == cEntryText));
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

            mMockConsole.GetInput().Returns(searchCommand, searchText, startDate, endDate, "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(5).GetInput();
            mMockLog.Received().GetEntries();

            mMockConsole.Received(2).OutputLine(testEntry.Text);
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

            mMockConsole.GetInput().Returns(">s", searchText, startDate.ToString(), endDate.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(5).GetInput();
            mMockLog.Received().GetEntries();

            mMockConsole.Received(1).OutputLine(Arg.Is<string>(str => str.Contains(testEntry.Text)));
        }

        [TestMethod]
        public void CommandRunnerRequestsStartDateUntilEnteredCorrectly()
        {
            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.GetInput().Returns(">s", testEntry.Text, "INVALID DATE 1", mYesterday.ToString(), mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(6).GetInput();
            mMockLog.Received().GetEntries();

            mMockConsole.Received(2).OutputLine(testEntry.Text);
        }

        [TestMethod]
        public void CommandRunnerRequestsEndDateUntilEnteredCorrectly()
        {
            LogEntry testEntry = new LogEntry("search text");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.GetInput().Returns(">s", testEntry.Text, mYesterday.ToString(), "INVALID DATE 1", mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(6).GetInput();
            mMockLog.Received().GetEntries();

            mMockConsole.Received(2).OutputLine(testEntry.Text);
        }

        [TestMethod]
        public void CommandRunnerSearchesForEntriesContainingAnySearchElements()
        {
            LogEntry testEntry1 = new LogEntry("entry containing element1");
            LogEntry testEntry2 = new LogEntry("entry containing element2");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry1, testEntry2 };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.GetInput().Returns(">s", "element1 element2", mYesterday.ToString(), mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(5).GetInput();
            mMockLog.Received().GetEntries();

            mMockConsole.Received(2).OutputLine(testEntry1.Text);
            mMockConsole.Received(2).OutputLine(testEntry2.Text);
        }

        [TestMethod]
        public void CommandRunnerCanSearchWithinRetrievedEntries()
        {
            LogEntry testEntry1 = new LogEntry("entry 1");
            LogEntry testEntry2 = new LogEntry("entry 2");
            LogEntry testEntry3 = new LogEntry("other thing 1");
            List<LogEntry> testEntries = new List<LogEntry>() { testEntry1, testEntry2, testEntry3 };
            mMockLog.GetEntries().Returns(testEntries);

            mMockConsole.GetInput().Returns(">s", "entry", mYesterday.ToString(), mTomorrow.ToString(), ">rs", "1", mYesterday.ToString(), mTomorrow.ToString(), "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(9).GetInput();
            mMockLog.Received().GetEntries();

            mMockConsole.Received(3).OutputLine(testEntry1.Text);
            mMockConsole.Received(2).OutputLine(testEntry2.Text);
            mMockConsole.Received(1).OutputLine(testEntry3.Text);
        }

        [TestMethod]
        public void TypingQuestionMarkPresentsListOfCommands()
        {
            mMockConsole.GetInput().Returns(">?", "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(2).GetInput();

            mMockConsole.Received().Output(Arg.Is<String>(line => line.StartsWith(">")));
        }

        [TestMethod]
        public void TypingUnknownCommandShowsWarningAndPrintsListOfAvailableCommands()
        {
            mMockConsole.GetInput().Returns(">THISISNOTACOMMAND", "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(2).GetInput();

            mMockConsole.Received(1).OutputLine("Unrecognised command. Please enter one of the following commands");
            mMockConsole.Received().Output(Arg.Is<String>(line => line.StartsWith(">")));
        }

        [TestMethod]
        public void TypingTodoCommandListsAllItemsInTodoList()
        {
            IEnumerable<TodoEntry> todoList = new List<TodoEntry>() { new TodoEntry("todo entry") };
            mMockConsole.GetInput().Returns(">t", "", "");
            mMockTodoList.GetEntries().Returns(todoList);

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(3).GetInput();
            mMockTodoList.Received(1).GetEntries();
            mMockConsole.Received(1).OutputLine("todo entry");
        }

        [TestMethod]
        public void CommandRunnerAddsNewTodoEntryWhenPlainTextEntered()
        {
            const string cEntryText = "This is a new log entry";

            mMockConsole.GetInput().Returns(">t", cEntryText, "", "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(4).GetInput();
            mMockConsole.Received().Clear();
            mMockTodoList.Received(1).AddEntry(Arg.Is<TodoEntry>(entry => entry.Text == cEntryText));
        }

        [TestMethod]
        public void CommandRunnerParsesRemoveCommand()
        {
            mMockConsole.GetInput().Returns(">t", ">r", "0", "", "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(5).GetInput();
            mMockTodoList.DidNotReceive().AddEntry(Arg.Any<TodoEntry>());
            mMockTodoList.Received(1).RemoveEntry(0);
        }

        [TestMethod]
        public void CommandRunnerIgnoresNegativeNumbersForRemoveCommand()
        {
            TestRemoveTodoItemErrorCases("-1");
            TestRemoveTodoItemErrorCases("NotANumber");
            TestRemoveTodoItemErrorCases(null);
        }

        private void TestRemoveTodoItemErrorCases(string itemId)
        {
            mMockConsole.GetInput().Returns(">t", ">r", itemId, "", "");

            CommandRunner runner = new CommandRunner(mMockConsole, mMockLog, mMockTodoList);

            mMockConsole.Received(5).GetInput();
            mMockTodoList.DidNotReceive().AddEntry(Arg.Any<TodoEntry>());
            mMockTodoList.DidNotReceive().RemoveEntry(Arg.Any<uint>());

            mMockConsole.ClearReceivedCalls();
            mMockTodoList.ClearReceivedCalls();
        }
    }
}
