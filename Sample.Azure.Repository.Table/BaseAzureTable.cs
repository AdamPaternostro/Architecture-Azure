using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Sample.Azure.Repository.Table
{
    internal class BaseAzureTable : TableEntity
    {
        public BaseAzureTable() { }

        public string UserCreated { get; set; }

        public System.DateTime DateCreated { get; set; }

        public string UserUpdated { get; set; }

        public System.DateTime DateUpdated { get; set; }

        public bool IsDeleted { get; set; }

        public string UserDeleted { get; set; }

        public Nullable<System.DateTime> DateDeleted { get; set; }

        public string RowVersion { get; set; }
    }
}
