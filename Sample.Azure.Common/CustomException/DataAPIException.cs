using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Common.CustomException
{
    public class DataAPIException : Exception
    {
        private string reasonPhase;
        private System.Net.HttpStatusCode httpStatusCode;


        public DataAPIException(System.Net.HttpStatusCode httpStatusCode, string reasonPhase)
        {
            this.httpStatusCode = httpStatusCode;
            this.reasonPhase = reasonPhase;
        }

        public string ReasonPhase
        {
            get { return this.reasonPhase; }
        }

        public System.Net.HttpStatusCode HttpStatusCode
        {
            get { return this.httpStatusCode; }
        }

    } // class
} // namespace
