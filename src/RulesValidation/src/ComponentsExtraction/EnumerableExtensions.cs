namespace Byndyusoft.ArchitectureTesting.RulesValidation.ComponentsExtraction
{
    using System;
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Проецирует элемены последовательности <paramref name="source" /> в новую форму с помощью <paramref name="selector" />.
        ///     В результирующую последовательность попадают только отличные от null значения.
        /// </summary>
        /// <param name="source">Последовательность значений, для которых вызывается функция преобразования</param>
        /// <param name="selector">Функция преобразования, применяемая к каждому элементу исходной последовательности</param>
        /// <typeparam name="TSource">Тип элементов исходной последовательности</typeparam>
        /// <typeparam name="TResult">Тип элементов результирующей последовательности</typeparam>
        /// <returns>Результирующая последовательность</returns>
        public static IEnumerable<TResult> SelectPresentedValues<TSource, TResult>(
            this IEnumerable<TSource?> source,
            Func<TSource?, TResult?> selector
        ) where TSource : class
          where TResult : class
        {
            foreach (var item in source)
            {
                var result = selector(item);

                if (result != null)
                    yield return result;
            }
        }
    }
}