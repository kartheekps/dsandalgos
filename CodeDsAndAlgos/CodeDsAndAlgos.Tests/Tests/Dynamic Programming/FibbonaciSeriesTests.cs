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
    public class FibbonaciSeriesTests
    {
        [TestMethod]
        public void DP_FibbonaciSeries_5_ShouldMatch_8()
        {
            //assemble
            FibbonaciSeries series = new FibbonaciSeries();
            //act
            int result = series.GetValueIterative(5);
            //assert
            Assert.IsTrue(result == 8);
        }
    }
}
