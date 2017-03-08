using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Service.Customer
{
    public class CustomerService : BaseService, Interface.Service.ICustomerService
    {
        Interface.Repository.ICustomerRepository customerRepository = null;

        public CustomerService(Interface.Repository.ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }


        /// <summary>
        /// Gets a customer based upon their customer key (provided by LTI Token)
        /// </summary>
        /// <param name="customerKey"></param>
        /// <returns></returns>
        public Model.Customer.CustomerModel Get(Guid customerId)
        {
            if (customerId == Guid.Empty)
            {
                List<Model.BrokenRule.BrokenRuleModel> brokenRuleList = new List<Model.BrokenRule.BrokenRuleModel>();
                brokenRuleList.Add(new Model.BrokenRule.BrokenRuleModel() { Message = "Please provide a customer id.", PropertyName = "CustomerId" });
                throw new Common.CustomException.ModelValidationException(brokenRuleList);
            }
            return customerRepository.Select(customerId);
        }


        /// <summary>
        /// Validates and saves a customer
        /// </summary>
        /// <param name="customerModel"></param>
        public void Save(Model.Customer.CustomerModel customerModel)
        {
            Common.Validation.ValidationService validationService = new Common.Validation.ValidationService();
            validationService.ValidateObject(customerModel);

            customerRepository.InsertOrReplace(customerModel);
        }

    }
}
