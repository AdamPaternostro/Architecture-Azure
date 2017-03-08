using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface
{
    public class GlobalEnum
    {
        public enum IndexerRepositoryIndexType { SystemDefined, TypeAheadAutoComplete };

        public enum IndexerIndexName { Customer, LargeObject };

        public enum CachingStrategy { None, Near }; // not coded yet, Remote};
    }
}
