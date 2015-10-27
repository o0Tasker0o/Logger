using System;

namespace LoggerLib
{
    public class TodoEntry
    {
        public String Text
        {
            get;
            private set;
        }

        public TodoEntry(String text)
        {
            if (null == text)
            {
                throw new ArgumentNullException("TODO text must not be null");
            }

            if (String.IsNullOrEmpty(text))
            {
                throw new ArgumentException("TODO text must not be empty");
            }

            Text = text;
        }
    }
}
