using System.Data.Entity;

namespace LoggerLib
{
    public class LoggerDbContext : DbContext
    {
        public LoggerDbContext()
        {

        }

        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
