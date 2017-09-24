using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDsAndAlgos.DataStructures;

namespace CodeDsAndAlgos.Tests
{
    [TestClass]
    public class LinkedListsTests
    {

        [TestMethod]
        public void LinkedList_Loops_HasLoop_ShouldReturnTrue()
        {
            //assemble
            DataStructures.LinkedList<int> list =
                new DataStructures.LinkedList<int>(1);
            list.AddLast(2);
            list.AddLast(3);
            list.AddLast(4);
            list.AddLast(5);
            list.AddLast(6);
            DataStructures.LinkedListNode<int> last = list.FindLast();
            last.Next = list.Find(4);

            //Act
            DataStructures.Impl.Loops loops = new DataStructures.Impl.Loops();
     
            //assert
            Assert.IsTrue(loops.HasLoop<int>(list));
        }
    }
}
