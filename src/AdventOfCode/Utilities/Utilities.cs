using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Utilities
{
    public static class Utilities
    {
        public static T[] Numbers<T>(this string input)
        {
            MatchCollection matches = Regex.Matches(input, @"-?\d+");

            return matches
                  .Select(m => m.Value)
                  .Select(m => (T)Convert.ChangeType(m, typeof(T)))
                  .ToArray();
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) where TValue : new()
        {
            if (!@this.TryGetValue(key, out TValue value))
            {
                value = new TValue();
                @this.Add(key, value);
            }

            return value;
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TValue> factory)
        {
            if (!@this.TryGetValue(key, out TValue value))
            {
                value = factory();
                @this.Add(key, value);
            }

            return value;
        }
    }
}
