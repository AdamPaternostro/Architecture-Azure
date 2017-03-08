using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Sample.Azure.Repository.Cache
{
    public class RuntimeCacheManager : Sample.Azure.Interface.Repository.ICacheRepository
    {
        private static MemoryCache cache = null;
        private readonly object local = new object();

        public RuntimeCacheManager()
        {
            if (cache == null)
            {
                lock (local)
                {
                    cache = new MemoryCache("RuntimeCacheManager");
                }
            }
        }


        /// <summary>
        /// Retrieves an item from the cache
        /// </summary>
        public T Get<T>(string key)
        {
            lock (local)
            {
                var res = (T)cache[key];
                return res;
            }
        } // GetItem


        /// <summary>
        /// Retrieves an item from the cache
        /// </summary>
        public object Get(string key)
        {
            lock (local)
            {
                object res = cache[key];
                return res;
            }
        } // Get


        /// <summary>
        /// Retrieves an item from the cache
        /// This does not supprot near caching, so we just do a Get
        /// </summary>
        public T GetNearCache<T>(string key, TimeSpan timeSpanForLocal)
        {
            return Get<T>(key);
        } // Get


        /// <summary>
        /// Retrieves an item from the cache
        /// This does not supprot near caching, so we just do a Get
        /// </summary>
        public object GetNearCache(string key, TimeSpan timeSpanForLocal)
        {
            return Get(key);
        } // Get


        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        public void Set(string key, object value)
        {
            lock (local)
            {
                cache.Add(key, value, DateTimeOffset.MaxValue);
            }
        } // Set


        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        public void Set(string key, object value, TimeSpan timeSpan)
        {
            lock (local)
            {
                cache.Add(key, value, new CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.UtcNow.Add(timeSpan)
                });
            }
        } // Set


        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        public void Remove(string key)
        {
            lock (local)
            {
                cache.Remove(key);
            }
        } // RemoveItem


    } // class
} // namespace
