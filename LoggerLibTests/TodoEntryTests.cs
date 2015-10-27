using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LoggerLib;

namespace LoggerLibTests
{
    [TestClass]
    public class TodoEntryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TodoEntryThrowsExceptionWhenGivenNullText()
        {
            TodoEntry entry = new TodoEntry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TodoEntryThrowsExceptionWhenGivenEmptyText()
        {
            TodoEntry entry = new TodoEntry("");
        }

        [TestMethod]
        public void TodoEntryStoresInputText()
        {
            const string cEntryText = "This is a log entry";

            TodoEntry entry = new TodoEntry(cEntryText);

            Assert.AreEqual(cEntryText, entry.Text);
        }
    }
}
