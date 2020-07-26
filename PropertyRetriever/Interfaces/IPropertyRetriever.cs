namespace PropertyRetriever.Interfaces
{
    using System.Collections.Generic;

    public interface IPropertyRetriever
    {
        T RetrievePropertyFromEnvironment<T>(string propertyName);

        bool CheckPropertyFromCommandLine(string propertyLongName = null, string propertyShortName = null);

        IEnumerable<T> RetrievePropertiesFromCommandLine<T>(string propertyLongName = null, string propertyShortName = null);
    }
}
