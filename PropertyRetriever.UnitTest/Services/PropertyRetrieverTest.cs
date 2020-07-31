namespace PropertyRetriever.UnitTest.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
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
            var result = _propertyRetriever.RetrieveFromEnvironment(variableName, fallbackValue);

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
            // arrange
            var parameters = new List<string> { "program.exe" };
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
            // arrange
            var parameters = new List<string> { "program.exe" };
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
            // arrange
            var parameters = new List<string> { "program.exe" };
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

        #region RetrieveFromCommandLine - String

        [Theory]
        [InlineData('c', "program.exe -c 1 -d a -c a -c test", new[] { "1", "a", "test" })]
        [InlineData('c', "program.exe -C test -c 1234_5", new[] { "test", "1234_5" })]
        [InlineData('c', "program.exe -d xyz -e 23", new string[] { })]
        public void RetrieveFromCommandLine_String_ShortName(char shortName, string commandLine, string[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(shortName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData('c', "program.exe -d 1 -e a -f a -g test", new[] { "1", "2", "3" })]
        [InlineData('c', "program.exe -D 456 -d 1234", new[] { "test1", "test2" })]
        public void RetrieveFromCommandLine_String_ShortName_FallbackValue(char shortName, string commandLine, string[] fallback)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(shortName, fallback);

            // assert
            Assert.Equal(fallback, result.ToArray());
        }

        [Theory]
        [InlineData("prop", "program.exe --PROP 1 --ProP a --other a --PRop test", new[] { "1", "a", "test" })]
        [InlineData("ProP", "program.exe --pROp test --PROP 1234_5", new[] { "test", "1234_5" })]
        [InlineData("ProP", "program.exe --otherProp test --otherProp 1234_5", new string[] { })]
        public void RetrieveFromCommandLine_String_LongName(string longName, string commandLine, string[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData("prop", "program.exe --OTHERPROP a --OtherProp c", new[] { "1", "2", "3" })]
        [InlineData("PRop", "program.exe --prop1 456 -prop2 1234", new[] { "test1", "test2" })]
        public void RetrieveFromCommandLine_String_LongName_FallbackValue(string shortName, string commandLine, string[] fallback)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(shortName, fallback);

            // assert
            Assert.Equal(fallback, result.ToArray());
        }

        [Theory]
        [InlineData("prop", 'c', "program.exe --PROP 123 -c 456", new[] { "123", "456" })]
        [InlineData("prop", 'c', "program.exe --ProP abc -C def", new[] { "abc", "def" })]
        [InlineData("prop", null, "program.exe --ProP 55 -C def", new[] { "55" })]
        [InlineData(null, 'C', "program.exe --ProP 55 -c yy", new[] { "yy" })]
        public void RetrieveFromCommandLine_String_LongName_ShortName(string longName, char? shortName, string commandLine, string[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longName, shortName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData("otherprop", 'p', "program.exe --PROP 123 -c 456", new[] { "123", "456" })]
        [InlineData("otherProp", 'p', "program.exe --ProP abc -C def", new[] { "abc", "def" })]
        [InlineData("otherProp", null, "program.exe --ProP 55 -C def", new[] { "55" })]
        [InlineData(null, 'C', "program.exe --ProP 55 -g yy", new[] { "yy" })]
        public void RetrieveFromCommandLine_String_LongName_ShortName_FallbackValue(string longName, char? shortName, string commandLine, string[] fallback)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longName, shortName, fallback);

            // assert
            Assert.Equal(fallback, result.ToArray());
        }

        [Fact]
        public void RetrieveFromCommandLine_String_LongName_ShortName_NoParameterProvided()
        {
            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine(null, shortName: null));

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You need to supply a longName and/or a shortName.", result.Message);
        }

        #endregion

        #region RetrieveFromCommandLine - Generic

        [Theory]
        [InlineData('c', "program.exe -c r -d a -c a -c y", new[] { 'r', 'a', 'y' })]
        [InlineData('c', "program.exe -C test -C 1234_5", new[] { "test", "1234_5" })]
        [InlineData('C', "program.exe -C 123.4 -c 567.8", new[] { 123.4, 567.8 })]
        [InlineData('c', "program.exe -c true -e false", new[] { true })]
        public void RetrieveFromCommandLine_Generic_ShortName<T>(char shortName, string commandLine, T[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(shortName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData('c', "program.exe -d 1 -e a -f a -g test", new[] { 1, 2, 3 })]
        [InlineData('c', "program.exe -D 456 -d 1234", new[] { "test1", "test2" })]
        public void RetrieveFromCommandLine_Generic_ShortName_FallbackValue<T>(char shortName, string commandLine, T[] fallback)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(shortName, fallback);

            // assert
            Assert.Equal(fallback, result.ToArray());
        }

        [Theory]
        [InlineData("prop", "program.exe --PROP 1 --ProP 2 --other a --PRop 3", new[] { 1, 2, 3 })]
        [InlineData("ProP", "program.exe --pROp test --PROP test2", new[] { "test", "test2" })]
        [InlineData("ProP", "program.exe --otherProp test --Prop t", new[] { 't' })]
        public void RetrieveFromCommandLine_Generic_LongName<T>(string longName, string commandLine, T[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(longName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData("prop", "program.exe --Prop true -o --PROP false", new[] { true, false })]
        [InlineData("PROP", "program.exe --prOP 456 -d 1234", new[] { 456 })]
        public void RetrieveFromCommandLine_Generic_LongName_FallbackValue<T>(string longName, string commandLine, T[] fallback)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longName, fallback);

            // assert
            Assert.Equal(fallback, result.ToArray());
        }

        [Theory]
        [InlineData("prop", 'c', "program.exe --PROP 123 -c 456", new[] { 123, 456 })]
        [InlineData("prop", 'c', "program.exe --ProP abc -C def", new[] { "abc", "def" })]
        public void RetrieveFromCommandLine_Generic_LongName_ShortName<T>(string longName, char? shortName, string commandLine, T[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(longName, shortName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData("prop", "program.exe --ProP 55 -C def", new[] { 55 })]
        public void RetrieveFromCommandLine_Generic_LongName_ShortName_Null<T>(string longName, string commandLine, T[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(longName, shortName: null);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Theory]
        [InlineData('C', "program.exe --ProP 55 -c yy", new[] { "yy" })]
        public void RetrieveFromCommandLine_Generic_LongName_Null_ShortName<T>(char shortName, string commandLine, T[] expectedResult)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<T>(null, shortName);

            // assert
            Assert.Equal(expectedResult, result.ToArray());
        }

        [Fact]
        public void RetrieveFromCommandLine_Generic_LongName_ShortName_NoParameterProvided()
        {
            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine<int>(null, shortName: null));

            // assert
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("You need to supply a longName and/or a shortName.", result.Message);
        }

        [Fact]
        public void RetrieveFromCommandLine_Generic_LongName_ShortName_NoParameterProvided_FallbackValue()
        {
            // arrange
            var fallbackValue = new[] { 1, 2, 3 };

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine<int>(null, null, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        [Theory]
        [InlineData("longName", null, "Error while converting value found for property with name longName.")]
        [InlineData(null, 'c', "Error while converting value found for property with name c.")]
        [InlineData("longName", 'c', "Error while converting value found for property with name longName/c.")]
        public void RetrieveFromCommandLine_Generic_LongName_ShortName_ErrorInConversion(string longName, char? shortName, string errorMessage)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(new[] { "program.exe", "--longName", "u", "-c", "test" });

            // act
            var result = Record.Exception(() => _propertyRetriever.RetrieveFromCommandLine<int>(longName, shortName));

            // assert
            Assert.IsType<InvalidOperationException>(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Theory]
        [InlineData("longName", null)]
        [InlineData(null, 'c')]
        [InlineData("longName", 'c')]
        public void RetrieveFromCommandLine_Generic_LongName_ShortName_ErrorInConversion_FallbackValue(string longName, char? shortName)
        {
            // arrange
            var fallbackValue = new[] { 1, 2, 3 };
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(new[] { "program.exe", "--longName", "u", "-c", "test" });

            // act
            var result = _propertyRetriever.RetrieveFromCommandLine(longName, shortName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        #endregion

        #region RetrieveFromCommandLineOrEnvironment - String

        [Theory]
        [InlineData("program.exe -c value", "value", 'c', "variableName")]
        [InlineData("program.exe -d otherValue", "value", 'c', "variableName")]
        public void RetrieveFromCommandLineOrEnvironment_String_ShortName_Environment(string commandLine, string desiredValue, char shortName, string variableName)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(desiredValue);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(shortName, variableName);

            // assert
            Assert.Equal(desiredValue, result);
        }

        [Fact]
        public void RetrieveFromCommandLineOrEnvironment_String_ShortName_Environment_FallbackValue()
        {
            // arrange
            const char shortName = 'c';
            const string variableName = "variableName";
            const string commandLine = "program.exe";
            const string fallbackValue = "fallback";

            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(shortName, variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        [Theory]
        [InlineData("program.exe --longName value", "value", "longName", "variableName")]
        [InlineData("program.exe --anotherName otherValue", "value", "longName", "variableName")]
        public void RetrieveFromCommandLineOrEnvironment_String_LongName_Environment(string commandLine, string desiredValue, string longName, string variableName)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(desiredValue);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(longName, variableName);

            // assert
            Assert.Equal(desiredValue, result);
        }

        [Fact]
        public void RetrieveFromCommandLineOrEnvironment_String_LongName_Environment_FallbackValue()
        {
            // arrange
            const string longName = "longName";
            const string variableName = "variableName";
            const string commandLine = "program.exe";
            const string fallbackValue = "fallback";

            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(longName, variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        [Theory]
        [InlineData("program.exe --longName value -c otherValue", "value", "longName", 'c', "variableName")]
        [InlineData("program.exe -c value --longName otherValue", "value", "longName", 'c', "variableName")]
        [InlineData("program.exe -d value --anotherName otherValue", "value", "longName", 'c', "variableName")]
        public void RetrieveFromCommandLineOrEnvironment_String_LongName_ShortName_Environment(string commandLine, string desiredValue, string longName, char shortName, string variableName)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(desiredValue);

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(longName, shortName, variableName);

            // assert
            Assert.Equal(desiredValue, result);
        }
        
        [Fact]
        public void RetrieveFromCommandLineOrEnvironment_String_LongName_ShortName_Environment_FallbackValue()
        {
            // arrange
            const char shortName = 'c';
            const string longName = "longName";
            const string variableName = "variableName";
            const string commandLine = "program.exe";
            const string fallbackValue = "fallback";

            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(longName, shortName, variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        #endregion

        #region RetrieveFromCommandLineOrEnvironment - Generic

        [Theory]
        [InlineData("program.exe -c 1", 1, 'c', "variableName")]
        [InlineData("program.exe -d otherValue", 1, 'c', "variableName")]
        [InlineData("program.exe -c 1.234", 1.234, 'c', "variableName")]
        [InlineData("program.exe -d otherValue", 1.234, 'c', "variableName")]
        [InlineData("program.exe -c true", true, 'c', "variableName")]
        [InlineData("program.exe -d otherValue", true, 'c', "variableName")]
        [InlineData("program.exe -c t", 't', 'c', "variableName")]
        [InlineData("program.exe -d otherValue", 't', 'c', "variableName")]
        public void RetrieveFromCommandLineOrEnvironment_Generic_ShortName_Environment<T>(string commandLine, T desiredValue, char shortName, string variableName)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(FormattableString.Invariant($"{desiredValue}"));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment<T>(shortName, variableName);

            // assert
            Assert.Equal(desiredValue, result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2.4)]
        [InlineData('c')]
        [InlineData(true)]
        public void RetrieveFromCommandLineOrEnvironment_Generic_ShortName_Environment_FallbackValue<T>(T fallbackValue)
        {
            // arrange
            const char shortName = 'c';
            const string variableName = "variableName";
            const string commandLine = "program.exe";

            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(shortName, variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        [Theory]
        [InlineData("program.exe --longName x", 'x', "longName", "variableName")]
        [InlineData("program.exe --anotherName otherValue", 'x', "longName", "variableName")]
        [InlineData("program.exe --longName 4", 4, "longName", "variableName")]
        [InlineData("program.exe --anotherName otherValue", 4, "longName", "variableName")]
        [InlineData("program.exe --longName 4.6", 4.6, "longName", "variableName")]
        [InlineData("program.exe --anotherName otherValue", 4.6, "longName", "variableName")]
        [InlineData("program.exe --longName 4.6", false, "longName", "variableName")]
        [InlineData("program.exe --anotherName otherValue", false, "longName", "variableName")]
        public void RetrieveFromCommandLineOrEnvironment_Generic_LongName_Environment<T>(string commandLine, T desiredValue, string longName, string variableName)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(FormattableString.Invariant($"{desiredValue}"));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment<T>(longName, variableName);

            // assert
            Assert.Equal(desiredValue, result);
        }

        [Theory]
        [InlineData(794525)]
        [InlineData(0.00009)]
        [InlineData('x')]
        [InlineData(false)]
        public void RetrieveFromCommandLineOrEnvironment_Generic_LongName_Environment_FallbackValue<T>(T fallbackValue)
        {
            // arrange
            const string longName = "longName";
            const string variableName = "variableName";
            const string commandLine = "program.exe";

            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(longName, variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        [Theory]
        [InlineData("program.exe --longName 1 -c otherValue", 1, "longName", 'c', "variableName")]
        [InlineData("program.exe -c 1 --longName otherValue", 1, "longName", 'c', "variableName")]
        [InlineData("program.exe -d 2 --anotherName otherValue", 1, "longName", 'c', "variableName")]
        [InlineData("program.exe --longName 1.0000008 -c otherValue", 1.0000008, "longName", 'c', "variableName")]
        [InlineData("program.exe -c 1.0000008 --longName otherValue", 1.0000008, "longName", 'c', "variableName")]
        [InlineData("program.exe -d 5 --anotherName otherValue", 1.0000008, "longName", 'c', "variableName")]
        [InlineData("program.exe --longName t -c otherValue", 't', "longName", 'c', "variableName")]
        [InlineData("program.exe -c t --longName otherValue", 't', "longName", 'c', "variableName")]
        [InlineData("program.exe -d t --anotherName otherValue", 't', "longName", 'c', "variableName")]
        [InlineData("program.exe --longName false -c otherValue", false, "longName", 'c', "variableName")]
        [InlineData("program.exe -c false --longName otherValue", false, "longName", 'c', "variableName")]
        [InlineData("program.exe -d true --anotherName otherValue", false, "longName", 'c', "variableName")]
        public void RetrieveFromCommandLineOrEnvironment_Generic_LongName_ShortName_Environment<T>(string commandLine, T desiredValue, string longName, char shortName, string variableName)
        {
            // arrange
            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Returns(FormattableString.Invariant($"{desiredValue}"));

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment<T>(longName, shortName, variableName);

            // assert
            Assert.Equal(desiredValue, result);
        }
        
        [Theory]
        [InlineData(463534252)]
        [InlineData(1.00009)]
        [InlineData('x')]
        [InlineData(true)]
        public void RetrieveFromCommandLineOrEnvironment_Generic_LongName_ShortName_Environment_FallbackValue<T>(T fallbackValue)
        {
            // arrange
            const char shortName = 'c';
            const string longName = "longName";
            const string variableName = "variableName";
            const string commandLine = "program.exe";

            _localEnvironmentMock
                .Setup(x => x.GetCommandLineArgs())
                .Returns(commandLine.Split(' '));

            _localEnvironmentMock
                .Setup(x => x.GetEnvironmentVariable(variableName))
                .Throws<Exception>();

            // act
            var result = _propertyRetriever.RetrieveFromCommandLineOrEnvironment(longName, shortName, variableName, fallbackValue);

            // assert
            Assert.Equal(fallbackValue, result);
        }

        #endregion
    }
}
