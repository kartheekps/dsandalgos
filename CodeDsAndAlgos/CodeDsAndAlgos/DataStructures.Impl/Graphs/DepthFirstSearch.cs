using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures.Impl
{
    public class DepthFirstSearch<T>
    {
        private HashSet<int> visited;
        private Dictionary<int,int> parent;
        private Graph<T> graph;
        private int sourceVertexId;
        private System.Collections.Generic.Stack<T> reverseOrder;

        public DepthFirstSearch(Graph<T> _graph,int _sourceVertexId)
        {
            graph = _graph;
            visited = new HashSet<int>();
            parent = new Dictionary<int, int>(graph.GetVertices().Count());
            sourceVertexId = _sourceVertexId;
            reverseOrder = new System.Collections.Generic.Stack<T>(graph.GetVertices().Count());
        }

        public void Search()
        {
            visited.Clear();
            foreach(Vertex<T> vertex in graph.GetVertices())
            {
                if (!visited.Contains(vertex.Id))
                {
                    parent.Add(vertex.Id, -1);                                      
                    DFSUtil(vertex);
                }
            }
        }

        private void DFSUtil(Vertex<T> _vertex)
        {
            visited.Add(_vertex.Id);
            foreach (Vertex<T> adjVertex in _vertex.GetAdjacencyList())
            {
                if(!visited.Contains(adjVertex.Id))
                {
                    parent.Add(adjVertex.Id, _vertex.Id);
                    DFSUtil(adjVertex);
                }
            }
            reverseOrder.Push(_vertex.Data);
        }

        public void IterativeSearch(Vertex<T> _vertex)
        {
            visited.Clear();

            Stack<Vertex<T>> stack = new Stack<Vertex<T>>();
            stack.Push(_vertex);
            
            while (stack.Count() > 0)
            {
                Vertex<T> top = stack.Pop();
                visited.Add(top.Id);

                foreach (Vertex<T> adjVertex in top.GetAdjacencyList())
                {
                    if(!visited.Contains(adjVertex.Id))
                        stack.Push(adjVertex);
                }
            }
        }

        public IEnumerable<T> GetTopologicalOrder()
        {
            if (reverseOrder.Count() == 0)
            {
                this.Search();
            }                
            return reverseOrder;
        }
    }
}
