using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeDsAndAlgos.DataStructures.Impl
{
   public class BreadthFirstSearch<T>
    {
        private HashSet<int> visited;
        private Graph<T> graph;
        private int sourceVertexId;

        public BreadthFirstSearch(Graph<T> _graph, int _sourceVertexId)
        {
            visited = new HashSet<int>();
            graph = _graph;
            sourceVertexId = _sourceVertexId;
        }
        public BreadthFirstSearch(Graph<T> _graph, Vertex<T> _sourceVertex)
            : this(_graph,_sourceVertex.Id)
        {        
        }

        public void Search()
        {
            Vertex<T> sourceVertex = graph.GetVertex(sourceVertexId);
            if (sourceVertex == null)
                return;

            System.Collections.Generic.Queue<Vertex<T>> queue = 
                new System.Collections.Generic.Queue<Vertex<T>>(graph.GetVertices().Count()); 
            
            queue.Enqueue(sourceVertex);
            visited.Add(sourceVertex.Id);

            while (queue.Count != 0)
            {
                Vertex<T> front = queue.Dequeue();
                
                Console.WriteLine("Depth First Search Visited Vertex with Id = " + front.Id 
                    +":"+ front.Data.ToString());

                foreach (Vertex<T> adjVertex in front.GetAdjacencyList())
                {
                    if (!visited.Add(adjVertex.Id))
                    {
                        visited.Add(adjVertex.Id);
                        queue.Enqueue(adjVertex);
                    }                        
                }
            }
        }
    }
}
