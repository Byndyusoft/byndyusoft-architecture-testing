namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DependencyValidators;
    using ServiceContracts;
    using ServiceContracts.Dependencies;
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
        /// <param name="contract">Описание сериса</param>
        /// <param name="implementation">Реализация сервиса</param>
        /// <returns>Список выявленных проблем</returns>
        public IEnumerable<string> Validate(ServiceContract contract, ServiceImplementation implementation)
        {
            var dependenciesByValidators = _dependencyValidators.Values.ToDictionary(x => x, _ => new List<DependencyBase>());
            foreach (var dependency in contract.Dependencies)
            {
                if(_dependencyValidators.TryGetValue(dependency.GetType(), out var validator) == false)
                    continue;

                dependenciesByValidators[validator].Add(dependency);
            }

            return dependenciesByValidators.SelectMany(pair => pair.Key.Validate(pair.Value.ToArray(), implementation));
        }
    }
}