using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            Fibonacci fibonacci = new Fibonacci("localhost");
            Console.WriteLine(fibonacci.CalculateStandardCache(10));
            Console.WriteLine(fibonacci.CalculateStandardCache(10));
            Console.WriteLine(fibonacci.Calculate(10));
            //Console.WriteLine(fibonacci.CalculateRedis(5));
            Console.ReadKey();
        }
    }
}
