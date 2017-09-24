using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeDsAndAlgos.DataStructures;

namespace CodeDsAndAlgos.Tests
{
    [TestClass]
    public class GraphsTests
    {
       
        [TestMethod]
        public void Graph_AddEdge_Count_ShouldMatch_EdgesCount()
        {
            //assemble
            Graph<int> graph = new Graph<int>(false);

            //act
            graph.AddEdge(0, 0, 1, 1);
            graph.AddEdge(1, 1, 2, 2);
            graph.AddEdge(2, 2, 3, 3);
            graph.AddEdge(3, 3, 1, 1);

            //assert
            Assert.IsTrue(graph.Edges.Count == 4);
        }
    }
}
