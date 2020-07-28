namespace PropertyRetriever.UnitTest.Services
{
    using System;
    using System.Security;
    using Interfaces;
    using Moq;
    using PropertyRetriever.Services;
    using Xunit;

    public class PropertyRetrieverTest
    {
        private readonly Mock<ILocalEnvironment> _localEnvironmentMock;
        private readonly PropertyRetriever _propertyRetriever;

        public PropertyRetrieverTest()
        {
            _localEnvironmentMock = new Mock<ILocalEnvironment>();
            _propertyRetriever = new PropertyRetriever(_localEnvironmentMock.Object);
        }

        #region RetrieveFromEnvironment

        [Fact]
        public void RetrieveFromEnvironment_String()
        {
            // arrange
            const string variableName = "variableName";
            const string variableValue = "variableValue";
            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(variableValue);

            // act
            var result = _propertyRetriever.RetrieveFromEnvironment(variableName);

            // assert
            Assert.Equal(variableValue, result);
        }

        [Theory]
        [InlineData("test", "test")]
        [InlineData("a", 'a')]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        [InlineData("-2", -2)]
        [InlineData("1.1", 1.1)]
        [InlineData("2.23456", 2.23456)]
        [InlineData("0.902", 0.902)]
        [InlineData("-1.8", -1.8)]
        [InlineData("-2.0009", -2.0009)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("True", true)]
        [InlineData("TrUe", true)]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        [InlineData("False", false)]
        [InlineData("FaLsE", false)]
        public void RetrieveFromEnvironment<T>(string variableValue, T expectedVariableValue)
        {
            // arrange
            const string variableName = "variableName";
            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(variableValue);

            // act
            var result = _propertyRetriever.RetrieveFromEnvironment<T>(variableName);

            // assert
            Assert.Equal(expectedVariableValue, result);
        }

        [Fact]
        public void RetrieveFromEnvironment_Error()
        {
            // arrange
            const string variableName = "variableName";
            const string variableValue = "variableValue";
            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(variableValue);

            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromEnvironment<int>(variableName));

            // assert
            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal($"The environment variable {variableName} was found, but could not be converted.", result.Message);
        }

        #endregion

        #region CheckFromCommandLine

        [Fact]
        public void CheckFromCommandLine_NoPropertyNameProvided()
        {
            // act
            var result = Record.Exception(() => _propertyRetriever.CheckFromCommandLine());

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You need to supply a propertyLongName and/or a propertyShortName.", result.Message);
        }

        [Fact]
        public void CheckFromCommandLine_NoPropertyFound()
        {
            // assert
            var parameters = new[] { "program.exe", "--longPropertyName", "-shortPropertyName" };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.CheckFromCommandLine("longProperty", "shortProperty");

            // assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("longPropertyName", null)]
        [InlineData(null, "shortPropertyName")]
        [InlineData("longPropertyName", "shortPropertyName")]
        public void CheckFromCommandLine_PropertyFound(string longPropertyName, string shortPropertyName)
        {
            // assert
            var parameters = new[] { "program.exe", "--longPropertyName", "-shortPropertyName" };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.CheckFromCommandLine(longPropertyName, shortPropertyName);

            // assert
            Assert.True(result);
        }

        #endregion

        #region RetrieveFromCommandLine

        [Fact]
        public void RetrieveFromCommandLine_NoPropertyNameProvided()
        {
            // arrange

            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine<string>());

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You need to supply a propertyLongName and/or a propertyShortName.", result.Message);
        }

        [Fact]
        public void RetrieveFromCommandLine_NoPropertyFound()
        {
            // arrange
            var parameters = new[] { "program.exe", "--longPropertyName", "value", "-shortPropertyName", "anotherValue" };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<string>("longName", "shortName");

            // assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("firstValue", "secondValue", "thirdValue", "fourthValue", "firstValue", "secondValue", "thirdValue", "fourthValue")]
        [InlineData("a", "b", "c", "d", 'a', 'b', 'c', 'd')]
        [InlineData("9", "8", "7", "6", 9, 8, 7, 6)]
        [InlineData("0", "0", "-1", "-2", 0, 0, -1, -2)]
        [InlineData("-100", "0", "56", "12345767", -100, 0, 56, 12345767)]
        [InlineData("1.1", "2.23456", "0.902", "-1.8", 1.1, 2.23456, 0.902, -1.8)]
        [InlineData("0.00001", "0.1234", "12351.7345", "-0.001", 0.00001, 0.1234, 12351.7345, -0.001)]
        [InlineData("true", "TRUE", "True", "trUE", true, true, true, true)]
        [InlineData("false", "FALSE", "False", "faLSE", false, false, false, false)]
        public void RetrieveFromCommandLine<T>(string property1, string property2, string property3, string property4, T value1, T value2, T value3, T value4)
        {
            // arrange
            const string longPropertyName = "longPropertyName";
            const string shortPropertyName = "shortPropertyName";

            var parameters = new[]
            {
                "program.exe",
                $"--{longPropertyName}", property1,
                $"-{shortPropertyName}", property2,
                $"-{shortPropertyName}", property3,
                $"--{longPropertyName}", property4
            };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(longPropertyName, shortPropertyName);

            // assert
            Assert.Equal(new[] { value1, value2, value3, value4 }, result);
        }

        [Theory]
        [InlineData("firstValue", "secondValue", "thirdValue", "fourthValue", "firstValue", "secondValue", "thirdValue", "fourthValue")]
        public void RetrieveFromCommandLine_String(string property1, string property2, string property3, string property4, string value1, string value2, string value3, string value4)
        {
            // arrange
            const string longPropertyName = "longPropertyName";
            const string shortPropertyName = "shortPropertyName";

            var parameters = new[]
            {
                "program.exe",
                $"--{longPropertyName}", property1,
                $"-{shortPropertyName}", property2,
                $"-{shortPropertyName}", property3,
                $"--{longPropertyName}", property4
            };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longPropertyName, shortPropertyName);

            // assert
            Assert.Equal(new[] { value1, value2, value3, value4 }, result);
        }

        [Theory]
        [InlineData("longProperty", null, "Error while converting value found for property with name longProperty.")]
        [InlineData(null, "shortProperty", "Error while converting value found for property with name shortProperty.")]
        [InlineData("longProperty", "shortProperty", "Error while converting value found for property with name longProperty/shortProperty.")]
        public void RetriveFromCommandLine_Error(string longPropertyName, string shortPropertyName, string expectedErrorMessage)
        {
            // arrange
            var parameters = new[] { "program.exe", "--longProperty", "value", "-shortProperty", "anotherValue" };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine<int>(longPropertyName, shortPropertyName));

            // assert
            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal(expectedErrorMessage, result.Message);
        }

        #endregion
    }
}
