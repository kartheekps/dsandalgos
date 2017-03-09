using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures
{
    public class Graph<T>
    {
        public bool IsDirected { get; set; }
        public List<Edge<T>> Edges { get; set; }
        private Dictionary<int,Vertex<T>> Vertices { get; set; }

        public Graph(bool _isDirected)
        {
            IsDirected = _isDirected;
        }

        public void AddEdge(int _id1, T _data1, int _id2, T _data2)
        {
            this.AddEdge(_id1, _data1, _id2, _data2, 0);
        }
        
        public void AddEdge(int _id1,T _data1, int _id2, T _data2,int _weight)
        {
            Vertex<T> start,end ;

            if (Vertices.ContainsKey(_id1))
                Vertices.TryGetValue(_id1, out start);
            else
            {
                start = new Vertex<T>(_id1, _data1);
                Vertices.Add(_id1, start);
            }

            if (Vertices.ContainsKey(_id2))
                Vertices.TryGetValue(_id2, out end);
            else
            {
                end = new Vertex<T>(_id2, _data2);
                Vertices.Add(_id2, end);
            }

            Edge<T> edge = new Edge<T>(start, end, _weight);
            Edges.Add(edge);
            start.AddAdjancentVertex(end, edge);
            if (!IsDirected)
                end.AddAdjancentVertex(start, edge);
        }

        public IEnumerable<Vertex<T>> GetVertices()
        {
            return Vertices.Values;
        }

        public Vertex<T> GetVertex(int _id)
        {
            Vertex<T> vertex = null;
            Vertices.TryGetValue(_id, out vertex);
            return vertex;
        }
    }

    public class Edge<T> : IComparable<Edge<T>>
    {
        public  int Weight { get; set; }
        public bool IsDirected { get; set; }
        private Vertex<T> start;
        private Vertex<T> end;

        public Edge(Vertex<T> _first, Vertex<T> _second,
            int _weight)
        {
            Weight = _weight;
            start = _first;
            end = _second;
        }

        public int CompareTo(Edge<T> other)
        {
            if (Weight == other.Weight)
                return 0;
            else if (Weight < other.Weight)
                return -1;
            else
                return 0;
        }

        public Vertex<T> GetEither()
        {
            return start;
        }

        public Vertex<T> GetOther(Vertex<T> _vertex)
        {
            if (_vertex.Id == start.Id)
                return end;
            else
                return start;
        }
    }

    public class Vertex<T>
    {
        private List<Vertex<T>> AdjacencyList { get; set; }
        public int Id { get; set; }
        public T Data { get; set; }
        private List<Edge<T>> Edges { get; set; }

        public Vertex(int _id,T _data)
        {
            Id = _id;
            Data = _data;
        }

        public void AddAdjancentVertex(Vertex<T> _vertex,Edge<T> _edge)
        {
            Edges.Add(_edge);
            AdjacencyList.Add(_vertex);
        }

        public IEnumerable<Vertex<T>> GetAdjacencyList(Vertex<T> _vertex)
        {
            return AdjacencyList;
        }

        public IEnumerable<Edge<T>> GetEdges(Vertex<T> _vertex)
        {
            return Edges;
        }       
    }
}
