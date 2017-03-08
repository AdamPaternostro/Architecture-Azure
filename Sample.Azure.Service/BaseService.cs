using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Azure.Service
{
    public class BaseService
    {
        Interface.GlobalEnum.CachingStrategy cachingStrategy = Interface.GlobalEnum.CachingStrategy.Near;

        /// <summary>
        /// Set this so we can non get from cache for admin pages and such
        /// </summary>
        /// <param name="cachingStrategy"></param>
        public void SetCachingStrategy(Interface.GlobalEnum.CachingStrategy cachingStrategy)
        {
            this.cachingStrategy = cachingStrategy;
        }


        /// <summary>
        /// Loads a JSON string from blob storage (or cache)
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="fileRepository"></param>
        /// <param name="cacheKey"></param>
        /// <param name="blobContainer"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public List<T> LoadJsonFile<T>(
            Interface.Cache.ICacheManager cacheManager,
            Interface.Repository.IFileRepository fileRepository,
            string cacheKey,
            string blobContainer,
            string blobName,
            TimeSpan timeSpan)
        {
            // Try to get from cache
            if (cachingStrategy == Interface.GlobalEnum.CachingStrategy.Near)
            {
                List<T> cacheResult = cacheManager
                    .GetNearCache<List<T>>(cacheKey, timeSpan);
                if (cacheResult != null)
                {
                    return cacheResult;
                }
            }

            Model.File.FileModel fileModel = fileRepository.GetFileAsText(blobContainer, blobName);
            if (fileModel == null)
            {
                fileModel = CreateDefaultData();
            }

            Common.JSON.JSONService jsonService = new Common.JSON.JSONService();
            List<T> result = jsonService.Deserialize<List<T>>(fileModel.Text);

            if (cachingStrategy != Interface.GlobalEnum.CachingStrategy.None)
            {
                cacheManager.Set(cacheKey, result, TimeSpan.FromMinutes(60));
            }

            return result;
        }


        /// <summary>
        /// Used to create initial set of blob data
        /// </summary>
        /// <returns></returns>
        public virtual Model.File.FileModel CreateDefaultData()
        {
            return null; // must be implemented by consumer
        }



    } // class
} // namespace
