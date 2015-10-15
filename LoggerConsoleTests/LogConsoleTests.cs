using LoggerConsole;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace LoggerConsoleTests
{
    [TestClass]
    public class LogConsoleTests
    {
        private static MemoryStream mOutStream;
        private static StreamWriter mOutWriter;

        [TestInitialize]
        public void Setup()
        {
            mOutStream = new MemoryStream();

            mOutWriter = new StreamWriter(mOutStream);

            Console.SetOut(mOutWriter);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mOutStream.Dispose();
        }

        [TestMethod]
        public void LogConsoleWritesToConsole()
        {
            const string cInputString = "test";
            LogConsole logConsole = new LogConsole();

            logConsole.Write("test");

            Assert.AreEqual(cInputString, GetOutputString());
        }

        [TestMethod]
        public void LogConsoleWritesLineToConsole()
        {
            const string cInputString = "test";
            LogConsole logConsole = new LogConsole();

            logConsole.WriteLine("test");

            Assert.AreEqual(cInputString + Environment.NewLine, GetOutputString());
        }

        private string GetOutputString()
        {
            mOutWriter.Flush();
            return GetStringFromStream(mOutStream);
        }

        private static string GetStringFromStream(MemoryStream stream)
        {
            stream.Position = 0;
            StreamReader streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }
    }
}
