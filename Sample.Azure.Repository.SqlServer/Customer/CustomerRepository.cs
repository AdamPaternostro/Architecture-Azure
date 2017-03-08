using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.SqlServer.Customer
{
   public  class CustomerRepository : Interface.Repository.ICustomerRepository
    {
  
        public Sample.Azure.Model.Customer.CustomerModel Select(Guid customerId)
        {
            using (var context = new SampleBaseContent())
            {
                return context.CustomerModels.Where(o => o.CustomerId == customerId).FirstOrDefault();
            }
              
        }


        public void InsertOrReplace(Sample.Azure.Model.Customer.CustomerModel customerModel)
        {
            using (var context = new SampleBaseContent())
            {
                context.CustomerModels.Add(customerModel);
                context.SaveChanges();
            }
        }

    }
}
