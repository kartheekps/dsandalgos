using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeDsAndAlgos.Algorithms.DynamicProgramming;

namespace CodeDsAndAlgos.Tests.Tests.DynamicProgramming
{
    [TestClass]
    public  class RodCuttingTests
    {
        [TestMethod]
        public void DP_RodCutting_RodOfLength4_ShouldMatch_10()
        {
            //assemble
            RodCutting rods = new RodCutting();
            //act
            int result = rods.GetMaxBenefit(new int[] {0, 1, 5, 8, 9 }, 4,0);
            //assert
            Assert.IsTrue(result == 10);
        }
        [TestMethod]
        public void DP_RodCutting_RodOfLength7_ShouldMatch_16_WithCutCost1()
        {
            //assemble
            RodCutting rods = new RodCutting();
            //act
            int result = rods.GetMaxBenefit(new int[] { 0, 1, 6, 8, 9, 11, 12, 13 }, 7, 1);
            //assert
            Assert.IsTrue(result == 18);
        }
    }
}
