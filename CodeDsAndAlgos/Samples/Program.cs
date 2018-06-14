using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeDsAndAlgos.Algorithms.Cache;
using CodeDsAndAlgos.DataStructures.Heaps;
using CodeDsAndAlgos.DataStructures.Tries;

namespace Samples
{
    class ListNode<T>
    {
        public T value { get; set; }
        public ListNode<T> next { get; set; }
    }
    public struct Index
    {
        public int start;
        public int end;
    }
    //
    class Program
    {
        static void  DisplayList(List<string> list)
        {
            foreach(string str in list)
            {
                Console.WriteLine(str);
            }
        }

       static int nthDerivative(int[] coefficients, int n, int x)
        {
            if (coefficients.Length == 0) return 0;
            int result = 0;
            int xm=1;
            int nm=1;            
            for(int i = n; i < coefficients.Length; i++)
            {
                nm = 1;                   
                for(int j=i,temp=n;j>1 && temp>0;temp--,j--)                
                    nm *= j;
                result += coefficients[i] * xm * nm;
                xm *= x;
            }
            return result;
        }
        static string simplifyPath(string path)
        {
            string[] paths = path.Split('/');
            Stack<string> pathStk = new Stack<string>();
            foreach (string str in paths)
            {
                if (str == string.Empty) continue;
                if (str == ".") continue;
                if (str == ".." && pathStk.Count > 0)
                    pathStk.Pop();
                else if (Regex.IsMatch(str, @"[\w]+$"))
                    pathStk.Push(str);
            }
            string result = string.Empty;

            while (pathStk.Count > 0)
                result = "/" + pathStk.Pop() + result;

            return result == string.Empty ? "/" : result;
        }

