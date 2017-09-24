using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.DynamicProgramming
{
    class StockBuySellKTransactions
    {
        /**
         * This is slow method but easier to understand.
         * Time complexity is O(k * number of days ^ 2)
         * T[i][j] = max(T[i][j-1], max(prices[j] - prices[m] + T[i-1][m])) where m is 0...j-1
        */

        public int GetMaxProfitSlow(int[] prices, int k)
        {
            int[,] T = new int[k + 1, prices.Length];

            for (int i = 1; i <= k; i++)
            {
                for (int j = 1; j < prices.Length; j++)
                {
                    int max = 0;

                    for (int m = 0; m < j; m++)
                    {
                        max = Math.Max(max, prices[j] - prices[m] + T[i - 1,m]);
                    }

                    T[i,j] = Math.Max(max, T[i,j - 1]);
                }
            }

            return T[k + 1,prices.Length - 1];
        }
    }
}
