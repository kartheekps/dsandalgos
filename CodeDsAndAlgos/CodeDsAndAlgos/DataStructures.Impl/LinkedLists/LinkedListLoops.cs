using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures.Impl
{
   public class Loops
    {
		public bool HasLoop<T>(LinkedList<T> _head)
        {
            LinkedListNode<T> slow = _head.Head;
            LinkedListNode<T> fast = _head.Head.Next;
            while(fast != _head.Head && fast.Next != _head.Head)
            {
                if (slow == fast)
                    return true;
                slow = slow.Next;
                fast = fast.Next.Next;
            }
            return false;
        }
    }
}
