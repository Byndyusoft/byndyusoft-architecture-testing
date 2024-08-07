namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Очищает строку от разделителей и приводит ее к нижнему регистру
        /// </summary>
        /// <param name="value">Очищаемая строка</param>
        public static string CleanString(this string value)
            => value.Replace("-", "").Replace(".", "").Replace("_", "").ToLowerInvariant();
    }
}