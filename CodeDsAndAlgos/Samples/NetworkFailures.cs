using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    public class Result
    {
        public int Count { get; set; }
        public int Time { get; set; }
    }

    public class NetworkFailures
    {
      public static int singlePointOfFailure(int[][] c)
        {
            int[] low = new int[c.Length];
            int[] arr = new int[c.Length];
            int[] parent = new int[c.Length];
            Result state = new Result();
            dfsUtil(c, 0, -1, arr, low, parent, state);
            return state.Count;
        }
        public static int dfsUtil(int[][] c, int u, int parv, int[] arr, int[] low, int[] parent, Result state)
        {
            low[u] = ++state.Time;
            arr[u] = low[u];
            parent[u] = parv;
            for (int v = 0; v < c[u].Length; v++)
            {
                if (c[u][v] == 1)
                {
                    if (arr[v] == 0)
                        low[u] = Math.Min(low[u], dfsUtil(c, v, u, arr, low, parent, state));
                    else if (parent[u] != v)                    
                        low[u] = Math.Min(low[u], arr[v]);

                    if (low[v] > arr[u])
                        state.Count++;
                }
            }
            return low[u];
        }
    }
}
