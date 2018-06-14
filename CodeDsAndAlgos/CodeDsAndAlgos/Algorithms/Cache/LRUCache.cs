using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms.Cache
{
    public class CacheNode
    {
        public int Key;
        public int Value;
    }

    public class LRUCache
    {
        private Dictionary<int, LinkedListNode<CacheNode>> cacheLookup;
        private LinkedList<CacheNode> cacheList;
        private int capacity;
        public LRUCache(int _capacity)
        {
            cacheLookup = new Dictionary<int, LinkedListNode<CacheNode>>(capacity);
            cacheList = new LinkedList<CacheNode>();
            capacity = _capacity;
        }

        public int Get(int key)
        {
            if (!cacheLookup.ContainsKey(key))
                return -1;

            LinkedListNode<CacheNode>  cacheNode = cacheLookup[key];
            if (cacheList.First != cacheNode)
            {
                cacheList.Remove(cacheNode);
                cacheList.AddFirst(cacheNode);
            }
            return cacheNode.Value.Value;
        }

        public void Put(int key, int value)
        {
            if (!cacheLookup.ContainsKey(key))
            {
                if (cacheLookup.Count() == capacity)
                {
                    LinkedListNode<CacheNode> cacheNode = cacheList.Last;
                    cacheList.RemoveLast();
                    cacheLookup.Remove(cacheNode.Value.Key);
                }
                LinkedListNode<CacheNode> node = 
                    new LinkedListNode<CacheNode>(new CacheNode() { Key = key, Value = value });
                cacheLookup.Add(key, node);
                cacheList.AddFirst(node);
            }
        }
    }
}
