namespace Byndyusoft.ArchitectureTesting.Validation.StorageDependencyValidators
{
    using Byndyusoft.ArchitectureTesting.Abstractions.ServiceContracts.Dependencies;

    public class DbDependencyValidator : StorageDependencyValidatorBase<DbDependency>
    {
        public DbDependencyValidator() : base("Byndyusoft.Data.Relational")
        {
        }
    }
}