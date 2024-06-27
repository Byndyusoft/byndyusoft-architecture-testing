namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.DependencyValidators
{
    using System.Collections.Generic;
    using ServiceContracts.Dependencies;
    using ServiceImplementations;

    internal interface IDependencyValidator
    {
        IEnumerable<string> Validate(DependencyBase[] dependencies, ServiceImplementation serviceImplementation);
    }
}