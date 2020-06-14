namespace PropertyRetriever
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface ILocalEnvironment
    {
        /// <summary>
        /// Retrieve all command line arguments
        /// </summary>
        /// <returns>List with all command line arguments used when is program is called</returns>
        string[] GetCommandLineArgs();

        /// <summary>
        /// Get an environment variable value given a specific name
        /// </summary>
        /// <param name="name">Environment variable name</param>
        /// <returns>Environment variable value or null when no environment variable with given name is found</returns>
        string GetEnvironmentVariable(string name);
    }

    [ExcludeFromCodeCoverage]
    public class LocalEnvironment : ILocalEnvironment
    {
        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        public string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}
