using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.DynamicProgramming
{
   public class FibbonaciSeries
    {
        public int GetValueIterative(int n)
        {
            int n1 = 0;
            int n2 = 1;
            int f = 0;

            for (int i = 0; i < n; i++)
            {
                f = n1 + n2;
                n1 = n2;
                n2 = f;
            }

            return f;
        }
    }
}
