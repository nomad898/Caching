using NorthwindLibrary;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Generic
{
    public class GenericRedisCache<T> : ICache<T>
	{
		private ConnectionMultiplexer redisConnection;
        string prefix = $"Cache_{nameof(T)}";
        DataContractSerializer serializer = new DataContractSerializer(
            typeof(IEnumerable<T>));

        public GenericRedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public IEnumerable<T> Get(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + forUser);
            if (s == null)
                return null;

            return (IEnumerable<T>)serializer
                .ReadObject(new MemoryStream(s));
        }

        public void Set(string forUser, IEnumerable<T> list)
        {
            var db = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (list == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, list);
                db.StringSet(key, stream.ToArray());
            }
        }      
    }
}
