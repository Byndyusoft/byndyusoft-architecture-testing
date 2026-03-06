namespace Byndyusoft.ArchitectureTesting.RulesValidation.Abstractions
{
    using System;
    using System.Linq;

    public static class ComponentsGraphPathExtensions
    {
        /// <summary>
        ///     Определяет, является ли путь <paramref name="current" /> ребенком <paramref name="possibleParent" />
        /// </summary>
        public static bool IsChildOf(this ComponentsGraphPath current, ComponentsGraphPath possibleParent)
            => possibleParent.Segments.Length == current.Segments.Length - 1
               && possibleParent.Segments.SequenceEqual(current.Segments.SkipLast(1), StringComparer.InvariantCultureIgnoreCase);
    }
}