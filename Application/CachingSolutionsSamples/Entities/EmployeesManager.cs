using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CachingSolutionsSamples.Entities
{
    public class EmployeesManager
    {
        private ICache<Employee> cache;

        public EmployeesManager(ICache<Employee> cache)
        {
            this.cache = cache;
        }

        public IEnumerable<Employee> GetCategories()
        {
            Console.WriteLine("Get Employees");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var employees = cache.Get(user);

            if (employees == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    employees = dbContext.Employees.ToList();
                    cache.Set(user, employees);
                }
            }

            return employees;
        }
    }
}
