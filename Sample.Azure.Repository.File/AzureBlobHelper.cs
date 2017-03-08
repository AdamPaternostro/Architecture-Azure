using System;
using System.Collections.Generic;
using System.IO;

namespace Sample.Azure.Repository.File
{
    internal class AzureBlobHelper
    {
        internal class BlobResultModel
        {
            public string Text { get; set; }

            public MemoryStream Stream { get; set; }

            public string eTag { get; set; }
        }

        private const int DEFAULT_SAVE_AND_READ_BLOB_TIMEOUT_IN_MINUTES = 5;
        private const int DEFAULT_SAVE_BLOB_RETRY_ATTEMPTS = 4;
        private const int DEFAULT_SAVE_BLOB_RETRY_WAIT_IN_MILLISECONDS = 100;

        private static Dictionary<string, bool> storageInitializedDictionary = new Dictionary<string, bool>();
        private static object gate = new Object();
        private static Dictionary<string, Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient> blobStorageDictionary = new Dictionary<string, Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient>();

        public AzureBlobHelper()
        {
        }


        /// <summary>
        /// Gets from blob storage
        /// NOTE: If you plan on getting the same blob over and over and quickly saving you will need to throttle and retry
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public BlobResultModel GetBlob(string blobContainer, string fileName)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            if (blob.Exists())
            {
                BlobResultModel blobResultModel = new BlobResultModel();

                blobResultModel.Stream = new MemoryStream();
                blob.DownloadToStream(blobResultModel.Stream);
                blobResultModel.eTag = blob.Properties.ETag;

                return blobResultModel;
            }
            else
            {
                return null;
            }
        } // GetBlob


