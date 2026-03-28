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

        /// <summary>
        ///     Проверяет, содержится ли <paramref name="other" /> внутри <paramref name="current" />
        /// </summary>
        public static bool Contains(this ComponentsGraphPath current, ComponentsGraphPath other)
            => other.Text.StartsWith(current.Text, StringComparison.InvariantCultureIgnoreCase);

        /// <summary>
        ///     Вычисляет расстояние от <paramref name="current" /> до общего предка <paramref name="current" />
        ///     и <paramref name="other" />
        /// </summary>
        public static int CalculateDistanceFromCommonAncestor(this ComponentsGraphPath current, ComponentsGraphPath other)
        {
            var commonSegmentsCount = 0;
            while (commonSegmentsCount < current.Segments.Length && commonSegmentsCount < other.Segments.Length)
            {
                if (current.Segments[commonSegmentsCount] != other.Segments[commonSegmentsCount])
                    break;

                commonSegmentsCount++;
            }
            
            return current.Segments.Length - commonSegmentsCount;
        }
    }
}