using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.Algorithms
{
    public class TreeNode
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int MaxEnd { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public TreeNode(int _start, int _end)
        {
            Start = _start;
            End = _end;
            MaxEnd = _end;
        }
    }
    public class MyCalendarTwo
    {
        private ISTree treeEvents;
        public MyCalendarTwo()
        {
            treeEvents = new ISTree();
        }

        public bool Book(int start, int end)
        {
            TreeNode booking = new TreeNode(start, end);
            if (treeEvents.Intersect(booking))
                return false;
            treeEvents.Insert(booking);
            return true;
        }
    }

    public class ISTree
    {
        private TreeNode root;

        public void Insert(TreeNode e1)
        {
            root = this.Insert(root, e1);
        }

        private TreeNode Insert(TreeNode _root, TreeNode e1)
        {
            if (_root == null)
                return e1;

            if ( e1.Start < _root.Start)
            {
                _root.Left = Insert(_root.Left, e1);
                _root.MaxEnd = Math.Max(_root.MaxEnd, _root.Left.MaxEnd);
            }
            else
            {
                _root.Right = Insert(_root.Right, e1);
                _root.MaxEnd = Math.Max(_root.MaxEnd, _root.Right.MaxEnd);
            }
            return _root;
        }

        public bool Intersect(TreeNode e1)
        {
            if (root == null) return false;
            TreeNode current = root;
            while (current != null)
            {
                if (e1.End > current.Start && e1.Start < current.End)
                    return true;
                else if (current.Left == null)
                    current = current.Right;
                else if (current.Left.MaxEnd < e1.Start)
                    current = current.Right;
                else
                    current = current.Left;
            }
            return false;
        }
    }



    /**
     * Your MyCalendarTwo object will be instantiated and called as such:
     * MyCalendarTwo obj = new MyCalendarTwo();
     * bool param_1 = obj.Book(start,end);
     */
}
