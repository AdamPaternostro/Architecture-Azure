using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.Table.ExceptionLog
{
    public class ExceptionLogRepository : Interface.Repository.IExceptionLogRepository
    {
        public ExceptionLogRepository() 
        {
        }


        /// <summary>
        /// Saves the data to an Azure Table named ExeptionLog
        /// </summary>
        /// <param name="exceptionLogModel"></param>
        public void Save(Model.ExceptionLog.ExceptionLogModel exceptionLogModel)
        {
            ExceptionLogServiceDataServiceContext context = new ExceptionLogServiceDataServiceContext();

            // Copy fields (did not want to include an auto mapper refernce so this is done by hand
            ExceptionLog exceptionLog = new ExceptionLog();
            exceptionLog.ExceptionLogId = exceptionLogModel.ExceptionLogId;
            exceptionLog.HandlingInstanceId = exceptionLogModel.HandlingInstanceId;
            exceptionLog.AssemblyName = exceptionLogModel.AssemblyName;
            exceptionLog.AssemblyVersion = exceptionLogModel.AssemblyVersion;
            exceptionLog.AssemblyLocation = exceptionLogModel.AssemblyLocation;
            exceptionLog.MachineName = exceptionLogModel.MachineName;
            exceptionLog.UserDomainName = exceptionLogModel.UserDomainName;
            exceptionLog.UserName = exceptionLogModel.UserName;
            exceptionLog.OSPlatform = exceptionLogModel.OSPlatform;
            exceptionLog.OSServicePack = exceptionLogModel.OSServicePack;
            exceptionLog.OSVersion = exceptionLogModel.OSVersion;
            exceptionLog.is64BitOperatingSystem = exceptionLogModel.is64BitOperatingSystem;
            exceptionLog.is64BitProcess = exceptionLogModel.is64BitProcess;
            exceptionLog.ProcessorCount = exceptionLogModel.ProcessorCount;
            exceptionLog.ExceptionInnerException = exceptionLogModel.ExceptionInnerException;
            exceptionLog.ExceptionMessage = exceptionLogModel.ExceptionMessage;
            exceptionLog.ExceptionSource = exceptionLogModel.ExceptionSource;
            exceptionLog.ExceptionStackTrace = exceptionLogModel.ExceptionStackTrace;
            exceptionLog.UserCreated = exceptionLogModel.UserCreated;
            exceptionLog.DateCreated = exceptionLogModel.DateCreated;
            exceptionLog.UserUpdated = exceptionLogModel.UserUpdated;
            exceptionLog.DateUpdated = exceptionLogModel.DateUpdated;
            exceptionLog.IsDeleted = exceptionLogModel.IsDeleted;
            exceptionLog.UserDeleted = exceptionLogModel.UserDeleted;
            exceptionLog.DateDeleted = exceptionLogModel.DateDeleted;

            // set some defaults if not provided
            if (exceptionLog.DateCreated == DateTime.MinValue) { exceptionLog.DateCreated = DateTime.UtcNow; }
            if (exceptionLog.DateUpdated == DateTime.MinValue) { exceptionLog.DateUpdated = DateTime.UtcNow; }

            // Make the RowKey = Exception Log Id so we can query faster
            exceptionLog.PartitionKey = exceptionLog.ExceptionLogId.ToString().ToLower();
            exceptionLog.RowKey = exceptionLog.ExceptionLogId.ToString().ToLower();

            context.InsertOrReplace(exceptionLog);
        } // Save


        /// <summary>
        /// Gets a single item 
        /// NOTE: This is every expensive since we are not searching by a parition or row key!!!
        /// </summary>
        /// <param name="exceptionLogId"></param>
        /// <returns></returns>
        public Model.ExceptionLog.ExceptionLogModel Get(Guid exceptionLogId)
        {
            ExceptionLogServiceDataServiceContext context = new ExceptionLogServiceDataServiceContext();
            ExceptionLog exceptionLog = context.Select(exceptionLogId);
            if (exceptionLog == null)
            {
                return null;
            }
            else
            {
                Model.ExceptionLog.ExceptionLogModel exceptionLogModel = new Model.ExceptionLog.ExceptionLogModel();
                exceptionLogModel.ExceptionLogId = exceptionLog.ExceptionLogId;
                exceptionLogModel.HandlingInstanceId = exceptionLog.HandlingInstanceId;
                exceptionLogModel.AssemblyName = exceptionLog.AssemblyName;
                exceptionLogModel.AssemblyVersion = exceptionLog.AssemblyVersion;
                exceptionLogModel.AssemblyLocation = exceptionLog.AssemblyLocation;
                exceptionLogModel.MachineName = exceptionLog.MachineName;
                exceptionLogModel.UserDomainName = exceptionLog.UserDomainName;
                exceptionLogModel.UserName = exceptionLog.UserName;
                exceptionLogModel.OSPlatform = exceptionLog.OSPlatform;
                exceptionLogModel.OSServicePack = exceptionLog.OSServicePack;
                exceptionLogModel.OSVersion = exceptionLog.OSVersion;
                exceptionLogModel.is64BitOperatingSystem = exceptionLog.is64BitOperatingSystem;
                exceptionLogModel.is64BitProcess = exceptionLog.is64BitProcess;
                exceptionLogModel.ProcessorCount = exceptionLog.ProcessorCount;
                exceptionLogModel.ExceptionInnerException = exceptionLog.ExceptionInnerException;
                exceptionLogModel.ExceptionMessage = exceptionLog.ExceptionMessage;
                exceptionLogModel.ExceptionSource = exceptionLog.ExceptionSource;
                exceptionLogModel.ExceptionStackTrace = exceptionLog.ExceptionStackTrace;
                exceptionLogModel.UserCreated = exceptionLog.UserCreated;
                exceptionLogModel.DateCreated = exceptionLog.DateCreated;
                exceptionLogModel.UserUpdated = exceptionLog.UserUpdated;
                exceptionLogModel.DateUpdated = exceptionLog.DateUpdated;
                exceptionLogModel.IsDeleted = exceptionLog.IsDeleted;
                exceptionLogModel.UserDeleted = exceptionLog.UserDeleted;
                exceptionLogModel.DateDeleted = exceptionLog.DateDeleted;
                return exceptionLogModel;
            }
        }        

    } // class
} // namespace
