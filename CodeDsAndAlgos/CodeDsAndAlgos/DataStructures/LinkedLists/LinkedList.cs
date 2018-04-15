using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LinkedList<T>
    {
        public LinkedListNode<T> Head { get; set; }
        private int count = 0;
        public int Count { get { return count; } private set { count = value; } }

        public LinkedList()
        {           
        }
        public LinkedList(LinkedListNode<T> _head)
        {          
            Head = _head;
            _head.List = this;
        }
        public LinkedList(T _data)
        {
            LinkedListNode<T> node = 
                new LinkedListNode<T>(_data);
            node.List = this;          
            this.Head = node;
        }

        public void AddLast(T _data)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(_data);
            if (Head == null)
                InternalInsertNodeToEmptyList(node);
            else
                InternalInsertNodeBefore(node, Head);
            node.List = this;

        }

        public LinkedListNode<T> FindLast()
        {
            LinkedListNode<T> current = Head;
            while (current.Next != Head)
                current = current.Next;
            return current;
        }
        
        public void AddFirst(LinkedListNode<T> node)
        {
            if (Head == null)
                InternalInsertNodeToEmptyList(node);
            else
            {
                InternalInsertNodeBefore(node, Head);
                Head = node;
            }
            node.List = this;
        }

        private void InternalInsertNodeBefore(LinkedListNode<T> newnode, 
            LinkedListNode<T> node)
        {
            newnode.Previous = node.Previous;
            newnode.Next = node;
            node.Previous.Next = newnode;
            node.Previous = newnode;
            count++;
        }

        private void InternalInsertNodeToEmptyList(LinkedListNode<T> node)
        {            
            node.Next = node;
            node.Previous = node;           
            Head = node;
            count++;
        }

        public LinkedListNode<T> Find(T _data)
        {
            if (Head.Value.Equals(_data))
                return Head;

            LinkedListNode<T> current = Head.Next;

            while(current != Head)
            {
                if(current.Value.Equals(_data))
                {
                    return current;
                }
                current = current.Next;
            }
            return null;
        }

        public void Remove(T _data)
        {
            LinkedListNode<T> node = this.Find(_data);
            InternalRemoveNode(node);
        }

        private void InternalRemoveNode(LinkedListNode<T> node)
        {
            if (node.Next == node)
                Head = null;            
            else
            {
                if (Head == node)
                    Head = node.Next;
                node.Next.Previous = node.Previous;
                node.Previous.Next = node.Next;
            }            
        }
    }
}
