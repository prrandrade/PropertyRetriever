PropertyRetriever

====================================

# Summary

- [Introduction](#introduction)
- [Package Installation](package-installation)
- [Retrieving variables from the Environment](#retrieving-variables-from-the-environment)
- [Checking parameters from Command Line](#checking-parameters-from-command-line)
- [Retrieving parameters from Command Line](#retrieving-parameters-from-command-line)



# Introduction

This small project can be used to access command-line parameters or environment variables using dependency injection on .NET Core Projects.

Nuget package: https://www.nuget.org/packages/PropertyRetriever/.



# Package Installation

Using the native .NET Core dependency injection framework, the installation consists of a single extension method that adds the injection in a `IServiceCollection`. For a .NET Core API project, for example, the configuration is added on the `ConfigureServices` method:

```csharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddPropertyRetriever();
}
```

For a console application, the `ServiceCollection` must be manually initialized:

```csharp
var services = new ServiceCollection();
services.AddPropertyRetriever();
```

If you use another dependency injection framework, two dependencies must be injected:

- Interface `ILocalEnvironment` and implementation `LocalEnvironment`.
- Interface `IPropertyRetriever` and implementation `PropertyRetriever`.

The interface `IPropertyRetriever` can be injected on classes that will access the environment or the command line.



# Retrieving variables from the Environment

As explained before, the interface `IPropertyRetriever` can be injected on any class and can be used to retrieve values from the environment or the command line.

```csharp
public class SomeClass
{
	private readonly IPropertyRetriever propertyRetriever;
	
	public SomeClass(IPropertyRetriever propertyRetriever)
	{
		this.propertyRetriever = propertyRetriever;
	}
}
```

To retrieve a environment variable, simply use:

```csharp
string result1 = propertyRetriever.RetrieveFromEnvironment("variableName");
```

You can use the generic version of this method to convert the retrieved environment variable:

```csharp
string result2 = propertyRetriever.RetrieveFromEnvironment<string>("variableName");
int result3 = propertyRetriever.RetrieveFromEnvironment<int>("variableName");
double result4 = propertyRetriever.RetrieveFromEnvironment<double>("variableName");
char result5 = propertyRetriever.RetrieveFromEnvironment<char>("variableName");
bool result6 = propertyRetriever.RetrieveFromEnvironment<bool>("variableName");
```

The method `RetrieveFromEnvironment` can throw an `ArgumentException` if the environment variable name is invalid or a `InvalidOperationException` if the variable is not found or can not be converted.



# Checking parameters from Command Line

To simply check if a property is set via command line, using the patterns --*longPropertyName* (two dashes) and -*shortPropertyName* (one dash), simply use:

```csharp
bool propertyIsSet1 = propertyRetriever.CheckFromCommandLine("longPropertyName", "shortPropertyName");
```

In this case, if at least one `--longPropertyName` or `-shortPropertyName` is set, the result will be `true`. You don't need to pass both names, of course:

```csharp
bool propertyIsSet2 = propertyRetriever.CheckFromCommandLine(propertyLongName: "longPropertyName");
bool propertyIsSet3 = propertyRetriever.CheckFromCommandLine(propertyShortName: "shortPropertyName");
```

The method `CheckForCommandLine` can throw an `ArgumentException` if neither property name is passed (at least one property name must be called).



# Retrieving parameters from Command Line

If you retrieve one or more property values from the command line, the method `RetrieveFromCommandLine` can be easily used:

```csharp
IEnumerable<string> values1 = propertyRetriever.RetrieveFromCommandLine("longPropertyName", "shortPropertyName");
```

If the command line is like: program.exe --longProperyName **value1** -shortPropertyName **value2** -shortPropertyName **value3**, then the result will be `{"value1", "value2", "value3"}`.

The operation of this method is like the the previous `CheckFromCommandLine` method; you can pass just the long property name or the short property name. And a generic variant will convert the retrieved values:

```csharp
IEnumerable<string> values2 = propertyRetriver.RetriveFromCommandLine<string>(propertyLongName: "longPropertyName");
IEnumerable<int> values2 = propertyRetriver.RetriveFromCommandLine<int>(propertyLongName: "longPropertyName");
IEnumerable<double> values2 = propertyRetriver.RetriveFromCommandLine<double>(propertyLongName: "longPropertyName");
IEnumerable<char> values2 = propertyRetriver.RetriveFromCommandLine<char>(propertyShortName: "shortPropertyName");
IEnumerable<bool> values2 = propertyRetriver.RetriveFromCommandLine<bool>(propertyShortName: "shortPropertyName");
```

The method `RetriveFromCommandLine` can throw an `ArgumentException` if neither property name is passed (at least one property name must be called) or a `InvalidOperationException` if the conversion is not possible. If no value is retrieved, then then returned `IEnumerable` is empty.



