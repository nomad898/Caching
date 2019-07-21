using NorthwindLibrary;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Entities
{
    public class EmployeesRedusCache : ICache<Employee>
    {
        private ConnectionMultiplexer redisConnection;
        string prefix = "Cache_Employees";
        DataContractSerializer serializer = new DataContractSerializer(
            typeof(IEnumerable<Category>));

        public EmployeesRedusCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public IEnumerable<Employee> Get(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + forUser);
            if (s == null)
                return null;

            return (IEnumerable<Employee>)serializer
                .ReadObject(new MemoryStream(s));

        }

        public void Set(string forUser, IEnumerable<Employee> categories)
        {
            var db = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (categories == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, categories);
                db.StringSet(key, stream.ToArray());
            }
        }
    }
}
