namespace Byndyusoft.ArchitectureTesting.Validation.DependencyValidators
{
    using Abstractions.ServiceContracts.Dependencies;

    public class DbDependencyValidator : StorageDependencyValidatorBase<DbDependency>
    {
        public DbDependencyValidator() : base("Byndyusoft.Data.Relational")
        {
        }
    }
}