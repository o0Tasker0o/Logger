using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerConsole
{
    public interface IConsole
    {
        String ReadLine();
        void WriteLine(string text);
        void Write(string text);
        void SetColour(ConsoleColor colour);
        void Clear();
    }
}
