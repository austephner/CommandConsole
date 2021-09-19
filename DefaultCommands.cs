using System.Collections.Generic;

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
}