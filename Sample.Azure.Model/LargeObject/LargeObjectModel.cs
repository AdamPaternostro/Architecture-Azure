using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Sample.Azure.Model.LargeObject
{
    [Serializable()]
    public class LargeObjectModel : BaseModel
    {
        public LargeObjectModel()
        {
            this.LargeObjectId = 0;
        }

        public int LargeObjectId { get; set; }

        public string Payload { get; set; }

    }
}
