using System;
using System.Collections.Generic;


namespace CodeDsAndAlgos.DataStructures
{
   public class LinkedListNode<T>
    {
        public LinkedListNode<T> Next { get; set; }
        public T Value { get; set; }
        public LinkedList<T> List { get; internal set; }
        public LinkedListNode<T> Previous { get; set; }

        public LinkedListNode(T _data)
        {
            Value = _data;
            Next = this;
            Previous = this;
        }
    }
}
