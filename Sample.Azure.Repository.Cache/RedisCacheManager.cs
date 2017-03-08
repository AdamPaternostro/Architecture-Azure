using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using StackExchange.Redis;

namespace Sample.Azure.Repository.Cache
{
    public class RedisCacheManager : Sample.Azure.Interface.Repository.ICacheRepository
    {
        // https://msdn.microsoft.com/en-us/library/azure/dn690521.aspx
        // https://azure.microsoft.com/en-us/documentation/articles/cache-dotnet-how-to-use-azure-redis-cache/
        public RedisCacheManager()
        {
        }

        /// <summary>
        /// Allows for retrying and thread safe connection to Redis
        /// </summary>
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(Sample.Azure.Common.Setting.SettingService.RedisCacheConnection);
        }); // lazyConnection


        /// <summary>
        /// Gets the connection to Redis
        /// </summary>
        private static ConnectionMultiplexer RedisConnection
        {
            get
            {
                return lazyConnection.Value;
            }
        } // RedisConnection


        /// <summary>
        /// Retrieves an item from the cache
        /// </summary>
        public T Get<T>(string key)
        {
            IDatabase cache = RedisConnection.GetDatabase();
            return Deserialize<T>(cache.StringGet(key));
        } // Get


        /// <summary>
        /// Retrieves an item from the cache
        /// </summary>
        public object Get(string key)
        {
            IDatabase cache = RedisConnection.GetDatabase();
            return Deserialize<object>(cache.StringGet(key));
        } // Get


        /// <summary>
        /// Retrieves an item from the cache
        /// 1st tries the local cache (System.Runtime)
        ///   If found => okay
        ///   If not found => Get from central server and cache locally
        /// NOTE: Near Caching is used: This means we will bring items from a central distributed cache (Redis) to be 
        /// cached on the local machine for short period of time so we do not have the overhead of the distributed cache call
        /// </summary>
        public T GetNearCache<T>(string key, TimeSpan timeSpanForLocal)
        {
            RuntimeCacheManager localCache = new RuntimeCacheManager();
            T localCacheResult = localCache.Get<T>(key);
            if (localCacheResult != null)
            {
                return localCacheResult;
            }
            else
            {
                IDatabase cache = RedisConnection.GetDatabase();
                T centralCacheResult = Deserialize<T>(cache.StringGet(key));
                if (centralCacheResult != null)
                {
                    // cache locally
                    localCache.Set(key, centralCacheResult, timeSpanForLocal);
                }
                return centralCacheResult;
            }
        } // Get


        /// <summary>
        /// Retrieves an item from the cache
        /// 1st tries the local cache (System.Runtime)
        ///   If found => okay
        ///   If not found => Get from central server and cache locally
        /// NOTE: Near Caching is used: This means we will bring items from a central distributed cache (Redis) to be 
        /// cached on the local machine for short period of time so we do not have the overhead of the distributed cache call
        /// </summary>
        public object GetNearCache(string key, TimeSpan timeSpanForLocal)
        {
            RuntimeCacheManager localCache = new RuntimeCacheManager();
            object localCacheResult = localCache.Get(key);
            if (localCacheResult != null)
            {
                return localCacheResult;
            }
            else
            {
                IDatabase cache = RedisConnection.GetDatabase();
                object centralCacheResult = Deserialize<object>(cache.StringGet(key));
                if (centralCacheResult != null)
                {
                    // cache locally
                    localCache.Set(key, centralCacheResult, timeSpanForLocal);
                }
                return centralCacheResult;
            }
        } // Get


        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        public void Set(string key, object value)
        {
            IDatabase cache = RedisConnection.GetDatabase();
            cache.StringSet(key, Serialize(value));

            // Clear the local cache on this server
            RuntimeCacheManager localCache = new RuntimeCacheManager();
            localCache.Remove(key);

        } // Set


        /// <summary>
        /// Adds an item to the cache
        /// </summary>
        public void Set(string key, object value, TimeSpan timeSpan)
        {
            IDatabase cache = RedisConnection.GetDatabase();
            cache.StringSet(key, Serialize(value), timeSpan);

            // Clear the local cache on this server
            RuntimeCacheManager localCache = new RuntimeCacheManager();
            localCache.Remove(key);

        } // Set


        /// summary>
        /// Removes an item from the cache
        /// </summary>
        public void Remove(string key)
        {
            IDatabase cache = RedisConnection.GetDatabase();
            cache.KeyDelete(key);
        } // Remove



        /// <summary>
        /// Serializes the data for Redis
        /// </summary>
        private byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        } // Serialize



        /// <summary>
        /// Deserializes the data from Redis
        /// </summary>
        private T Deserialize<T>(byte[] stream)
        {
            if (stream == null)
            {
                return default(T);
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream(stream))
            {
                T result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        } // Deserialize

    } // class
} // namespace
