using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.Table.Customer
{
    public class CustomerRepository : Sample.Azure.Interface.Repository.ICustomerRepository
    {

        public CustomerRepository()
        {
        }

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerKey"></param>
        /// <returns></returns>
        public Sample.Azure.Model.Customer.CustomerModel Select(Guid customerId)
        {
            CustomerDataServiceContext context = new CustomerDataServiceContext();
            Customer customer = context.Select(customerId);

            if (customer == null)
            {
                return null;
            }
            else
            {
                // Copy from Azure model to POCO model
                Sample.Azure.Model.Customer.CustomerModel customerModel = new Model.Customer.CustomerModel();

                customerModel.CustomerId = customerId;
                customerModel.CustomerName = customer.CustomerName;
                customerModel.CustomerAddress = customer.CustomerAddress;
                customerModel.CustomerCity = customer.CustomerCity;
                customerModel.CustomerState = customer.CustomerState;
                customerModel.CustomerZip = customer.CustomerZip;
                customerModel.CustomerRanking = customer.CustomerRanking;


                customerModel.DateCreated = customer.DateCreated;
                customerModel.DateDeleted = customer.DateDeleted;
                customerModel.DateUpdated = customer.DateUpdated;
                customerModel.IsDeleted = customer.IsDeleted;
                customerModel.UserCreated = customer.UserCreated;
                customerModel.UserDeleted = customer.UserDeleted;
                customerModel.UserUpdated = customer.UserUpdated;
                customerModel.RowVersion = customer.RowVersion;
                return customerModel;

            }
        } // Select


        /// <summary>
        /// Upserts a customer
        /// </summary>
        /// <param name="customerModel"></param>
        public void InsertOrReplace(Sample.Azure.Model.Customer.CustomerModel customerModel)
        {
            Customer customer = new Customer();

            // Set the Azure partitioning
            customer.PartitionKey = customerModel.CustomerId.ToString().ToLower();
            customer.RowKey = customerModel.CustomerId.ToString().ToLower();

            customer.CustomerName = customerModel.CustomerName;
            customer.CustomerAddress = customerModel.CustomerAddress;
            customer.CustomerCity = customerModel.CustomerCity;
            customer.CustomerState = customerModel.CustomerState;
            customer.CustomerZip = customerModel.CustomerZip;
            customer.CustomerRanking = customerModel.CustomerRanking;

            customer.DateCreated = customerModel.DateCreated;
            customer.DateDeleted = customerModel.DateDeleted;
            customer.DateUpdated = customerModel.DateUpdated;
            customer.IsDeleted = customerModel.IsDeleted;
            customer.UserCreated = customerModel.UserCreated;
            customer.UserDeleted = customerModel.UserDeleted;
            customer.UserUpdated = customerModel.UserUpdated;

            CustomerDataServiceContext context = new CustomerDataServiceContext();
            context.InsertOrReplace(customer);
        } // InsertOrReplace


    } // class
} // namespace
