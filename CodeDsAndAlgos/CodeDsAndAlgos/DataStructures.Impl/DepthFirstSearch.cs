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

        public DepthFirstSearch(Graph<T> _graph,int _sourceVertexId)
        {
            graph = _graph;
            visited = new HashSet<int>();
            parent = new Dictionary<int, int>(graph.GetVertices().Count());
            sourceVertexId = _sourceVertexId;
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
        }
    }
}
