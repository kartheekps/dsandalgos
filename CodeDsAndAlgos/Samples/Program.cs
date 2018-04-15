using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "1500";
            string str1 = "1500";

            Console.WriteLine(str.GetHashCode());
            Console.WriteLine(str1.GetHashCode());
            Console.ReadLine();
        }
    }
}
