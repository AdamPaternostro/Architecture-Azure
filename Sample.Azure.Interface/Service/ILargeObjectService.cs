using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Service
{
    public interface ILargeObjectService
    {
        Model.LargeObject.LargeObjectModel Get(int largeObjectId);

        void Save(Model.LargeObject.LargeObjectModel largeObjectModel);
    }
}
