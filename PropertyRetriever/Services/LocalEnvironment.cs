namespace PropertyRetriever.Services
{
    using System;
    using Interfaces;

    public class LocalEnvironment : ILocalEnvironment
    {
        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        public string GetEnvironmentVariable(string name)
        {
            return !string.IsNullOrEmpty(name) 
                ? Environment.GetEnvironmentVariable(name) 
                : throw new InvalidOperationException("No environment variable was found.");
        }
    }
}
