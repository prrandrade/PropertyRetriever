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
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="propertyLongName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="propertyShortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="propertyLongName"/> and <paramref name="propertyShortName"/> are not provided.</exception>
        public bool CheckFromCommandLine(string propertyLongName = null, string propertyShortName = null)
        {
            if (propertyLongName == null && propertyShortName == null)
                throw new ArgumentException("You need to supply a propertyLongName and/or a propertyShortName.");

            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            var numberOfLongNames = string.IsNullOrEmpty(propertyLongName) ? 0 : commandArray.Count(x => x.Equals($"--{propertyLongName}"));
            var numberOfShortNames = string.IsNullOrEmpty(propertyShortName) ? 0 : commandArray.Count(x => x.Equals($"-{propertyShortName}"));
            return numberOfLongNames + numberOfShortNames > 0;
        }

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="propertyLongName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="propertyShortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="propertyLongName"/> and <paramref name="propertyShortName"/> are not provided.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for <typeparamref name="T"/> can not be processed.</exception>
        public IEnumerable<T> RetrieveFromCommandLine<T>(string propertyLongName = null, string propertyShortName = null)
        {
            if (propertyLongName == null && propertyShortName == null)
                throw new ArgumentException("You need to supply a propertyLongName and/or a propertyShortName.");

            var returnedList = new List<T>();

            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            for (var i = 0; i < commandArray.Count - 1; i++)
            {
                if (!string.IsNullOrEmpty(propertyLongName) && commandArray[i].Equals($"--{propertyLongName}") || !string.IsNullOrEmpty(propertyShortName) && commandArray[i].Equals($"-{propertyShortName}"))
                {
                    try
                    {
                        returnedList.Add((T)Convert.ChangeType(commandArray[i + 1], typeof(T), CultureInfo.InvariantCulture));
                    }
                    catch
                    {
                        if (propertyLongName != null && propertyShortName == null)
                            throw new InvalidOperationException($"Error while converting value found for property with name {propertyLongName}.");
                        if (propertyLongName == null)
                            throw  new InvalidOperationException($"Error while converting value found for property with name {propertyShortName}.");
                        throw new InvalidOperationException($"Error while converting value found for property with name {propertyLongName}/{propertyShortName}.");
                    }
                }
            }

            return returnedList;
        }
    }
}
