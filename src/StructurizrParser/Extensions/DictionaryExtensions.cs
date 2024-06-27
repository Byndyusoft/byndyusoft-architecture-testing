namespace Byndyusoft.ArchitectureTesting.StructurizrParser.Extensions
{
    using System;
    using System.Collections.Generic;

    internal static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            dictionary.TryGetValue(key, out var value);
            return value;
        }
    }
}