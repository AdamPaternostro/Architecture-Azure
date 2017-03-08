using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.Table
{
    public class BaseDataServiceContext
    {
        private static List<string> tableCreatedList = new List<string>();

        private Microsoft.WindowsAzure.Storage.Auth.StorageCredentials storageCredentials = null;
        private Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = null;
        private Microsoft.WindowsAzure.Storage.Table.CloudTableClient tableClient = null;
        protected Microsoft.WindowsAzure.Storage.Table.CloudTable cloudTable = null;

        /// <summary>
        /// Sets the connection to Azure
        /// Creates the table if it does not exist
        /// </summary>
        public BaseDataServiceContext()
        {
            storageCredentials = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                Sample.Azure.Common.Setting.SettingService.CloudStorageAccountName,
                Sample.Azure.Common.Setting.SettingService.CloudStorageKey);

            storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(storageCredentials,
                new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageBlobEndPoint),
                new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageQueueEndPoint),
                new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageTableEndPoint),
                new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageFileEndPoint));

            tableClient = storageAccount.CreateCloudTableClient();

            cloudTable = tableClient.GetTableReference(this.TableName);

            if (!tableCreatedList.Contains(this.TableName))
            {
                cloudTable.CreateIfNotExists();
                tableCreatedList.Add(this.TableName);
            }
        }

        public virtual string TableName { get;}

    } // class
} // namespace
