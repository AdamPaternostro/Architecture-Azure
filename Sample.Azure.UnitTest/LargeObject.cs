using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample.Azure.UnitTest
{
    [TestClass]
    public class LargeObject
    {
        public LargeObject()
        {
            // Configure the system based upon where it is running
            // This will set all configuration values and register all types for dependency injection
            Sample.Azure.Config.Configuration.Configure();
        }

        private Model.LargeObject.LargeObjectModel CreateLargeObjectModel()
        {
            Model.LargeObject.LargeObjectModel largeObjectModel = new Model.LargeObject.LargeObjectModel();

            largeObjectModel.LargeObjectId = 0;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // 8 MB
            for (int i=1; i <= 8000000;  i++)
            {
                sb.Append("A");
            }
            largeObjectModel.Payload = sb.ToString();

            return largeObjectModel;
        }


        /// <summary>
        /// Use blob storage to store JSON
        /// </summary>
        [TestMethod]
        public void LargeObjectAzureBlob()
        {
            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ILargeObjectRepository,
                Sample.Azure.Repository.File.LargeObject.LargeObjectRepository>();

            Model.LargeObject.LargeObjectModel largeObjectModel = this.CreateLargeObjectModel();
            largeObjectModel.LargeObjectId = 1;

            Interface.Service.ILargeObjectService largeObjectService = DI.Container.Resolve<Interface.Service.ILargeObjectService>();
            largeObjectService.Save(largeObjectModel);

            Model.LargeObject.LargeObjectModel loadedModel = largeObjectService.Get(largeObjectModel.LargeObjectId);
            Assert.AreEqual(loadedModel.Payload, largeObjectModel.Payload);
        } // LargeObjectAzureBlob


        [TestMethod]
        public void LargeObjectRedisCache()
        {
            Model.LargeObject.LargeObjectModel largeObjectModel = this.CreateLargeObjectModel();
            largeObjectModel.LargeObjectId = 1;

            Interface.Repository.ICacheRepository cacheManager = DI.Container.Resolve<Interface.Repository.ICacheRepository>();
            string cacheKey = Guid.NewGuid().ToString();

            cacheManager.Set(cacheKey, largeObjectModel, TimeSpan.FromMinutes(1));

            Model.LargeObject.LargeObjectModel loadedModel = cacheManager.Get<Model.LargeObject.LargeObjectModel>(cacheKey);
            Assert.AreEqual(loadedModel.Payload, largeObjectModel.Payload);
        } // LargeObjectRedisCache


        /// <summary>
        /// Store an index or whole document in Azure Search
        /// </summary>
        [TestMethod]
        public void LargeObjectAzureSearch()
        {
            Model.LargeObject.LargeObjectModel largeObjectModel = this.CreateLargeObjectModel();
            largeObjectModel.LargeObjectId = 1;

            // This would save to blob or somewhere
            //Interface.Service.ILargeObjectService largeObjectService = DI.Container.Resolve<Interface.Service.ILargeObjectService>();
            //largeObjectService.Save(largeObjectModel);

            Model.Search.SearchLargeObjectModel searchLargeObjectModel = new Model.Search.SearchLargeObjectModel();
            searchLargeObjectModel.LargeObjectId = largeObjectModel.LargeObjectId.ToString();
            searchLargeObjectModel.Payload = largeObjectModel.Payload;

            Interface.Repository.IIndexerRepository azureSearch = DI.Container.Resolve<Interface.Repository.IIndexerRepository>();

            azureSearch.UpsertLargeObject(Interface.GlobalEnum.IndexerIndexName.LargeObject, searchLargeObjectModel);

            List<Model.Search.SearchLargeObjectModel> list = azureSearch.LargeObjectSearch(
                Interface.GlobalEnum.IndexerIndexName.LargeObject, largeObjectModel.LargeObjectId.ToString());

            Assert.AreEqual(list[0].LargeObjectId, largeObjectModel.LargeObjectId.ToString());

        } // LargeObjectAzureSearch



    }
}
