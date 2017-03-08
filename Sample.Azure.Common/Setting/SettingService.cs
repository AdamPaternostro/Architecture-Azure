using System.Collections.Generic;
using System.Linq;


namespace Sample.Azure.Common.Setting
{
    /// <summary>
    /// This is used to configure settings to the classes that are use by DI
    /// This is designed to avoid using an app/web config (which are going away)
    /// </summary>
    public class SettingService
    {
        private static Dictionary<string, string> settingDictionary = new Dictionary<string, string>();

        public static void Set(string key, string value)
        {
            if (settingDictionary.ContainsKey(key))
            {
                settingDictionary[key] = value;
            }
            else
            {
                settingDictionary.Add(key, value);
            }
        }

        private static string Get(string key)
        {
            if (settingDictionary.ContainsKey(key))
            {
                return settingDictionary[key];
            }
            else
            {
                return null;
            }
        }

        public static string RedisCacheConnection { get { return Get("RedisCacheConnection"); } }

        public static string CloudStorageAccountName { get { return Get("CloudStorageAccountName"); } }

        public static string CloudStorageKey { get { return Get("CloudStorageKey"); } }

        public static string CloudStorageBlobEndPoint { get { return Get("CloudStorageBlobEndPoint"); } }

        public static string CloudStorageQueueEndPoint { get { return Get("CloudStorageQueueEndPoint"); } }

        public static string CloudStorageTableEndPoint { get { return Get("CloudStorageTableEndPoint"); } }

        public static string CloudStorageFileEndPoint { get { return Get("CloudStorageFileEndPoint"); } }

        public static string SaveAndReadBlobTimeoutInMinutes { get { return Get("SaveAndReadBlobTimeoutInMinutes"); } }

        public static string SaveAndReadQueueTimeoutInMinutes { get { return Get("SaveAndReadQueueTimeoutInMinutes"); } }

        public static string SearchIndexerServiceCheckQueueIntervalInMilliseconds { get { return Get("SearchIndexerService-CheckQueue-Interval-In-Milliseconds"); } }

        public static string AzureSearchName { get { return Get("Azure-Search-Name"); } }

        public static string AzureSearchApiKey { get { return Get("Azure-Search-ApiKey"); } }

        public static List<string> AzureSearchCORS {
            get {
                return Get("AzureSearchCORS").Split(new char[] { ',' }).ToList<string>();
            }
        }

        public static string AzureSearchQueryKey { get { return Get("Azure-Search-QueryKey"); } }



        public static string DocumentDBEndPoint { get { return Get("DocumentDBEndPoint"); } }

        public static string DocumentDBAuthKey { get { return Get("DocumentDBAuthKey"); } }

        public static string DocumentDBDatabase { get { return Get("DocumentDBDatabase"); } }

        public static string DocumentDBCollection { get { return Get("DocumentDBCollection"); } }


    } // class
} // namespace
