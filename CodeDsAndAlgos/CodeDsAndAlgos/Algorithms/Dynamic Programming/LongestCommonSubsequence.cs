using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.DynamicProgramming
{
    public class LongestCommonSubsequence
    {
        public int GetLCSNR(string str1, string str2, int s1, int s2)
        {
            int lcs = 0;

            if (s1 < 0 || s2 < 0)
                return 0;

            if (str1[s1] == str2[s2])
                lcs =  1 + GetLCSNR(str1, str2, s1 - 1, s2 - 1);
            else
            {
                lcs = Math.Max(GetLCSNR(str1, str2, s1 - 1, s2), 
                    GetLCSNR(str1, str2, s1, s2-1));
            }
            return lcs;
        }

        public int GetLcsMr(string str1, string str2, int s1, int s2)
        {
            int[,] m = new int[s1+1, s2+1];

            for (int i = 1; i <= s1; i++)
            {
                for (int j = 1; j <= s2; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                        m[i, j] = 1 + m[i - 1, j - 1];
                    else
                    {
                            m[i, j] = Math.Max(m[i - 1, j],
                                               m[i, j - 1]);
                    }
                }
            }
            InternalPrintSolution(str1, str2, s1, s2, m);
            return m[s1,s2];
        }

        private char[] InternalPrintSolution(string str1, string str2, 
                                            int s1, int s2,int[,] m)
        {
            int i = s1, j = s2;
            char[] result = new char[m[s1,s2]];
            int idx = result.Length;

            while (i > 0 && j > 0)
            {
                if (str1[i - 1] == str2[j - 1])
                {
                    Console.WriteLine(str1[i - 1]);
                    result[--idx] = str1[i - 1];
                    i--; j--;
                }
                else
                {
                    if (m[i - 1, j] >= m[i, j - 1])
                        i--;
                    else
                        j--;
                }
            }           
            return result;
        }
    }
}




