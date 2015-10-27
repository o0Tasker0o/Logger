using System;
using System.Collections.Generic;

namespace LoggerLib
{
    public interface ITodoList
    {
        void AddEntry(TodoEntry entry);
        void RemoveEntry(Int32 id);
        IEnumerable<TodoEntry> GetEntries();
    }
}
