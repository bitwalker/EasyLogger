using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

namespace EasyLogger
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Consumes a sequence while applying a function to each element.
        /// </summary>
        public static void Do<T>(this IEnumerable<T> source, Action<T> f)
        {
            Contract.Requires(source != null);
            Contract.Requires(f != null);

            foreach (var t in source)
            {
                f(t);
            }
        }
    }
}
