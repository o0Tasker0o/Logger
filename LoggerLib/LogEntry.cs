using System;
namespace LoggerLib
{
    public class LogEntry
    {
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
    }
}
