namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.DependencyValidators
{
    using System.Collections.Generic;
    using System.Linq;
    using ServiceContracts.Dependencies;
    using ServiceImplementations;

    /// <summary>
    /// Валидатор определенного типа зависимостей сервиса на соответствие описанию
    /// </summary>
    /// <typeparam name="TDependency">Тип валидируемых зависимостей сервиса</typeparam>
    public abstract class DependencyValidatorBase<TDependency> : IDependencyValidator where TDependency : DependencyBase
    {
        protected abstract IEnumerable<string> Validate(TDependency[] dependencies, ServiceImplementation serviceImplementation);

        /// <summary>
        /// Валидирует зависимости сервиса на соответствие описанию
        /// </summary>
        /// <param name="dependencies">Зависимости сервиса</param>
        /// <param name="serviceImplementation">Реализация сервиса</param>
        /// <returns>Список выявленных проблем</returns>
        public IEnumerable<string> Validate(DependencyBase[] dependencies, ServiceImplementation serviceImplementation)
            => Validate(dependencies.Cast<TDependency>().ToArray(), serviceImplementation);
    }
}