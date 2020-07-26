namespace PropertyRetriever.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class PropertyRetriever : IPropertyRetriever
    {
        private readonly ILocalEnvironment _localEnvironment;

        public PropertyRetriever(ILocalEnvironment localEnvironment)
        {
            _localEnvironment = localEnvironment;
        }

        public T RetrievePropertyFromEnvironment<T>(string propertyName)
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

            return finalValue;
        }

        public bool CheckPropertyFromCommandLine(string propertyLongName = null, string propertyShortName = null)
        {
            if (propertyLongName == null && propertyShortName == null)
                throw new ArgumentException("You need to supply a propertyLongName and/or a propertyShortName.");

            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();
            var numberOfLongNames = string.IsNullOrEmpty(propertyLongName) ? 0 : commandArray.Count(x => x.Equals($"--{propertyLongName}"));
            var numberOfShortNames = string.IsNullOrEmpty(propertyShortName) ? 0 : commandArray.Count(x => x.Equals($"--{propertyShortName}"));
            return numberOfLongNames + numberOfShortNames > 0;
        }

        public IEnumerable<T> RetrievePropertiesFromCommandLine<T>(string propertyLongName = null, string propertyShortName = null)
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
                        returnedList.Add((T)Convert.ChangeType(commandArray[i + 1], typeof(T)));
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
