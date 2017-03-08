using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Service.ExceptionLog
{
    public class ExceptionLogService : Interface.Service.IExceptionLogService
    {
        private Interface.Repository.IExceptionLogRepository IExceptionLogRepository = null;

        public ExceptionLogService(Interface.Repository.IExceptionLogRepository IExceptionLogRepository)
        {
            this.IExceptionLogRepository = IExceptionLogRepository;
        }

        /// <summary>
        /// Saves based upon a mode
        /// </summary>
        public void Save(Model.ExceptionLog.ExceptionLogModel exceptionLogModel)
        {
            // Set some defaults if null
            if (exceptionLogModel.UserCreated == null)
            {
                exceptionLogModel.UserCreated = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            }

            if (exceptionLogModel.UserUpdated == null)
            {
                exceptionLogModel.UserUpdated = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            }

            DateTime now = DateTime.UtcNow;
            if (exceptionLogModel.DateCreated == System.DateTime.MinValue)
            {
                exceptionLogModel.DateCreated = now;
            }

            if (exceptionLogModel.DateUpdated == System.DateTime.MinValue)
            {
                exceptionLogModel.DateUpdated = now;
            }

            try
            {
                this.IExceptionLogRepository.Save(exceptionLogModel);
            }
            catch (Exception innerEx)
            {
                string breakPointMe = innerEx.ToString(); // since we are logging an exception we do not want to recursively call
                System.Diagnostics.EventLog.WriteEntry("Application", "Service.ExceptionLog: Could not write the exception: " + breakPointMe, System.Diagnostics.EventLogEntryType.Error);
            }
        }


        /// <summary>
        /// Saves an Exception
        /// </summary>
        public void Save(System.Exception exception)
        {
            this.Save(exception, Guid.NewGuid(), Guid.NewGuid());
        } // Save


        /// <summary>
        /// Saves an Exception
        /// </summary>
        public void Save(System.Exception exception, Guid exceptionLogId)
        {          
            this.Save(exception, exceptionLogId, Guid.NewGuid());
        } // Save


        /// <summary>
        /// Saves an Exception
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="HandlingInstanceId">Used to tie exceptions together</param>
        public void Save(System.Exception exception, Guid exceptionLogId, Guid handlingInstanceId)
        {
            Model.ExceptionLog.ExceptionLogModel exceptionLogModel = new Model.ExceptionLog.ExceptionLogModel();

            DateTime now = DateTime.UtcNow;

            exceptionLogModel.ExceptionLogId = exceptionLogId;
            exceptionLogModel.HandlingInstanceId = handlingInstanceId;

            // Set Audit Fields
            exceptionLogModel.DateCreated = now;
            exceptionLogModel.UserCreated = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            exceptionLogModel.AssemblyName = null;
            exceptionLogModel.AssemblyVersion = null;
            exceptionLogModel.AssemblyLocation = null;

            if (System.Reflection.Assembly.GetEntryAssembly() != null)
            {
                exceptionLogModel.AssemblyName = System.Reflection.Assembly.GetEntryAssembly().GetName().FullName;
                exceptionLogModel.AssemblyVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
                exceptionLogModel.AssemblyLocation = System.Reflection.Assembly.GetEntryAssembly().Location;
            }

            exceptionLogModel.MachineName = System.Environment.MachineName;
            exceptionLogModel.UserDomainName = System.Environment.UserDomainName;
            exceptionLogModel.UserName = System.Environment.UserName;

            exceptionLogModel.OSPlatform = System.Environment.OSVersion.Platform.ToString();
            exceptionLogModel.OSServicePack = System.Environment.OSVersion.ServicePack;
            exceptionLogModel.OSVersion = System.Environment.OSVersion.VersionString;

            exceptionLogModel.is64BitOperatingSystem = System.Environment.Is64BitOperatingSystem;
            exceptionLogModel.is64BitProcess = System.Environment.Is64BitProcess;
            exceptionLogModel.ProcessorCount = System.Environment.ProcessorCount;

            string exceptionInnerException = null;
            if (exception.InnerException != null) exceptionInnerException = exception.InnerException.ToString();
            exceptionLogModel.ExceptionMessage = exception.Message;
            exceptionLogModel.ExceptionSource = exception.Source;
            exceptionLogModel.ExceptionStackTrace = exception.StackTrace;

            this.Save(exceptionLogModel);

        } // Save


        /// <summary>
        /// Gets based upon an id
        /// </summary>
        /// <param name="exceptionLogId"></param>
        /// <returns></returns>
        public Model.ExceptionLog.ExceptionLogModel Get(Guid exceptionLogId)
        {
            return this.IExceptionLogRepository.Get(exceptionLogId);
        }

    } // class
} // namespace
