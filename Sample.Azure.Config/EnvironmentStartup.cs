using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Config
{
    public class EnvironmentStartup
    {
        /// <summary>
        /// This tries to read the Azure configuration which is only available when running
        /// the emulator or in Azure.  If the emulator is not running (typical debugging) then
        /// we default to "Dev Local"     
        /// </summary>
        public static Environment.SystemEnvironment GetEnvironment
        {
            get
            {
                Environment.SystemEnvironment result = Environment.SystemEnvironment.DevLocal;
                //try
                //{
                //    string enviroment = Microsoft.Azure.CloudConfigurationManager.GetSetting("SystemEnvironment");
                //    if (!string.IsNullOrWhiteSpace(enviroment))
                //    {
                //        Enum.TryParse<Environment.SystemEnvironment>(enviroment, out result);
                //    }

                //}
                //catch { }

                return result;
            }
        }

        /// <summary>
        /// This tries to read the Azure configuration which is only available when running
        /// the emulator or in Azure.  If the emulator is not running (typical debugging) then
        /// we default to "Dev Local"     
        /// </summary>
        public static string GetDatabaseConnectionString
        {
            get
            {
                string result = "???";
                //try
                //{
                //    string readValue = Microsoft.Azure.CloudConfigurationManager.GetSetting("????");
                //    if (!string.IsNullOrWhiteSpace(readValue))
                //    {
                //        result = readValue;
                //    }

                //}
                //catch { }

                return result;
            }
        }

    }
}
