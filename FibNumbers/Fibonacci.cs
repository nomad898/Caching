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
            redis = ConnectionMultiplexer.Connect(connectionString);
            db = redis.GetDatabase();
            memoryCache = MemoryCache.Default;
        }

        public int CalculateRedis(int value)
        {
            if (value <= 1)
            {
                return 1;
            }
            int result = GetValue(value.ToString());
            result = (CalculateRedis(value - 1) + CalculateRedis(value - 2));
            SetValue(value.ToString(), result);

            if (result != 0)
            {
                return result;
            }
            return result;
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

        public int Calculate(int value)
        {
            if (value <= 1)
            {
                return 1;
            }
            var  result = (Calculate(value - 1) + Calculate(value - 2));           
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

        private int GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                db.SetAdd(key, default(int));
                return default(int);
            }
            db.SetAdd(key, default(int));
            var redisValue = db.StringGet(key);
            if (redisValue.HasValue)
            {
                int.TryParse(redisValue, out int result);
                return result;
            }
            return default(int);
        }

        private void SetValue(string key, int value)
        {
            db.StringSet(key, value);
        }
    }
}
