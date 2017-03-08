using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.Table.ExceptionLog
{
    internal class ExceptionLogServiceDataServiceContext : BaseDataServiceContext
    {
        public override string TableName { get { return "ExceptionLog"; } }


        /// <summary>
        /// Connects to Azure and creates the table if it does not exist
        /// </summary>
        public ExceptionLogServiceDataServiceContext()
        {

        } // CustomerDataServiceContext


        /// <summary>
        /// Loads the customer based upon their customer key
        /// </summary>
        internal ExceptionLog Select(Guid exceptionLogId)
        {
            string filter1 = Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition("PartitionKey",
                Microsoft.WindowsAzure.Storage.Table.QueryComparisons.Equal, exceptionLogId.ToString().ToLower());

            string filter2 = Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition("RowKey",
                Microsoft.WindowsAzure.Storage.Table.QueryComparisons.Equal, exceptionLogId.ToString().ToLower());

            string filter = Microsoft.WindowsAzure.Storage.Table.TableQuery.CombineFilters(filter1,
                Microsoft.WindowsAzure.Storage.Table.TableOperators.And, filter2);

            Microsoft.WindowsAzure.Storage.Table.TableQuery<ExceptionLog> tableQuery =
                new Microsoft.WindowsAzure.Storage.Table.TableQuery<ExceptionLog>().Where(filter);

            foreach (ExceptionLog item in cloudTable.ExecuteQuery(tableQuery))
            {
                return item;
            }

            return null;

        }


        /// <summary>
        /// Adds and saves and item to the table
        /// </summary>
        internal void InsertOrReplace(ExceptionLog exceptionLog)
        {
            // Set some default
            if (exceptionLog.DateCreated == DateTime.MinValue) { exceptionLog.DateCreated = DateTime.UtcNow; }
            if (exceptionLog.DateUpdated == DateTime.MinValue) { exceptionLog.DateUpdated = DateTime.UtcNow; }

            Microsoft.WindowsAzure.Storage.Table.TableOperation insertOrReplace =
                Microsoft.WindowsAzure.Storage.Table.TableOperation.InsertOrReplace(exceptionLog);

            // Execute the insert operation.
            cloudTable.Execute(insertOrReplace);
        } // InsertOrReplace



    } // class
} // namespace