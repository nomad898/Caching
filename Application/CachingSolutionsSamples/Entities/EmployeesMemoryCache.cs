using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Entities
{
    public class EmployeesMemoryCache : ICache<Employee>
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_Employees";

        public IEnumerable<Employee> Get(string forUser)
        {
            return (IEnumerable<Employee>)cache.Get(prefix + forUser);
        }

        public void Set(string forUser, IEnumerable<Employee> categories)
        {
            cache.Set(prefix + forUser, categories, ObjectCache.InfiniteAbsoluteExpiration);
        }
    }
}
