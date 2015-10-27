using System;
using System.ComponentModel.DataAnnotations;

namespace LoggerLib
{
    public class TodoEntry
    {
        [Key]
        public long EntryId { get; private set; }

        public String Text
        {
            get;
            private set;
        }

        private TodoEntry()
        {
            //Required for Entity framework
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
