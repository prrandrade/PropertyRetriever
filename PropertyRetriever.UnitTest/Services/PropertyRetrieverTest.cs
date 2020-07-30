namespace PropertyRetriever.UnitTest.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Microsoft.VisualBasic.CompilerServices;
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

        #region RetrieveFromEnvironment - String

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
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(variableName), Times.Once);
            Assert.Equal(variableValue, result);
        }

        [Fact]
        public void RetrieveFromEnvironment_String_FallbackValue()
        {
            // arrange
            const string variableName = "variableName";
            const string variableFallbackValue = "variableFallbackValue";
            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromEnvironment(variableName, variableFallbackValue);

            // assert
            Assert.Equal(variableFallbackValue, result);
        }

        #endregion

        #region RetrieveFromEnvironment - Generic

        [Theory]
        [InlineData("test", "test")]
        [InlineData("a", 'a')]
        [InlineData("0", 0)]
        [InlineData("-1", -1)]
        [InlineData("2.23456", 2.23456)]
        [InlineData("-2.0009", -2.0009)]
        [InlineData("true", true)]
        [InlineData("TrUe", true)]
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

        [Theory]
        [InlineData("test")]
        [InlineData('a')]
        [InlineData(-1)]
        [InlineData(2.23456)]
        [InlineData(false)]
        public void RetrieveFromEnvironment_FallbackValue_NotFound<T>(T fallbackValue)
        {
            // arrange
            const string variableName = "variableName";
            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromEnvironment<T>(variableName, fallbackValue);

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(variableName), Times.Once);
            Assert.Equal(fallbackValue, result);
        }

        [Theory]
        [InlineData("asd", 'a')]
        [InlineData("a", -1)]
        [InlineData("a", 2.23456)]
        [InlineData("a", false)]
        public void RetrieveFromEnvironment_FallbackValue_NotConverted<T>(string variableValue, T fallbackValue)
        {
            // arrange
            const string variableName = "variableName";
            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(variableValue);

            // act
            var result = _propertyRetriever.RetrieveFromEnvironment(variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
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

        [Theory]
        [InlineData('a', true, "-a", "-b")]
        [InlineData('a', true, "-vAr", "-a")]
        [InlineData('A', true, "-A", "-bc")]
        [InlineData('A', true, "-ev", "-a")]
        [InlineData('A', false, "-etds", "-yu")]
        [InlineData('B', true, "-b", "-ce")]
        [InlineData('B', true, "-te", "-B")]
        [InlineData('b', true, "-c", "-bc")]
        [InlineData('b', true, "-uBv", "-d")]
        [InlineData('b', false, "-a", "-u")]
        public void CheckFromCommandLine_ShortName(char shortName, bool expectedResult, params string[] parameter)
        {
            // assert
            var parameters = new List<string> {"program.exe"};
            parameters.AddRange(parameter);
            
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters.ToArray);

            // act
            var result = _propertyRetriever.CheckFromCommandLine(shortName);

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("longName", true, "--LONGNAME", "--otherName")]
        [InlineData("LongNAME", true, "--OTHERNAME", "--longName")]
        [InlineData("LONGNAME", true, "--loNGNAme", "--otherNAME")]
        [InlineData("longname", true, "--othername", "--LOngNAME")]
        [InlineData("longname", false, "--firstName", "--secondName")]
        public void CheckFromCommandLine_LongName(string longName, bool expectedResult, params string[] parameter)
        {
            // assert
            var parameters = new List<string> {"program.exe"};
            parameters.AddRange(parameter);
            
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters.ToArray);

            // act
            var result = _propertyRetriever.CheckFromCommandLine(longName);

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("longName", 'l', true, "--LONGNAME", "-g")]
        [InlineData("longName", 'l', true, "--blabla", "-l")]
        [InlineData("LONGName", 'l', true, "--longname", "-gf")]
        [InlineData("longName", 'l', true, "--ble", "-L")]
        [InlineData("longName", 'l', false, "--ble", "-x")]
        public void CheckFromCommandLine(string longName, char shortName, bool expectedResult, params string[] parameter)
        {
            // assert
            var parameters = new List<string> {"program.exe"};
            parameters.AddRange(parameter);
            
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters.ToArray);

            // act
            var result = _propertyRetriever.CheckFromCommandLine(longName, shortName);

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void CheckFromCommandLine_NoPropertyNameProvided()
        {
            // act
            var result = Record.Exception(() => _propertyRetriever.CheckFromCommandLine(null, null));

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You need to supply a longName and/or a shortName.", result.Message);
        }

        #endregion

        #region RetrieveFromCommandLine

        [Fact]
        public void RetrieveFromCommandLine_NoPropertyNameProvided()
        {
            // arrange

            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine<string>(null, shortName: null));

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You need to supply a longName and/or a shortName.", result.Message);
        }

        [Fact]
        public void RetrieveFromCommandLine_NoPropertyFound()
        {
            // arrange
            var parameters = new[] { "program.exe", "--longPropertyName", "value", "-shortPropertyName", "anotherValue" };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<string>("longName", 's');

            // assert
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("firstValue", "secondValue", "thirdValue", "fourthValue", "firstValue", "secondValue", "thirdValue", "fourthValue")]
        public void RetrieveFromCommandLine_String(string property1, string property2, string property3, string property4, string value1, string value2, string value3, string value4)
        {
            // arrange
            const string longName = "longPropertyName";
            const char shortName = 's';

            var parameters = new[]
            {
                "program.exe",
                $"--{longName}", property1,
                $"-{shortName}", property2,
                $"-{shortName}", property3,
                $"--{longName}", property4
            };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longName, shortName);

            // assert
            Assert.Equal(new[] { value1, value2, value3, value4 }, result);
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
            const string longName = "longPropertyName";
            const char shortName = 's';

            var parameters = new[]
            {
                "program.exe",
                $"--{longName}", property1,
                $"-{shortName}", property2,
                $"-{shortName}", property3,
                $"--{longName}", property4
            };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(longName, shortName);

            // assert
            Assert.Equal(new[] { value1, value2, value3, value4 }, result);
        }

        [Theory]
        [InlineData("firstValue", "secondValue", "firstValue", "secondValue")]
        public void RetrieveFromCommandLine_ShortName_String(string property1, string property2, string value1, string value2)
        {
            // arrange
            const char shortName = 's';
            var parameters = new[]
            {
                "program.exe",
                $"-{shortName}", property1,
                $"-{shortName}", property2
            };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(shortName);

            // assert
            Assert.Equal(new[] { value1, value2 }, result);
        }

        [Theory]
        [InlineData("firstValue", "secondValue", "firstValue", "secondValue")]
        [InlineData("a", "b", 'a', 'b')]
        [InlineData("9", "8", 9, 8)]
        [InlineData("-100", "0", -100, 0)]
        [InlineData("0.00001", "0.1234", 0.00001, 0.1234)]
        [InlineData("true", "TRUE", true, true)]
        public void RetrieveFromCommandLine_ShortName<T>(string property1, string property2, T value1, T value2)
        {
            // arrange
            const char shortName = 's';
            var parameters = new[]
            {
                "program.exe",
                $"-{shortName}", property1,
                $"-{shortName}", property2
            };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(shortName);

            // assert
            Assert.Equal(new[] { value1, value2 }, result);
        }

        [Theory]
        [InlineData("longProperty", null, "Error while converting value found for property with name longProperty.")]
        [InlineData(null, 'l', "Error while converting value found for property with name l.")]
        [InlineData("longProperty", 'l', "Error while converting value found for property with name longProperty/l.")]
        public void RetriveFromCommandLine_Error(string longName, char? shortName, string expectedErrorMessage)
        {
            // arrange
            var parameters = new[] { "program.exe", "--longProperty", "value", "-l", "anotherValue" };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine<int>(longName, shortName));

            // assert
            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal(expectedErrorMessage, result.Message);
        }

        [Theory]
        [InlineData("Property")]
        [InlineData("property")]
        [InlineData("PROPERTY")]
        [InlineData("propERTy")]
        public void RetrieveFromCommandLine_LongNameCaseInsensitive(string longName)
        {
            // arrange
            const string value = "value";
            var parameters = new[] { "program.exe", "--property", value };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<string>(longName);

            // assert
            Assert.Equal(value, result.ToList()[0]);
        }

        [Theory]
        [InlineData('c')]
        [InlineData('C')]
        public void RetrieveFromCommandLine_ShortNameCaseInsensitive(char shortName)
        {
            // arrange
            const string value = "value";
            var parameters = new[] { "program.exe", "-c", value };
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(parameters);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(shortName);

            // assert
            Assert.Equal(value, result.ToList()[0]);
        }

        #endregion
    }
}
