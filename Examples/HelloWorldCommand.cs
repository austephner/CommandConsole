using System.Collections.Generic;
using CommandConsole;

public class HelloWorldCommand : ConsoleCommand
{
    public override string[] GetNames()
    {
        // note that capitalization doesn't matter, the parse function ignores casing
        return new string[]
        {
            "HelloWorld",
            "hw"
        };
    }

    public override string GetHelp()
    {
        return "Prints out \"Hello World\" to the console.";
    }
    
    public override void Execute(List<string> parameters)
    {
        CommandConsoleBehaviour.Instance.Print("Hello World");
    }
}