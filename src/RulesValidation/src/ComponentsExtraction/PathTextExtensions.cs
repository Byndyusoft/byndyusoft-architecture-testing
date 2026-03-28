namespace Byndyusoft.ArchitectureTesting.RulesValidation.ComponentsExtraction
{
    using System.Linq;

    public static class PathTextExtensions
    {
        /// <summary>
        ///     Добавляет новые сегменты <paramref name="newSegments" /> к уже существующему пути <paramref name="basePath" />
        /// </summary>
        /// <param name="basePath">Уже существующий путь</param>
        /// <param name="newSegments">Новые сегменты</param>
        /// <returns>Путь с добавленными сегментами</returns>
        public static string Combine(this string basePath, params string[] newSegments)
            => newSegments.Aggregate(basePath, (current, newSegment) => current + $".{newSegment}");
    }
}