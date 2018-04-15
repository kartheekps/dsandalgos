//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CodeDsAndAlgos.DataStructures.Impl.LinkedLists
//{
//    public class Utils<T>
//    {
//        public static LinkedList<T> Clone(LinkedList<T> list)
//        {
//            if (list == null || list.Head == null)
//                return list;

//            LinkedList<T> newList = new LinkedList<T>();

//            LinkedListNode<T> current = list.Head;

//            //Create ClonedList
//            while(current.Next != list.Head)
//            {
//                LinkedListNode<T> clonedNode = new LinkedListNode<T>(current.Value);
//                clonedNode.Next = current.Random;
//                current.Random = clonedNode;
//                current = current.Next;
//            }

//            //Setting up head of the new list
//            newList.Head = list.Head.Random;

//            current = list.Head;
//            //Setup random node
//            while(current.Next != list.Head)
//            {
//                LinkedListNode<T> clonedNode = current.Random;
//                clonedNode.Random = clonedNode.Next.Random;
//                current = current.Next;
//            }

//            current = list.Head;
//            //Restore pointers
//            while(current.Next != list.Head)
//            {
//                LinkedListNode<T> clonedNode = current.Random;
//                LinkedListNode<T> temp = clonedNode.Next;
//                clonedNode.Next = current.Next.Random;
//                current.Random = temp;
//                current = current.Next;
//            }

//            return newList;
//        }
//    }
//}


/**
 * Definition for singly-linked list with a random pointer.
 * public class RandomListNode {
 *     public int label;
 *     public RandomListNode next, random;
 *     public RandomListNode(int x) { this.label = x; }
 * };
 */

public class RandomListNode
{
     public int label;
     public RandomListNode next, random;
     public RandomListNode(int x) { this.label = x; }
 };
public class Solution
{
    public RandomListNode CopyRandomList(RandomListNode head)
    {
        if (head == null)
            return head;

        RandomListNode current = head;

        // Create Cloned Nodes and Point random pointers to cloned nodes
        while(current != null)
        {
            RandomListNode cnode = new RandomListNode(current.label);
            cnode.next = current.random;
            current.random = cnode;
            current = current.next;
        }

        current = head;
        RandomListNode chead = current.random;
        //Set random pointers of cloned nodes;
        while (current != null)
        {
            RandomListNode cnode = current.random;
            cnode.random = cnode.next.random;           
            current = current.next;
        }

        current = head;

        //restore pointers
        while(current != null)
        {
            RandomListNode cnode = current.random;
            current.random = cnode.next;
            if (current.next != null)
                cnode.next = current.next.random;
            current = current.next;
        }
        return chead;
    }

    public RandomListNode CopyRandomList1(RandomListNode head)
    {
        if (head == null)
            return head;

        if (head.next == null)
            return new RandomListNode(head.label);

        RandomListNode current = head;

        // Create Cloned Nodes and Point random pointers to cloned nodes
        while (current != null)
        {
            RandomListNode cnode = new RandomListNode(current.label);
            cnode.next = current.random;
            current.random = cnode;
            current = current.next;
        }

        current = head;
        RandomListNode chead = current.random;
        //Set random pointers of cloned nodes;
        while (current != null)
        {
            RandomListNode cnode = current.random;
            cnode.random = cnode.next.random;
            current = current.next;
        }

        current = head;

        //restore pointers
        while (current != null)
        {
            RandomListNode cnode = current.random;
            current.random = cnode.next;
            if (current.next != null)
                cnode.next = current.next.random;
            current = current.next;
        }
        return chead;
    }
}

