namespace PropertyRetriever.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public static class Extensions
    {
        public static T RetrieveFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, T defaultFallbackValue)
        {
            try
            {
                return propertyRetriever.RetrieveFromEnvironment<T>(propertyName);
            }
            catch
            {
                return defaultFallbackValue;
            }
        }

        public static T RetrieveFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, IEnumerable<T> possibleValues)
        {
            var result = propertyRetriever.RetrieveFromEnvironment<T>(propertyName);

            if (possibleValues == null)
                return result;

            if (!possibleValues.Contains(result))
                throw new InvalidOperationException($"The value retrieved for environment variable {propertyName} is not permitted.");

            return result;
        }

        public static T RetrieveFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, IEnumerable<T> possibleValues, T defaultFallbackValue)
        {
            try
            {
                return propertyRetriever.RetrieveFromEnvironment(propertyName, possibleValues);
            }
            catch
            {
                return defaultFallbackValue;
            }
        }
    }
}
