using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sample.Azure.Model.File
{
    public class FileModel
    {
        /// <summary>
        /// The Drive or Blob Container name (e.g. "C:" or "xyzContainer")
        /// </summary>
        [Required(ErrorMessage ="Container name may not be empty or null.")]
        public string Container { get; set; }

        /// <summary>
        /// Name of the file (case sensitive)
        /// </summary>
        [Required(ErrorMessage = "File name may not be empty or null.")]
        public string Name { get; set; }

        /// <summary>
        /// Path (can be null)  (case sensitive)
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The bytes of the file (assuming binary file)
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// The text of the file (assuming text file)
        /// </summary>
        public string Text{ get; set; }

        /// <summary>
        /// For concurrency
        /// </summary>
        public string eTag { get; set; }


        /// <summary>
        /// Tells the storage engine only to save with matching eTags
        /// </summary>
        public bool VerifyETagWhenSaving { get; set; }


        // Name and Path (no container)
        public string FullPath
        {
            get
            {
                string fullPath = string.Empty;
                if (string.IsNullOrWhiteSpace(this.Path))
                {
                    fullPath = this.Name;
                }
                else
                {
                    fullPath = System.IO.Path.Combine(this.Path, this.Name);
                }
                return fullPath;
            }
        }

        public string FullName
        {
            get
            {
                string fullName = string.Empty;
                if (string.IsNullOrWhiteSpace(this.Path))
                {
                    fullName = System.IO.Path.Combine(this.Container.ToLower(), this.Name);
                }
                else
                {
                    fullName = System.IO.Path.Combine(this.Container.ToLower(), this.Path, this.Name);
                }
                return fullName;
            }
        }


    } // class
} // namespace

