using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.SqlServer.Repository
{
    public class UnitOfWork<T> : Sample.Azure.Interface.Interface.IUnitOfWork<T>, IDisposable where T : System.Data.Entity.DbContext, new()
    {
        T dbContext = null;
        System.Data.Entity.DbContextTransaction dbContextTransaction = null;
        private readonly string threadSessionId = null;
        bool isTopMostTransaction = false;


        /// <summary>
        /// Either enlist in the current transaction or create a new dbContext
        /// </summary>
        /// <param name="databaseConnection"></param>
        public UnitOfWork()
        {
            threadSessionId = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            dbContext = (T)System.Threading.Thread.GetData(System.Threading.Thread.GetNamedDataSlot(threadSessionId));

            if (dbContext == null)
            {
                isTopMostTransaction = true;
                dbContext = new T();
                System.Threading.Thread.SetData(System.Threading.Thread.GetNamedDataSlot(threadSessionId), dbContext);
            } // dbContext == null       
        } // UnitOfWork


        /// <summary>
        /// Starts a topmost transaction
        /// </summary>
        public void BeginTransaction()
        {
            if (isTopMostTransaction)
            {
                dbContextTransaction = dbContext.Database.BeginTransaction();
            }
        } // BeginTransaction


        /// <summary>
        /// Commits the transaction
        /// </summary>
        public void Commit()
        {
            if (isTopMostTransaction && dbContextTransaction != null)
            {
                dbContextTransaction.Commit();
                dbContextTransaction.Dispose();
                dbContextTransaction = null;
            }
        } // Commit


        /// <summary>
        /// Exposes EF's dbContext
        /// This would be passed to each repository in order to do an n-tier (or accross business objects) transaction
        /// </summary>
        public T DbContext
        {
            get
            {
                return dbContext;
            }
        } // DbContext


        /// <summary>
        /// Dispose the dbContext and transaction.  If a transaction has not been committed then roll it back
        /// </summary>
        public void Dispose()
        {
            if (isTopMostTransaction)
            {
                if (dbContextTransaction != null)
                {
                    dbContextTransaction.Rollback();
                }

                // Clean up
                dbContext.Dispose();
                dbContext = null;

                // Remove the session
                System.Threading.Thread.FreeNamedDataSlot(threadSessionId);
            } // isTopMostTransaction
        } // Dispose


    } // class
} // namespace
