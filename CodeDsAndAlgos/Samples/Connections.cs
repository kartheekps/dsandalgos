using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
   public  class Connections
    {
       public static bool hasDeadlock(int[][] c)
        {
            int len = c.Length;
            bool[,] visited = new bool[len, len];

            for (int i = 0; i < len; i++)
            {
              for (int j = 0; j < c[i].Length; j++)
                {

                    int k = c[i][j];

                    if (visited[k,i])
                        return true;

                    visited[i,k] = true;

                    for (int x = 0; x < len; x++)
                    {
                        visited[x,k] = visited[x,k] || visited[x,i];
                        if (visited[k,x] && visited[x,i])
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
