using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Model.ExceptionLog
{
    public class ExceptionLogModel
    {
        public ExceptionLogModel()
        {
            this.ExceptionLogId = Guid.NewGuid();
        }

        public System.Guid ExceptionLogId { get; set; }
        public System.Guid HandlingInstanceId { get; set; }
        public string AssemblyName { get; set; }
        public string AssemblyVersion { get; set; }
        public string AssemblyLocation { get; set; }
        public string MachineName { get; set; }
        public string UserDomainName { get; set; }
        public string UserName { get; set; }
        public string OSPlatform { get; set; }
        public string OSServicePack { get; set; }
        public string OSVersion { get; set; }
        public Nullable<bool> is64BitOperatingSystem { get; set; }
        public Nullable<bool> is64BitProcess { get; set; }
        public Nullable<int> ProcessorCount { get; set; }
        public string ExceptionInnerException { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionSource { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string UserCreated { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string UserUpdated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public bool IsDeleted { get; set; }
        public string UserDeleted { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public byte[] RowVersion { get; set; }
    } // class
} // namespace
