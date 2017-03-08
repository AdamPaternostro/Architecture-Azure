using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.Table.Customer
{
    internal class CustomerDataServiceContext : BaseDataServiceContext
    {

        public override string TableName { get { return "Customer"; } }


        /// <summary>
        /// Connects to Azure and creates the table if it does not exist
        /// </summary>
        public CustomerDataServiceContext()
        {

        } // CustomerDataServiceContext


        /// <summary>
        /// Loads the customer based upon their customer key
        /// </summary>
        internal Customer Select(Guid customerId)
        {
            string filter1 = Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition("PartitionKey",
                Microsoft.WindowsAzure.Storage.Table.QueryComparisons.Equal, customerId.ToString().ToLower());

            string filter2 = Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition("RowKey",
                Microsoft.WindowsAzure.Storage.Table.QueryComparisons.Equal, customerId.ToString().ToLower());

            string filter = Microsoft.WindowsAzure.Storage.Table.TableQuery.CombineFilters(filter1,
                Microsoft.WindowsAzure.Storage.Table.TableOperators.And, filter2);

            Microsoft.WindowsAzure.Storage.Table.TableQuery<Customer> tableQuery =
                new Microsoft.WindowsAzure.Storage.Table.TableQuery<Customer>().Where(filter);

            foreach (Customer item in cloudTable.ExecuteQuery(tableQuery))
            {
                return item;
            }

            return null;

        }


        /// <summary>
        /// Adds and saves and item to the table
        /// </summary>
        internal void InsertOrReplace(Customer customer)
        {
            // Set some default
            if (customer.DateCreated == DateTime.MinValue) { customer.DateCreated = DateTime.UtcNow; }
            if (customer.DateUpdated == DateTime.MinValue) { customer.DateUpdated = DateTime.UtcNow; }

            Microsoft.WindowsAzure.Storage.Table.TableOperation insertOrReplace =
                Microsoft.WindowsAzure.Storage.Table.TableOperation.InsertOrReplace(customer);

            // Execute the insert operation.
            cloudTable.Execute(insertOrReplace);
        } // InsertOrReplace



    } // class
} // namespace
