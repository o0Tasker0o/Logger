using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib
{
    public class Log : ILog
    {
        public void AddEntry(LogEntry entry)
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                loggerDatabase.LogEntries.Add(entry);
                loggerDatabase.SaveChanges();
            }
        }

        public IEnumerable<LogEntry> GetEntries()
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                return loggerDatabase.LogEntries.ToList();
            }
        }
    }
}
