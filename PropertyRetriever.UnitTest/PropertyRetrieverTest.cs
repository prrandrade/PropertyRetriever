namespace PropertyRetriever.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class PropertyRetrieverTest
    {
        private readonly Mock<ILocalEnvironment> _localEnvironmentMock;
        private readonly PropertyRetriever _propertyRetriever;

        public PropertyRetrieverTest()
        {
            _localEnvironmentMock = new Mock<ILocalEnvironment>();
            _propertyRetriever = new PropertyRetriever(_localEnvironmentMock.Object);
        }

        #region RetrieveSimpleProperty from command line/environment without alternative name

        [Theory]
        [InlineData("value", typeof(string))]
        [InlineData(1, typeof(int))]
        [InlineData(1.0, typeof(double))]
        [InlineData(-1, typeof(int))]
        [InlineData(-1.0, typeof(double))]
        public void RetrieveSimpleProperty_FromCommandLine(object expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{expectedValue}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<string>("propertyName");

                // assert
                Assert.IsType<string>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<int>("propertyName");

                // assert
                Assert.IsType<int>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<double>("propertyName");

                // assert
                Assert.IsType<double>(result);
                Assert.Equal(expectedValue, result);
            }
        }

        [Theory]
        [InlineData("value", typeof(string))]
        [InlineData(1, typeof(int))]
        [InlineData(1.0, typeof(double))]
        [InlineData(-1, typeof(int))]
        [InlineData(-1.0, typeof(double))]
        public void RetrieveSimpleProperty_FromEnvironment(object expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("propertyName")).Returns($"{expectedValue}");

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<string>("propertyName");

                // assert
                Assert.IsType<string>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<int>("propertyName");

                // assert
                Assert.IsType<int>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<double>("propertyName");

                // assert
                Assert.IsType<double>(result);
                Assert.Equal(expectedValue, result);
            }
        }

        [Theory]
        [InlineData("xyz", "property1")]
        [InlineData("xyz", "property2")]
        public void RetrieveSimpleProperty_FailedConversion(object expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--property1", $"{expectedValue}" });
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("property2")).Returns($"{expectedValue}");

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimpleProperty<int>(propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RetrieveSimpleProperty_NoPropertyFound()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimpleProperty<int>(""));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Theory]
        [InlineData("value")]
        [InlineData(1)]
        [InlineData(1.0)]
        [InlineData(-1)]
        [InlineData(-1.0)]
        public void RetrieveSimpleProperty_WithDefaultValue(object defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("property", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_WithListOfValues_WithDefaultValue_FromCommand(object expectedValue, object[] possibleValues)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{expectedValue}" });

            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("propertyName", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_WithListOfValues_WithDefaultValue_FromEnvironment(object expectedValue, object[] possibleValues)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("propertyName")).Returns($"{expectedValue}");

            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("propertyName", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_WithListOfValues_WithDefaultValue_NoPropertyFound(object expectedValue, object[] possibleValues)
        {
            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("propertyName", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
        }

        #endregion

        #region RetrieveSimpleProperty from command line/environment with alternative name

        [Theory]
        [InlineData("value", typeof(string))]
        [InlineData(1, typeof(int))]
        [InlineData(1.0, typeof(double))]
        [InlineData(-1, typeof(int))]
        [InlineData(-1.0, typeof(double))]
        public void RetrieveSimpleProperty_FromCommandLine_WithAlternativeName(object expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{expectedValue}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<string>("propertyName", "prop");

                // assert
                Assert.IsType<string>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<int>("propertyName", "prop");

                // assert
                Assert.IsType<int>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<double>("propertyName", "prop");

                // assert
                Assert.IsType<double>(result);
                Assert.Equal(expectedValue, result);
            }
        }

        [Theory]
        [InlineData("value", typeof(string))]
        [InlineData(1, typeof(int))]
        [InlineData(1.0, typeof(double))]
        [InlineData(-1, typeof(int))]
        [InlineData(-1.0, typeof(double))]
        public void RetrieveSimpleProperty_FromEnvironment_WithAlternativeName(object expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("prop")).Returns($"{expectedValue}");

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<string>("propertyName", "prop");

                // assert
                Assert.IsType<string>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<int>("propertyName", "prop");

                // assert
                Assert.IsType<int>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveSimpleProperty<double>("propertyName", "prop");

                // assert
                Assert.IsType<double>(result);
                Assert.Equal(expectedValue, result);
            }
        }

        [Theory]
        [InlineData("xyz", "property1")]
        [InlineData("xyz", "property2")]
        public void RetrieveSimpleProperty_FailedConversion_WithAlternativeName(object expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-property1", $"{expectedValue}" });
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("property2")).Returns($"{expectedValue}");

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimpleProperty<int>("bigProperytyName", propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RetrieveSimpleProperty_NoPropertyFound_WithAlternativeName()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimpleProperty<int>("propertyName", "prop"));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Theory]
        [InlineData("value")]
        [InlineData(1)]
        [InlineData(1.0)]
        [InlineData(-1)]
        [InlineData(-1.0)]
        public void RetrieveSimpleProperty_WithDefaultValue_WithAlternativeName(object defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("property", "prop", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_WithListOfValues_WithDefaultValue_FromCommand_WithAlternativeName(object expectedValue, object[] possibleValues)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-property", $"{expectedValue}" });

            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("propertyName", "property", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_WithListOfValues_WithDefaultValue_FromEnvironment_WithAlternativeName(object expectedValue, object[] possibleValues)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("property")).Returns($"{expectedValue}");

            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("propertyName", "property", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_WithListOfValues_WithDefaultValue_NoPropertyFound_WithAlternativeName(object expectedValue, object[] possibleValues)
        {
            // act
            var result = _propertyRetriever.RetrieveSimpleProperty("propertyName", "property", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
        }

        #endregion

        #region RetrieveBooleanPropertyFromCommandLine

        [Fact]
        public void RetrieveBooleanPropertyFromCommandLine()
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName" });

            // act
            var result = _propertyRetriever.RetrieveBooleanPropertyFromCommandLine("propertyName");

            // assert
            Assert.True(result);
        }

        [Fact]
        public void RetrieveBooleanPropertyFromCommandLine_NotFound()
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName" });

            // act
            var result = _propertyRetriever.RetrieveBooleanPropertyFromCommandLine("otherPropertyName");

            // assert
            Assert.False(result);
        }
        
        [Fact]
        public void RetrieveBooleanPropertyFromCommandLine_WithAlternativeName()
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop" });

            // act
            var result = _propertyRetriever.RetrieveBooleanPropertyFromCommandLine("propertyName", "prop");

            // assert
            Assert.True(result);
        }

        [Fact]
        public void RetrieveBooleanPropertyFromCommandLine_NotFound_WithAlternativeName()
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop" });

            // act
            var result = _propertyRetriever.RetrieveBooleanPropertyFromCommandLine("otherPropertyName");

            // assert
            Assert.False(result);
        }

        #endregion
    }
}
