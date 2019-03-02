using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDsAndAlgos.DataStructures.Heaps;

namespace Samples
{
    public class NewGraph
    {
        public Dictionary<int, List<Edge>> adjacencyList = new Dictionary<int, List<Edge>>();

        public void AddEdge(int start, int end, int weight)
        {
            if (adjacencyList.ContainsKey(start))
                adjacencyList[start].Add(new Edge() { Start = start,End = end, Weight = weight });
            else
                adjacencyList.Add(start, new List<Edge>() { new Edge() { Start = start, End = end, Weight = weight } });
        }
        public List<Edge> GetAdjacentEdges(int vertex)
        {
            return adjacencyList[vertex];
        }
    }
    public class Edge : IComparable<Edge>
    {
        public int Start { get; set; }
        public int End { get; set; }
        public int Weight { get; set; }
        public int GetEither()
        {
            return Start;
        }
        public int GetOther(int v)
        {
            if (Start == v)
                return End;
            return Start;
        }
        public int CompareTo(Edge that)
        {
            return this.Weight.CompareTo(that.Weight);
        }
    }
    public class KeyNode : IComparable<KeyNode>
    {
       public int Weight { get; set; }
       public int Vertex { get; set; }
       public int CompareTo(KeyNode that)
        {
            return Weight.CompareTo(that.Weight);
        }
       public override int GetHashCode()
        {
            return Vertex.GetHashCode();
        }
    }
    public class NetworkWires
    {
        public int GetMinLenWire(int n, int[][] wires)
        {
            if (wires.Length == 0) return 0;
            int length = 0;
            NewGraph graph = new NewGraph();

            for (int i = 0; i < wires.Length; i++)
            {
                graph.AddEdge(wires[i][0], wires[i][1], wires[i][2]);
                graph.AddEdge(wires[i][1], wires[i][0], wires[i][2]);
            }
            IndexedMinHeap<int> vtod = new IndexedMinHeap<int>();           
            foreach (int i in graph.adjacencyList.Keys)
            {               
                vtod.Insert(i,Int32.MaxValue);
            }

            vtod.DecreaseKey(0, 0);

            while (vtod.Size() > 0)
            {
                KeyValuePair<int,int> min = vtod.DeleteMin();
                if (min.Value == Int32.MaxValue)
                {
                    vtod.Insert(min.Key, 0);
                    continue;
                }
                length += min.Value;

                foreach (Edge adj in graph.GetAdjacentEdges(min.Key))
                {
                    int v = adj.GetOther(min.Key);
                    if (vtod.ContainsKey(v))
                    {
                        if(adj.Weight < vtod.GetWeight(v))
                        {
                            vtod.DecreaseKey(v, adj.Weight);
                        }
                    }
                }
            }
            return length;
        }
    }
}
