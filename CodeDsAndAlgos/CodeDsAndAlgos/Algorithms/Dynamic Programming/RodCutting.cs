using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.DynamicProgramming
{
    public class RodCutting
    {       

        public int GetMaxBenefit(int[] p,int n,int c)
        {
            int mq;            
            int ms = 0;
            int mc = 0;

            int[] memoizedr = new int[p.Length];       
            int[] solution =  new int[p.Length]; 

            memoizedr[0] = 0;
            
            for (int i = 1; i < p.Length; i++)
            {
                mq = Int32.MinValue;
                ms = 0;
                mc = c;
                for (int j = 1; j <= i; j++)
                {
                    if (i == j) mc = 0;

                    if (mq < p[j] + memoizedr[i - j] - mc)
                    {
                        mq = p[j] + memoizedr[i - j] - mc;
                        ms = j;
                    }
                }

                memoizedr[i] = mq; 
                solution[i] = ms;
            }

            return memoizedr[n];
        }       
    }
}