        public static string decodeString(string s)
        {
            StringBuilder rsB = new StringBuilder();
            Stack<string> strStk = new Stack<string>();
            Stack<int> kStk = new Stack<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    int start = i;
                    while (++i < s.Length && Char.IsLetter(s[i]));
                    int end = --i;
                    if (strStk.Count == 0) rsB.Append(s.Substring(start, end - start + 1));
                    else strStk.Push(s.Substring(start, end-start+1));
                }
                else if (Char.IsDigit(s[i]))
                {
                    int start = i;
                    while (++i < s.Length && Char.IsDigit(s[i])) ;
                    int end = --i;

                    int count = 0;
                    if (Int32.TryParse(s.Substring(start, end - start + 1), out count))
                        kStk.Push(count);
                }
                else if(s[i] == ']')
                {
                    if (kStk.Count == 0) throw new Exception("Format exception");
                    int count = kStk.Pop();
                    string temp = string.Empty;
                    while(strStk.Count > 0 && strStk.Peek() != "[")
                    {
                        temp = strStk.Pop() + temp;
                    }

                    if(strStk.Count > 0 && strStk.Peek() == "[") strStk.Pop();
                    else throw new Exception("Format exception");

                    StringBuilder tSB = new StringBuilder();
                    while(count > 0)
                    {
                        tSB.Append(temp);
                        count--;
                    }
                    if (strStk.Count == 0) rsB.Append(tSB);
                    else strStk.Push(tSB.ToString());
                }
                else if(s[i] == '[')
                {
                    strStk.Push("[");
                }
            }
           return rsB.ToString();
        }

        public static int[] minimumOnStack(string[] operations)
        {
            Stack<int> opStk = new Stack<int>();
            Stack<int> minStk = new Stack<int>();
            List<int> result = new List<int>();
            foreach (string op in operations)
            {
                string[] ops = op.Split(' ');
                if (ops[0].ToUpper().Equals("PUSH"))
                {
                    opStk.Push(Convert.ToInt32(ops[1]));
                    if (minStk.Count == 0 || opStk.Peek() <= minStk.Peek())
                        minStk.Push(opStk.Peek());
                }
                else if (ops[0].ToUpper().Equals("POP"))
                {
                    if (opStk.Peek() == minStk.Peek())
                        minStk.Pop();
                    opStk.Pop();
                }
                else if (ops[0].ToUpper().Equals("MIN"))
                    result.Add(minStk.Peek());
            }
            return result.ToArray();
        }

       public static int[] nearestGreater(int[] a)
        {

            int[] b = new int[a.Length];
            Stack<int> maxStk = new Stack<int>();

            for (int i = 0; i < a.Length; i++)
            {
                while (maxStk.Count > 0 && a[i] > a[maxStk.Peek()])
                    b[maxStk.Pop()] = i;
                maxStk.Push(i);
            }
            while (maxStk.Count > 0)
                b[maxStk.Pop()] = Int32.MaxValue;

            for (int i = a.Length - 1; i >= 0; i--)
            {
                while (maxStk.Count > 0 && a[i] > a[maxStk.Peek()])
                {
                    b[maxStk.Peek()] = Math.Min(b[maxStk.Peek()], i);
                    maxStk.Pop();
                }
                    
                maxStk.Push(i);
            }
            while (maxStk.Count > 0)
            {
                if(b[maxStk.Peek()]==Int32.MaxValue)                
                    b[maxStk.Peek()] = -1;                
                maxStk.Pop();
            }
            return b;
        }

        public static string decodeString1(string s)
        {
            StringBuilder rsB = new StringBuilder();
            StringBuilder tsB = new StringBuilder();
            Stack<Index> cStk = new Stack<Index>();
            Stack<Index> dStk = new Stack<Index>();
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsLetter(s[i]))
                {
                    int start = i;
                    while (++i < s.Length && Char.IsLetter(s[i])) ;

                    int end = --i;
                    if (cStk.Count == 0 && dStk.Count == 0)
                        rsB.Append(s.Substring(start, end - start + 1));
                    else if (s[start - 1] == ']')
                        tsB.Append(s.Substring(start, end  - start + 1));
                    else
                    {
                        Index idx = new Index();
                        idx.start = start;
                        idx.end = end - start + 1;
                        cStk.Push(idx);
                    }                        
                }
                else if (Char.IsDigit(s[i]))
                {
                    int start = i;
                    while (++i < s.Length && Char.IsDigit(s[i])) ;
                    int end = --i;
                    Index idx = new Index();
                    idx.start = start;
                    idx.end = end - start + 1;
                    dStk.Push(idx);
                }
                else if (s[i] == '[')
                {
                    if (dStk.Count == 0)
                        throw new Exception("Input String not in a correct format");
                }
                else if (s[i] == ']')
                {
                    Index dS = dStk.Pop();
                    if (cStk.Count > 0)
                    {
                        Index cS = cStk.Pop();
                        StringBuilder temp = new StringBuilder();
                        string str = s.Substring(cS.start, cS.end);
                        int itr = 0;
                        if (Int32.TryParse(s.Substring(dS.start, dS.end), out itr))
                        {
                            while (itr > 0)
                            {
                                temp.Append(str);
                                itr--;
                            }
                            tsB.Append(temp.ToString());
                        }
                    }
                    else
                    {
                        string str = tsB.ToString();                   
                        int itr = 0;
                        if (Int32.TryParse(s.Substring(dS.start, dS.end), out itr))
                        {
                            while (itr > 1)
                            {
                                tsB.Append(str);
                                itr--;
                            }                            
                        }
                    }

                    if (cStk.Count == 0 && dStk.Count == 0)
                    {
                        rsB.Append(tsB.ToString());
                        tsB.Clear();
                    }
                }
            }
            return rsB.ToString();
        }

        public static void RunningMedian()
        {
            int n = 1001;

            int[] a = new int[1001] { 1000 ,94455,
20555,
20535,
53125,
73634,
148,
63772,
17738,
62995,
13401,
95912,
13449,
92211,
17073,
69230,
22016,
22120,
78563,
16571,
1817,
41510,
74518,
81638,
89659,
60445,
35597,
15237,
88830,
26019,
77519,
10914,
36827,
98074,
31450,
89952,
71709,
31598,
70076,
5799 ,
10945,
83478,
1711 ,
24394,
92041,
18784,
93624,
30409,
57256,
88540,
46981,
75425,
30050,
21499,
57064,
19709,
98296,
92661,
51299,
87127,
35032,
45170,
98041,
71859,
43245,
45843,
78164,
31306,
93793,
48240,
37105,
4738 ,
31718,
38816,
45484,
23759,
73952,
55461,
70521,
47560,
44001,
17502,
22986,
90403,
39001,
96402,
26464,
53649,
5415 ,
77763,
40776,
40447,
22934,
55170,
28659,
82531,
1013 ,
6823 ,
13837,
94807,
71415,
67294,
15897,
19486,
22462,
61382,
59597,
12766,
16843,
30118,
60326,
77196,
47620,
83312,
67599,
2973 ,
96066,
94063,
56623,
1481 ,
88179,
13751,
58281,
11113,
68921,
86940,
93644,
86287,
10115,
23833,
97446,
81530,
91127,
13343,
17368,
29941,
74725,
76966,
42707,
7920 ,
7084 ,
3033 ,
85116,
71057,
2698 ,
69067,
74030,
98764,
63131,
47005,
16598,
51310,
60757,
74879,
78775,
29678,
61819,
88771,
32317,
88286,
12604,
29763,
86168,
20083,
43107,
3537 ,
50024,
34184,
80503,
92731,
58457,
87587,
12116,
59925,
58644,
14814,
28993,
49027,
29931,
8476 ,
96032,
46529,
76138,
56789,
21408,
54913,
2820 ,
99579,
43684,
51489,
87865,
72640,
81253,
90385,
9075 ,
40712,
93922,
75451,
91248,
74425,
84534,
66057,
78365,
96650,
25983,
53361,
27817,
54976,
18740,
74100,
79804,
14773,
20629,
55942,
87914,
58389,
10855,
7086 ,
74320,
70891,
58576,
78537,
59883,
56181,
68922,
68958,
13245,
62845,
44409,
4493 ,
53622,
45295,
70551,
48339,
41945,
12886,
1701 ,
86114,
84214,
20441,
60214,
64018,
51566,
97195,
19960,
55833,
55584,
47167,
62919,
29904,
18058,
37847,
8441 ,
77941,
94028,
93716,
63251,
7273 ,
56561,
24012,
28119,
26535,
69307,
15022,
74875,
11252,
27908,
92928,
13719,
12122,
13369,
73933,
76140,
81288,
71129,
12452,
37121,
26713,
59619,
16392,
72970,
77677,
54240,
97763,
71970,
48268,
91479,
51573,
71894,
64392,
75585,
13   ,
90928,
44892,
15035,
65803,
72496,
42943,
58731,
86215,
71417,
88452,
76501,
63909,
86092,
47630,
76361,
39565,
90695,
35980,
55958,
80017,
30009,
26550,
94133,
18331,
74818,
1964 ,
69904,
46712,
66357,
45489,
63077,
73637,
6733 ,
78112,
55792,
95581,
37407,
30875,
98149,
8824 ,
35679,
74650,
89085,
21772,
38632,
81798,
77689,
29327,
34130,
49999,
9345 ,
64139,
76549,
3478 ,
98822,
51368,
21794,
68726,
14432,
88151,
30567,
77510,
78140,
53652,
71974,
33932,
49234,
9382 ,
81159,
63735,
34558,
16839,
38385,
39996,
54963,
93369,
21794,
32652,
22696,
55925,
82652,
48393,
36416,
59201,
68223,
35239,
26921,
90018,
20317,
57706,
78169,
50885,
51568,
72662,
20889,
23542,
6594 ,
86475,
49276,
87754,
50210,
187  ,
20945,
4947 ,
40183,
75908,
98316,
61977,
24912,
37365,
34254,
23916,
85758,
70671,
99470,
53982,
5910 ,
26391,
44000,
42579,
449  ,
38521,
9816 ,
52017,
27535,
30706,
91912,
34130,
17181,
41188,
38236,
83744,
57727,
59181,
5043 ,
97910,
51441,
19712,
76240,
76353,
57077,
26846,
270  ,
42835,
97517,
99740,
13169,
19779,
42483,
73521,
62359,
59285,
28395,
88527,
11302,
55930,
19233,
19566,
6412 ,
52767,
77107,
61000,
52863,
34834,
20181,
57906,
49097,
87974,
77618,
41689,
80680,
51047,
68535,
80950,
10235,
82405,
97042,
23404,
2184 ,
55877,
13278,
80895,
15162,
41673,
69423,
42817,
13955,
5008 ,
62383,
20368,
57775,
39490,
81368,
10638,
90677,
17902,
84897,
39774,
22228,
78867,
81463,
2908 ,
29915,
66350,
83858,
40150,
48755,
97252,
79906,
67292,
53130,
9536 ,
48187,
84644,
67561,
33962,
43813,
81517,
38971,
6197 ,
18237,
13098,
62039,
99605,
23737,
52716,
33859,
24986,
8842 ,
56088,
3853 ,
6657 ,
58996,
50120,
73008,
59207,
90270,
21763,
72811,
86529,
89055,
25941,
96065,
53595,
26938,
63627,
87557,
70751,
45144,
26528,
93300,
79733,
55979,
55340,
95690,
96068,
24408,
29550,
21054,
33251,
1990 ,
41259,
39908,
77338,
91380,
12916,
36545,
81650,
51032,
9357 ,
84531,
56439,
51650,
80597,
10034,
94940,
60576,
97592,
65692,
5720 ,
40472,
58992,
1805 ,
96451,
30684,
97495,
92519,
71445,
27045,
29925,
4696 ,
45387,
71185,
60956,
22726,
62565,
90225,
59271,
60567,
57609,
84980,
45099,
14048,
52983,
25696,
24083,
47923,
86272,
38027,
13615,
8344 ,
78499,
88960,
10149,
91303,
35996,
7644 ,
174  ,
7441 ,
51042,
30100,
28489,
96429,
1285 ,
5798 ,
35507,
63850,
12375,
11131,
40769,
69984,
96111,
85868,
84032,
49094,
27916,
24467,
13370,
30540,
62494,
43337,
55236,
40994,
48649,
81737,
48649,
84646,
5734 ,
48823,
8439 ,
56776,
78923,
53281,
69557,
96560,
59079,
5065 ,
76762,
87806,
32548,
17532,
57790,
28659,
3400 ,
58174,
94106,
47669,
82642,
23828,
78209,
45136,
67165,
33446,
2482 ,
15815,
15183,
51131,
16813,
20917,
99955,
25252,
94045,
95230,
78533,
79955,
91791,
53964,
1372 ,
84905,
58122,
33920,
2437 ,
15912,
78931,
22190,
90439,
89389,
69859,
73081,
13217,
64420,
18217,
80383,
97866,
37052,
96198,
29402,
88183,
13011,
50319,
4490 ,
54615,
60717,
16073,
49501,
40672,
7864 ,
19817,
42044,
9121 ,
77940,
75964,
11559,
10204,
54895,
50101,
643  ,
44285,
36312,
73724,
57502,
732  ,
8294 ,
54237,
98599,
45346,
66787,
44353,
49881,
79798,
94672,
54372,
50766,
55389,
70445,
267  ,
96061,
94661,
36436,
54457,
3782 ,
14376,
30421,
31693,
24581,
1669 ,
98146,
41576,
45954,
34458,
15301,
19808,
35191,
39947,
90398,
50142,
1645 ,
57185,
94495,
51526,
53336,
5519 ,
22250,
4102 ,
77261,
92695,
20721,
89674,
3708 ,
57157,
44132,
23843,
87886,
90905,
71888,
12467,
92574,
70035,
70395,
54880,
20845,
2048 ,
91041,
72388,
41995,
81439,
38882,
43640,
38624,
49729,
11519,
8312 ,
55249,
33769,
12414,
48862,
42817,
49487,
38536,
46525,
6645 ,
82668,
86720,
94531,
73574,
58609,
23350,
82500,
44996,
93745,
37381,
82193,
95794,
28422,
54582,
54141,
9861 ,
9816 ,
14134,
64837,
59546,
25653,
73150,
14795,
59422,
1916 ,
63657,
2239 ,
67756,
18545,
65117,
74401,
1214 ,
51837,
85284,
91140,
10446,
24986,
73640,
71794,
35083,
27373,
53988,
47229,
55795,
24922,
1371 ,
82008,
34738,
15505,
63198,
94284,
57510,
52700,
25431,
16932,
70968,
5440 ,
35524,
38724,
23986,
641  ,
29477,
41552,
52478,
31113,
32692,
79277,
56099,
22684,
67423,
7535 ,
50058,
21411,
54764,
22205,
62685,
56135,
20566,
97424,
87992,
116  ,
91708,
45502,
69168,
33492,
78787,
40136,
38932,
14311,
78861,
62918,
31304,
24690,
4470 ,
134  ,
55804,
53514,
79411,
11903,
76199,
63187,
19438,
42609,
84598,
90555,
81166,
63636,
63042,
18084,
61060,
51035,
18200,
69120,
12889,
3720 ,
2612 ,
8028 ,
43857,
41545,
38691,
39070,
4463 ,
69995,
63760,
25286,
86482,
19564,
78800,
82245,
47820,
71351,
45432,
83610,
13960,
46383,
74165,
11479,
10019,
53560,
29563,
71079,
4595 ,
64116,
56551,
33836,
67836,
59164,
58217,
28045,
17061,
96908,
67115,
37876,
83256,
47228,
63162,
69738,
83144,
58315,
51983,
47316,
29666,
97416,
30927,
59979,
60151,
21444,
71458,
70170,
75004,
17373,
41249,
95951,
97841,
97800,
46140,
82030,
73316,
4357 ,
10075,
90377,
17617,
93543,
28254,
17225,
40771,
7768 ,
3315 ,
40267,
66083,
71651,
87584,
95750,
85419,
34863,
55729,
45570,
56307};
            MaxHeap<int> maxheap = new MaxHeap<int>(n);
            MinHeap<int> minheap = new MinHeap<int>(n);

            for (int i = 0; i < a.Length; i++)
            {
                int aItem = a[i];

                if (minheap.Count() == 0)
                    minheap.Insert(aItem);
                else if (maxheap.Count() == 0)
                    maxheap.Insert(aItem);
                else 
                {
                    if (maxheap.GetMax() > minheap.GetMin())
                    {
                        int temp = minheap.DeleteMin();
                        maxheap.Insert(temp);
                        minheap.Insert(maxheap.DeleteMax());
                    }

                    if (maxheap.Count() == minheap.Count())
                    {
                        if (aItem < maxheap.GetMax())
                        {
                            minheap.Insert(maxheap.DeleteMax());
                            maxheap.Insert(aItem);
                        }
                        else
                            minheap.Insert(aItem);
                    }
                    else
                    {
                        if (aItem < minheap.GetMin())
                            maxheap.Insert(aItem);
                        else
                        {
                            maxheap.Insert(minheap.DeleteMin());
                            minheap.Insert(aItem);
                        }
                    }
                }

                double result = 0.0D;
                if (i % 2 == 0)
                    result = (double)minheap.GetMin();
                else
                    result = ((double)maxheap.GetMax() + (double)minheap.GetMin()) / 2.0;
                Console.WriteLine(result.ToString("0.0", CultureInfo.InvariantCulture));                
            }
        }


        public static string findProfession(int level, int pos)
        {
            return new BitArray(new[] { pos - 1 }).Cast<bool>().Where(x => x).Count() % 2 == 0 ? "Engineer" : "Doctor";
        }
        
       public static string[] findSubstrings(string[] words, string[] parts)
        {
            Trie partsTrie = new Trie();
            foreach (string key in parts)
                partsTrie.Insert(key);
            Tuple<int,int>[] tuples = new Tuple<int,int>[words.Length];
            int j = 0;
            foreach (string word in words)
            {
                tuples[j] = new Tuple<int, int>(0, 0);

                for (int i = 0; i < word.Length; i++)
                {
                    int length = partsTrie.LongestKeyPrefixOf(word.Substring(i, word.Length - i));
                    if (tuples[j].Item2 < length)                    
                        tuples[j] = Tuple.Create<int, int>(i, length);                    
                }
                j++;
            }
            string[] mwords = new string[words.Length];
            for (int i = 0; i < words.Length; i++)
            {
                if (tuples[i].Item2 > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(words[i].Substring(0, tuples[i].Item1));
                    sb.Append("[");
                    sb.Append(words[i].Substring(tuples[i].Item1, tuples[i].Item2));
                    sb.Append("]");
                    sb.Append(words[i].Substring(tuples[i].Item1 + tuples[i].Item2,
                        words[i].Length - (tuples[i].Item1 + tuples[i].Item2)));
                    mwords[i] = sb.ToString();
                }
                else
                    mwords[i] = words[i];
            }

            return mwords;
        }

        public static void Main(string[] args)
        {
            IndexedMinHeap<int> mheap = new IndexedMinHeap<int>();
            mheap.Insert(9);
            mheap.Insert(4);
            mheap.Insert(1);
            mheap.Insert(5);
            mheap.Insert(6);
            mheap.Insert(3);
            mheap.Insert(7);
            mheap.Insert(8);
            mheap.Insert(2);
            int j = mheap.DeleteMin();
            mheap.DecreaseKey(9, 0);


            MFS mfs = new MFS();
            mfs.GetMostFrequentSum();
            Class2 c2 = new Class2();
            c2.treeDiameter(10, new int[][] { new int[] { 2, 5 }
            , new int[] { 5, 7 }, new int[] { 5, 1 }, new int[] { 1, 9 }, new int[] {1, 0 }
            , new int[] {7, 6 }, new int[] {6, 3 }, new int[] {3, 8 }, new int[] {8, 4 } });
            if ((int) Math.Pow(10, 9) < Int32.MaxValue)
                Console.WriteLine("less");
            findSubstrings(new string[] { "roar"  },new string[] {"r","roA" });

            //LRUCache cache = new LRUCache(2);
            //cache.Put(2, 2);
            RunningMedian();
            //           nearestGreater(new int[] { 1, 4, 2, 1, 7, 6 });

            //           minimumOnStack(new string[] { "push 10",
            //"min",
            //"pop",
            //"push 3",
            //"min",
            //"push 5",
            //"pop",
            //"push 3",
            //"min",
            //"pop"});

            //           //decodeString("2[abc]xyc3[z]");
            //           int a = '0';
            //           nthDerivative(new int[] { 4, -9, -6, -4, -5, -6, -5, -3, 7, 4, 8, -4, -10, 7, -10 },5,1);
            //           string strPath = "folder/subfolder/subsubfolder/.//../anotherfolder/file.txt";
            //           simplifyPath(strPath);
            //           string[] paths = strPath.Split(new string[] { "/" },StringSplitOptions.None);




            //           int[] nums = new int[] { 7, 6, 5, 4, 3, 2, 1};
            //           MinHeap<int> minheap = new MinHeap<int>(nums, 2);
            //           minheap.Heapify();

            //                      //        Anagrams();
            //                      //        //string[] str = new string[] { "AAAAAAAAAAAAAA", "BBBBBBBBBBBBBB", "CCCCCCCCCCCCCC" };
            //                      //        //char[][] chars = { new char[] { 'A','1'}, new char[] {'B','2' }, new char[] {'C','3' } };
            //                      //        //isCryptSolution(str, chars);
            //                      //        ListNode<int> first = new ListNode<int>();


            //           //        List<string> list = new List<string>();
            //           //        list.Add("cbda");
            //           //        list.Add("cbad");
            //           //        list.Add("dbac");
            //           //        list.Add("dbca");

            //           //        DisplayList(list);
            //           //        Console.WriteLine("-----------------");

            //           //        list.Sort();



            //           //        DisplayList(list);

            //           //        Console.WriteLine("-----------------");
            //           //        list.Sort(StringComparer.Ordinal);



            //           //        DisplayList(list);

            //           //        int[][] pairs = new int[][]
            //           //        {
            //           //            new int[] {16,26},
            //           //            new int[] { 2,25 },
            //           //            new int[] { 25,27 },
            //           //            new int[] { 19,20 },
            //           //            new int[] { 13,20},
            //           //             new int[] { 4,26 },
            //           //             new int[] { 19,27 },
            //           //              new int[] { 18,26 },
            //           //               new int[] { 13,23 },
            //           //               new int[] { 1,4 },
            //           //               new int[] { 11, 19 },

            //           //                new int[] {16,19},               
            //           //            new int[] { 25,28 },
            //           //            new int[] { 19,30 },
            //           //            new int[] { 19,25},
            //           //             new int[] { 1,11 },
            //           //             new int[] { 2,20},
            //           //             new int[] { 10,22 },
            //           //               new int[] { 6,19 },
            //           //               new int[] { 7, 26 },

            //           //               new int[] {3,30},
            //           //            new int[] { 15,23 },
            //           //            new int[] { 12,26 },
            //           //            new int[] { 1,3 },
            //           //            new int[] { 3,12},
            //           //             new int[] { 3,26 },
            //           //             new int[] { 16,30},
            //           //             new int[] { 2,16 },
            //           //               new int[] { 4,13 }
            //           //        };
            //           //       // int length = "fixmfbhyutghwbyezkveyameoamqoi".Length;
            //           //        SwapLexOrder("qvspxdrbvwfuaahtzbpjudfyzccgzwynkgihwmdshvfnvyvfjc", pairs);

            //           //        char[][] grid = new char[9][]
            //           //        {
            //           //            new char[9] {'.','.','.','1','4','.','.','2','.'},
            //           //            new char[9] {'.','.','6','.','.','.','.','.','.'},
            //           //            new char[9] {'.','.','.','.','.','.','.','.','.'},
            //           //            new char[9] {'.','.','1','.','.','.','.','.','.'},
            //           //            new char[9] {'.','6','7','.','.','.','.','.','9'},
            //           //            new char[9] {'.','.','.','.','.','.','8','1','.'},
            //           //            new char[9] {'.','3','.','.','.','.','.','.','6'},
            //           //            new char[9] {'.','.','.','.','.','7','.','.','.'},
            //           //            new char[9] {'.','.','.','5','.','.','.','7','.'},
            //           //        };
            //           ////bool result =         sudoku2(grid);
            //           ////
            //           //       // Console.ReadLine();


            //           //        first.value = 1;
            //           //        string str1 = "AB";
            //           //        string str2 = "ab";
            //           //        int j = string.Compare(str1, str2,StringComparison.InvariantCulture);
            //           //        int i = String.CompareOrdinal(str1,str2);

            //           //        char[] chars = new char[] { 'A', 'a', 'A', 'b' };
            //           //        Array.Sort(chars);

            //           //        //ListNode<int> second = new ListNode<int>();
            //           //        //second.value = 4;
            //           //        //ListNode<int> third = new ListNode<int>();
            //           //        //third.value = 5;

            //           //        first.next = null;
            //           //        //second.next = third;
            //           //        //third.next = null;


            //           //        ListNode<int> first1 = new ListNode<int>();
            //           //        first1.value = 9999;
            //           //        ListNode<int> second1 = new ListNode<int>();
            //           //        second1.value = 9999;
            //           //        ListNode<int> third1 = new ListNode<int>();
            //           //        third1.value = 9999;
            //           //        ListNode<int> four1 = new ListNode<int>();
            //           //        four1.value = 9999;
            //           //        ListNode<int> five1 = new ListNode<int>();
            //           //        five1.value = 9999;
            //           //        ListNode<int> six1 = new ListNode<int>();
            //           //        six1.value = 9999;

            //           //        first1.next = second1;
            //           //        second1.next = third1;
            //           //        third1.next = four1;
            //           //        four1.next = five1;
            //           //        five1.next = six1;
            //           //        six1.next = null;
            //           //        //ListNode<int> result = addTwoHugeNumbers(first,first1);
            //           //        //  Console.WriteLine(lon);
            //           //        //RemoveWithDuplicates(new int[] {1,1,1,2,2,3});
            //           //        string[][] dishes = new string[][]
            //           //        {
            //           //           new string[] { "Salad", "Tomato", "Cucumber", "Salad", "Sauce" },
            //           //           new string[] { "Pizza", "Tomato", "Sausage", "Sauce", "Dough" },
            //           //           new string[] { "Quesadilla", "Chicken", "Cheese", "Sauce" },
            //           //           new string[] { "Sandwich", "Salad", "Bread", "Tomato", "Cheese" }
            //           //        };
            //           //        //groupingDishes(dishes);
            //           //                    //LeftRotation();

        }

      public static  string[][] groupingDishes(string[][] dishes)
        {

            if (dishes.Length == 0) return dishes;

            SortedDictionary<string, SortedSet<string>> ingredientsToDishes =
                   new SortedDictionary<string, SortedSet<string>>(StringComparer.Ordinal);
           // ingredientsToDishes.Comparer = StringComparison.Ordinal;

            for (int i = 0; i < dishes.Length; i++)
            {
                for (int j = 1; j < dishes[i].Length; j++)
                {
                    if (ingredientsToDishes.ContainsKey(dishes[i][j]))
                    {
                        SortedSet<string> objset = ingredientsToDishes[dishes[i][j]];
                        objset.Add(dishes[i][0]);
                    }
                    else
                    {
                        SortedSet<string> objset = new SortedSet<string>(StringComparer.Ordinal);
                        objset.Add(dishes[i][0]);
                        ingredientsToDishes.Add(dishes[i][j], objset);
                    }

                }
            }

            SortedDictionary<string, SortedSet<string>>.KeyCollection keyColl =
                ingredientsToDishes.Keys;

            List<string> keystoremove = new List<string>();

            foreach (string key in keyColl)
            {
                if (ingredientsToDishes[key].Count() < 2)
                {
                    keystoremove.Add(key);
                }
            }

            foreach (string key in keystoremove)
                ingredientsToDishes.Remove(key);

            string[][] result = new string[ingredientsToDishes.Count][];
            int k = 0;

            foreach (KeyValuePair<string, SortedSet<string>> kvp in ingredientsToDishes)
            {
                result[k] = new string[kvp.Value.Count + 1];
                result[k][0] = kvp.Key;
                int j = 1;
                foreach (string dish in kvp.Value)
                {
                    result[k][j] = dish;
                    j++;
                }
                k++;
            }

            return result;
        }


        public static ListNode<int> addTwoHugeNumbers(ListNode<int> a, ListNode<int> b)
        {
            if (a == null & b == null)
                return null;
            else if (a == null)
                return b;
            else if (b == null)
                return a;

            int i = (int)Math.Pow(10, 4);

            ListNode<int> head = null;

            ListNode<int> current1 = reverse(a);
            ListNode<int> current2 = reverse(b);
            int carry = 0;
            int num1 = 0, num2 = 0;
            while (current1 != null || current2 != null)
            {
                if (current1 == null) num1 = 0; else num1 = current1.value;
                if (current2 == null) num2 = 0; else num2 = current2.value;
                int sum = num1 + num2 + carry;
                ListNode<int> oldhead = head;
                head = new ListNode<int>();
                head.value = sum % i;
                head.next = oldhead;
                carry = sum / i;
                if (current1 != null) current1 = current1.next;
                if (current2 != null) current2 = current2.next;
            }

            if (carry > 0)
            {
                ListNode<int> oldhead = head;
                head = new ListNode<int>();
                head.value = carry;
                head.next = oldhead;
            }


            a = reverse(a);
            b = reverse(b);

            return head;
        }

        public static ListNode<int> reverse(ListNode<int> node)
        {

            ListNode<int> prev = null;
            ListNode<int> current = node;

            while (current != null)
            {
                ListNode<int> next = current.next;
                current.next = prev;
                prev = current;
                current = next;
            }

            return prev;
        }

        static bool isCryptSolution(string[] crypt, char[][] solution)
        {
            if (crypt.Length != 3)
                return false;

            Dictionary<char, char> lookup = new Dictionary<char, char>();

            for (int row = 0; row < solution.Length; row++)
            {
                if (lookup.ContainsKey(solution[row][0]))
                    return false;
                if (lookup.ContainsValue(solution[row][1]))
                    return false;
                lookup.Add(solution[row][0], solution[row][1]);
            }


            string code1 = GetCode(crypt[0], lookup);
            string code2 = GetCode(crypt[1], lookup);
            string code3 = GetCode(crypt[2], lookup);

            if (code1[0] == '0' && code2[0] == '0')
            {
                if (code1.Length > 1 && code2.Length > 1)
                    return false;
            }
            else if (code3[0] == '0')
                return false;

            long first;
            bool result = Int64.TryParse(code1, out first);
            if (!result) return false;

            long second;
            result = Int64.TryParse(code2, out second);
            if (!result) return false;

            if (first == 0 ^ second == 0) return false;

            long third;
            result = Int64.TryParse(code3, out third);
            if (!result) return false;

            long sum = Int64.MinValue;

            try
            {
                checked
                {
                    sum = first + second;
                }
            }
            catch (System.OverflowException e)
            {

            }

            if (sum == third)
                return true;

            return false;
        }

        static string GetCode(string s, Dictionary<char, char> lookup)
        {
            string str = String.Empty;

            foreach (char c in s.ToCharArray())
            {
                str += lookup[c];
            }
            return str;
        }
        public static int RemoveWithDuplicates(int[] nums)
        {
            int j = -1;
            for (int i = 0; i < nums.Length; i++)
            {
                int temp = i;
                while (i + 1 < nums.Length && nums[i] == nums[i + 1])
                    i++;

                nums[++j] = nums[i];
                if (i - temp > 0)
                    nums[++j] = nums[i];


            }
            return Math.Min(nums.Length, ++j);
        }


      //public static string swapLexOrder(string str, int[][] pairs)
      //  {
      //      if (pairs.Length == 0) return str;

      //      Dictionary<string, string> mem = new Dictionary<string, string>();
      //      HashSet<string> visited = new HashSet<String>();
      //      string result = swapLexOrderUtil(str, pairs, mem, visited);
      //      return result;
      //  }

      //public static  string swapLexOrderUtil(string str, int[][] pairs
      //                  , Dictionary<string, string> mem, HashSet<string> visited)
      //  {
      //      if (mem.ContainsKey(str)) return mem[str];

      //      visited.Add(str);

      //      string result = str;

      //      for (int i = 0; i < pairs.Length; i++)
      //      {
      //          if (pairs[i][0] - 1 < 0 || pairs[i][0] - 1 > str.Length
      //            || pairs[i][1] - 1 < 0 || pairs[i][1] - 1 > str.Length)
      //              continue;

      //          char[] strChars = str.ToCharArray();

      //          char temp = strChars[pairs[i][0] - 1];
      //          strChars[pairs[i][0] - 1] = strChars[pairs[i][1] - 1];
      //          strChars[pairs[i][1] - 1] = temp;

      //          string newStr = new string(strChars);

      //          if (!visited.Contains(newStr))
      //          {
      //              string newresult = swapLexOrderUtil(newStr, pairs, mem, visited);
      //              if (string.Compare(result, newresult, StringComparison.Ordinal) < 0)
      //                  result = newresult;
      //          }
      //      }

      //      mem.Add(str, result);

      //      return result;
      //  }

       public static int[][] rotateImage(int[][] a)
        {

            int rcol = a.Length;
            int rrow = a.Length;

            for (int row = 0; row < a.Length; row++)
            {
                rcol--;
                rrow = a.Length - 1;
                for (int col = 0; col < a[row].Length && (row != rrow && col != rcol); col++, rrow--)
                {
                    int temp = a[row][col];
                    a[row][col] = a[rrow][rcol];
                    a[rrow][rcol] = temp;
                }
            }

            for (int col = 0; col < a.Length; col++)
            {
                int i = 0, j = a.Length - 1;
                while (i < j)
                {
                    int temp = a[i][col];
                    a[i][col] = a[j][col];
                    a[j][col] = temp;
                    i++;
                    j--;
                }
            }

            return a;
        }

      public static  bool sudoku2(char[][] grid)
        {
            var list = grid.Select((_, i) => i);
            foreach(var iterator1 in list)
            {
                Console.ReadLine();
            }
            if (grid.Any(row => IsInvalid(row)))
                //|| // across rows
                //grid.Select((_, i) => i).Any(c => IsInvalid(grid.Select(_ => _[c])))) // down columns
                return false;

            // within sub-grids
            for (int r = 0; r < grid.Length; r += 3)
            {
                for (int c = 0; c < grid.Length; c += 3)
                {
                    if (IsInvalid(grid.Skip(r).Take(3).SelectMany(_ => _.Skip(c).Take(3))))
                        return false;
                }
            }
            return true;
        }

      public static  bool IsInvalid(IEnumerable<char> numbers)
        {
            var counts = new int[9];
            return numbers.Any(n => n != '.' && counts[n - '1']++ > 0);
        }

        static void LeftRotation()
        {
            //string[] tokens_n = Console.ReadLine().Split(' ');
            //int n = Convert.ToInt32(tokens_n[0]);
            //int k = Convert.ToInt32(tokens_n[1]);
            //string[] a_temp = Console.ReadLine().Split(' ');
            int[] a = new int[] { 1, 2, 3, 4, 5 };
            int n = a.Length;
            int k =4;

            int nk = k % n;

            if (nk == 0)
                PrintArray(a);
            else
            {
                int i = 0;
                while (i < nk)
                {
                    int tempk = nk;
                    while (i < tempk && nk < n)
                    {
                        int temp = a[i];
                        a[i] = a[nk];
                        a[nk] = temp;

                        i++;
                        nk++;
                    }
                    if (nk == n) nk = tempk;
                }
                PrintArray(a);
            }
        }


        static void PrintArray(int[] nums)
        {
            for (int i = 0; i < nums.Length - 1; i++)
            {
                Console.Write(nums[i]);
                Console.Write(' ');
            }
                

            Console.WriteLine(nums[nums.Length - 1]);
            //Console.ReadLine();
        }

        public static string SwapLexOrder(string str, int[][] pairs)
        {
            Dictionary<int, SortedSet<int>> lookup = new Dictionary<int, SortedSet<int>>();

            for (int i = 0; i < pairs.Length; i++)
            {
                if (lookup.ContainsKey(pairs[i][0]) && lookup.ContainsKey(pairs[i][1]))
                {
                    lookup[pairs[i][0]].UnionWith(lookup[pairs[i][1]]);
                    SortedSet<int> bkSet = lookup[pairs[i][0]];
                    foreach (var key in lookup[pairs[i][1]]){
                        lookup.Remove(key);
                        lookup.Add(key, bkSet);
                    }                    
                }
                else if (lookup.ContainsKey(pairs[i][0]))
                {
                    lookup[pairs[i][0]].Add(pairs[i][1]);
                    lookup.Add(pairs[i][1], lookup[pairs[i][0]]);
                }
                else if (lookup.ContainsKey(pairs[i][1]))
                {
                    lookup[pairs[i][1]].Add(pairs[i][0]);
                    lookup.Add(pairs[i][0], lookup[pairs[i][1]]);
                }
                else
                {
                    SortedSet<int> pairSet = new SortedSet<int>();
                    pairSet.Add(pairs[i][0]);
                    pairSet.Add(pairs[i][1]);
                    lookup.Add(pairs[i][0], pairSet);
                    lookup.Add(pairs[i][1], pairSet);
                }
            }

            char[] strChars = str.ToCharArray();
            HashSet<int> visited = new HashSet<int>();

            foreach (KeyValuePair<int, SortedSet<int>> kvp in lookup)
            {
                var valueSet = kvp.Value;
                if (!visited.Contains(valueSet.Min))
                {
                    visited.Add(valueSet.Min);
                    char[] chars = new char[valueSet.Count];
                    int i = 0;
                    foreach (int num in valueSet)
                    {
                        chars[i] = strChars[num - 1];
                        i++;
                    }
                    Array.Sort(chars);
                    i = valueSet.Count - 1;
                    foreach (int num in valueSet)
                    {
                        strChars[num - 1] = chars[i--];
                    }
                }
            }

            return new string(strChars);
        }

        public static void Anagrams()
        {
            string str1 = "fcrxzwscanmligyxyvym";
            string str2 = "jxwtrhvujlmrpdoqbisbwhmgpmeoke";

            if (str1.Length == 0) Console.WriteLine(str2.Length);
            if (str2.Length == 0) Console.WriteLine(str1.Length);

            int count = 0;
            Dictionary<char, int> aChars = new Dictionary<char, int>();
            Dictionary<char, int> bChars = new Dictionary<char, int>();

            foreach (char c in str1.ToCharArray())
            {
                if (!aChars.ContainsKey(c))
                    aChars.Add(c, 1);
                else
                    aChars[c] += 1;
            }
            foreach (char c in str2.ToCharArray())
            {
                if (!bChars.ContainsKey(c))
                    bChars.Add(c, 1);
                else
                    bChars[c] += 1;
            }

            foreach (char c in str1.ToCharArray())
            {
                if (!bChars.ContainsKey(c) && aChars.ContainsKey(c))
                {
                    count += aChars[c];
                    aChars.Remove(c);
                }
            }
            foreach (char c in str2.ToCharArray())
            {
                if (!aChars.ContainsKey(c) && bChars.ContainsKey(c))
                {
                    count += bChars[c];                    
                    bChars.Remove(c);
                }
            }
            foreach (KeyValuePair<char, int> kvp in aChars)
            {
                count += Math.Abs(kvp.Value - bChars[kvp.Key]);
            }
            Console.WriteLine(count);
        }

    }
}
