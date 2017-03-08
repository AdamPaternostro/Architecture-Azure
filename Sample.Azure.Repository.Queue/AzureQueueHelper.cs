using System;
using System.Collections.Generic;
using System.IO;

namespace Sample.Azure.Repository.Queue
{
    /// <summary>
    /// Shield this from other classes
    /// </summary>
    internal class AzureQueueHelper
    {
        private const int DEFAULT_SAVE_AND_READ_QUEUE_TIMEOUT_IN_MINUTES = 5;
        private const int DEFAULT_SAVE_QUEUE_RETRY_ATTEMPTS = 4;
        private const int DEFAULT_SAVE_QUEUE_RETRY_WAIT_IN_MILLISECONDS = 100;

        private static Dictionary<string, bool> storageInitializedDictionary = new Dictionary<string, bool>();
        private static object gate = new Object();
        private static Dictionary<string, Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient> queueStorageDictionary = new Dictionary<string, Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient>();


        public AzureQueueHelper() { }


        /// <summary>
        /// Removes a message from the queue
        /// </summary>
        /// <param name="queueContainer"></param>
        /// <param name="cloudQueueMessage"></param>
        public void DeleteQueueItem(string queueContainer, Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage cloudQueueMessage)
        {
            InitializeStorage(queueContainer);

            string containerName = queueContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient queueStorage = queueStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Queue.CloudQueue cloudQueue = queueStorage.GetQueueReference(containerName);
            cloudQueue.DeleteMessage(cloudQueueMessage);
        } // DeleteQueueItem


        public void DeleteQueueItem(string queueContainer, string messageId, string popReceipt)
        {
            InitializeStorage(queueContainer);

            string containerName = queueContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient queueStorage = queueStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Queue.CloudQueue cloudQueue = queueStorage.GetQueueReference(containerName);
            cloudQueue.DeleteMessage(messageId, popReceipt);
        } // DeleteQueueItem


        /// <summary>
        /// Returns the 1st message as a queue item
        /// </summary>
        /// <param name="queueContainer"></param>
        /// <returns></returns>
        public Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage GetQueueItem(string queueContainer)
        {
            InitializeStorage(queueContainer);

            string containerName = queueContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient queueStorage = queueStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Queue.CloudQueue cloudQueue = queueStorage.GetQueueReference(containerName);

            Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage cloudQueueMessage = cloudQueue.GetMessage(TimeSpan.FromMinutes(1));

            return cloudQueueMessage;
        } // GetQueueItem



        /// <summary>
        /// Puts a string message
        /// </summary>
        /// <param name="queueContainer"></param>
        /// <param name="message"></param>
        public void PutQueueItem(string queueContainer, string message)
        {
            InitializeStorage(queueContainer);

            string containerName = queueContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient queueStorage = queueStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Queue.CloudQueue cloudQueue = queueStorage.GetQueueReference(containerName);

            cloudQueue.AddMessage(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message));
        } // PutQueueItem



        /// <summary>
        /// Puts a byte[] message
        /// </summary>
        /// <param name="queueContainer"></param>
        /// <param name="message"></param>
        public void PutQueueItem(string queueContainer, MemoryStream message)
        {
            InitializeStorage(queueContainer);

            string containerName = queueContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient queueStorage = queueStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Queue.CloudQueue cloudQueue = queueStorage.GetQueueReference(containerName);

            cloudQueue.AddMessage(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message.ToArray()));
        } // PutQueueItem



        /// <summary>
        /// Creates the storage and gets a reference (once)
        /// </summary>
        private static void InitializeStorage(string queueContainer)
        {
            string containerName = queueContainer.ToString().ToLower(); // must be lower case!

            if (storageInitializedDictionary.ContainsKey(containerName) && storageInitializedDictionary[containerName] == true) return;

            lock (gate)
            {
                if (storageInitializedDictionary.ContainsKey(containerName) && storageInitializedDictionary[containerName] == true) return;

                try
                {
                    Microsoft.WindowsAzure.Storage.Auth.StorageCredentials storageCredentials = new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                        Sample.Azure.Common.Setting.SettingService.CloudStorageAccountName,
                        Sample.Azure.Common.Setting.SettingService.CloudStorageKey);

                    Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(storageCredentials,
                        new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageBlobEndPoint),
                        new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageQueueEndPoint),
                        new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageTableEndPoint),
                        new Uri(Sample.Azure.Common.Setting.SettingService.CloudStorageFileEndPoint));

                    Microsoft.WindowsAzure.Storage.Queue.CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();

                    queueStorage.DefaultRequestOptions.RetryPolicy = new Microsoft.WindowsAzure.Storage.RetryPolicies.LinearRetry(TimeSpan.FromSeconds(DEFAULT_SAVE_QUEUE_RETRY_WAIT_IN_MILLISECONDS), DEFAULT_SAVE_QUEUE_RETRY_ATTEMPTS);

                    int queueSaveTimeoutInMinutes = DEFAULT_SAVE_AND_READ_QUEUE_TIMEOUT_IN_MINUTES;
                    string timeOutOverRide = Sample.Azure.Common.Setting.SettingService.SaveAndReadQueueTimeoutInMinutes;
                    if (timeOutOverRide != null) queueSaveTimeoutInMinutes = int.Parse(timeOutOverRide);
                    queueStorage.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(queueSaveTimeoutInMinutes);

                    queueStorage.DefaultRequestOptions.ServerTimeout = new TimeSpan(0, DEFAULT_SAVE_AND_READ_QUEUE_TIMEOUT_IN_MINUTES, 0);

                    Microsoft.WindowsAzure.Storage.Queue.CloudQueue cloudQueue = queueStorage.GetQueueReference(containerName);
                    cloudQueue.CreateIfNotExists();

                    queueStorageDictionary.Add(containerName, queueStorage);
                    storageInitializedDictionary.Add(containerName, true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Storage services initialization failure. "
                        + "Check your storage account configuration settings. If running locally, "
                        + "ensure that the Development Storage service is running. \n"
                        + ex.Message);
                }

            } // lock
        } // InitializeStorage

    } // class
} // namespace
