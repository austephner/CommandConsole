using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CommandConsole
{
    public abstract class CommandConsoleBehaviour : MonoBehaviour
    {
        public const string INVALID_CMD_MSG = "<color=#FF0000FF>Command \"{0}\" does not exist.</color>";
        
        #region Settings

        [SerializeField] private bool _dontDestroyOnLoad;

        [SerializeField] private KeyCode
            _toggleKeyPrimary = KeyCode.LeftShift,
            _toggleKeySecondary = KeyCode.BackQuote;
        
        #endregion

        #region Properties
        
        public bool open => _open;
        
        public static CommandConsoleBehaviour Instance { get; private set; }

        #endregion
        
        #region Private & Protected Fields
        
        private bool _open;

        protected List<ConsoleCommand> consoleCommands;
        
        #endregion

        #region Unity Events
        
        protected virtual void OnEnable()
        {
            if (Instance && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            if (_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            Instance = this;
            
            Clear();

            consoleCommands = new List<ConsoleCommand>();

            var derivedTypes = GetDerivedTypes(typeof(ConsoleCommand));
            
            foreach (var derivedType in derivedTypes)
            {
                try
                {
                    consoleCommands.Add((ConsoleCommand) Activator.CreateInstance(derivedType));
                }
                catch (Exception exception)
                {
                    Debug.LogError($"Failed to add console command for derived type: \"{derivedType}\", reason: {exception.Message}");
                }
            }
        }

        protected virtual void Update()
        {
            if (Input.GetKey(_toggleKeyPrimary) && Input.GetKeyDown(_toggleKeySecondary))
            {
                Toggle();
            }
        }

        #endregion

        #region Protected Utilities

        /// <summary>
        /// Handles the given input, invoking commands as needed with parameters.
        /// </summary>
        /// <param name="rawInput">The uninterpreted user input.</param>
        protected void HandleInput(string rawInput)
        {
            var input = rawInput.Trim().Split(' ');
            
            if (input.Length > 0)
            {
                var commandText = input.First();
                var parameters = new List<string>();

                for (int i = 1; i < input.Length; i++)
                {
                    parameters.Add(input[i]);
                }

                var command = GetCommand(commandText);

                if (command != null)
                {
                    Print($"<color=#999999FF>{rawInput}</color>");
                    command.Execute(parameters);
                }
                else
                {
                    Print(string.Format(INVALID_CMD_MSG, commandText));
                }
            }
            else
            {
                Print("");
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Processes the current console input as commands.
        /// </summary>
        protected abstract void Submit();
        
        /// <summary>
        /// Invoked when <see cref="Show"/> is called.
        /// </summary>
        protected abstract void OnShow();
        
        /// <summary>
        /// Invoked when <see cref="Hide"/> is called.
        /// </summary>
        protected abstract void OnHide();

        /// <summary>
        /// Prints the given text.
        /// </summary>
        /// <param name="text">The text to print.</param>
        public abstract void Print(string text);

        /// <summary>
        /// Clears the console of all text.
        /// </summary>
        public abstract void Clear();
        
        #endregion
        
        #region Public Utilities

        /// <summary>
        /// Switches between hidden and shown depending on the current state.
        /// </summary>
        public void Toggle()
        {
            if (_open)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        /// <summary>
        /// Marks the console as open and invokes <see cref="OnShow"/>.
        /// </summary>
        public void Show()
        {
            _open = true;
            OnShow();
        }
        
        /// <summary>
        /// Marks the console as closed and invokes <see cref="OnHide"/>.
        /// </summary>
        public void Hide()
        {
            _open = false;
            OnHide();
        }
        
        #endregion
        
        #region Private Utilities
        
        /// <summary>
        /// Gets types derived from the given type.
        /// </summary>
        /// <param name="type">The parent type.</param>
        /// <returns>All ancestral types.</returns>
        private Type[] GetDerivedTypes(Type type)
        {
            var result = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var allTypes = assembly.GetTypes();
                foreach (var givenType in allTypes)
                {
                    if (givenType.IsSubclassOf(type) && !givenType.IsAbstract)
                    {
                        result.Add(givenType);
                    }
                }
            }
            return result.ToArray();
        }

        #endregion

        #region Static Functions
       
        /// <summary>
        /// Gets a command matching the given name.
        /// </summary>
        /// <param name="name">Name of the command to get.</param>
        /// <returns>A console command matching the given name.</returns>
        internal static ConsoleCommand GetCommand(string name)
        {
            var formattedName = name.ToLower();
             
            foreach (var command in Instance.consoleCommands)
            {
                foreach (var commandName in command.GetNames())
                {
                    var formattedCommandName = commandName.ToLower();
                     
                    if (formattedCommandName == formattedName)
                    {
                        return command;
                    }
                }
            }
    
            return null;
        }
    
        /// <summary>
        /// Gets a list of all console commands loaded by the instance.
        /// </summary>
        /// <returns></returns>
        internal static List<ConsoleCommand> GetConsoleCommands() => Instance.consoleCommands;
        
        #endregion
    }
}