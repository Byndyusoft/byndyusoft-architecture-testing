namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.Abstractions.Validation.Extensions
{
    using System.Reflection;

    public static class AssemblyExtensions
    {
        /// <summary>
        ///     Проверяет, что сборка <paramref name="assembly" /> является корневой сборкой сервиса
        ///     <paramref name="serviceName" />
        /// </summary>
        /// <param name="assembly">Проверяемая сборка</param>
        /// <param name="serviceName">Название сервиса</param>
        public static bool IsServiceRootAssembly(this Assembly assembly, string serviceName)
            => assembly.GetName().Name!.CleanString().Equals(serviceName.CleanString());
    }
}