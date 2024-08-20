namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyExtensions
    {
        /// <summary>
        /// Проверяет, что сборка <paramref name="assembly"/> является корневой сборкой сервиса <paramref name="serviceName"/>
        /// </summary>
        /// <param name="assembly">Проверяемая сборка</param>
        /// <param name="serviceName">Название сервиса</param>
        public static bool IsServiceRootAssembly(this Assembly assembly, string serviceName)
            => assembly.GetName().Name!.CleanString().Equals(serviceName.CleanString());

        /// <summary>
        /// Проверяет, что имя сборки <paramref name="assembly"/> совпадает с переданным <paramref name="assemblyName"/>
        /// </summary>
        /// <param name="assembly">Проверяемая сборка</param>
        /// <param name="assemblyName">Предполагаемое имя сборки</param>
        public static bool AssemblyNameEquals(this Assembly assembly, string assemblyName)
            => assembly.GetName().Name!.Equals(assemblyName);

        private static IEnumerable<Assembly> GetServiceAssemblies(
            IDictionary<string, Assembly> loadedAssemblies,
            ISet<string> processedAssemblyNames,
            Func<AssemblyName, bool>? assembliesFilter,
            Assembly assembly
        ) => Enumerable.Empty<Assembly>()
            .Append(assembly)
            .Concat(
                assembly.GetReferencedAssemblies()
                    .Where(x => assembliesFilter?.Invoke(x) ?? true)
                    .Where(x => processedAssemblyNames.Contains(x.FullName) == false)
                    .Where(x => x.FullNameStartsWith("System.", "Microsoft.") == false)
                    .SelectMany(
                        x =>
                        {
                            processedAssemblyNames.Add(x.FullName);
                            if (loadedAssemblies.TryGetValue(x.FullName, out var loadedAssembly) == false)
                            {
                                loadedAssembly = Assembly.Load(x);
                                loadedAssemblies[x.FullName] = loadedAssembly;
                            }

                            return GetServiceAssemblies(loadedAssemblies, processedAssemblyNames, assembliesFilter, loadedAssembly);
                        }
                    )
            );

        /// <summary>
        /// Строит коллекцию сборок, используемых в сервисе
        /// </summary>
        /// <param name="rootAssembly">Корневая сборка сервиса</param>
        /// <param name="assembliesFilter">Предикат для фильтрации сборок по имени</param>
        public static Assembly[] GetServiceAssemblies(this Assembly rootAssembly, Func<AssemblyName, bool>? assembliesFilter = null)
            => GetServiceAssemblies(
                    AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => assembliesFilter?.Invoke(x.GetName()) ?? true)
                        .GroupBy(x => x.GetName().FullName)
                        .ToDictionary(group => group.Key, group => group.First()),
                    new HashSet<string>(),
                    assembliesFilter,
                    rootAssembly
                )
                .ToArray();
    }
}