using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindLibrary;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Generic;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
		[TestMethod]
		public void MemoryCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesMemoryCache());

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

		[TestMethod]
		public void RedisCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesRedisCache("localhost"));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

        [TestMethod]
        public void GenericMemoryCache()
        {
            var cahceManager = new CacheManager<Category>(new GenericMemoryCache<Category>());

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(cahceManager.Get().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
        public void GenericRedisCache()
        {
            var categoryManager = new CacheManager<Category>(new GenericRedisCache<Category>("localhost"));

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(categoryManager.Get().Count());
                Thread.Sleep(100);
            }
        }
    }
}
