namespace PropertyRetriever.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IPropertyRetriever
    {
        #region Retrieve only from Environment - String

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        string RetrieveFromEnvironment(string variableName);

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Environment variable value or <paramref name="fallbackValue"/> if the environment variable is not found.</returns>
        string RetrieveFromEnvironment(string variableName, string fallbackValue);

        #endregion

        #region Retrieve only from Environment - Generic

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that the environment variable value will be converted.</typeparam>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
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
        /// <exception cref="ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        bool CheckFromCommandLine(string longName, char? shortName);

        #endregion

        #region Retrieve only from Command Line - String

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
        IEnumerable<string> RetrieveFromCommandLine(char shortName, params string[] fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        IEnumerable<string> RetrieveFromCommandLine(string longName);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        IEnumerable<string> RetrieveFromCommandLine(string longName, params string[] fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the conversion for string can not be processed.</exception>
        IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line.</returns>
        IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName, params string[] fallbackValue);

        #endregion

        #region Retrieve only from Command Line - Generic

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
        IEnumerable<T> RetrieveFromCommandLine<T>(char shortName, params T[] fallbackValue);

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
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName, params T[] fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are not provided.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the conversion for <typeparamref name="T"/> can not be processed.</exception>
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName, params T[] fallbackValue);

        #endregion

        #region Retrieve from Command Line then from Environment - String

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromCommandLineOrEnvironment(char shortName, string variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public string RetrieveFromCommandLineOrEnvironment(char shortName, string variableName, string fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromCommandLineOrEnvironment(string longName, string variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment.
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public string RetrieveFromCommandLineOrEnvironment(string longName, string variableName, string fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromCommandLineOrEnvironment(string longName, char? shortName, string variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public string RetrieveFromCommandLineOrEnvironment(string longName, char? shortName, string variableName, string fallbackValue);

        #endregion

        #region Retrieve from Command Line then from Environment - Generic

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public T RetrieveFromCommandLineOrEnvironment<T>(char shortName, string variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public T RetrieveFromCommandLineOrEnvironment<T>(char shortName, string variableName, T fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, string variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment.
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, string variableName, T fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, char? shortName, string variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, char? shortName, string variableName, T fallbackValue);

        #endregion

        #region Special cases

        /// <summary>
        /// Retrieve the name of the executing service (without extensions)
        /// </summary>
        /// <returns>Executing service name without extensions</returns>
        string RetrieveServiceName();

        #endregion
    }
}
