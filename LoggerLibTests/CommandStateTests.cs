using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;
using NSubstitute;
using System.Collections.Generic;

namespace LoggerLibTests
{
    [TestClass]
    public class CommandStateTests
    {
        [TestMethod]
        public void CommandStateReturnsReadState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            CommandState state = new CommandState(mockConsole, mockLog);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadState));
        }
        
        [TestMethod]
        public void CommandStateSearchesEntriesWhenGivenSearchString()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", yesterday.ToString(), tomorrow.ToString());

            CommandState state = new CommandState(mockConsole, mockLog);
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
        public void CommandStateFindsEntriesMatchingSearchTerms()
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

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandStateFindsEntriesMatchingSearchTermsCaseInsensitive()
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

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "s";

            state.Execute();

            mockConsole.Received(1).Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.Received(1).OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandStateFindsEntriesFromStartDate()
        {
            LogEntry searchableEntry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { searchableEntry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            DateTime startDate = DateTime.Now.AddDays(+1);
            DateTime endDate = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", startDate.ToString(), endDate.ToString());

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "s";

            state.Execute();

            mockConsole.DidNotReceive().Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.DidNotReceive().OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandStateFindsEntriesFromEndDate()
        {
            LogEntry searchableEntry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { searchableEntry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();

            DateTime startDate = DateTime.Now.AddDays(-1);
            DateTime endDate = DateTime.Now.AddDays(-1);
            mockConsole.GetInput().Returns("search term", startDate.ToString(), endDate.ToString());

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "s";

            state.Execute();

            mockConsole.DidNotReceive().Output(searchableEntry.CreatedTime.ToString("dd/MM/yy HH:mm> "));
            mockConsole.DidNotReceive().OutputLine(searchableEntry.Text);
        }

        [TestMethod]
        public void CommandStateRequestsStartDateUntilValidDateProvided()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", "NOT A DATE", yesterday.ToString(), tomorrow.ToString());

            CommandState state = new CommandState(mockConsole, mockLog);
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
        public void CommandStateRequestsEndDateUntilValidDateProvided()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", yesterday.ToString(), "NOT A DATE", tomorrow.ToString());

            CommandState state = new CommandState(mockConsole, mockLog);
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
        public void CommandStateSearchesAllEntriesIfNoDatesSpecified()
        {
            LogEntry entry = new LogEntry("search term");
            List<LogEntry> logEntries = new List<LogEntry>() { entry };
            ILog mockLog = Substitute.For<ILog>();
            mockLog.GetEntries().Returns(logEntries);

            IConsole mockConsole = Substitute.For<IConsole>();
            DateTime yesterday = DateTime.Now.AddDays(-1);
            DateTime tomorrow = DateTime.Now.AddDays(+1);
            mockConsole.GetInput().Returns("search term", "", "");

            CommandState state = new CommandState(mockConsole, mockLog);
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
        public void CommandStateHandlesUnknownCommandStrings()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "UNKNOWN COMMAND";

            state.Execute();

            mockConsole.Received(1).OutputLine("Unrecognised command. Please enter one of the following commands");
            mockConsole.Received(1).OutputLine("s\t- Search log entries");
            mockConsole.Received(1).OutputLine("\t- Search log entries");
            mockConsole.Received(1).OutputLine("rs\t- (UNAVAILABLE) Search previous results");
            mockConsole.Received(1).OutputLine("t\t- Enter TODO list");
            mockConsole.Received(1).OutputLine("?\t- Display help");
        }

        [TestMethod]
        public void CommandStateDisplaysHelpOnQuestionMark()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "?";

            state.Execute();

            mockConsole.Received(0).OutputLine("Unrecognised command. Please enter one of the following commands");
            mockConsole.Received(1).OutputLine("s\t- Search log entries");
            mockConsole.Received(1).OutputLine("\t- Search log entries");
            mockConsole.Received(1).OutputLine("rs\t- (UNAVAILABLE) Search previous results");
            mockConsole.Received(1).OutputLine("t\t- Enter TODO list");
            mockConsole.Received(1).OutputLine("?\t- Display help");
        }

        [TestMethod]
        public void CommandStateReturnsTodoListState()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();

            CommandState state = new CommandState(mockConsole, mockLog);
            state.Input = "t";

            state.Execute();

            Assert.IsInstanceOfType(state.GetNextState(), typeof(DisplayTodoListHeaderState));
        }
    }
}
