using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    public class IndexedMinHeap2<TKey> where TKey : IComparable<TKey>
    {
        public class Node : IComparable<Node>
        {
            public TimeSpan time;
            public TKey key;
            public int CompareTo(Node that)
            {
                return this.time.CompareTo(that.time);
            }
        }
        Node[] pq;
        Dictionary<TKey, int> qp = new Dictionary<TKey, int>();
        int N = -1;
        public IndexedMinHeap2(int size)
        {
            pq = new Node[size];
        }
        public int Size()
        {
            return N + 1;
        }
        public TKey GetMinKey()
        {
            return pq[0].key;
        }
        public TimeSpan GetMinValue()
        {
            return pq[0].time;
        }
        private void Sink(int k)
        {
            while (2 * k + 1 <= N)
            {
                int j = 2 * k + 1;
                if (j + 1 <= N) j = pq[j].CompareTo(pq[j + 1]) < 0 ? j : j + 1;
                if (pq[k].CompareTo(pq[j]) < 0) break;
                qp[pq[k].key] = j;
                qp[pq[j].key] = k;
                Node temp = pq[k];
                pq[k] = pq[j];
                pq[j] = temp;
                k = j;
            }
        }
        private void Swim(int k)
        {
            while (k > 0 && pq[(k - 1) / 2].CompareTo(pq[k]) > 0)
            {
                int j = (k - 1) / 2;
                qp[pq[k].key] = j;
                qp[pq[j].key] = k;
                Node temp = pq[k];
                pq[k] = pq[j];
                pq[j] = temp;
                k = j;
            }
        }
        public void Heapify()
        {
            for (int i = (N - 1) / 2; i >= 0; i--)
                Sink(i);
        }
        public void Insert(TKey item, TimeSpan value)
        {
            pq[++N] = new Node() { key = item, time = value };
            qp.Add(item, N);
            Swim(N);
        }
        public KeyValuePair<TKey, TimeSpan> DeleteMin()
        {
            TKey key = GetMinKey();
            TimeSpan item = GetMinValue();
            qp.Remove(pq[0].key);
            pq[0] = pq[N];
            qp[pq[0].key] = 0;
            pq[N] = default(Node);
            N--;
            Sink(0);
            return new KeyValuePair<TKey, TimeSpan>(key, item);
        }
        public void DecreaseKey(TKey okey, TimeSpan newtime)
        {
            if (pq[qp[okey]].time.CompareTo(newtime) > 0)
            {
                int index = qp[okey];
                pq[qp[okey]].time = newtime;
                Swim(index);
            }
        }
        public bool ContainsKey(TKey key)
        {
            return qp.ContainsKey(key);
        }
        public TimeSpan GetTime(TKey key)
        {
            if (ContainsKey(key))
            {
                return pq[qp[key]].time;
            }
            return TimeSpan.MaxValue;
        }
    }
    public class DepArrTime : IComparable<DepArrTime>
    {
        public TimeSpan Dep { get; set; }
        public TimeSpan Arr { get; set; }
        public int CompareTo(DepArrTime other)
        {
            return Dep.CompareTo(other.Dep);
        }
    }
    public class FlightPaths
    {
        public static string flightPlan(string[][] times, string source, string dest)
        {
            Dictionary<string, Dictionary<string, SortedSet<DepArrTime>>> graph = new Dictionary<string, Dictionary<string, SortedSet<DepArrTime>>>();
            for (int i = 0; i < times.Length; i++)
            {
                if (!graph.ContainsKey(times[i][0]))
                    graph.Add(times[i][0], new Dictionary<string, SortedSet<DepArrTime>>());
                if (!graph.ContainsKey(times[i][1]))
                    graph.Add(times[i][1], new Dictionary<string, SortedSet<DepArrTime>>());
                if (!graph[times[i][0]].ContainsKey(times[i][1]))
                    graph[times[i][0]].Add(times[i][1], new SortedSet<DepArrTime>());
                string[] deps = times[i][2].Split(new char[] { ':' });
                string[] arrs = times[i][3].Split(new char[] { ':' });
                TimeSpan dep = new TimeSpan(Convert.ToInt32(deps[0]), Convert.ToInt32(deps[1]), 0);
                TimeSpan arr = new TimeSpan(Convert.ToInt32(arrs[0]), Convert.ToInt32(arrs[1]), 0);
                graph[times[i][0]][times[i][1]].Add(new DepArrTime() { Dep = dep, Arr = arr });

            }
            if (!graph.ContainsKey(source))
                return "-1";

            IndexedMinHeap2<string> minheap = new IndexedMinHeap2<string>(graph.Keys.Count);           
            foreach (string str in graph.Keys)
                minheap.Insert(str, TimeSpan.MaxValue);
            minheap.DecreaseKey(source, new TimeSpan(0, 0, 0));
            TimeSpan layover = new TimeSpan(0, 0, 0);
            KeyValuePair<string, TimeSpan> min = default(KeyValuePair<string, TimeSpan>);
            while (minheap.Size() > 0)
            {
                min = minheap.DeleteMin();
                if (min.Value == TimeSpan.MaxValue)
                    continue;
                if(min.Key == dest)                
                    break;
                if (!graph.ContainsKey(min.Key))
                    continue;
                foreach(var outadj in graph[min.Key])
                {
                    if (minheap.ContainsKey(outadj.Key))
                    {
                        foreach (var adj in outadj.Value.Where(d => d.Dep >= min.Value.Add(layover)))
                        {
                            if (min.Value.Add(adj.Arr - min.Value) < minheap.GetTime(outadj.Key))
                                minheap.DecreaseKey(outadj.Key, min.Value.Add(adj.Arr - min.Value));
                        }
                    } 
                }
                if (layover.Hours == 0)
                    layover = new TimeSpan(1, 0, 0);
            }

            if (min.Value == TimeSpan.MaxValue)
                return "-1";

            string eartime = string.Empty;
            if (min.Value.Days > 0)
                eartime = (min.Value.Days * 24 + min.Value.Hours).ToString();
            else
                eartime = min.Value.ToString("hh");

            return eartime + ":" + min.Value.ToString("mm");            
        }
        
        public static string flightPlanRec(string[][] times, string source, string dest)
        {
             Dictionary<string, Dictionary<string, SortedSet<DepArrTime>>> graph = 
                new Dictionary<string, Dictionary<string, SortedSet<DepArrTime>>>();
            for(int i = 0; i < times.Length; i++)
            {
                if (!graph.ContainsKey(times[i][0]))
                    graph.Add(times[i][0], new Dictionary<string, SortedSet<DepArrTime>>());
                if (!graph[times[i][0]].ContainsKey(times[i][1]))
                    graph[times[i][0]].Add(times[i][1], new SortedSet<DepArrTime>());
                string[] deps = times[i][2].Split(new char[] { ':' });
                string[] arrs = times[i][3].Split(new char[] { ':' });
                TimeSpan dep = new TimeSpan(Convert.ToInt32(deps[0]), Convert.ToInt32(deps[1]), 0);
                TimeSpan arr = new TimeSpan(Convert.ToInt32(arrs[0]), Convert.ToInt32(arrs[1]), 0);
                graph[times[i][0]][times[i][1]].Add(new DepArrTime() { Dep = dep,Arr = arr});
             }
            if (!graph.ContainsKey(source))
                return "-1";
            HashSet<string> visited = new HashSet<string>();
            TimeSpan time = flightPlanUtil(graph, new TimeSpan(0, 0, 0),new TimeSpan(1,0,0), source, dest, visited,true);
            
            return (time.Days * 24 + time.Hours).ToString() +  ":" + time.Minutes.ToString();
        }
        private static TimeSpan flightPlanUtil(Dictionary<string, Dictionary<string, SortedSet<DepArrTime>>> graph,
            TimeSpan arr,TimeSpan minlayover,string source,string dest, HashSet<string> visited,bool IsOrigin)
        {
            if (source == dest)
                return new TimeSpan(0, 0, 0);

            visited.Add(source);
            TimeSpan min = TimeSpan.MaxValue;

            TimeSpan newDep = arr;
            if (!IsOrigin) newDep = newDep.Add(minlayover);

            foreach (var kvp in graph[source])
            {
                if (!visited.Contains(kvp.Key))
                {
                    foreach (var edge in kvp.Value.Where(t => t.Dep >= newDep))
                    {
                        TimeSpan newArr = edge.Arr;
                        if (newArr.Hours > 23)
                            newArr = newArr.Subtract(new TimeSpan(newArr.Hours - 24, 0, 0));
                        
                        TimeSpan newmin = flightPlanUtil(graph,newArr,minlayover,kvp.Key,dest,visited,false);

                        if(newmin != TimeSpan.MaxValue)
                            if (edge.Arr.Subtract(arr).Add(newmin) < min) 
                                min = edge.Arr.Subtract(arr).Add(newmin);
                    }
                }
            }

            visited.Remove(source);
            return min;
        }
    }
}
