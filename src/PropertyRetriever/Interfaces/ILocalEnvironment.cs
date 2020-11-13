namespace PropertyRetriever.Interfaces
{
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
}