        /// <summary>
        /// Returns the raw blob handle so you can obtain leases, read data, delete, etc...
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob GetBlobHandle(string blobContainer, string fileName)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            if (blob.Exists())
            {
                return blob;
            }
            else
            {
                return null;
            }
        } // GetBlobHandle




        /// <summary>
        /// Tests to see if a blob exists
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Exists(string blobContainer, string fileName)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            return blob.Exists();
        }


        /// <summary>
        /// Tests to see if a blob exists
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer GetBlobContainer(string blobContainer)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);

            return container;
        }


        /// <summary>
        /// Saves to blob storage
        /// NOTE: If you plan on getting the same blob over and over and quickly saving you will need to throttle and retry
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <param name="memoryStream"></param>
        public void SaveBlob(string blobContainer, string fileName, MemoryStream memoryStream)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            blobStorage.DefaultRequestOptions.ServerTimeout = new TimeSpan(0, 30, 0);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            // save
            if (fileName.Contains("."))
            {
                blob.Properties.ContentType = GetMimeTypeFromExtension(fileName.Substring(fileName.LastIndexOf(".")));
            }
            else
            {
                blob.Properties.ContentType = "application/octet-stream";
            }

            // save
            memoryStream.Position = 0; // rewind
            blob.UploadFromStream(memoryStream);
        } // SaveBlob



        /// <summary>
        /// Saves to blob storage
        /// NOTE: If you plan on getting the same blob over and over and quickly saving you will need to throttle and retry
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <param name="memoryStream"></param>
        public void SaveBlob(string blobContainer, string fileName, string text)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            // save
            if (fileName.Contains("."))
            {
                blob.Properties.ContentType = GetMimeTypeFromExtension(fileName.Substring(fileName.LastIndexOf(".")));
            }
            else
            {
                blob.Properties.ContentType = "application/octet-stream";
            }


            PutText(blob, text);
        } // SaveBlob


        /// <summary>
        /// Verifies the eTag when saving
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        /// <param name="eTag"></param>
        public void SaveBlob(string blobContainer, string fileName, string text, string eTag)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            // save
            if (fileName.Contains("."))
            {
                blob.Properties.ContentType = GetMimeTypeFromExtension(fileName.Substring(fileName.LastIndexOf(".")));
            }
            else
            {
                blob.Properties.ContentType = "application/octet-stream";
            }

            Microsoft.WindowsAzure.Storage.AccessCondition accessCondition = new Microsoft.WindowsAzure.Storage.AccessCondition();
            accessCondition.IfMatchETag = eTag;

            PutText(blob, text, accessCondition);
        } // SaveBlob


        /// <summary>
        /// Writes text to a blob reference
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="text"></param>
        public void PutText(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob, string text)
        {
            if (text == null) { text = string.Empty; }
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
            {
                blob.UploadFromStream(memoryStream);
            }
        }


        /// <summary>
        /// Writes text to a blob reference with an access condition
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="text"></param>
        /// <param name="accessCondition"></param>
        public void PutText(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob, string text, Microsoft.WindowsAzure.Storage.AccessCondition accessCondition)
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(text)))
            {
                try
                {
                    blob.UploadFromStream(memoryStream, accessCondition);
                }
                catch (Microsoft.WindowsAzure.Storage.StorageException storageException)
                {
                    if (storageException.Message.Contains("conditional header(s) is not met"))
                    {
                        throw new Sample.Azure.Common.CustomException.CustomConcurrencyException(System.Net.HttpStatusCode.PreconditionFailed, "The file has been updated by another user.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// Gets the text from a blob reference
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public BlobResultModel GetText(Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob)
        {
            if (blob.Exists())
            {
                string text = string.Empty;
                BlobResultModel blobResultModel = new BlobResultModel();
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    blob.DownloadToStream(memoryStream);
                    blobResultModel.Text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                    blobResultModel.eTag = blob.Properties.ETag;
                }
                return blobResultModel;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Gets from blob storage
        /// NOTE: If you plan on getting the same blob over and over and quickly saving you will need to throttle and retry
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public BlobResultModel GetBlobAsText(string blobContainer, string fileName)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            return GetText(blob);
        } // GetBlob


        /// <summary>
        ///  Saves to blob storage
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <param name="fileName"></param>
        public void DeleteBlob(string blobContainer, string fileName)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = container.GetBlockBlobReference(fileName);

            blob.DeleteIfExists();
        } // DeleteBlob


        /// <summary>
        /// Gets a list of blobs in the container
        /// </summary>
        /// <param name="blobContainer"></param>
        /// <returns></returns>
        public IEnumerable<Microsoft.WindowsAzure.Storage.Blob.IListBlobItem> ListBlobs(string blobContainer)
        {
            InitializeStorage(blobContainer);

            string containerName = blobContainer.ToString().ToLower(); // must be lower case!
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = blobStorageDictionary[containerName];
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobStorage.GetContainerReference(containerName);
            return container.ListBlobs();
        }


        public long GetBlobSize(string blobContainer, string fileName)
        {
            long output = 0;
            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blob = this.GetBlobHandle(blobContainer, fileName);
            blob.FetchAttributes();

            if (blob.Properties != null)
            {
                output = blob.Properties.Length;
            }
            return output;
        }


        private string GetFileNameFromUri(Uri uri)
        {
            string absolutePath = uri.AbsolutePath;
            absolutePath = absolutePath.TrimEnd('/');
            return absolutePath.Substring(absolutePath.LastIndexOf("/") + 1);
        }



        public string GetBlobUrl(string blobId, string container)
        {
            return string.Format("{0}/{1}/{2}", Sample.Azure.Common.Setting.SettingService.CloudStorageBlobEndPoint, container.ToString().ToLower(), blobId); ;
        }


        public void SetBlobContainerPermissions(string blobContainer, Microsoft.WindowsAzure.Storage.Blob.BlobContainerPermissions permissions)
        {
            var container = GetBlobContainer(blobContainer);
            container.SetPermissions(permissions);
        }


        public string GetMimeTypeFromExtension(string extension)
        {
            //clean up
            extension = extension.Trim().ToLower().Replace(".", "");

            string mimeType = string.Empty;

            if (extension == "jpg" || extension == "jpeg")
            {
                mimeType = "image/jpeg";
            }
            else if (extension == "png")
            {
                mimeType = "image/png";
            }
            else if (extension == "gif")
            {
                mimeType = "image/gif";
            }
            else if (extension == "bmp")
            {
                mimeType = "image/bmp";
            }
            else if (extension == "wav")
            {
                mimeType = "audio/wav";
            }
            else if (extension == "txt")
            {
                mimeType = "text/plain";
            }
            else if (extension == "html")
            {
                mimeType = "text/html";
            }
            else if (extension == "css")
            {
                mimeType = "text/css";
            }
            else if (extension == "doc" || extension == "docx")
            {
                mimeType = "application/msword";
            }
            else if (extension == "pdf")
            {
                mimeType = "application/pdf";
            }
            else
            {
                mimeType = "application/octet-stream";
            }

            return mimeType;
        }

        public string GetMimeTypeFromFilename(string filename)
        {
            string[] f = filename.Split('.');
            return this.GetMimeTypeFromExtension(f[f.GetUpperBound(0)]);
        }


        /// <summary>
        /// Creates the storage and gets a reference (once)
        /// </summary>
        private static void InitializeStorage(string blobContainer)
        {
            string containerName = blobContainer.ToString().ToLower(); // must be lower case!

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

                    Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobStorage = storageAccount.CreateCloudBlobClient();

                    int blobSaveTimeoutInMinutes = DEFAULT_SAVE_AND_READ_BLOB_TIMEOUT_IN_MINUTES;
                    string timeOutOverRide = Sample.Azure.Common.Setting.SettingService.SaveAndReadBlobTimeoutInMinutes;
                    if (timeOutOverRide != null) blobSaveTimeoutInMinutes = int.Parse(timeOutOverRide);
                    blobStorage.DefaultRequestOptions.ServerTimeout = TimeSpan.FromMinutes(blobSaveTimeoutInMinutes);

                    blobStorage.DefaultRequestOptions.RetryPolicy = new Microsoft.WindowsAzure.Storage.RetryPolicies.LinearRetry(TimeSpan.FromSeconds(1), 10);
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer cloudBlobContainer = blobStorage.GetContainerReference(containerName);
                    cloudBlobContainer.CreateIfNotExists();

                    Microsoft.WindowsAzure.Storage.Blob.BlobContainerPermissions permissions = new Microsoft.WindowsAzure.Storage.Blob.BlobContainerPermissions();
                    permissions.PublicAccess = Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Off;
                    cloudBlobContainer.SetPermissions(permissions);

                    blobStorageDictionary.Add(containerName, blobStorage);
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
