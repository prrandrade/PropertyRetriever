namespace PropertyRetriever.UnitTest
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
        public void RetrieveSimpleProperty_FromCommandLine_WithListOfValues_WithDefaultValue(object expectedValue, object[] possibleValues)
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
        public void RetrieveSimpleProperty_FromEnvironment_WithListOfValues_WithDefaultValue(object expectedValue, object[] possibleValues)
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

        #region RetrieveSimpleProperty from command line without alternative name

        [Theory]
        [InlineData("value", typeof(string))]
        [InlineData(1, typeof(int))]
        [InlineData(1.0, typeof(double))]
        [InlineData(-1, typeof(int))]
        [InlineData(-1.0, typeof(double))]
        public void RetrieveSimpleProperty_JustFromCommandLine(object expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{expectedValue}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine<string>("propertyName");

                // assert
                Assert.IsType<string>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>("propertyName");

                // assert
                Assert.IsType<int>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine<double>("propertyName");

                // assert
                Assert.IsType<double>(result);
                Assert.Equal(expectedValue, result);
            }

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("xyz", "property1")]
        public void RetrieveSimpleProperty_JustFromCommandLine_FailedConversion(object expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--property1", $"{expectedValue}" });

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>(propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public void RetrieveSimpleProperty_JustFromCommandLine_NoPropertyFound()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>(""));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value")]
        [InlineData(1)]
        [InlineData(1.0)]
        [InlineData(-1)]
        [InlineData(-1.0)]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithDefaultValue(object defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine("property", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithListOfValues_WithDefaultValue(object expectedValue, object[] possibleValues)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{expectedValue}" });

            // act
            var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithListOfValues_WithDefaultValue_NoPropertyFound(object expectedValue, object[] possibleValues)
        {
            // act
            var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region RetrieveSimpleProperty from command line with alternative name

        [Theory]
        [InlineData("value", typeof(string))]
        [InlineData(1, typeof(int))]
        [InlineData(1.0, typeof(double))]
        [InlineData(-1, typeof(int))]
        [InlineData(-1.0, typeof(double))]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithAlternativeName(object expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{expectedValue}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine<string>("propertyName", "prop");

                // assert
                Assert.IsType<string>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>("propertyName", "prop");

                // assert
                Assert.IsType<int>(result);
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine<double>("propertyName", "prop");

                // assert
                Assert.IsType<double>(result);
                Assert.Equal(expectedValue, result);
            }

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("xyz", "property1")]
        public void RetrieveSimpleProperty_JustFromCommandLine_FailedConversion_WithAlternativeName(object expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-property1", $"{expectedValue}" });

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>("bigProperytyName", propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void RetrieveSimpleProperty_JustFromCommandLine_NoPropertyFound_WithAlternativeName()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>("propertyName", "prop"));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value")]
        [InlineData(1)]
        [InlineData(1.0)]
        [InlineData(-1)]
        [InlineData(-1.0)]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithDefaultValue_WithAlternativeName(object defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine("property", "prop", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithListOfValues_WithDefaultValue_WithAlternativeName(object expectedValue, object[] possibleValues)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-property", $"{expectedValue}" });

            // act
            var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", "property", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value", new[] { "value1", "value2" })]
        [InlineData(1, new object[] { 2, 3, 5 })]
        public void RetrieveSimpleProperty_JustFromCommandLine_WithListOfValues_WithDefaultValue_NoPropertyFound_WithAlternativeName(object expectedValue, object[] possibleValues)
        {
            // act
            var result = _propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", "property", possibleValues, expectedValue);

            // assert
            Assert.Equal(expectedValue, result);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region RetrieveListProperty from command line/environment without alternative name 

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string))]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int))]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double))]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int))]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double))]
        public void RetrieveListProperty_FromCommandLine(object[] expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{string.Join(';', expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName");

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string), '.')]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int), '-')]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double), '|')]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int), ',')]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double), '^')]
        public void RetrieveListProperty_FromCommandLine_DifferentSeparators(object[] expectedValue, Type type, char separator)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{string.Join(separator, expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string))]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int))]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double))]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int))]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double))]
        public void RetrieveListProperty_FromEnvironment(object[] expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("propertyName")).Returns($"{string.Join(';', expectedValue)}");

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName");

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string), '.')]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int), '-')]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double), '|')]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int), ',')]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double), '^')]
        public void RetrieveListProperty_FromEnvironment_DifferentSeparators(object[] expectedValue, Type type, char separator)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{string.Join(separator, expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "xyz", "abc" }, "property1")]
        [InlineData(new object[] { "abc", "xyz" }, "property2")]
        public void RetrieveListProperty_FailedConversion(object[] expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--property1", $"{string.Join(';', expectedValue)}" });
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("property2")).Returns($"{string.Join(';', expectedValue)}");

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListProperty<int>(propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RetrieveListProperty_NoPropertyFound()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListProperty<int>(""));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Theory]
        [InlineData("value1", "value2", "value3")]
        [InlineData(1, 2, 3)]
        [InlineData(1.0, 2.0, 3.0)]
        [InlineData(-1, -2, -3)]
        [InlineData(-1.0, -2.0, -3.0)]
        public void RetrieveListProperty_WithDefaultValue(params object[] defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveListProperty("property", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
        }

        #endregion

        #region RetrieveListProperty from command line/environment with alternative name 

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string))]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int))]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double))]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int))]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double))]
        public void RetrieveListProperty_FromCommandLine_WithAlternativeName(object[] expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{string.Join(';', expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string), '.')]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int), '-')]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double), '|')]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int), ',')]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double), '^')]
        public void RetrieveListProperty_FromCommandLine_DifferentSeparators_WithAlternativeName(object[] expectedValue, Type type, char separator)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{string.Join(separator, expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string))]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int))]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double))]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int))]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double))]
        public void RetrieveListProperty_FromEnvironment_WithAlternativeName(object[] expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("prop")).Returns($"{string.Join(';', expectedValue)}");

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string), '.')]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int), '-')]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double), '|')]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int), ',')]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double), '^')]
        public void RetrieveListProperty_FromEnvironment_DifferentSeparators_WithAlternativeName(object[] expectedValue, Type type, char separator)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{string.Join(separator, expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<string>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<int>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListProperty<double>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }
        }

        [Theory]
        [InlineData(new object[] { "xyz", "abc" }, "prop1")]
        [InlineData(new object[] { "abc", "xyz" }, "prop2")]
        public void RetrieveListProperty_FailedConversion_WithAlternativeName(object[] expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop1", $"{string.Join(';', expectedValue)}" });
            _localEnvironmentMock.Setup(x => x.GetEnvironmentVariable("prop2")).Returns($"{string.Join(';', expectedValue)}");

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListProperty<int>("fullPropertyName", propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void RetrieveListProperty_NoPropertyFound_WithAlternativeName()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListProperty<int>("fullName", "shortName"));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Theory]
        [InlineData("value1", "value2", "value3")]
        [InlineData(1, 2, 3)]
        [InlineData(1.0, 2.0, 3.0)]
        [InlineData(-1, -2, -3)]
        [InlineData(-1.0, -2.0, -3.0)]
        public void RetrieveListProperty_WithDefaultValue_WithAlternativeName(params object[] defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveListProperty("property", "prop", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
        }

        #endregion

        #region RetrieveListProperty from command line without alternative name 

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string))]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int))]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double))]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int))]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double))]
        public void RetrieveListProperty_JustFromCommandLine(object[] expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{string.Join(';', expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<string>("propertyName");

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<int>("propertyName");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<double>("propertyName");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string), '.')]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int), '-')]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double), '|')]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int), ',')]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double), '^')]
        public void RetrieveListProperty_JustFromCommandLine_DifferentSeparators(object[] expectedValue, Type type, char separator)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--propertyName", $"{string.Join(separator, expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<string>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<int>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<double>("propertyName", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(new object[] { "xyz", "abc" }, "property")]
        [InlineData(new object[] { "abc", "xyz" }, "property")]
        public void RetrieveListProperty_JustFromCommandLine_FailedConversion(object[] expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "--property", $"{string.Join(';', expectedValue)}" });

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListPropertyFromCommandLine<int>(propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void RetrieveListProperty_JustFromCommandLine_NoPropertyFound()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListPropertyFromCommandLine<int>(""));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value1", "value2", "value3")]
        [InlineData(1, 2, 3)]
        [InlineData(1.0, 2.0, 3.0)]
        [InlineData(-1, -2, -3)]
        [InlineData(-1.0, -2.0, -3.0)]
        public void RetrieveListProperty_JustFromCommandLine_WithDefaultValue(params object[] defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveListPropertyFromCommandLine("property", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        #endregion

        #region RetrieveListProperty from command line/environment with alternative name 

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string))]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int))]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double))]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int))]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double))]
        public void RetrieveListProperty_JustFromCommandLine_WithAlternativeName(object[] expectedValue, Type type)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{string.Join(';', expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<string>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<int>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<double>("propertyName", "prop");

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never());
        }

        [Theory]
        [InlineData(new object[] { "value1", "value2", "value3" }, typeof(string), '.')]
        [InlineData(new object[] { 1, 2, 3 }, typeof(int), '-')]
        [InlineData(new object[] { 1.0, 2.0, 3.0 }, typeof(double), '|')]
        [InlineData(new object[] { -1, -2, -3 }, typeof(int), ',')]
        [InlineData(new object[] { -1.0, -2.0, -3.0 }, typeof(double), '^')]
        public void RetrieveListProperty_JustFromCommandLine_DifferentSeparators_WithAlternativeName(object[] expectedValue, Type type, char separator)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", "-prop", $"{string.Join(separator, expectedValue)}" });

            if (type == typeof(string))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<string>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue, result);
            }
            else if (type == typeof(int))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<int>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToInt32), result);
            }
            else if (type == typeof(double))
            {
                // act
                var result = _propertyRetriever.RetrieveListPropertyFromCommandLine<double>("propertyName", "prop", separator);

                // assert
                Assert.Equal(expectedValue.Select(Convert.ToDouble), result);
            }

            // assert
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData(new object[] { "xyz", "abc" }, "prop1")]
        [InlineData(new object[] { "abc", "xyz" }, "prop2")]
        public void RetrieveListProperty_JustFromCommandLine_FailedConversion_WithAlternativeName(object[] expectedValue, string propertyName)
        {
            // arrange
            _localEnvironmentMock.Setup(x => x.GetCommandLineArgs()).Returns(new[] { "example.exe", $"-{propertyName}", $"{string.Join(';', expectedValue)}" });

            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListPropertyFromCommandLine<int>("fullPropertyName", propertyName));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void RetrieveListProperty_JustFromCommandLine_NoPropertyFound_WithAlternativeName()
        {
            // act
            var exception = Record.Exception(() => _propertyRetriever.RetrieveListPropertyFromCommandLine<int>("fullName", "shortName"));

            // assert
            Assert.IsType<InvalidOperationException>(exception);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("value1", "value2", "value3")]
        [InlineData(1, 2, 3)]
        [InlineData(1.0, 2.0, 3.0)]
        [InlineData(-1, -2, -3)]
        [InlineData(-1.0, -2.0, -3.0)]
        public void RetrieveListProperty_JustFromCommandLine_WithDefaultValue_WithAlternativeName(params object[] defaultValue)
        {
            // act
            var result = _propertyRetriever.RetrieveListPropertyFromCommandLine("property", "prop", defaultValue);

            // assert
            Assert.Equal(result, defaultValue);
            _localEnvironmentMock.Verify(x => x.GetEnvironmentVariable(It.IsAny<string>()), Times.Never);
        }

        #endregion
    }
}
