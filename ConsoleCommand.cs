﻿using System.Collections.Generic;

namespace CommandConsole
{
    /// <summary>
    /// The abstract class used to create new executable console commands. Inherit from this class and it will
    /// automatically be found by the console system using Reflection.
    /// </summary>
    public abstract class ConsoleCommand
    {
        public ConsoleCommand() { }
        
        /// <summary>
        /// Gets a list of all names this console command should be recognized by.
        /// </summary>
        public abstract string[] GetNames();
        
        /// <summary>
        /// Gets a string with helpful information about this command.
        /// </summary>
        public abstract string GetHelp();
        
        /// <summary>
        /// Called when this command is executed by the console.
        /// </summary>
        /// <param name="parameters">All parameters from the console input which have been separated by spaces.</param>
        public abstract void Execute(List<string> parameters);
    }
}