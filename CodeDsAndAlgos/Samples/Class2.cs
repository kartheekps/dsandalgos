using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    public class State
    {
        public int Vertex { get; set; }
    }

    public class GraphBFS
    {
        public int Search(Graph graph, int start, State state)
        {
            int count = 0;
            int v = start;
            HashSet<int> vset = new HashSet<int>();
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                int temp = queue.Count;
                while (temp > 0)
                {
                    v = queue.Dequeue();
                    state.Vertex = v;
                    vset.Add(v);
                    foreach (int adjVertex in graph.GetAdjacentVertices(v))
                    {
                        if (!vset.Contains(adjVertex))
                            queue.Enqueue(adjVertex);
                    }
                    temp--;
                }
                if(queue.Count > 0)                
                    count++;          
            }
            
            return count;
        }
    }

    public class Graph
    {
        public Dictionary<int, List<int>> adjacencyList = new Dictionary<int, List<int>>();

        public void AddEdge(int start, int end)
        {
            if (adjacencyList.ContainsKey(start))
                adjacencyList[start].Add(end);
            else
                adjacencyList.Add(start, new List<int>() {end});
        }
        public List<int> GetAdjacentVertices(int vertex)
        {
            return adjacencyList[vertex];
        }
    }

   public class Class2
    {
        public int treeDiameter(int n, int[][] tree)
        {
            Graph graph = new Graph();
            GraphBFS Bfs = new GraphBFS();
            for (int i = 0; i < tree.Length; i++)
            {
                graph.AddEdge(tree[i][0], tree[i][1]);
                graph.AddEdge(tree[i][1], tree[i][0]);
            }
            State state = new State();
            int d1 = Bfs.Search(graph, 0, state);
            int d2 = Bfs.Search(graph, state.Vertex, state);
            return Math.Max(d1, d2);
        }
    }
}
