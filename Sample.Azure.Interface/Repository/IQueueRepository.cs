using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Interface.Repository
{
    public interface IQueueRepository
    {
        /// <summary>
        /// Ands an item to the queue
        /// The queue data should be JSON or other strucuted data.
        /// When passing a lot of data to a queue, you should place a file in blob storage instead
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="queueData"></param>        
        void Enqueue(string queueName, string queueData);

        /// <summary>
        /// Grabs an item from the queue locking it for a specified timespan (you need to call DeleteItem to remove
        /// it from the queue)
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        Model.Queue.QueueModel Dequeue(string queueName, TimeSpan timeSpan);

        /// <summary>
        /// Removes an item from the queue provided the lock has not timed out
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="queueModel"></param>
        /// <returns></returns>
        bool DeleteItem(string queueName, Sample.Azure.Model.Queue.QueueModel queueModel);
    }
}
