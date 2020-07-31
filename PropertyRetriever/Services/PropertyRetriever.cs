namespace PropertyRetriever.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Interfaces;

    public class PropertyRetriever : IPropertyRetriever
    {
        private readonly ILocalEnvironment _localEnvironment;

        public PropertyRetriever(ILocalEnvironment localEnvironment)
        {
            _localEnvironment = localEnvironment;
        }

        #region Retrieve only from Environment - String

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromEnvironment(string variableName)
            => RetrieveFromEnvironment<string>(variableName);

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Environment variable value or <paramref name="fallbackValue"/> if the environment variable is not found.</returns>
        public string RetrieveFromEnvironment(string variableName, string fallbackValue)
            => RetrieveFromEnvironment<string>(variableName, fallbackValue);

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
        public T RetrieveFromEnvironment<T>(string variableName)
        {
            var environmentValue = _localEnvironment.GetEnvironmentVariable(variableName);
            T finalValue;

            try
            {
                finalValue = (T)Convert.ChangeType(environmentValue, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                throw new InvalidOperationException($"The environment variable {variableName} was found, but could not be converted.");
            }

            return finalValue;
        }

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that the environment variable value will be converted.</typeparam>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Environment variable value converted to <typeparamref name="T"/> or <paramref name="fallbackValue"/> if the environment variable is not found or can not be converted.</returns>
        public T RetrieveFromEnvironment<T>(string variableName, T fallbackValue)
        {
            try
            {
                return RetrieveFromEnvironment<T>(variableName);
            }
            catch
            {
                return fallbackValue;
            }
        }

        #endregion

        #region Check from Command Line

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="shortName">Short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        public bool CheckFromCommandLine(char shortName)
            => CheckFromCommandLine(null, shortName);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="longName">Long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        public bool CheckFromCommandLine(string longName)
            => CheckFromCommandLine(longName, null);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="longName">Long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        public bool CheckFromCommandLine(string longName, char? shortName)
        {
            if (longName == null && shortName == null)
                throw new ArgumentException($"You need to supply a {nameof(longName)} and/or a {nameof(shortName)}.");

            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            var commandsWithShortPrefix = commandArray.Where(x => x.StartsWith("-") && !x.StartsWith("--"));
            var commandsWithLongPrefix = commandArray.Where(x => x.StartsWith("--"));
            var numberOfLongNames = string.IsNullOrEmpty(longName) ? 0 : commandsWithLongPrefix.Count(y => y.ToLower().Equals($"--{longName.ToLower()}"));
            var numberOfShortNames = shortName == null ? 0 : commandsWithShortPrefix.Count(x => x.ToLower().Contains(char.ToLower(shortName.Value, CultureInfo.InvariantCulture)));
            return numberOfLongNames + numberOfShortNames > 0;
        }

        #endregion

        #region Retrieve only from Command Line - String

        /// <summary>
        /// Retrieve a list of values passed from the command line.
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(char shortName)
            => RetrieveFromCommandLine<string>(null, shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(char shortName, params string[] fallbackValue)
            => RetrieveFromCommandLine(null, shortName, fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line.
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(string longName)
            => RetrieveFromCommandLine<string>(longName, shortName: null);

        /// <summary>
        /// Retrieve a list of values passed from the command line.
        /// </summary>
        /// <param name="longName">Property short name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(string longName, params string[] fallbackValue)
            => RetrieveFromCommandLine(longName, null, fallbackValue: fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        public IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName)
            => RetrieveFromCommandLine<string>(longName, shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName, params string[] fallbackValue)
            => RetrieveFromCommandLine<string>(longName, shortName, fallbackValue);

        #endregion

        #region Retrieve only form Command Line - Generic

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(char shortName)
            => RetrieveFromCommandLine<T>(null, shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(char shortName, params T[] fallbackValue)
            => RetrieveFromCommandLine(null, shortName, fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName)
            => RetrieveFromCommandLine<T>(longName, shortName: null);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName, params T[] fallbackValue)
            => RetrieveFromCommandLine(longName, null, fallbackValue: fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are not provided.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the conversion for <typeparamref name="T"/> can not be processed.</exception>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName)
        {
            if (longName == null && shortName == null)
                throw new ArgumentException($"You need to supply a {nameof(longName)} and/or a {nameof(shortName)}.");

            var returnedList = new List<T>();

            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            for (var i = 0; i < commandArray.Count - 1; i++)
            {
                try
                {
                    if (!string.IsNullOrEmpty(longName) && commandArray[i].ToLowerInvariant().Equals($"--{longName.ToLowerInvariant()}"))
                    {
                        returnedList.Add((T)Convert.ChangeType(commandArray[i + 1], typeof(T), CultureInfo.InvariantCulture));
                    }
                    else if (shortName != null && commandArray[i].ToLowerInvariant().Equals($"-{char.ToLowerInvariant(shortName.Value)}"))
                    {
                        returnedList.Add((T)Convert.ChangeType(commandArray[i + 1], typeof(T), CultureInfo.InvariantCulture));
                    }
                }
                catch
                {
                    if (longName != null && shortName == null)
                        throw new InvalidOperationException($"Error while converting value found for property with name {longName}.");
                    if (longName == null)
                        throw new InvalidOperationException($"Error while converting value found for property with name {shortName}.");
                    throw new InvalidOperationException($"Error while converting value found for property with name {longName}/{shortName}.");
                }
            }

            return returnedList;
        }

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName, params T[] fallbackValue)
        {
            try
            {
                var values = RetrieveFromCommandLine<T>(longName, shortName);
                return values.Any()
                    ? values
                    : fallbackValue;
            }
            catch
            {
                return fallbackValue;
            }
        }

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
        public string RetrieveFromCommandLineOrEnvironment(char shortName, string variableName)
            => RetrieveFromCommandLineOrEnvironment<string>(null, shortName, variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public string RetrieveFromCommandLineOrEnvironment(char shortName, string variableName, string fallbackValue)
            => RetrieveFromCommandLineOrEnvironment<string>(null, shortName, variableName, fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromCommandLineOrEnvironment(string longName, string variableName)
            => RetrieveFromCommandLineOrEnvironment<string>(longName, shortName: null, variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment.
        /// </summary>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public string RetrieveFromCommandLineOrEnvironment(string longName, string variableName, string fallbackValue)
            => RetrieveFromCommandLineOrEnvironment<string>(longName, null, variableName, fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromCommandLineOrEnvironment(string longName, char? shortName, string variableName)
            => RetrieveFromCommandLineOrEnvironment<string>(longName, shortName, variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public string RetrieveFromCommandLineOrEnvironment(string longName, char? shortName, string variableName, string fallbackValue)
            => RetrieveFromCommandLineOrEnvironment<string>(longName, shortName, variableName, fallbackValue);

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
        public T RetrieveFromCommandLineOrEnvironment<T>(char shortName, string variableName)
            => RetrieveFromCommandLineOrEnvironment<T>(null, shortName, variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public T RetrieveFromCommandLineOrEnvironment<T>(char shortName, string variableName, T fallbackValue)
            => RetrieveFromCommandLineOrEnvironment(null, shortName, variableName, fallbackValue);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, string variableName)
            => RetrieveFromCommandLineOrEnvironment<T>(longName, null, variableName);

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment.
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, string variableName, T fallbackValue)
            => RetrieveFromCommandLineOrEnvironment(longName, null, variableName, fallbackValue);

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
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, char? shortName, string variableName)
        {
            try
            {
                var commandValues = RetrieveFromCommandLine<T>(longName, shortName).ToList();
                if (commandValues.Any()) return commandValues[0];
                throw new Exception();
            }
            catch
            {
                return RetrieveFromEnvironment<T>(variableName);
            }
        }

        /// <summary>
        /// Tries to retrieve a property from the command line or from the environment as a fallback
        /// </summary>
        /// <typeparam name="T">Desired type that the retrieved value will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Value retrieved from command line or environment converted to <typeparamref name="T"/>, or <paramref name="fallbackValue"/> if neither can be retrieved.</returns>
        public T RetrieveFromCommandLineOrEnvironment<T>(string longName, char? shortName, string variableName, T fallbackValue)
        {
            try
            {
                return RetrieveFromCommandLineOrEnvironment<T>(longName, shortName, variableName);
            }
            catch
            {
                return fallbackValue;
            }
        }

        #endregion
    }
}
