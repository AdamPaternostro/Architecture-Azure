using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Config
{
    public class Environment
    {
        public enum SystemEnvironment { DevLocal, DevAzure, QAAzure, ProdAzure, UnitTestLocal }
    }
}
