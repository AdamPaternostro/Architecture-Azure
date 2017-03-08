using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Model.Queue
{
    public class QueueModel
    {
        public string Id { get; set; }

        public string PopReceipt { get; set; }

        public string Item { get; set; }

        public DateTime LockExpirationInUTC { get; set; }

        public bool IsLockedExpired
        {
            get
            {
                return this.LockExpirationInUTC > DateTime.UtcNow;
            }
        }
    }
}
