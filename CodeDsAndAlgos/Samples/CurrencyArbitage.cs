using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    public class CurrencyArbitage
    {
       public static bool currencyArbitrage1(double[][] exchange)
        {
            Graph1 g = new Graph1();
            for (int i = 0; i < exchange.Length; i++)
            {
                for (int j = 0; j < exchange[i].Length; j++)
                {
                    if (i == j) continue;
                    g.AddEdge(i, j, -Math.Log(exchange[i][j]));
                }
            }
            BellmanFordSP spt = new BellmanFordSP(g, 0);
            if (spt.HasNegativeCycle) return true;
            return false;
        }
    }
    public class Graph1
    {
        public Dictionary<int, List<Edge1>> adjacencyList = new Dictionary<int, List<Edge1>>();

        public void AddEdge(int start, int end, double weight)
        {
            if (adjacencyList.ContainsKey(start))
                adjacencyList[start].Add(new Edge1() { Start = start, End = end, Weight = weight });
            else
                adjacencyList.Add(start, new List<Edge1>() { new Edge1() { Start = start, End = end, Weight = weight } });
        }
        public void AddEdge(Edge1 e)
        {
            if (adjacencyList.ContainsKey(e.Start))
                adjacencyList[e.Start].Add(e);
            else
                adjacencyList.Add(e.Start, new List<Edge1>() { e });
        }
        public List<Edge1> GetAdjacentEdges(int vertex)
        {
            if(adjacencyList.ContainsKey(vertex))
                return adjacencyList[vertex];
            return new List<Edge1>();
        }
    }
    public class Edge1 : IComparable<Edge1>
    {
        public int Start { get; set; }
        public int End { get; set; }
        public double Weight { get; set; }
        public double GetEither()
        {
            return Start;
        }
        public double GetOther(int v)
        {
            if (Start == v)
                return End;
            return Start;
        }
        public int CompareTo(Edge1 that)
        {
            return this.Weight.CompareTo(that.Weight);
        }
    }
    public class BellmanFordSP
    {
        private int cost = 0;
        private double[] distTo;
        private Edge1[] edgeTo;
        Queue<int> queue = new Queue<int>();
        public bool HasNegativeCycle { get; set; }
        private bool[] onQ;
        private Graph1 g;
        public BellmanFordSP(Graph1 _g, int s)
        {
            g = _g;
            distTo = new double[g.adjacencyList.Keys.Count];
            edgeTo = new Edge1[g.adjacencyList.Keys.Count];
            onQ = new bool[g.adjacencyList.Keys.Count];
            for (int i = 0; i < distTo.Length; i++)
                distTo[i] = Double.MaxValue;
            distTo[s] = 0;
            queue.Enqueue(s);
            onQ[s] = true;
            while (queue.Count > 0 && !HasNegativeCycle)
            {
                int u = queue.Dequeue();
                onQ[u] = false;
                relax(u);
            }
        }
        private void relax(int u)
        {
            foreach (Edge1 edge in g.GetAdjacentEdges(u))
            {
                int v = edge.End;
                if (distTo[u] + edge.Weight < distTo[v])
                {
                    distTo[v] = distTo[u] + edge.Weight;
                    edgeTo[v] = edge;
                    if (!onQ[v])
                    {
                        queue.Enqueue(v);
                        onQ[v] = true;
                    }
                }
                if (cost++ % g.adjacencyList.Keys.Count == 0 && queue.Count > 0)
                {                  
                    Graph1 subg = new Graph1();
                    foreach (Edge1 e in edgeTo)
                    {
                        if (e != null)
                            subg.AddEdge(e);
                    }
                    DiGraphCycle gcycle = new DiGraphCycle(subg);
                    if (gcycle.HasCycle)
                        HasNegativeCycle = true;
                }
            }
        }
    }
    public class DiGraphCycle
    {
        public bool HasCycle { get; private set; }
        private Graph1 graph;
        public DiGraphCycle(Graph1 g)
        {
            graph = g;
            HashSet<int> map = new HashSet<int>();
            HashSet<int> map2 = new HashSet<int>();
            foreach (int key in graph.adjacencyList.Keys)
                if (!map.Contains(key))
                    DFSSearch(key, map, map2);
        }
        private void DFSSearch(int u, HashSet<int> map, HashSet<int> map2)
        {
            map.Add(u);
            map2.Add(u);
            foreach (Edge1 v in graph.GetAdjacentEdges(u))
            {
                if (HasCycle) break;
                if (!map2.Contains(v.End))
                    DFSSearch(v.End, map, map2);
                else
                    HasCycle = true;
            }
            map2.Remove(u);
        }
    }
}
