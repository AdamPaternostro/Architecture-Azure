using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Sample.Azure.Repository.File
{
    public class FileRepository : Sample.Azure.Interface.Repository.IFileRepository
    {

        /// <summary>
        /// Does a file exist
        /// </summary>
        /// <param name="container"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool FileExists(string container, string path, string name)
        {
            if (string.IsNullOrWhiteSpace(container)) { throw new ArgumentException("Container may not be null;"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Name may not be null;"); }

            string fullPath = string.Empty;
            if (string.IsNullOrWhiteSpace(path))
            {
                fullPath = name;
            }
            else
            {
                fullPath = System.IO.Path.Combine(path, name);
            }

            AzureBlobHelper azureBlobHelper = new AzureBlobHelper();
            return azureBlobHelper.Exists(container.ToLower(), fullPath);
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="container"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public void DeleteFile(string container, string path, string name)
        {
            if (string.IsNullOrWhiteSpace(container)) { throw new ArgumentException("Container may not be null;"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Name may not be null;"); }

            string fullPath = string.Empty;
            if (string.IsNullOrWhiteSpace(path))
            {
                fullPath = name;
            }
            else
            {
                fullPath = System.IO.Path.Combine(path, name);
            }

            AzureBlobHelper azureBlobHelper = new AzureBlobHelper();
            azureBlobHelper.DeleteBlob(container.ToLower(), fullPath);
        }


        public Sample.Azure.Model.File.FileModel GetFileAsBytes(string container,string name)
        {
            return GetFileAsBytes(container, name);
        }


        /// <summary>
        /// Returns the files as binary
        /// </summary>
        /// <param name="container"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sample.Azure.Model.File.FileModel GetFileAsBytes(string container, string path, string name)
        {
            if (string.IsNullOrWhiteSpace(container)) { throw new ArgumentException("Container may not be null;"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Name may not be null;"); }

            string fullPath = string.Empty;
            if (string.IsNullOrWhiteSpace(path))
            {
                fullPath = name;
            }
            else
            {
                fullPath = System.IO.Path.Combine(path, name);
            }

            AzureBlobHelper azureBlobHelper = new AzureBlobHelper();
            AzureBlobHelper.BlobResultModel blobResultModel = azureBlobHelper.GetBlob(container.ToLower(), fullPath);
            Sample.Azure.Model.File.FileModel fileModel = new Sample.Azure.Model.File.FileModel();

            System.IO.MemoryStream ms = null;
            if (blobResultModel != null)
            {
                ms = blobResultModel.Stream;
                if (ms == null)
                {
                    return null;
                }
                fileModel.eTag = blobResultModel.eTag;
                fileModel.Bytes = ms.ToArray();
            }

            fileModel.Container = container.ToLower();
            fileModel.Name = name;
            fileModel.Path = path;

            return fileModel;
        }


        /// <summary>
        /// Returns the file as text
        /// </summary>
        public Sample.Azure.Model.File.FileModel GetFileAsText(string container, string name)
        {
            return GetFileAsText(container, null, name);
        }


        /// <summary>
        /// Returns the file as text
        /// </summary>
        /// <param name="container"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sample.Azure.Model.File.FileModel GetFileAsText(string container, string path, string name)
        {
            if (string.IsNullOrWhiteSpace(container)) { throw new ArgumentException("Container may not be null;"); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Name may not be null;"); }

            string fullPath = string.Empty;
            if (string.IsNullOrWhiteSpace(path))
            {
                fullPath = name;
            }
            else
            {
                fullPath = System.IO.Path.Combine(path, name);
            }

            AzureBlobHelper azureBlobHelper = new AzureBlobHelper();

            Sample.Azure.Model.File.FileModel fileModel = new Sample.Azure.Model.File.FileModel();
            fileModel.Container = container.ToLower();
            AzureBlobHelper.BlobResultModel blobResultModel = azureBlobHelper.GetBlobAsText(container.ToLower(), fullPath);
            if (blobResultModel != null)
            {
                fileModel.Text = blobResultModel.Text;
                fileModel.eTag = blobResultModel.eTag;
            }
            fileModel.Name = name;
            fileModel.Path = path;

            if (blobResultModel == null)
            {
                return null;
            }
            else
            {
                return fileModel;
            }
        }


        /// <summary>
        /// Saves file as text
        /// </summary>
        /// <param name="fileModel"></param>
        public void PutFileAsText(Sample.Azure.Model.File.FileModel fileModel)
        {
            Sample.Azure.Common.Validation.ValidationService validationService = new Sample.Azure.Common.Validation.ValidationService();
            validationService.ValidateObject<Sample.Azure.Model.File.FileModel>(fileModel);

            AzureBlobHelper azureBlobHelper = new AzureBlobHelper();

            string fullPath = string.Empty;
            if (string.IsNullOrWhiteSpace(fileModel.Path))
            {
                fullPath = fileModel.Name;

            }
            else
            {
                fullPath = System.IO.Path.Combine(fileModel.Path, fileModel.Name);
            }

            if (fileModel.VerifyETagWhenSaving)
            {
                azureBlobHelper.SaveBlob(fileModel.Container.ToLower(), fullPath, fileModel.Text, fileModel.eTag);
            }
            else
            {
                azureBlobHelper.SaveBlob(fileModel.Container.ToLower(), fullPath, fileModel.Text);
            }
        }


        /// <summary>
        /// Saves the file as binary
        /// </summary>
        /// <param name="fileModel"></param>
        public void PutFileAsBytes(Sample.Azure.Model.File.FileModel fileModel)
        {
            Sample.Azure.Common.Validation.ValidationService validationService = new Common.Validation.ValidationService();
            validationService.ValidateObject<Sample.Azure.Model.File.FileModel>(fileModel);
            if (fileModel.Bytes == null) { throw new ArgumentException("Bytes may not be null;"); }

            AzureBlobHelper azureBlobHelper = new AzureBlobHelper();

            string fullPath = string.Empty;
            if (string.IsNullOrWhiteSpace(fileModel.Path))
            {
                fullPath = fileModel.Name;

            }
            else
            {
                fullPath = System.IO.Path.Combine(fileModel.Path, fileModel.Name);
            }
            if (fileModel.Bytes != null)
            {
                System.IO.MemoryStream ms = new MemoryStream(fileModel.Bytes);
                azureBlobHelper.SaveBlob(fileModel.Container.ToLower(), fullPath, ms);
            }
            else
            {
                System.IO.MemoryStream ms = null;
                azureBlobHelper.SaveBlob(fileModel.Container.ToLower(), fullPath, ms);
            }
        }

    } // class
} // namespace
