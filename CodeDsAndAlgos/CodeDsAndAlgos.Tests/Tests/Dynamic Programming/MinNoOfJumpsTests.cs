using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDsAndAlgos.Algorithms.DynamicProgramming;

namespace CodeDsAndAlgos.Tests.Tests.Dynamic_Programming
{
    [TestClass]
    public class MinNoOfJumpsTests
    {
        [TestMethod]
        public void DP_MinNoOfJumps_GetMinNR_10_ShouldMatch_3()
        {
            //assemble
            MinNoOfJumps jumps = new MinNoOfJumps();
            //act
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            int result = jumps.GetMinNR(new int[] { 1,3,5,8,9,2,6,7,6,8,9 }, 10);
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            //assert
            Assert.IsTrue(result == 3);
        }


        [TestMethod]
        public void DP_MinNoOfJumps_GetMinTDMR_10_ShouldMatch_3()
        {
            //assemble
            MinNoOfJumps jumps = new MinNoOfJumps();
            //act
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            int result = jumps.GetMinTDMR(new int[] { 1, 3, 5, 8, 9, 2, 6, 7, 6, 8, 9 }, 10);
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            //assert
            Assert.IsTrue(result == 3);
        }

        [TestMethod]
        public void DP_MinNoOfJumps_GetMinBUM_10_ShouldMatch_3()
        {
            //assemble
            MinNoOfJumps jumps = new MinNoOfJumps();
            //act
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            int result = jumps.GetMinBUM(new int[] { 1, 3, 5, 8, 9, 2, 6, 7, 6, 8, 9 }, 10);
            Console.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            //assert
            Assert.IsTrue(result == 3);
        }
    }

}
