using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;
using System.Collections.Generic;

namespace LoggerLibTests
{
    [TestClass]
    public class CommandLogStateTests
    {
        [TestMethod]
        public void CommandLogStateReturnsReadState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadState));
        }
        
        [TestMethod]
        public void CommandLogStateSearchesEntriesWhenGivenSearchString()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", yesterday.ToString(), tomorrow.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();
            
            mockConsole.Received(1).Output("Please enter the term you wish to search for: ");
            mockConsole.Received(1).Output("Please enter the date to start searching from: ");
            mockConsole.Received(1).Output("Please enter the date to search up to: ");

            mockConsole.Received(3).GetInput();

            mockConsole.Received(1).Output(entry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(entry.Text);
        }

        [TestMethod]
        public void CommandLogStateFindsEntriesMatchingSearchTerms()
        {
            LogEntry searchableEntry = new LogEntry("search term");
            LogEntry unsearchableEntry = new LogEntry("Nothing here");
            List<LogEntry> logEntries = new List<LogEntry>() { searchableEntry, unsearchableEntry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", yesterday.ToString(), tomorrow.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandLogStateFindsEntriesMatchingSearchTermsCaseInsensitive()
        {
            LogEntry searchableEntry = new LogEntry("SeArCh TeRm");
            LogEntry unsearchableEntry = new LogEntry("Nothing here");
            List<LogEntry> logEntries = new List<LogEntry>() { searchableEntry, unsearchableEntry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", yesterday.ToString(), tomorrow.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandLogStateFindsEntriesFromStartDate()
        {
            LogEntry searchableEntry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { searchableEntry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            DateTime startDate = DateTime.Now.AddDays(+1);
            DateTime endDate = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", startDate.ToString(), endDate.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.DidNotReceive().Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.DidNotReceive().OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandLogStateFindsEntriesToEndDate()
        {
            LogEntry searchableEntry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { searchableEntry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            DateTime startDate = DateTime.Now.AddDays(-1);
            DateTime endDate = DateTime.Now.AddDays(-1);
            mockConsole.GetInput().Returns("search term", startDate.ToString(), endDate.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.DidNotReceive().Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.DidNotReceive().OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandLogStateRequestsStartDateUntilValidDateProvided()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", "NOT A DATE", yesterday.ToString(), tomorrow.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output("Please enter the term you wish to search for: ");
            mockConsole.Received(2).Output("Please enter the date to start searching from: ");
            mockConsole.Received(1).Output("Please enter the date to search up to: ");

            mockConsole.Received(4).GetInput();

            mockConsole.Received(1).Output(entry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(entry.Text);
        }

        [TestMethod]
        public void CommandLogStateRequestsEndDateUntilValidDateProvided()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", yesterday.ToString(), "NOT A DATE", tomorrow.ToString());

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output("Please enter the term you wish to search for: ");
            mockConsole.Received(1).Output("Please enter the date to start searching from: ");
            mockConsole.Received(2).Output("Please enter the date to search up to: ");

            mockConsole.Received(4).GetInput();

            mockConsole.Received(1).Output(entry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(entry.Text);
        }

        [TestMethod]
        public void CommandLogStateSearchesAllEntriesIfNoDatesSpecified()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", "", "");

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output("Please enter the term you wish to search for: ");
            mockConsole.Received(1).Output("Please enter the date to start searching from: ");
            mockConsole.Received(1).Output("Please enter the date to search up to: ");

            mockConsole.Received(3).GetInput();

            mockConsole.Received(1).Output(entry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(entry.Text);
        }

        [TestMethod]
        public void CommandLogStateHandlesUnknownCommandStrings()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "UNKNOWN COMMAND";

            state.Execute();

            mockConsole.Received(1).OutputLine("Unrecognised command. Please enter one of the following commands");
            mockConsole.Received(2).OutputLine("Search log entries");
            mockConsole.Received(1).OutputLine("Search previous results");
            mockConsole.Received(1).OutputLine("Enter TODO list");
            mockConsole.Received(1).OutputLine("Display help");

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadState));
        }

        [TestMethod]
        public void CommandLogStateDisplaysHelpOnQuestionMark()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "?";

            state.Execute();

            mockConsole.Received(0).OutputLine("Unrecognised command. Please enter one of the following commands");
            mockConsole.Received(2).OutputLine("Search log entries");
            mockConsole.Received(1).OutputLine("Search previous results");
            mockConsole.Received(1).OutputLine("Enter TODO list");
            mockConsole.Received(1).OutputLine("Display help");

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadState));
        }

        [TestMethod]
        public void CommandLogStateReturnsTodoListState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "t";

            state.Execute();

            Assert.IsInstanceOfType(state.GetNextState(), typeof(DisplayTodoListHeaderState));
        }

        [TestMethod]
        public void CommandLogStateCanSearchPreviouslyReturnedResults()
        {
            List<LogEntry> logEntries = new List<LogEntry>() { new LogEntry("search term a"), 
                                                               new LogEntry("search term b"), 
                                                               new LogEntry("other text b") };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            mockConsole.GetInput().Returns("search term", "", "");

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).OutputLine("search term a");
            mockConsole.Received(1).OutputLine("search term b");
            mockConsole.DidNotReceive().OutputLine("other text b");

            mockConsole.ClearReceivedCalls();

            mockConsole.GetInput().Returns("b", "", "");
            state.Input = "rs";

            state.Execute();

            mockConsole.DidNotReceive().OutputLine("search term a");
            mockConsole.Received(1).OutputLine("search term b");
            mockConsole.DidNotReceive().OutputLine("other text b");
        }

        [TestMethod]
        public void CommandLogStateSearchesAllResultsIfRecursiveSearchIsCalledBeforeStandardSearch()
        {
            List<LogEntry> logEntries = new List<LogEntry>() { new LogEntry("search term a"), 
                                                               new LogEntry("search term b"), 
                                                               new LogEntry("other text b") };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            mockConsole.GetInput().Returns("search term", "", "");

            ITodoList mockTodoList = Substitute.For<ITodoList>();

            CommandLogState state = new CommandLogState(mockConsole, mockLog, mockTodoList);

            mockConsole.GetInput().Returns("b", "", "");
            state.Input = "rs";

            state.Execute();

            mockConsole.DidNotReceive().OutputLine("search term a");
            mockConsole.Received(1).OutputLine("search term b");
            mockConsole.Received(1).OutputLine("other text b");
        }
    }
}
