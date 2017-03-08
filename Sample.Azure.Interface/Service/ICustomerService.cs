using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Service
{
    public interface ICustomerService
    {
        Model.Customer.CustomerModel Get(Guid customerId);

        void Save(Model.Customer.CustomerModel customerModel);
    }
}
