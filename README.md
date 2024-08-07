[![License](https://img.shields.io/badge/License-Apache--2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

Provides the necessary tooling for testing the architecture.

# Use cases

## Validation of architecture implementation in services

How to implement:
- describe the architecture as code and store it in a Git repository;
- set up versioning and sharing of the architecture, for example, using NuGet packages;
- create a project in the service to test its architecture;
- add package with architecture to the project; 
- add package `Byndyusoft.ArchitectureTesting.StructurizrParser` to the project in order to parse architecture into an object model, currently only the architecture described in the Structurizr is supported;
- implement dependency validators and [<code>ServiceImplementation</code>](src/Abstractions/ServiceImplementations/ServiceImplementation.cs) ititializer;
- add a test to check the compliance of the service implementation with its description on the architecture, example can be found [here](examples/Api.Tests/ArchitectureImplementationTest.cs).

# Byndyusoft.ArchitectureTesting.Abstractions[![Nuget](https://img.shields.io/nuget/v/Byndyusoft.ArchitectureTesting.Abstractions.svg)](https://www.nuget.org/packages/Byndyusoft.ArchitectureTesting.Abstractions/)[![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.ArchitectureTesting.Abstractions.svg)](https://www.nuget.org/packages/Byndyusoft.ArchitectureTesting.Abstractions/)

This package provides an architecture object model difinition ando primitives for validating it.

## Installing

```shell
dotnet add package Byndyusoft.ArchitectureTesting.Abstractions
```

## Usage

To implement a dependency validator, you need to inherit from the `DependencyValidatorBase` class and implement the `Validate` method.

```csharp
public class DbDependencyValidator : DependencyValidatorBase<DbDependency>
{
    ...
	 
	protected override IEnumerable<string> Validate(DbDependency[] dependencies, ServiceImplementation serviceImplementation)
	{
		...
	}
}
```

# Byndyusoft.ArchitectureTesting.StructurizrParser[![Nuget](https://img.shields.io/nuget/v/Byndyusoft.ArchitectureTesting.StructurizrParser.svg)](https://www.nuget.org/packages/Byndyusoft.ArchitectureTesting.StructurizrParser/)[![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.ArchitectureTesting.StructurizrParser.svg)](https://www.nuget.org/packages/Byndyusoft.ArchitectureTesting.StructurizrParser/)

This package is used to parse architecture described through Structurizr into an object model

## Installing

```shell
dotnet add package Byndyusoft.ArchitectureTesting.StructurizrParser
```

## Usage

To parse an architecture described as JSON, you need to read it as a string and pass it to the parser.

```csharp
var parser = new JsonParser(x => x.StartsWith("musicality-labs", StringComparison.InvariantCultureIgnoreCase));
var serviceContracts = parser.Parse(File.ReadAllText("musicality-labs.json"));
```

# Contributing

To contribute, you will need to setup your local environment, see [prerequisites](#prerequisites). For the contribution and workflow guide, see [package development lifecycle](#package-development-lifecycle).

## Prerequisites

Make sure you have installed all of the following prerequisites on your development machine:

- Git - [Download & Install Git](https://git-scm.com/downloads). OSX and Linux machines typically have this already installed.
- .NET (.net version) - [Download & Install .NET](https://dotnet.microsoft.com/en-us/download/dotnet/).

## Package development lifecycle

- Implement package logic in `src`
- Add or adapt unit-tests (prefer before and simultaneously with coding) in `tests`
- Add or change the documentation as needed
- Open pull request in the correct branch. Target the project's `master` branch

# Maintainers
[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)
