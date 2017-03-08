using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Sample.Azure.Repository.SqlServer
{
    public class SampleBaseContextConfiguration : DbConfiguration
    {
        public SampleBaseContextConfiguration()
        {
            // https://azure.microsoft.com/en-us/documentation/articles/best-practices-retry-service-specific/#sql-database-using-entity-framework-6-retry-guidelines
            // Set up the execution strategy for SQL Database (exponential) with 5 retries and 4 sec delay
            this.SetExecutionStrategy(
                 "System.Data.SqlClient", () => new System.Data.Entity.SqlServer.SqlAzureExecutionStrategy(5, TimeSpan.FromSeconds(4)));
        }
    }
}
