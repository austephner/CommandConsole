namespace CommandConsole
{
    /// <summary>
    /// Static reference class for the <see cref="CommandConsoleBehaviour"/>, provides shorthand access.
    /// </summary>
    public static class CommandConsoleHelper
    {
        /// <summary>
        /// Shows the console.
        /// </summary>
        public static void Show() => CommandConsoleBehaviour.Instance.Show();
        
        /// <summary>
        /// Hides the console.
        /// </summary>
        public static void Hide() => CommandConsoleBehaviour.Instance.Hide();
        
        /// <summary>
        /// Prints the given text to the console.
        /// </summary>
        /// <param name="text">The text to print.</param>
        public static void Print(string text) => CommandConsoleBehaviour.Instance.Print(text);
        
        /// <summary>
        /// Clears the console.
        /// </summary>
        public static void Clear() => CommandConsoleBehaviour.Instance.Clear();
    }
}