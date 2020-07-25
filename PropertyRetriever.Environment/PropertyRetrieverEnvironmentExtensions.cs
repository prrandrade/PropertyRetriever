namespace PropertyRetriever.Environment
{
    using System.Collections.Generic;

    public static class PropertyRetrieverEnvironmentExtensions
    {
        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName)
        {
            return propertyRetriever.RetrievePropertyFromEnvironment<T>(propertyName, null);
        }

        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, T defaultFallbackValue)
        {
            try
            {
                return propertyRetriever.RetrievePropertyFromEnvironment<T>(propertyName, null);
            }
            catch
            {
                return defaultFallbackValue;
            }
        }

        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, IEnumerable<T> possibleValues)
        {
            return propertyRetriever.RetrievePropertyFromEnvironment(propertyName, possibleValues);
        }

        public static T RetrieveSimplePropertyFromEnvironment<T>(this IPropertyRetriever propertyRetriever, string propertyName, IEnumerable<T> possibleValues, T defaultFallbackValue)
        {
            try
            {
                return propertyRetriever.RetrievePropertyFromEnvironment<T>(propertyName, possibleValues);
            }
            catch
            {
                return defaultFallbackValue;
            }
        }
    }
}
