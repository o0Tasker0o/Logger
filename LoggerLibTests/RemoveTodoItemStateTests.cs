using LoggerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace LoggerLibTests
{
    [TestClass]
    public class RemoveTodoItemStateTests
    {
        [TestMethod]
        public void RemoveTodoItemStateRequestsItemIdToRemove()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            const UInt32 cTodoItemId = 0;

            mockConsole.GetInput().Returns(cTodoItemId.ToString());

            RemoveTodoItemState state = new RemoveTodoItemState(mockConsole, mockLog, mockTodoList);

            state.Execute();

            mockConsole.Received(1).Output("Enter the item ID to remove");
            mockConsole.Received(1).GetInput();
            mockTodoList.Received(1).RemoveEntry(cTodoItemId);

            Assert.IsInstanceOfType(state.GetNextState(), typeof(ReadTodoState));
        }

        [TestMethod]
        public void RemoveTodoItemStateRequestsItemIdToRemoveUntilValidIdIsProvided()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            const UInt32 cTodoItemId = 0;

            mockConsole.GetInput().Returns("", "-1", "NOT A NUMBER", cTodoItemId.ToString());

            RemoveTodoItemState state = new RemoveTodoItemState(mockConsole, mockLog, mockTodoList);

            state.Execute();

            mockConsole.Received(4).Output("Enter the item ID to remove");
            mockConsole.Received(4).GetInput();
            mockTodoList.Received(1).RemoveEntry(cTodoItemId);
        }

        [TestMethod]
        public void RemoveTodoItemStateHandlesUnknownTodoListIDs()
        {
            ILog mockLog = Substitute.For<ILog>();
            IConsole mockConsole = Substitute.For<IConsole>();
            ITodoList mockTodoList = Substitute.For<ITodoList>();

            mockConsole.GetInput().Returns("0");
            mockTodoList.When(todoList => todoList.RemoveEntry(Arg.Any<UInt32>())).Do(x => { throw new ArgumentOutOfRangeException(); });

            RemoveTodoItemState state = new RemoveTodoItemState(mockConsole, mockLog, mockTodoList);

            state.Execute();

            mockConsole.Received(1).OutputLine("Item with given ID not found");
        }
    }
}
