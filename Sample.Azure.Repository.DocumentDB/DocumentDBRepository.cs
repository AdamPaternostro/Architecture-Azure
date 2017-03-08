using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;


namespace Sample.Azure.Repository.DocumentDB
{
    /// <summary>
    /// https://github.com/Azure/azure-documentdb-dotnet/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DocumentDBRepository<T>
    {
        public string DatabaseId { get { return Sample.Azure.Common.Setting.SettingService.DocumentDBDatabase; } }

        public string CollectionId { get { return Sample.Azure.Common.Setting.SettingService.DocumentDBCollection; } }

        private FeedOptions DefaultOptions { get { return new FeedOptions { EnableCrossPartitionQuery = true }; } }

        public Uri CollectionUri { get { return UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId); } }


        /// <summary>
        /// Create and "cache" the DocumentClient.
        /// This will also create the database/collection if it does not exist
        /// </summary>
        private static DocumentClient client;
        internal DocumentClient Client
        {
            get
            {
                if (client == null)
                {
                    string endpoint = Sample.Azure.Common.Setting.SettingService.DocumentDBEndPoint;
                    string authKey = Sample.Azure.Common.Setting.SettingService.DocumentDBAuthKey;
                    Uri endpointUri = new Uri(endpoint);
                    client = new DocumentClient(endpointUri, authKey);

                    client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseId }).Wait();

                    DocumentCollection collectionDefinition = new DocumentCollection();
                    collectionDefinition.Id = CollectionId;

                    client.CreateDocumentCollectionIfNotExistsAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        collectionDefinition,
                        new RequestOptions { OfferThroughput = 400 }).Wait();
                }

                return client;
            }
        }

        // Query the database
        public IEnumerable<T> GetItems(Expression<Func<T, bool>> predicate)
        {
            return Client.CreateDocumentQuery<T>(CollectionUri, DefaultOptions)
                .Where(predicate)
                .AsEnumerable();
        }

    } // class
} // namespace