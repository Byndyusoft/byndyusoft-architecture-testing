namespace Byndyusoft.ArchitectureTesting.RulesValidation.ComponentsExtraction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstractions;
    using ArchUnitNET.Domain;
    using ArchUnitNET.Fluent.Slices;
    using ArchUnitNET.Loader;
    using Common.Abstractions.Extensions;

    /// <summary>
    ///     Базовый класс экстрактора компонентов приложения
    /// </summary>
    /// <typeparam name="TEntryPoint">Точки входа приложения, обычно Startup или Program</typeparam>
    public abstract class ComponentsExtractorBase<TEntryPoint> where TEntryPoint : class
    {
        /// <summary>
        ///     Корневой неймспейс приложения
        /// </summary>
        protected abstract string RootNamespace { get; }

        /// <summary>
        ///     Извлекает компоненты, из которых состоит приложение
        /// </summary>
        /// <returns>Компоненты, из которых состоит приложение</returns>
        public Component[] Extract()
        {
            var rootAssembly = typeof(TEntryPoint).Assembly;
            var serviceAssemblies = rootAssembly.GetServiceAssemblies(assemblyName => assemblyName.FullNameStartsWith(RootNamespace));
            var architecture = new ArchLoader()
                .LoadAssemblies(serviceAssemblies)
                .Build();

            var slices = SliceRuleDefinition
                .Slices()
                .Matching($"{RootNamespace}.(*)")
                .GetObjects(architecture)
                .ToArray();
            var slicePathTexts = slices
                .Select(slice => slice.Identifier.Identifier)
                .ToHashSet(StringComparer.InvariantCultureIgnoreCase);

            var components = new List<Component>();
            var componentsToSlicesMap = new Dictionary<Component, Slice>();
            foreach (var slice in slices)
            {
                var component = new Component { Path = GetComponentsGraphPath(slicePathTexts, slice) };
                components.Add(component);
                componentsToSlicesMap.Add(component, slice);
            }
            
            FillComponentsChildren(components);
            FillComponentsDependencies(components, componentsToSlicesMap);

            return components.ToArray();
        }

        private static ComponentsGraphPath GetComponentsGraphPath(HashSet<string> slicePathTexts, Slice slice)
        {
            var slicePathSegments = slice.Identifier.Identifier.Split('.');
            
            var slicePathText = slicePathSegments[0];
            var componentGraphPathSegment = slicePathText;
            var componentGraphPathSegments = new List<string>();
            for (var i = 1; i < slicePathSegments.Length; i++)
            {
                var slicePathSegment = slicePathSegments[i];

                if (slicePathTexts.Contains(slicePathText))
                {
                    componentGraphPathSegments.Add(componentGraphPathSegment);
                    componentGraphPathSegment = slicePathSegment;
                }
                else
                    componentGraphPathSegment = componentGraphPathSegment.Combine(slicePathSegment);

                slicePathText = slicePathText.Combine(slicePathSegment);
            }

            componentGraphPathSegments.Add(componentGraphPathSegment);
            return new ComponentsGraphPath(componentGraphPathSegments.ToArray());
        }

        private static void FillComponentsChildren(IReadOnlyList<Component> components)
        {
            foreach (var component in components)
                component.Children = components
                    .Where(
                        otherComponent => otherComponent.Equals(component) == false
                                          && otherComponent.Path.IsChildOf(component.Path)
                    )
                    .ToArray();
        }

        private static void FillComponentsDependencies(
            IReadOnlyList<Component> components,
            IReadOnlyDictionary<Component, Slice> componentsToSlicesMap
        )
        {
            var slicesByContainedTypes = new Dictionary<IType, Slice>();
            foreach (var slice in componentsToSlicesMap.Values)
            foreach (var type in slice.Types)
                slicesByContainedTypes.Add(type, slice);

            var slicesToComponentsMap = componentsToSlicesMap.ToDictionary(x => x.Value, x => x.Key);
            foreach (var component in components)
            {
                var slice = componentsToSlicesMap[component];
                component.Dependencies = slice.Dependencies
                    .SelectPresentedValues(sliceTypeDependency => slicesByContainedTypes.GetValueOrDefault(sliceTypeDependency!.Target))
                    .Distinct()
                    .Where(sliceDependency => slice.Equals(sliceDependency) == false)
                    .Select(x => slicesToComponentsMap[x])
                    .ToArray();
            }
        }
    }
}