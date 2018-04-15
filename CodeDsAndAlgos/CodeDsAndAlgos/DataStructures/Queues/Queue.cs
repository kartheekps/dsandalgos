using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures
{
    public class Queue<T>
    {
        private const int CAPACITY = 10;
        private T[] data = new T[CAPACITY];
        private int count = 0;
        private int front = -1;
        private int tail = -1;
        public int Count { get { return count; } }

        public bool IsEmpty()
        {
            return Count == 0;
        }

        public bool IsFull()
        {
            return Count == CAPACITY;
        }

        public void Enqueue(T item)
        {
            if (IsFull())           
                throw new Exception("Queue is Full");

            tail = tail + 1 % CAPACITY;
            data[tail] = item;

            if (front == -1) front = tail;

            count++;
        }

        public T Dequeue()
        {
            if (IsEmpty())
                throw new Exception("Queue is Empty");

            T item = data[front];
            //avoid loitering
            data[front] = default(T);

            if (front == tail) { front = -1; tail = -1; }
            else front = front + 1 % CAPACITY;

            count--;

            return item;
        }

    }
}
