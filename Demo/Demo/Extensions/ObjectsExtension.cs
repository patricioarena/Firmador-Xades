using System.Collections.Generic;
using System.Linq;

namespace Demo.Extensions
{
    public static class ObjectsExtension
    {
        /// <summary>
        /// Verifies if an object is null.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <returns>True if the object is null, otherwise false.</returns>
        public static bool IsNull<T>(this T obj) => obj == null;

        /// <summary>
        /// Verifies if an object is not null.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object to check.</param>
        /// <returns>True if the object is not null, otherwise false.</returns>
        public static bool NonNull<T>(this T obj) => obj != null;

        /// <summary>
        /// Checks if a string is null or empty.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string is null or empty, otherwise false.</returns>
        public static bool IsEmpty(this string str) => string.IsNullOrEmpty(str);

        /// <summary>
        /// Checks if a string is not null or empty.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <returns>True if the string is not null or empty, otherwise false.</returns>
        public static bool IsNotEmpty(this string str) => !string.IsNullOrEmpty(str);

        /// <summary>
        /// Checks if a collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>True if the collection is null or contains no elements, otherwise false.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();

        /// <summary>
        /// Checks if a collection is not null and contains at least one element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to check.</param>
        /// <returns>True if the collection is not null and contains at least one element, otherwise false.</returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> collection) => collection != null && collection.Any();

        /// <summary>
        /// Checks if a boolean value is true.
        /// </summary>
        /// <param name="value">The boolean value to check.</param>
        /// <returns>True if the value is true, otherwise false.</returns>
        public static bool IsTrue(this bool value) => value;

        /// <summary>
        /// Checks if a boolean value is false.
        /// </summary>
        /// <param name="value">The boolean value to check.</param>
        /// <returns>True if the value is false, otherwise false.</returns>
        public static bool IsFalse(this bool value) => !value;
    }
}
