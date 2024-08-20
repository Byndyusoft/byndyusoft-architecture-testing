namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyNameExtensions
    {
        /// <summary>
        /// Проверяет, что имя сборки <paramref name="assemblyName"/> начинается с одного из переданных префиксов <paramref name="patterns"/>
        /// </summary>
        /// <param name="assemblyName">Проверяемое имя сборки</param>
        /// <param name="patterns">Возможные префиксы</param>
        public static bool FullNameStartsWith(this AssemblyName assemblyName, params string[] patterns)
            => patterns.Any(x => assemblyName.FullName.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
    }
}