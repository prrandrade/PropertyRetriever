# PropertyRetriever

### Introduction

This small project can be used to access command-line parameters or environment variables using dependency injection on .NET Core Projects.

### Project installation

Using the native .NET Core dependency injection framework, the installation consists of a single extension method that adds the injection in a `IServiceCollection`. For a .NET Core API project, for example, the configuration is added on the `ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddPropertyRetriever();
}
```

For a console application, the ServiceCollection must be manually initialized:

```csharp
var services = new ServiceCollection();
services.AddPropertyRetriever();
```

If you use another dependency injection framework, two dependencies must be injected:

- Interface `ILocalEnvironment` and implementation `LocalEnvironment`
- Interface `IPropertyRetriever` and implementation `PropertyRetriever`

The interface `IPropertyRetriever` can be injected on classes that will access the environment or the command line.

### How to pass properties

There are four ways that the PropertyRetriever Package read properties, always using the same order:

- The **command line** with a long property name preceded with two dashes. The property value will be the first text found after the property name, like `--propertyName propertyValue`
- The **command line** with a short property name preceded with one dash. The property value will be the first text found after the property name, like `-p propertyValue`
- The **environment** with a long property name (without the preceding dashes).
- The **environment** with a short property name (without the preceding dash).

### Examples

As explained before, the interface `IPropertyRetriever` can be injected on any class and can be used to retrieve values from the environment or the command line.

```csharp
public class SomeClass
{
	private readonly IPropertyRetriever _propertyRetriever;
	
	public SomeClass(IPropertyRetriever propertyRetriever)
	{
		_propertyRetriever = propertyRetriever;
	}
}
```

When you what to retrieve a simple property, the method `RetrieveSimpleProperty` can be used and will try to convert the property:

```
int result1 = propertyRetriever.RetrieveSimpleProperty<int>("propertyName");
```

If you want a default value to be retrieved if the property is not found:

```
double result2 = propertyRetriever.RetrieveSimpleProperty("propertyName", 2.0);
```

If you want to check for a list of specific values and a default value if the check fails:

```
string result3 = propertyRetriever.RetrieveSimpleProperty("propertyName", new[] { "possibleValue1", "possibleValue2" }, "defaultValue");
```

All these examples can be used to retrieve properties also the short name:

```csharp
int result4 = propertyRetriever.RetrieveSimpleProperty<int>("propertyName", "pn");
double result5 = propertyRetriever.RetrieveSimpleProperty("propertyName", "pn", 2.0);
string result6 = propertyRetriever.RetrieveSimpleProperty("propertyName", "pn" new[] { "possibleValue1", "possibleValue2" }, "defaultValue");
```

If you want only to check properties from the command line, the method `RetrieveSimplePropertyFromCommandLine` can be used, with the same approach:

```csharp
int result1 = propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>("propertyName");
double result2 = propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", 2.0);
string result3 = propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", new[] { "possibleValue1", "possibleValue2" }, "defaultValue");

int result4 = propertyRetriever.RetrieveSimplePropertyFromCommandLine<int>("propertyName", "pn");
double result5 = propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", "pn", 2.0);
string result6 = propertyRetriever.RetrieveSimplePropertyFromCommandLine("propertyName", "pn" new[] { "possibleValue1", "possibleValue2" }, "defaultValue");
```

If you need a boolean property from the command line (true if a property is found, false otherwise), the method `RetrieveBooleanPropertyFromCommandLine` can be used:

```csharp
bool result1 = propertyRetriever.RetrieveBooleanPropertyFromCommandLine("propertyName");
bool result2 = propertyRetriever.RetrieveBooleanPropertyFromCommandLine("propertyName", "pn");
```

Finally, if you need to retrieve a list of separated values on the same property, the method `RetrieveListProperty` can be used, defining a custom separator (semi comma is the default choice):

```csharp
IEnumerable<int> result1 = propertyRetriever.RetrieveListProperty<int>("propertyName");
IEnumerable<string> result2 = propertyRetriever.RetrieveListProperty("propertyName", new[]{ "defaultValue1", "defautlValue2" }, '|'); // pipe as separator
IEnumerable<int> result3 = propertyRetriever.RetrieveListProperty<int>("propertyName", "pn", ','); // comma as separator
IEnumerable<string> result4 = propertyRetriever.RetrieveListProperty("propertyName", "pn", new[]{ "defaultValue1", "defautlValue2" });
```

You can retrieve a list of separated values on the same property only from the command line, using the method `RetrieveListPropertyFromCommandLine`:

```csharp
IEnumerable<int> result1 = propertyRetriever.RetrieveListPropertyFromCommandLine<int>("propertyName");
IEnumerable<string> result2 = propertyRetriever.RetrieveListPropertyFromCommandLine("propertyName", new[]{ "defaultValue1", "defautlValue2" }, '|'); // pipe as separator
IEnumerable<int> result3 = propertyRetriever.RetrieveListPropertyFromCommandLine<int>("propertyName", "pn", ','); // comma as separator
IEnumerable<string> result4 = propertyRetriever.RetrieveListPropertyFromCommandLine("propertyName", "pn", new[]{ "defaultValue1", "defautlValue2" });
```

### Next Steps

- [X] Version 1.0.0 on Nuget 
- [ ] Add way to retrieve multiple properties with same name 
- [ ] Add way to retrive properties with multiple values

