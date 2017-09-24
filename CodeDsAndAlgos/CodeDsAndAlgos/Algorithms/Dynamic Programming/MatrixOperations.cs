using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.DynamicProgramming
{
  public class MatrixOperations
    {
        public int[][] Multiply(int[][] a, int[][] b, int r1,int c1,int r2,int c2)
        {
            int[][] result = null;

            if (c1 == r2)
            {
                result = new int[r1][];
                for (int i = 0; i < r1; i++)
                    result[i] = new int[c2];
            }
            else if (c2 == r1)
            {
                result = new int[r2][];
                for (int i = 0; i < r2; i++)
                    result[i] = new int[c1];

                int[][] temp = a;
                a = b;
                b = temp;

                int temp1 = r1;
                r1 = r2;
                r2 = r1;

                temp1 = c1;
                c1 = c2;
                c2 = c1;
            }
            else
            {
                throw new Exception("wrong dimensions");
            }

            for (int i = 0; i < r1; i++)
            {
                for (int j = 0; j < c2; j++)
                {
                    int dproduct = 0;

                    for (int k = 0; k < c1; k++)
                    {
                        dproduct += a[i][k] * b[k][j];
                    }
                    result[i][j] = dproduct;
                }
            }

            return result;
        }
    }
}
