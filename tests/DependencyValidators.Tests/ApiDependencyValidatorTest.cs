namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;
    using Abstractions.ServiceContracts.Dependencies;
    using Abstractions.ServiceImplementations;
    using FluentAssertions;
    using Validation.ApiDependencyValidators;
    using Xunit;

    public class ApiDependencyValidatorTest
    {
        public static readonly TheoryData<DependencyValidatorTestCase> ApiDependenciesValidationTestCases;

        private static Assembly CreateAssembly(AssemblyName assemblyName)
        {
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name!);
            return moduleBuilder.Assembly;
        }

        static ApiDependencyValidatorTest()
        {
            const string apiDependencyRepositoryName = "musicality-labs-composer-assistant-storage-api";
            var clientAssemblyName = new AssemblyName("MusicalityLabs.ComposerAssistant.Storage.Api.Clients");
            ApiDependenciesValidationTestCases
                = new TheoryData<DependencyValidatorTestCase>
                  {
                      new DependencyValidatorTestCase
                      {
                          Description = "There are no API dependencies in either the architecture or the application",
                          Dependencies = Array.Empty<DependencyBase>(),
                          ServiceImplementation = new ServiceImplementation {ServiceAssemblies = Array.Empty<Assembly>()},
                          ExpectedErrors = Array.Empty<string>()
                      },
                      new DependencyValidatorTestCase
                      {
                          Description = "A dependency missing from the architecture has been added to the service",
                          Dependencies = Array.Empty<DependencyBase>(),
                          ServiceImplementation = new ServiceImplementation {ServiceAssemblies = new[] {CreateAssembly(clientAssemblyName)}},
                          ExpectedErrors
                              = new[]
                                {
                                    $"Assembly {clientAssemblyName.Name} is not allowed by architecture"
                                }
                      },
                      new DependencyValidatorTestCase
                      {
                          Description = "The architecture dependency was not implemented in the service",
                          Dependencies = new DependencyBase[] {new ApiDependency {Name = apiDependencyRepositoryName}},
                          ServiceImplementation = new ServiceImplementation {ServiceAssemblies = Array.Empty<Assembly>()},
                          ExpectedErrors
                              = new[]
                                {
                                    $"Dependency {nameof(ApiDependency)}: {apiDependencyRepositoryName} is not implemented in service"
                                }
                      },
                      new DependencyValidatorTestCase
                      {
                          Description = "The architecture dependency was implemented in the service",
                          Dependencies = new DependencyBase[] {new ApiDependency {Name = apiDependencyRepositoryName}},
                          ServiceImplementation = new ServiceImplementation {ServiceAssemblies = new[] {CreateAssembly(clientAssemblyName)}},
                          ExpectedErrors = Array.Empty<string>()
                      }
                  };
        }


        [Theory]
        [MemberData(nameof(ApiDependenciesValidationTestCases))]
        public void ShouldValidateApiDependencies(DependencyValidatorTestCase testCase)
        {
            // Given
            var dependencyValidator = new ApiDependencyValidator();

            // When
            var actualErrors = dependencyValidator.Validate(testCase.Dependencies, testCase.ServiceImplementation);

            // Then
            actualErrors.Should().BeEquivalentTo(testCase.ExpectedErrors);
        }
    }
}