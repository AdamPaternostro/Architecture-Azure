using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Repository
{
    public interface IFileRepository
    {
        Sample.Azure.Model.File.FileModel GetFileAsText(string container, string path, string name);

        Sample.Azure.Model.File.FileModel GetFileAsText(string container, string name);

        Sample.Azure.Model.File.FileModel GetFileAsBytes(string container, string path, string name);

        Sample.Azure.Model.File.FileModel GetFileAsBytes(string container, string name);

        void PutFileAsText(Sample.Azure.Model.File.FileModel fileModel);

        void PutFileAsBytes(Sample.Azure.Model.File.FileModel fileModel);

        void DeleteFile(string container, string path, string name);

        bool FileExists(string container, string path, string name);

    }
}
