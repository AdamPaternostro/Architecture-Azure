using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Net.Http;

using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Sample.Azure.Repository.Search
{
    public class IndexerRepository : Interface.Repository.IIndexerRepository
    {
        private static List<string> doesIndexExistsCheck = new List<string>();

        private Interface.Repository.IQueueRepository queueRepository = null;
        private Interface.Repository.IFileRepository fileRepository = null;
        private Interface.Service.IExceptionLogService exceptionLogService = null;
        private SearchServiceClient serviceClient = null;


        public IndexerRepository(Interface.Repository.IQueueRepository queueRepository,
            Interface.Repository.IFileRepository fileRepository,
            Interface.Service.IExceptionLogService exceptionLogService)
        {
            this.queueRepository = queueRepository;
            this.fileRepository = fileRepository;
            this.exceptionLogService = exceptionLogService;

            serviceClient = new SearchServiceClient(Common.Setting.SettingService.AzureSearchName,
                new SearchCredentials(Common.Setting.SettingService.AzureSearchApiKey));
        }



        /// <summary>
        /// Inserts/Updates a customer
        /// </summary>
        public void UpsertCustomer(Interface.GlobalEnum.IndexerIndexName indexName, Model.Search.SearchCustomerModel searchCustomerModel)
        {
            // only check once per run
            if (!doesIndexExistsCheck.Contains(indexName.ToString().ToLower()))
            {
                CreateIndexIfNotExists(indexName, Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined);
                doesIndexExistsCheck.Add(indexName.ToString().ToLower());
            }

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName.ToString().ToLower());

            // Can be done in batches, but since we are using batching we can do one by one for retries
            List<Model.Search.SearchCustomerModel> itemsToIndex = new List<Model.Search.SearchCustomerModel>();
            itemsToIndex.Add(searchCustomerModel);

            indexClient.Documents.Index(IndexBatch.Create(itemsToIndex.Select(doc => IndexAction.Create(IndexActionType.MergeOrUpload, doc))));
        } // UpsertCustomer


        /// <summary>
        /// Removes a customer
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="searchDocument"></param>
        public void DeleteCustomer(Interface.GlobalEnum.IndexerIndexName indexName, Model.Search.SearchCustomerModel searchCustomerModel)
        {
            // only check once per run
            if (!doesIndexExistsCheck.Contains(indexName.ToString().ToLower()))
            {
                CreateIndexIfNotExists(indexName, Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined);
                doesIndexExistsCheck.Add(indexName.ToString().ToLower());
            }

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName.ToString().ToLower());

            // Can be done in batches, but since we are using batching we can do one by one for retries
            List<Model.Search.SearchCustomerModel> itemsToIndex = new List<Model.Search.SearchCustomerModel>();
            itemsToIndex.Add(searchCustomerModel);

            indexClient.Documents.Index(IndexBatch.Create(itemsToIndex.Select(doc => IndexAction.Create(IndexActionType.Delete, doc))));

            // Sometimes when your Search service is under load, indexing will fail for some of the documents in
            // the batch. Depending on your application, you can take compensating actions like delaying and
            // retrying. 
        } // DeleteCustomer


        /// <summary>
        /// Search for a csutomer
        /// </summary>
        public List<Model.Search.SearchCustomerModel> CustomerSearch(Interface.GlobalEnum.IndexerIndexName indexName, string customerName)
        {
            // only check once per run
            if (!doesIndexExistsCheck.Contains(indexName.ToString().ToLower()))
            {
                CreateIndexIfNotExists(indexName, Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined);
                doesIndexExistsCheck.Add(indexName.ToString().ToLower());
            }

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName.ToString().ToLower());

            SearchParameters searchParameters = new SearchParameters();

            List<Model.Search.SearchCustomerModel> resultList = new List<Model.Search.SearchCustomerModel>();

            DocumentSearchResponse<Model.Search.SearchCustomerModel> response =
                indexClient.Documents.Search<Model.Search.SearchCustomerModel>(customerName, searchParameters);

            foreach (SearchResult<Model.Search.SearchCustomerModel> item in response)
            {
                Model.Search.SearchCustomerModel searchDocument = new Model.Search.SearchCustomerModel();

                searchDocument.CustomerId = item.Document.CustomerId;
                searchDocument.CustomerName = item.Document.CustomerName;

                resultList.Add(searchDocument);
            }

            return resultList;
        }



        /// <summary>
        /// Inserts/Updates a largeObject
        /// </summary>
        public void UpsertLargeObject(Interface.GlobalEnum.IndexerIndexName indexName, Model.Search.SearchLargeObjectModel searchLargeObjectModel)
        {
            // only check once per run
            if (!doesIndexExistsCheck.Contains(indexName.ToString().ToLower()))
            {
                CreateIndexIfNotExists(indexName, Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined);
                doesIndexExistsCheck.Add(indexName.ToString().ToLower());
            }

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName.ToString().ToLower());

            // Can be done in batches, but since we are using batching we can do one by one for retries
            List<Model.Search.SearchLargeObjectModel> itemsToIndex = new List<Model.Search.SearchLargeObjectModel>();
            itemsToIndex.Add(searchLargeObjectModel);

            indexClient.Documents.Index(IndexBatch.Create(itemsToIndex.Select(doc => IndexAction.Create(IndexActionType.MergeOrUpload, doc))));
        } // UpsertLargeObject


        /// <summary>
        /// Removes a largeObject
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="searchDocument"></param>
        public void DeleteLargeObject(Interface.GlobalEnum.IndexerIndexName indexName, Model.Search.SearchLargeObjectModel searchLargeObjectModel)
        {
            // only check once per run
            if (!doesIndexExistsCheck.Contains(indexName.ToString().ToLower()))
            {
                CreateIndexIfNotExists(indexName, Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined);
                doesIndexExistsCheck.Add(indexName.ToString().ToLower());
            }

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName.ToString().ToLower());

            // Can be done in batches, but since we are using batching we can do one by one for retries
            List<Model.Search.SearchLargeObjectModel> itemsToIndex = new List<Model.Search.SearchLargeObjectModel>();
            itemsToIndex.Add(searchLargeObjectModel);

            indexClient.Documents.Index(IndexBatch.Create(itemsToIndex.Select(doc => IndexAction.Create(IndexActionType.Delete, doc))));

            // Sometimes when your Search service is under load, indexing will fail for some of the documents in
            // the batch. Depending on your application, you can take compensating actions like delaying and
            // retrying. 
        } // DeleteLargeObject


        /// <summary>
        /// Search for a csutomer
        /// </summary>
        public List<Model.Search.SearchLargeObjectModel> LargeObjectSearch(Interface.GlobalEnum.IndexerIndexName indexName, string largeObjectId)
        {
            // only check once per run
            if (!doesIndexExistsCheck.Contains(indexName.ToString().ToLower()))
            {
                CreateIndexIfNotExists(indexName, Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined);
                doesIndexExistsCheck.Add(indexName.ToString().ToLower());
            }

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName.ToString().ToLower());

            SearchParameters searchParameters = new SearchParameters();

            List<Model.Search.SearchLargeObjectModel> resultList = new List<Model.Search.SearchLargeObjectModel>();

            DocumentSearchResponse<Model.Search.SearchLargeObjectModel> response =
                indexClient.Documents.Search<Model.Search.SearchLargeObjectModel>(largeObjectId, searchParameters);

            foreach (SearchResult<Model.Search.SearchLargeObjectModel> item in response)
            {
                Model.Search.SearchLargeObjectModel searchDocument = new Model.Search.SearchLargeObjectModel();

                searchDocument.LargeObjectId = item.Document.LargeObjectId;
                searchDocument.Payload = item.Document.Payload;

                resultList.Add(searchDocument);
            }

            return resultList;
        }


        /// <summary>
        /// Creates the index if it does not exists
        /// </summary>
        /// <param name="indexName"></param>
        public void CreateIndexIfNotExists(Interface.GlobalEnum.IndexerIndexName indexName, Interface.GlobalEnum.IndexerRepositoryIndexType indexTypeEnum)
        {
            if (DoesIndexExist(indexName.ToString().ToLower()) == false)
            {
                List<string> allowedOrigins = Common.Setting.SettingService.AzureSearchCORS;

                Index definition = null;

                if (indexTypeEnum == Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined && indexName == 
                    Interface.GlobalEnum.IndexerIndexName.Customer)
                {
                    definition = new Index()
                    {
                        Name = indexName.ToString().ToLower(),
                        Fields = new[]
                        {
                        new Field{ Name = "CustomerId",    Type = "Edm.String", IsKey = true,  IsSearchable = true },
                        new Field{ Name = "CustomerName", Type = "Edm.String", IsKey = false, IsSearchable = true }
                        },
                        Suggesters = new[]
                        {
                            new Suggester {
                                Name = "autocomplete",
                                SearchMode = SuggesterSearchMode.AnalyzingInfixMatching,
                                SourceFields = new []
                                {
                                    "CustomerName"
                                }
                            }
                        },
                        CorsOptions = new CorsOptions()
                        {
                            AllowedOrigins = allowedOrigins,
                            MaxAgeInSeconds = 300
                        }
                    };
                }
                else if (indexTypeEnum == Interface.GlobalEnum.IndexerRepositoryIndexType.SystemDefined && indexName == 
                    Interface.GlobalEnum.IndexerIndexName.LargeObject)
                {
                    definition = new Index()
                    {
                        Name = indexName.ToString().ToLower(),
                        Fields = new[]
                        {
                        new Field{ Name = "LargeObjectId",  Type = "Edm.String",    IsKey = true,  IsSearchable = true },
                        new Field{ Name = "Payload",        Type = "Edm.String", IsKey = false, IsSearchable = true }
                        },
                        CorsOptions = new CorsOptions()
                        {
                            AllowedOrigins = allowedOrigins,
                            MaxAgeInSeconds = 300
                        }
                    };
                }

                else
                {
                    throw new Exception("Azure Search CreateIndexIfNotExists does not have a indexName = " + indexName.ToString().ToLower());
                }

                serviceClient.Indexes.Create(definition);
            }
        }


        /// <summary>
        /// Deletes an index if it exists
        /// </summary>
        /// <param name="indexName"></param>
        public void DeleteIndexIfExists(string indexName)
        {
            indexName = indexName.ToLower();

            if (DoesIndexExist(indexName))
            {
                serviceClient.Indexes.Delete(indexName.ToLower());
            }
        }


        /// <summary>
        /// Checks to see if an index exists
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public bool DoesIndexExist(string indexName)
        {
            indexName = indexName.ToLower();
            return serviceClient.Indexes.Exists(indexName);
        }

    } // class
} // namespace
