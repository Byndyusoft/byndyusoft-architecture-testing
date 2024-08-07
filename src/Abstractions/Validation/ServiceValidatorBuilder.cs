namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DependencyValidators;
    using ServiceContracts.Dependencies;

    internal class ServiceValidatorBuilder : IServiceValidatorBuilder
    {
        private readonly Type _dependencyValidatorInterfaceType = typeof(IDependencyValidator);
        private readonly Type _dependencyValidatorBaseType = typeof(DependencyValidatorBase<>);
        private readonly Dictionary<Type, IDependencyValidator> _dependencyValidators = new Dictionary<Type, IDependencyValidator>();

        public IServiceValidatorBuilder AddDependencyValidator<TDependency>(DependencyValidatorBase<TDependency> dependencyValidator)
            where TDependency : DependencyBase
        {
            _dependencyValidators[typeof(TDependency)] = dependencyValidator;
            return this;
        }

        private Type? GetDependencyType(Type dependencyValidatorType)
        {
            for (var baseType = dependencyValidatorType; baseType != null; baseType = baseType.BaseType)
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == _dependencyValidatorBaseType)
                    return baseType.GetGenericArguments().Single();

            return null;
        }

        public IServiceValidatorBuilder AddDependencyValidatorsFromAssembly(Assembly assembly)
        {
            var compatibleTypes = assembly.GetTypes()
                .Where(x => x is { IsPublic: true, IsClass: true, IsAbstract: false, IsGenericType: false })
                .Where(type => _dependencyValidatorInterfaceType.IsAssignableFrom(type));
            foreach (var compatibleType in compatibleTypes)
            {
                var dependencyType = GetDependencyType(compatibleType);
                if(dependencyType == null)
                    continue;

                var defaultConstructor = compatibleType.GetConstructor(Type.EmptyTypes);
                if (defaultConstructor == null)
                    continue;

                _dependencyValidators[dependencyType] = (IDependencyValidator)defaultConstructor.Invoke(null);
            }
                
            return this;
        }

        public IReadOnlyDictionary<Type, IDependencyValidator> Build() => _dependencyValidators;
    }
}