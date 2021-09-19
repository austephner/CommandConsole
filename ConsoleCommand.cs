using System.Collections.Generic;

namespace CommandConsole
{
    public abstract class ConsoleCommand
    {
        public ConsoleCommand() { }
        
        public abstract string[] GetNames();
        
        public abstract string GetHelp();
        
        public abstract void Execute(List<string> parameters);
    }
}