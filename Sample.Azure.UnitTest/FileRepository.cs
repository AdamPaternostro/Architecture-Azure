using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample.Azure.UnitTest
{
    [TestClass]
    public class FileRepository
    {
        public FileRepository()
        {
            // Configure the system based upon where it is running
            // This will set all configuration values and register all types for dependency injection
            Sample.Azure.Config.Configuration.Configure();
        }


        [TestMethod]
        public void ExerciseBlobStorage()
        {
            Interface.Repository.IFileRepository fileRepository = DI.Container.Resolve<Interface.Repository.IFileRepository>();
            string jsonText = "{ \"name\" : \"test\" }";
            string container = "unittest";
            string fileName = Guid.NewGuid().ToString() + ".json"; ;

            Model.File.FileModel fileModel = new Model.File.FileModel();
            fileModel.Container = container;
            fileModel.Name = fileName;
            fileModel.Text = jsonText;
            fileRepository.PutFileAsText(fileModel);

            Model.File.FileModel loadedFileModel =  fileRepository.GetFileAsText(container, fileName);

            Assert.IsTrue(loadedFileModel != null);
            Assert.AreEqual(fileModel.Text, loadedFileModel.Text);

            fileRepository.DeleteFile(container, null, fileName);

            Model.File.FileModel loadedDeleteModel = fileRepository.GetFileAsText(container, fileName);

            Assert.IsTrue(loadedDeleteModel == null);
        } // ExerciseBlobStorage

    }
}
