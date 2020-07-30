namespace PropertyRetriever.Interfaces
{
    using System.Collections.Generic;

    public interface IPropertyRetriever
    {
        #region Retrieve only from Environment - String Values

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        string RetrieveFromEnvironment(string variableName);
        
        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Environment variable value or <paramref name="fallbackValue"/> if the environment variable is not found.</returns>
        string RetrieveFromEnvironment(string variableName, string fallbackValue);

        #endregion

        #region Retrieve only from Environment - Generic values

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that the environment variable value will be converted.</typeparam>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        T RetrieveFromEnvironment<T>(string variableName);

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that the environment variable value will be converted.</typeparam>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Environment variable value converted to <typeparamref name="T"/> or <paramref name="fallbackValue"/> if the environment variable is not found or can not be converted.</returns>
        T RetrieveFromEnvironment<T>(string variableName, T fallbackValue);

        #endregion

        #region Check from Command Line

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="shortName">Short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        bool CheckFromCommandLine(char shortName);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="longName">Long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        bool CheckFromCommandLine(string longName);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="longName">Long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        bool CheckFromCommandLine(string longName, char? shortName);

        #endregion

        #region Retrieve only from Command Line - String Values

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        IEnumerable<string> RetrieveFromCommandLine(char shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        IEnumerable<string> RetrieveFromCommandLine(char shortName, IEnumerable<string> fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="longName">Property short name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        IEnumerable<string> RetrieveFromCommandLine(string longName);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="longName">Property short name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        IEnumerable<string> RetrieveFromCommandLine(string longName, IEnumerable<string> fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for string can not be processed.</exception>
        IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line.</returns>
        IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName, IEnumerable<string> fallbackValue);

        #endregion

        #region Retrieve only from Command Line - Generic Values

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        IEnumerable<T> RetrieveFromCommandLine<T>(char shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        IEnumerable<T> RetrieveFromCommandLine<T>(char shortName, IEnumerable<T> fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName, IEnumerable<T> fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are not provided.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for <typeparamref name="T"/> can not be processed.</exception>
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName);
        
        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName, IEnumerable<T> fallbackValue);

        #endregion
    }
}
