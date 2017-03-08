using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Azure.Config
{
    public class RegisterTypes
    {

        public static void Configure(Environment.SystemEnvironment systemEnvironment)
        {
            // Use Azure blob storage
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.IFileRepository, 
                Sample.Azure.Repository.File.FileRepository>();

            // Use Azure redis cache
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ICacheRepository,
                Sample.Azure.Repository.Cache.RedisCacheManager>();


            // Use Azure queues
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.IQueueRepository,
                Sample.Azure.Repository.Queue.QueueRepository>();

            // Use Azure Search
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.IIndexerRepository,
                Sample.Azure.Repository.Search.IndexerRepository>();

            // Exception Handling
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Service.IExceptionLogService,
                Sample.Azure.Service.ExceptionLog.ExceptionLogService>();

            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.IExceptionLogRepository,
                Sample.Azure.Repository.Table.ExceptionLog.ExceptionLogRepository>();

            ////// Customer //////
            // Use Azure table for customers
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ICustomerRepository,
                Sample.Azure.Repository.Table.Customer.CustomerRepository>();

            // Customer Service
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Service.ICustomerService,
                Sample.Azure.Service.Customer.CustomerService>();

            ////// LargeObject //////
            // Use Azure blob for LargeObject
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ILargeObjectRepository,
                Sample.Azure.Repository.File.LargeObject.LargeObjectRepository>();

            // LargeObject Service
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Service.ILargeObjectService,
                Sample.Azure.Service.LargeObject.LargeObjectService>();


        }

    } // class
} // namespace
