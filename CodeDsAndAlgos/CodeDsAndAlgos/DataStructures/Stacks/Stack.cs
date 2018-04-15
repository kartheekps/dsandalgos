using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures
{
   public class Stack<T>
    {
       private int top = -1;       
       private const int capacity = 10;
       private T[] data = new T[capacity];

        public void Push(T item)
        {
            if (Count() == capacity)
                throw new Exception("Stack is Full");

            data[++top] = item;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new Exception("Stack is Empty");

            return data[top];
        }

        public T Pop()
        {
            if (IsEmpty())
                throw new Exception("Stack is Empty");

            T item = data[top];

            data[top--] = default(T);

            return item;
        }

        public bool IsEmpty()
        {
            return Count() == 0;
        }

        public int Count()
        {
            return top + 1;
        }
    }
}
