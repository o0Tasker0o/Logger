using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib
{
    public interface ILog
    {
        void AddEntry(LogEntry entry);
        IEnumerable<LogEntry> GetEntries();
    }
}
