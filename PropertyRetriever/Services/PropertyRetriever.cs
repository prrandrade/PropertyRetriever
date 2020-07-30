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

        #region Retrieve only from Environment - String values

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        public string RetrieveFromEnvironment(string variableName) => RetrieveFromEnvironment<string>(variableName);

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>Environment variable value or <paramref name="fallbackValue"/> if the environment variable is not found.</returns>
        public string RetrieveFromEnvironment(string variableName, string fallbackValue) => RetrieveFromEnvironment<string>(variableName, fallbackValue);

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
        public bool CheckFromCommandLine(char shortName) => CheckFromCommandLine(null, shortName);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="longName">Long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        public bool CheckFromCommandLine(string longName) => CheckFromCommandLine(longName, null);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="longName">Long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
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

        #region Retrieve only from Command Line - String values

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(char shortName) => RetrieveFromCommandLine<string>(null, shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(char shortName, IEnumerable<string> fallbackValue) => RetrieveFromCommandLine(null, shortName, fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="longName">Property short name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(string longName) => RetrieveFromCommandLine<string>(longName, shortName: null);

        /// <summary>
        /// Retrieve a list of values passed from the command line..
        /// </summary>
        /// <param name="longName">Property short name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line or <paramref name="fallbackValue"/> if nothing is found.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(string longName, IEnumerable<string> fallbackValue) => RetrieveFromCommandLine(longName, char.MinValue, fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are null.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for string can not be processed.</exception>
        public IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName) => RetrieveFromCommandLine<string>(longName, shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="longName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="shortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line.</returns>
        public IEnumerable<string> RetrieveFromCommandLine(string longName, char? shortName, IEnumerable<string> fallbackValue) => RetrieveFromCommandLine<string>(longName, shortName, fallbackValue);

        #endregion

        #region Retrieve only form Command Line - Generic Values

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(char shortName) => RetrieveFromCommandLine<T>(null, shortName);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(char shortName, IEnumerable<T> fallbackValue) => RetrieveFromCommandLine(null, shortName, fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName) => RetrieveFromCommandLine<T>(longName, shortName: null);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="fallbackValue">Fallback value if something goes wrong.</param>
        /// <returns>List of properties retrieved from command line converted to the specified type or <paramref name="fallbackValue"/> if the values are not found or can not be converted.</returns>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName, IEnumerable<T> fallbackValue) => RetrieveFromCommandLine(longName, null, fallbackValue);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="longName">Property long name (identified with the '--' prefix as a command line parameter).</param>
        /// <param name="shortName">Property short name (identified with the '-' prefix as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="longName"/> and <paramref name="shortName"/> are not provided.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for <typeparamref name="T"/> can not be processed.</exception>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName)
        {
            if (longName == null && shortName == null)
                throw new ArgumentException($"You need to supply a {nameof(longName)} and/or a {nameof(shortName)}.");

            var returnedList = new List<T>();

            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            for (var i = 0; i < commandArray.Count - 1; i++)
            {
                if (!string.IsNullOrEmpty(longName) && commandArray[i].ToLowerInvariant().Equals($"--{longName.ToLowerInvariant()}") || shortName != null && commandArray[i].ToLowerInvariant().Equals($"-{char.ToLowerInvariant(shortName.Value)}"))
                {
                    try
                    {
                        returnedList.Add((T)Convert.ChangeType(commandArray[i + 1], typeof(T), CultureInfo.InvariantCulture));
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
        public IEnumerable<T> RetrieveFromCommandLine<T>(string longName, char? shortName, IEnumerable<T> fallbackValue)
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
    }
}
