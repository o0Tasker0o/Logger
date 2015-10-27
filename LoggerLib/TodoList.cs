using System;
using System.Collections.Generic;
using System.Linq;

namespace LoggerLib
{
    public class TodoList : ITodoList
    {
        public void AddEntry(TodoEntry entry)
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                loggerDatabase.TodoEntries.Add(entry);
                loggerDatabase.SaveChanges();
            }
        }

        public IEnumerable<TodoEntry> GetEntries()
        {
            using (LoggerDbContext loggerDatabase = new LoggerDbContext())
            {
                return loggerDatabase.TodoEntries.ToList();
            }
        }

        public void RemoveEntry(Int32 id)
        {

        }
    }
}
