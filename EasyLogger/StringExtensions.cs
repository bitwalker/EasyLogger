using System.Diagnostics.Contracts;

namespace EasyLogger
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns true if the string is null, or contains only whitespace.
        /// </summary>
        [Pure]
        public static bool Empty(this string s)
        {
            Contract.Ensures((Contract.Result<bool>() && string.IsNullOrWhiteSpace(s)) || (!Contract.Result<bool>() && !string.IsNullOrWhiteSpace(s)));

            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// Returns true if the string is not null and contains characters other than whitespace.
        /// </summary>
        [Pure]
        public static bool NotEmpty(this string s)
        {
            Contract.Ensures((Contract.Result<bool>() && !string.IsNullOrWhiteSpace(s)) || (!Contract.Result<bool>() && string.IsNullOrWhiteSpace(s)));

            return !string.IsNullOrWhiteSpace(s);
        }
    }
}