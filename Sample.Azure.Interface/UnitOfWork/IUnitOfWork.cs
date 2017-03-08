using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Interface
{ 
    /// <summary>
    /// Unit of work for doing database transactions accross service objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork<T>
    {
        void BeginTransaction();
        void Commit();
        T DbContext { get; }
    } // interface
} // namespace
