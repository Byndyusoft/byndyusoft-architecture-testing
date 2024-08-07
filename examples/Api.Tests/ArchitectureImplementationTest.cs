namespace MusicalityLabs.Storage.Api.Tests
{
    using System;
    using System.IO;
    using Byndyusoft.ArchitectureTesting.Abstractions.Validation;
    using Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions;
    using Byndyusoft.ArchitectureTesting.StructurizrParser;
    using FluentAssertions;
    using Validation.DependencyValidators;
    using Validation.ServiceImplementations;
    using Xunit;

    public class ArchitectureImplementationTest
    {
        [Fact]
        public void ServiceShouldImplementArchitecture()
        {
            // Given
            var startupType = typeof(Startup);
            var parser = new JsonParser(x => x.StartsWith("musicality-labs", StringComparison.InvariantCultureIgnoreCase));
            var serviceContract = parser.Parse(File.ReadAllText("musicality-labs.json")).FindServiceContract(startupType.Assembly);
            using var serviceImplementation = ServiceImplementationsFactory.Create(startupType);

            // When
            var serviceValidationErrors = ServiceValidatorsFactory.Create(
                x => x.AddDependencyValidatorsFromAssembly(typeof(DbDependencyValidator).Assembly)
            ).Validate(serviceContract, serviceImplementation);

            // Then
            serviceValidationErrors.Should().BeEmpty();
        }
    }
}