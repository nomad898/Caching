using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FibNumbers
{
    internal class Fibonacci
    {
        private ConnectionMultiplexer redis;
        IDatabase db;
        private MemoryCache memoryCache;

        public Fibonacci(string connectionString)
        {
            var options = ConfigurationOptions.Parse(connectionString);
            options.AllowAdmin = true;
            redis = ConnectionMultiplexer.Connect(options);
            db = redis.GetDatabase();
            var endpoints = redis.GetEndPoints();
            var server = redis.GetServer(endpoints[0]);
            server.FlushAllDatabases();
            memoryCache = MemoryCache.Default;
        }

        public int Calculate(int value)
        {
            if (value <= 1)
            {
                return 1;
            }
            var result = (Calculate(value - 1) + Calculate(value - 2));
            return result;
        }

        public int CalculateRedis(int value)
        {
            if (value <= 1)
            {
                return 1;
            }
            int result = GetValue(value.ToString());         
            if (result != 0)
            {
                return result;
            }
            result = (CalculateRedis(value - 1) + CalculateRedis(value - 2));
            SetValue(value.ToString(), result);
            return result;
        }


        private int GetValue(string key)
        {
            var redisValue = db.StringGet(key);
            if (redisValue.HasValue)
            {
                int.TryParse(redisValue, out int result);
                return result;
            }
            return 0;
        }

        private void SetValue(string key, int value)
        {
            db.StringSet(key, value);
        }

        public int CalculateStandardCache(int value)
        {
            if (value <= 1)
            {
                return 1;
            }
            int result = GetValueFromStandardCache(value.ToString());
            if (result != 0)
            {
                return result;
            }
            result = (CalculateStandardCache(value - 1) + CalculateStandardCache(value - 2));
            SetValueFromStandardCache(value.ToString(), result.ToString());                     
            return result;
        }

        private int GetValueFromStandardCache(string key)
        {
            var result = memoryCache.Get(key);
            return Convert.ToInt32(result);
        }

        private void SetValueFromStandardCache(string key, string value)
        {
            memoryCache.Set(key, value, DateTime.Now.AddMinutes(1));
        }          

    }
}
