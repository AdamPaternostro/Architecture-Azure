using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Repository.Queue
{
    public class QueueRepository : Sample.Azure.Interface.Repository.IQueueRepository
    {
        public QueueRepository ()
        {

        }


        /// <summary>
        /// Ands an item to the queue
        /// The queue data should be JSON or other strucuted data.
        /// When passing a lot of data to a queue, you should place a file in blob storage instead
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="queueData"></param>
        public void Enqueue(string queueName, string queueData)
        {
            AzureQueueHelper azureQueueHelper = new AzureQueueHelper();
            azureQueueHelper.PutQueueItem(queueName.ToLower(), queueData);
        }


        /// <summary>
        /// Grabs an item from the queue locking it for a specified timespan (you need to call DeleteItem to remove
        /// it from the queue)
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public Model.Queue.QueueModel Dequeue(string queueName, TimeSpan timeSpan)
        {
            AzureQueueHelper azureQueueHelper = new AzureQueueHelper();
            DateTime expirationDate = DateTime.UtcNow.Add(timeSpan);
            Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage item = azureQueueHelper.GetQueueItem(queueName.ToLower());
            if (item == null)
            {
                return null;
            }
            else
            {
                Model.Queue.QueueModel queueModel = new Model.Queue.QueueModel();
                queueModel.Id = item.Id;
                queueModel.PopReceipt = item.PopReceipt;
                queueModel.Item = item.AsString;
                queueModel.LockExpirationInUTC = expirationDate;

                return queueModel;           
            }
        }

        /// <summary>
        /// Removes an item from the queue provided the lock has not timed out
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="queueModel"></param>
        /// <returns></returns>
        public bool DeleteItem(string queueName, Model.Queue.QueueModel queueModel)
        {
            if (queueModel.LockExpirationInUTC < DateTime.UtcNow)
            {
                AzureQueueHelper azureQueueHelper = new AzureQueueHelper();
                azureQueueHelper.DeleteQueueItem(queueName, queueModel.Id, queueModel.PopReceipt);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
