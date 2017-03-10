using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDsAndAlgos.DataStructures;

namespace CodeDsAndAlgos
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Graphs
            Graph<int> graph = new Graph<int>(false);
            graph.AddEdge(0, 15, 1, 15);
            graph.AddEdge(1, 15, 2, 15);
            graph.AddEdge(2, 15, 3, 15);
            graph.AddEdge(3, 15, 1, 15);
            #endregion Graphs       
        }
    }
}
