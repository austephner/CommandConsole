# Command Console
#### Summary
An in-game console API with easily implementable commands. New actionable commands are discovered at startup using C# Reflection, so no dependency injection or additional setup is required!

![Console Example](https://i.imgur.com/mVoAzjC.gif)

#### Features
- Easily implement new custom commands with C#
- Example console is fully functional and customizable
- Some default commands like "help", "echo", "clear", "dump"

#### Issues
The current implementation doesn't supporting pressing the "Enter" / "Return" key to submit a command. I'm too lazy to figure that one out. 

# Usage
### General Setup
1. Drag the example console prefab into the current scene
2. In play mode, use the configured keystroke settings to open/close the console (by default this is `Left Shift` and `Back Quote` pressed at the same time)
3. Type out a command then press click the "Submit" button

### Creating New Commands
1. Create a new C# file in the project
2. Import the `CommandConsole` namespace as well as `System.Collections.Generic`. 
3. Create a new class that inherits from `ConsoleCommand`
4. Implement all abstract members of the class
5. The command will automatically be picked up by the console via C# Reflection, no injection or other updates are needed!

```c#
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
```

### Creating a Custom Console
1. Create a new C# file in the project
2. Create a new class and have it inherit from `CommandConsoleBehaviour`
3. Implement all abstract functions
4. Here is the code to the example `BasicCommandConsole` class which uses standard Unity Input components

```c#
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CommandConsole.Examples
{
    public class BasicCommandConsole : CommandConsoleBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private RectTransform _outputContainer;

        [SerializeField] private ScrollRect _scrollRect;
        
        [SerializeField] private Text _output;

        [SerializeField] private InputField _inputField;

        public void OnSubmitButtonClicked()
        {
            Submit();
        }
        
        protected override void Submit()
        {
            HandleInput(_inputField.text);
            _inputField.text = ""; 
        }

        protected override void OnShow()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _inputField.ActivateInputField();
            _inputField.Select();
            EventSystem.current?.SetSelectedGameObject(_inputField.gameObject);
        }

        protected override void OnHide()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _inputField.DeactivateInputField();
        }

        public override void Print(string text)
        {
            _output.text += $"\n{text}\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate(_outputContainer);
            _scrollRect.normalizedPosition = new Vector2(0, 0);
        }

        public override void Clear()
        {
            _output.text = "";
            LayoutRebuilder.ForceRebuildLayoutImmediate(_outputContainer);
        }
    }
}
```