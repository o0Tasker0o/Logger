using System.Data.Entity;

namespace LoggerLib
{
    internal class LoggerDbContext : DbContext
    {
        public LoggerDbContext()
        {

        }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
