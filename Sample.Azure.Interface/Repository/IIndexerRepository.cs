using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Repository
{
    public interface IIndexerRepository
    {
        void UpsertCustomer(GlobalEnum.IndexerIndexName indexName, Model.Search.SearchCustomerModel searchCustomerModel);

        void DeleteCustomer(GlobalEnum.IndexerIndexName indexName, Model.Search.SearchCustomerModel searchCustomerModel);

        List<Model.Search.SearchCustomerModel> CustomerSearch(GlobalEnum.IndexerIndexName indexName, string customerName);


        void UpsertLargeObject(GlobalEnum.IndexerIndexName indexName, Model.Search.SearchLargeObjectModel searchLargeObjectModel);

        void DeleteLargeObject(GlobalEnum.IndexerIndexName indexName, Model.Search.SearchLargeObjectModel searchLargeObjectModel);

        List<Model.Search.SearchLargeObjectModel> LargeObjectSearch(GlobalEnum.IndexerIndexName indexName, String largeObjectId);


        void DeleteIndexIfExists(string indexName);

        bool DoesIndexExist(string indexName);
    }
}
