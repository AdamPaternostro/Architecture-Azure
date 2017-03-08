using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Cache
{
    public interface ICacheManager
    {
        T Get<T>(string key);

        object Get(string key);

        void Set(string key, object value);

        void Set(string key, object value, TimeSpan timeSpan);

        void Remove(string key);

        T GetNearCache<T>(string key, TimeSpan timeSpanForLocal);

        object GetNearCache(string key, TimeSpan timeSpanForLocal);

    } // interface
} // namespace
