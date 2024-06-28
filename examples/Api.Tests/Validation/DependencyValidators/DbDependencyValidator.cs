namespace MusicalityLabs.Storage.Api.Tests.Validation.DependencyValidators
{
    using Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts.Dependencies;

    public class DbDependencyValidator : StorageDependencyValidatorBase<DbDependency>
    {
        public DbDependencyValidator() : base("Byndyusoft.Data.Relational")
        {
        }
    }
}