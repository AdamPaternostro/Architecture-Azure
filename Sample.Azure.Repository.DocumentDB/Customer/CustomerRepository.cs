using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Sample.Azure.Repository.DocumentDB.Customer
{
    public class CustomerRepository : Interface.Repository.ICustomerRepository 
    {

        public Sample.Azure.Model.Customer.CustomerModel Select(Guid customerId)
        {
            DocumentDBRepository<Sample.Azure.Model.Customer.CustomerModel> documentDBRepository = new DocumentDBRepository<Model.Customer.CustomerModel>();
            return documentDBRepository.GetItems(o => o.CustomerId == customerId).FirstOrDefault();
        }

        public void InsertOrReplace(Sample.Azure.Model.Customer.CustomerModel customerModel)
        {
            DocumentDBRepository<Sample.Azure.Model.Customer.CustomerModel> documentDBRepository = new DocumentDBRepository<Model.Customer.CustomerModel>();
            documentDBRepository.Client.CreateDocumentAsync(documentDBRepository.CollectionUri, customerModel);

        }

    }
}
