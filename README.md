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

But if you need a fallback value (that will be returned instead of a `Exception`), you can pass it as a parameter:

```csharp
string result2 = propertyRetriever.RetrieveFromEnvironment<string>("variableName", "fallback");
int result3 = propertyRetriever.RetrieveFromEnvironment<int>("variableName", 0);
double result4 = propertyRetriever.RetrieveFromEnvironment<double>("variableName", 0.5);
char result5 = propertyRetriever.RetrieveFromEnvironment<char>("variableName", 'a');
bool result6 = propertyRetriever.RetrieveFromEnvironment<bool>("variableName", true);
```



# Checking parameters from Command Line

To simply check if a property is set via command line, using the patterns --*longName* (two dashes followed by a `string`) and -*l* (one dash, followed by a `char`), simply use:

```csharp
bool propertyIsSet1 = propertyRetriever.CheckFromCommandLine("longName", "l");
```

In this case, if at least one `--longName` or `-l` is set, the result will be `true`. You don't need to pass both names, of course:

```csharp
bool propertyIsSet2 = propertyRetriever.CheckFromCommandLine("longPropertyName");
bool propertyIsSet3 = propertyRetriever.CheckFromCommandLine('s');
```

This method is also valid if multiple short property names are grouped with the same prefix. For example, with this two command lines:

```shell
command.exe -a -b -c
command.exe -abc
```

The method `CheckFromCommandLine` can be used to check if the property *a* is set for both scenarios:

```csharp
bool propertyIsSet4 = propertyRetriever.CheckFromCommandLine('a');
```

The method `CheckForCommandLine` can throw an `ArgumentException` if neither property name is passed (at least one property name must be called).

Take note that this method is **case insensive**, so `-s` and `-S` represent the **same** property. The same values for *-longName* and *-LONGNAME*.



# Retrieving parameters from Command Line

If you retrieve one or more property values from the command line, the method `RetrieveFromCommandLine` can be easily used:

```csharp
IEnumerable<string> values1 = propertyRetriever.RetrieveFromCommandLine("longName", 'l');
```

As before, you can pass just one name:

```csharp
IEnumerable<string> values1 = propertyRetriever.RetrieveFromCommandLine('l');
IEnumerable<string> values2 = propertyRetriever.RetrieveFromCommandLine("longName", 'l');
```

If the command line is like: *program.exe --longName **value1** -s **value2** -s **value3***, then the result will be `{"value1", "value2", "value3"}`.

The operation of this method is like the the previous `CheckFromCommandLine` method; you can pass just the long property name or the short property name. And a generic variant will convert the retrieved values:

```csharp
IEnumerable<string> values2 = propertyRetriver.RetriveFromCommandLine<string>("longName");
IEnumerable<int> values2 = propertyRetriver.RetriveFromCommandLine<int>("longName");
IEnumerable<double> values2 = propertyRetriver.RetriveFromCommandLine<double>("longPropertyName", 'l');
IEnumerable<char> values2 = propertyRetriver.RetriveFromCommandLine<char>('l');
IEnumerable<bool> values2 = propertyRetriver.RetriveFromCommandLine<bool>('l');
```

The method `RetriveFromCommandLine` can throw an `ArgumentException` if neither property name is passed (at least one property name must be called) or a `InvalidOperationException` if the conversion is not possible. If no value is retrieved, then then returned `IEnumerable` is empty.

Take note that this method is **case insensitive**, so *--Property **1***  and *-PROPERTY **2*** represent the same property with two values. The call `propertyRetriver.RetriveFromCommandLine<int>("property");` will return a `IEnumerable<int>` with values **1** and **2**. This applies to short names as well, so, *-C* and *-c* refer to the same property. 

Just like previously written, you can have a fallback value instead of a `Exception` or a empty `IEnumerable`:

```csharp
IEnumerable<string> values2 = propertyRetriver.RetriveFromCommandLine<string>("longName", new[]{ "fallbackValue" });
IEnumerable<int> values2 = propertyRetriver.RetriveFromCommandLine<int>("longName", new[]{ 0,1,2 });
IEnumerable<double> values2 = propertyRetriver.RetriveFromCommandLine<double>("longPropertyName", new[]{ 0.235, 1.234 });
IEnumerable<char> values2 = propertyRetriver.RetriveFromCommandLine<char>('l', new[] { 'l', 'h'});
IEnumerable<bool> values2 = propertyRetriver.RetriveFromCommandLine<bool>('l', new { false, false, false});
```

As you can see, you don't even need to pass a `IEnumerable` with a fixed number of fallback values.

