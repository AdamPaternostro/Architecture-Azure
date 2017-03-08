using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Sample.Azure.UnitTest
{
    [TestClass]
    public class Customer
    {
        public Customer()
        {
            // Configure the system based upon where it is running
            // This will set all configuration values and register all types for dependency injection
            Sample.Azure.Config.Configuration.Configure();
        }

        private Model.Customer.CustomerModel CreateCustomerModel()
        {
            Model.Customer.CustomerModel customerModel = new Model.Customer.CustomerModel();

            customerModel.CustomerName = string.Format("{0} - {1:mm-dd-yyyy}", customerModel.CustomerId, DateTime.Now);
            customerModel.CustomerRanking = (int)DateTime.Now.Millisecond;
            customerModel.CustomerCity = "Gotham";
            customerModel.CustomerState = "FL";
            customerModel.CustomerZip = "33000";
            customerModel.CustomerAddress = "123 Anywhere St";

            return customerModel;
        }

        [TestMethod]
        public void CustomerAzureTable()
        {
            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ICustomerRepository, 
                Sample.Azure.Repository.Table.Customer.CustomerRepository>();

            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();

            Interface.Service.ICustomerService customerService = DI.Container.Resolve<Interface.Service.ICustomerService>();
            customerService.Save(customerModel);

            Model.Customer.CustomerModel loadedModel = customerService.Get(customerModel.CustomerId);
            Assert.AreEqual(loadedModel.CustomerName, customerModel.CustomerName);
        } // CustomerAzureTable


        [TestMethod]
        public void CustomerDocumentDb()
        {
            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ICustomerRepository, 
                Sample.Azure.Repository.DocumentDB.Customer.CustomerRepository>();

            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();

            Interface.Service.ICustomerService customerService = DI.Container.Resolve<Interface.Service.ICustomerService>();
            customerService.Save(customerModel);

            Model.Customer.CustomerModel loadedModel = customerService.Get(customerModel.CustomerId);
            Assert.AreEqual(loadedModel.CustomerName, customerModel.CustomerName);
        } // CustomerDocumentDb



        [TestMethod]
        public void CustomerRedisCache()
        {
            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();

            Interface.Repository.ICacheRepository cacheManager = DI.Container.Resolve<Interface.Repository.ICacheRepository>();
            string cacheKey = Guid.NewGuid().ToString();

            cacheManager.Set(cacheKey, customerModel, TimeSpan.FromMinutes(1));

            Model.Customer.CustomerModel loadedModel = cacheManager.Get<Model.Customer.CustomerModel>(cacheKey);
            Assert.AreEqual(loadedModel.CustomerName, customerModel.CustomerName);
        } // CustomerRedisCache


        /// <summary>
        /// Store an index or whole document in Azure Search
        /// </summary>
        [TestMethod]
        public void CustomerAzureSearch()
        {
            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();

            Interface.Service.ICustomerService customerService = DI.Container.Resolve<Interface.Service.ICustomerService>();
            customerService.Save(customerModel);

            Model.Search.SearchCustomerModel searchCustomerModel = new Model.Search.SearchCustomerModel();
            searchCustomerModel.CustomerId = customerModel.CustomerId.ToString().ToLower();
            searchCustomerModel.CustomerName = customerModel.CustomerName;

            Interface.Repository.IIndexerRepository azureSearch = DI.Container.Resolve<Interface.Repository.IIndexerRepository>();

            azureSearch.UpsertCustomer(Interface.GlobalEnum.IndexerIndexName.Customer, searchCustomerModel);

            List<Model.Search.SearchCustomerModel> list = azureSearch.CustomerSearch(Interface.GlobalEnum.IndexerIndexName.Customer, customerModel.CustomerName);

            bool found = false;
            foreach (var item in list)
            {
                if (item.CustomerName == customerModel.CustomerName)
                {
                    Assert.AreEqual(list[0].CustomerName, customerModel.CustomerName);
                    found = true;
                    break;

                }
            }

            if (!found)
            {
                Assert.Fail("Could not find customer in Azure Search");
            }
            
        } // CustomerAzureSearch


        /// <summary>
        /// Use blob storage to store JSON
        /// </summary>
        [TestMethod]
        public void CustomerAzureBlob()
        {
            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ICustomerRepository, 
                Sample.Azure.Repository.File.Customer.CustomerRepository>();

            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();

            Interface.Service.ICustomerService customerService = DI.Container.Resolve<Interface.Service.ICustomerService>();
            customerService.Save(customerModel);

            Model.Customer.CustomerModel loadedModel = customerService.Get(customerModel.CustomerId);
            Assert.AreEqual(loadedModel.CustomerName, customerModel.CustomerName);
        } // CustomerAzureTable


        [TestMethod]
        public void CustomerEntityFramework()
        {
            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ICustomerRepository, Sample.Azure.Repository.SqlServer.Customer.CustomerRepository>();

            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();

            Interface.Service.ICustomerService customerService = DI.Container.Resolve<Interface.Service.ICustomerService>();
            customerService.Save(customerModel);

            Model.Customer.CustomerModel loadedModel = customerService.Get(customerModel.CustomerId);
            Assert.AreEqual(loadedModel.CustomerName, customerModel.CustomerName);
        } // CustomerEntityFramework


        [TestMethod]
        public void CustomerQueue()
        {
            Model.Customer.CustomerModel customerModel = this.CreateCustomerModel();
            Common.JSON.JSONService jsonService = new Common.JSON.JSONService();
            string json = jsonService.Serialize<Model.Customer.CustomerModel>(customerModel);

            Interface.Repository.IQueueRepository queueRepository = DI.Container.Resolve<Interface.Repository.IQueueRepository>();
            queueRepository.Enqueue("customer", json); // Typically you would just put an id in the queue since they have size limits on their payload

            Model.Queue.QueueModel queueModel = new Model.Queue.QueueModel();
            queueModel = queueRepository.Dequeue("customer", TimeSpan.FromMinutes(1));
            Model.Customer.CustomerModel loadedModel = jsonService.Deserialize<Model.Customer.CustomerModel>(queueModel.Item);
            Assert.AreEqual(loadedModel.CustomerName, customerModel.CustomerName);
            queueRepository.DeleteItem("customer", queueModel);

        } // CustomerEntityFramework



    } // class
} // namespace
