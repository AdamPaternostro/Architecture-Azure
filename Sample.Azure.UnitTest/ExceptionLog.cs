using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample.Azure.UnitTest
{
    [TestClass]
    public class ExceptionLog
    {
        public ExceptionLog()
        {
            // Configure the system based upon where it is running
            // This will set all configuration values and register all types for dependency injection
            Sample.Azure.Config.Configuration.Configure();
        }

        [TestMethod]
        public void LogException()
        {
            System.Exception ex = new System.Exception("Unit test exception: " + Guid.NewGuid().ToString());

            Interface.Service.IExceptionLogService exceptionLogService = DI.Container.Resolve<Interface.Service.IExceptionLogService>();

            Guid exceptionLogId = Guid.NewGuid();

            exceptionLogService.Save(ex, exceptionLogId);

            Model.ExceptionLog.ExceptionLogModel exceptionLogModel = exceptionLogService.Get(exceptionLogId);

            Assert.AreEqual(exceptionLogModel.ExceptionMessage, ex.Message);

        }
    }
}
