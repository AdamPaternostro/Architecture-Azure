using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Sample.Azure.Repository.Table.ExceptionLog
{
    internal class ExceptionLog : BaseAzureTable
    {
        public ExceptionLog()
            : base()
        {
            PartitionKey = this.GeneratedParitionkey; // group by 15 minute intervals for easy querying
            RowKey = Guid.NewGuid().ToString(); // no need for row key that is special
        }

        private string GeneratedParitionkey
        {
            get
            {
                DateTime utcDate = DateTime.UtcNow;
                string minute = string.Empty;
                if (utcDate.Minute < 15)
                {
                    minute = "00";
                }
                else if (utcDate.Minute < 30)
                {
                    minute = "15";
                }
                else if (utcDate.Minute < 45)
                {
                    minute = "30";
                }
                else
                {
                    minute = "45";
                }
                return string.Format("{0:MM-dd-yyyy HH}:{1}", utcDate, minute);
            }
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

    } // class
} // namespace
