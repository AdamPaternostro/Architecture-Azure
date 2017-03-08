using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Repository
{
    public interface IExceptionLogRepository
    {
        void Save(Model.ExceptionLog.ExceptionLogModel exceptionLogModel);

        Model.ExceptionLog.ExceptionLogModel Get(Guid exceptionLogId);
    }
}
