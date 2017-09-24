using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.DynamicProgramming
{
   public class MinNoOfJumps
    {
        public int GetMinNR(int[] jumps,int d)
        {
            int result =  GetMinUtilNR(jumps,d);

            return result;
        }

        private int GetMinUtilNR(int[] jumps, int d)
        {
            if (d == 0) return 0;

            int ms = Int32.MaxValue;

            for (int i = d - 1; i >= 0; i--)
            {
                if (i + jumps[i] >= d)
                {
                    ms = Math.Min(ms, GetMinUtilNR(jumps, i) + 1);
                }
            }

            return ms;
        }

        public int GetMinTDMR(int[] jumps, int d)
        {
            int[] mr = new int[jumps.Length];

            for (int i = 0; i < mr.Length; i++)
                mr[i] = Int32.MaxValue;

            mr[0] = 0;

            int result = GetMinUtilTDMR(jumps, d,mr);   

            return mr[d];
        }
        
        private int GetMinUtilTDMR(int[] jumps, int d, int[] mr)
        {
            if (mr[d] >= 0 && mr[d] != Int32.MaxValue)
                return mr[d];

            int ms = Int32.MaxValue;

            for (int i = d - 1; i >= 0; i--)
            {
                if (i + jumps[i] >= d )
                {
                    ms = Math.Min(ms, GetMinUtilTDMR(jumps, i,mr) + 1);
                }
            }

            mr[d] = ms;

            return ms;
        }

        public int GetMinBUM(int[] jumps, int d)
        {
            int[] mr = new int[jumps.Length];
            for (int i = 0; i < mr.Length; i++)
                mr[i] = Int32.MaxValue;

            mr[0] = 0;
            int ms = Int32.MaxValue;

            for (int i = 1; i < jumps.Length; i++)
            {
                ms = Int32.MaxValue;

                for (int j = i-1; j >=0 ; j--)
                {
                    if(jumps[j] + j >= i) 
                        ms = Math.Min(ms, mr[j] + 1);
                }
                mr[i] = ms;
            }
            
            return mr[d];
        }
    }
}
