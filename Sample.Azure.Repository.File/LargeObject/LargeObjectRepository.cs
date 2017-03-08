using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.File.LargeObject
{
    public class LargeObjectRepository : Interface.Repository.ILargeObjectRepository
    {
        Interface.Repository.IFileRepository fileRepository = null;
        public LargeObjectRepository(Interface.Repository.IFileRepository fileRepository)
        {
            this.fileRepository = fileRepository;
        }

        public Sample.Azure.Model.LargeObject.LargeObjectModel Select(int largeObjectId)
        {
            string fileName = largeObjectId.ToString().ToLower() + ".json";
            string container = "largeobject";

            Common.JSON.JSONService jsonService = new Common.JSON.JSONService();
            Model.File.FileModel loadedFileModel = fileRepository.GetFileAsText(container, fileName);

            if (loadedFileModel == null)
            {
                return null;
            }
            else
            {
                string loadedJson = loadedFileModel.Text;
                Model.LargeObject.LargeObjectModel loadedModel = jsonService.Deserialize<Model.LargeObject.LargeObjectModel>(loadedJson);
                return loadedModel;
            }

        }

        public void InsertOrReplace(Sample.Azure.Model.LargeObject.LargeObjectModel largeObjectModel)
        {
            Common.JSON.JSONService jsonService = new Common.JSON.JSONService();
            string json = jsonService.Serialize<Model.LargeObject.LargeObjectModel>(largeObjectModel);

            Model.File.FileModel fileModel = new Model.File.FileModel();
            string fileName = largeObjectModel.LargeObjectId.ToString().ToLower() + ".json";
            string container = "largeobject";

            fileModel.Container = container;
            fileModel.Name = fileName;
            fileModel.Text = json;

            fileRepository.PutFileAsText(fileModel);
        }

    }
}
