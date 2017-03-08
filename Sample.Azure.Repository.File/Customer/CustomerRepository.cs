using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.File.Customer
{
    public class CustomerRepository : Interface.Repository.ICustomerRepository
    {
        Interface.Repository.IFileRepository fileRepository = null;
        public CustomerRepository(Interface.Repository.IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }

        public Sample.Azure.Model.Customer.CustomerModel Select(Guid customerId)
        {
            string fileName = customerId.ToString().ToLower() + ".json";
            string container = "customer";

            Common.JSON.JSONService jsonService = new Common.JSON.JSONService();
            Model.File.FileModel loadedFileModel = fileRepository.GetFileAsText(container, fileName);

            if (loadedFileModel == null)
            {
                return null;
            }
            else
            {
                string loadedJson = loadedFileModel.Text;
                Model.Customer.CustomerModel loadedModel = jsonService.Deserialize<Model.Customer.CustomerModel>(loadedJson);
                return loadedModel;
            }

        }

        public void InsertOrReplace(Sample.Azure.Model.Customer.CustomerModel customerModel)
        {
            Common.JSON.JSONService jsonService = new Common.JSON.JSONService();
            string json = jsonService.Serialize<Model.Customer.CustomerModel>(customerModel);

            Model.File.FileModel fileModel = new Model.File.FileModel();
            string fileName = customerModel.CustomerId.ToString().ToLower() + ".json";
            string container = "customer";

            fileModel.Container = container;
            fileModel.Name = fileName;
            fileModel.Text = json;

            fileRepository.PutFileAsText(fileModel);
        }

    }
}
