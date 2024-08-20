namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DependencyValidators;
    using ServiceContracts;
    using ServiceImplementations;

    /// <summary>
    /// Валидатор реализации сервиса на соответствие описанию
    /// </summary>
    public class ServiceValidator
    {
        private readonly IReadOnlyDictionary<Type, IDependencyValidator> _dependencyValidators;

        internal ServiceValidator(IReadOnlyDictionary<Type, IDependencyValidator> dependencyValidators)
        {
            _dependencyValidators = dependencyValidators ?? throw new ArgumentNullException(nameof(dependencyValidators));
        }

        /// <summary>
        /// Валидирует реализацию сервиса <paramref name="implementation"/> на соответствие описанию <paramref name="contract"/>
        /// </summary>
        /// <param name="contract">Описание сервиса</param>
        /// <param name="implementation">Реализация сервиса</param>
        /// <param name="validationConfiguration">Настройки валидации реализации сервиса</param>
        /// <returns>Список выявленных проблем</returns>
        public IEnumerable<string> Validate(
            ServiceContract contract,
            ServiceImplementation implementation,
            ServiceValidationConfiguration? validationConfiguration = null
        )
        {
            var configuration = validationConfiguration ?? new ServiceValidationConfiguration();
            return contract.Dependencies
                .GroupBy(dependency => dependency.GetType())
                .Where(dependenciesGroup => configuration.IgnoredDependencyTypes.Contains(dependenciesGroup.Key) == false)
                .SelectMany(
                    dependenciesGroup =>
                        {
                            if (_dependencyValidators.TryGetValue(dependenciesGroup.Key, out var validator) == false)
                                throw new InvalidOperationException($"Validator for dependency {dependenciesGroup.Key.Name} must be provided");

                            return validator.Validate(dependenciesGroup.ToArray(), implementation);
                        }
                );
        }
    }
}