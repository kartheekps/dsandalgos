using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.Arrays
{
   public class RemoveDuplicates
    {
        public int Remove(int[] nums)
        {
            int j = 0;
            for (int i = 1; i < nums.Length; i++)
            {
                if (nums[j] < nums[i])
                {
                    nums[++j] = nums[i];
                }
            }
            return Math.Min(nums.Length, ++j);
        }
        public int RemoveWithDuplicates(int[] nums)
        {
            int j = -1;
            for (int i = 0; i < nums.Length; i++)
            {
                int temp = i;
                while (i + 1 < nums.Length && nums[i] == nums[i + 1])
                    i++;

                nums[++j] = nums[i];
                if (i - temp > 1)
                    nums[++j] = nums[i];


            }
            return Math.Min(nums.Length, ++j);
        }
    }
}
