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
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("You must provide an environment variable name.");

            var result = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrEmpty(result))
                throw new InvalidOperationException($"No environment variable with name {name} was found.");

            return result;
        }
    }
}
