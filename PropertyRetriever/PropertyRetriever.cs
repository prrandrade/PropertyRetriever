namespace PropertyRetriever
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public interface IPropertyRetriever
    {
        #region Simple properties from command line OR environment

        /// <summary>
        /// Retrieve a property on command line or environment given a name
        /// </summary>
        /// <typeparam name="T">Type of property that will be retrived (a conversion will be tried)</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value found on command line or environment</returns>
        /// <exception cref="InvalidOperationException">Thrown if no property is found or the type conversion is invalid</exception>
        T RetrieveSimpleProperty<T>(string propertyName);

        /// <summary>
        /// Retrieve a property on command line or environment given a name
        /// </summary>
        /// <typeparam name="T">Type of property that will be retrived (a conversion will be tried)</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="defaultValue">If no property is found or the type conversion is invalid, this default value will be returned</param>
        /// <returns>Property value found on command line or environment</returns>
        T RetrieveSimpleProperty<T>(string propertyName, T defaultValue);

        /// <summary>
        /// Retrieve a property on command line or environment given a name
        /// </summary>
        /// <typeparam name="T">Type of property that will be retrived (a conversion will be tried)</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="possibleLimitedValues">List of expected values that this property can be</param>
        /// <param name="defaultValue">If no property is found or the type conversion is invalid or the property value is not expected, this default value will be returned</param>
        /// <returns>Property value found on command line or environment</returns>
        T RetrieveSimpleProperty<T>(string propertyName, T[] possibleLimitedValues, T defaultValue);

        /// <summary>
        /// Retrieve a property on command line or environment given a name
        /// </summary>
        /// <typeparam name="T">Type of property that will be retrived (a conversion will be tried)</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="alternativeShortCommandName">Alternative short property name</param>
        /// <returns>Property value found on command line or environment</returns>
        /// <exception cref="InvalidOperationException">Thrown if no property is found or the type conversion is invalid</exception>
        T RetrieveSimpleProperty<T>(string propertyName, string alternativeShortCommandName);

        /// <summary>
        /// Retrieve a property on command line or environment given a name
        /// </summary>
        /// <typeparam name="T">Type of property that will be retrived (a conversion will be tried)</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="alternativeShortCommandName">Alternative short property name</param>
        /// <param name="defaultValue">If no property is found or the type conversion is invalid, this default value will be returned</param>
        /// <returns>Property value found on command line or environment</returns>
        T RetrieveSimpleProperty<T>(string propertyName, string alternativeShortCommandName, T defaultValue);

        /// <summary>
        /// Retrieve a property on command line or environment given a name
        /// </summary>
        /// <typeparam name="T">Type of property that will be retrived (a conversion will be tried)</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="alternativeShortCommandName">Alterenative short property name</param>
        /// <param name="possibleLimitedValues">List of expected values that this property can be</param>
        /// <param name="defaultValue">If no property is found or the type conversion is invalid or the property value is not expected, this default value will be returned</param>
        /// <returns>Property value found on command line or environment</returns>
        T RetrieveSimpleProperty<T>(string propertyName, string alternativeShortCommandName, T[] possibleLimitedValues, T defaultValue);
        
        #endregion

        #region Boolean Properties from command line

        bool RetrieveBooleanPropertyFromCommandLine(string propertyName);

        bool RetrieveBooleanPropertyFromCommandLine(string propertyName, string alternativeShortCommandName);

        #endregion

        #region Simple properties just from command line

        T RetrieveSimplePropertyFromCommandLine<T>(string propertyName);

        T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, T defaultValue);

        T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, T[] possibleValues, T defaultValue);

        T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName);

        T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, T defaultValue);

        T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, T[] possibleValues, T defaultValue);

        #endregion

        #region List properties from command line OR environment

        IEnumerable<T> RetrieveListProperty<T>(string propertyName, char separator = ';');

        IEnumerable<T> RetrieveListProperty<T>(string propertyName, IEnumerable<T> defaultValue, char separator = ';');

        IEnumerable<T> RetrieveListProperty<T>(string propertyName, string alternativeShortCommandName, char separator = ';');

        IEnumerable<T> RetrieveListProperty<T>(string propertyName, string alternativeShortCommandName, IEnumerable<T> defaultValue, char separator = ';');

        #endregion

        #region List properties just from command line

        IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, char separator = ';');

        IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, IEnumerable<T> defaultValue, char separator = ';');

        IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, char separator = ';');

        IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, IEnumerable<T> defaultValue, char separator = ';');

        #endregion
    }

    public class PropertyRetriever : IPropertyRetriever
    {
        private readonly ILocalEnvironment _localEnvironment;

        public PropertyRetriever(ILocalEnvironment localEnvironment)
        {
            _localEnvironment = localEnvironment;
        }

        #region Simple properties from command line OR environment

        public T RetrieveSimpleProperty<T>(string propertyName)
        {
            return RetrieveProperty<T>(propertyName, null, null, true);
        }

        public T RetrieveSimpleProperty<T>(string propertyName, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, null, null, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        public T RetrieveSimpleProperty<T>(string propertyName, T[] possibleValues, T defaultValue)
        {
            try
            {
                return RetrieveProperty(propertyName, null, possibleValues, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        public T RetrieveSimpleProperty<T>(string propertyName, string alternativeShortCommandName)
        {
            return RetrieveProperty<T>(propertyName, alternativeShortCommandName, null, true);
        }

        public T RetrieveSimpleProperty<T>(string propertyName, string alternativeShortCommandName, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, alternativeShortCommandName, null, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        public T RetrieveSimpleProperty<T>(string propertyName, string alternativeShortCommandName, T[] possibleValues, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, alternativeShortCommandName, possibleValues, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region Boolean Properties from command line

        public bool RetrieveBooleanPropertyFromCommandLine(string propertyName)
        {
            return ContainsCommandLineValue(propertyName, null);
        }

        public bool RetrieveBooleanPropertyFromCommandLine(string propertyName, string alternativeShortCommandName)
        {
            return ContainsCommandLineValue(propertyName, alternativeShortCommandName);
        }

        #endregion

        #region Simple properties just from command line

        public T RetrieveSimplePropertyFromCommandLine<T>(string propertyName)
        {
            return RetrieveProperty<T>(propertyName, null, null, false);
        }

        public T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, null, null, false);
            }
            catch
            {
                return defaultValue;
            }
        }

        public T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, T[] possibleValues, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, null, possibleValues, false);
            }
            catch
            {
                return defaultValue;
            }
        }

        public T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName)
        {
            return RetrieveProperty<T>(propertyName, alternativeShortCommandName, null, false);
        }

        public T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, alternativeShortCommandName, null, false);
            }
            catch
            {
                return defaultValue;
            }
        }

        public T RetrieveSimplePropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, T[] possibleValues, T defaultValue)
        {
            try
            {
                return RetrieveProperty<T>(propertyName, alternativeShortCommandName, possibleValues, false);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region List properties from command line OR environment

        public IEnumerable<T> RetrieveListProperty<T>(string propertyName, char separator = ';')
        {
            return RetrieveProperties<T>(propertyName, null, true, separator);
        }

        public IEnumerable<T> RetrieveListProperty<T>(string propertyName, IEnumerable<T> defaultValue, char separator = ';')
        {
            try
            {
                return RetrieveProperties<T>(propertyName, null, true, separator);
            }
            catch
            {
                return defaultValue;
            }
        }

        public IEnumerable<T> RetrieveListProperty<T>(string propertyName, string alternativeShortCommandName, char separator = ';')
        {
            return RetrieveProperties<T>(propertyName, alternativeShortCommandName, true, separator);
        }

        public IEnumerable<T> RetrieveListProperty<T>(string propertyName, string alternativeShortCommandName, IEnumerable<T> defaultValue, char separator = ';')
        {
            try
            {
                return RetrieveProperties<T>(propertyName, alternativeShortCommandName, true, separator);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region List properties just from command line

        public IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, char separator = ';')
        {
            return RetrieveProperties<T>(propertyName, null, false, separator);
        }

        public IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, IEnumerable<T> defaultValue, char separator = ';')
        {
            try
            {
                return RetrieveProperties<T>(propertyName, null, false, separator);
            }
            catch
            {
                return defaultValue;
            }
        }

        public IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, char separator = ';')
        {
            return RetrieveProperties<T>(propertyName, alternativeShortCommandName, false, separator);
        }

        public IEnumerable<T> RetrieveListPropertyFromCommandLine<T>(string propertyName, string alternativeShortCommandName, IEnumerable<T> defaultValue, char separator = ';')
        {
            try
            {
                return RetrieveProperties<T>(propertyName, alternativeShortCommandName, false, separator);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region Private methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T RetrieveProperty<T>(string propertyName, string alternativeShortCommandName, T[] possibleValues, bool alsoOnEnvironment)
        {
            // retrieving command line argument
            var commandLineArg = RetrieveCommandLineValue(propertyName, alternativeShortCommandName);
            if (commandLineArg != null)
            {
                try
                {
                    var value = (T)Convert.ChangeType(commandLineArg, typeof(T));
                    if (possibleValues == null || possibleValues.Contains(value))
                        return value;
                    else
                        throw new InvalidOperationException();
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }

            // retrieving environment variable
            if (alsoOnEnvironment)
            {
                var environmentValue = RetrieveEnvironmentValue(propertyName, alternativeShortCommandName);
                if (environmentValue != null)
                {
                    try
                    {
                        var value = (T)Convert.ChangeType(environmentValue, typeof(T));
                        if (possibleValues == null || possibleValues.Contains(value))
                            return value;
                        else
                            throw new InvalidOperationException();
                    }
                    catch
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<T> RetrieveProperties<T>(string propertyName, string alternativeShortCommandName, bool alsoOnEnvironment, char separator)
        {
            // retrieving command line argument
            var commandLineArg = RetrieveCommandLineValue(propertyName, alternativeShortCommandName);
            if (commandLineArg != null)
            {
                try
                {
                    return commandLineArg
                        .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => (T) Convert.ChangeType(x, typeof(T))).ToList();
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }

            // retrieving environment variable
            if (alsoOnEnvironment)
            {
                var environmentValue = RetrieveEnvironmentValue(propertyName, alternativeShortCommandName);
                if (environmentValue != null)
                {
                    try
                    {
                        return environmentValue
                            .Split(separator, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => (T)Convert.ChangeType(x, typeof(T))).ToList();
                    }
                    catch
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            throw new InvalidOperationException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ContainsCommandLineValue(string propertyName, string alternativePropertyShortName)
        {
            var containsPropName = !string.IsNullOrEmpty(propertyName) 
                                   && _localEnvironment.GetCommandLineArgs().Contains($"--{propertyName}");

            var containsShortPropName = !string.IsNullOrEmpty(alternativePropertyShortName)
                                    && _localEnvironment.GetCommandLineArgs().Contains($"-{alternativePropertyShortName}");

            return containsPropName || containsShortPropName;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string RetrieveCommandLineValue(string propertyName, string alternativeShortName)
        {
            var commandArray = _localEnvironment.GetCommandLineArgs().ToList();

            var commandLinePosition = commandArray.FindIndex(x => x.StartsWith($"--{propertyName}"));
            if (commandLinePosition > 0 && commandLinePosition < commandArray.Count)
                return commandArray[commandLinePosition + 1];

            if (!string.IsNullOrEmpty(alternativeShortName))
            {
                commandLinePosition = commandArray.FindIndex(x => x.StartsWith($"-{alternativeShortName}"));
                if (commandLinePosition > 0 && commandLinePosition < commandArray.Count)
                    return commandArray[commandLinePosition + 1];
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string RetrieveEnvironmentValue(string propertyName, string alternativeShortName)
        {
            var valueGivenProperytyName = propertyName != null ? _localEnvironment.GetEnvironmentVariable(propertyName) : null;
            var valueGivenAlternativeShortName = alternativeShortName != null ? _localEnvironment.GetEnvironmentVariable(alternativeShortName) : null;

            return valueGivenProperytyName ?? valueGivenAlternativeShortName;
        }

        #endregion
    }
}
