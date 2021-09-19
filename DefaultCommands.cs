using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CommandConsole
{
    public class EchoCommand : ConsoleCommand
    {
        public override string[] GetNames()
        {
            return new string[] {"echo"};
        }

        public override string GetHelp()
        {
            return "Takes input and \"echoes\" it back to the console.";
        }

        public override void Execute(List<string> parameters)
        {
            CommandConsoleHelper.Print(string.Join(" ", parameters));
        }
    }
    
    public class HelpCommand : ConsoleCommand
    {
        public override string[] GetNames()
        {
            return new string[] { "h", "help" };
        }

        public override string GetHelp()
        {
            return "Provides help, info, documentation, etc. about the given command.";
        }

        public override void Execute(List<string> parameters)
        {
            if (parameters.Count == 0)
            {
                var commands = CommandConsoleBehaviour.GetConsoleCommands();
                foreach (var command in commands)
                {
                    CommandConsoleHelper.Print($"{string.Join(", ", command.GetNames())} --> <i> {command.GetHelp()}</i>");
                }
            }
            else
            {
                var command = CommandConsoleBehaviour.GetCommand(parameters[0].ToString());

                if (command != null)
                {
                    CommandConsoleHelper.Print(command.GetHelp());
                }
                else
                {
                    CommandConsoleHelper.Print(string.Format(CommandConsoleBehaviour.INVALID_CMD_MSG, parameters[0]));
                }
            }
        }
    }

    public class ClearCommand : ConsoleCommand
    {
        public override string[] GetNames()
        {
            return new string[] { "c", "cls", "clr", "clear" };
        }

        public override string GetHelp()
        {
            return "Clears the console of all text.";
        }

        public override void Execute(List<string> parameters)
        {
            CommandConsoleHelper.Clear();
        }
    }

    public class DumpCommand : ConsoleCommand
    {
        public override string[] GetNames()
        {
            return new string[] { "dump" };
        }

        public override string GetHelp()
        {
            return "Dumps all console content to a new file. If no filename is specified, the default \"consoledump_x.txt\" will be used.";
        }

        public override void Execute(List<string> parameters)
        {
            if (parameters.Count > 0)
            {
                try
                {
                    File.WriteAllText(parameters[0], CommandConsoleBehaviour.Instance.GetCurrentContent());
                }
                catch (Exception exception)
                {
                    CommandConsoleHelper.Print($"Failed to dump console, reason: {exception.Message}");
                }
            }
            else
            {
                var fileName = Path.Combine(Application.dataPath, $"consoledump_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.txt"); 
                
                File.WriteAllText(
                    fileName,
                    CommandConsoleBehaviour.Instance.GetCurrentContent());
                
                CommandConsoleHelper.Print($"Successfully dumped console content to:\n{fileName}");
            }
        }
    }
}