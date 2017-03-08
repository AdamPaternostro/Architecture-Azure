using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sample.Azure.Model.Customer
{
    [Serializable()]
    public class CustomerModel : BaseModel
    {
        public CustomerModel()
        {
            this.CustomerId = Guid.NewGuid();
        }

        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Please provide a customer name.")]
        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerZip { get; set; }

        public string CustomerState { get; set; }

        public int CustomerRanking { get; set; }

    }
}
