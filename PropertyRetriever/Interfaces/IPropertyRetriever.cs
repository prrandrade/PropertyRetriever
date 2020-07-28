namespace PropertyRetriever.Interfaces
{
    using System.Collections.Generic;

    public interface IPropertyRetriever
    {
        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        string RetrieveFromEnvironment(string variableName);

        /// <summary>
        /// Retrieve a value from the environment variable set and convert to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that the environment variable value will be converted.</typeparam>
        /// <param name="variableName">Environment variable name.</param>
        /// <returns>Environment variable value converted to <typeparamref name="T"/>.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the environment variable name is not valid.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown when the variable is not found or can not be converted.</exception>
        T RetrieveFromEnvironment<T>(string variableName);

        /// <summary>
        /// Check if a property is set via command line.
        /// </summary>
        /// <param name="propertyLongName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="propertyShortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>True if at least one property is found, False otherwise.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="propertyLongName"/> and <paramref name="propertyShortName"/> are not provided.</exception>
        bool CheckFromCommandLine(string propertyLongName = null, string propertyShortName = null);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <param name="propertyLongName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="propertyShortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="propertyLongName"/> and <paramref name="propertyShortName"/> are not provided.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for string can not be processed.</exception>
        IEnumerable<string> RetrieveFromCommandLine(string propertyLongName = null, string propertyShortName = null);

        /// <summary>
        /// Retrieve a list of values passed from the command line, converted to a specific type.
        /// </summary>
        /// <typeparam name="T">Desired type that all the retrieved properties will be converted.</typeparam>
        /// <param name="propertyLongName">Optional property long name (identified with -- as a command line parameter).</param>
        /// <param name="propertyShortName">Optional property short name (identified with - as a command line parameter).</param>
        /// <returns>List of properties retrieved from command line converted to the specified type.</returns>
        /// <exception cref="System.ArgumentException">Thrown if <paramref name="propertyLongName"/> and <paramref name="propertyShortName"/> are not provided.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the conversion for <typeparamref name="T"/> can not be processed.</exception>
        IEnumerable<T> RetrieveFromCommandLine<T>(string propertyLongName = null, string propertyShortName = null);
    }
}
