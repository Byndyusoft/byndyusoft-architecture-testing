namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.Validation.StorageDependencyValidators
{
    using Abstractions.ServiceContracts.Dependencies;

    public class DbDependencyValidator : StorageDependencyValidatorBase<DbDependency>
    {
        public DbDependencyValidator() : base("Byndyusoft.Data.Relational")
        {
        }
    }
}