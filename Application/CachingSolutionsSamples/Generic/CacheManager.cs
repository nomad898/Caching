using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Generic
{
    public class CacheManager<T> where T : class
    {
        private ICache<T> cache;
        private readonly string managerName;

        public CacheManager(ICache<T> cache)
        {
            this.cache = cache;
            this.managerName = nameof(T);
        }

        public IEnumerable<T> Get()
        {
            Console.WriteLine($"Get {managerName}");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var cacheList = cache.Get(user);

            if (cacheList == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    cacheList = dbContext.Set<T>().ToList();
                    cache.Set(user, cacheList);
                }
            }

            return cacheList;
        }
    }
}
