using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    class Tree<T>
    {
        public T value { get; set; }
        public Tree<T> left { get; set; }
        public Tree<T> right { get; set; }
    }
    class MFS
    {
        public void GetMostFrequentSum()
        {
            Tree<int> root = new Tree<int>();
            root.value = -2;
            root.left = new Tree<int>() { value = -3 };
            root.right = new Tree<int>() { value = 2 };
            mostFrequentSum(root);
        }
        int[] mostFrequentSum(Tree<int> t)
        {
            if (t == null)
                return null;
            Dictionary<int, int> map = new Dictionary<int, int>();
            SortedDictionary<int, SortedSet<int>> map2 = new SortedDictionary<int, SortedSet<int>>();
            mostFrequentSum(t, map, map2);
            return map2[map2.Keys.Max()].ToArray(); ;
        }

        int mostFrequentSum(Tree<int> t, Dictionary<int, int> map,
                            SortedDictionary<int, SortedSet<int>> map2)
        {
            if (t == null) return 0;
            int left = mostFrequentSum(t.left, map, map2);
            int right = mostFrequentSum(t.right, map, map2);
            int sum = left + right + t.value;
            if (map.ContainsKey(sum))
                map[sum]++;
            else
                map.Add(sum, 1);

            if (map2.ContainsKey(map[sum] - 1))
                map2[map[sum] - 1].Remove(sum);
            if (map2.ContainsKey(map[sum]))
                map2[map[sum]].Add(sum);
            else
                map2.Add(map[sum], new SortedSet<int>() { sum });

            return sum;
        }
    }
}
