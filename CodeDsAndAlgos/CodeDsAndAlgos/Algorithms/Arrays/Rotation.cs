using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.Arrays
{
   public class Rotation
    {
       static void LeftRotation()
        {
            string[] tokens_n = Console.ReadLine().Split(' ');
            //int n = Convert.ToInt32(tokens_n[0]);
            //int k = Convert.ToInt32(tokens_n[1]);
            string[] a_temp = Console.ReadLine().Split(' ');
            int[] a = new int[] { 1, 2, 3, 4, 5 };
            int n = a.Length;
            int k = 1;

            int nk = k % n;

            if (nk == 0)
                PrintArray(a);
            else
            {
                int i = 0;
                while(i < nk)
                {
                    while (nk < n)
                    {
                        int temp = a[i];
                        a[i] = a[nk];
                        a[nk] = temp;

                        i++;
                        nk++;
                    }
                    if (nk == n) nk--;
                }
                PrintArray(a);
            }
        }

        static void PrintArray(int[] nums)
        {
            for(int i = 0; i < nums.Length -1; i++)            
                Console.WriteLine(nums[i]+' ');

            Console.WriteLine(nums[nums.Length - 1]);
        }
    }
}

