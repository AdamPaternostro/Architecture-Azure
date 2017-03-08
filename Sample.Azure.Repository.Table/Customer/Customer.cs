using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sample.Azure.Repository.Table.Customer
{
    internal class Customer : BaseAzureTable
    {
        public Customer()
            : base()
        {
        }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerZip { get; set; }

        public string CustomerState { get; set; }

        public int CustomerRanking { get; set; }

    }
}
