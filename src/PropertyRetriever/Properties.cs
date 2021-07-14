namespace PropertyRetriever
{
    using System;
    using System.Globalization;
    using System.Linq;

    public static class Properties
    {
        public static T RetrieveProperty<T>(string propertyName, params string[] otherNames)
        {
            // command line
            var commandLine = Environment.GetCommandLineArgs();  
            var indexCommandLine = Array.FindIndex(commandLine, s => s == propertyName || otherNames != null && otherNames.Contains(s));
            if (indexCommandLine != -1 && indexCommandLine < commandLine.Length - 1)
                return (T)Convert.ChangeType(commandLine[indexCommandLine + 1], typeof(T), CultureInfo.InvariantCulture);
                    
            // environment
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(propertyName)))
                return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(propertyName), typeof(T), CultureInfo.InvariantCulture);

            return default;
        }
    }
}
