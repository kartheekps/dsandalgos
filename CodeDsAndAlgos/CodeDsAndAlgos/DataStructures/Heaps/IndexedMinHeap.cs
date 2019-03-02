using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures.Heaps
{
    public class IndexedMinHeap<TKey> where TKey : IComparable<TKey>
    {
        public class Node : IComparable<Node>
        {
            public int weight;
            public TKey key;     
            public int CompareTo(Node that)
            {
                return this.weight.CompareTo(that.weight);
            }
        }
        Node[] pq = new Node[10];     
        Dictionary<TKey, int> qp = new Dictionary<TKey, int>();
        int N = -1;
        public int Size()
        {
            return N + 1;
        }
        public TKey GetMinKey()
        {
            return pq[0].key;
        }
        public int GetMinValue()
        {
            return pq[0].weight;
        }
        private void Sink(int k)
        {
            while (2 * k + 1 <= N)
            {
                int j = 2 * k + 1;
                if (j + 1 <= N) j = pq[j].CompareTo(pq[j + 1]) < 0 ? j : j + 1;
                if (pq[k].CompareTo(pq[j]) < 0) break;
                qp[pq[k].key] = j;
                qp[pq[j].key] = k;
                Node temp = pq[k];
                pq[k] = pq[j];
                pq[j] = temp;
                k = j;
            }
        }
        private void Swim(int k)
        {
            while (k > 0 && pq[(k - 1) / 2].CompareTo(pq[k]) > 0)
            {
                int j = (k - 1) / 2;
                qp[pq[k].key] = j;
                qp[pq[j].key] = k;
                Node temp = pq[k];
                pq[k] = pq[j];
                pq[j] = temp;
                k = j;
            }
        }
        public void Heapify()
        {
            for (int i = (N - 1) / 2; i >= 0; i--)
                Sink(i);
        }
        public void Insert(TKey item,int value)
        {
            pq[++N] = new Node() { key = item, weight = value }; 
            qp.Add(item, N);
            Swim(N);
        }
        public KeyValuePair<TKey,int> DeleteMin()
        {
            TKey key = GetMinKey();
            int item = GetMinValue();
            qp.Remove(pq[0].key);
            pq[0] = pq[N];          
            qp[pq[0].key] = 0;
            pq[N] = default(Node);           
            N--;
            Sink(0);
            return new KeyValuePair<TKey, int>(key, item);
        }
        public void DecreaseKey(TKey okey,int newWeight)
        {
            if(pq[qp[okey]].weight.CompareTo(newWeight) > 0)
            {
                int index = qp[okey];
                pq[qp[okey]].weight = newWeight;
                Swim(index);
            }
        }
        public bool ContainsKey(TKey key)
        {
            return qp.ContainsKey(key);
        }
        public int GetWeight(TKey key)
        {
            if (ContainsKey(key))
            {
                return pq[qp[key]].weight;
            }
            return Int32.MaxValue;
        }
    }
}
