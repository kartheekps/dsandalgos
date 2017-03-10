using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures
{
    public class SymbolGraph<T> : Graph<T>
    {        
        private Dictionary<T , int> lookup;
        private int count = 0;

        public SymbolGraph(bool _isDirected) : base(_isDirected)
        {
            lookup = new Dictionary<T, int>();
        }

        public virtual void AddEdge(T _data1,T _data2)
        {
            this.AddEdge(_data1,_data2, 0);
        }
        public virtual void AddEdge(T _data1, T _data2, int weight)
        {
            int id1 = 0, id2 = 0;

            if (lookup.ContainsKey(_data1))
                lookup.TryGetValue(_data1, out id1);
            else
                lookup.Add(_data1, count++);

            if (lookup.ContainsKey(_data2))
                lookup.TryGetValue(_data2, out id2);
            else
                lookup.Add(_data2, count++);

            base.AddEdge(id1, _data1, id2, _data2, weight);
        }
    }

}
