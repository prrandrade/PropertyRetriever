namespace PropertyRetriever
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public interface IPropertyRetriever
    {
        T RetrievePropertyFromEnvironment<T>(string propertyName, IEnumerable<T> possibleValues);

        bool CheckPropertyFromCommandLine(string propertyName, string alternativeShortName = null);

        T RetrievePropertyFromCommandLine<T>(string propertyName, string alternativeShortName = null);
    }

    public class PropertyRetriever : IPropertyRetriever
    {
        private readonly ILocalEnvironment _localEnvironment;

        public PropertyRetriever(ILocalEnvironment localEnvironment)
        {
            _localEnvironment = localEnvironment;
        }

        public T RetrievePropertyFromEnvironment<T>(string propertyName, IEnumerable<T> possibleValues)
        {
            var environmentValue = _localEnvironment.GetEnvironmentVariable(propertyName);
            T finalValue;

            try
            {
                finalValue = (T)Convert.ChangeType(environmentValue, typeof(T));
            }
            catch
            {
                throw new InvalidOperationException($"The environment variable {propertyName} found could not be converted.");
            }

            if (possibleValues == null || possibleValues.Contains(finalValue))
                return finalValue;
            else
                throw new InvalidOperationException($"The environment variable is not present on the {nameof(possibleValues)} list.");
        }

        public bool CheckPropertyFromCommandLine(string propertyName, string alternativeShortName = null)
        {
            try
            {
                return ContainsCommandLinePropertyName(propertyName, alternativeShortName);
            }
            catch
            {
                throw new InvalidOperationException($"The environment variable {propertyName} found could not be converted.");
            }
        }

        public T RetrievePropertyFromCommandLine<T>(string propertyName, string alternativeShortName)
        {
            return RetrieveIndividualCommandLineValue<T>(propertyName, alternativeShortName);
        }

        #region Private methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ContainsCommandLinePropertyName(string propertyName, string alternativeShortName, int numberOfAllowedProperties = 1)
        {
            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            var numberOfPropertiesNames = string.IsNullOrEmpty(propertyName) ? 0 : commandArray.Count(x => x.Equals($"--{propertyName}"));
            var numberOfAlternativeShortNames = string.IsNullOrEmpty(alternativeShortName) ? 0 : commandArray.Count(x => x.Equals($"--{alternativeShortName}"));
            return numberOfPropertiesNames + numberOfAlternativeShortNames == numberOfAllowedProperties;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T RetrieveIndividualCommandLineValue<T>(string propertyName, string alternativeShortName)
        {
            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            for (var i = 0; i < commandArray.Count - 1; i++)
            {
                if (commandArray[i].Equals($"--{propertyName}") || !string.IsNullOrEmpty(alternativeShortName) && commandArray[i].Equals($"-{alternativeShortName}"))
                {
                    try
                    {
                        return (T) Convert.ChangeType(commandArray[i + 1], typeof(T));
                    }
                    catch
                    {
                        throw new InvalidOperationException($"Error while converting value found for property with name {propertyName}{(alternativeShortName != null ? $"or {alternativeShortName}." : "")}");
                    }
                }
            }
            throw new ArgumentException($"No property was found with name {propertyName}{(alternativeShortName != null ? $"or {alternativeShortName}." : "")}");
        }

        #endregion
    }
}
