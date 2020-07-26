namespace PropertyRetriever.CommandLine
{
    public static class PropertyRetrieverCommandLineExtensions
    {
        public static bool CheckPropertyFromCommandLine(this IPropertyRetriever propertyRetriever, string propertyName, string alternativeShortName = null)
        {
            return propertyRetriever.RetrievePropertyFromCommandLine<bool>(propertyName, alternativeShortName);
        }
    }
}
