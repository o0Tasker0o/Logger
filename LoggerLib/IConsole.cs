using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib
{
    public interface IConsole
    {
        String GetInput();
        void Output(string text);
        void OutputLine(string text);
        void SetColour(ConsoleColor colour);
        void Clear();
    }
}
