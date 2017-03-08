using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Config
{
    public class Configuration
    {
        private static Environment.SystemEnvironment systemEnvironment;

        public static Environment.SystemEnvironment SystemEnvironment
        {
            get
            {
                return Configuration.systemEnvironment;
            }
        }


        /// <summary>
        /// Configures the system will settings that can be consumed accross tiers
        /// Set all the config values based upon environment
        /// This will be changed to read from a config file or database
        /// </summary>
        /// <param name="systemEnvironment"></param>
        public static void Configure()
        {
            Configuration.systemEnvironment = EnvironmentStartup.GetEnvironment;
            string databaseConnectionString = EnvironmentStartup.GetDatabaseConnectionString;


            // Configure DI so we can resolve our Configuration Repository
            RegisterTypes.Configure(Configuration.systemEnvironment);


            string systemEnv = Configuration.systemEnvironment.ToString().ToUpper();

            Sample.Azure.Common.Setting.SettingService.Set("RedisCacheConnection", "samplebase.redis.cache.windows.net,abortConnect=false,ssl=true,password=<<REMOVED>>");

            Sample.Azure.Common.Setting.SettingService.Set("CloudStorageAccountName", "samplebase");
            Sample.Azure.Common.Setting.SettingService.Set("CloudStorageKey", "<<REMOVED>>");
            Sample.Azure.Common.Setting.SettingService.Set("CloudStorageBlobEndPoint", "https://samplebase.blob.core.windows.net");
            Sample.Azure.Common.Setting.SettingService.Set("CloudStorageQueueEndPoint", "https://samplebase.queue.core.windows.net");
            Sample.Azure.Common.Setting.SettingService.Set("CloudStorageTableEndPoint", "https://samplebase.table.core.windows.net");
            Sample.Azure.Common.Setting.SettingService.Set("CloudStorageFileEndPoint", "https://samplebase.file.core.windows.net");

            Sample.Azure.Common.Setting.SettingService.Set("SaveAndReadBlobTimeoutInMinutes", "5");
            Sample.Azure.Common.Setting.SettingService.Set("SaveAndReadQueueTimeoutInMinutes", "5");
            Sample.Azure.Common.Setting.SettingService.Set("SearchIndexerService-CheckQueue-Interval-In-Milliseconds", "500");
            Sample.Azure.Common.Setting.SettingService.Set("Azure-Search-Name", "samplebase"); //.search.windows.net");
            Sample.Azure.Common.Setting.SettingService.Set("Azure-Search-ApiKey", "<<REMOVED>>");
            Sample.Azure.Common.Setting.SettingService.Set("AzureSearchCORS", "*"); // comma seperated
            Sample.Azure.Common.Setting.SettingService.Set("Azure-Search-QueryKey", "<<REMOVED>>");

            Sample.Azure.Common.Setting.SettingService.Set("DocumentDBEndPoint", "https://samplebase.documents.azure.com:443/");
            Sample.Azure.Common.Setting.SettingService.Set("DocumentDBAuthKey", "<<REMOVED>>");
            Sample.Azure.Common.Setting.SettingService.Set("DocumentDBDatabase", "SampleBase");
            Sample.Azure.Common.Setting.SettingService.Set("DocumentDBCollection", "Customers");



        } // Configure


    }
}
