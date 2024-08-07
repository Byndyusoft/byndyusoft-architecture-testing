namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation
{
    using System;
    using System.Collections.Generic;
    using ServiceContracts.Dependencies;

    /// <summary>
    /// Настройки валидации реализации сервиса
    /// </summary>
    public class ServiceValidationConfiguration
    {
        /// <summary>
        /// Типы зависимостей, игнорируемых при валидации реализации сервиса
        /// </summary>
        public HashSet<Type> IgnoredDependencyTypes { get; } = new();

        /// <summary>
        /// Добавляет тип зависимостей <typeparamref name="TDependency"/> к списку игнорируемых при валидации
        /// </summary>
        /// <typeparam name="TDependency">Тип игнорируемой зависимости</typeparam>
        /// <returns>Настройки валидации</returns>
        public ServiceValidationConfiguration IgnoreDependency<TDependency>() where TDependency : DependencyBase
        {
            IgnoredDependencyTypes.Add(typeof(TDependency));
            return this;
        }
    }
}