namespace PropertyRetriever.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public static class Extensions
    {
        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName)
        {
            return propertyRetriever.RetrievePropertyFromEnvironment<T>(propertyName);
        }

        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, T defaultFallbackValue)
        {
            try
            {
                return propertyRetriever.RetrievePropertyFromEnvironment<T>(propertyName);
            }
            catch
            {
                return defaultFallbackValue;
            }
        }

        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, IEnumerable<T> possibleValues)
        {
            var result = propertyRetriever.RetrievePropertyFromEnvironment<T>(propertyName);

            if (possibleValues == null)
                return result;

            if (!possibleValues.Contains(result))
                throw new InvalidOperationException($"The value retrieved for environment variable {propertyName} is not permitted.");

            return result;
        }

        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, IEnumerable<T> possibleValues, T defaultFallbackValue)
        {
            try
            {
                return propertyRetriever.RetrieveSimplePropertyFromEnvironment(propertyName, possibleValues);
            }
            catch
            {
                return defaultFallbackValue;
            }
        }
    }
}
