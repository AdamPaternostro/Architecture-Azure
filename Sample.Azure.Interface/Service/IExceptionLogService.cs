using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Service
{
    public interface IExceptionLogService
    {
        void Save(Model.ExceptionLog.ExceptionLogModel exceptionLogModel);

        void Save(System.Exception exception);

        void Save(System.Exception exception, Guid exceptionLogId);

        void Save(System.Exception exception, Guid exceptionLogId, Guid handlingInstanceId);

        Model.ExceptionLog.ExceptionLogModel Get(Guid exceptionLogId);

    } // interface
} // namespace
