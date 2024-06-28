namespace MusicalityLabs.Storage.Api.Tests.Validation.DependencyValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts.Dependencies;
    using Byndyusoft.ArchitectureTesting.Abstractions.ServiceImplementations;
    using Byndyusoft.ArchitectureTesting.Abstractions.Validation.DependencyValidators;
    using Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions;

    public class StorageDependencyValidatorBase<TDependency> : DependencyValidatorBase<TDependency>
        where TDependency : StorageDependencyBase
    {
        private readonly string _storageClientAssemblyName;

        protected StorageDependencyValidatorBase(string storageClientAssemblyName)
        {
            _storageClientAssemblyName = storageClientAssemblyName ?? throw new ArgumentNullException(nameof(storageClientAssemblyName));
        }

        protected override IEnumerable<string> Validate(TDependency[] dependencies, ServiceImplementation serviceImplementation)
        {
            var dependency = dependencies.SingleOrDefault();
            var compatibleAssemblies = serviceImplementation
                .ServiceAssemblies.Where(assembly => assembly.IsAssemblyNameEquals(_storageClientAssemblyName)).ToArray();

            var errors = Enumerable.Empty<string>();
            if (dependency == null && compatibleAssemblies.Length > 0)
                errors = errors.Concat(compatibleAssemblies.Select(x => $"Assembly {x.GetName().Name} is not allowed by architecture"));
            else if (dependency != null && compatibleAssemblies.Length != 1)
                errors = errors.Append($"Dependency {typeof(TDependency).Name}: {dependency} is not properly implemented in service");

            return errors;
        }
    }
}