using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Sample.Azure.Common.JSON
{
    /// <summary>
    /// Needed so the $type is added for polymorphic lists
    /// </summary>
    public class KnownTypesBinder : SerializationBinder
    {
        public IList<Type> KnownTypes { get; set; }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
}
