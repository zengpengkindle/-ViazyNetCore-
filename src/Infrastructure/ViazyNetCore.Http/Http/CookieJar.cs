using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ViazyNetCore.Http
{
    /// <summary>
    /// A collection of CaesarCookies that can be attached to one or more CaesarRequests, either explicitly via WithCookies
    /// or implicitly via a CookieSession. Stores cookies received via Set-Cookie response headers.
    /// </summary>
    public class CookieJar : IReadOnlyCollection<CaesarCookie>
    {
        private readonly ConcurrentDictionary<string, CaesarCookie> _dict = new();

        /// <summary>
        /// Adds a cookie to the jar or replaces one with the same Name/Domain/Path.
        /// Throws InvalidCookieException if cookie is invalid.
        /// </summary>
        /// <param name="name">Name of the cookie.</param>
        /// <param name="value">Value of the cookie.</param>
        /// <param name="originUrl">URL of request that sent the original Set-Cookie header.</param>
        /// <param name="dateReceived">Date/time that original Set-Cookie header was received. Defaults to current date/time. Important for Max-Age to be enforced correctly.</param>
        public CookieJar AddOrReplace(string name, object value, string originUrl, DateTimeOffset? dateReceived = null) =>
            AddOrReplace(new CaesarCookie(name, value.ToInvariantString(), originUrl, dateReceived));

        /// <summary>
        /// Adds a cookie to the jar or replaces one with the same Name/Domain/Path.
        /// Throws InvalidCookieException if cookie is invalid.
        /// </summary>
        public CookieJar AddOrReplace(CaesarCookie cookie)
        {
            if (!TryAddOrReplace(cookie, out var reason))
                throw new InvalidCookieException(reason);

            return this;
        }

        public CaesarCookie TryGet(string name)
        {
            _dict.TryGetValue(name, out var value);
            return value;

        }

        /// <summary>
        /// Adds a cookie to the jar or updates if one with the same Name/Domain/Path already exists,
        /// but only if it is valid and not expired.
        /// </summary>
        /// <returns>true if cookie is valid and was added or updated. If false, provides descriptive reason.</returns>
        public bool TryAddOrReplace(CaesarCookie cookie, out string reason)
        {
            if (!cookie.IsValid(out reason))
                return false;

            if (cookie.IsExpired(out reason))
            {
                // when server sends an expired cookie, it's effectively an instruction for client to delete it.
                // https://stackoverflow.com/a/53573622/62600
                _dict.TryRemove(cookie.GetKey(), out _);
                return false;
            }

            cookie.Lock(); // makes immutable
            _dict[cookie.GetKey()] = cookie;

            return true;
        }

        /// <summary>
        /// Removes all cookies matching the given predicate.
        /// </summary>
        public CookieJar Remove(Func<CaesarCookie, bool> predicate)
        {
            var keys = _dict.Where(kv => predicate(kv.Value)).Select(kv => kv.Key).ToList();
            foreach (var key in keys)
                _dict.TryRemove(key, out _);
            return this;
        }

        /// <summary>
        /// Removes all cookies from this CookieJar
        /// </summary>
        public CookieJar Clear()
        {
            _dict.Clear();
            return this;
        }

        /// <inheritdoc/>
        public IEnumerator<CaesarCookie> GetEnumerator() => _dict.Values.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

        /// <inheritdoc/>
        public int Count => _dict.Count;
    }

    /// <summary>
    /// Exception thrown when attempting to add or update an invalid CaesarCookie to a CookieJar.
    /// </summary>
    public class InvalidCookieException : Exception
    {
        /// <summary>
        /// Creates a new InvalidCookieException.
        /// </summary>
        public InvalidCookieException(string reason) : base(reason) { }
    }
}
