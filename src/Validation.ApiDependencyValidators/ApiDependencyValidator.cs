namespace Byndyusoft.ArchitectureTesting.Validation.ApiDependencyValidators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Abstractions.ServiceContracts.Dependencies;
    using Abstractions.ServiceImplementations;
    using Abstractions.Validation.DependencyValidators;
    using Abstractions.Validation.Extensions;

    public class ApiDependencyValidator : DependencyValidatorBase<ApiDependency>
    {
        private static bool IsApiClient(Assembly assembly)
        {
            var cleanedAssemblyName = assembly.GetName().Name!.CleanString();
            if (cleanedAssemblyName.Equals("Byndyusoft.ApiClient".CleanString()))
                return false;

            return cleanedAssemblyName.EndsWith(".Api.Client".CleanString())
                || cleanedAssemblyName.EndsWith(".Api.Clients".CleanString());
        }

        private static bool IsRepositoryApiClientAssembly(Assembly assembly, string repositoryName)
        {
            var cleanedAssemblyName = assembly.GetName().Name!.CleanString();
            return cleanedAssemblyName.Equals($"{repositoryName}.Client".CleanString())
                || cleanedAssemblyName.Equals($"{repositoryName}.Clients".CleanString());
        }

        private static bool IsValidServiceAssembly(Assembly serviceAssembly, IEnumerable<ApiDependency> dependencies)
            => IsApiClient(serviceAssembly) == false
                || dependencies.Any(dependency => IsRepositoryApiClientAssembly(serviceAssembly, dependency.Name));

        private static bool IsValidDependency(DependencyBase dependency, IEnumerable<Assembly> serviceAssemblies)
            => serviceAssemblies.Any(serviceAssembly => IsRepositoryApiClientAssembly(serviceAssembly, dependency.Name));

        protected override IEnumerable<string> Validate(ApiDependency[] dependencies, ServiceImplementation serviceImplementation)
            => Enumerable.Empty<string>()
                .Concat(
                    serviceImplementation.ServiceAssemblies
                        .Where(serviceAssembly => IsValidServiceAssembly(serviceAssembly, dependencies) == false)
                        .Select(serviceAssembly => $"Assembly {serviceAssembly.GetName().Name} is not allowed by architecture")
                )
                .Concat(
                    dependencies
                        .Where(dependency => IsValidDependency(dependency, serviceImplementation.ServiceAssemblies) == false)
                        .Select(dependency => $"Dependency {nameof(ApiDependency)}: {dependency} is not implemented in service")
                );
    }
}