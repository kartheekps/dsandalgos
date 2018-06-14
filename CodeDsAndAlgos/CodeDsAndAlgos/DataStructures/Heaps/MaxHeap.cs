using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures.Heaps
{
    public class MaxHeap<T> where T : IComparable<T>
    {
        int N = -1;
        T[] data;

        public MaxHeap(T[] _data, int size)
        {
            data = _data;
            N = size - 1;
        }

        public MaxHeap() : this(new T[10], 0)
        { }
        public MaxHeap(int size) : this(new T[size], 0)
        { }
        public int Count()
        {
            return N + 1;
        }
        public T GetMax()
        {
            return data[0];
        }
        public T DeleteMax()
        {
            T item = GetMax();
            data[0] = data[N];
            data[N--] = default(T);
            Sink(0);
            return item;
        }
        public void Insert(T item)
        {
            data[++N] = item;
            Swim(N);
        }

        public void Heapify()
        {
            for (int i = (N - 1) / 2; i >= 0; i--)
                Sink(i);
        }

        private void Swim(int k)
        {
            while (k > 0 && data[(k - 1) / 2].CompareTo(data[k]) < 0)
            {
                int j = (k - 1) / 2;
                T temp = data[k];
                data[k] = data[j];
                data[j] = temp;
                k = j;
            }
        }

        private void Sink(int k)
        {
            while (2 * k + 1 <= N)
            {
                int j = 2 * k + 1;
                if (j + 1 <= N) j = data[j].CompareTo(data[j + 1]) > 0 ? j : j + 1;
                if (data[k].CompareTo(data[j]) > 0) break;
                T temp = data[k];
                data[k] = data[j];
                data[j] = temp;
                k = j;
            }
        }
    }
}
