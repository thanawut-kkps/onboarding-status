using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Runtime.Caching;
using Phatra.Core.Logging;

namespace Phatra.Core.Web.Caching
{
    public class PerUserCacheManager : IPerUserCacheManager
    {
        private readonly HttpContextBase _context;
        private readonly ILogger _log;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="log"></param>
        public PerUserCacheManager(HttpContextBase context, ILogger log)
        {
            _context = context;
            _log = log;
            _log.Debug("PerUserCacheManager.Ctor..");
        }

        protected string GetFullStringKey(string key)
        {
            _log.Debug($"GetFullStringKey: '{_context.User.Identity.Name}_{key}'");
            return _context.User.Identity.Name + '_' + key;
        }

        protected ObjectCache Cache => MemoryCache.Default;

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public virtual T Get<T>(string key)
        {
            return (T)Cache[GetFullStringKey(key)];
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            Cache.Add(new CacheItem(GetFullStringKey(key), data), policy);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(GetFullStringKey(key)));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(GetFullStringKey(key));
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(GetFullStringKey(pattern), RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in Cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            foreach (var item in Cache)
            {
                if (item.Key.StartsWith(_context.User.Identity.Name))
                {
                    Remove(item.Key);
                }
            }

        }
    }
}
