namespace PropertyRetriever.UnitTest.Services
{
    using System;
    using PropertyRetriever.Services;
    using Xunit;

    public class LocalEnvironmentTest
    {
        #region GetCommandLineArgs
        
        [Fact]
        public void GetCommandLineArgs()
        {
            // arrange
            var localEnvironment = new LocalEnvironment();
            var expectedResult = Environment.GetCommandLineArgs();

            // act
            var result = localEnvironment.GetCommandLineArgs();

            // assert
            Assert.Equal(expectedResult, result);
        }

        #endregion

        #region GetEnvironmentVariable

        [Fact]
        public void GetEnvironmentVariable_NoNameProvided()
        {
            // arrange
            var localEnvironment = new LocalEnvironment();

            // act
            var result = Record.Exception(() => localEnvironment.GetEnvironmentVariable(null));

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You must provide an environment variable name.", result.Message);
        }

        [Fact]
        public void GetEnvironmentVariable_NullEnvironmentVariable()
        {
            // arrange
            var localEnvironment = new LocalEnvironment();
            const string environmentVariableName = "variable";

            // act
            var result = Record.Exception(() => localEnvironment.GetEnvironmentVariable(environmentVariableName));

            // assert
            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal($"No environment variable with name {environmentVariableName} was found.", result.Message);
        }

        [Fact]
        public void GetEnvironmentVariable()
        {
            // arrange
            var localEnvironment = new LocalEnvironment();
            const string environmentVariableName = "variable";
            const string environmentVariableValue = "value";
            Environment.SetEnvironmentVariable(environmentVariableName, environmentVariableValue);

            // act
            var result = localEnvironment.GetEnvironmentVariable(environmentVariableName);

            // assert
            Assert.Equal(environmentVariableValue, result);

            // post test
            Environment.SetEnvironmentVariable(environmentVariableName, null);
        }

        #endregion
    }
}
