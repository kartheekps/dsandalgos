using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures.Impl
{
   public class BreadthFirstSearch<T>
    {
        private bool[] visited;
        private Graph<T> graph;
        private int sourceVertexId;

        public BreadthFirstSearch(Graph<T> _graph, int _sourceVertexId)
        {
            visited = new bool[_graph.GetVertices().Count()];
            graph = _graph;
            sourceVertexId = _sourceVertexId;
        }

        public void Search()
        {
            Vertex<T> sourceVertex = graph.GetVertex(sourceVertexId);
            if (sourceVertex == null)
                return;
            for (int i = 0; i < visited.Count(); i++)
                visited[i] = false;

            Queue<Vertex<T>> queue = new Queue<Vertex<T>>(graph.GetVertices().Count());            
            queue.Enqueue(sourceVertex);
            visited[sourceVertex.Id] = true;

            while (queue.Count != 0)
            {
                Vertex<T> front = queue.Dequeue();
                
                Console.WriteLine("Depth First Search Visited Vertex with Id = " + front.Id 
                    +":"+ front.Data.ToString());

                foreach (Vertex<T> adjVertex in front.GetAdjacencyList())
                {
                    if (!visited[adjVertex.Id])
                    {
                        visited[adjVertex.Id] = true;
                        queue.Enqueue(adjVertex);
                    }                        
                }
            }

        }

    }
}
