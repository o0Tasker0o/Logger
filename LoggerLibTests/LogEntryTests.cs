using LoggerLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LoggerLibTests
{
    [TestClass]
    public class LogEntryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LogEntryThrowsExceptionWhenGivenNullText()
        {
            LogEntry entry = new LogEntry(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LogEntryThrowsExceptionWhenGivenEmptyText()
        {
            LogEntry entry = new LogEntry("");
        }

        [TestMethod]
        public void LogEntryStoresCurrentTimeWhenConstructed()
        {
            LogEntry entry = new LogEntry("test");

            DateTime currentTime = DateTime.Now;
            double delta = 2.0 * TimeSpan.TicksPerSecond;

            Assert.AreEqual(currentTime.Ticks, entry.CreatedTime.Ticks, delta);
        }

        [TestMethod]
        public void LogEntryStoresInputText()
        {
            const string cEntryText = "This is a log entry";

            LogEntry entry = new LogEntry(cEntryText);

            Assert.AreEqual(cEntryText, entry.Text);
        }
    }
}
