namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests
{
    using Abstractions.ServiceContracts.Dependencies;
    using Abstractions.ServiceImplementations;

    public class DependencyValidatorTestCase
    {
        public string Description { get; set; }

        public DependencyBase[] Dependencies { get; set; }

        public ServiceImplementation ServiceImplementation { get; set; }

        public string[] ExpectedErrors { get; set; }

        public override string ToString() => Description;
    }
}