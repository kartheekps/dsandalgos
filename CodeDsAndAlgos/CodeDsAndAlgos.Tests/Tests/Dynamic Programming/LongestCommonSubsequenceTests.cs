using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDsAndAlgos.Algorithms.DynamicProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CodeDsAndAlgos.Tests.Tests.DynamicProgramming
{
    [TestClass]
    public class LongestCommonSubsequenceTests
    {
        [TestMethod]
        public void LCSRecrusive_ShouldPass()
        {
            LongestCommonSubsequence objLcs = new LongestCommonSubsequence();
            int lcs = objLcs.GetLCSNR("AGGTAB", "GXTXAYB", "AGGTAB".Length-1,
                "GXTXAYB".Length-1);
            Assert.IsTrue(lcs == 4);
        }
        [TestMethod]
        public void LCSMemoizedRecrusive_ShouldPass()
        {
            LongestCommonSubsequence objLcs = new LongestCommonSubsequence();
            int lcs = objLcs.GetLcsMr("AGGTAB", "GXTXAYB", "AGGTAB".Length,
                "GXTXAYB".Length);
            Assert.IsTrue(lcs == 4);
        }
    }
}
