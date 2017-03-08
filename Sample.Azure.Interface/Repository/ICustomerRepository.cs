using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Repository
{
    public interface ICustomerRepository
    {
        Sample.Azure.Model.Customer.CustomerModel Select(Guid customerId);

        void InsertOrReplace(Sample.Azure.Model.Customer.CustomerModel customerModel);
    }
}
