using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Sample.Azure.Model
{
    [Serializable()]
    public class BaseModel 
    {
        /// <summary>
        /// Sets the base auditing fields
        /// </summary>
        public BaseModel() 
        {
            DateTime now = DateTime.UtcNow;

            // Get from IPrinciple
            this.UserCreated = "Test";
            this.UserUpdated = "Test";

            this.DateCreated = now;
            this.DateUpdated = now;

            this.IsDeleted = false;
        }

        [Required(ErrorMessage = "User Created is required")]
        [StringLength(100, ErrorMessage = "User Created cannot exceed 100 characters.")]
        public string UserCreated { get; set; }

        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "User Updated is required")]
        [StringLength(100, ErrorMessage = "User Updated cannot exceed 100 characters.")]
        public string UserUpdated { get; set; }

        public DateTime DateUpdated { get; set; }

        public bool IsDeleted { get; set; }

        [StringLength(100, ErrorMessage = "User Deleted cannot exceed 100 characters.")]
        public string UserDeleted { get; set; }

        public Nullable<DateTime> DateDeleted { get; set; }
       
        public string RowVersion { get; set; }
    }
}