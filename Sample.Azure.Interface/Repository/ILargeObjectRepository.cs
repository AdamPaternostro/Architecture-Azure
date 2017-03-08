using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Repository
{
    public interface ILargeObjectRepository
    {
        Sample.Azure.Model.LargeObject.LargeObjectModel Select(int largeObjectId);

        void InsertOrReplace(Sample.Azure.Model.LargeObject.LargeObjectModel largeObjectModel);
    }
}
