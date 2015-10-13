using System;
using System.ComponentModel.DataAnnotations;
namespace LoggerLib
{
    public class LogEntry
    {
        [Key]
        public long EntryId { get; private set; }

        public String Text
        {
            get;
            private set;
        }

        public DateTime CreatedTime
        {
            get;
            private set;
        }

        private LogEntry()
        {
            //Required for Entity framework
        }

        public LogEntry(string text)
        {
            if(null == text)
            {
                throw new ArgumentNullException("Log text must not be null");
            }

            if(String.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Log text must not be empty");
            }

            Text = text;
            CreatedTime = DateTime.Now;
        }

        public override string ToString()
        {
            return CreatedTime.ToString("dd/MM/yy HH:mm") + "> " + Text;
        }
    }
}
