using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Generic
{
    public class GenericMemoryCache<T> : ICache<T>
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = $"Cache_{nameof(T)}";

        public IEnumerable<T> Get(string forUser)
        {
            return (IEnumerable<T>)cache.Get(prefix + forUser);
        }

        public void Set(string forUser, IEnumerable<T> list)
        {
            cache.Set(prefix + forUser, list, ObjectCache.InfiniteAbsoluteExpiration);
        }

        public void Set(string forUser, IEnumerable<T> list, string sqlQuery)
        {
            cache.Set(prefix + forUser, list, CreatePolicy(sqlQuery));
        }

        public string ConnectionString { get; set; }

        private CacheItemPolicy CreatePolicy(string sqlQuery)
        {
            var cacheItemPolicy = new CacheItemPolicy();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    var sqlDependency = new SqlDependency(command);
                    SqlDependency.Start(ConnectionString);
                    var monitor = new SqlChangeMonitor(sqlDependency);
                    cacheItemPolicy.ChangeMonitors.Add(monitor);
                    command.ExecuteNonQuery();
                    return cacheItemPolicy;
                }
            }
        }


    }
}
