namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation
{
    using System.Reflection;
    using DependencyValidators;
    using ServiceContracts.Dependencies;

    /// <summary>
    /// Контракт построителя валидатора сервиса
    /// </summary>
    public interface IServiceValidatorBuilder
    {
        /// <summary>
        /// Добавляет для зависомсти с типом <typeparamref name="TDependency"/> валидатор <paramref name="dependencyValidator"/>
        /// </summary>
        /// <typeparam name="TDependency">Тип проверяемой зависимости</typeparam>
        /// <param name="dependencyValidator">Валидатор зависимости</param>
        IServiceValidatorBuilder AddDependencyValidator<TDependency>(DependencyValidatorBase<TDependency> dependencyValidator)
            where TDependency : DependencyBase;

        /// <summary>
        /// Добавляет все валидаторы из сборки <paramref name="assembly"/>
        /// </summary>
        /// <param name="assembly">Сборка, из которой будут загружены валидаторы</param>
        IServiceValidatorBuilder AddDependencyValidatorsFromAssembly(Assembly assembly);
    }
}