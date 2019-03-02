﻿using System;
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
    class NState
    {
        public int max = Int32.MaxValue;
        public string str = string.Empty;
    }
    class Position
    {
        public int row = 0;
        public int col = 0;
    }
    public class State1
    {
        public int MinHeight = Int32.MaxValue;
        public List<int> Roots = new List<int>();
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

       public static int[] findLongestSubarrayBySum(int s, int[] a)
        {
            int l = 0, sum = 0;
            int[] range = new int[] { 1, 1 };
            int maxLen = Int32.MinValue;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
                if (sum > s)
                    sum -= a[l++];
                if (sum == s)
                {
                    while (i + 1 < a.Length && a[i + 1] == 0) i++;
                    if (i - l > maxLen)
                    {
                        range[0] = l + 1;
                        range[1] = i + 1;
                        maxLen = i - l;
                    }
                    sum -= a[l++];
                }
            }
            if (maxLen == Int32.MinValue) return new int[] { -1 };
            return range;
        }

        public static int productExceptSelf(int[] nums, int m)
        {
            int prod = 1;
            for (int i = 0; i < nums.Length; i++)
            {
                try
                {
                    prod *= nums[i];
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            
            int result = 0;
            for (int i = 0; i < nums.Length; i++)
                result += (prod / nums[i]) % m;
            return result % m;
        }

      public static string minSubstringWithAllChars(string s, string t)
        {
            Dictionary<char, int> tlookup = new Dictionary<char, int>();
            SortedSet<int> map = new SortedSet<int>();
            int b = 0, e = s.Length-1;
            foreach (char c in t)
                tlookup.Add(c, -1);
            for (int i = 0; i < s.Length; i++)
            {
                if (!tlookup.ContainsKey(s[i])) continue;
                if (tlookup[s[i]] != -1) map.Remove(tlookup[s[i]]);
                tlookup[s[i]] = i; 
                map.Add(i);
                if (map.Count == t.Length)
                {
                    if (map.Max - map.Min < e - b)
                    {
                        e = map.Max;
                        b = map.Min;
                    }
                }
            }
            return s.Substring(b, e - b + 1);
        }
        static int minimumSwaps(int[] arr)
        {
            int swap = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                while (arr[i] != i + 1)
                {
                    int temp = arr[arr[i] - 1];
                    arr[arr[i] - 1] = arr[i];
                    arr[i] = temp;      
                    swap++;
                }
            }
            return swap;
        }
       public static int[][] climbingStaircase(int n, int k)
        {
            List<List<int>> paths = new List<List<int>>();
            Console.WriteLine(paths.Count);
            climbingStaircaseUtil(n, k, paths, new List<int>(), 0);
            Console.WriteLine(paths.Count);
            int[][] arr = new int[paths.Count][];
            for (int i = 0; i < paths.Count; i++)
            {
                arr[i] = paths[i].ToArray();
            }
            return arr;
        }
       public static void climbingStaircaseUtil(int n, int k, List<List<int>> paths, List<int> path, int level)
        {
            if (n < 0) return;
            if (n == 0)
            {
                paths.Add(path.ToList());
                return;
            }
            for (int i = 1; i <= k; i++)
            {
                path.Add(i);
                climbingStaircaseUtil(n - i, k, paths, path, level + 1);
                path.RemoveAt(level);
            }
        }
       public static int[][] nQueens(int n)
        {
            List<List<int>> lists = new List<List<int>>();
            List<int> list = new List<int>();
            int start = 0;
            nQueensUtil(n, 0, start, list, lists);
            int[][] arr = new int[lists.Count][];
            for (int i = 0; i < lists.Count; i++)
                arr[i] = lists[i].ToArray();
            return arr;
        }
        public static void nQueensUtil(int n, int q, int start, List<int> list, List<List<int>> lists)
        {
            if (q == n) { lists.Add(list.ToList()); return; }
            for (int i = start; i < n; i++)
            {
                if (isSafe(list, q, i, n))
                {
                    list.Add(i + 1);
                    nQueensUtil(n, q + 1, 0, list, lists);
                    list.RemoveAt(list.Count - 1);
                }
            }
        }
        public static bool isSafe(List<int> list, int q, int pos, int n)
        {
            if (list.Count == 0) return true;
            for (int i = 0; i < list.Count; i++)
            {
                //horizontal
                //for (int k = i; k < n; k++)
                //    if (k == q && k == pos)
                //        return false;
                //vertical
                for (int k = 0, c = list[i] - 1; k < n; k++)
                    if (k == q && c == pos)
                        return false;
                //rightdiagonal
                for (int k = i + 1, c = list[i]; k < n && c < n; k++, c++)
                    if (k == q && c == pos)
                        return false;
                //leftdiagonal
                for (int k = i + 1, c = list[i] - 2; k < n && c >= 0; k++, c--)
                    if (k == q && c == pos)
                        return false;
            }
            return true;
        }

       public static int[][] sumSubsets(int[] a, int num)
        {
            List<int[]> solutions = new List<int[]>();
            sumSubsetsUtil(a, num, 0, 0, new List<int>(), solutions);
            return solutions.ToArray();
        }
        public static void sumSubsetsUtil(int[] a, int num, int sum, int start, List<int> soFar, List<int[]> solutions)
        {
            if (sum == num)
            {
                for (int i = 0; i < solutions.Count; i++)
                {
                    if (solutions[i].Length == soFar.Count &&
                      soFar.Intersect(solutions[i]).Count() == soFar.Distinct().Count())
                    {
                        return;
                    }
                }
                solutions.Add(soFar.ToArray());
                return;
            }
            if (start == a.Length) return;
            if (sum + a[start] > num) return;
            for (int i = start; i < a.Length; i++)
            {
                soFar.Add(a[i]);
                sumSubsetsUtil(a, num, sum + a[i], i + 1, soFar, solutions);
                soFar.RemoveAt(soFar.Count - 1);
            }
        }


        // Complete the stepPerms function below.
        static int stepPerms(int n)
        {
            Dictionary<int, int> mSet = new Dictionary<int, int>();
            return stepPermsUtil(n, 3, mSet);
        }
        static int stepPermsUtil(int n, int m, Dictionary<int, int> mSet)
        {
            if (n <= 0) return 0;
            if (mSet.ContainsKey(n)) return mSet[n];
            int steps = 0;
            for (int i = 1; i <= m; i++)
            {
                steps = 1 + stepPermsUtil(n - i, m, mSet);
            }
            mSet[n] = steps;
            return steps;
        }

        // Complete the ways function below.
      public static int ways(int n, int[] coins)
        {            
            return waysRecursive(coins,n,coins.Length-1);
        }
        // Complete the ways function below.
        public static int waysRecursive(int[] coins, int n,int m)
        {
            int j = 0;
            if (n == 0)
                return 1;
            if (n < 0)
                return 0;
            if (m < 0 && n > 0)
                return 0;
            //if (mSet.ContainsKey(n)) return mSet[n];
            int noOfWays = waysRecursive(coins,n,m-1) + waysRecursive(coins,n-coins[m],m);           
            return noOfWays;
        }

       public static int mapDecoding(string message)
        {
            int[] memo = new int[message.Length];
            return mapDecodingUtil(message, memo) % 1000000007;
        }
       public static int mapDecodingUtil(string message, int[] memo)
        {
            if (message.Length == 0) return 1;
            if (memo[message.Length - 1] != 0) return memo[message.Length - 1];
            int ways = 0;
            string str1 = message.Substring(message.Length - 1, 1);
            if (Convert.ToInt32(str1) > 0)
                ways = (ways + mapDecodingUtil(message.Substring(0, message.Length - 1), memo)) % 1000000007;
            if (message.Length > 1)
            {
                string str2 = message.Substring(message.Length - 2, 2);
                if (Convert.ToInt32(str2) > 9 && Convert.ToInt32(str2) < 27)
                    ways = (ways + mapDecodingUtil(message.Substring(0, message.Length - 2), memo)) % 1000000007;
            }
            memo[message.Length - 1] = ways % 1000000007;
            return ways;
        }

        public static int LargestRectangleArea(int[] heights)
        {
            int max = 0;
            int i = 0;
            int ctop = 0;
            Stack<int> rectStk = new Stack<int>();
            for (i = 0; i < heights.Length; i++)
            {
                if (rectStk.Count == 0 || heights[rectStk.Peek()] <= heights[i])
                    rectStk.Push(i);
                else
                {
                    ctop = rectStk.Peek();
                    while (rectStk.Count > 0 && heights[rectStk.Peek()] > heights[i])
                    {
                        max = Math.Max(max, heights[rectStk.Peek()] * (ctop - rectStk.Peek() + 1));
                        rectStk.Pop();
                    }
                    if (rectStk.Count == 0)
                        max = Math.Max(max, heights[i] * (i + 1));
                }
            }
            i--;
            int cmax = 0;
            int idx = 0;
            while (rectStk.Count > 0)
            {
                Console.WriteLine(i);
                Console.WriteLine(rectStk.Peek());
                cmax = Math.Max(cmax, heights[rectStk.Peek()] * (i - rectStk.Peek() + 1));
                idx = rectStk.Pop();
            }
            if (rectStk.Count == 0)
                max = Math.Max(max, cmax + heights[i] * idx);

            return max;
        }




        public static int longestPath(string fileSystem)
        {
            string[] paths = fileSystem.Split(new string[] { "\f" }, StringSplitOptions.None);
            int lvl = fileSystem.LastIndexOf('\t');
            if (lvl == -1 && paths.Length == 1)
            {
                if (fileSystem.Contains("."))
                    return fileSystem.Length;
                else
                    return 0;
            }
            int[] levels = new int[lvl];
            int maxLen = 0;
            int curLen = 0;
            for (int i = 0; i < paths.Length; i++)
            {
                lvl = paths[i].LastIndexOf('\t') + 1;
                levels[lvl + 1] = levels[lvl] + paths[i].Length - lvl;
                curLen = levels[lvl + 1];
                if (paths[i].Contains("."))
                {
                    if (curLen + lvl > maxLen)
                        maxLen = curLen + lvl;
                }
            }
            return maxLen;
        }

       public static bool regularExpressionMatching(string s, string p)
        {
            bool[][] memo = new bool[s.Length + 1][];
            for (int i = 0; i < memo.Length; i++)
                memo[i] = new bool[p.Length + 1];
            memo[0][0] = true;

            for (int j = 1; j < memo[0].Length ; j++)
                if (isMatch(p[j - 1], '*'))
                    memo[0][j] = memo[0][j - 2];

            for (int i = 1; i < memo.Length; i++)
            {
                for (int j = 1; j < memo[i].Length; j++)
                {
                    if (isMatch(s[i - 1], p[j - 1]))
                        memo[i][j] = memo[i - 1][j - 1];
                    else if (isMatch(p[j - 1], '*'))
                    {
                        if (memo[i][j - 1])
                            memo[i][j] = memo[i - 1][j] || memo[i][j - 2];
                        else
                            memo[i][j] = memo[i][j - 2];
                    }
                }
            }
            Console.WriteLine(memo[s.Length][p.Length]);
            return memo[s.Length][p.Length];
        }
        public static bool regExMatchingUtil(string s, string p, int si, int pi)
        {
            if (si == -1 && pi == -1) return true;
            if (pi == -1) return false;
            if (si == -1 && !isMatch(p[pi], '*')) return false;

            if (si == -1)
                return regExMatchingUtil(s, p, -1, pi - 2);
            if (isMatch(s[si], p[pi]))
                return regExMatchingUtil(s, p, si - 1, pi - 1);
            else if (isMatch(p[pi], '*'))
            {
                if (isMatch(s[si], p[pi - 1]))
                {
                    return regExMatchingUtil(s, p, si - 1, pi)
                        || regExMatchingUtil(s, p, si, pi - 2);
                }
                else
                    regExMatchingUtil(s, p, si, pi - 2);
            }
            return false;
        }
        public static bool isMatch(char a, char b)
        {
            if (b == '.') return true;
            return a.CompareTo(b) == 0;
        }
        public static int paintHouses(int[][] cost)
        {
            if (cost.Length == 0) return 0;
            int[][] cache = new int[cost.Length][];
            for (int i = 0; i < cache.Length; i++)
                cache[i] = new int[cost[i].Length];
            return paintHousesUtil(cost, cache, 0, 0, 0);
        }
        public static int paintHousesUtil(int[][] cost, int[][] cache, int house, int color, int ccost)
        {
            if (house == cost.Length) return ccost;
            if (cache[house][color] != 0) return cache[house][color];
            int min = Int32.MaxValue;
            for (int i = 0; i < cost[house].Length; i++)
            {
                if (i+1 != color)
                {
                    min = Math.Min(min, ccost + paintHousesUtil(cost, cache, house + 1, i + 1, cost[house][i]));
                }
            }
            cache[house][color] = min;
            return min;
        }
       public static int singleNumber(int[] nums)
        {
            int res = 0;
            foreach (int num in nums)
            {
                res = res ^ num;
            }
            return res;
        }

        private static void Add(int sum)
        {
            return;
        }
        // Complete the maxSubsetSum function below.
       public static int maxSubsetSum(int[] arr)
        {
            if (arr.Length == 0) return 0;
            if (arr.Length == 1) return arr[0];
            if (arr.Length == 2) return Math.Max(arr[0], arr[1]);
            int[] cache = new int[arr.Length];
            cache[0] = arr[0];
            cache[1] = arr[1];
            int max = Math.Max(arr[0], arr[1]);
            cache[1] = max;
            for (int i = 2; i < arr.Length; i++)
            {
                cache[i] = Math.Max(max, arr[i] + cache[i - 2]);
                max = Math.Max(max, cache[i]);
            }
            return max;
        }
        // Complete the candies function below.
       public static long candies(int[] arr)
        {
            if (arr.Length == 0) return 0L;
            int[] cache = new int[arr.Length];
            cache[0] = 1;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] > arr[i - 1])
                    cache[i] = 1 + cache[i - 1];
                else if (arr[i] == arr[i - 1])
                    cache[i] = 1;
                else
                {
                    if (cache[i - 1] == 1)
                    {
                        int j = i - 1;
                        for (; j >= 0 && arr[j] > arr[j + 1]; j--)
                            cache[j + 1]++;
                        if (j == -1) cache[0]++;
                    }
                    else  cache[i] = 1;
                }
            }
            long result = 0L;
            for (int i = 0; i < cache.Length; i++)
            {
                result += cache[i];
                Console.WriteLine(cache[i]);
            }

            return result;
        }
        // Complete the abbreviation function below.
        public static string abbreviation(string a, string b)
        {
            if (a == string.Empty) return b == string.Empty ? "YES" : "NO";
            if (b == string.Empty)
            {
                foreach (char c in a)
                    if (!Char.IsLower(c))
                        return "NO";
            }
            bool[] cache = new bool[b.Length + 1];
            cache[0] = true;
            for (int i = 0; i < a.Length; i++)
            {
                bool[] next = new bool[b.Length + 1];
                next[0] = cache[0] && Char.IsLower(a[i]);
                for (int j = 1; j < b.Length + 1; j++)
                {
                    if (a[i] == b[j - 1])
                        next[j] = cache[j - 1];
                    else if (Char.ToUpper(a[i]) == b[j - 1])
                        next[j] = cache[j - 1] || cache[j];
                    else if (Char.IsLower(a[i]))
                        next[j] = cache[j];
                }
                cache = next;
            }
            return cache[b.Length] ? "YES" : "NO";
        }
        // Complete the abbreviation function below.
        public static bool abbreviation(string a, string b, int ai, int bi)
        {
            if (ai == -1 && bi == -1) return true;
            if (ai == -1) return false;
            else if (bi == -1)
            {
                for (int i = ai; i >= 0; i--)
                {
                    if (!Char.IsLower(a[i]))
                        return false;
                }
                return true;
            }
            if (Char.ToUpper(a[ai]) == b[bi]) return abbreviation(a, b, ai - 1, bi - 1);
            else if (Char.IsLower(a[ai])) return abbreviation(a, b, ai - 1, bi);
            return false;
        }
      public static  int findFirstSubstringOccurrence(string s, string x)
        {
            if (x.Length > s.Length) return -1;
            int[] state = new int[x.Length];
            int i = 0, j = 0;
            for (j = 0, i = 1; j <= i && i < x.Length; i++)
            {
                if (x[i] == x[j]) state[i] = state[j++] + 1;
                else state[i] = j;
            }
            for (i = 0, j = 0; i < s.Length && j < x.Length; i++)
            {
                if (x[j] == s[i]) j++;
                else if (j > 0) j = state[j - 1];
            }
            return (j == x.Length) ? i - j : -1;
        }
        public static string classifyStrings(string s)
        {
            string result = "good";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '?')
                {
                    int ti = i;
                    while (i + 1 < s.Length && s[i + 1] == '?') i++;
                    if (i - ti > 1)
                        result = "mixed";
                }
                else if (IsVowel(s[i]))
                {
                    int ti = i;
                    while (i + 1 < s.Length
                          && (IsVowel(s[i + 1]) || s[i + 1] == '?'))
                        i++;
                    if (i - ti > 1)
                    {
                        if (s.Substring(ti, i - ti + 1).Contains("?"))
                            result = "mixed";
                        else
                            result = "bad";
                    }
                }
                else
                {
                    int ti = i;
                    while (i + 1 < s.Length
                          && !IsVowel(s[i + 1]))
                        i++;
                    if (i - ti > 3)
                    {
                        if (s.Substring(ti, i - ti + 1).Contains("?"))
                            result = "mixed";
                        else
                            result = "bad";
                    }
                }
            }
            return result;
        }

      public  static int alternatingCharacters(string s)
        {
            int count = 0;
            int index = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (s[i] == s[index])
                    count++;
                else
                    index = i;
            }
            return count;
        }

       public static string[] textJustification(string[] words, int l)
        {
            List<string> formattedList = new List<string>();
            int noOfWords = 0;
            int len = 0;
            for (int i = 0; i <= words.Length; i++)
            {
                if (i != words.Length &&
                    len + words[i].Length + noOfWords - 1 < l)
                {
                    len += words[i].Length;
                    noOfWords++;
                }
                else
                {
                    StringBuilder jSb = new StringBuilder(l);
                    int noOfSpaces = l - len;
                    for (int j = i - noOfWords,k=0; j < i - 1; j++,k++)
                    {
                        jSb.Append(words[j]);
                        int wSpaces = (noOfSpaces/(noOfWords - k - 1));
                        noOfSpaces -= wSpaces;
                        while (wSpaces-- > 0)
                            jSb.Append(" ");
                    }
                    jSb.Append(words[i - 1]);
                    formattedList.Add(jSb.ToString());
                    if (i != words.Length)
                    {
                        len = words[i].Length;
                        noOfWords = 1;
                    }
                }
            }
            return formattedList.ToArray<string>();
        }

       public static int[] treeBottom(string tree)
        {
            int level = 0;
            int lValue = 0;
            List<int> nodesList = new List<int>();
            SortedDictionary<int, List<int>> lnodesDictionary = new SortedDictionary<int, List<int>>();
            for (int i = 0; i < tree.Length; i++)
            {
                if (Char.IsWhiteSpace(tree[i]))
                    continue;
                else if (tree[i] == ')')
                {
                    level--;
                    if (tree[i - 1] == '(')
                    {
                        if (lnodesDictionary.ContainsKey(level))
                            lnodesDictionary[level].Add(lValue);
                        else
                            lnodesDictionary.Add(level, new List<int> { lValue });
                    }
                }
                else if (tree[i] == '(')
                    level++;
                else if (Char.IsDigit(tree[i]))
                {
                    lValue = 0;
                    while (!Char.IsWhiteSpace(tree[i]))
                    {
                        lValue = lValue * 10 + Convert.ToInt32(tree[i].ToString());
                        i++;
                    }
                    i--;
                }
            }
            return lnodesDictionary.Count > 0 ? lnodesDictionary[lnodesDictionary.Keys.Max()].ToArray() : new int[0];
        }


        public static string classifyStrings1(string s)
        {
            int l = 0;
            int vd = 2, cd = 4;
            string result = "good";
            SortedSet<int> qSet = new SortedSet<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '?')
                {
                    
                }
                else if (IsVowel(s[i]))
                {

                }
                else
                {

                }
            }
            //        
            //        {
            //            if (i - l == 2)
            //            {
            //                if (qSet.GetViewBetween(l, i).Count() > 0)
            //                    result = "mixed";
            //                else
            //                    result = "bad";
            //            }
            //        }
            //        else
            //        {
            //            if (qSet.GetViewBetween(l, i).Count > 0)
            //                l = qSet.GetViewBetween(l, i).Max;
            //            else l = i;
            //        }
            //    }
            //    else
            //    {
            //        if (!IsVowel(s[l]) || s[l] == '?')
            //        {
            //            if (i - l == 4)
            //            {
            //                if (qSet.GetViewBetween(l, i).Count() > 0)
            //                    result = "mixed";
            //                else
            //                    result = "bad";
            //            }
            //        }
            //        else
            //        {
            //            if (qSet.GetViewBetween(l, i).Count > 0)
            //                l = qSet.GetViewBetween(l, i).Max;
            //            else l = i;
            //        }
            //    }
            //}
            return result;
        }

        public static  bool IsVowel(char c)
        {
            return Regex.IsMatch(c.ToString(), "[aeiouAEIOU]");
        }

        // Complete the beautifulBinaryString function below.
       public static int beautifulBinaryString(string b)
        {
            int count = 0;
            int subStrIdx = 0;
            string p = "010";
            for (int i = 0; i < b.Length && subStrIdx != -1; i++)
            {
                subStrIdx = kmp(b, p, i);
                if (subStrIdx != -1)
                {
                    count++;
                    i = subStrIdx + p.Length - 1;
                }
            }
            return count;
        }
        public static int kmp(string b, string p, int start)
        {
            int[] state = new int[p.Length];
            int i = 0, j = 0;
            for (j = 0, i = 1; i < p.Length; i++)
            {
                if (p[i] == p[j])
                    state[i] = state[j++] + 1;
                else
                    state[i] = j;
            }
            for (i = start, j = 0; i < b.Length && j < p.Length; i++)
            {
                if (b[i] == p[j])
                    j++;
                else if (j > 0)
                {
                    j = state[j - 1];
                    i--;
                }
            }
            return j < p.Length ? -1 : i - j;
        }
        // Complete the palindromeIndex function below.
        public static int palindromeIndex(string s)
        {
            int result = -1;
            for (int i = 0; i < s.Length && result == -1; i++)
            {
                if (s[i] == s[s.Length - 1 - i])
                    continue;
                if (i + 1 < s.Length && s[i + 1] == s[s.Length - 1 - i])
                {
                    int left = i + 1;
                    int right = s.Length - 1 - i;
                    while (left < right && s[left++] == s[right--]) ;
                    if (left >= right)
                        result = i;
                    break;
                }
                if (s.Length - 2 - i >= 0 && s[i] == s[s.Length - 2 - i])
                {
                    int left = i;
                    int right = s.Length - 2 - i;
                    while (left < right && s[left++] == s[right--]) ;
                    if (left >= right)
                        result = s.Length - 1 - i;
                    break;
                }
            }
            return result;
        }

        // Complete the circularArrayRotation function below.
       public static int[] circularArrayRotation(int[] a, int k)
        {          
            int nk = k % a.Length;
            if (nk != 0)
            {
                int left = 0;
                int right = a.Length - nk;
                while (left < right)
                {
                    int tright = right;
                    while (right < a.Length && left < tright)
                    {
                        int temp = a[left];
                        a[left] = a[right];
                        a[right] = temp;
                        left++;
                        right++;
                    }
                    if (right == a.Length) right = a.Length - nk;
                }
            }
            return a;
        }

        // Complete the stockmax function below.
      public static int stockmax(int[] prices)
        {
            if (prices.Length == 0) return 0;

            int[] cache = new int[prices.Length+1];
            for (int i = 0; i < prices.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (prices[j] < prices[i])
                        cache[i+1] = Math.Max(cache[i+1],prices[i] - prices[j] + cache[j]);
                }
            }
            return cache[prices.Length];
        }

        public static int Reverse(int x)
        {
            bool IsNegative = x < 0 ? true : false;

            if (IsNegative) x = -x;

            int rev = 0;
            while (x > 0)
            {
                try
                {
                    checked
                    {
                        rev = rev * 10 + (x % 10);
                        x /= 10;
                    }
                }
                catch (System.OverflowException e)
                {
                    rev = 0;
                }
            }
            return IsNegative ? -rev : rev;
        }

        public static int LengthOfLongestSubstring(string s)
        {
            int prev = 0;
            int max = 0;
            Dictionary<char, int> map = new Dictionary<char, int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (map.ContainsKey(s[i]))
                {
                    if (i - prev > max)
                        max = i - prev;
                    prev = Math.Max(prev, map[s[i]] + 1);                    
                    map[s[i]] = i;
                }
                else
                    map.Add(s[i], i);
            }

            return Math.Max(max, s.Length - prev);
        }

        public static int OpenLock(string[] deadends, string target)
        {
            HashSet<string> deadendsSet = new HashSet<string>();
            HashSet<string> visited = new HashSet<string>();
            Queue<string> queue = new Queue<string>();
            bool noresult = false;
            int level = 0;

            foreach (string str in deadends)
                deadendsSet.Add(str);

            queue.Enqueue("0000");
            visited.Add("0000");

            while (!noresult)
            {
                string front = queue.Dequeue();
                int count = queue.Count;
                for (int i = 0; i < front.Length; i++)
                {
                    string end1 = front.Substring(0, i) + ((Convert.ToInt32(front[i].ToString()) + 1) % 10) + front.Substring(i + 1, front.Length - i - 1);
                    Console.WriteLine(end1);
                    if (end1.CompareTo(target) == 0)
                        break;
                    if (!visited.Contains(end1) && !deadendsSet.Contains(end1))
                    {
                        visited.Add(end1);
                        queue.Enqueue(end1);
                    }


                    end1 = front.Substring(0, i) + ((10 + (Convert.ToInt32(front[i].ToString()) - 1)) % 10) + front.Substring(i + 1, front.Length - i - 1);
                    Console.WriteLine(end1);
                    if (end1.CompareTo(target) == 0)
                        break;
                    if (!visited.Contains(end1) && !deadendsSet.Contains(end1))
                    {
                        visited.Add(end1);
                        queue.Enqueue(end1);
                    }
                }
                if (count == queue.Count)
                    noresult = true;
            }

            if (noresult)
                return -1;

            return level;
        }

        public static string DecodeString(string s)
        {
            string result = s;
            Stack<int> bracketsStk = new Stack<int>();
            StringBuilder sb = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (s[i] == ']')
                    bracketsStk.Push(i);
                else if (Char.IsDigit(s[i]))
                {
                    int j = i - 1;
                    while (j >= 0 && Char.IsDigit(s[j]))
                        j--;
                    if (j != 0) j += 1;
                    int k = Convert.ToInt32(s.Substring(j, i - j + 1));
                    string pat = s.Substring(i + 2, bracketsStk.Pop() - (i + 2));
                    sb.Clear();
                    for (int x = 0; x < k; x++)
                        sb.Append(pat);
                    result = result.Replace(k + "[" + pat + "]", sb.ToString());
                    i = j;
                }
            }
            return result;
        }

       public static bool[] squaresUnderQueenAttack(int n, int[][] queens, int[][] queries)
        {
            bool[] result = new bool[queries.Length];
            /*for(int i=0;i<queries.Length;i++) {        
                for(int j=0;j<queens.Length;j++) {
                    if(CanAttack(queries[i][0],queries[i][1],
                                 queens[j][0],queens[j][1])) {
                        result[i] = true;
                        break;
                    }
                }
            }*/
            Dictionary<int, HashSet<int>> lookUp = new Dictionary<int, HashSet<int>>();
            for (int q = 0; q < queens.Length; q++)
            {
                int r_q = queens[q][0];
                int c_q = queens[q][1];
                //move up
                for (int i = r_q - 1; i >= 0; i--)
                {
                    if (lookUp.ContainsKey(i) && lookUp[i].Contains(c_q))
                        break;
                    if (!lookUp.ContainsKey(i))
                        lookUp.Add(i, new HashSet<int>());
                    lookUp[i].Add(c_q);
                }
                //move down
                for (int i = r_q + 1; i < n; i++)
                {
                    if (lookUp.ContainsKey(i) && lookUp[i].Contains(c_q))
                        break;
                    if (!lookUp.ContainsKey(i))
                        lookUp.Add(i, new HashSet<int>());
                    lookUp[i].Add(c_q);
                }
                //move left
                for (int i = c_q - 1; i >= 0; i--)
                {
                    if (lookUp.ContainsKey(r_q) && lookUp[r_q].Contains(i))
                        break;
                    if (!lookUp.ContainsKey(r_q))
                        lookUp.Add(r_q, new HashSet<int>());
                    lookUp[r_q].Add(i);
                }
                //move right
                for (int i = c_q + 1; i < n; i++)
                {
                    if (lookUp.ContainsKey(r_q) && lookUp[r_q].Contains(i))
                        break;
                    if (!lookUp.ContainsKey(r_q))
                        lookUp.Add(r_q, new HashSet<int>());
                    lookUp[r_q].Add(i);
                }
                //move topleft
                for (int i = r_q - 1, j = c_q - 1; i >= 0 && j >= 0; i--, j--)
                {
                    if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                        break;
                    if (!lookUp.ContainsKey(i))
                        lookUp.Add(i, new HashSet<int>());
                    lookUp[i].Add(j);
                }
                //move bottomright
                for (int i = r_q + 1, j = c_q + 1; i < n && j < n; i++, j++)
                {
                    if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                        break;
                    if (!lookUp.ContainsKey(i))
                        lookUp.Add(i, new HashSet<int>());
                    lookUp[i].Add(j);
                }
                //move topright
                for (int i = r_q - 1, j = c_q + 1; i >= 0 && j < n; i--, j++)
                {
                    if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                        break;
                    if (!lookUp.ContainsKey(i))
                        lookUp.Add(i, new HashSet<int>());
                    lookUp[i].Add(j);
                }
                //move bottomleft
                for (int i = r_q + 1, j = c_q - 1; i < n && j >= 0; i++, j--)
                {
                    if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                        break;
                    if (!lookUp.ContainsKey(i))
                        lookUp.Add(i, new HashSet<int>());
                    lookUp[i].Add(j);
                }
            }

            return result;
        }
        bool CanAttack(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) == Math.Abs(y1 - y2) || y1 == y2 || x1 == x2;
        }

        // Complete the queensAttack function below.
        public static int queensAttack(int n, int k, int r_q, int c_q, int[][] obstacles)
        {
            int count = 0;
            Dictionary<int, HashSet<int>> lookUp = new Dictionary<int, HashSet<int>>();
            for (int i = 0; i < obstacles.Length; i++)
            {
                if (!lookUp.ContainsKey(obstacles[i][0]))
                    lookUp.Add(obstacles[i][0], new HashSet<int>());
                lookUp[obstacles[i][0]].Add(obstacles[i][1]);
            }
            //move up
            for (int i = r_q + 1; i <= n; i++)
            {
                if (lookUp.ContainsKey(i) && lookUp[i].Contains(c_q))
                break;
                count++;
            }
            //move down
            for (int i = r_q - 1; i >= 1; i--)
            {
                if (lookUp.ContainsKey(i) && lookUp[i].Contains(c_q))
                break;
                count++;
            }
            //move left
            for (int i = c_q - 1; i >= 1; i--)
            {
                if (lookUp.ContainsKey(r_q) && lookUp[r_q].Contains(i))
                break;
                count++;
            }
            //move right
            for (int i = c_q + 1; i <= n; i++)
            {
                if (lookUp.ContainsKey(r_q) && lookUp[r_q].Contains(i))
                break;
                count++;
            }
            //move lefttop diagonal
            for (int i = r_q + 1, j = c_q - 1; i <= n && j >= 1; i++, j--)
            {
                if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                break;
                count++;
            }
            //move bottomright diagonal
            for (int i = r_q - 1, j = c_q + 1; i >= 1 && j <= n; i--, j++)
            {
                if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                break;
                count++;
            }
            //move bottomleft diagonal
            for (int i = r_q - 1, j = c_q - 1; i >= 1 && j >= 1; i--, j--)
            {
                if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                break;
                count++;
            }
            //move topright diagonal
            for (int i = r_q + 1, j = c_q + 1; i <= n && j <= n; i++, j++)
            {
                if (lookUp.ContainsKey(i) && lookUp[i].Contains(j))
                break;
                count++;
            }            
            return count;
        }

      public static double escapeMaze(string[] grid,
            Dictionary<string, Tuple<int, int>> tunnels)
        {
            int i = 0;
            int index = 0;
            for (; i < grid.Length; i++)
            {
                index = grid[i].IndexOf('A');
                if (index >= 0)
                    break;
            }
            Dictionary<string,int> visited = new Dictionary<string, int>();
            int ways = escapeMaze(i, index, grid, tunnels, visited);
            return Convert.ToDouble(ways)/4;
        }

      public  static int escapeMaze(int i, int j, string[] grid,
                    Dictionary<string, Tuple<int, int>> tunnels, Dictionary<string, int> visited)
        {
            visited.Add(i + "-" + j,-1);
            if (grid[i][j] == '#' || grid[i][j] == '*')
                return 0;
            if (grid[i][j] == '%')
                return 1;
            int ways = 0;
            for (int col = Math.Max(0, j - 1); col <= Math.Min(j + 1, grid[i].Length - 1); col++)
            {
                if (col != j)
                {
                    if (!visited.ContainsKey(i + "-" + col))
                    {
                        if (tunnels.ContainsKey(i + "-" + col))
                            ways += escapeMaze(tunnels[i + "-" + col].Item1, tunnels[i + "-" + col].Item2, grid, tunnels, visited);
                        else
                            ways += escapeMaze(i, col, grid, tunnels, visited);
                    }
                    else if(visited[i + "-" + col] != -1)
                        ways += visited[i + "-" + col];

                }
            }
            for (int row = Math.Max(0, i - 1); row <= Math.Min(i + 1, grid.Length - 1); row++)
            {
                if (row != i)   
                {
                    if (!visited.ContainsKey(row + "-" + j))
                    {
                        if (tunnels.ContainsKey(row + "-" + j))
                            ways += escapeMaze(tunnels[row + "-" + j].Item1, tunnels[row + "-" + j].Item2, grid, tunnels, visited);
                        else 
                            ways += escapeMaze(row, j, grid, tunnels, visited);
                    }
                    else if (visited[row + "-" + j] != -1)
                        ways += visited[row + "-" + j];
                }
            }
            visited[i + "-" + j] = ways;
            return ways;
        }

        public static int GetCountOfPairs(int[] a, int k)
        {
            int count = 0;
            HashSet<int> map = new HashSet<int>();
            foreach (int num in a) {
                
            }
            return count;
        }

       public static string lexicographicallyGreater(string s)
        {
            NState state = new NState();
            state.str = s;
            lexicographicallyGreater(s.ToCharArray(), s, state, 0, s.Length - 1);
            return state.str == s ? "no answer" : state.str;
        }
       public static void lexicographicallyGreater(char[] str, string original, NState state, int lo, int hi)
        {
            if (lo == hi)
            {
                int res = string.Compare(new string(str),original, StringComparison.Ordinal);
                if (res > 0 && res < state.max)
                {
                    state.max = res;
                    state.str = new string(str);
                }
                return;
            }
            for (int i = lo; i <= hi; i++)
            {
                char c = str[lo];
                str[lo] = str[i];
                str[i] = c;
                lexicographicallyGreater(str, original, state, lo + 1, hi);
                c = str[lo];
                str[lo] = str[i];
                str[i] = c;
            }
        }

        public static string lcs(int[] seq1, int[] seq2)
        {
            int[][] cache = new int[seq1.Length + 1][];
            cache[0] = new int[seq2.Length + 1];
            Position[][] solution = new Position[seq1.Length + 1][];
            solution[0] = new Position[seq2.Length + 1];

            for (int i = 1; i < cache.Length; i++)
            {
                cache[i] = new int[seq2.Length + 1];
                solution[i] = new Position[seq2.Length + 1];
                for (int j = 1; j < cache[i].Length; j++)
                {
                    if (seq1[i - 1] == seq2[j - 1])
                    {
                        cache[i][j] = cache[i - 1][j - 1] + 1;
                        solution[i][j] = new Position() { row = i, col = j };
                    }
                    else
                    {
                        if (cache[i][j - 1] > cache[i - 1][j])
                        {
                            cache[i][j] = cache[i][j - 1];
                            solution[i][j] = new Position() { row = i, col = j - 1 };
                        }
                        else
                        {
                            cache[i][j] = cache[i - 1][j];
                            solution[i][j] = new Position() { row = i - 1, col = j };
                        }
                    }
                }
            }
            for (int i = 1; i < solution.Length; i++)
            {
                for (int j = 1; j < solution[i].Length; j++)
                {
                    Console.Write(cache[i][j] + "$" + solution[i][j].row + ":" + solution[i][j].col + "\t");
                }
                Console.WriteLine(string.Empty);
            }

            Position origin = solution[seq1.Length][seq2.Length];
            Console.WriteLine(origin.row + "/" + origin.col);
            List<int> result = new List<int>();

            while (origin != null && origin.row > 0 && origin.col > 0)
            {
                if (seq1[origin.row - 1] == seq2[origin.col - 1])
                {
                    result.Add(seq1[origin.row - 1]);
                    origin = solution[origin.row - 1][origin.col - 1];
                }
                else
                {
                    if (cache[origin.row][origin.col - 1] >
                       cache[origin.row - 1][origin.col])
                    {
                        origin = solution[origin.row][origin.col - 1];
                    }
                    else
                        origin = solution[origin.row - 1][origin.col];
                }
            }

            String res = string.Empty;
            for (int i = result.Count - 1; i >= 0; i--)
                res += result[i] + " ";

            return res.Trim();
        }
        public static int[] leftRotate(int[] a,int rotations)
        {
            int d = rotations % a.Length;
            if (d == 0)
                return a;

            int i = 0;
            int newindex = 0;
            int backup = 0;            
            int newVal = a[0];
            while (i < a.Length)
            {
                newindex = ((newindex - d) + a.Length) % a.Length;
                Console.WriteLine(newindex);
                backup = a[newindex];
                a[newindex] = newVal;
                newVal = backup;
                i++;
            }
            return a;
        }
        public static IList<int> FindMinHeightTrees(int n, int[,] edges)
        {
            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                if (!graph.ContainsKey(edges[i, 0]))
                    graph.Add(edges[i, 0], new List<int>());
                graph[edges[i, 0]].Add(edges[i, 1]);
                if (!graph.ContainsKey(edges[i, 1]))
                    graph.Add(edges[i, 1], new List<int>());
                graph[edges[i, 1]].Add(edges[i, 0]);
            }

            State1 result = new State1();
            int height = dfs(graph, 0, -1, new Dictionary<int, int>(), 0, result);
            return result.Roots;
        }
        public static int dfs(Dictionary<int, List<int>> graph, int u, int parent, Dictionary<int, int> visited, int level, State1 state)
        {

            visited.Add(u, 0);

            foreach (int v in graph[u])
            {
                if (!visited.ContainsKey(v))
                    visited[u] = Math.Max(visited[u], dfs(graph, v, u, visited, level + 1, state));
                else if (v != parent)
                    visited[u] = Math.Max(visited[u], visited[v]);
            }

            if(visited[u] > 0)
            {
                if (level+visited[u] < state.MinHeight)
                {
                    state.MinHeight = level + visited[u];
                    state.Roots.Clear();
                    state.Roots.Add(u);
                }
                else if (Math.Max(level, visited[u]) == state.MinHeight)
                    state.Roots.Add(u);
            }

            return visited[u] + 1;
        }
        // Complete the getWays function below.
        public static long getWays(long n, long[] c)
        {
            long result = getWays(n, c, c.Length-1);
            Console.WriteLine(result);
            return result;
        }
       public static long getWays(long n, long[] c, int index)
        {
            if (n == 0)
                return 1;
            if (n < 0)
                return 0;
            if (index == -1)
                return 0;

            long sumSoFar = n;
            long noOfWays = 0;

            while (sumSoFar > 0)
            {
                noOfWays += getWays(sumSoFar, c, index-1);
                sumSoFar -= c[index];
            }
            return noOfWays;
        }
        public static int MissingNumber(int[] nums)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                if (nums[i] < nums.Length)
                {
                    int end = nums[i];
                    while (i != end && end < nums.Length)
                    {
                        int temp = nums[end];
                        nums[end] = end;
                        end = temp;
                    }
                    if (i == end)
                        nums[i] = i;
                }
            }
            int m = 0;
            while (nums[m] == m) m++;
            return m;
        }


       public static long bricksGameHelper(int[] arr, int index, Dictionary<int, long> cache)
        {
            if (index >= arr.Length)
                return 0;
            if (cache.ContainsKey(index))
                return cache[index];
            int sumSoFar = 0;
            long max = 0;
            for (int i = index; i < Math.Min(index + 3, arr.Length); i++)
            {
                sumSoFar += arr[i];
                max = Math.Max(max, sumSoFar + bricksGameHelper(arr, i + 4, cache));
            }
            cache.Add(index, max);
            return max;
        }
        public static long bricksGame(int[] arr)
        {
            Tuple<long, long> result = helper(arr, 0, true, new Dictionary<string, Tuple<long, long>>());
            return result.Item1;
        }
        public static Tuple<long, long> helper(int[] arr, int index, bool firstPlayer, Dictionary<string, Tuple<long, long>> cache)
        {
            if (index == arr.Length)
                return new Tuple<long, long>(0, 0);
            if (cache.ContainsKey(firstPlayer.ToString() + ":" + index))
                return cache[firstPlayer.ToString() + ":" + index];
            long max1 = 0L, max2 = 0L;
            Tuple<long, long> result;
            if (firstPlayer)
            {
                max2 = Int64.MaxValue;                
                if (index + 1 <= arr.Length)
                {
                    result = helper(arr, index + 1, !firstPlayer, cache);
                    max1 = Math.Max(max1, arr[index] + result.Item1);
                    max2 = Math.Min(max2, result.Item2);
                }
                if (index + 2 <= arr.Length)
                {
                    result = helper(arr, index + 2, !firstPlayer, cache);
                    max1 = Math.Max(max1, arr[index] + arr[index + 1] + result.Item1);
                    max2 = Math.Min(max2, result.Item2);
                }
                if (index + 3 <= arr.Length)
                {
                    result = helper(arr, index + 3, !firstPlayer, cache);
                    max1 = Math.Max(max1, arr[index] + arr[index + 1] + arr[index + 2] + result.Item1);
                    max2 = Math.Min(max2, result.Item2);
                }
            }
            else
            {
                max1 = Int64.MaxValue;
                if (index + 1 <= arr.Length)
                {
                    result = helper(arr, index + 1, !firstPlayer, cache);
                    max2 = Math.Max(max2, arr[index] + result.Item2);
                    max1 = Math.Min(max1, result.Item1);
                }
                if (index + 2 <= arr.Length)
                {
                    result = helper(arr, index + 2, !firstPlayer, cache);
                    max2 = Math.Max(max2, arr[index] + arr[index + 1] + result.Item2);
                    max1 = Math.Min(max1, result.Item1);
                }
                if (index + 3 <= arr.Length)
                {
                    result = helper(arr, index + 3, !firstPlayer, cache);
                    max2 = Math.Max(max2, arr[index] + arr[index + 1] + arr[index + 2] + result.Item2);
                    max1 = Math.Min(max1, result.Item1);
                }
            }
            cache.Add(firstPlayer.ToString() + ":" + index, new Tuple<long, long>(max1, max2));
            return cache[firstPlayer.ToString() + ":" + index];
        }
        public static int GetKthElement(int[] arr1,int[] arr2,int k)
        {
            int result = 0;
            int m1 = (arr1.Length)/ 2;
            int m2 = (arr2.Length)/ 2;
            int pos = m1 + m2 + 1;
            int lo1 = 0, lo2 = 0;
            while(pos != k - 1)
            {
                if (arr1[m1] <= arr2[m2])
                    m1 = lo1 + ((m1 - lo1) / 2);
                else
                    m2 = lo2 + ((m2 - lo2) / 2);
                pos = m1 + m2 + 2;
            }
            return result;
        }
        public static int UniquePaths(int m, int n)
        {
            int[][] cache = new int[n][];
            for (int i = 0; i < n; i++)
                cache[i] = new int[m];
            for (int i = 1; i < m; i++)
                cache[0][i] = 1;
            for (int i = 1; i < n; i++)
                cache[i][0] = 1;
            for (int i = 1; i < m; i++)
            {
                for (int j = 1; j < n; j++)
                {
                    Console.WriteLine("-------------");
                    Console.WriteLine("row:" + j + " col:" + i);
                    Console.WriteLine("aboverow:" + (j - 1) + " col:" + i);
                    Console.WriteLine("row:" + j + " leftcol:" + (i - 1));
                    cache[j][i] = cache[j - 1][i] + cache[j][i - 1];
                }
            }
            return cache[n - 1][m - 1];
        }
        public static int playWithWords(string s)
        {
            /*
             * Write your code here.
             */

            if (string.IsNullOrEmpty(s))
                return 0;

            int max = 0;
            int[][] cache = new int[s.Length][];
            for (int i = 0; i < cache.Length; i++)
            {
                cache[i] = new int[s.Length];
                cache[i][i] = 1;
            }
            for (int i = s.Length - 1; i >= 0; i--)
            {
                for (int j = i + 1; j < cache[i].Length; j++)
                {
                    if (s[i] == s[j])
                        cache[i][j] = cache[i + 1][j - 1] + 2;
                    else
                    {
                        cache[i][j] = Math.Max(cache[i + 1][j], cache[i][j - 1]);
                    }
                }
            }
            for (int i = 0; i < cache[0].Length; i++)
            {
                int lmax = 1;
                if (i - 1 >= 0)
                    lmax = cache[0][i - 1];
                if (i + 1 < cache.Length)
                    lmax *= cache[i][cache[0].Length - 1];
                max = Math.Max(max, lmax);
            }
            return max;
        }
        public static IList<IList<int>> ThreeSum(int[] nums)
        {
            if (nums.Length <= 2) return null;
            List<IList<int>> results = new List<IList<int>>();
            for (int i = 0; i < nums.Length; i++)
            {
                int[] twoSum = TwoSum(nums, -nums[i], i + 1);
                if (twoSum != default(int[]))
                {
                    List<int> result = new List<int>();

                    result.Add(nums[i]);
                    result.Add(twoSum[0]);
                    result.Add(twoSum[1]);

                    results.Add(result);
                }
            }
            return results;
        }
        public static int[] TwoSum(int[] nums, int target, int left)
        {            
            int[] result = default(int[]);
            HashSet<int> cache = new HashSet<int>();
            for (int i = left; i < nums.Length; i++)
            {
                if (cache.Contains(target - nums[i]))
                {
                    result = new int[2];
                    result[0] = target - nums[i];
                    result[1] = nums[i];
                    break;
                }
                if (!cache.Contains(nums[i]))
                    cache.Add(nums[i]);
            }
            return result;
        }
        public static IList<int> SpiralOrder(int[,] matrix)
        {
            if (matrix == null) return null;
            List<int> result = new List<int>();
            int sr = 0, er = matrix.GetLength(0) - 1;
            int sc = 0, ec = matrix.GetLength(1) - 1;
            while (sr <= er && sc <= ec)
            {
                for (int tc = sc; tc <= ec; tc++)
                    result.Add(matrix[sr, tc]);
                sr++;
                for (int tr = sr; tr <= er; tr++)
                    result.Add(matrix[tr, ec]);
                ec--; 
                if(sr <= er)
                {
                    for (int tc = ec; tc >= sc; tc--)
                        result.Add(matrix[er, tc]);
                    er--;
                }
                if(sc <= ec)
                {
                    for (int tr = er; tr >= sr; tr--)
                        result.Add(matrix[tr, sc]);
                    sc++;
                }                
            }
            return result;
        }
        public static int MaxSumSubmatrix(int[,] matrix, int k)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[] temp = default(int[]);
            int globalMax = Int32.MinValue;
            for (int left = 0; left < cols && globalMax < k; left++)
            {
                temp = new int[rows];
                for (int right = left; right < cols && globalMax < k; right++)
                {
                    for (int i = 0; i < rows; i++)
                        temp[i] += matrix[i, right];
                    int localMax = SubArraySumWithUpperBound(temp, k);
                    globalMax = Math.Max(localMax, globalMax);
                }
            }
            return globalMax;
        }

        public static string Multiply(string num1, string num2)
        {
            string result = string.Empty;
            int carry = 0, mul = 0;
            for (int i = num2.Length - 1; i >= 0; i--)
            {
                carry = 0;
                string temp = String.Empty;
                for (int j = num1.Length - 1; j >= 0; j--)
                {
                    carry += (num2[i] - '0') * (num1[j] - '0');
                    temp = (carry % 10) + temp;
                    carry /= 10;
                }
                if (carry > 0)
                    temp = carry + temp;
                for (int c = 0; c < mul; c++)
                    temp += "0";
                mul++;
                result = Add(result, temp);
            }
            return result;
        }
        public static string Add(string num1, string num2)
        {
            if (string.IsNullOrEmpty(num1))
                return num2;
            if (string.IsNullOrEmpty(num2))
                return num1;
            string result = string.Empty;
            int carry = 0, n1 = num1.Length - 1, n2 = num2.Length - 1;
            while (carry > 0 || n1 >= 0 || n2 >= 0)
            {
                carry += (n1 < 0) ? 0 : (num1[n1--] - '0');
                carry += (n2 < 0) ? 0 : (num2[n2--] - '0');
                result = (carry % 10) + result;
                carry /= 10;
            }
            return result;
        }
        private static int SubArraySumWithUpperBound(int[] arr, int k)
        {
            if (arr == default(int[]))
                return 0;
            int sum = 0, max = Int32.MinValue; ;
            SortedSet<int> set = new SortedSet<int>();
            set.Add(0);
            for (int i = 0; i < arr.Length && max < k; i++)
            {
                sum += arr[i];
                foreach (int x in set.Where(y => y <= k - sum))
                    max = Math.Max(max, sum - x);
                if (!set.Contains(sum))
                    set.Add(sum);
            }
            return max;
        }
        public static int MySqrt(int x)
        {            
            if (x == 0 || x == 1)
                return x;
            int sqrt = 1;
            int lo = 1, hi = x;
            while (lo <= hi)
            {
                int mid = lo + ((hi - lo) / 2);
                int temp = x / mid;
                if (mid == temp)
                {
                    sqrt = mid;
                    break;
                }
                if (temp < mid)
                {
                    sqrt = mid-1;
                    hi = mid-1;
                }
                else
                    lo = mid+1;
            }
            return sqrt;
        }
        public static void Main(string[] args)
        {
            int n1 = 'Z' - 'z';

            int ixc = 49;
            char xc = (char)ixc;
            int test = 2147483647;
            string s1 = "AB";
            string s2 = "ab";
            int c = String.CompareOrdinal(s1,s2);
            List<int>[] lists = new List<int>[5];
            SortedSet<int> nbSet = new SortedSet<int>();
            for(int ix = 0; ix < 5; ix++)
            {
                nbSet.Add(ix);
            }
            SortedSet<int> subSet = nbSet.GetViewBetween(1, 3);
            char kx = (char)('a' + 25);
            int x = 1 + 2 % 2;
            int cv= 2147395599;
            MySqrt(6);
            Multiply("9", "9");
            SortedSet<int> set1 = new SortedSet<int>();
            set1.Add(0);
            int i = set1.GetViewBetween(0, set1.Max).Min;
            IEnumerable<int> last = set1.Where(xy => xy > -1);
           
            int n0;
            if (last.Count() > 0)
                 n0 = last.First();
            MaxSumSubmatrix(new int[3, 4] { { 5, -4, -3, 4 }, { -3, -4, 4, 5 }, { 5, 1, 5, -4 } },3);
            SpiralOrder(new int[3, 4] { { 1, 2, 3, 4 }, { 5, 6, 7, 8 }, { 9, 10, 11, 12 } });
            int[,] spiral = new int[5, 3];

            for(int r = 0; r < spiral.GetLength(0); r++)
            {
                for(int j = 0;j < spiral.GetLength(1); j++)
                {
                    spiral[r, j] = r + j;
                }

            }
            Multiply("99","9");
            ThreeSum(new int[6] { -1, 0, 1, 2, -1, -4 });
            playWithWords("dbcbcbededadecbcdecbaeadcecada");
            string str = "abca";
            HashSet<string> set = new HashSet<string>();
            int count = 0;
            for(int v = 0; v < str.Length; v++)
            {
                for(int j= v; j < str.Length; j++)
                {
                    if (!set.Contains(str.Substring(v, (j - v)+1)))
                    {
                        count++;
                        set.Add(str.Substring(v, (j - v) + 1));
                    }
                }
            }
            UniquePaths(3, 2);
            bricksGame(new int[] { 0, 1 ,1, 1, 999 });
            MissingNumber(new int[] { 3, 0, 2 });
            
          
            
            getWays(4, new long[] { 1, 2, 3 });
            FindMinHeightTrees(5,new int[4, 2] { { 0, 1 }, { 0, 2 }, { 0, 3 }, { 3, 4 } });          
            string xa = Convert.ToString(x, 2);
            leftRotate(new int[] { 98, 67, 35, 1, 74, 79, 7, 26, 54, 63, 24, 17, 32, 81 }, 7);
            lcs(new int[] { 1, 2, 3, 4, 1 }, new int[] {3,4,1,2,1,3});
            lexicographicallyGreater("hefg");
            string a = "hefg";
            string b = "efgh";
            int res = String.CompareOrdinal(a, b);
            GetCountOfPairs(new int[] {1,3,2,6,1,2}, 3);
            escapeMaze(new string[] { "##OOO##", "%OOAOO*", "##OOO##" }, new Dictionary<string, Tuple<int, int>>());
            squaresUnderQueenAttack(5, new int[][] { new int[] { 1, 1 }, new int[] { 3, 2 } },new int[][] { new int[] { 1, 1 }, new int[] { 0, 3 }, new int[] { 0, 4 } ,new int[] {3,4},new int[] {2,0},new int[] {4,3},new int[] { 4,0}});
            queensAttack(4, 0, 4, 4, new int[0][]);
            Regex.IsMatch("3[a2[c]]", @"\d");
            DecodeString("3[a2[c]]");
            OpenLock(new string[] { "0201", "0101", "0102", "1212", "2002" }, "0202");
            LengthOfLongestSubstring("abba");
            Reverse(123);
            stockmax(new int[] { 1, 3, 1, 2 });
            circularArrayRotation(new int[] { 5,3,4 }, 3);
            palindromeIndex("hgygsvlfcwnswtuhmyaljkqlqjjqlqkjlaymhutwsnwcwflvsgygh");

            if(1 == 1.0)
            {
                Console.WriteLine("hi");
            }
            beautifulBinaryString("1110011110001100010100000011011101100001101010001111101101000010111111001110110000010110010011100010");
            treeBottom("(2 (7 (2 () ()) (6 (5 () ()) (11 () ()))) (5 () (9 (4 () ()) ())))");
            textJustification(new string[] { "This", "is", "an", "example", "of", "text", "justification." },16);
            alternatingCharacters("ABBABBAA");
            classifyStrings("aeu");
            findFirstSubstringOccurrence("sst", "st");
            abbreviation("lyylyyllyyylllyylyyylyllylyllllllyyylyllyyyylllllylyylyyllylyylllyhyllllyylllyllylyllylyllllyylylylyyylyllyyylylllyylylllyyllyylylyyllyylyyylllyllylyyllyllllyylylyylllllllyllyyyyyylllyyylylylylyyyyyyyymylyyyylyyyylyyyylyyyylylylylylyllylyylllyllyylylyyllyyyylylllyyyyyllllllyllyylllylyylyllllyyllllylyyyyyllllylylllyyyylyylyyyllyylyyyylylyyyylyyyyylyylllyyllylyyllyllyyyyyylylllylyyyyyllyylyyyyylyyylyylyylylylyyllllyylllyylylllyllyylylyllylllyllyyyyyylyyyllyllyyllyllyylyllyllyyylyyyyylylllyyylllyyyllylyllylylyyllylllylyyyyylyyyylyyyylyyyyylylllllyylyylyyyylyylyyylyylllllllyyyyyyyylyyylylllllylyrlyylllyylylllllylyylyylyyllylyyyyllyyyllylllyllylllylyyyyylylylyyllyyyyylllyyyllllylyllyyyllllyyllyyylllylyylyyyllllyllylllylyllylllyyllllyllyyymyylylllyylllllllyyyyylyyyllyyyyyyylylylyylylyylylyyllyyyllylylyyyylyyyyyyyyyyylllylylllllylllyylllyyllllllyylllllyllyyllyylyyllllyylyylyyllllyyyllllyyylylylylyylyllylllyyylylylylyyylyllllllylyllllyylyllylllyllyylylllylllyllllylyyylylllyyylllyylllllllyllyyy",
                "LYYLYYLYYYLLLYYLYYYYLLYLLLLLLYYYLYLLYYYYLLLLYLYLYYLLYLYLLYYLLLYYLLLLYLYLLYLYLLLLYYLYLLYYLYLLYLLLLYLYLLYLYYLLYYLLYYLYYYLLLYLYLYYLLYLLYYYLYLLLLLLLYLLYYYYYLLLYYYLYLYLYLYYYYYYYLYYYLYYYYLYYLYYYYLYYLYLYLYLLYLYYLLLYLLYYLYLYLLYYYLYLLLYYYYLLLLLLYLLYYLLLYLYYLYLLLLYLLLYLYYYYYLLLLLYLLLYYYYLYYLYYLLYYLYYYYLYLYYYYLYYYYYLYYLLLYYLLYLYLLYLLYYYYYLYLLYLYYYYYLLYYLYYYYLYYYLYYLYYLYLYLYYLLLLYYLLLYYLYLLLYLLYLYLYLLYLLLYLLYYYYYYLYYYLLYLYYLLYLLYLYLLYLLYYYLYYYYLLLLYYYLLLYYYLLYLYLLYLYLYYLLYLLLYLYYYYYLYYYYLYYYYLYYYYYLYLLLLLYYLYYLYYYLYYYYYLYYLLLLLLLYYYYYYYYLYYLYLLLLYLYLYYLLYYLYLLLLLYLYYLYYLYLLYLYYYLYYYLYLLLYLLYLLYLYYYYYLLYYYLLYYYYYLLYYYLLLLYLYLLYYYLLLLYYLLYYYLLLYLYYLYYYLLLYLLYLYLYLLYLLYYLLLYLLYYYYYLYLLLYYLLLLLLLYYYYYLYYLLYYYYYYLYYLLYYLYLYYLLYYYLLYYLYYYYLYYYYYYYYYYYLLLYYLLLLLYLLLYYLLLYYLLLLLYYLLLLYLYYLLYYLYYLLLYYLYYLYYLLLLYYYLLLLYYYYLYLYLYYYLLYLLLYYYLYLYLYLYYLYLLLLLYLYLLLYYYLLYLYLLYYLYLLYLLLYLLYLYYYLYLLLYYLLYYLLLLLLYLYY");
            candies(new int[] { 50,2,51,63,65,30,92,49,83,37,167,74});

//            //candies(16387, new int[] { 59801,2225
//,51489
//,63693
//,65074
//,30389
//,92493
//,49135
//,83523
//,37766
//,16728
//,74433
//,64881
//,4280
//,93171
//,91649
//,85163
//,61161
//,47441
//,47265
//,17338
//,85194
//,51669
//,17817
//,41112
//,45456
//,10451
//,57276
//,40707
//,15801
//,79337
//,44392
//,7505
//,89319
//,42883
//,74846
//,366
//,95095
//,13153
//,27507
//,94963
//,23744
//,66363
//,54633
//,70285
//,65185
//,94133
//,90621
//,59241
//,76588
//,86984
//,4781
//,93233
//,80135
//,60550
//,12433
//,4309
//,93925
//,80065
//,88114
//,74897
//,28046
//,99377
//,48137
//,31009
//,89912
//,5094
//,47602
//,74427
//,27
//,15601
//,31073
//,46329
//,37459
//,90921
//,70305
//,2760
//,5541
//,41631
//,76631
//,27509
//,62481
//,3341
//,42069
//,15166
//,3623
//,47147
//,52199
//,75105
//,25007
//,74570
//,53455
//,37768
//,45909
//,36921
//,22701
//,92773
//,64645
//,57011
//,623
//,70796
//,99311
//,92381
//,53165
//,61451
//,41889
//,80241
//,92273
//,78331
//,90017
//,27301
//,32427
//,34185
//,50548
//,36899
//,2889
//,95041
//,99007
//,50129
//,95056
//,18361
//,97993
//,63141
//,65569
//,74644
//,55853
//,7786
//,32643
//,21806
//,84365
//,34843
//,30377
//,51817
//,43361
//,28402
//,42747
//,11847
//,91849
//,63826
//,64579
//,35323
//,90701
//,47472
//,92001
//,26629
//,78146
//,75865
//,38261
//,86241
//,98281
//,78926
//,92153
//,66421
//,78914
//,49903
//,49509
//,94656
//,57025
//,20961
//,54110
//,55201
//,18620
//,15918
//,71642
//,35053
//,15665
//,81045
//,43525
//,26571
//,72748
//,74549
//,53991
//,67421
//,10481
//,15137
//,96065
//,74380
//,49881
//,23173
//,85781
//,72904
//,89020
//,1635
//,55275
//,79971
//,94861
//,2240
//,25312
//,23933
//,44479
//,43608
//,46871
//,16275
//,2657
//,99557
//,10941
//,25801
//,93783
//,62161
//,69937
//,99083
//,59861
//,43235
//,33441
//,8768
//,40739
//,21681
//,75616
//,28481
//,25280
//,40016
//,27873
//,85927
//,94241
//,87059
//,91013
//,22702
//,5701
//,4526
//,65441
//,14830
//,76753
//,21473
//,35761
//,76672
//,63481
//,73329
//,43605
//,6435
//,65991
//,36090
//,58267
//,96075
//,64695
//,82681
//,55161
//,42751
//,14201
//,2133
//,97989
//,8414
//,4429
//,47223
//,34979
//,897
//,10309
//,28001
//,98113
//,20930
//,68281
//,7207
//,77277
//,80815
//,20751
//,15029
//,68422
//,72313
//,23707
//,30062
//,26001
//,60381
//,69861
//,50284
//,81651
//,92615
//,41871
//,53311
//,68561
//,23927
//,60717
//,78529
//,43169
//,66669
//,29927
//,15249
//,95219
//,23121
//,8301
//,68986
//,11293
//,68559
//,17241
//,55063
//,51725
//,16413
//,86851
//,71449
//,82197
//,12837
//,55465
//,98738
//,43766
//,22569
//,61385
//,26203
//,27741
//,88640
//,85368
//,70125
//,56479
//,33818
//,90427
//,25926
//,5675
//,60817
//,81634
//,34077
//,49205
//,33538
//,12114
//,96451
//,77824
//,60486
//,14466
//,12279
//,98325
//,25591
//,29076
//,32953
//,771
//,28185
//,7135
//,82161
//,64073
//,43273
//,55383
//,19751
//,86401
//,11165
//,84506
//,82441
//,77926
//,37872
//,95990
//,85026
//,92665
//,65399
//,68287
//,87209
//,36498
//,19571
//,1347
//,40343
//,23549
//,43001
//,19993
//,5026
//,93083
//,41753
//,89579
//,55825
//,36077
//,36607
//,86663
//,92431
//,74781
//,24381
//,24669
//,9731
//,80871
//,71566
//,46609
//,91334
//,82129
//,5236
//,9079
//,60125
//,30769
//,55949
//,3235
//,27717
//,48747
//,67229
//,8061
//,20231
//,81513
//,27980
//,23533
//,65513
//,85921
//,28537
//,18861
//,54506
//,26281
//,11437
//,1489
//,82105
//,45271
//,23801
//,97748
//,92021
//,66663
//,36397
//,53760
//,72477
//,91328
//,82119
//,50701
//,50263
//,24274
//,46252
//,41809
//,48794
//,47361
//,94245
//,9044
//,70701
//,52015
//,95455
//,83287
//,74149
//,79601
//,31326
//,18785
//,57947
//,27141
//,79471
//,83399
//,67693
//,88471
//,52049
//,65527
//,89528
//,28131
//,17001
//,77492
//,25366
//,36147
//,83029
//,17760
//,23576
//,41057
//,79601
//,75931
//,86160
//,17837
//,39131
//,55129
//,61841
//,46257
//,78319
//,28629
//,73415
//,4945
//,28983
//,2361
//,87164
//,41500
//,39542
//,83973
//,68901
//,33073
//,20254
//,20651
//,28073
//,423
//,84835
//,38181
//,32252
//,79245
//,99192
//,20474
//,45271
//,95122
//,64393
//,8791
//,45830
//,74275
//,16481
//,59484
//,56176
//,734
//,48213
//,95637
//,59846
//,32335
//,26293
//,13901
//,32441
//,62301
//,98305
//,20826
//,45880
//,75268
//,56501
//,4441
//,91321
//,14217
//,87937
//,39441
//,24175
//,23191
//,47169
//,22350
//,1281
//,69776
//,21081
//,91647
//,47042
//,29285
//,15935
//,77846
//,45630
//,25234
//,75375
//,17868
//,3721
//,29025
//,65631
//,34734
//,80433
//,44187
//,87581
//,58716
//,40447
//,84282
//,79457
//,72300
//,60726
//,29095
//,45749
//,12057
//,38005
//,47918
//,97307
//,64601
//,85105
//,9869
//,93608
//,89622
//,3657
//,59663
//,27297
//,62431
//,33865
//,34859
//,6063
//,4796
//,69453
//,67746
//,35489
//,54701
//,37365
//,97705
//,18838
//,11142
//,24032
//,98453
//,3249
//,36876
//,74981
//,1375
//,628
//,28112
//,9257
//,15682
//,74026
//,60395
//,53809
//,36282
//,21989
//,11185
//,93755
//,97726
//,78396
//,34681
//,46076
//,77241
//,89056
//,29949
//,24821
//,59665
//,54199
//,36651
//,95353
//,62032
//,1829
//,99735
//,66129
//,49516
//,85471
//,81923
//,29325
//,74433
//,42641
//,95738
//,6755
//,45297
//,79384
//,47207
//,28385
//,77231
//,45897
//,8536
//,36593
//,76559
//,38503
//,20801
//,82564
//,33549
//,71289
//,18008
//,54618
//,97086
//,43316
//,81481
//,83765
//,23241
//,164
//,78745
//,4688
//,32920
//,64481
//,95267
//,5985
//,13081
//,44590
//,86401
//,37701
//,25081
//,25936
//,71953
//,67736
//,99731
//,48565
//,83497
//,10481
//,54646
//,54642
//,89976
//,93617
//,28316
//,12545
//,44011
//,69311
//,95837
//,3992
//,85195
//,79197
//,21679
//,40752
//,55289
//,93225
//,42001
//,56013
//,41356
//,28416
//,52291
//,35981
//,33716
//,66242
//,23201
//,40451
//,89921
//,88181
//,22220
//,17748
//,59341
//,77849
//,84416
//,84496
//,5045
//,34271
//,64751
//,84704
//,95841
//,1055
//,51937
//,53269
//,6793
//,21451
//,62817
//,93501
//,73839
//,6843
//,27428
//,83474
//,67843
//,84983
//,85369
//,5366
//,36700
//,83441
//,12134
//,96577
//,38085
//,4101
//,23435
//,71701
//,58581
//,41915
//,22157
//,35329
//,60275
//,89666
//,58161
//,93306
//,21516
//,79561
//,21717
//,63467
//,37193
//,50801
//,39993
//,81973
//,86080
//,10826
//,20897
//,68508
//,74833
//,80285
//,26943
//,64931
//,20225
//,81193
//,42628
//,29232
//,79199
//,8989
//,25921
//,52561
//,83169
//,69257
//,89454
//,13789
//,34857
//,46181
//,46273
//,74333
//,12109
//,75838
//,1561
//,11942
//,18119
//,24874
//,55516
//,39626
//,45853
//,77147
//,29820
//,72468
//,63139
//,62069
//,53551
//,22624
//,76121
//,79445
//,38847
//,32793
//,82321
//,93746
//,3501
//,73297
//,63306
//,76143
//,10561
//,66640
//,94449
//,28417
//,44159
//,76415
//,22189
//,99363
//,68141
//,57765
//,92481
//,73051
//,83537
//,29895
//,98297
//,40075
//,12919
//,21268
//,32756
//,43505
//,74283
//,1419
//,17129
//,67541
//,67566
//,51087
//,27161
//,83820
//,89700
//,49349
//,14259
//,94068
//,11141
//,3780
//,17643
//,24605
//,75939
//,46593
//,64558
//,64611
//,95963
//,527
//,29193
//,77597
//,31481
//,93801
//,93577
//,80061
//,45812
//,54851
//,27851
//,71721
//,66521
//,23949
//,91761
//,43796
//,28466
//,26594
//,56162
//,37037
//,10000
//,78999
//,28381
//,64176
//,75392
//,28685
//,54001
//,16609
//,77436
//,70527
//,24082
//,74315
//,2465
//,94929
//,52455
//,18883
//,51581
//,11757
//,48423
//,38651
//,10251
//,89044
//,58601
//,28817
//,12257
//,17401
//,40898
//,85790
//,35745
//,7840
//,26801
//,74279
//,75693
//,27798
//,55633
//,52361
//,95132
//,64861
//,57398
//,45273
//,14519
//,60074
//,44001
//,98369
//,83569
//,10969
//,87781
//,51406
//,65613
//,22193
//,5708
//,97466
//,79225
//,50585
//,5841
//,3651
//,54691
//,51809
//,11855
//,37499
//,98101
//,99476
//,5577
//,61201
//,97801
//,70921
//,99227
//,15478
//,38817
//,79137
//,52449
//,90576
//,78217
//,57665
//,99491
//,57421
//,12013
//,48753
//,88641
//,52511
//,50795
//,99335
//,11823
//,25586
//,86287
//,48495
//,22423
//,78274
//,99951
//,66847
//,71888
//,86289
//,69535
//,52687
//,25261
//,8717
//,6401
//,71657
//,81327
//,84493
//,62903
//,99435
//,57873
//,58085
//,55080
//,91221
//,67157
//,51219
//,58851
//,51014
//,34273
//,39941
//,23753
//,60634
//,26011
//,59739
//,46365
//,83653
//,28889
//,5915
//,80071
//,25614
//,89377
//,51942
//,81585
//,30069
//,98361
//,65441
//,7891
//,52461
//,20440
//,87621
//,16589
//,38215
//,72165
//,28693
//,14571
//,33537
//,60237
//,31587
//,40851
//,86873
//,29733
//,29523
//,83985
//,46061
//,59201
//,7681
//,12339
//,75781
//,87193
//,25905
//,76199
//,96596
//,72295
//,97408
//,46041
//,65736
//,78029
//,55662
//,61473
//,63146
//,92520
//,11077
//,84713
//,78586
//,91301
//,38801
//,72366
//,17697
//,9266
//,3691
//,69711
//,82177
//,91801
//,57146
//,19530
//,93800
//,11151
//,39791
//,63693
//,26574
//,44341
//,6258
//,7917
//,12756
//,83289
//,41397
//,74967
//,45881
//,51631
//,22083
//,46913
//,63733
//,7308
//,68329
//,3841
//,22150
//,80721
//,38623
//,89191
//,42801
//,60707
//,86357
//,29660
//,42069
//,74749
//,20401
//,39849
//,84193
//,13697
//,97741
//,13977
//,70026
//,20429
//,87300
//,42719
//,13655
//,29723
//,22623
//,3277
//,56947
//,17926
//,8795
//,58515
//,19450
//,37269
//,2911
//,59700
//,9847
//,24736
//,32385
//,96581
//,98757
//,90497
//,84415
//,1433
//,20519
//,72851
//,12161
//,92820
//,29971
//,91597
//,11611
//,21365
//,51779
//,10853
//,11769
//,82411
//,8353
//,18541
//,70705
//,70681
//,87586
//,85893
//,22801
//,40641
//,25368
//,46301
//,33305
//,5541
//,40113
//,38776
//,56151
//,73321
//,18001
//,94657
//,79161
//,62785
//,69774
//,14176
//,78233
//,2129
//,47201
//,30817
//,26511
//,19541
//,97917
//,7249
//,59551
//,97585
//,71748
//,14731
//,34053
//,34101
//,28584
//,62913
//,82840
//,55073
//,49886
//,30751
//,83172
//,62339
//,48631
//,43863
//,97481
//,33467
//,6365
//,40941
//,94697
//,75195
//,16529
//,64615
//,98241
//,60780
//,51414
//,88741
//,37681
//,55138
//,79079
//,21412
//,8909
//,10245
//,13827
//,95255
//,10248
//,76
//,81376
//,76970
//,99033
//,52517
//,18562
//,35361
//,40019
//,91360
//,96205
//,26145
//,73827
//,54320
//,30001
//,70809
//,83027
//,56506
//,56868
//,19899
//,93688
//,25013
//,9525
//,69032
//,75755
//,65111
//,77006
//,25263
//,57201
//,72939
//,32901
//,2931
//,9117
//,19609
//,55691
//,79297
//,34713
//,28377
//,52358
//,27991
//,59513
//,10157
//,12025
//,76739
//,46011
//,6073
//,28893
//,48092
//,43863
//,35918
//,60433
//,58491
//,73461
//,25806
//,45991
//,56093
//,87977
//,67745
//,84769
//,25662
//,6924
//,61261
//,95595
//,80229
//,92579
//,92117
//,77681
//,91609
//,83930
//,19071
//,72593
//,72822
//,38615
//,61361
//,13541
//,59390
//,14484
//,2335
//,23417
//,24487
//,68714
//,88801
//,84193
//,39051
//,22722
//,87029
//,14233
//,80965
//,42077
//,69609
//,69711
//,76474
//,69801
//,82239
//,97532
//,87229
//,13761
//,23375
//,85961
//,99627
//,93541
//,83772
//,87817
//,15586
//,72449
//,82329
//,47880
//,77901
//,19737
//,44613
//,7689
//,29501
//,17307
//,68186
//,22886
//,64429
//,97433
//,56447
//,87397
//,18231
//,26139
//,97046
//,42801
//,43225
//,41507
//,55011
//,51369
//,39906
//,64591
//,588
//,72385
//,76113
//,58913
//,83893
//,70033
//,27861
//,49266
//,45931
//,28969
//,45711
//,81521
//,71161
//,30448
//,84642
//,70876
//,87645
//,48623
//,11326
//,85973
//,10838
//,9072
//,9661
//,15561
//,62561
//,12521
//,7389
//,8582
//,44163
//,82497
//,59809
//,97205
//,11408
//,86919
//,29649
//,22883
//,54785
//,90345
//,73759
//,87886
//,29419
//,34887
//,52641
//,95996
//,881
//,32096
//,80718
//,35243
//,74891
//,24745
//,62911
//,32961
//,78919
//,734
//,12421
//,72321
//,13131
//,31534
//,2701
//,70703
//,75669
//,31043
//,20121
//,50984
//,99259
//,61371
//,19213
//,84600
//,45849
//,18597
//,97401
//,50129
//,73094
//,94939
//,86445
//,55401
//,95840
//,39921
//,86893
//,32517
//,91570
//,84677
//,6325
//,35895
//,19057
//,11516
//,17216
//,98894
//,18051
//,35225
//,17054
//,50429
//,15601
//,44831
//,37413
//,36577
//,87909
//,94259
//,78385
//,88999
//,95355
//,46231
//,1349
//,82826
//,18733
//,62721
//,70761
//,87401
//,83545
//,65589
//,77107
//,27361
//,80497
//,68945
//,86161
//,54661
//,52951
//,13583
//,9163
//,50041
//,87505
//,53759
//,16256
//,69909
//,55103
//,68685
//,97950
//,55521
//,52180
//,2771
//,14601
//,12089
//,10411
//,24895
//,99273
//,10197
//,41398
//,12911
//,55762
//,23821
//,90257
//,40923
//,64956
//,65185
//,18911
//,39057
//,4477
//,85265
//,12231
//,85237
//,85705
//,63607
//,27254
//,58015
//,59831
//,85608
//,60343
//,42465
//,88413
//,34240
//,59609
//,23831
//,12384
//,22056
//,12769
//,86601
//,99245
//,81955
//,85320
//,78795
//,16539
//,33561
//,18756
//,19397
//,22665
//,21028
//,36376
//,93302
//,92214
//,52180
//,4272
//,16229
//,50264
//,78774
//,62885
//,10811
//,2086
//,3001
//,90699
//,72649
//,24121
//,51121
//,61283
//,13647
//,63414
//,4423
//,37407
//,15057
//,85099
//,48387
//,86035
//,96477
//,18741
//,18077
//,38381
//,88785
//,27019
//,85690
//,51636
//,16195
//,2977
//,30615
//,72128
//,10485
//,18801
//,57796
//,96696
//,22859
//,10474
//,98061
//,78215
//,75205
//,36491
//,26533
//,70874
//,37105
//,10253
//,95041
//,64042
//,84200
//,75441
//,98329
//,92825
//,18285
//,84554
//,78937
//,68230
//,40254
//,21601
//,42395
//,31185
//,69557
//,10907
//,8085
//,5409
//,92751
//,14073
//,98661
//,11841
//,77603
//,57761
//,17811
//,32849
//,66234
//,37322
//,73978
//,30385
//,37619
//,36388
//,98548
//,10364
//,41279
//,6562
//,27107
//,31481
//,69521
//,53488
//,16852
//,71381
//,6986
//,66081
//,41224
//,65526
//,68499
//,55123
//,90219
//,24299
//,17346
//,28671
//,97102
//,48525
//,57901
//,79118
//,98405
//,31799
//,97598
//,28287
//,53726
//,74308
//,56011
//,42676
//,2354
//,31027
//,18001
//,55199
//,4768
//,69146
//,89511
//,14893
//,53567
//,81863
//,56831
//,86709
//,27576
//,73237
//,94136
//,33533
//,79084
//,35053
//,2825
//,96003
//,86359
//,55745
//,21247
//,53025
//,55577
//,78135
//,54761
//,51061
//,63375
//,6591
//,12465
//,66301
//,47974
//,10418
//,64861
//,62561
//,16729
//,49299
//,31088
//,36139
//,88822
//,72193
//,85065
//,54235
//,38449
//,80335
//,55526
//,20301
//,70973
//,34683
//,48561
//,47727
//,71291
//,81297
//,2730
//,61358
//,89585
//,40161
//,27927
//,43703
//,13751
//,66561
//,5219
//,55927
//,95151
//,52198
//,3232
//,84805
//,66055
//,26231
//,94113
//,91075
//,52977
//,35057
//,91833
//,46643
//,27209
//,16583
//,17163
//,26293
//,52052
//,84409
//,30921
//,32829
//,32405
//,65593
//,5277
//,14020
//,90273
//,70801
//,71125
//,17606
//,62404
//,54563
//,27785
//,85051
//,26126
//,65
//,20675
//,94421
//,39441
//,20491
//,62769
//,78381
//,92601
//,63084
//,81480
//,58313
//,95401
//,43553
//,58959
//,25053
//,51691
//,48929
//,98745
//,23049
//,78013
//,40846
//,61093
//,68850
//,61298
//,50611
//,14298
//,61119
//,57513
//,5090
//,42451
//,91031
//,65001
//,13337
//,14105
//,23547
//,86852
//,2107
//,47484
//,64988
//,61126
//,92041
//,17441
//,89424
//,89788
//,84305
//,54901
//,39089
//,45516
//,57701
//,76642
//,82201
//,8943
//,17689
//,65850
//,3713
//,83737
//,73633
//,34200
//,88478
//,97993
//,24037
//,81513
//,72459
//,67219
//,19261
//,29647
//,49551
//,37385
//,40257
//,22286
//,10489
//,32433
//,54401
//,3201
//,5302
//,34642
//,31303
//,70271
//,70851
//,66721
//,81323
//,88377
//,4181
//,14317
//,94867
//,26135
//,46426
//,95729
//,7231
//,65921
//,70385
//,19441
//,41761
//,65961
//,21618
//,12201
//,73841
//,51421
//,30331
//,75917
//,64001
//,97529
//,72321
//,20761
//,29797
//,78501
//,7920
//,26155
//,23963
//,65678
//,52257
//,29752
//,54841
//,43943
//,90586
//,31452
//,27813
//,84696
//,77639
//,25778
//,42366
//,218
//,62898
//,7425
//,12825
//,48045
//,60191
//,38599
//,50023
//,68239
//,24717
//,61057
//,82011
//,75229
//,26591
//,27063
//,75931
//,37113
//,41
//,81180
//,17637
//,69761
//,58051
//,18950
//,3701
//,30169
//,34020
//,7736
//,51597
//,71256
//,24966
//,39420
//,86875
//,48974
//,15665
//,62401
//,97633
//,65183
//,12501
//,4229
//,32856
//,84301
//,79781
//,47633
//,52317
//,39781
//,62706
//,5426
//,60999
//,46711
//,49085
//,76512
//,69537
//,49273
//,80938
//,40436
//,94783
//,68324
//,10542
//,11540
//,41321
//,82225
//,22519
//,13743
//,34081
//,34801
//,56026
//,48737
//,31581
//,48959
//,83481
//,81477
//,18243
//,4845
//,64436
//,47577
//,65903
//,66381
//,5181
//,50837
//,46700
//,55396
//,57009
//,6437
//,49324
//,8625
//,84724
//,63316
//,80319
//,23906
//,23895
//,29873
//,48411
//,68920
//,89591
//,87079
//,49706
//,90119
//,28400
//,67687
//,59268
//,27331
//,30006
//,94401
//,25123
//,7681
//,56521
//,84813
//,18395
//,28366
//,72177
//,73381
//,78371
//,24597
//,47601
//,86128
//,27086
//,1503
//,69171
//,56492
//,53445
//,40779
//,46243
//,31147
//,91001
//,81263
//,13191
//,46671
//,88957
//,48136
//,77550
//,92433
//,79827
//,74487
//,82337
//,3407
//,42589
//,13313
//,1068
//,38093
//,58049
//,19751
//,67911
//,1977
//,62593
//,52709
//,26809
//,29113
//,39883
//,85717
//,7457
//,49057
//,97277
//,32801
//,19053
//,35601
//,5119
//,30761
//,1226
//,98365
//,47711
//,32906
//,5698
//,49143
//,67866
//,53791
//,96351
//,55791
//,97207
//,85105
//,15794
//,51937
//,74573
//,95110
//,26513
//,22371
//,44200
//,2776
//,61091
//,12180
//,10169
//,81262
//,71131
//,83357
//,45377
//,40079
//,31505
//,89201
//,40378
//,30651
//,13371
//,27176
//,90348
//,43733
//,1965
//,66379
//,27829
//,9787
//,29036
//,41841
//,85621
//,29300
//,5933
//,66159
//,71650
//,47916
//,14916
//,59889
//,5122
//,55314
//,32513
//,66706
//,38785
//,57593
//,67541
//,12689
//,79241
//,26471
//,47608
//,22169
//,15041
//,10136
//,36527
//,77673
//,42590
//,17665
//,6073
//,40663
//,69441
//,28209
//,79175
//,4733
//,29997
//,88611
//,72567
//,56984
//,59001
//,20059
//,99307
//,7772
//,12683
//,30105
//,87529
//,84352
//,74161
//,55136
//,5581
//,49376
//,54626
//,1585
//,22247
//,24165
//,98745
//,40913
//,69157
//,15621
//,86806
//,12305
//,79482
//,1970
//,66237
//,82047
//,6285
//,9305
//,2721
//,61734
//,58301
//,63925
//,74018
//,72934
//,57461
//,47886
//,87147
//,75221
//,21515
//,82808
//,33201
//,5081
//,31510
//,6070
//,93501
//,57156
//,1265
//,62799
//,63001
//,12741
//,50575
//,38678
//,45871
//,26511
//,73301
//,74270
//,99635
//,52291
//,60225
//,96711
//,33713
//,62977
//,11029
//,9905
//,68625
//,78001
//,39487
//,65721
//,68881
//,17405
//,29301
//,72008
//,53376
//,57044
//,62488
//,65191
//,12493
//,32070
//,75473
//,20321
//,8006
//,36297
//,6629
//,79249
//,24945
//,10666
//,95341
//,68114
//,89827
//,29765
//,62522
//,97303
//,80941
//,93273
//,22066
//,5127
//,21063
//,88455
//,51826
//,95056
//,80161
//,96273
//,28815
//,43745
//,39217
//,32326
//,26679
//,68272
//,1431
//,21576
//,31327
//,63476
//,73445
//,68269
//,10019
//,33485
//,36034
//,13271
//,9348
//,6401
//,35907
//,80061
//,45481
//,5346
//,41029
//,83161
//,98861
//,94461
//,76733
//,22501
//,9550
//,18376
//,86447
//,91578
//,13758
//,16271
//,53841
//,34381
//,9401
//,75959
//,46992
//,7571
//,90392
//,79594
//,88801
//,87645
//,21705
//,82513
//,9825
//,78620
//,80981
//,8059
//,29403
//,53213
//,24320
//,19969
//,5348
//,91773
//,67899
//,32961
//,26529
//,12956
//,45181
//,32611
//,96297
//,13637
//,54401
//,38129
//,9661
//,7909
//,56423
//,9389
//,77578
//,35715
//,15259
//,62194
//,44767
//,77366
//,91769
//,83696
//,31781
//,6879
//,16386
//,47791
//,69385
//,90747
//,10873
//,77926
//,28563
//,84568
//,15571
//,96562
//,4105
//,40857
//,70288
//,92621
//,87169
//,22157
//,51457
//,78444
//,72341
//,38401
//,49310
//,95285
//,71878
//,80540
//,77609
//,78689
//,96717
//,94591
//,5229
//,50675
//,97592
//,47825
//,28233
//,73241
//,55971
//,9221
//,40097
//,20071
//,24101
//,63605
//,28593
//,74676
//,9976
//,30305
//,76429
//,29490
//,9741
//,40011
//,7302
//,88295
//,45601
//,5223
//,48001
//,9621
//,4485
//,19777
//,52937
//,57553
//,70031
//,8121
//,86275
//,65965
//,98582
//,79921
//,2745
//,37178
//,99803
//,98081
//,51861
//,8021
//,25600
//,17107
//,33921
//,67158
//,55281
//,15625
//,104
//,37847
//,99339
//,44809
//,58927
//,20677
//,2173
//,67249
//,51391
//,2317
//,43037
//,4595
//,46217
//,88977
//,39345
//,83625
//,72173
//,72681
//,25526
//,41473
//,99771
//,52951
//,15513
//,64233
//,72385
//,60801
//,70947
//,53201
//,32865
//,39681
//,36285
//,85317
//,97118
//,82929
//,43461
//,87641
//,20148
//,51393
//,4103
//,76251
//,78173
//,87679
//,52206
//,53706
//,17689
//,67577
//,21899
//,16299
//,41777
//,29257
//,75437
//,15531
//,32033
//,99651
//,32381
//,15877
//,25443
//,34985
//,10121
//,8828
//,93582
//,6977
//,99181
//,70685
//,8165
//,89435
//,12881
//,95193
//,17079
//,98789
//,74675
//,51689
//,59157
//,69991
//,26401
//,30433
//,37267
//,43825
//,72787
//,45406
//,50973
//,64785
//,78281
//,15535
//,18673
//,94301
//,16943
//,78691
//,8781
//,60801
//,44021
//,91505
//,16555
//,6393
//,20513
//,38409
//,85021
//,59077
//,55104
//,56535
//,61039
//,3939
//,50541
//,34981
//,87115
//,42601
//,16231
//,20771
//,45121
//,30751
//,77428
//,62545
//,63168
//,95921
//,32826
//,34745
//,96736
//,30852
//,52673
//,61933
//,67127
//,50231
//,62181
//,27809
//,21517
//,537
//,3871
//,29685
//,96341
//,34707
//,80981
//,32411
//,39221
//,8735
//,56543
//,71381
//,31717
//,9461
//,10286
//,51281
//,74193
//,87185
//,9900
//,42248
//,83055
//,61975
//,17165
//,88515
//,91661
//,3357
//,50913
//,80251
//,38759
//,83984
//,54266
//,18109
//,23582
//,48973
//,2225
//,95626
//,64320
//,73452
//,34331
//,89871
//,48867
//,24526
//,22599
//,36671
//,47009
//,19737
//,1805
//,37577
//,49536
//,7204
//,4625
//,65801
//,66601
//,14626
//,72501
//,78281
//,53546
//,7601
//,236
//,65453
//,80107
//,90557
//,25127
//,37907
//,4541
//,9712
//,39161
//,61281
//,27411
//,33105
//,88020
//,4093
//,84914
//,83981
//,91851
//,89582
//,97984
//,166
//,71772
//,71811
//,53537
//,5449
//,41069
//,41829
//,4161
//,73167
//,69324
//,25343
//,99221
//,2356
//,18157
//,34076
//,83783
//,86154
//,18645
//,4859
//,97633
//,46493
//,49251
//,52409
//,28871
//,68759
//,94594
//,71129
//,89291
//,74601
//,30801
//,50621
//,46425
//,65987
//,8716
//,3063
//,15981
//,56493
//,71897
//,37761
//,75396
//,26481
//,37166
//,19969
//,72001
//,17173
//,7745
//,53953
//,57761
//,24149
//,85009
//,79417
//,81947
//,85851
//,92857
//,68065
//,36955
//,28331
//,82789
//,56109
//,23005
//,17693
//,106
//,42700
//,78793
//,82605
//,33005
//,36035
//,59125
//,73969
//,8216
//,83561
//,50581
//,23098
//,10622
//,68467
//,64939
//,36873
//,12953
//,22810
//,11465
//,74779
//,70173
//,23937
//,41151
//,78913
//,99548
//,30501
//,27253
//,61801
//,41289
//,31261
//,27673
//,28141
//,35631
//,50889
//,90625
//,31817
//,89993
//,27531
//,8601
//,83729
//,1338
//,65237
//,46177
//,6004
//,23917
//,50441
//,85973
//,83327
//,29113
//,79221
//,12371
//,38248
//,70486
//,34683
//,49487
//,27861
//,50785
//,33596
//,1001
//,18317
//,9327
//,63496
//,73201
//,46265
//,19179
//,35096
//,45202
//,17026
//,34066
//,23145
//,91867
//,36698
//,18592
//,22795
//,84123
//,18525
//,53513
//,58781
//,93305
//,62306
//,55301
//,33233
//,29276
//,69548
//,16466
//,74971
//,59136
//,66516
//,31521
//,87201
//,68905
//,5533
//,92073
//,21301
//,99225
//,17632
//,87351
//,17678
//,14192
//,97627
//,11981
//,62842
//,44456
//,30791
//,68319
//,81386
//,77960
//,60255
//,35378
//,65963
//,1049
//,56529
//,57886
//,10621
//,10581
//,42369
//,99166
//,44462
//,19779
//,85665
//,42153
//,71521
//,57686
//,59339
//,62261
//,38649
//,75465
//,64863
//,75189
//,50103
//,46959
//,85551
//,59281
//,95351
//,24777
//,76823
//,99476
//,71074
//,34353
//,2569
//,45953
//,25001
//,77781
//,59161
//,79399
//,34273
//,98946
//,79
//,6609
//,85001
//,35208
//,32806
//,33607
//,21905
//,76057
//,39017
//,12245
//,53429
//,93899
//,11446
//,93380
//,21571
//,94687
//,5271
//,50637
//,22083
//,37249
//,63491
//,9831
//,705
//,55227
//,43353
//,30757
//,50821
//,61725
//,79329
//,37800
//,26509
//,77767
//,65843
//,58791
//,61055
//,8482
//,52999
//,13505
//,62515
//,85077
//,72261
//,46657
//,69071
//,98109
//,47577
//,14816
//,68731
//,64363
//,95966
//,4949
//,28621
//,71420
//,26726
//,57576
//,74449
//,68643
//,54252
//,27432
//,35297
//,63553
//,18281
//,72553
//,22776
//,5499
//,44097
//,12821
//,71737
//,75303
//,31275
//,97850
//,29385
//,56629
//,14792
//,72097
//,3319
//,71463
//,49265
//,16897
//,49709
//,67831
//,43393
//,22401
//,69357
//,65137
//,5932
//,40495
//,56341
//,71932
//,88233
//,9126
//,24505
//,3881
//,71293
//,39805
//,60993
//,67197
//,67326
//,98135
//,7602
//,97276
//,85333
//,21726
//,87251
//,23811
//,2009
//,19673
//,99822
//,68741
//,30437
//,77581
//,401
//,98861
//,40691
//,28875
//,4409
//,43201
//,14441
//,96651
//,81523
//,74315
//,60383
//,41358
//,65761
//,1675
//,27412
//,39349
//,68562
//,33943
//,40976
//,8649
//,60357
//,53889
//,9895
//,89706
//,33111
//,68321
//,28277
//,54551
//,84314
//,75528
//,52993
//,95198
//,17676
//,42390
//,65085
//,41441
//,31616
//,89107
//,15137
//,17821
//,81441
//,66153
//,998
//,20173
//,89041
//,21797
//,16613
//,18670
//,87525
//,69664
//,16345
//,55220
//,10418
//,64721
//,74271
//,57833
//,78703
//,40484
//,17760
//,27267
//,53279
//,19412
//,40093
//,53953
//,4129
//,82016
//,30682
//,31309
//,10161
//,41845
//,16225
//,60081
//,9013
//,14738
//,56011
//,74001
//,78476
//,54961
//,17325
//,47650
//,57561
//,50317
//,23777
//,22961
//,22096
//,17114
//,18591
//,45477
//,72431
//,1456
//,67121
//,95708
//,47851
//,85441
//,68036
//,22657
//,80321
//,20881
//,15185
//,34473
//,67165
//,6287
//,48899
//,47041
//,13851
//,30672
//,79901
//,71818
//,98379
//,8317
//,5672
//,92385
//,16941
//,83028
//,23135
//,96161
//,2837
//,20981
//,68028
//,28379
//,94582
//,12661
//,73921
//,53615
//,52705
//,61216
//,23709
//,67701
//,15251
//,31617
//,70291
//,58081
//,72341
//,70347
//,81576
//,68147
//,81401
//,85145
//,46101
//,80541
//,59110
//,33484
//,87861
//,81259
//,79941
//,42401
//,30661
//,80801
//,15210
//,4353
//,99701
//,35633
//,34401
//,71361
//,17427
//,97334
//,44300
//,76421
//,70118
//,69683
//,66737
//,49033
//,18991
//,29239
//,26281
//,79777
//,5489
//,4215
//,6704
//,51643
//,88856
//,37301
//,62017
//,78177
//,2810
//,69601
//,93901
//,40332
//,20289
//,13396
//,10630
//,41215
//,81177
//,21841
//,66779
//,48329
//,24251
//,94696
//,97345
//,18205
//,41813
//,21209
//,33951
//,32614
//,22319
//,54401
//,34055
//,66467
//,85001
//,19085
//,35706
//,77585
//,68261
//,19436
//,22913
//,36793
//,76801
//,92750
//,63612
//,25446
//,93941
//,94335
//,43321
//,47673
//,17116
//,57101
//,18960
//,27749
//,99489
//,64426
//,4205
//,17069
//,33281
//,8590
//,80503
//,73001
//,85883
//,66089
//,31701
//,51557
//,41010
//,60365
//,26724
//,97537
//,62941
//,95422
//,46313
//,23542
//,16031
//,908
//,51265
//,52759
//,40361
//,50161
//,47231
//,60201
//,71341
//,45875
//,46081
//,83201
//,87231
//,40839
//,91515
//,18375
//,18897
//,60556
//,12128
//,10227
//,73375
//,71047
//,73351
//,83202
//,6056
//,67190
//,82187
//,69036
//,17431
//,44134
//,13501
//,78076
//,40201
//,49149
//,62546
//,93131
//,75113
//,65827
//,37425
//,87241
//,51306
//,26951
//,14651
//,52327
//,94776
//,57519
//,86971
//,75787
//,27101
//,83253
//,63561
//,58751
//,98471
//,71599
//,83761
//,41868
//,71760
//,77969
//,22463
//,82961
//,5821
//,7821
//,81985
//,15548
//,93321
//,63246
//,94357
//,77877
//,72641
//,43637
//,13601
//,63166
//,92358
//,52185
//,7441
//,87721
//,10065
//,8353
//,3146
//,51431
//,36721
//,68113
//,64260
//,48061
//,85001
//,40321
//,77537
//,35774
//,49107
//,52561
//,93385
//,75754
//,35281
//,62849
//,39469
//,48201
//,40083
//,89351
//,49257
//,86601
//,17073
//,74113
//,49233
//,32465
//,30401
//,35493
//,24218
//,86953
//,28014
//,33657
//,23664
//,70282
//,28944
//,76776
//,6255
//,64449
//,62771
//,6237
//,46713
//,23751
//,82161
//,74031
//,33701
//,79505
//,40945
//,26018
//,38135
//,59041
//,75221
//,36609
//,89021
//,14230
//,72201
//,41873
//,9333
//,55796
//,59066
//,80970
//,38821
//,74001
//,7839
//,82755
//,64193
//,56641
//,50061
//,91326
//,82666
//,33045
//,26281
//,74716
//,97977
//,92101
//,80005
//,48410
//,42759
//,6905
//,68933
//,88665
//,83041
//,43367
//,1718
//,23403
//,2161
//,1741
//,68177
//,44776
//,14511
//,46105
//,79459
//,50431
//,94544
//,25722
//,86281
//,87857
//,79475
//,13529
//,92475
//,37066
//,89001
//,9933
//,68589
//,65354
//,24113
//,12081
//,36025
//,50753
//,12806
//,55325
//,15698
//,5541
//,65525
//,5505
//,93421
//,56243
//,35661
//,71873
//,94316
//,99669
//,29417
//,22145
//,19875
//,54095
//,67083
//,19909
//,52710
//,17961
//,31696
//,45457
//,88001
//,76545
//,15469
//,504
//,68977
//,98461
//,6755
//,54699
//,1765
//,42279
//,85069
//,1365
//,18685
//,49795
//,1358
//,58609
//,2646
//,69771
//,45256
//,9965
//,60575
//,11926
//,72149
//,66831
//,64933
//,52741
//,12339
//,27216
//,27207
//,73629
//,59186
//,89719
//,37597
//,69611
//,43003
//,95527
//,87124
//,58001
//,5726
//,68144
//,82001
//,85776
//,80181
//,15265
//,96113
//,19766
//,3897
//,36806
//,76539
//,85441
//,52001
//,53306
//,88483
//,80453
//,80428
//,72951
//,12001
//,75681
//,99405
//,67290
//,97319
//,95151
//,84101
//,46890
//,20401
//,42941
//,81817
//,89969
//,48617
//,19875
//,88971
//,26409
//,76126
//,90641
//,32885
//,65832
//,4266
//,79807
//,10732
//,62601
//,66664
//,8160
//,24410
//,1806
//,39955
//,68601
//,34159
//,8090
//,87829
//,53337
//,33811
//,51656
//,50933
//,30225
//,41644
//,37933
//,86637
//,51569
//,70592
//,26839
//,87201
//,9421
//,20385
//,18401
//,7973
//,45757
//,71314
//,74081
//,32748
//,71021
//,65181
//,19336
//,80261
//,61337
//,93893
//,85321
//,44691
//,86425
//,37683
//,10929
//,86744
//,71609
//,79181
//,1583
//,47090
//,42729
//,261
//,7153
//,56826
//,79576
//,30305
//,98426
//,33161
//,22281
//,78705
//,35329
//,34413
//,12501
//,34262
//,69770
//,41283
//,12891
//,52929
//,53537
//,38467
//,38435
//,2273
//,15681
//,78014
//,28855
//,46867
//,29953
//,36687
//,88838
//,27876
//,4631
//,95151
//,81632
//,8859
//,96013
//,63673
//,36415
//,83679
//,94612
//,37401
//,49492
//,32861
//,64711
//,16812
//,21193
//,15321
//,74849
//,37031
//,10778
//,65904
//,39661
//,56351
//,84077
//,84943
//,19787
//,76874
//,85200
//,91028
//,94419
//,83451
//,99321
//,24145
//,86153
//,11291
//,32401
//,16330
//,30892
//,66109
//,52469
//,94401
//,41435
//,98753
//,18142
//,32432
//,88972
//,4981
//,45201
//,89332
//,82363
//,18491
//,50003
//,43841
//,38471
//,87201
//,80456
//,98501
//,44685
//,80091
//,13199
//,14763
//,4405
//,23830
//,50646
//,95010
//,26571
//,96147
//,68038
//,80593
//,48679
//,98301
//,49313
//,82475
//,38421
//,13922
//,47044
//,5688
//,21495
//,58221
//,80230
//,62449
//,18637
//,25221
//,277
//,16841
//,45621
//,24858
//,94525
//,86861
//,80825
//,2065
//,86201
//,24371
//,38513
//,63041
//,18036
//,63151
//,62296
//,29697
//,52676
//,16789
//,78703
//,77846
//,43763
//,29573
//,56555
//,80409
//,97936
//,87485
//,27663
//,77503
//,37681
//,16669
//,68969
//,5368
//,7569
//,54126
//,64717
//,95397
//,7345
//,51356
//,68521
//,65636
//,23876
//,72011
//,14084
//,94457
//,6429
//,78385
//,80081
//,7353
//,9594
//,60161
//,13893
//,56707
//,13816
//,21625
//,92177
//,60432
//,14368
//,5200
//,21953
//,65713
//,1204
//,68417
//,92717
//,92009
//,24686
//,9432
//,3795
//,56641
//,5271
//,33401
//,39821
//,64301
//,10603
//,7489
//,63252
//,68353
//,10998
//,56955
//,23779
//,78000
//,9681
//,9169
//,35617
//,76081
//,84989
//,1532
//,98603
//,58081
//,81876
//,34641
//,81467
//,13769
//,16361
//,88311
//,81307
//,62801
//,42861
//,77497
//,29089
//,59279
//,1288
//,17162
//,43881
//,42795
//,17075
//,44951
//,56416
//,35379
//,72073
//,45448
//,68335
//,38726
//,52797
//,84425
//,94473
//,23715
//,10461
//,36017
//,8126
//,97157
//,97103
//,38163
//,37425
//,29397
//,18045
//,98979
//,81377
//,534
//,85005
//,63319
//,55617
//,2445
//,15432
//,5301
//,50553
//,87887
//,12686
//,1191
//,80433
//,77405
//,35651
//,30784
//,76930
//,72145
//,69659
//,26873
//,96382
//,27857
//,89963
//,82051
//,54120
//,28851
//,17274
//,78417
//,36913
//,37873
//,19321
//,34193
//,88737
//,7005
//,67021
//,76879
//,47293
//,10973
//,9125
//,86497
//,83578
//,85141
//,39843
//,40833
//,93955
//,74211
//,17544
//,18461
//,29725
//,87907
//,46251
//,87566
//,62314
//,49731
//,15401
//,46121
//,13603
//,40785
//,69639
//,84957
//,961
//,27574
//,56767
//,75281
//,10749
//,79021
//,11282
//,61016
//,24669
//,39246
//,36917
//,70855
//,50125
//,6157
//,24126
//,19285
//,25509
//,91816
//,2072
//,63395
//,691
//,18228
//,12930
//,15471
//,20643
//,7949
//,76248
//,70946
//,95749
//,61881
//,76825
//,97483
//,56741
//,6137
//,11995
//,33001
//,20121
//,4001
//,22181
//,43997
//,87351
//,38025
//,7557
//,75297
//,93548
//,99876
//,34665
//,10209
//,23749
//,65007
//,54413
//,16525
//,95295
//,13853
//,26741
//,8581
//,8616
//,79447
//,86281
//,92395
//,91011
//,9586
//,97153
//,96926
//,76472
//,80652
//,68239
//,68479
//,91997
//,31296
//,69093
//,7735
//,6131
//,16081
//,78561
//,84429
//,27481
//,12001
//,35101
//,66245
//,61629
//,59505
//,43089
//,52637
//,90145
//,42883
//,69433
//,18142
//,68897
//,48781
//,22489
//,51303
//,49038
//,13113
//,45583
//,94471
//,44877
//,68809
//,2617
//,44737
//,7919
//,82031
//,59351
//,36285
//,23777
//,90062
//,19373
//,33240
//,34373
//,69251
//,49089
//,84508
//,40614
//,43501
//,60125
//,10343
//,95116
//,29418
//,1537
//,39905
//,3741
//,8801
//,49417
//,62897
//,55321
//,36995
//,20571
//,31909
//,13733
//,76221
//,90341
//,81926
//,8561
//,77405
//,36902
//,27351
//,25181
//,45288
//,9938
//,55161
//,19827
//,81751
//,62861
//,49765
//,42209
//,57792
//,78011
//,61226
//,42349
//,89937
//,70893
//,21601
//,45945
//,80637
//,1801
//,15561
//,23404
//,69738
//,14757
//,76620
//,15371
//,15903
//,27307
//,55777
//,22461
//,32989
//,74195
//,84983
//,99297
//,52987
//,30027
//,93186
//,23845
//,39018
//,28723
//,2414
//,92099
//,26987
//,12801
//,39705
//,15813
//,39177
//,53779
//,50927
//,95885
//,31025
//,94017
//,42076
//,95769
//,23981
//,11251
//,14937
//,40037
//,33155
//,94008
//,15901
//,8456
//,88641
//,40621
//,72251
//,69795
//,56831
//,97852
//,12485
//,89041
//,59728
//,88447
//,81821
//,30041
//,56177
//,58544
//,59561
//,48073
//,61217
//,98743
//,44016
//,32421
//,37501
//,90178
//,88802
//,51957
//,89931
//,2252
//,73729
//,76425
//,45306
//,29246
//,13169
//,46859
//,94041
//,88407
//,19937
//,51241
//,3489
//,78305
//,25513
//,15407
//,78031
//,33351
//,49921
//,60545
//,61841
//,96581
//,83441
//,8161
//,98961
//,95741
//,79553
//,92897
//,51813
//,23733
//,26861
//,51599
//,3801
//,23793
//,99409
//,10512
//,92055
//,83761
//,78673
//,98329
//,21041
//,67210
//,25361
//,24379
//,26873
//,61432
//,46543
//,71665
//,26161
//,23161
//,20371
//,48267
//,66305
//,21925
//,52945
//,41150
//,68065
//,13453
//,90189
//,34507
//,50635
//,18438
//,67821
//,83471
//,37366
//,47429
//,17825
//,8351
//,96865
//,37773
//,64889
//,24591
//,71691
//,98735
//,97317
//,29109
//,13068
//,85765
//,72261
//,3085
//,48553
//,68679
//,75181
//,73201
//,86757
//,33784
//,29837
//,50765
//,7120
//,82526
//,98841
//,91681
//,27305
//,66725
//,42301
//,24001
//,29661
//,32099
//,20947
//,17953
//,31917
//,24823
//,15103
//,47752
//,3234
//,95189
//,81650
//,44809
//,94581
//,36354
//,87469
//,5170
//,38935
//,14360
//,89708
//,48641
//,56298
//,30251
//,56409
//,76019
//,91557
//,62280
//,64799
//,63736
//,82993
//,32616
//,7059
//,67344
//,93921
//,88841
//,54033
//,86088
//,35806
//,11066
//,79370
//,6761
//,27555
//,99637
//,1888
//,57605
//,28241
//,29921
//,85500
//,1085
//,52877
//,12001
//,97001
//,6349
//,7581
//,13585
//,26391
//,24273
//,4289
//,88434
//,24533
//,47231
//,4759
//,88741
//,93411
//,68141
//,28445
//,61683
//,63334
//,43169
//,8756
//,91507
//,32472
//,64357
//,35185
//,96947
//,7943
//,90751
//,88729
//,2865
//,53801
//,84925
//,14214
//,81055
//,11861
//,48769
//,79942
//,96983
//,84751
//,99132
//,6701
//,47075
//,65374
//,99752
//,55077
//,87481
//,96384
//,41333
//,81626
//,70617
//,47199
//,34673
//,21655
//,11589
//,39756
//,22399
//,26178
//,15569
//,50066
//,96801
//,2401
//,97015
//,41922
//,75717
//,13697
//,40973
//,17753
//,89866
//,16787
//,41711
//,19745
//,2726
//,44795
//,43603
//,1490
//,41015
//,18039
//,3918
//,71041
//,56817
//,98731
//,31305
//,82653
//,21623
//,95149
//,30531
//,14176
//,35232
//,37060
//,59821
//,92926
//,9853
//,82459
//,14641
//,27701
//,91401
//,76265
//,10953
//,22200
//,44093
//,48489
//,97061
//,55529
//,35075
//,67745
//,9917
//,63791
//,79089
//,17881
//,1377
//,53825
//,38722
//,65757
//,54045
//,18153
//,13983
//,82563
//,24777
//,78211
//,95259
//,11681
//,43353
//,16841
//,68613
//,81020
//,45633
//,26777
//,73664
//,81665
//,69627
//,13101
//,84877
//,56655
//,83166
//,92394
//,35316
//,27371
//,30281
//,53957
//,37501
//,99711
//,28969
//,45355
//,94883
//,51623
//,84203
//,66187
//,64319
//,26363
//,26579
//,49680
//,95801
//,11423
//,92497
//,22641
//,29841
//,25669
//,97436
//,58215
//,99349
//,37428
//,14469
//,88193
//,23119
//,23297
//,385
//,53947
//,18929
//,3861
//,93378
//,32687
//,84941
//,24971
//,27301
//,51298
//,87648
//,56390
//,2851
//,53736
//,55249
//,41292
//,44383
//,69461
//,17429
//,22373
//,13457
//,22273
//,49541
//,945
//,73717
//,75601
//,52063
//,54783
//,18676
//,26260
//,29511
//,61555
//,34870
//,25195
//,56456
//,18583
//,39617
//,79608
//,53732
//,79736
//,24173
//,61017
//,53252
//,76991
//,75506
//,33356
//,91161
//,27381
//,62481
//,67201
//,69456
//,16513
//,29893
//,13026
//,6039
//,40259
//,27706
//,20461
//,47608
//,81143
//,31551
//,67441
//,29597
//,58840
//,85436
//,72937
//,51337
//,17751
//,80145
//,1153
//,38985
//,72389
//,76777
//,45481
//,57211
//,48673
//,36651
//,64147
//,68805
//,30072
//,63291
//,10821
//,105
//,6636
//,64139
//,740
//,14081
//,16695
//,1101
//,80072
//,5646
//,68546
//,89206
//,89415
//,27200
//,62711
//,54531
//,35877
//,97995
//,81764
//,46701
//,79489
//,8949
//,15361
//,54239
//,97818
//,69672
//,53610
//,18751
//,75588
//,94357
//,82346
//,19601
//,43601
//,68401
//,75593
//,21251
//,40419
//,68284
//,41780
//,28161
//,30793
//,93960
//,68193
//,16637
//,16713
//,8641
//,29047
//,51651
//,73697
//,31077
//,65889
//,77875
//,55143
//,53161
//,15645
//,79505
//,55811
//,29631
//,86631
//,94737
//,23773
//,39634
//,66108
//,75356
//,17886
//,94337
//,73627
//,77735
//,78737
//,67201
//,45857
//,53591
//,73674
//,6416
//,18713
//,53545
//,71361
//,68052
//,44221
//,51745
//,20481
//,44141
//,12673
//,88217
//,92917
//,11941
//,73641
//,72628
//,82976
//,96159
//,83663
//,69011
//,89417
//,33812
//,79176
//,86674
//,37378
//,45551
//,70758
//,64397
//,10926
//,19222
//,37350
//,66767
//,99877
//,40524
//,26361
//,38217
//,78569
//,25025
//,2693
//,78546
//,37269
//,145
//,37388
//,76845
//,50338
//,89113
//,56729
//,59393
//,22426
//,3440
//,78381
//,65401
//,23187
//,99702
//,5781
//,22868
//,93467
//,32917
//,66631
//,78931
//,88010
//,74465
//,29241
//,64209
//,75601
//,60865
//,29700
//,14113
//,21519
//,38248
//,9248
//,21539
//,87880
//,53281
//,23799
//,80687
//,18468
//,1096
//,44574
//,2733
//,22851
//,32849
//,64821
//,27912
//,89249
//,86727
//,37361
//,48760
//,61219
//,39073
//,47773
//,28905
//,28353
//,42961
//,15755
//,83603
//,94351
//,67475
//,88801
//,24284
//,25742
//,31481
//,43169
//,32093
//,7695
//,95951
//,26145
//,67745
//,52193
//,96438
//,45371
//,9685
//,67585
//,93302
//,39803
//,30294
//,38967
//,84351
//,63927
//,24176
//,52091
//,89323
//,65525
//,33643
//,356
//,92885
//,62272
//,95017
//,46597
//,65921
//,44395
//,13001
//,38041
//,65200
//,41203
//,18023
//,44866
//,62945
//,69665
//,90206
//,11385
//,67711
//,67740
//,91202
//,90559
//,13330
//,57367
//,56071
//,80569
//,1925
//,47827
//,73909
//,5534
//,11001
//,4840
//,31349
//,97929
//,40929
//,8529
//,40781
//,93747
//,64743
//,90116
//,31921
//,85389
//,92289
//,22911
//,91813
//,3272
//,33185
//,91653
//,81609
//,23656
//,80441
//,16369
//,34302
//,75497
//,29334
//,43081
//,62123
//,9978
//,69501
//,44513
//,63244
//,9345
//,25367
//,15153
//,52985
//,40929
//,80549
//,69011
//,42881
//,62076
//,43261
//,47101
//,41256
//,27477
//,90331
//,91721
//,8001
//,31782
//,65253
//,80301
//,18866
//,74138
//,9406
//,14637
//,62125
//,13761
//,11471
//,49801
//,56469
//,29303
//,40616
//,58233
//,4091
//,47821
//,60185
//,81517
//,79491
//,94635
//,72968
//,98277
//,42763
//,47577
//,61481
//,25595
//,71181
//,63426
//,21729
//,30820
//,61013
//,95551
//,16441
//,20269
//,16861
//,14465
//,24404
//,68997
//,70522
//,98765
//,88100
//,67811
//,75816
//,43860
//,52835
//,42126
//,16065
//,12137
//,51265
//,2395
//,63829
//,5041
//,35937
//,27261
//,54321
//,68250
//,52121
//,69944
//,84961
//,86901
//,42049
//,94447
//,10023
//,44676
//,56193
//,95492
//,96336
//,6696
//,173
//,37705
//,2997
//,22777
//,22737
//,22093
//,13521
//,58569
//,61681
//,50901
//,83541
//,47665
//,96223
//,53365
//,59373
//,42101
//,44086
//,481
//,9321
//,82447
//,15639
//,47386
//,3001
//,23666
//,60391
//,44489
//,25967
//,271
//,56976
//,40907
//,67463
//,28061
//,40971
//,57025
//,68085
//,88301
//,90941
//,97856
//,90954
//,16569
//,39481
//,49409
//,28915
//,84502
//,9085
//,66473
//,3941
//,40178
//,47369
//,86941
//,45919
//,46481
//,72255
//,73497
//,42365
//,91432
//,63882
//,16353
//,20431
//,94797
//,74536
//,91374
//,27695
//,46941
//,86129
//,22358
//,81842
//,27989
//,38633
//,13971
//,969
//,74531
//,20063
//,32321
//,86361
//,34761
//,1134
//,77777
//,57937
//,1953
//,69417
//,26699
//,92582
//,92935
//,38230
//,66701
//,82054
//,91787
//,34563
//,94243
//,42013
//,97222
//,97387
//,95554
//,70154
//,50906
//,93541
//,46619
//,30481
//,80490
//,1611
//,86447
//,32151
//,99966
//,3048
//,36231
//,95705
//,85963
//,99896
//,26569
//,67837
//,58384
//,85491
//,61785
//,1817
//,1057
//,64999
//,85326
//,69449
//,6290
//,71017
//,32527
//,57964
//,59219
//,68033
//,21601
//,1653
//,17983
//,88413
//,40456
//,99183
//,751
//,81575
//,16313
//,52349
//,6605
//,60291
//,5809
//,13176
//,99069
//,11713
//,98738
//,19941
//,75881
//,94413
//,56191
//,25940
//,79735
//,15383
//,89593
//,70474
//,84558
//,81513
//,3482
//,76747
//,16111
//,62727
//,94510
//,37347
//,36525
//,11426
//,89945
//,82683
//,4113
//,72488
//,95216
//,16337
//,53457
//,60077
//,39285
//,72481
//,88867
//,15038
//,13889
//,735
//,67811
//,17041
//,85977
//,7883
//,69919
//,11864
//,2117
//,73077
//,95489
//,52465
//,60536
//,52170
//,63076
//,16801
//,56037
//,10657
//,55856
//,60481
//,71713
//,28108
//,43762
//,5394
//,31377
//,90621
//,86646
//,86921
//,70305
//,22134
//,14669
//,40386
//,91697
//,55251
//,37449
//,92540
//,97963
//,16993
//,7513
//,50922
//,56391
//,30402
//,62733
//,61153
//,36998
//,87895
//,70162
//,3101
//,61597
//,14401
//,38331
//,85381
//,92105
//,90301
//,27601
//,93026
//,58004
//,10162
//,13987
//,51941
//,72265
//,25829
//,66951
//,4489
//,5890
//,61435
//,24715
//,62425
//,88330
//,48251
//,62287
//,78726
//,3737
//,4196
//,64317
//,17449
//,93130
//,90178
//,96150
//,20601
//,58171
//,74273
//,5025
//,38785
//,91793
//,23243
//,64835
//,35125
//,32358
//,89005
//,46317
//,38953
//,50337
//,34609
//,69153
//,83071
//,28316
//,21197
//,88451
//,68990
//,17526
//,26881
//,67291
//,1491
//,74091
//,46221
//,33921
//,29566
//,67902
//,10337
//,27291
//,82053
//,70938
//,50827
//,14937
//,49223
//,4393
//,80851
//,93754
//,21706
//,45560
//,5584
//,46334
//,97479
//,95081
//,67301
//,23983
//,26081
//,18679
//,20495
//,92185
//,32717
//,62866
//,68067
//,58491
//,22841
//,87665
//,13701
//,20141
//,16261
//,92087
//,41199
//,80425
//,79061
//,70033
//,77577
//,35742
//,93887
//,38966
//,66771
//,33057
//,37801
//,41864
//,3641
//,56433
//,47681
//,93390
//,48011
//,78538
//,85694
//,24295
//,49065
//,23617
//,48489
//,26169
//,41206
//,18255
//,27353
//,14717
//,43801
//,78539
//,81991
//,17546
//,64385
//,97831
//,1601
//,1273
//,92253
//,31699
//,99838
//,73081
//,74025
//,58493
//,2216
//,7376
//,20145
//,67008
//,48641
//,74651
//,15306
//,96033
//,34236
//,34119
//,75399
//,8433
//,81747
//,25001
//,45365
//,15291
//,74185
//,12801
//,4949
//,44111
//,36246
//,59977
//,78217
//,53668
//,85986
//,10421
//,50246
//,65555
//,85974
//,5468
//,28801
//,61609
//,68311
//,38189
//,24517
//,21767
//,34384
//,47220
//,39441
//,25921
//,38538
//,65581
//,39765
//,96751
//,56161
//,55151
//,28432
//,64235
//,9273
//,93821
//,70564
//,12945
//,81271
//,23475
//,11755
//,25457
//,83431
//,12103
//,69383
//,11126
//,93661
//,44193
//,32521
//,54696
//,80571
//,6991
//,56734
//,6731
//,59905
//,29759
//,33825
//,48311
//,16702
//,77512
//,86141
//,12517
//,29544
//,91049
//,47406
//,59637
//,77557
//,15057
//,51701
//,56617
//,25445
//,95175
//,95824
//,66721
//,59544
//,34441
//,30163
//,26921
//,76497
//,97501
//,63275
//,90957
//,44588
//,3993
//,11153
//,90241
//,46669
//,54317
//,71091
//,43097
//,98738
//,59209
//,80531
//,59655
//,74392
//,55019
//,86576
//,90153
//,42705
//,11495
//,13045
//,91736
//,47633
//,98601
//,51801
//,90995
//,18962
//,41772
//,68801
//,24945
//,42597
//,25715
//,31013
//,72303
//,84835
//,18812
//,41121
//,12986
//,42393
//,98245
//,49621
//,41936
//,36601
//,87165
//,89177
//,69973
//,76609
//,52001
//,5483
//,13341
//,17057
//,96151
//,63078
//,60076
//,72801
//,50399
//,49569
//,89289
//,16192
//,89638
//,6508
//,67105
//,19916
//,20513
//,4341
//,17613
//,68837
//,1214
//,97790
//,85066
//,32513
//,64695
//,71341
//,78940
//,37086
//,26390
//,71757
//,21527
//,16807
//,55739
//,9902
//,93111
//,77081
//,92385
//,42051
//,45205
//,72029
//,7536
//,57211
//,69091
//,8681
//,38737
//,30329
//,15449
//,86732
//,74257
//,60851
//,40673
//,42936
//,86521
//,99801
//,39523
//,49199
//,20226
//,25671
//,55377
//,5511
//,67017
//,28433
//,57316
//,34636
//,33985
//,80682
//,25461
//,44738
//,91702
//,8553
//,86471
//,12860
//,88040
//,59713
//,12033
//,72883
//,49781
//,70813
//,2901
//,96919
//,97669
//,95796
//,58151
//,72871
//,65436
//,73985
//,7091
//,46520
//,61921
//,54305
//,90044
//,38542
//,96301
//,55761
//,35441
//,23401
//,30588
//,40721
//,56135
//,55859
//,55554
//,18185
//,13821
//,36729
//,63711
//,51973
//,73889
//,95751
//,45256
//,78970
//,38737
//,35208
//,95341
//,95926
//,94393
//,26566
//,61448
//,56725
//,96161
//,13901
//,39421
//,15601
//,44171
//,42771
//,70417
//,33449
//,37566
//,5155
//,91623
//,48953
//,60013
//,12256
//,57217
//,11955
//,13955
//,25988
//,27566
//,88801
//,93701
//,31579
//,56511
//,38769
//,15265
//,3191
//,84741
//,19891
//,28845
//,59666
//,45281
//,99095
//,64961
//,9665
//,76621
//,13726
//,79987
//,34706
//,64688
//,64677
//,35549
//,90491
//,56201
//,94401
//,72367
//,87251
//,65319
//,18881
//,29509
//,30442
//,21441
//,6801
//,38837
//,57724
//,47521
//,71669
//,78271
//,86194
//,18601
//,15547
//,99353
//,38846
//,70071
//,87585
//,31186
//,13052
//,98505
//,61385
//,55115
//,70459
//,21099
//,81171
//,85369
//,54128
//,73472
//,14942
//,21251
//,30821
//,55737
//,97918
//,50135
//,21373
//,19633
//,72459
//,39481
//,28873
//,71434
//,67976
//,61707
//,6411
//,68757
//,22161
//,25970
//,26233
//,68155
//,42773
//,56897
//,95809
//,97663
//,40428
//,91116
//,76697
//,39085
//,1347
//,91119
//,95271
//,96001
//,48532
//,67305
//,28921
//,84739
//,57153
//,31149
//,87136
//,96881
//,88975
//,36751
//,80241
//,2707
//,73253
//,50350
//,50591
//,82307
//,93129
//,32263
//,93116
//,9089
//,2221
//,53237
//,76393
//,57501
//,32193
//,73653
//,32297
//,31601
//,53679
//,25101
//,68569
//,91117
//,5459
//,88227
//,97191
//,14765
//,65001
//,24367
//,3901
//,51449
//,15428
//,56501
//,46351
//,4585
//,99409
//,79761
//,61089
//,2133
//,94215
//,76929
//,69939
//,96677
//,53217
//,61127
//,55147
//,14761
//,39521
//,14127
//,21670
//,50233
//,1773
//,73733
//,14185
//,17535
//,45742
//,55492
//,95238
//,30401
//,49867
//,18137
//,11811
//,42957
//,61251
//,71547
//,92954
//,17487
//,99633
//,44559
//,59369
//,97885
//,99188
//,7458
//,20373
//,42697
//,73201
//,42601
//,6753
//,8381
//,10121
//,88857
//,10069
//,12481
//,28686
//,42016
//,18244
//,57258
//,73753
//,19911
//,14681
//,60151
//,97267
//,73637
//,33466
//,39781
//,87713
//,73509
//,76413
//,5669
//,96939
//,70113
//,75946
//,82601
//,63993
//,43189
//,67321
//,27315
//,49828
//,2226
//,5823
//,4547
//,88211
//,79777
//,80141
//,72689
//,44489
//,20397
//,43498
//,16073
//,68339
//,79567
//,85317
//,76373
//,97781
//,60657
//,85875
//,42944
//,96107
//,2041
//,26477
//,93437
//,81921
//,60251
//,11905
//,28501
//,67551
//,51676
//,42583
//,67409
//,54077
//,18341
//,19951
//,29208
//,86709
//,10481
//,37701
//,55903
//,21936
//,46758
//,51761
//,29151
//,49017
//,56023
//,96274
//,47516
//,32235
//,66625
//,44295
//,46219
//,74099
//,60009
//,80423
//,7996
//,46966
//,53932
//,75687
//,2773
//,28813
//,7387
//,67089
//,76267
//,59421
//,9061
//,20113
//,22769
//,49807
//,30046
//,70555
//,62082
//,20277
//,42205
//,24556
//,91757
//,47671
//,67328
//,69559
//,71931
//,47633
//,90625
//,57621
//,481
//,36207
//,62964
//,4761
//,8705
//,52237
//,62145
//,70141
//,82025
//,52577
//,98980
//,19872
//,72054
//,12497
//,6121
//,27839
//,79916
//,88897
//,38951
//,47661
//,79223
//,11777
//,54798
//,6296
//,22121
//,31901
//,82633
//,28897
//,14250
//,66581
//,3524
//,47137
//,28003
//,29746
//,90758
//,49781
//,91859
//,20009
//,98941
//,99951
//,50376
//,8189
//,84225
//,67425
//,7201
//,78951
//,13741
//,83301
//,12514
//,64337
//,78751
//,84925
//,7345
//,17411
//,35127
//,52689
//,63785
//,22425
//,24465
//,89673
//,80001
//,54551
//,65791
//,48163
//,92811
//,8446
//,55201
//,20713
//,26103
//,89267
//,61251
//,57730
//,59811
//,3749
//,2521
//,50482
//,54721
//,19876
//,86833
//,13290
//,20771
//,68501
//,24961
//,3695
//,30826
//,6041
//,80836
//,82529
//,27984
//,14484
//,22369
//,53649
//,97962
//,99861
//,38102
//,91817
//,32410
//,92010
//,15259
//,96501
//,3381
//,79728
//,83687
//,21583
//,44506
//,90039
//,74943
//,39105
//,29303
//,29317
//,12952
//,32412
//,65856
//,73415
//,36081
//,34521
//,90484
//,81421
//,99707
//,60617
//,62957
//,31701
//,77324
//,33479
//,93908
//,45025
//,63089
//,30190
//,40366
//,11481
//,95121
//,42411
//,58493
//,30067
//,14266
//,91705
//,51697
//,32849
//,48701
//,55826
//,24626
//,63101
//,79126
//,32726
//,59037
//,60126
//,72726
//,71771
//,5125
//,68734
//,74145
//,11356
//,72727
//,53829
//,21873
//,92841
//,83191
//,33119
//,59657
//,12443
//,26521
//,46509
//,26847
//,69781
//,79809
//,46003
//,55919
//,84929
//,13841
//,41047
//,25531
//,35655
//,99805
//,38299
//,34996
//,88727
//,91253
//,46346
//,92809
//,21785
//,26207
//,24593
//,92933
//,81874
//,58176
//,29717
//,60721
//,40631
//,47473
//,87351
//,8261
//,64536
//,76034
//,80217
//,48086
//,41238
//,10599
//,78546
//,79873
//,74244
//,84621
//,8361
//,15473
//,9328
//,39361
//,55261
//,51401
//,86641
//,67393
//,75345
//,55408
//,86118
//,25953
//,47460
//,5041
//,44241
//,51601
//,74828
//,26489
//,57929
//,43785
//,71673
//,96707
//,32305
//,52861
//,31833
//,42721
//,61881
//,7341
//,98741
//,41662
//,43665
//,98337
//,87490
//,46451
//,4293
//,25361
//,60261
//,30301
//,83081
//,14911
//,17823
//,61377
//,29729
//,20969
//,42277
//,5543
//,38296
//,59586
//,88081
//,16120
//,39412
//,34821
//,16818
//,59592
//,40635
//,58901
//,91381
//,43981
//,20701
//,12561
//,74351
//,8471
//,71131
//,4005
//,58261
//,63721
//,6571
//,8382
//,75579
//,25920
//,44679
//,99493
//,47898
//,74205
//,39321
//,16044
//,98029
//,430
//,17376
//,17708
//,75939
//,46951
//,27766
//,84061
//,91281
//,52429
//,15953
//,34839
//,47413
//,64696
//,34821
//,8424
//,12596
//,52714
//,22139
//,26124
//,19129
//,87514
//,19800
//,56191
//,92465
//,50501
//,64000
//,30401
//,59943
//,25209
//,4288
//,27433
//,49235
//,57628
//,19221
//,86155
//,54826
//,43441
//,74651
//,90702
//,2106
//,22801
//,73269
//,80449
//,31741
//,56563
//,84753
//,26993
//,73851
//,46964
//,36001
//,17246
//,53546
//,58501
//,65719
//,31637
//,16338
//,6661
//,8021
//,27046
//,84321
//,80109
//,72929
//,78711
//,32553
//,70016
//,86003
//,47297
//,49868
//,52791
//,60252
//,92796
//,99256
//,11313
//,49490
//,20733
//,18299
//,55966
//,45262
//,38901
//,92777
//,29548
//,87991
//,28066
//,83751
//,54055
//,44963
//,63564
//,19983
//,98266
//,74657
//,23301
//,32907
//,37819
//,91257
//,28902
//,799
//,67031
//,58769
//,82153
//,52160
//,12724
//,24224
//,67555
//,23203
//,78044
//,83767
//,38881
//,78391
//,3161
//,96121
//,72576
//,63679
//,74501
//,43438
//,79637
//,36501
//,91951
//,64033
//,25178
//,124
//,51683
//,55641
//,26671
//,58694
//,595
//,85731
//,38019
//,88886
//,55773
//,62379
//,22321
//,35111
//,73075
//,61757
//,49473
//,57578
//,11705
//,96621
//,41821
//,24917
//,6290
//,34700
//,17501
//,40693
//,84439
//,22546
//,54561
//,73595
//,71259
//,27393
//,76419
//,51625
//,65981
//,3207
//,84641
//,57631
//,5586
//,16876
//,5661
//,37961
//,62524
//,10735
//,47857
//,12869
//,22129
//,89069
//,58381
//,12801
//,51401
//,97661
//,53065
//,61194
//,44172
//,11861
//,2313
//,24141
//,77258
//,10705
//,64226
//,10473
//,59728
//,62550
//,3773
//,5508
//,71465
//,24972
//,65569
//,98593
//,28626
//,18485
//,67989
//,25696
//,38141
//,85625
//,59150
//,76991
//,97491
//,16635
//,12843
//,60971
//,80940
//,10871
//,80465
//,60259
//,9784
//,82800
//,23127
//,37486
//,79817
//,3836
//,38901
//,29740
//,2560
//,58106
//,6249
//,67151
//,35289
//,90911
//,73189
//,325
//,77141
//,58977
//,35608
//,44651
//,18373
//,3091
//,23165
//,26953
//,29776
//,47305
//,31665
//,9240
//,33511
//,29819
//,8128
//,70901
//,97097
//,88606
//,94999
//,39035
//,28501
//,70531
//,10001
//,20340
//,46312
//,26251
//,86333
//,37710
//,19466
//,52963
//,51363
//,91041
//,96801
//,95771
//,12765
//,22017
//,29729
//,499
//,21571
//,47919
//,99709
//,11671
//,84895
//,54975
//,5731
//,48472
//,40049
//,58189
//,81275
//,58751
//,9633
//,48041
//,25076
//,34791
//,57016
//,85512
//,80893
//,65211
//,7233
//,82423
//,49361
//,45153
//,8705
//,92137
//,35871
//,91448
//,12029
//,86707
//,99972
//,85225
//,18695
//,76170
//,2801
//,71194
//,15861
//,37881
//,25969
//,46216
//,95011
//,74551
//,61897
//,60743
//,35683
//,93789
//,75007
//,28650
//,25606
//,19611
//,5995
//,72404
//,42785
//,6033
//,67121
//,79687
//,43889
//,66856
//,57391
//,85063
//,40351
//,99361
//,5175
//,18307
//,49916
//,17597
//,68965
//,751
//,82561
//,79029
//,73137
//,15484
//,99521
//,18896
//,44535
//,43861
//,57784
//,62076
//,46001
//,89145
//,94066
//,43238
//,73785
//,27365
//,37949
//,36934
//,69601
//,52545
//,59465
//,6346
//,87981
//,55431
//,54125
//,27915
//,57401
//,71121
//,46341
//,24084
//,13436
//,43841
//,96701
//,12048
//,38031
//,51157
//,70709
//,95195
//,18201
//,75009
//,2381
//,56212
//,5401
//,96021
//,75401
//,60349
//,38361
//,48129
//,36945
//,76459
//,72039
//,57141
//,5161
//,67007
//,51781
//,76697
//,8929
//,33558
//,68173
//,71079
//,36181
//,85069
//,49901
//,12189
//,83775
//,42136
//,32051
//,43921
//,94837
//,3566
//,32039
//,52537
//,69846
//,98001
//,52598
//,65247
//,62231
//,1717
//,60565
//,52593
//,17757
//,12275
//,96418
//,37057
//,52614
//,4320
//,50237
//,55636
//,66513
//,86961
//,10051
//,68377
//,1707
//,81792
//,49137
//,1822
//,38011
//,37055
//,25622
//,15475
//,27553
//,17819
//,293
//,52993
//,38802
//,51858
//,67591
//,73907
//,17450
//,48657
//,84761
//,8403
//,69713
//,66063
//,87761
//,25605
//,92001
//,56468
//,74513
//,73279
//,6797
//,81636
//,66559
//,21809
//,82853
//,76895
//,60901
//,42081
//,77893
//,68649
//,76217
//,59185
//,56701
//,24918
//,84850
//,79522
//,85825
//,15721
//,32025
//,67361
//,47557
//,60771
//,57633
//,43667
//,60335
//,19405
//,67721
//,76680
//,82012
//,82085
//,62164
//,60721
//,69778
//,4463
//,91921
//,84157
//,77010
//,98978
//,2760
//,45761
//,83152
//,9245
//,72782
//,85291
//,49703
//,97808
//,81901
//,84467
//,15550
//,88113
//,35150
//,40864
//,77713
//,98647
//,92673
//,95991
//,78729
//,71464
//,68513
//,5501
//,64774
//,6488
//,38210
//,44561
//,35077
//,17501
//,36023
//,83063
//,88961
//,11045
//,92395
//,16004
//,66437
//,64046
//,81729
//,64766
//,31551
//,49853
//,91361
//,60661
//,83307
//,2952
//,97235
//,29245
//,62807
//,89550
//,70730
//,58895
//,32815
//,58196
//,60395
//,4101
//,7751
//,66401
//,97088
//,35088
//,74225
//,71011
//,9713
//,72199
//,78909
//,27201
//,42961
//,4841
//,20117
//,24301
//,62714
//,90251
//,30924
//,33350
//,96081
//,6791
//,20731
//,65145
//,52801
//,52431
//,86766
//,43825
//,98987
//,15138
//,21913
//,94753
//,63801
//,84415
//,58787
//,54208
//,38639
//,90676
//,28154
//,40513
//,87082
//,25626
//,19393
//,32681
//,33163
//,44773
//,53433
//,49265
//,20101
//,49241
//,57261
//,4689
//,48213
//,29233
//,7145
//,70089
//,68557
//,32226
//,20141
//,27501
//,53301
//,82005
//,86197
//,34896
//,3772
//,85293
//,80411
//,28445
//,59809
//,61551
//,44588
//,47111
//,69622
//,99535
//,38965
//,46081
//,71908
//,49509
//,96845
//,98421
//,86266
//,7739
//,38201
//,73913
//,43844
//,49732
//,69002
//,58553
//,68017
//,89430
//,71489
//,59147
//,33069
//,92305
//,1601
//,23153
//,97611
//,45197
//,7986
//,96344
//,69897
//,7426
//,67499
//,67614
//,69377
//,90195
//,97571
//,16141
//,95583
//,64133
//,98142
//,35457
//,58528
//,1681
//,57787
//,57712
//,73037
//,6765
//,67227
//,18951
//,82893
//,21311
//,18709
//,66768
//,15145
//,77117
//,22753
//,25544
//,93363
//,11488
//,66961
//,53501
//,67419
//,48181
//,83879
//,76873
//,13643
//,90203
//,28927
//,65076
//,34673
//,76657
//,43226
//,62887
//,17369
//,36176
//,30983
//,33500
//,23873
//,71104
//,80639
//,72001
//,3024
//,69981
//,91442
//,1414
//,91212
//,11832
//,97049
//,28761
//,80433
//,88371
//,66181
//,36311
//,77235
//,86433
//,62899
//,78128
//,17441
//,59334
//,86822
//,82511
//,24943
//,3320
//,70689
//,52693
//,22682
//,27585
//,8897
//,72297
//,86801
//,53377
//,69791
//,96081
//,93841
//,63152
//,58251
//,46777
//,45884
//,88779
//,29250
//,46395
//,66379
//,51041
//,82315
//,8841
//,5097
//,58593
//,57941
//,97411
//,76034
//,57279
//,19553
//,31642
//,89376
//,2761
//,66611
//,99971
//,28267
//,61
//,71138
//,9827
//,25479
//,68381
//,73141
//,85713
//,67716
//,65857
//,8913
//,72067
//,52212
//,52501
//,16991
//,93973
//,90821
//,12856
//,79451
//,21983
//,49349
//,82021
//,21688
//,9345
//,33068
//,96491
//,73406
//,51
//,50801
//,625
//,69304
//,46931
//,57161
//,36791
//,39452
//,27053
//,20311
//,43693
//,71425
//,47569
//,65081
//,45821
//,91353
//,64343
//,34741
//,60012
//,67788
//,6529
//,88017
//,28465
//,21799
//,99137
//,92257
//,33821
//,22281
//,85827
//,84349
//,53693
//,10832
//,39329
//,13041
//,6983
//,42080
//,84216
//,54547
//,43001
//,64502
//,8069
//,30050
//,77313
//,14448
//,54831
//,57214
//,34857
//,85814
//,63027
//,3326
//,56251
//,145
//,76297
//,98745
//,82601
//,38148
//,19683
//,70321
//,5936
//,6121
//,5065
//,2578
//,43506
//,43161
//,64745
//,53801
//,91521
//,24285
//,27301
//,94957
//,71066
//,36729
//,40737
//,37405
//,39705
//,41966
//,87088
//,63119
//,41911
//,65431
//,48881
//,81435
//,84537
//,54881
//,72481
//,98651
//,35247
//,21813
//,48897
//,69434
//,77237
//,30518
//,6675
//,64806
//,3333
//,55599
//,98715
//,87813
//,52497
//,16541
//,17017
//,7017
//,19267
//,39723
//,10836
//,91834
//,18019
//,74361
//,92536
//,84127
//,28301
//,37189
//,39036
//,49131
//,73953
//,24697
//,14179
//,22997
//,6555
//,42191
//,75553
//,3561
//,83858
//,7221
//,67214
//,96401
//,45826
//,16217
//,46423
//,75305
//,49521
//,7441
//,21329
//,82073
//,6145
//,83589
//,68981
//,67601
//,38193
//,8175
//,48567
//,70881
//,50601
//,32235
//,87355
//,34967
//,52851
//,25937
//,73125
//,62271
//,67823
//,28101
//,35191
//,24847
//,94981
//,16333
//,80721
//,66691
//,58933
//,23793
//,94131
//,80241
//,9761
//,78209
//,48481
//,47148
//,44641
//,65561
//,7391
//,41646
//,45381
//,1188
//,42618
//,9911
//,49985
//,84922
//,14963
//,37705
//,14310
//,37073
//,43083
//,31601
//,81541
//,73164
//,33826
//,24988
//,95401
//,2497
//,21892
//,13996
//,12382
//,9765
//,80505
//,52657
//,1391
//,86904
//,41443
//,57823
//,70497
//,53117
//,8944
//,67341
//,38922
//,50185
//,43075
//,58201
//,68969
//,30633
//,66497
//,42033
//,86058
//,41178
//,64879
//,68601
//,61481
//,79289
//,7825
//,43905
//,24321
//,96576
//,44999
//,81330
//,22266
//,26109
//,41601
//,65025
//,66281
//,38951
//,68967
//,98465
//,90343
//,26356
//,39137
//,43431
//,45025
//,93383
//,52379
//,90209
//,16508
//,84245
//,5633
//,47617
//,62441
//,65711
//,67945
//,79901
//,55473
//,94017
//,23885
//,93101
//,77185
//,96935
//,74349
//,75121
//,71806
//,76373
//,69079
//,8143
//,9555
//,8841
//,2011
//,16525
//,68316
//,2809
//,34513
//,86292
//,2496
//,88530
//,36612
//,81665
//,39902
//,77171
//,65456
//,11431
//,74901
//,22056
//,2161
//,80499
//,1301
//,16826
//,24775
//,14785
//,57629
//,33501
//,70707
//,74405
//,88449
//,39473
//,8779
//,15697
//,37469
//,44121
//,66330
//,58156
//,80303
//,43913
//,40461
//,59113
//,75026
//,19421
//,34047
//,30895
//,82876
//,65225
//,77441
//,98881
//,44812
//,44434
//,46778
//,10561
//,18826
//,31981
//,59678
//,58299
//,88991
//,63717
//,4217
//,89313
//,66666
//,71243
//,70097
//,35180
//,96657
//,74986
//,52212
//,76884
//,53608
//,24993
//,97411
//,48601
//,19489
//,42865
//,49361
//,59377
//,40587
//,66425
//,54445
//,61845
//,37871
//,43911
//,53847
//,16829
//,20196
//,14424
//,72639
//,24033
//,69509
//,1137
//,37231
//,90647
//,50661
//,65043
//,31589
//,80329
//,34897
//,5116
//,45153
//,30351
//,96197
//,15873
//,64439
//,73481
//,65789
//,69861
//,87441
//,23951
//,20175
//,23409
//,32641
//,53171
//,14305
//,61601
//,23243
//,84511
//,69918
//,96513
//,71441
//,39785
//,93321
//,26537
//,47718
//,77330
//,31947
//,86616
//,12741
//,65949
//,19780
//,53977
//,23906
//,28331
//,14043
//,82476
//,58422
//,82386
//,54207
//,16577
//,37185
//,11661
//,937
//,71065
//,69953
//,99679
//,55601
//,58231
//,40545
//,57395
//,40663
//,18525
//,82381
//,78305
//,36251
//,74737
//,51433
//,19651
//,18172
//,92761
//,19950
//,13137
//,92609
//,31875
//,91854
//,81755
//,36433
//,78643
//,4125
//,32461
//,20029
//,86593
//,86557
//,79841
//,85653
//,33711
//,73068
//,7856
//,58495
//,92646
//,9816
//,47373
//,28571
//,9426
//,95303
//,70757
//,97701
//,83635
//,93216
//,65096
//,40982
//,99913
//,5145
//,45201
//,52602
//,67369
//,17041
//,17931
//,57116
//,10222
//,83094
//,95927
//,82427
//,26768
//,85055
//,96118
//,50543
//,42207
//,36961
//,90496
//,1837
//,91651
//,50034
//,26644
//,57981
//,34226
//,63894
//,92797
//,73889
//,73826
//,83067
//,11951
//,77258
//,89539
//,18684
//,89165
//,76981
//,52617
//,67551
//,22191
//,2255
//,21825
//,99928
//,74751
//,13320
//,94501
//,88815
//,16551
//,45618
//,76041
//,89737
//,32289
//,98655
//,19429
//,87910
//,58721
//,18473
//,47345
//,89275
//,80753
//,58039
//,2859
//,18337
//,38229
//,56631
//,88870
//,86593
//,93198
//,80161
//,3841
//,45009
//,22995
//,91729
//,93551
//,76291
//,83741
//,13921
//,67245
//,90401
//,15601
//,60773
//,24101
//,54833
//,57651
//,88853
//,4719
//,4901
//,69420
//,83384
//,51409
//,91428
//,64726
//,82511
//,28497
//,39859
//,22233
//,9921
//,70204
//,67393
//,881
//,33251
//,46929
//,20817
//,45857
//,99091
//,38049
//,16997
//,43773
//,87895
//,15721
//,65769
//,47676
//,34841
//,91001
//,92571
//,11342
//,62354
//,29698
//,6213
//,16563
//,46207
//,87561
//,60993
//,82845
//,53489
//,78386
//,30801
//,46905
//,8193
//,27031
//,19177
//,5000
//,59303
//,34669
//,13503
//,52471
//,78861
//,3351
//,99881
//,62285
//,11326
//,19831
//,26188
//,32536
//,94009
//,98404
//,75329
//,75593
//,97457
//,11981
//,94966
//,94901
//,49105
//,5623
//,97729
//,95233
//,10005
//,27076
//,19419
//,33593
//,8001
//,7086
//,35401
//,71421
//,20484
//,92521
//,91292
//,78861
//,31201
//,41933
//,35378
//,32091
//,85341
//,74369
//,98340
//,76932
//,88731
//,36397
//,74437
//,32805
//,257
//,31617
//,12997
//,39139
//,40161
//,3637
//,73121
//,70081
//,34003
//,8297
//,37126
//,74213
//,49791
//,44337
//,29433
//,44545
//,14669
//,22727
//,79892
//,465
//,85354
//,82563
//,89123
//,21184
//,52541
//,26687
//,57521
//,73001
//,10237
//,61045
//,20937
//,39873
//,8363
//,53981
//,75953
//,34241
//,4447
//,10001
//,60601
//,25755
//,53959
//,19391
//,52081
//,61889
//,63160
//,85295
//,13166
//,91377
//,92864
//,82456
//,41949
//,98421
//,60001
//,46840
//,9499
//,30741
//,57663
//,94721
//,60065
//,39847
//,99206
//,29351
//,55383
//,28161
//,94240
//,6454
//,98118
//,74969
//,18936
//,54513
//,58897
//,47089
//,82091
//,22749
//,45156
//,99913
//,89675
//,85380
//,44251
//,4137
//,53297
//,18317
//,42503
//,90113
//,20511
//,58957
//,86739
//,73458
//,77567
//,5771
//,81157
//,36139
//,71395
//,58281
//,76248
//,68201
//,76457
//,12109
//,69579
//,84726
//,7691
//,64745
//,60422
//,66921
//,61836
//,48401
//,9844
//,69809
//,7426
//,86003
//,71197
//,16649
//,70776
//,96381
//,81113
//,74085
//,57383
//,70161
//,73767
//,40412
//,58265
//,59347
//,77197
//,42541
//,57213
//,23776
//,5100
//,78326
//,68485
//,95669
//,82612
//,77237
//,68881
//,57681
//,97241
//,43778
//,94133
//,63116
//,71517
//,38761
//,34131
//,59701
//,88326
//,62782
//,77510
//,69106
//,45201
//,34156
//,26329
//,73937
//,50049
//,69879
//,59097
//,31412
//,53256
//,12096
//,39078
//,49426
//,42959
//,1529
//,12680
//,87897
//,57288
//,83647
//,17321
//,35201
//,50701
//,9261
//,67155
//,47753
//,19971
//,51177
//,13513
//,10921
//,23496
//,28817
//,37101
//,5125
//,39329
//,44049
//,61801
//,35687
//,5489
//,84681
//,29841
//,19689
//,25581
//,53779
//,46517
//,15513
//,49751
//,42405
//,77743
//,56189
//,6781
//,16416
//,9601
//,14177
//,36471
//,40452
//,61531
//,31251
//,57585
//,72573
//,79089
//,89057
//,56057
//,9708
//,68720
//,64089
//,66113
//,34334
//,86041
//,23426
//,675
//,95612
//,97413
//,50129
//,84801
//,5343
//,95896
//,70160
//,57542
//,13865
//,35847
//,65729
//,15455
//,15201
//,46649
//,72913
//,59255
//,69300
//,76020
//,27046
//,37288
//,34201
//,41893
//,99201
//,7758
//,81889
//,73599
//,27511
//,93701
//,27563
//,78966
//,95670
//,57465
//,81032
//,11301
//,31196
//,8145
//,45945
//,74001
//,45180
//,18367
//,41409
//,20785
//,97057
//,38002
//,83109
//,70651
//,28531
//,42119
//,44915
//,92376
//,22143
//,74286
//,38539
//,57251
//,29693
//,64826
//,59277
//,72541
//,95576
//,29389
//,99537
//,12519
//,57001
//,21695
//,45501
//,47129
//,86507
//,77830
//,22742
//,58489
//,5681
//,6865
//,74374
//,76221
//,2779
//,23899
//,79601
//,95009
//,60638
//,13025
//,90452
//,83011
//,73501
//,97033
//,50895
//,99196
//,93471
//,55297
//,5393
//,13391
//,11587
//,93153
//,33903
//,25321
//,70546
//,85559
//,4371
//,4822
//,66841
//,89921
//,54763
//,74969
//,61519
//,12481
//,31731
//,57494
//,52281
//,19361
//,3371
//,55665
//,40767
//,65107
//,57761
//,59977
//,78325
//,37296
//,81351
//,65651
//,63081
//,46143
//,44937
//,85469
//,37
//,72236
//,4315
//,65430
//,2987
//,19492
//,79791
//,98521
//,19445
//,68245
//,9024
//,48417
//,17937
//,38141
//,53676
//,38626
//,40453
//,29505
//,40641
//,42688
//,74937
//,58201
//,1547
//,75268
//,5640
//,48364
//,83357
//,5026
//,44780
//,9921
//,94165
//,621
//,96456
//,59061
//,13420
//,95545
//,97461
//,69729
//,79889
//,31583
//,18737
//,86593
//,72585
//,59202
//,29816
//,50471
//,97573
//,94569
//,74315
//,98465
//,12841
//,76705
//,37014
//,674
//,90101
//,85257
//,51785
//,24033
//,57745
//,50881
//,74047
//,69206
//,81526
//,77465
//,99803
//,75257
//,21793
//,71921
//,3177
//,3026
//,71501
//,31753
//,29
//,4185
//,71765
//,8161
//,13626
//,50267
//,86841
//,59751
//,13827
//,34470
//,7358
//,86518
//,65655
//,50123
//,74501
//,88881
//,54413
//,71971
//,51605
//,33113
//,82255
//,43676
//,19921
//,50922
//,88382
//,25188
//,21601
//,90095
//,1577
//,62541
//,64210
//,87999
//,24353
//,27371
//,63313
//,57319
//,9705
//,19152
//,31366
//,29092
//,11001
//,12171
//,71749
//,68957
//,20449
//,93035
//,93902
//,36861
//,48707
//,97721
//,71101
//,76241
//,27690
//,5793
//,95928
//,39021
//,76932
//,2531
//,76921
//,59787
//,23029
//,72898
//,16006
//,53270
//,87380
//,70027
//,85025
//,44041
//,20580
//,50563
//,83603
//,78801
//,93335
//,83557
//,7201
//,35721
//,3046
//,1987
//,45058
//,32476
//,17061
//,29680
//,1201
//,87093
//,98382
//,50171
//,93621
//,22691
//,51562
//,16985
//,71112
//,73427
//,10092
//,93593
//,70397
//,78396
//,32279
//,43384
//,98169
//,75489
//,78756
//,18321
//,62311
//,48481
//,49596
//,37681
//,81031
//,76547
//,80490
//,67230
//,1251
//,68213
//,40185
//,40766
//,83256
//,79641
//,15706
//,26691
//,87617
//,13193
//,65919
//,51633
//,28885
//,31872
//,74851
//,45159
//,86681
//,484
//,72390
//,8421
//,45691
//,2495
//,46485
//,8180
//,53621
//,36537
//,56881
//,28343
//,41433
//,12232
//,44641
//,96699
//,95041
//,10201
//,60145
//,25072
//,61919
//,71811
//,71817
//,57771
//,86619
//,52877
//,56755
//,17683
//,40121
//,43681
//,48121
//,48173
//,48289
//,49665
//,46058
//,80379
//,15784
//,10561
//,19220
//,62631
//,31343
//,25627
//,57709
//,37281
//,99380
//,46769
//,10819
//,48957
//,5471
//,54607
//,65937
//,53476
//,35598
//,40491
//,96152
//,99167
//,81937
//,3229
//,24854
//,96738
//,26007
//,97992
//,13886
//,93393
//,7597
//,76795
//,55433
//,75183
//,38313
//,87821
//,50497
//,71306
//,4873
//,60511
//,66671
//,75201
//,8816
//,96705
//,72559
//,78589
//,29558
//,411
//,11442
//,88644
//,20257
//,16483
//,8223
//,48715
//,54929
//,8179
//,26289
//,54245
//,68277
//,70490
//,91181
//,46053
//,66577
//,69286
//,39606
//,68386
//,51308
//,95201
//,51376
//,1750
//,5870
//,65783
//,77404
//,64201
//,17889
//,28796
//,5573
//,42495
//,22817
//,73079
//,78601
//,89613
//,11225
//,6265
//,2771
//,93543
//,9099
//,79741
//,73823
//,87153
//,99151
//,85851
//,19873
//,56744
//,21441
//,84006
//,60953
//,81908
//,14242
//,1785
//,20426
//,84393
//,34835
//,75187
//,84445
//,18137
//,9816
//,48657
//,9375
//,97697
//,25155
//,28441
//,48817
//,47136
//,26221
//,17455
//,94671
//,14849
//,9289
//,51191
//,97901
//,85263
//,55251
//,20716
//,20021
//,17417
//,20053
//,70881
//,16161
//,44771
//,60411
//,85587
//,17236
//,94274
//,26929
//,10626
//,29176
//,24106
//,64383
//,74001
//,37088
//,20567
//,64641
//,61671
//,40338
//,30281
//,10049
//,44481
//,69821
//,81584
//,31165
//,41353
//,18377
//,70373
//,3601
//,82529
//,58157
//,40661
//,99785
//,5901
//,91583
//,1221
//,24901
//,32996
//,58464
//,81309
//,52551
//,65077
//,6030
//,44641
//,69427
//,99619
//,86481
//,98242
//,56609
//,84601
//,77756
//,6808
//,56513
//,93316
//,43381
//,90081
//,49657
//,54153
//,78617
//,62678
//,18573
//,56605
//,86691
//,6177
//,35649
//,54809
//,11325
//,54649
//,35373
//,67672
//,41548
//,69252
//,70263
//,75201
//,77412
//,79393
//,68313
//,34445
//,89416
//,20165
//,90334
//,42567
//,74623
//,19638
//,66593
//,80913
//,13901
//,7583
//,11031
//,10106
//,36869
//,8706
//,84359
//,31426
//,65551
//,20225
//,75145
//,90961
//,3309
//,60142
//,9097
//,25053
//,6721
//,36946
//,30478
//,93617
//,72164
//,40975
//,13245
//,44623
//,97941
//,63560
//,56705
//,21601
//,1063
//,50344
//,18995
//,70881
//,65460
//,57677
//,19091
//,81631
//,49825
//,33614
//,76294
//,33945
//,34181
//,63715
//,86261
//,53227
//,5741
//,36015
//,66371
//,50684
//,20245
//,40951
//,16293
//,34856
//,28252
//,59796
//,84869
//,91827
//,28501
//,33696
//,4231
//,21456
//,83071
//,7648
//,24001
//,55856
//,24841
//,73624
//,22065
//,36724
//,19707
//,38833
//,30441
//,35061
//,16927
//,64337
//,48817
//,58874
//,23265
//,67795
//,25675
//,63283
//,31481
//,24125
//,43411
//,81631
//,25153
//,51626
//,72278
//,20929
//,20993
//,3925
//,59731
//,24387
//,49917
//,53084
//,79741
//,52513
//,98769
//,73961
//,8733
//,7585
//,90515
//,2789
//,99974
//,64851
//,84159
//,24241
//,70589
//,95869
//,4427
//,3671
//,24705
//,33911
//,55176
//,58113
//,70961
//,94037
//,9172
//,22217
//,43445
//,90011
//,58571
//,8431
//,69499
//,77526
//,74568
//,4993
//,65614
//,53041
//,53041
//,45421
//,52081
//,3893
//,79577
//,83051
//,40073
//,93105
//,1881
//,28067
//,90393
//,85029
//,20321
//,25401
//,41997
//,45019
//,90242
//,49796
//,33127
//,31596
//,35465
//,40531
//,86921
//,65861
//,85266
//,15257
//,47111
//,73026
//,78633
//,16968
//,32141
//,73398
//,51713
//,90866
//,20569
//,86765
//,40561
//,94201
//,82682
//,59909
//,45844
//,40605
//,76353
//,82522
//,83036
//,67733
//,1739
//,13537
//,82051
//,43841
//,51974
//,71801
//,5376
//,34363
//,32264
//,26226
//,14351
//,45044
//,76379
//,30289
//,18709
//,77441
//,85832
//,52896
//,74851
//,47064
//,90251
//,7741
//,16669
//,66609
//,33749
//,18129
//,58179
//,26252
//,5381
//,12017
//,16254
//,98785
//,10169
//,37021
//,68445
//,14235
//,10949
//,95757
//,92366
//,57691
//,90361
//,88908
//,83017
//,97138
//,80355
//,4473
//,74326
//,77088
//,59261
//,9088
//,61839
//,78872
//,86989
//,66423
//,49155
//,83984
//,30378
//,85958
//,31561
//,25661
//,90969
//,8256
//,29611
//,86292
//,4991
//,7813
//,67476
//,39497
//,5761
//,64049
//,51
//,81031
//,54631
//,11278
//,89901
//,11596
//,91716
//,27457
//,93601
//,41430
//,38653
//,34697
//,90042
//,60001
//,17761
//,40659
//,10086
//,95293
//,59960
//,57893
//,87961
//,13057
//,88220
//,97513
//,3953
//,59009
//,40771
//,85602
//,30819
//,77575
//,81
//,73087
//,21104
//,19095
//,30189
//,71618
//,47701
//,48081
//,26386
//,9430
//,43213
//,96096
//,40488
//,77293
//,30625
//,42069
//,50523
//,6581
//,45226
//,86979
//,68031
//,72196
//,54331
//,84101
//,8551
//,10964
//,10472
//,67933
//,94241
//,38337
//,74373
//,89219
//,38293
//,44441
//,74484
//,8230
//,81151
//,13437
//,23386
//,97101
//,98837
//,82676
//,86833
//,88356
//,76713
//,33941
//,1181
//,73009
//,65795
//,28005
//,41961
//,95073
//,18434
//,87001
//,93941
//,32143
//,5697
//,70913
//,57368
//,35681
//,30838
//,2707
//,13405
//,82561
//,67321
//,22914
//,68733
//,9361
//,84163
//,52737
//,92914
//,26001
//,61381
//,92251
//,50896
//,90516
//,77953
//,99545
//,42797
//,35121
//,79417
//,37722
//,43665
//,75168
//,45785
//,97661
//,66634
//,75125
//,32096
//,57355
//,51061
//,81225
//,25095
//,67263
//,79011
//,21257
//,36526
//,56265
//,83713
//,6024
//,6371
//,39107
//,8731
//,75060
//,72999
//,28902
//,24419
//,74877
//,40886
//,44201
//,78961
//,50441
//,37327
//,55977
//,83728
//,7547
//,67761
//,78491
//,19249
//,14666
//,32718
//,49257
//,77730
//,50152
//,803
//,81449
//,40041
//,37375
//,47509
//,75665
//,10353
//,82564
//,45126
//,77712
//,65975
//,23805
//,47027
//,53330
//,54029
//,2384
//,64640
//,8110
//,67173
//,45826
//,16185
//,98017
//,79033
//,86643
//,88017
//,379
//,45990
//,67334
//,70951
//,76333
//,85071
//,15521
//,69188
//,54226
//,97371
//,45703
//,80485
//,91609
//,89379
//,90057
//,35017
//,23585
//,15083
//,39865
//,57256
//,72449
//,59129
//,67905
//,19591
//,49751
//,37716
//,61543
//,83793
//,19541
//,81841
//,42707
//,12077
//,56370
//,74091
//,15082
//,64847
//,85625
//,6209
//,83618
//,80812
//,75595
//,42189
//,77029
//,46349
//,2667
//,56499
//,65661
//,16547
//,25281
//,12131
//,78891
//,19848
//,75006
//,93225
//,88328
//,31151
//,45924
//,80581
//,21781
//,11601
//,70623
//,89025
//,59873
//,74265
//,33364
//,17331
//,23505
//,42507
//,56633
//,91965
//,37705
//,63932
//,86213
//,54383
//,10642
//,97360
//,93409
//,7987
//,40474
//,30054
//,82609
//,19219
//,61829
//,9809
//,86173
//,97969
//,21566
//,59881
//,44431
//,18657
//,85281
//,69859
//,61736
//,58389
//,75745
//,61141
//,37025
//,48843
//,71653
//,4831
//,32733
//,55301
//,11646
//,39013
//,59489
//,31400
//,44623
//,97662
//,45097
//,3621
//,52733
//,1399
//,4391
//,83761
//,61357
//,53128
//,13126
//,47313
//,45016
//,63888
//,11761
//,72632
//,49992
//,28147
//,48811
//,91675
//,44853
//,66696
//,25551
//,69153
//,62351
//,91166
//,77943
//,86765
//,53285
//,8187
//,96651
//,94661
//,66886
//,65535
//,69161
//,49653
//,55633
//,85740
//,6604
//,63241
//,86537
//,4535
//,88416
//,79367
//,69808
//,72399
//,94181
//,14219
//,84945
//,3981
//,56063
//,73200
//,56992
//,76031
//,79809
//,15011
//,82177
//,19773
//,93481
//,65927
//,95161
//,28933
//,10441
//,55030
//,64491
//,1161
//,137
//,61925
//,90861
//,19124
//,20263
//,51560
//,98465
//,42805
//,1410
//,43362
//,98412
//,89895
//,411
//,66003
//,9506
//,18437
//,48635
//,32001
//,12847
//,25
//,30125
//,22988
//,85601
//,60510
//,81281
//,24401
//,82725
//,72849
//,3825
//,49375
//,30869
//,741
//,88543
//,11241
//,70795
//,88857
//,81815
//,72665
//,9217
//,28331
//,77131
//,14879
//,2433
//,28891
//,15059
//,82728
//,38097
//,741
//,6037
//,14706
//,51366
//,68065
//,69108
//,74915
//,37132
//,53376
//,33736
//,45743
//,63329
//,49588
//,46931
//,71169
//,48385
//,45703
//,5801
//,99943
//,31281
//,42879
//,84737
//,20289
//,83019
//,6856
//,91778
//,74001
//,47064
//,43731
//,38633
//,63431
//,93137
//,71081
//,41356
//,36195
//,17163
//,52808
//,98436
//,48405
//,36671
//,73501
//,94778
//,40995
//,57799
//,22958
//,36685
//,85283
//,35529
//,79809
//,41301
//,57981
//,77173
//,2322
//,67923
//,37503
//,47766
//,60201
//,31517
//,15994
//,26921
//,46621
//,95391
//,63713
//,15731
//,71471
//,13496
//,62986
//,5801
//,26277
//,73376
//,7821
//,2111
//,93390
//,16889
//,95201
//,16541
//,27653
//,19283
//,52737
//,10711
//,36287
//,1601
//,75021
//,59133
//,90313
//,68113
//,11033
//,58591
//,55121
//,48662
//,31124
//,34896
//,23459
//,16811
//,70213
//,47793
//,4581
//,85800
//,81864
//,48385
//,39425
//,6529
//,69249
//,5997
//,71817
//,43905
//,24847
//,67032
//,88857
//,43334
//,46051
//,47440
//,18041
//,96833
//,33642
//,90900
//,38661
//,91801
//,76369
//,59285
//,53321
//,86645
//,23757
//,86126
//,41523
//,43787
//,90151
//,15777
//,60381
//,58721
//,90865
//,97267
//,38335
//,11047
//,61016
//,29765
//,1741
//,23234
//,32101
//,36805
//,15070
//,39769
//,77212
//,55777
//,50069
//,26126
//,81703
//,42273
//,91873
//,27681
//,84662
//,50041
//,21821
//,57583
//,73983
//,63408
//,34325
//,351
//,94329
//,7116
//,8658
//,8409
//,58537
//,74852
//,14001
//,86569
//,39805
//,49746
//,42343
//,77675
//,22659
//,44391
//,86559
//,47805
//,21057
//,57169
//,46595
//,74993
//,52741
//,50549
//,82561
//,12901
//,29141
//,60274
//,52501
//,66257
//,23321
//,20548
//,94241
//,36561
//,88211
//,27265
//,78831
//,68833
//,82443
//,9663
//,32003
//,65317
//,72915
//,32407
//,10944
//,22315
//,68986
//,47483
//,70452
//,68100
//,49446
//,19517
//,29347
//,36961
//,50003
//,27771
//,2929
//,12376
//,54341
//,68931
//,57934
//,85594
//,7801
//,65028
//,83914
//,29649
//,4717
//,6991
//,33066
//,8996
//,6111
//,69217
//,99345
//,6081
//,17821
//,51603
//,393
//,59612
//,7805
//,26251
//,53905
//,34169
//,51243
//,84551
//,11254
//,89029
//,71782
//,20081
//,17322
//,72013
//,19534
//,77061
//,58783
//,54109
//,99564
//,20653
//,93460
//,23463
//,48701
//,1201
//,78291
//,15321
//,1985
//,64252
//,19999
//,25886
//,34877
//,96389
//,43237
//,97241
//,19592
//,19443
//,20943
//,53799
//,673
//,16361
//,18069
//,85801
//,80797
//,66633
//,63633
//,17902
//,98592
//,156
//,86717
//,38799
//,44393
//,18016
//,76819
//,11067
//,22051
//,52881
//,4862
//,19943
//,5606
//,88433
//,34941
//,17821
//,49312
//,6021
//,74227
//,82325
//,23561
//,79606
//,44737
//,5257
//,39589
//,8367
//,59832
//,10497
//,89887
//,65305
//,12816
//,6909
//,91861
//,39401
//,24409
//,32307
//,54959
//,1113
//,52442
//,97325
//,16216
//,80083
//,65893
//,61033
//,88443
//,49323
//,73423
//,35671
//,34509
//,96760
//,5451
//,61622
//,25877
//,70226
//,61917
//,58977
//,73085
//,12456
//,82811
//,24704
//,6624
//,8115
//,76701
//,22485
//,87430
//,10209
//,75279
//,65187
//,84838
//,54936
//,24006
//,98853
//,12188
//,96279
//,6142
//,13849
//,20501
//,17606
//,81953
//,77765
//,33065
//,22517
//,11695
//,38843
//,54649
//,38017
//,52721
//,46785
//,60903
//,18673
//,28444
//,85761
//,64706
//,56093
//,80647
//,37617
//,1479
//,60755
//,30030
//,72379
//,46395
//,7901
//,51529
//,79415
//,72799
//,37973
//,24111
//,26355
//,24201
//,34986
//,2009
//,12201
//,44245
//,19670
//,11455
//,66429
//,41217
//,60598
//,24705
//,53447
//,16856
//,69283
//,40726
//,57304
//,43261
//,8261
//,98954
//,60656
//,11305
//,88817
//,80043
//,73483
//,73597
//,8136
//,76861
//,14949
//,8551
//,92195
//,96282
//,13278
//,31555
//,6486
//,82977
//,63020
//,52821
//,70997
//,93601
//,55105
//,93073
//,49123
//,54603
//,23145
//,65505
//,40139
//,33057
//,57509
//,76143
//,28888
//,21710
//,10471
//,84601
//,39201
//,16376
//,13291
//,62165
//,86649
//,18661
//,32273
//,43953
//,8305
//,68958
//,73831
//,66681
//,78201
//,6401
//,90841
//,49543
//,78390
//,76322
//,66439
//,24673
//,12327
//,7081
//,46825
//,10718
//,63098
//,68679
//,60027
//,49771
//,78346
//,52881
//,57121
//,9091
//,86446
//,67873
//,36737
//,39224
//,71324
//,82714
//,68831
//,82250
//,10501
//,69674
//,85305
//,18153
//,55539
//,60593
//,65153
//,20161
//,49940
//,45251
//,11905
//,59955
//,95931
//,49236
//,61860
//,58119
//,75479
//,27793
//,89001
//,78654
//,87191
//,62219
//,82312
//,23591
//,70425
//,74565
//,3111
//,89813
//,69957
//,68416
//,11339
//,40229
//,90341
//,95281
//,56304
//,12113
//,50801
//,65140
//,27241
//,20832
//,96461
//,9489
//,44557
//,12801
//,65503
//,40610
//,15334
//,98277
//,5121
//,17181
//,10621
//,14191
//,1321
//,99931
//,1761
//,391
//,89736
//,29305
//,79603
//,11901
//,61256
//,46295
//,933
//,11129
//,34907
//,52710
//,1602
//,23217
//,25943
//,23963
//,10201
//,5651
//,64254
//,65791
//,91841
//,21127
//,83201
//,29751
//,9796
//,32981
//,72658
//,20089
//,85441
//,85771
//,75673
//,96377
//,35553
//,10116
//,37665
//,76289
//,42241
//,75591
//,14921
//,87057
//,93629
//,47897
//,20153
//,40701
//,77758
//,94241
//,52811
//,22534
//,13778
//,15378
//,79673
//,57601
//,16149
//,3800
//,90497
//,15699
//,73665
//,70881
//,83951
//,17668
//,52326
//,73627
//,56953
//,17076
//,4647
//,67628
//,87959
//,88345
//,69337
//,30013
//,43296
//,982
//,21367
//,7661
//,67841
//,63294
//,24101
//,58913
//,67229
//,35396
//,47761
//,7422
//,72359
//,75440
//,52919
//,14186
//,84740
//,11485
//,56579
//,84603
//,41367
//,33145
//,22241
//,11169
//,66017
//,57422
//,52384
//,70828
//,57649
//,23403
//,32763
//,48721
//,64859
//,53106
//,95741
//,19201
//,81795
//,32516
//,54561
//,42841
//,90323
//,29625
//,75773
//,36931
//,74968
//,58013
//,36205
//,68286
//,99761
//,23879
//,19359
//,17089
//,13978
//,26689
//,70865
//,25551
//,42972
//,63976
//,98495
//,85381
//,76165
//,21666
//,37304
//,21841
//,62048
//,5693
//,81539
//,29515
//,20534
//,20098
//,16675
//,87733
//,6529
//,75441
//,14651
//,96226
//,14376
//,15809
//,62590
//,95626
//,13401
//,81138
//,73775
//,19361
//,13137
//,38553
//,53617
//,54851
//,41262
//,71911
//,98425
//,10887
//,46829
//,91202
//,29929
//,15605
//,22495
//,18976
//,12657
//,62750
//,32801
//,56406
//,83697
//,13313
//,55247
//,3491
//,10627
//,36049
//,84245
//,48105
//,48185
//,9869
//,74077
//,53361
//,34808
//,37961
//,47261
//,40405
//,91497
//,13849
//,95084
//,80991
//,35289
//,78089
//,38141
//,32817
//,86833
//,65394
//,84596
//,74169
//,8897
//,25281
//,86852
//,18154
//,68633
//,15676
//,10620
//,48760
//,73497
//,2501
//,48877
//,53577
//,40025
//,48598
//,61557
//,19659
//,46153
//,19174
//,27376
//,89833
//,61559
//,4809
//,12740
//,74897
//,12253
//,6961
//,67762
//,37121
//,54243
//,4941
//,22951
//,38821
//,36885
//,22166
//,12047
//,12657
//,60517
//,32339
//,59609
//,33701
//,46144
//,2243
//,33309
//,91479
//,7837
//,31119
//,22597
//,20865
//,50737
//,78091
//,2720
//,19423
//,79081
//,93077
//,20115
//,96881
//,72917
//,28935
//,92423
//,65761
//,92828
//,67089
//,99376
//,5615
//,61200
//,12696
//,46913
//,28494
//,20617
//,84815
//,33385
//,59641
//,99461
//,30865
//,17415
//,58201
//,35173
//,81327
//,2070
//,78231
//,11081
//,66469
//,40161
//,24900
//,33209
//,1265
//,70464
//,34751
//,2626
//,31401
//,82325
//,22555
//,32947
//,80746
//,40019
//,58009
//,19245
//,80111
//,97711
//,51233
//,78545
//,15564
//,9005
//,9937
//,2276
//,94431
//,43713
//,41425
//,11075
//,26141
//,57994
//,45401
//,31931
//,65031
//,15214
//,5985
//,57827
//,77937
//,24835
//,9586
//,86324
//,62013
//,93876
//,63585
//,86761
//,6316
//,82321
//,46861
//,74590
//,559
//,89521
//,97470
//,61825
//,52431
//,98515
//,4156
//,16145
//,97076
//,28992
//,76301
//,18200
//,25211
//,12385
//,10847
//,94751
//,94583
//,17281
//,12656
//,65
//,70401
//,89130
//,73365
//,5766
//,37985
//,19934
//,66165
//,46481
//,44243
//,24561
//,14466
//,123
//,67085
//,46306
//,86976
//,53250
//,4966
//,88015
//,87106
//,54873
//,80161
//,12629
//,54601
//,23132
//,3015
//,79030
//,71057
//,79572
//,19787
//,345
//,15606
//,25686
//,2353
//,5927
//,19327
//,30426
//,66432
//,1671
//,98167
//,37056
//,84391
//,66584
//,17001
//,37041
//,64041
//,93478
//,86666
//,74701
//,37138
//,66321
//,27761
//,10929
//,92333
//,91665
//,95646
//,45301
//,6211
//,79977
//,67361
//,31054
//,92161
//,43295
//,4011
//,1617
//,87033
//,42535
//,78121
//,99086
//,29930
//,97857
//,18857
//,18051
//,2347
//,70301
//,67060
//,16049
//,89951
//,25145
//,1046
//,8760
//,45423
//,21372
//,94039
//,40857
//,67782
//,70568
//,91181
//,69467
//,77021
//,34077
//,5309
//,81737
//,50963
//,92707
//,29645
//,8621
//,298
//,85793
//,90833
//,99349
//,84082
//,79621
//,95712
//,16671
//,29669
//,15689
//,8105
//,41085
//,50741
//,99869
//,87306
//,5425
//,75389
//,41329
//,26235
//,35877
//,11133
//,97391
//,96521
//,47670
//,50473
//,84761
//,40375
//,2141
//,44163
//,29321
//,27499
//,68309
//,16841
//,20077
//,61426
//,40181
//,3841
//,37050
//,11050
//,38941
//,48248
//,37537
//,43933
//,86499
//,68097
//,54566
//,34041
//,7361
//,63892
//,31174
//,19339
//,27023
//,18649
//,7473
//,38220
//,46886
//,39081
//,41573
//,85481
//,23862
//,92073
//,29869
//,45627
//,71287
//,5904
//,93211
//,52889
//,93096
//,65511
//,33189
//,66820
//,90388
//,60681
//,97176
//,69681
//,57860
//,36777
//,38784
//,10806
//,75739
//,52396
//,87041
//,36682
//,95821
//,18981
//,42651
//,33307
//,92051
//,68016
//,56789
//,94427
//,20280
//,31701
//,48035
//,96601
//,79337
//,24713
//,41575
//,43719
//,37659
//,681
//,31489
//,97891
//,81953
//,87105
//,63797
//,41685
//,40891
//,51821
//,67359
//,98189
//,79279
//,25911
//,37241
//,4257
//,30721
//,20761
//,94791
//,5961
//,21877
//,30646
//,91021
//,90376
//,96879
//,8709
//,8081
//,38616
//,17571
//,67333
//,70063
//,5151
//,70001
//,99487
//,77045
//,92731
//,84101
//,3862
//,66277
//,17459
//,99926
//,95515
//,99630
//,73246
//,82926
//,83539
//,31187
//,97955
//,81905
//,55219
//,12065
//,56113
//,58681
//,43425
//,25944
//,13908
//,90981
//,23411
//,38389
//,45345
//,18689
//,97287
//,23672
//,44
//,8073
//,97801
//,40003
//,38553
//,37658
//,3053
//,39295
//,73143
//,90255
//,54556
//,24018
//,69156
//,87933
//,71861
//,46685
//,92011
//,51890
//,19521
//,85006
//,20186
//,15273
//,41275
//,14021
//,30641
//,80275
//,32749
//,83128
//,92575
//,38828
//,79481
//,4401
//,53426
//,8767
//,19265
//,75505
//,12521
//,48360
//,85421
//,62224
//,27017
//,49521
//,93726
//,68401
//,45874
//,15153
//,93949
//,37624
//,44830
//,63399
//,57188
//,40001
//,10711
//,67021
//,77876
//,25363
//,8831
//,44993
//,5197
//,83606
//,27197
//,56321
//,87441
//,68241
//,32691
//,34024
//,5961
//,16841
//,84751
//,64009
//,80417
//,91507
//,90223
//,81725
//,81803
//,38621
//,24926
//,60995
//,23540
//,2441
//,11881
//,19901
//,25171
//,48950
//,12988
//,55154
//,61556
//,20665
//,7575
//,16723
//,12053
//,8593
//,89071
//,13249
//,52202
//,84511
//,82133
//,49203
//,45616
//,72453
//,36915
//,1519
//,43232
//,16309
//,87431
//,64641
//,67891
//,58401
//,23530
//,38021
//,9701
//,68561
//,4806
//,18317
//,81009
//,76572
//,21197
//,25183
//,6027
//,91847
//,65713
//,2834
//,34231
//,98365
//,76791
//,32681
//,46061
//,97519
//,45315
//,60969
//,19401
//,29701
//,41676
//,10805
//,75039
//,79731
//,32951
//,87887
//,50630
//,94421
//,14761
//,425
//,30121
//,90656
//,52593
//,70761
//,85151
//,66771
//,34789
//,71939
//,93306
//,23871
//,5161
//,19221
//,52927
//,65811
//,43121
//,48289
//,80740
//,16739
//,15281
//,91273
//,57473
//,96817
//,12117
//,90273
//,61117
//,58496
//,43251
//,84704
//,84740
//,99268
//,13813
//,78605
//,20067
//,84571
//,78338
//,26201
//,6893
//,37951
//,44577
//,86642
//,10601
//,23038
//,28126
//,67615
//,29775
//,67393
//,85496
//,36353
//,27187
//,1201
//,14241
//,39837
//,62181
//,24016
//,99773
//,32248
//,9485
//,95887
//,80279
//,18211
//,27141
//,33703
//,24481
//,13745
//,39441
//,57497
//,83749
//,70801
//,19810
//,81521
//,99521
//,1569
//,58873
//,50506
//,83386
//,35001
//,94235
//,1841
//,75273
//,6094
//,68865
//,34686
//,40672
//,35177
//,94017
//,44346
//,96458
//,38781
//,64993
//,11945
//,20855
//,81046
//,73777
//,46439
//,88446
//,52181
//,78320
//,67471
//,38402
//,93110
//,1366
//,88346
//,1537
//,43713
//,71073
//,97251
//,20941
//,39097
//,22719
//,76936
//,47468
//,57921
//,71231
//,30497
//,63926
//,58031
//,14770
//,92232
//,99697
//,53498
//,45369
//,88901
//,23056
//,85651
//,57625
//,8261
//,13136
//,20917
//,96459
//,65281
//,4368
//,58321
//,79393
//,36524
//,18744
//,10901
//,30705
//,35166
//,67726
//,15741
//,96071
//,49465
//,97361
//,62371
//,14077
//,30915
//,37228
//,35905
//,32333
//,81396
//,87901
//,53809
//,44561
//,80008
//,6005
//,40761
//,81635
//,20467
//,36315
//,73845
//,9661
//,38246
//,71005
//,40307
//,87362
//,47665
//,86709
//,48368
//,89921
//,38487
//,50121
//,1057
//,94957
//,65769
//,82730
//,68761
//,38878
//,75501
//,80987
//,86721
//,68131
//,52828
//,15044
//,38321
//,56571
//,45473
//,1878
//,93901
//,63376
//,16676
//,33646
//,46281
//,55441
//,17641
//,74954
//,60094
//,14129
//,18127
//,7746
//,56545
//,45581
//,4072
//,71504
//,18806
//,4655
//,1185
//,98701
//,49639
//,60017
//,15186
//,84341
//,66096
//,60545
//,29961
//,87737
//,95261
//,15554
//,60561
//,93709
//,71821
//,8507
//,57807
//,96778
//,73232
//,93582
//,93914
//,70353
//,26883
//,8946
//,84801
//,80681
//,44753
//,98597
//,13183
//,59721
//,29441
//,86459
//,46149
//,1901
//,14047
//,23869
//,98061
//,27051
//,47969
//,34926
//,59584
//,2205
//,56481
//,11731
//,44989
//,78929
//,47668
//,44751
//,96265
//,42337
//,67062
//,27229
//,12869
//,25331
//,11749
//,49851
//,40705
//,55281
//,29225
//,22297
//,46131
//,48961
//,12699
//,59425
//,35375
//,85153
//,33577
//,76201
//,25799
//,18101
//,33894
//,35832
//,12649
//,30953
//,99169
//,42673
//,54504
//,82411
//,15467
//,62633
//,35513
//,75661
//,39745
//,66815
//,17194
//,14471
//,21365
//,67574
//,91541
//,59729
//,52257
//,581
//,21809
//,25233
//,1123
//,84775
//,32109
//,37313
//,26573
//,99538
//,75293
//,46131
//,65857
//,32357
//,89031
//,99401
//,90140
//,86690
//,45633
//,29697
//,45339
//,41143
//,50932
//,3197
//,32148
//,5441
//,72476
//,63549
//,32733
//,23413
//,50735
//,66964
//,30393
//,93875
//,13121
//,29245
//,23536
//,30609
//,9581
//,73236
//,53965
//,94391
//,28095
//,19461
//,19473
//,42219
//,10131
//,29911
//,38481
//,65028
//,24925
//,64501
//,43693
//,90497
//,28349
//,36318
//,33908
//,28369
//,80611
//,13681
//,62321
//,45771
//,26331
//,19660
//,20871
//,60353
//,84291
//,36441
//,40112
//,3697
//,69135
//,8922
//,9441
//,40141
//,49689
//,72785
//,95031
//,84081
//,44453
//,27889
//,5826
//,48529
//,10467
//,41876
//,56576
//,88976
//,30655
//,38020
//,87197
//,66485
//,96401
//,88993
//,30444
//,51025
//,67391
//,40033
//,66297
//,29715
//,88169
//,46316
//,70161
//,7062
//,12161
//,1021
//,56639
//,5691
//,48608
//,33625
//,6328
//,4305
//,2739
//,75993
//,32641
//,43391
//,84864
//,51513
//,11623
//,51715
//,30001
//,2731
//,81275
//,91489
//,25577
//,47948
//,84529
//,22505
//,81705
//,39281
//,5441
//,64525
//,96377
//,89719
//,98004
//,96125
//,83400
//,96289
//,60193
//,1326
//,14430
//,83313
//,81205
//,26080
//,30375
//,3121
//,44001
//,5941
//,79655
//,20614
//,37261
//,56001
//,35820
//,89094
//,7499
//,16291
//,82701
//,65967
//,84884
//,38753
//,80988
//,96699
//,10801
//,90986
//,77073
//,16401
//,5709
//,18484
//,63209
//,14201
//,16417
//,65539
//,33603
//,72473
//,801
//,28679
//,49828
//,64371
//,74317
//,93697
//,861
//,6541
//,36533
//,73652
//,78321
//,57066
//,20371
//,59819
//,82112
//,74073
//,15634
//,86116
//,73039
//,94155
//,60489
//,70031
//,19201
//,56092
//,37091
//,59089
//,72471
//,35725
//,60421
//,1617
//,86273
//,33919
//,67004
//,12097
//,81245
//,5921
//,92751
//,30764
//,16607
//,94670
//,69980
//,1501
//,90351
//,36897
//,12915
//,77802
//,71097
//,75974
//,15681
//,29625
//,79339
//,58493
//,37904
//,27876
//,27681
//,74826
//,66321
//,35837
//,61657
//,98987
//,15709
//,83051
//,2221
//,40533
//,28961
//,68194
//,71135
//,45806
//,36517
//,92521
//,9776
//,79003
//,98401
//,95427
//,70554
//,65461
//,14858
//,89297
//,44327
//,39468
//,53581
//,44657
//,34331
//,35258
//,71419
//,18897
//,30126
//,48547
//,23169
//,69551
//,85250
//,90356
//,51697
//,73243
//,86412
//,38445
//,96505
//,51408
//,99446
//,89001
//,12181
//,69451
//,1483
//,57360
//,89465
//,45426
//,91569
//,51361
//,93487
//,50811
//,60334
//,48653
//,73101
//,70369
//,87970
//,32502
//,54177
//,14817
//,2098
//,8089
//,53905
//,96817
//,93625
//,96601
//,74241
//,92541
//,19201
//,29017
//,31483
//,24612
//,65903
//,93293
//,49201
//,66086
//,6081
//,6427
//,31401
//,54438
//,59216
//,22878
//,661
//,30077
//,25861
//,38667
//,45887
//,46369
//,46073
//,49751
//,27757
//,42115
//,49421
//,92589
//,86081
//,98297
//,69671
//,69197
//,12914
//,26845
//,25969
//,80757
//,61940
//,65691
//,1297
//,76257
//,747
//,83831
//,41753
//,99997
//,1240
//,3435
//,94451
//,16864
//,39889
//,83476
//,74397
//,32611
//,28242
//,58156
//,22831
//,42665
//,43490
//,63391
//,15891
//,44697
//,35290
//,43838
//,58125
//,78083
//,47560
//,19393
//,39594
//,33922
//,18751
//,4789
//,93237
//,90141
//,91021
//,17149
//,7544
//,59649
//,82594
//,56157
//,93317
//,19915
//,62126
//,45069
//,16843
//,42981
//,26695
//,39421
//,68161
//,11091
//,39953
//,63697
//,55204
//,44066
//,7121
//,89949
//,66229
//,89122
//,72621
//,54847
//,35901
//,30927
//,62391
//,43601
//,14906
//,1001
//,28471
//,14933
//,33925
//,44969
//,30697
//,99900
//,27419
//,67814
//,88609
//,49351
//,41875
//,49867
//,8779
//,45501
//,75033
//,17953
//,6155
//,29457
//,60852
//,2261
//,66953
//,93920
//,54576
//,39413
//,64735
//,3771
//,39645
//,40113
//,78929
//,20817
//,3747
//,30507
//,59869
//,68776
//,15001
//,22399
//,86048
//,36991
//,9881
//,10026
//,3675
//,79597
//,50861
//,99979
//,95801
//,31013
//,49111
//,68665
//,8376
//,98301
//,62797
//,80931
//,21131
//,39513
//,57368
//,79351
//,82653
//,47121
//,89911
//,15949
//,28215
//,80373
//,57895
//,67226
//,51085
//,73985
//,12441
//,53846
//,68996
//,96501
//,64757
//,26921
//,56668
//,70017
//,53102
//,91771
//,39209
//,50691
//,9152
//,47169
//,25521
//,78029
//,17481
//,99995
//,62465
//,9697
//,81315
//,94863
//,42195
//,54181
//,95341
//,47295
//,8325
//,9759
//,9177
//,37111
//,4057
//,82893
//,83160
//,23140
//,50245
//,55171
//,90249
//,11149
//,8803
//,686
//,66637
//,53377
//,15343
//,28968
//,37921
//,76976
//,59403
//,74581
//,20213
//,85329
//,28922
//,38439
//,28749
//,66865
//,92033
//,59383
//,24346
//,16681
//,39322
//,37406
//,43251
//,1916
//,53935
//,5131
//,67405
//,54420
//,6661
//,11649
//,17801
//,60141
//,71694
//,69208
//,68169
//,12887
//,25025
//,33525
//,98201
//,31041
//,63406
//,31812
//,36093
//,67681
//,72149
//,64601
//,7870
//,80376
//,85118
//,76937
//,82377
//,95775
//,5404
//,4513
//,13921
//,13328
//,82672
//,61621
//,41013
//,45607
//,66625
//,65413
//,67749
//,75038
//,93679
//,96258
//,74925
//,7581
//,51526
//,48329
//,96639
//,37761
//,81937
//,5698
//,90721
//,77839
//,82635
//,62401
//,19841
//,27290
//,36901
//,91776
//,2532
//,26293
//,40486
//,79197
//,87021
//,71219
//,27981
//,28681
//,63719
//,82401
//,7408
//,82529
//,13821
//,3972
//,60257
//,53709
//,93423
//,6401
//,7351
//,49663
//,47937
//,60449
//,17291
//,99614
//,82420
//,98071
//,22989
//,66670
//,68473
//,47555
//,69261
//,99351
//,36617
//,81965
//,47058
//,77320
//,89193
//,14464
//,79449
//,86417
//,16496
//,21300
//,62514
//,68305
//,76496
//,69115
//,21025
//,41953
//,54731
//,24821
//,22584
//,22281
//,57913
//,76805
//,83045
//,96626
//,15709
//,52641
//,27640
//,25849
//,88581
//,72301
//,63201
//,93587
//,4853
//,42693
//,46126
//,61033
//,43321
//,39389
//,52965
//,63481
//,44315
//,50639
//,69715
//,67107
//,36403
//,11182
//,41968
//,14566
//,76955
//,8737
//,25558
//,7265
//,93313
//,87661
//,2836
//,3911
//,6545
//,44651
//,85375
//,59636
//,67293
//,47419
//,25033
//,26061
//,20865
//,81526
//,30041
//,27671
//,88598
//,36621
//,86624
//,85426
//,28421
//,62709
//,92132
//,5417
//,7426
//,19626
//,74881
//,32817
//,90601
//,59493
//,55751
//,64858
//,52633
//,2339
//,39199
//,75422
//,96163
//,70423
//,4541
//,68129
//,97661
//,97611
//,95718
//,5136
//,61774
//,3051
//,12031
//,11647
//,18143
//,22581
//,52529
//,10449
//,39403
//,55247
//,21753
//,51703
//,31397
//,52921
//,77505
//,57153
//,11001
//,55036
//,43569
//,64196
//,24811
//,91999
//,98737
//,50285
//,76683
//,80441
//,4865
//,41645
//,74462
//,74849
//,72049
//,41399
//,11377
//,92577
//,62784
//,4135
//,97472
//,5281
//,15865
//,17841
//,58976
//,25996
//,77585
//,39133
//,63521
//,12501
//,90641
//,1595
//,9421
//,65181
//,80745
//,46942
//,81445
//,27373
//,46681
//,89317
//,43814
//,74287
//,83901
//,86521
//,15476
//,56079
//,71153
//,22276
//,43601
//,18391
//,72045
//,77217
//,46289
//,35358
//,23113
//,18498
//,6919
//,24733
//,14197
//,13242
//,27445
//,79009
//,79137
//,78345
//,19717
//,38510
//,96762
//,70241
//,23508
//,31989
//,38629
//,17219
//,46498
//,69769
//,79017
//,24809
//,11051
//,2609
//,69935
//,56111
//,61886
//,55633
//,48705
//,31691
//,22829
//,92842
//,38856
//,1180
//,68121
//,49761
//,87537
//,65214
//,68673
//,54003
//,67649
//,39087
//,73545
//,4576
//,41461
//,71501
//,76111
//,97071
//,55137
//,29281
//,76479
//,22316
//,59478
//,15721
//,9097
//,46401
//,38359
//,70232
//,57855
//,33265
//,20265
//,73873
//,78857
//,49563
//,61541
//,72551
//,8877
//,47439
//,32557
//,36946
//,25359
//,74416
//,370
//,61665
//,24579
//,95115
//,49405
//,6529
//,22177
//,51876
//,48609
//,649
//,22601
//,45669
//,42810
//,2726
//,27196
//,45245
//,84101
//,41768
//,37357
//,41561
//,24135
//,52486
//,20793
//,6519
//,20601
//,73159
//,73896
//,1207
//,40061
//,23097
//,65981
//,40059
//,46595
//,61901
//,63309
//,65131
//,61129
//,36833
//,43255
//,61613
//,4590
//,93153
//,14501
//,75571
//,57517
//,4303
//,83217
//,8898
//,63697
//,79696
//,5815
//,48061
//,53676
//,36785
//,42143
//,28243
//,88131
//,26327
//,50409
//,35433
//,35979
//,82963
//,72747
//,7006
//,74279
//,35601
//,92657
//,18331
//,92275
//,95951
//,86651
//,72421
//,95321
//,82241
//,5576
//,5857
//,89248
//,16313
//,85362
//,45451
//,66621
//,66237
//,7883
//,29815
//,12751
//,94360
//,877
//,67894
//,74376
//,10893
//,96833
//,24561
//,43661
//,80765
//,52019
//,29063
//,47231
//,98776
//,39610
//,34156
//,23175
//,34496
//,46451
//,89364
//,13099
//,86223
//,59331
//,81341
//,19097
//,92265
//,35957
//,22377
//,23023
//,40120
//,52939
//,29711
//,46121
//,83841
//,31505
//,55521
//,14023
//,31144
//,70229
//,94205
//,13261
//,48577
//,79495
//,42065
//,79727
//,19401
//,72545
//,61849
//,60568
//,50501
//,66754
//,44726
//,39841
//,43065
//,48487
//,40559
//,7273
//,83789
//,9877
//,90746
//,77115
//,55521
//,41296
//,83390
//,59105
//,13569
//,84883
//,34001
//,76961
//,83785
//,5352
//,39661
//,78323
//,37602
//,56561
//,77165
//,45057
//,40593
//,37226
//,47801
//,17160
//,35641
//,87126
//,677
//,3875
//,64204
//,93185
//,10638
//,67018
//,96525
//,2085
//,95519
//,65977
//,66828
//,75713
//,50419
//,48351
//,43011
//,2487
//,18041
//,19939
//,83309
//,95193
//,63746
//,1237
//,63111
//,99753
//,81975
//,59977
//,4395
//,74016
//,94681
//,21996
//,49985
//,93866
//,8376
//,69811
//,55894
//,91009
//,3626
//,82263
//,97713
//,24089
//,66468
//,82839
//,97158
//,93037
//,85101
//,60833
//,36293
//,72425
//,12921
//,42315
//,24278
//,61421
//,13934
//,47087
//,61293
//,17091
//,32671
//,22695
//,47021
//,14513
//,16056
//,27669
//,65276
//,22449
//,15485
//,63420
//,45663
//,31815
//,39053
//,3441
//,71671
//,66144
//,85218
//,3626
//,29141
//,92057
//,71597
//,19101
//,7714
//,98278
//,98086
//,5143
//,6713
//,41835
//,58401
//,1961
//,10941
//,6859
//,68543
//,93452
//,88446
//,51098
//,25234
//,36741
//,52996
//,95341
//,40207
//,33684
//,14125
//,96424
//,70379
//,73341
//,62107
//,773
//,28913
//,76876
//,52236
//,3884
//,92271
//,21409
//,14301
//,35818
//,57921
//,5281
//,51021
//,28591
//,94575
//,53551
//,26896
//,53251
//,26411
//,34549
//,74746
//,78283
//,57233
//,81351
//,78577
//,13725
//,14830
//,44901
//,37896
//,74951
//,94609
//,98081
//,57623
//,25341
//,6305
//,7433
//,17736
//,10211
//,72712
//,83301
//,8065
//,11195
//,62163
//,39537
//,71089
//,18703
//,50229
//,18094
//,35933
//,26417
//,19073
//,45234
//,31513
//,44339
//,21255
//,95745
//,94486
//,61257
//,75849
//,78891
//,73336
//,72537
//,20916
//,97937
//,83493
//,35629
//,97029
//,65525
//,44641
//,29305
//,61756
//,79816
//,43841
//,63101
//,15425
//,56477
//,26288
//,90189
//,81130
//,5967
//,9819
//,11481
//,95979
//,63835
//,23811
//,3084
//,14873
//,68735
//,7821
//,61461
//,93781
//,22286
//,86953
//,81421
//,80001
//,67020
//,84291
//,41151
//,6444
//,32445
//,90931
//,96453
//,829
//,58365
//,83830
//,38146
//,94755
//,42541
//,51035
//,34423
//,68520
//,27121
//,31980
//,70697
//,86555
//,17097
//,98556
//,87107
//,76375
//,86470
//,32675
//,38164
//,59048
//,25510
//,22554
//,46397
//,8715
//,43323
//,84192
//,84444
//,42941
//,30205
//,88161
//,14991
//,54356
//,22110
//,48816
//,25881
//,45081
//,63721
//,2701
//,11165
//,81153
//,8313
//,69971
//,38733
//,629
//,28577
//,72855
//,86777
//,51562
//,14999
//,24213
//,77273
//,9242
//,90877
//,43915
//,19558
//,17761
//,28556
//,77133
//,37485
//,6857
//,51041
//,91773
//,39488
//,37449
//,56525
//,30947
//,78901
//,13023
//,94521
//,95201
//,83811
//,81266
//,10586
//,20041
//,93481
//,43289
//,55879
//,64427
//,12141
//,28161
//,66141
//,12311
//,29651
//,2654
//,10189
//,88249
//,43738
//,95631
//,50031
//,44858
//,82299
//,5641
//,10646
//,25462
//,40021
//,4805
//,26109
//,13191
//,88973
//,56865
//,81035
//,78425
//,73169
//,42014
//,24197
//,34421
//,93001
//,49435
//,21104
//,99561
//,60036
//,26193
//,24791
//,25011
//,47003
//,2081
//,46017
//,61901
//,79065
//,12881
//,65126
//,44411
//,16599
//,78045
//,58236
//,93061
//,35831
//,31673
//,94933
//,49171
//,83934
//,8572
//,73793
//,5473
//,64833
//,7609
//,26837
//,55010
//,11161
//,57525
//,37101
//,27163
//,28800
//,78079
//,7081
//,45869
//,14357
//,17129
//,77505
//,59621
//,37425
//,41928
//,30681
//,60705
//,32171
//,33151
//,99343
//,42687
//,51491
//,58753
//,30144
//,71081
//,81085
//,12322
//,28022
//,37169
//,48423
//,96399
//,66649
//,64401
//,63966
//,16267
//,86932
//,21801
//,11241
//,58974
//,82256
//,14076
//,52546
//,69281
//,93127
//,45565
//,26403
//,95603
//,22671
//,72911
//,64960
//,98954
//,77577
//,91521
//,61819
//,33819
//,82491
//,23556
//,83245
//,92953
//,67372
//,45007
//,36493
//,7576
//,29238
//,2689
//,35479
//,75831
//,41345
//,80876
//,40057
//,22946
//,28409
//,16885
//,84059
//,50824
//,25354
//,4571
//,25316
//,7351
//,5548
//,32657
//,27009
//,77161
//,81635
//,35367
//,99001
//,31326
//,60705
//,99131
//,62107
//,61329
//,68181
//,40866
//,5449
//,41051
//,29513
//,24061
//,84588
//,87199
//,75404
//,71205
//,65601
//,23598
//,62413
//,72369
//,18161
//,10217
//,16057
//,12609
//,58721
//,52612
//,63393
//,52975
//,71121
//,417
//,29214
//,50699
//,13713
//,44827
//,89912
//,86256
//,65619
//,22151
//,81038
//,1031
//,88401
//,55701
//,7397
//,53105
//,3989
//,53205
//,22566
//,8322
//,80766
//,21625
//,97667
//,85661
//,18701
//,16021
//,60607
//,17035
//,72365
//,62682
//,94309
//,88628
//,87641
//,18986
//,57217
//,69525
//,25441
//,68099
//,50001
//,18826
//,44489
//,60709
//,74746
//,57457
//,45741
//,26329
//,52503
//,42671
//,40513
//,30285
//,79963
//,87621
//,64673
//,14386
//,68481
//,61465
//,56459
//,8373
//,68306
//,57711
//,11701
//,42911
//,75001
//,79502
//,38721
//,79501
//,31973
//,93761
//,84530
//,72004
//,95801
//,90981
//,90292
//,30656
//,15945
//,70090
//,47589
//,74009
//,74626
//,35772
//,71821
//,4147
//,19224
//,3282
//,16449
//,149
//,55922
//,23290
//,81056
//,12815
//,96636
//,42253
//,97151
//,29303
//,23253
//,15426
//,94101
//,70221
//,77321
//,80811
//,20817
//,76145
//,38911
//,4732
//,28098
//,35196
//,83835
//,64244
//,77541
//,78063
//,33619
//,3591
//,76281
//,1831
//,3073
//,33252
//,39151
//,46306
//,89703
//,29326
//,89639
//,59549
//,51251
//,30171
//,47541
//,1675
//,52851
//,94991
//,62066
//,18290
//,5071
//,10
//,5976
//,56657
//,91600
//,92029
//,36305
//,1297
//,46093
//,86453
//,93711
//,3522
//,47986
//,56001
//,71426
//,91171
//,15329
//,1875
//,28577
//,77320
//,99041
//,33681
//,40233
//,61671
//,57329
//,88201
//,56772
//,11107
//,24415
//,4001
//,99331
//,61
//,69891
//,38169
//,41
//,69065
//,13935
//,73349
//,17436
//,92796
//,78886
//,3007
//,76847
//,53791
//,21748
//,81043
//,73289
//,54651
//,68101
//,89871
//,94629
//,10451
//,27581
//,27986
//,37991
//,79621
//,47124
//,44895
//,15161
//,4466
//,45206
//,37186
//,48542
//,13208
//,61601
//,53713
//,7968
//,78961
//,26189
//,79571
//,12376
//,49001
//,9080
//,33841
//,68643
//,9660
//,99454
//,33285
//,62351
//,97573
//,18909
//,79061
//,4501
//,90034
//,22765
//,52131
//,77942
//,89071
//,27073
//,12866
//,42217
//,71809
//,17191
//,15877
//,73875
//,18071
//,5857
//,60541
//,22091
//,35521
//,60905
//,34741
//,13217
//,6991
//,73178
//,17385
//,36117
//,21231
//,26546
//,82402
//,23165
//,95843
//,63221
//,38465
//,2405
//,34937
//,58611
//,1791
//,72576
//,32689
//,88111
//,49808
//,61372
//,85707
//,81207
//,82763
//,3655
//,26576
//,25076
//,75382
//,17862
//,71641
//,12297
//,96929
//,7796
//,58203
//,39414
//,50188
//,23523
//,21953
//,4134
//,10117
//,11534
//,97927
//,34965
//,22189
//,82295
//,55543
//,56684
//,18397
//,64557
//,12769
//,6401
//,17192
//,64311
//,83825
//,5358
//,9808
//,27889
//,89217
//,14769
//,35683
//,94151
//,34526
//,5141
//,38695
//,73
//,15500
//,23370
//,20201
//,80813
//,64097
//,92201
//,36293
//,87608
//,77941
//,39266
//,60646
//,67868
//,52469
//,95555
//,73401
//,71902
//,53957
//,96189
//,83201
//,70766
//,46489
//,86256
//,58854
//,60105
//,23371
//,41281
//,57729
//,83773
//,35479
//,2426
//,30158
//,92748
//,39885
//,41313
//,29671
//,90399
//,11325
//,2858
//,51577
//,57981
//,13061
//,69949
//,75701
//,45477
//,91421
//,75496
//,52869
//,16513
//,753
//,52601
//,84131
//,42303
//,10929
//,82133
//,93586
//,80311
//,24155
//,99429
//,86693
//,67942
//,41761
//,83204
//,37713
//,32491
//,33675
//,99246
//,53702
//,64361
//,8601
//,40193
//,17721
//,71822
//,72169
//,48821
//,54031
//,66553
//,45137
//,92773
//,68104
//,20333
//,41412
//,80994
//,60725
//,82459
//,63081
//,99753
//,11987
//,46901
//,92257
//,14626
//,10226
//,16121
//,51958
//,97102
//,62729
//,62916
//,21177
//,68133
//,59731
//,66741
//,98163
//,46693
//,39223
//,29714
//,15401
//,95857
//,87801
//,26633
//,39389
//,36351
//,90471
//,19232
//,76351
//,94181
//,19461
//,79169
//,74773
//,51685
//,82601
//,12169
//,10635
//,53016
//,50926
//,98451
//,38181
//,84906
//,27067
//,36151
//,19521
//,10217
//,87581
//,80999
//,69801
//,88176
//,74695
//,91521
//,63452
//,19430
//,55809
//,77503
//,96696
//,90553
//,24591
//,36572
//,943
//,57539
//,96185
//,81321
//,11245
//,51521
//,3098
//,28053
//,43417
//,41717
//,70473
//,12329
//,17376
//,16854
//,29899
//,58781
//,72041
//,44333
//,72661
//,39441
//,87751
//,73926
//,44318
//,58131
//,96011
//,67341
//,99663
//,72331
//,60751
//,62229
//,90071
//,90609
//,88961
//,89289
//,77961
//,23272
//,54577
//,48269
//,8849
//,84815
//,43247
//,31331
//,30997
//,18401
//,91816
//,63469
//,16459
//,13323
//,88806
//,11138
//,6776
//,41905
//,54505
//,84698
//,59041
//,17601
//,60009
//,34245
//,91789
//,39653
//,52769
//,17123
//,87617
//,9647
//,73912
//,63257
//,26191
//,59281
//,50215
//,10306
//,81207
//,50813
//,56889
//,57741
//,35902
//,66956
//,36399
//,81281
//,15701
//,21665
//,25447
//,99325
//,27021
//,85101
//,93419
//,84745
//,78134
//,831
//,23921
//,68481
//,54722
//,76487
//,35990
//,77606
//,16841
//,68013
//,36990
//,63864
//,21537
//,32923
//,89969
//,5521
//,67639
//,23653
//,1506
//,43423
//,80453
//,5420
//,96181
//,24920
//,81751
//,56062
//,57203
//,88921
//,7069
//,73309
//,22897
//,42225
//,78261
//,99171
//,98932
//,71001
//,49621
//,10930
//,79320
//,27119
//,35101
//,9473
//,79167
//,48701
//,85131
//,58481
//,56851
//,65137
//,12158
//,15665
//,92739
//,74081
//,66077
//,32141
//,40101
//,56599
//,73121
//,25249
//,69777
//,57543
//,80143
//,47441
//,1311
//,43471
//,41920
//,53125
//,16015
//,69851
//,88649
//,35404
//,13381
//,18941
//,70109
//,1985
//,58069
//,64633
//,86776
//,60201
//,15275
//,7121
//,98561
//,2448
//,38707
//,30326
//,52481
//,20781
//,5667
//,78833
//,19609
//,5649
//,14501
//,88054
//,53773
//,91997
//,79001
//,99317
//,27807
//,30531
//,49825
//,96257
//,36621
//,51111
//,31469
//,91499
//,9345
//,148
//,5271
//,58101
//,79330
//,23019
//,23332
//,20937
//,41501
//,60569
//,53034
//,91796
//,57439
//,66531
//,20303
//,7345
//,16089
//,61525
//,79302
//,69451
//,36001
//,91571
//,54641
//,91001
//,92567
//,59231
//,28391
//,54074
//,88635
//,54546
//,26423
//,40066
//,99499
//,39621
//,17097
//,76546
//,85190
//,47016
//,35457
//,49651
//,64485
//,19135
//,57267
//,18569
//,35913
//,23201
//,93565
//,66463
//,14926
//,1217
//,86153
//,42613
//,52081
//,48687
//,38019
//,58932
//,81621
//,77131
//,76212
//,70321
//,43351
//,60574
//,48561
//,93845
//,26781
//,68490
//,87062
//,82863
//,10434
//,48691
//,41783
//,56342
//,87122
//,96741
//,71140
//,12273
//,34397
//,51217
//,31959
//,27256
//,96183
//,96426
//,43776
//,48215
//,25341
//,83671
//,18781
//,74505
//,84449
//,37776
//,66921
//,37852
//,65381
//,91361
//,58851
//,79011
//,66975
//,31936
//,23181
//,3195
//,50249
//,60225
//,93697
//,39405
//,46439
//,11218
//,19912
//,26165
//,50571
//,46231
//,40643
//,43611
//,99616
//,58131
//,85673
//,89111
//,18770
//,2269
//,83141
//,51626
//,17050
//,2851
//,16641
//,53681
//,87120
//,66333
//,68221
//,79803
//,88797
//,26389
//,61923
//,41916
//,26606
//,98176
//,93549
//,51905
//,52885
//,1390
//,88252
//,50742
//,59185
//,98001
//,11402
//,45201
//,6263
//,74465
//,89341
//,16999
//,89685
//,34543
//,93833
//,8517
//,15413
//,70051
//,61459
//,94437
//,92969
//,68821
//,11015
//,68609
//,91221
//,35473
//,43876
//,97119
//,63411
//,97049
//,78477
//,44526
//,42617
//,59375
//,1645
//,34841
//,88067
//,22137
//,75741
//,35049
//,72929
//,29037
//,90881
//,97005
//,19281
//,31869
//,99841
//,43009
//,19489
//,38945
//,29980
//,84063
//,59473
//,86969
//,66221
//,48817
//,50032
//,54529
//,42117
//,25236
//,56771
//,13758
//,27582
//,93251
//,88086
//,67042
//,60117
//,30281
//,70681
//,70779
//,70988
//,60121
//,93753
//,93934
//,44901
//,61541
//,89914
//,13883
//,9825
//,4751
//,55111
//,33600
//,20081
//,68645
//,47776
//,7495
//,51366
//,14001
//,99211
//,47131
//,34254
//,73158
//,37713
//,41981
//,37986
//,5444
//,93755
//,19879
//,11189
//,43611
//,23387
//,96529
//,47513
//,89817
//,841
//,24101
//,11823
//,92449
//,62540
//,54081
//,59505
//,73905
//,81075
//,70633
//,11801
//,23861
//,74777
//,63496
//,43649
//,9054
//,59881
//,12803
//,14401
//,97151
//,38063
//,84517
//,45437
//,81832
//,12543
//,35761
//,36247
//,19479
//,13001
//,19877
//,47725
//,22659
//,8826
//,19135
//,87571
//,49961
//,59291
//,21289
//,19596
//,5801
//,98536
//,95705
//,87005
//,95879
//,7126
//,28828
//,29673
//,85347
//,71937
//,80806
//,63330
//,62991
//,79283
//,82265
//,43717
//,64651
//,65947
//,29594
//,7477
//,34809
//,5001
//,97021
//,15681
//,85409
//,9689
//,51073
//,14426
//,89009
//,66343
//,63815
//,23363
//,90592
//,85326
//,78929
//,20076
//,63489
//,61906
//,13391
//,70289
//,27141
//,1389
//,5901
//,4576
//,94481
//,60455
//,70743
//,48729
//,3427
//,91210
//,30849
//,39033
//,72721
//,44705
//,22241
//,45222
//,87537
//,22237
//,28485
//,83122
//,26445
//,2211
//,72026
//,58814
//,9217
//,73081
//,6344
//,17567
//,31661
//,37414
//,26791
//,78177
//,17806
//,61701
//,55158
//,18628
//,67276
//,19821
//,22932
//,27232
//,86549
//,26101
//,97133
//,40121
//,96557
//,9626
//,38531
//,99555
//,43136
//,43937
//,83774
//,66741
//,44425
//,35541
//,74841
//,22913
//,76931
//,71539
//,25113
//,21113
//,23082
//,30049
//,82963
//,55937
//,11285
//,74642
//,8776
//,27571
//,89701
//,9785
//,35657
//,17521
//,66874
//,53391
//,36276
//,11929
//,46780
//,69889
//,46545
//,54965
//,57601
//,62705
//,75801
//,53457
//,43707
//,32280
//,9489
//,45401
//,30305
//,42926
//,45737
//,77485
//,36325
//,79841
//,28061
//,93348
//,93281
//,7888
//,59603
//,61469
//,6181
//,85777
//,26433
//,45409
//,52191
//,36915
//,33368
//,44386
//,35583
//,93596
//,65585
//,61201
//,41526
//,73041
//,8184
//,27195
//,87176
//,3753
//,65937
//,85153
//,87219
//,28364
//,28578
//,47428
//,69037
//,69056
//,22653
//,45015
//,97091
//,27662
//,38513
//,7971
//,5501
//,44688
//,64531
//,2421
//,74425
//,32767
//,61389
//,74657
//,93257
//,87241
//,33266
//,13579
//,46241
//,69977
//,17961
//,16663
//,23279
//,55161
//,28657
//,16941
//,45311
//,7627
//,59915
//,57163
//,14693
//,16121
//,83073
//,1814
//,22935
//,89875
//,43705
//,53701
//,84469
//,66999
//,29691
//,94361
//,24321
//,11149
//,31318
//,18177
//,9963
//,63825
//,59531
//,8984
//,77913
//,90963
//,85245
//,4861
//,16161
//,99446
//,97653
//,96481
//,8751
//,29457
//,68840
//,15601
//,10317
//,98417
//,80641
//,95716
//,84375
//,661
//,33546
//,52939
//,4483
//,64293
//,49385
//,5642
//,92441
//,29979
//,53071
//,98543
//,23354
//,36381
//,92929
//,24590
//,54283
//,46061
//,73353
//,66213
//,28121
//,10117
//,75808
//,69373
//,87201
//,26377
//,83901
//,56939
//,30096
//,91553
//,59181
//,47271
//,15008
//,94391
//,76196
//,78133
//,30041
//,27975
//,60404
//,31692
//,83757
//,40041
//,11309
//,36945
//,60652
//,22601
//,71353
//,28996
//,64776
//,34129
//,60425
//,37225
//,78548
//,44109
//,9245
//,16447
//,4501
//,67591
//,57621
//,39460
//,70041
//,17089
//,63461
//,6397
//,1865
//,6217
//,28959
//,24897
//,25094
//,70660
//,82613
//,51391
//,5217
//,57581
//,56641
//,71224
//,63292
//,35193
//,37804
//,2328
//,93401
//,94992
//,72177
//,8801
//,3351
//,82625
//,91250
//,68873
//,19561
//,91364
//,16477
//,67053
//,50482
//,48536
//,13556
//,45379
//,54152
//,67491
//,52603
//,95282
//,64951
//,53572
//,79297
//,31501
//,81015
//,87867
//,79929
//,70276
//,7177
//,64290
//,49234
//,94729
//,43489
//,15473
//,58957
//,2420
//,52713
//,64384
//,71717
//,46901
//,20801
//,30195
//,72001
//,39537
//,97337
//,72267
//,1551
//,36680
//,25266
//,82952
//,29811
//,46991
//,11185
//,52367
//,58956
//,24652
//,71175
//,87379
//,92257
//,94736
//,73261
//,84531
//,73126
//,66807
//,16444
//,5236
//,95137
//,94866
//,20725
//,24951
//,83625
//,52886
//,40409
//,98849
//,99921
//,10111
//,85017
//,53001
//,85053
//,75069
//,14735
//,28223
//,91457
//,65425
//,80981
//,58722
//,5314
//,66578
//,45971
//,78021
//,56865
//,99101
//,38105
//,72936
//,22865
//,41509
//,6993
//,40434
//,85647
//,91856
//,39104
//,88061
//,4881
//,78449
//,30918
//,33121
//,53129
//,84076
//,70795
//,18005
//,20016
//,5761
//,7551
//,66251
//,14881
//,24831
//,38937
//,70421
//,69621
//,459
//,69065
//,21807
//,20429
//,53206
//,82981
//,36321
//,65411
//,59777
//,3649
//,33703
//,62961
//,78631
//,10573
//,41621
//,96981
//,51532
//,32180
//,28233
//,55491
//,99197
//,60192
//,31615
//,19251
//,69457
//,32417
//,35965
//,90283
//,31159
//,23187
//,18625
//,57283
//,21152
//,4296
//,40567
//,8561
//,47927
//,89534
//,1781
//,6966
//,39346
//,42067
//,68660
//,40233
//,27117
//,37203
//,56369
//,27481
//,79597
//,29432
//,99001
//,6325
//,91465
//,56876
//,27628
//,58737
//,59251
//,26921
//,22533
//,14348
//,376
//,42504
//,45345
//,59190
//,89059
//,67556
//,53831
//,49271
//,87761
//,78380
//,56485
//,4443
//,2319
//,44212
//,91789
//,46541
//,36569
//,70599
//,17577
//,93300
//,85761
//,21209
//,84324
//,15201
//,3729
//,32481
//,17701
//,28093
//,36450
//,93482
//,15184
//,79535
//,46881
//,27916
//,50593
//,87641
//,25125
//,7201
//,62213
//,22080
//,73511
//,58747
//,14443
//,79585
//,91593
//,99055
//,14695
//,52751
//,8271
//,83676
//,51801
//,13261
//,46176
//,76941
//,43671
//,46776
//,66524
//,17175
//,38751
//,75001
//,13401
//,29917
//,61345
//,73651
//,92167
//,31451
//,77793
//,99277
//,32514
//,86336
//,52997
//,46926
//,28929
//,41171
//,30430
//,37166
//,98513
//,5403
//,52269
//,85461
//,21051
//,69561
//,9571
//,64690
//,53337
//,59996
//,2785
//,76405
//,84051
//,52193
//,53665
//,80426
//,38279
//,20265
//,9166
//,9172
//,54673
//,7053
//,48241
//,22929
//,85173
//,86057
//,82705
//,95993
//,95081
//,10591
//,38471
//,79423
//,5361
//,18631
//,94993
//,61049
//,67285
//,15509
//,17282
//,91846
//,57001
//,41449
//,3901
//,82082
//,10661
//,17968
//,27110
//,96251
//,94703
//,2151
//,69491
//,88629
//,10045
//,38283
//,94373
//,39785
//,84355
//,13081
//,807
//,40879
//,20545
//,67406
//,39091
//,90976
//,17922
//,98771
//,87989
//,62674
//,9684
//,5541
//,28251
//,77801
//,77075
//,92209
//,64947
//,79041
//,26409
//,55678
//,67133
//,71581
//,2579
//,51593
//,29153
//,65067
//,7517
//,99238
//,3493
//,47181
//,19039
//,95096
//,54474
//,2501
//,32071
//,49881
//,16011
//,30992
//,14121
//,66309
//,78633
//,9501
//,2513
//,21575
//,70957
//,27345
//,24072
//,59582
//,6016
//,53137
//,57108
//,34301
//,47029
//,16549
//,84114
//,63809
//,79396
//,91297
//,15338
//,18783
//,4801
//,34961
//,33761
//,26989
//,89071
//,40681
//,45109
//,28072
//,59370
//,26849
//,31525
//,41890
//,646
//,50257
//,74361
//,29280
//,59986
//,15233
//,47501
//,27941
//,44129
//,96175
//,93063
//,59145
//,59517
//,59613
//,82481
//,6553
//,85805
//,21986
//,75423
//,74421
//,16879
//,20661
//,83241
//,66039
//,17336
//,94025
//,40023
//,65925
//,42441
//,75491
//,68741
//,9996
//,78785
//,88117
//,66434
//,61931
//,79684
//,82266
//,70743
//,55340
//,46206
//,75729
//,801
//,21235
//,40929
//,92702
//,25559
//,47854
//,72419
//,73601
//,93410
//,11601
//,89117
//,6181
//,10105
//,81830
//,58184
//,43740
//,85373
//,51471
//,63361
//,72026
//,24512
//,31902
//,6199
//,72446
//,20841
//,77506
//,84153
//,19440
//,73787
//,17777
//,32805
//,16045
//,14511
//,6711
//,64619
//,57074
//,31491
//,60657
//,55385
//,6087
//,58041
//,72483
//,31719
//,86600
//,93446
//,76738
//,95441
//,40801
//,98615
//,16327
//,51681
//,66539
//,93802
//,77666
//,85019
//,33721
//,59299
//,29733
//,9618
//,51613
//,72296
//,83841
//,23617
//,94413
//,51924
//,83438
//,32551
//,90086
//,11878
//,59956
//,59796
//,44605
//,31293
//,56081
//,4505
//,9695
//,8737
//,67881
//,29677
//,87799
//,70516
//,7801
//,56750
//,55441
//,25981
//,67686
//,9881
//,8001
//,53644
//,14489
//,32109
//,30586
//,39001
//,93611
//,48040
//,65453
//,428
//,51415
//,94433
//,71857
//,14138
//,25726
//,47311
//,52033
//,92801
//,41026
//,3714
//,80582
//,47855
//,23381
//,40121
//,68865
//,17928
//,17801
//,74433
//,78113
//,77873
//,86961
//,69295
//,25667
//,73401
//,64597
//,31825
//,47206
//,53799
//,79801
//,51137
//,32423
//,86785
//,54669
//,69985
//,2011
//,7326
//,43961
//,76241
//,41953
//,93429
//,70347
//,23980
//,33120
//,23006
//,1621
//,60572
//,24049
//,62325
//,51652
//,68629
//,85591
//,54621
//,19131
//,56291
//,56601
//,91085
//,16129
//,55916
//,91953
//,86353
//,41117
//,25231
//,90293
//,79621
//,80863
//,46058
//,92207
//,15585
//,80501
//,10280
//,10328
//,91345
//,61291
//,91956
//,43746
//,73457
//,61731
//,57922
//,85691
//,88011
//,40674
//,93221
//,28959
//,38751
//,53616
//,25015
//,77915
//,94169
//,43501
//,471
//,19377
//,56895
//,58238
//,76647
//,77921
//,85724
//,962
//,42085
//,90611
//,25011
//,73887
//,58709
//,14945
//,56435
//,19578
//,39481
//,24295
//,24581
//,95266
//,19199
//,88897
//,27966
//,14227
//,85626
//,51925
//,90449
//,32729
//,50825
//,75625
//,22625
//,26756
//,45763
//,7029
//,5083
//,84493
//,63181
//,49905
//,65361
//,47286
//,75401
//,83753
//,48613
//,61761
//,58362
//,94541
//,72953
//,95876
//,82981
//,73681
//,65253
//,65201
//,2476
//,14461
//,97149
//,8161
//,25975
//,81183
//,65836
//,94209
//,83457
//,21514
//,26693
//,55909
//,99654
//,53914
//,49083
//,7363
//,94837
//,98911
//,64661
//,57903
//,35083
//,55892
//,65482
//,27483
//,44742
//,65621
//,38825
//,53117
//,3695
//,61053
//,6172
//,46817
//,34512
//,23913
//,81651
//,93586
//,91809
//,27867
//,62630
//,59521
//,61261
//,96991
//,59849
//,71578
//,39151
//,14868
//,83837
//,23717
//,77981
//,17337
//,93098
//,46473
//,29976
//,26041
//,46720
//,12801
//,80447
//,16353
//,57514
//,57341
//,73043
//,27617
//,79141
//,3037
//,8389
//,44001
//,47297
//,85603
//,13861
//,30495
//,27424
//,23079
//,40246
//,44228
//,59641
//,72341
//,40811
//,52269
//,59273
//,80851
//,9861
//,47091
//,25676
//,50949
//,42047
//,3549
//,87681
//,57291
//,49570
//,74183
//,2511
//,19231
//,70121
//,91371
//,10571
//,94953
//,90181
//,48937
//,48177
//,84612
//,26253
//,78405
//,39233
//,14092
//,18477
//,34556
//,29837
//,50441
//,27061
//,19850
//,11429
//,58411
//,2041
//,20441
//,78616
//,6729
//,60049
//,17537
//,44762
//,30221
//,36201
//,82161
//,11641
//,43265
//,27625
//,48005
//,37513
//,50849
//,12137
//,13178
//,10945
//,79500
//,10204
//,48789
//,13351
//,94007
//,48200
//,47521
//,84961
//,62497
//,25960
//,34886
//,26242
//,30025
//,55729
//,91136
//,89805
//,30401
//,14551
//,69172
//,5929
//,15906
//,63855
//,62441
//,42081
//,88261
//,6313
//,67281
//,22893
//,14011
//,65300
//,90257
//,6446
//,71101
//,25221
//,49273
//,54552
//,85185
//,71646
//,67897
//,70513
//,4741
//,44313
//,88641
//,20112
//,98177
//,24309
//,56343
//,10705
//,48085
//,31901
//,22631
//,2251
//,44836
//,90423
//,50659
//,78791
//,65401
//,57981
//,40513
//,66083
//,60297
//,42626
//,75681
//,66409
//,38560
//,41391
//,47561
//,11137
//,44321
//,42737
//,41473
//,57262
//,1037
//,48318
//,16561
//,33007
//,2983
//,63777
//,29213
//,67108
//,76521
//,20838
//,9741
//,36379
//,69620
//,1089
//,91553
//,69633
//,60641
//,24009
//,10231
//,2126
//,18081
//,50416
//,14322
//,50513
//,9769
//,22301
//,19189
//,83893
//,60171
//,31527
//,74872
//,33701
//,19369
//,47187
//,86001
//,10794
//,64837
//,53730
//,76769
//,39993
//,10369
//,58484
//,29363
//,41261
//,29408
//,14991
//,95100
//,55045
//,81725
//,66100
//,10743
//,82821
//,63882
//,59080
//,76977
//,65613
//,57019
//,44131
//,40309
//,22042
//,53011
//,96157
//,79634
//,95265
//,99189
//,50629
//,11866
//,41326
//,91165
//,44486
//,94801
//,4889
//,81009
//,19901
//,97301
//,345
//,69009
//,40320
//,84505
//,54397
//,73114
//,68458
//,29589
//,26429
//,67766
//,59160
//,33825
//,37651
//,11044
//,2213
//,38557
//,93151
//,89601
//,62138
//,28288
//,24721
//,86611
//,29561
//,63264
//,40497
//,57281
//,22226
//,27047
//,32130
//,10370
//,38979
//,95809
//,17961
//,99179
//,68181
//,39969
//,92940
//,83009
//,50326
//,86436
//,6039
//,44331
//,74945
//,68417
//,53285
//,59761
//,35525
//,89175
//,91419
//,15201
//,49945
//,98091
//,70807
//,99531
//,37321
//,80221
//,55801
//,521
//,79345
//,20358
//,26020
//,3647
//,94295
//,60097
//,13836
//,83821
//,36819
//,68993
//,83851
//,55901
//,36218
//,72785
//,29857
//,76639
//,35960
//,46411
//,77720
//,82726
//,24097
//,38016
//,62297
//,27045
//,29401
//,74763
//,92673
//,76019
//,80225
//,14946
//,49425
//,25637
//,91681
//,31749
//,52300
//,19152
//,32702
//,81272
//,42175
//,21925
//,13790
//,73847
//,19098
//,28721
//,70931
//,12606
//,39092
//,88840
//,9889
//,18152
//,12436
//,64306
//,86963
//,6762
//,34881
//,24376
//,18537
//,72675
//,59589
//,61857
//,9106
//,25126
//,71474
//,79927
//,13821
//,62356
//,52521
//,38695
//,18393
//,99233
//,80651
//,64965
//,84577
//,30633
//,31312
//,25041
//,99750
//,45532
//,99681
//,14457
//,27052
//,50252
//,91905
//,11366
//,38188
//,95251
//,65545
//,99981
//,86819
//,87407
//,31122
//,82517
//,91545
//,87417
//,84036
//,63925
//,44101
//,7859
//,74341
//,81325
//,75435
//,9366
//,5065
//,28182
//,33601
//,29178
//,52158
//,50976
//,77025
//,59658
//,71053
//,33657
//,96906
//,39783
//,18753
//,21695
//,70841
//,84875
//,24991
//,22158
//,23609
//,32467
//,78681
//,41207
//,35221
//,36721
//,84123
//,76353
//,91701
//,24961
//,52844
//,45461
//,74225
//,80201
//,15916
//,51961
//,929
//,50670
//,9217
//,2647
//,56521
//,52161
//,44321
//,96361
//,62681
//,42843
//,6606
//,44064
//,26953
//,71569
//,59921
//,95389
//,7276
//,58991
//,46708
//,74451
//,18450
//,57271
//,42201
//,95553
//,78681
//,65731
//,95881
//,48391
//,305
//,76521
//,6605
//,39019
//,6761
//,79370
//,32976
//,8129
//,76551
//,36071
//,2811
//,25607
//,41452
//,79094
//,49863
//,53081
//,83185
//,77655
//,83815
//,197
//,82735
//,31905
//,60948
//,5573
//,64252
//,63418
//,1641
//,45967
//,33541
//,76918
//,56781
//,64251
//,83501
//,28177
//,98311
//,84401
//,32337
//,32953
//,1972
//,1767
//,1093
//,99991
//,20434
//,67749
//,68928
//,57693
//,95901
//,44375
//,62836
//,65161
//,65811
//,2809
//,39140
//,6753
//,32099
//,83509
//,47503
//,12279
//,93154
//,63277
//,29249
//,69450
//,93186
//,66097
//,44293
//,77791
//,19195
//,16913
//,25133
//,6437
//,7183
//,46754
//,15557
//,60657
//,72927
//,37077
//,93651
//,43207
//,52261
//,62361
//,65699
//,28952
//,94514
//,40016
//,37505
//,44449
//,95764
//,57511
//,28218
//,56077
//,48585
//,61591
//,68488
//,92105
//,11356
//,93125
//,30713
//,42976
//,92773
//,84284
//,81712
//,31171
//,98417
//,67806
//,57621
//,78097
//,89741
//,57679
//,68867
//,38417
//,17701
//,41893
//,17740
//,29254
//,95801
//,55311
//,91876
//,88295
//,19401
//,34929
//,3599
//,22945
//,14820
//,56149
//,83751
//,69661
//,87976
//,51259
//,91701
//,64863
//,38443
//,6657
//,63851
//,72465
//,36037
//,81723
//,71873
//,71889
//,78766
//,889
//,90945
//,57453
//,61793
//,71499
//,66017
//,96741
//,18383
//,44001
//,60505
//,62707
//,58055
//,34084
//,64989
//,96742
//,7545
//,79197
//,11987
//,80541
//,44821
//,38347
//,68436
//,65489
//,45511
//,86401
//,48105
//,60113
//,46911
//,91400
//,29577
//,45921
//,37417
//,95576
//,86097
//,11243
//,75950
//,95436
//,35364
//,5596
//,77569
//,5576
//,14881
//,1341
//,85413
//,32587
//,33672
//,55183
//,33681
//,74041
//,22488
//,38302
//,55361
//,39537
//,69313
//,81908
//,14627
//,61253
//,71275
//,92188
//,99899
//,44451
//,2577
//,29085
//,76306
//,86094
//,25549
//,32638
//,14643
//,57131
//,93217
//,92092
//,16165
//,92385
//,98266
//,26161
//,14885
//,65995
//,47535
//,95473
//,21027
//,33907
//,83821
//,73488
//,23739
//,18081
//,79833
//,3087
//,52833
//,71911
//,22151
//,95083
//,1298
//,24416
//,73724
//,76741
//,95719
//,15176
//,2353
//,15136
//,55733
//,50074
//,20800
//,78584
//,84761
//,90545
//,64033
//,29822
//,95726
//,53301
//,91525
//,38665
//,89801
//,87937
//,97177
//,45171
//,66241
//,50673
//,68609
//,7571
//,87486
//,19226
//,99577
//,90423
//,65806
//,73342
//,9165
//,28865
//,33860
//,29651
//,64013
//,18019
//,70891
//,27525
//,69606
//,2401
//,93512
//,93303
//,6156
//,72629
//,69306
//,21212
//,67606
//,96666
//,693
//,62457
//,62101
//,24014
//,9681
//,92827
//,52095
//,41382
//,60717
//,1669
//,26603
//,25057
//,49963
//,90981
//,96595
//,61379
//,60203
//,52925
//,86301
//,38143
//,10801
//,29226
//,18143
//,8842
//,77551
//,82272
//,45889
//,66761
//,95523
//,21349
//,78545
//,77797
//,83994
//,17153
//,39850
//,5466
//,53482
//,69585
//,82398
//,13210
//,99377
//,61197
//,6473
//,51333
//,61161
//,98051
//,58921
//,21271
//,87903
//,99433
//,94640
//,84100
//,75414
//,33865
//,50992
//,67741
//,46101
//,99039
//,69999
//,54859
//,44005
//,17921
//,31915
//,68605
//,54225
//,18841
//,82793
//,90897
//,93341
//,74251
//,51538
//,36470
//,23327
//,64871
//,16161
//,16826
//,69011
//,35973
//,2856
//,12669
//,72370
//,72385
//,52611
//,60126
//,48791
//,93708
//,37668
//,40761
//,61617
//,45345
//,7602
//,89170
//,33815
//,97511
//,84785
//,46951
//,17199
//,90693
//,28519
//,63553
//,40675
//,88893
//,55857
//,70441
//,67361
//,49791
//,32721
//,37231
//,36471
//,6276
//,74552
//,32574
//,41381
//,27071
//,81846
//,67351
//,81960
//,89347
//,79853
//,237
//,48253
//,21171
//,37434
//,41901
//,27614
//,68188
//,39024
//,20710
//,36937
//,64237
//,93107
//,91077
//,45009
//,94452
//,33309
//,62401
//,56005
//,20757
//,45322
//,17313
//,97489
//,60355
//,69277
//,85664
//,44331
//,41035
//,23197
//,78503
//,45173
//,84221
//,21101
//,17881
//,43881
//,88245
//,51281
//,54283
//,47201
//,62983
//,56627
//,6081
//,67951
//,31693
//,4561
//,44513
//,98099
//,81686
//,54896
//,63585
//,49004
//,84992
//,2991
//,64293
//,21509
//,3913
//,91826
//,60090
//,88364
//,32661
//,85201
//,52689
//,93503
//,93396
//,6841
//,63473
//,13637
//,6261
//,29801
//,76016
//,5687
//,58769
//,2054
//,62961
//,3321
//,60524
//,94187
//,22619
//,13635
//,52772
//,35229
//,48281
//,25321
//,94345
//,37086
//,9449
//,89103
//,13951
//,88871
//,71363
//,72565
//,91025
//,75961
//,68891
//,99110
//,50621
//,86216
//,93847
//,33241
//,57330
//,18629
//,14473
//,88689
//,11727
//,3593
//,22291
//,8471
//,46433
//,14024
//,19835
//,82451
//,66028
//,79661
//,21587
//,5630
//,82196
//,22333
//,62760
//,37457
//,59333
//,45841
//,74661
//,59241
//,82521
//,67361
//,14953
//,78841
//,4081
//,85598
//,62345
//,20013
//,47156
//,60917
//,5261
//,87897
//,53604
//,69833
//,26618
//,70387
//,8302
//,69871
//,16111
//,36517
//,60112
//,46521
//,73011
//,10544
//,41441
//,26381
//,721
//,61891
//,80665
//,24701
//,50169
//,50601
//,18509
//,45761
//,86116
//,52081
//,26812
//,96191
//,31200
//,82149
//,22806
//,59229
//,26681
//,19406
//,44951
//,1649
//,2187
//,13101
//,47657
//,77851
//,92721
//,29021
//,48701
//,72681
//,655
//,2067
//,89581
//,27413
//,60785
//,64223
//,13361
//,51205
//,14799
//,21646
//,40385
//,83956
//,52049
//,43441
//,96423
//,89268
//,19281
//,2953
//,86682
//,82731
//,66113
//,58377
//,29417
//,85481
//,80589
//,70513
//,82777
//,7540
//,69811
//,34637
//,42017
//,58109
//,78801
//,8743
//,55529
//,16401
//,26001
//,18993
//,60518
//,52141
//,31226
//,66309
//,31342
//,43955
//,16059
//,63843
//,59926
//,91637
//,16849
//,93121
//,73246
//,80201
//,23321
//,50511
//,45765
//,92791
//,61189
//,93381
//,80703
//,78081
//,52452
//,46393
//,41961
//,15536
//,12871
//,3643
//,44785
//,7297
//,60245
//,62747
//,77711
//,39247
//,3909
//,4261
//,67035
//,64582
//,65245
//,17901
//,13746
//,70001
//,86593
//,40821
//,36037
//,195
//,18462
//,58311
//,67663
//,54626
//,14662
//,96751
//,88609
//,97786
//,77991
//,71741
//,82261
//,35889
//,43976
//,66476
//,65321
//,11152
//,43156
//,99813
//,71717
//,74257
//,2361
//,62310
//,68257
//,82521
//,67121
//,45942
//,55262
//,87957
//,36329
//,84649
//,95341
//,80049
//,37705
//,32059
//,50221
//,83343
//,87211
//,31937
//,90153
//,23876
//,53237
//,49917
//,78751
//,49581
//,17041
//,51313
//,84318
//,76081
//,21981
//,56561
//,33025
//,48551
//,11681
//,95459
//,95126
//,47681
//,54036
//,46821
//,67881
//,22091
//,92141
//,45222
//,35112
//,54173
//,81892
//,23715
//,43801
//,21645
//,10961
//,98479
//,23016
//,20705
//,40793
//,7675
//,84866
//,36303
//,78731
//,71217
//,98233
//,76855
//,61574
//,48669
//,55271
//,84539
//,65115
//,43061
//,46547
//,89249
//,80001
//,39649
//,47256
//,38893
//,59681
//,15873
//,90675
//,88153
//,33761
//,45747
//,33841
//,64634
//,15541
//,83953
//,99741
//,77633
//,16020
//,11021
//,15679
//,42210
//,58621
//,81137
//,18001
//,64865
//,9161
//,2369
//,72835
//,77401
//,22771
//,81261
//,28832
//,19645
//,65227
//,76689
//,50807
//,61937
//,28743
//,44385
//,32401
//,41033
//,22201
//,39351
//,97141
//,6676
//,17274
//,76583
//,17118
//,71081
//,80916
//,19561
//,22961
//,77736
//,3605
//,26017
//,27301
//,89621
//,66155
//,72851
//,34332
//,83143
//,56281
//,13611
//,84007
//,22901
//,16221
//,13172
//,56751
//,19890
//,99681
//,72859
//,63993
//,50227
//,94481
//,36564
//,97441
//,36157
//,50889
//,64836
//,24349
//,87781
//,93195
//,38683
//,61429
//,22205
//,979
//,72925
//,41754
//,62436
//,20456
//,29104
//,57412
//,63437
//,92321
//,90193
//,17661
//,19072
//,52829
//,86646
//,77433
//,98803
//,55261
//,16141
//,12415
//,30142
//,90035
//,27578
//,33201
//,98
//,7067
//,58441
//,80193
//,37139
//,81169
//,3437
//,13606
//,49321
//,38885
//,91113
//,19275
//,69729
//,73976
//,89173
//,54985
//,48891
//,54497
//,15748
//,93029
//,90973
//,51076
//,74985
//,73473
//,73957
//,501
//,40676
//,49256
//,57563
//,63623
//,17197
//,93956
//,78633
//,54843
//,22614
//,85519
//,10758
//,18476
//,39559
//,48865
//,22151
//,72196
//,67223
//,5009
//,34349
//,31997
//,8601
//,24957
//,99466
//,56134
//,1753
//,76721
//,83806
//,7675
//,41789
//,74821
//,24239
//,75434
//,99536
//,63363
//,41711
//,73203
//,8229
//,32679
//,88825
//,52806
//,7101
//,13853
//,75041
//,29537
//,19801
//,8589
//,26059
//,46075
//,99261
//,8193
//,71291
//,17193
//,33644
//,68881
//,49840
//,28460
//,63453
//,54442
//,96965
//,43713
//,59601
//,276
//,84942
//,73129
//,28065
//,61337
//,61726
//,64746
//,94231
//,27414
//,236
//,72845
//,75569
//,1333
//,3063
//,73659
//,69951
//,11540
//,58337
//,70198
//,46303
//,86561
//,89463
//,4861
//,57974
//,1350
//,51688
//,40306
//,12369
//,71941
//,53817
//,39457
//,70516
//,21249
//,82609
//,12241
//,35416
//,8041
//,59809
//,38081
//,73007
//,44682
//,53303
//,88797
//,27031
//,13388
//,64625
//,19146
//,57731
//,8405
//,78926
//,76845
//,85250
//,64753
//,88748
//,88051
//,32349
//,39777
//,44761
//,36749
//,51710
//,42881
//,29441
//,66931
//,70419
//,53066
//,42285
//,45767
//,36251
//,59381
//,12561
//,96103
//,6776
//,63537
//,52991
//,14639
//,89789
//,35701
//,92036
//,1341
//,33535
//,11062
//,19127
//,65449
//,91569
//,99321
//,56621
//,80449
//,84617
//,56525
//,84431
//,86027
//,84691
//,81817
//,64901
//,34469
//,26163
//,94065
//,33107
//,97333
//,1599
//,53751
//,49542
//,81037
//,67096
//,50757
//,27622
//,56666
//,71781
//,44150
//,3615
//,3296
//,64221
//,1201
//,56561
//,32341
//,66677
//,270
//,7809
//,33424
//,17657
//,36297
//,84401
//,47427
//,5869
//,87601
//,83865
//,3514
//,66673
//,17058
//,93738
//,57921
//,97559
//,95799
//,82722
//,5766
//,66225
//,89405
//,21739
//,87071
//,28496
//,67108
//,55801
//,78591
//,18545
//,22221
//,13969
//,18505
//,33847
//,49501
//,53651
//,94629
//,87001
//,76687
//,47497
//,18637
//,66561
//,52131
//,43417
//,19495
//,45125
//,57895
//,12093
//,4132
//,1997
//,36481
//,24087
//,38501
//,84856
//,77102
//,99611
//,43051
//,87957
//,99249
//,9041
//,17381
//,62621
//,26891
//,63884
//,92284
//,42486
//,42175
//,6339
//,34482
//,9206
//,11001
//,5313
//,88466
//,41997
//,16575
//,14317
//,48775
//,65001
//,23380
//,6219
//,30566
//,56665
//,8161
//,75271
//,97309
//,3009
//,76400
//,56565
//,44297
//,45166
//,51951
//,46069
//,2551
//,15981
//,41128
//,49421
//,85657
//,57266
//,85094
//,71201
//,19000
//,2555
//,51578
//,8689
//,57473
//,85200
//,58589
//,57477
//,6155
//,97431
//,39105
//,85737
//,32801
//,3086
//,28336
//,34813
//,42939
//,57729
//,90287
//,56216
//,47654
//,69341
//,55101
//,81570
//,818
//,48293
//,57441
//,24523
//,70041
//,26806
//,39695
//,31066
//,60051
//,9749
//,24501
//,15083
//,1663
//,3441
//,39501
//,37086
//,9188
//,75622
//,18769
//,23334
//,20781
//,47401
//,23777
//,51149
//,84098
//,25435
//,91534
//,75365
//,37383
//,64886
//,42056
//,36785
//,213
//,19897
//,46513
//,54194
//,63800
//,66825
//,645
//,98220
//,3537
//,91676
//,88679
//,60481
//,98381
//,77313
//,30381
//,66659
//,26777
//,32445
//,24784
//,46211
//,3691
//,11909
//,70630
//,46321
//,28607
//,38168
//,38842
//,89070
//,6981
//,38191
//,93349
//,81979
//,75241
//,33765
//,54803
//,15041
//,36705
//,18865
//,12825
//,2926
//,39199
//,67847
//,6890
//,25359
//,75833
//,81149
//,55657
//,51201
//,99751
//,90348
//,14721
//,53815
//,10868
//,64424
//,47777
//,86792
//,66917
//,76865
//,5161
//,56471
//,20257
//,12836
//,42781
//,73401
//,74325
//,4873
//,12193
//,7417
//,88241
//,64117
//,7225
//,7618
//,26175
//,5022
//,54060
//,51576
//,46841
//,42101
//,8060
//,47867
//,41726
//,44479
//,11231
//,69999
//,39237
//,45361
//,48929
//,18715
//,34191
//,4157
//,32206
//,29917
//,17946
//,21387
//,93211
//,70715
//,27589
//,57633
//,15913
//,8381
//,17501
//,42906
//,22277
//,2279
//,53309
//,95001
//,93183
//,81466
//,62551
//,58906
//,60348
//,98386
//,49347
//,12872
//,48119
//,38565
//,81026
//,16341
//,89119
//,20336
//,84101
//,95991
//,52001
//,14097
//,26317
//,73121
//,70588
//,72439
//,34189
//,81631
//,3698
//,29376
//,34709
//,14040
//,7068
//,31981
//,22494
//,86971
//,69241
//,39694
//,71187
//,56221
//,13715
//,57411
//,51377
//,44254
//,21889
//,59115
//,83671
//,9361
//,34456
//,72321
//,94156
//,72365
//,76254
//,90649
//,76459
//,11501
//,61581
//,57705
//,38592
//,88961
//,79005
//,96001
//,99726
//,87975
//,40041
//,56562
//,37857
//,88561
//,2261
//,72186
//,40781
//,71021
//,4456
//,34429
//,4321
//,65283
//,20326
//,96863
//,99661
//,75019
//,52701
//,24753
//,91624
//,86093
//,10116
//,40901
//,87727
//,19177
//,95487
//,55438
//,24431
//,25131
//,72207
//,9681
//,97257
//,74321
//,75940
//,7175
//,87766
//,76492
//,23841
//,21221
//,47862
//,61286
//,54891
//,67551
//,49631
//,11611
//,54586
//,35429
//,72641
//,78151
//,25377
//,56545
//,11905
//,54406
//,1618
//,19538
//,85766
//,24711
//,97128
//,14301
//,72162
//,20705
//,24653
//,34956
//,23634
//,54593
//,97018
//,77391
//,24956
//,40673
//,12752
//,60966
//,6915
//,73945
//,44197
//,66545
//,90667
//,87317
//,45525
//,55073
//,1315
//,78641
//,75740
//,62123
//,54903
//,47487
//,74125
//,87901
//,33881
//,55398
//,31056
//,54334
//,37477
//,73227
//,11515
//,34151
//,50581
//,45615
//,90597
//,40673
//,80547
//,30001
//,89361
//,44631
//,18354
//,1721
//,20063
//,96801
//,57841
//,25661
//,34475
//,76153
//,32843
//,33630
//,24321
//,28290
//,55097
//,63201
//,5563
//,41365
//,90781
//,46380
//,64021
//,1588
//,50881
//,69685
//,77517
//,63387
//,89146
//,22041
//,30841
//,50121
//,10299
//,92037
//,99149
//,11927
//,61151
//,38456
//,79019
//,49057
//,46088
//,32841
//,77520
//,42675
//,86018
//,25229
//,99721
//,56307
//,27916
//,76393
//,33547
//,8266
//,61483
//,91504
//,20073
//,89481
//,50881
//,87650
//,94033
//,49916
//,52628
//,52717
//,76861
//,74417
//,29761
//,45305
//,13581
//,38789
//,88856
//,78992
//,61353
//,93897
//,67704
//,32181
//,25721
//,39313
//,96763
//,18781
//,59243
//,59295
//,99274
//,9971
//,38406
//,53324
//,44858
//,12999
//,32881
//,57601
//,80441
//,721
//,7838
//,67723
//,79953
//,13545
//,89531
//,66040
//,98850
//,23081
//,17018
//,29507
//,10933
//,41664
//,87201
//,81601
//,74793
//,30329
//,44301
//,91833
//,72044
//,24531
//,50868
//,68125
//,48676
//,79998
//,72011
//,30179
//,46675
//,42487
//,5812
//,49377
//,3683
//,45401
//,41347
//,40206
//,685
//,32965
//,79274
//,99356
//,9596
//,30301
//,33313
//,10079
//,7251
//,70417
//,95058
//,56581
//,80784
//,32545
//,98273
//,71557
//,84977
//,85467
//,97951
//,88359
//,216
//,52351
//,66039
//,90250
//,65563
//,9164
//,31746
//,88165
//,20973
//,16801
//,55249
//,90149
//,60801
//,6504
//,3291
//,9063
//,68933
//,63701
//,55983
//,46539
//,56978
//,87170
//,82889
//,62925
//,60663
//,61591
//,75501
//,6641
//,87717
//,24284
//,1279
//,13658
//,41776
//,34088
//,47187
//,80667
//,11597
//,3001
//,16041
//,61725
//,28071
//,58497
//,58506
//,6206
//,11701
//,85337
//,48623
//,99166
//,5271
//,60898
//,96617
//,12957
//,22725
//,38839
//,93971
//,40369
//,17674
//,94793
//,82327
//,59090
//,9099
//,32932
//,24143
//,69434
//,79083
//,28025
//,10410
//,1221
//,32582
//,7221
//,87964
//,50299
//,48619
//,22401
//,32921
//,20401
//,69956
//,48701
//,91357
//,99009
//,11965
//,77901
//,46417
//,74945
//,79153
//,87203
//,99329
//,21813
//,54630
//,26763
//,60517
//,92673
//,64718
//,35297
//,45231
//,81675
//,18780
//,74981
//,35639
//,49272
//,71843
//,78933
//,57112
//,91397
//,20091
//,5744
//,50001
//,82203
//,61549
//,50981
//,99921
//,8225
//,51809
//,87601
//,66888
//,57305
//,69049
//,41896
//,1017
//,45335
//,61371
//,15735
//,47503
//,84176
//,56931
//,94076
//,52005
//,76101
//,34943
//,91217
//,81631
//,16015
//,11205
//,11060
//,70369
//,87737
//,1153
//,49285
//,77943
//,16348
//,52081
//,93207
//,82011
//,29201
//,98933
//,71781
//,79145
//,13904
//,30012
//,24825
//,218
//,19601
//,45946
//,92336
//,10458
//,53585
//,55345
//,54423
//,61450
//,80439
//,38389
//,52249
//,76729
//,33041
//,24013
//,88667
//,57247
//,14803
//,36481
//,29322
//,78167
//,80387
//,10966
//,81701
//,41017
//,63751
//,12471
//,62213
//,35489
//,39339
//,84721
//,32651
//,33379
//,10671
//,71887
//,76761
//,72613
//,4453
//,15038
//,60517
//,59324
//,39857
//,56345
//,1533
//,87175
//,57014
//,33946
//,53881
//,30721
//,4913
//,715
//,22113
//,4305
//,15575
//,57567
//,82201
//,28606
//,67121
//,49352
//,39948
//,35281
//,85001
//,72189
//,66937
//,6161
//,50301
//,51971
//,60036
//,13779
//,66169
//,83149
//,7585
//,34966
//,27931
//,63711
//,27499
//,19785
//,18717
//,8529
//,60841
//,36396
//,71104
//,28984
//,42081
//,53101
//,53493
//,5249
//,55695
//,41651
//,85159
//,90671
//,40673
//,31170
//,85235
//,74957
//,12478
//,27201
//,43615
//,43546
//,74478
//,70301
//,19585
//,21021
//,91105
//,7218
//,90347
//,1909
//,4954
//,14851
//,82971
//,45531
//,29691
//,8218
//,42459
//,48115
//,19265
//,33904
//,90605
//,34021
//,89825
//,89528
//,69512
//,20309
//,64873
//,17902
//,91796
//,60449
//,23737
//,47284
//,75111
//,14063
//,88261
//,65756
//,73481
//,4879
//,38069
//,27454
//,61889
//,7101
//,33506
//,59141
//,20065
//,78371
//,84171
//,29405
//,16105
//,48169
//,98901
//,9681
//,34171
//,6531
//,69027
//,45006
//,67757
//,87820
//,15913
//,19595
//,63636
//,81326
//,4851
//,41621
//,32699
//,58701
//,66049
//,91863
//,26946
//,13010
//,93589
//,96137
//,39004
//,5656
//,38305
//,67049
//,74272
//,48774
//,42273
//,60344
//,63581
//,64168
//,77537
//,16904
//,90169
//,6433
//,2601
//,67699
//,81273
//,95635
//,17159
//,94684
//,56627
//,19753
//,92723
//,66151
//,36521
//,22285
//,70241
//,6769
//,22913
//,37319
//,81586
//,5143
//,4681
//,62913
//,73201
//,41049
//,62351
//,38821
//,62491
//,63931
//,41286
//,93949
//,79444
//,18726
//,68177
//,16446
//,3311
//,58625
//,23449
//,53851
//,66893
//,14007
//,46474
//,93559
//,11237
//,45231
//,89541
//,97488
//,85867
//,7700
//,35764
//,39711
//,21847
//,59477
//,59917
//,82661
//,21997
//,96563
//,89782
//,80797
//,31417
//,93167
//,904
//,64781
//,41461
//,82081
//,85583
//,70241
//,69181
//,92381
//,71811
//,42303
//,10626
//,80212
//,92001
//,31921
//,24581
//,43585
//,51081
//,99745
//,45473
//,72821
//,17225
//,11905
//,40176
//,15781
//,17043
//,2934
//,99875
//,95035
//,78461
//,31655
//,73570
//,93023
//,72919
//,76369
//,91226
//,94976
//,95029
//,33801
//,56886
//,82011
//,84178
//,18349
//,211
//,7798
//,15849
//,6468
//,48865
//,24227
//,91897
//,15057
//,82096
//,40325
//,17777
//,32251
//,31489
//,18241
//,72401
//,70709
//,95393
//,92769
//,45057
//,65626
//,66054
//,87391
//,50262
//,74905
//,15217
//,82011
//,85872
//,47943
//,49793
//,60169
//,12398
//,67473
//,41683
//,82915
//,67681
//,40335
//,85427
//,56166
//,11145
//,39189
//,25513
//,82903
//,22355
//,10721
//,51451
//,14009
//,78353
//,19653
//,22741
//,98513
//,89817
//,69991
//,49097
//,92375
//,9913
//,71405
//,59921
//,21205
//,87271
//,85979
//,33817
//,38351
//,24505
//,43951
//,7211
//,3376
//,32589
//,38625
//,98723
//,298
//,80366
//,11409
//,50981
//,73011
//,16321
//,57976
//,48243
//,69543
//,28070
//,18951
//,98867
//,55536
//,12601
//,81771
//,4611
//,8403
//,86549
//,11310
//,79233
//,28307
//,89594
//,83033
//,40545
//,45045
//,37018
//,83201
//,57006
//,60545
//,64953
//,76154
//,3516
//,91711
//,43383
//,4827
//,83393
//,25526
//,32327
//,52974
//,78022
//,1036
//,77025
//,40882
//,23513
//,91963
//,44427
//,25457
//,23057
//,82949
//,35324
//,96811
//,88369
//,77837
//,39481
//,66203
//,65537
//,26159
//,44395
//,19785
//,4737
//,86876
//,74389
//,19409
//,12241
//,30636
//,25100
//,41097
//,15331
//,69610
//,71909
//,85231
//,27473
//,80863
//,45899
//,68221
//,28624
//,21689
//,60834
//,14886
//,30738
//,11055
//,3305
//,10012
//,99009
//,42959
//,40006
//,64777
//,38725
//,49770
//,34913
//,68323
//,70751
//,18713
//,48677
//,62187
//,74968
//,95505
//,62041
//,86721
//,30881
//,85241
//,24241
//,60981
//,79047
//,97337
//,70886
//,48661
//,90682
//,29021
//,71555
//,74570
//,64492
//,81775
//,19201
//,38246
//,77344
//,62049
//,55863
//,24493
//,74292
//,79768
//,69281
//,69744
//,1840
//,5661
//,77201
//,76736
//,34487
//,58163
//,76781
//,96165
//,20057
//,29701
//,15189
//,66581
//,13886
//,72001
//,79044
//,13351
//,1572
//,92596
//,43501
//,23557
//,52725
//,22501
//,59195
//,33281
//,90053
//,46786
//,67379
//,86491
//,17363
//,28648
//,86577
//,16361
//,88061
//,15577
//,42013
//,77771
//,48253
//,8373
//,79121
//,56271
//,77116
//,52968
//,80266
//,25080
//,43521
//,48919
//,8101
//,98713
//,64731
//,81801
//,18722
//,24968
//,28762
//,79050
//,60577
//,78257
//,8818
//,87101
//,49409
//,7509
//,19643
//,23580
//,23935
//,94001
//,47007
//,44459
//,25387
//,4881
//,42041
//,37441
//,51961
//,58705
//,9185
//,86451
//,82531
//,4689
//,58401
//,56683
//,76006});
            //maxSubsetSum(new int[] { 6768, -8906, 744, 930, -5685, -3551, -4105, 2300, -8849, -1310, 4216, 8962, 5990, -2632, 6817, -3929, -3119, 9618, -9502, -3314, -2123, -1150, 101, -3575, 3275, -3311, 9500, -9566, 7553, 5391, 3948, -596, 5100, -115, 8678, 6576, 7905, 5306, -9871, -9293, 1289, 3329, -5684, -148, -8545, 8848, -988, -7454, -4328, -6645, -296, -9245, -2994, 5253, 8764, 2355, 3178, 6167, -8011, -1010, -7825, 3931, 7607, -5412, 6810, 6102, 5269, 8961, -244, 540, -8304, 4275, -1419, 7900, -3315, 1655, -2196, 1798, -503, -9614, -1700, -6218, -298, -2993, 3740, 653, 754, 4933, 481, 300, 575, 1426, 9294, 9529, -2606, 8133, -9006, 5812, -7229, -735, -8187, 5043, -9491, -8492, 5286, -3598, 4879, 8396, 8695, -1805, 5838, 5235, 7039, 9399, -3314, -9493, 6332, -7865, 317, -7664, 4385, -5679, 7275, 1227, -8714, -5037, -8728, 5111, -2005, 6666, 5369, -7732, 1760, -9419, 1529, -4665, 7104, -494, -9459, -9290, -7795, 2427, 5171, 7307, 3185, 2922, 2969, 6140, -6977, -4577, -5143, 5944, 4182, 5347, 4075, 4518, 4443, 5646, -3099, 1009, -7343, -2986, -7710, -4529, 1742, -7194, -8771, -4303, -3160, 8244, 2271, 5968, 6530, -9388, 2862, -5681, -7492, 6995, -4230, -5762, -6994, 3205, -1365, 2802, 4575, -5897, -3248, 4801, -9950, 8310, -7982, -8070, 1884, 2765, -2328, 4542, -1781, -7444, 3195, -6937, 8606, 9866, -635, 1430, 1115, 9540, -5695, -3805, -2810, 5955, -6515, -6320, -2621, 7773, 4608, 4445, 9800, 5919, 3894, 5824, -8891, -632, 7131, -4915, -5834, 305, 3576, -3585, -2370, 1934, -3045, -9204, 3004, -3490, -3528, -6566, 8864, -5879, -3447, -3606, -1339, 6846, 3240, 9913, -661, -5447, -5218, -907, -3435, -6641, -3943, -6788, 4810, -3784, 8952, -815, 1215, -1065, 4272, 708, 2651, -3938, -7015, 632, 4516, 9024, 41, -8168, -2909, 3712, 8959, 6244, 9608, 1919, 4034, -1520, -8263, 3932, -3866, 3001, -9506, -6031, -3193, 8111, -8162, 2683, 7005, -4649, -7512, 9683, -7799, -3834, -5405, 6438, 1655, -2353, 2748, 4018, -8403, 1068, 7039, 7033, -1661, 3019, 1856, -7820, -9050, -5220, 2228, 5875, 2910, -7756, 5056, 4784, -2789, -6812, 7969, 8199, 3282, 28, -7773, -2618, 1332, -2339, -9114, 3454, 1532, 4549, -9056, 6998, 5502, -7546, -1305, 2458, 557, 2280, -359, -8566, 9815, 8161, -5396, 7267, -818, -7258, -6828, -3550, 728, -179, -5395, 5959, -5440, -3858, -5399, -7948, -3432, -9234, -8295, 1327, -5698, -9099, 596, -8125, 4963, -279, 9859, -1410, -4123, -7033, 5828, -8014, 9307, 5184, 5160, -6292, 9866, 9277, 7397, 1938, 7529, -2035, 1051, -5973, 6517, -238, -4203, -1565, 6027, 441, 5736, -3046, -1591, -215, -5525, 8789, 2323, 1876, 6667, -872, 3682, -8993, -8504, -638, 4247, 8059, -490, 9091, -6614, 4766, 3590, 2717, 5839, -1700, 4765, -3352, 6723, -8243, 8921, -4123, -2101, -8299, -2921, -2368, 7455, -3531, 8053, -4281, -1686, 4917, 1633, -5188, 6694, 7387, 6060, 8919, 574, 8948, 3781, -6422, -4411, -9651, 1287, -9031, -6535, 1121, 3233, -1561, -3021, 4388, 4096, 4869, -5435, -5311, -500, 5691, -4606, -5225, -4902, -422, 7227, -2722, -5804, -1407, -3261, -3671, 579, -5453, 7374, -3141, -9037, 4968, -7328, -7269, 2619, -7397, 4734, 1843, -9572, 6473, -676, 7786, 919, -2283, 3653, 5061, -2170, -4518, -7645, 170, 3997, -6306, 6791, 9438, 2007, 1566, 8531, -5680, 7660, 5240, -8257, 9051, -4428, 4151, -9764, 2689, 4968, 1138, 3890, -7777, 9987, 5456, -6252, 2973, 2282, -220, -9060, 9928, -4801, -687, 552, 8019, 8747, 621, 8692, 9036, -2165, -9853, 7439, -5578, -9442, -2837, -2974, 487, 5838, -3660, -7582, 2853, -374, 1481, -9527, -3756, -4891, 3656, -7522, 8525, -7583, 2540, 9188, 7054, -2684, 602, 5335, 5424, 1491, 2099, 4917, -6861, -6681, 519, 5518, -4051, -6486, 5530, 1449, -3009, -6880, -8249, 6165, -2847, 5931, -652, -2053, 3631, 6711, 8223, -5039, -8315, 4544, 5296, 5313, 8375, 3864, 5659, 8702, 2427, 8488, 244, 7034, 13, -295, 480, 6245, -206, -1683, -999, -8895, -9358, -5553, -6743, 1048, 7607, -5947, -4343, 7813, -4547, 4405, 1642, -2913, 9010, 8878, -8290, 4283, -202, -7156, -9065, 1400, 1258, -538, -5108, -1717, 9602, -4656, 5073, -5260, -2595, -2019, -5901, 2240, -6242, -2936, -138, -3336, 9708, 4181, -7015, -3024, -5556, -6519, -294, -5678, 3830, -1984, -922, -7010, 232, 4430, -4132, 5113, 6652, 3866, -6850, -4123, 9824, 2947, -331, -5264, -9944, 4756, 2445, 2594, 3066, 2050, -9953, -5056, 4836, -5856, -6500, -650, -6717, 1474, -5836, 3743, 5324, 1229, -7627, -412, -6681, 6716, -7165, 2914, 5821, 2615, 8170, 9194, -6725, -9111, 9953, 3910, -1488, -96, 6342, 523, 365, -283, 1661, 4502, -2135, -4357, -9963, 2453, 6168, 4476, -8340, -1459, -2874, 5409, 177, 3194, -6454, 3091, 193, 6895, -9750, 2024, -4488, -1530, 2630, -449, -151, -8397, -1365, -2239, -1079, 3445, -4037, 2062, 3423, -6532, 9929, -4118, -8003, -4655, 9497, 2409, -8884, -4156, -1346, 1537, -3996, -5633, -5961, -196, 7372, -7659, -796, -7350, 3755, -9739, 8990, 5495, -6265, 8550, 7068, 191, 4031, 9310, 7194, -1796, -1717, -554, -3969, -2123, -4949, -1783, -1612, 1282, -6400, 3677, -4429, -4238, 9733, -5136, 8864, -4971, -9995, -3622, -3300, -3930, -6554, -9144, -1076, -8441, -8723, 9975, 7264, 1994, 1919, 1269, -9146, 125, 1552, 7119, 9106, 9157, 5242, -5028, 6317, 2420, -805, -5622, 8721, -899, -3535, -4637, 8597, -3121, -8042, 2957, 9367, 187, -1923, 2153, -1229, -5210, 3468, 301, -2068, -9394, 2590, -5255, -4401, 6165, 2017, -7942, 400, -1832, -1888, -6255, -6592, -4274, -1922, 753, 3182, -4394, 7673, -2619, 6073, 1292, -7477, 1326, 9570, 4637, 3213, 9005, -4411, 4755, -606, 4926, -5340, -3694, 2846, -5169, -7399, -9579, 8585, 5688, -3470, -7973, 5908, -5184, 1030, 214, 9075, -6797, 738, 7622, 3832, 3915, -9305, -3296, 3974, 9911, -7963, 4096, 265, 7468, -7942, -6708, -661, 7079, -2179, -2051, 4018, -334, -3053, 7677, -3352, -1281, -418, 2659, -6875, -6279, -1145, 9194, -2775, -9806, -7856, -9672, 5539, 367, 4921, 403, -9378, -7529, -8525, -1876, 4052, 5082, 9254, -1942, 3987, 439, -465, -6886, 8884, 7822, -4963, -1159, 8600, 3515, -2348, 4808, -2241, -5754, -6869, -9729, -3161, -2362, -2042, -8990, -8312, 989, -5938, 3564, -9413, -6806, 8920, -9707, 7072, -6075, -3371, 158, 6599, 4110, -2867, -2106, 5256, -4705, -8181, -5066, 4612, 9021, 5542, 3517, -8840, 1844, 8872, 5982, -5263, 4369, -14, -7144, -1442, -2744, -7379, -4594, -2158, 5508, 4307, -4655, -9444, 5920, 2574, 4316, -3318, 1095, -202, 5930, -6352, 8706, -22, -4582, -1095, 3330, 5309, 5312, 7263, 2975, -8965, -2168, -4401, 7899, 1573, 6983, -3771, -1534, -7098, -5712, -2986, 1578, 4609, 8015, 4915, 885, 4407, -8787, 3728, -1851, -9434, 4794, 2368, 883, 4560, -2545, 5680, -8726, 4226, -6402, -1971, -2544, -5306, 4277, -3715, 4956, -6288, 3295, 1704, -6032, -3072, -7174, 6740, 8874, 1028, -1957, 7743, -672, -6514, -9126, 2165, 5232, 6326, -6657, -9008, -5545, 4782, 3673, 1178, 6596, 7539, 4942, 3411, -2806, 3549, 4041, 4421, 1411, -4312, 6434, -6240, 8834, -2266, -7281, 7767, 1974, -8998, 7322, 7325, 5032, 5703, -1281, -3480, -4767, 2961, -2506, -7542, -7347, -5572, 2562, -1899, -147, 1784, -5066, -3919, 5036, -3493, 5793, -9487, -2539, -609, -2362, 3195, 5207, -8820, 7579, -3140, 623, 8743, 462, 8642, -9173, -6660, -3215, 7877, 193, -5752, -2894, 4904, 1191, 6252, -1194, 928, 8466, 5758, 1623, 6385, -2847, 6651, 1772, -4089, 9942, 240, 5813, 6782, 8843, 4357, 6920, -4320, 6848, 8096, -8770, -2428, 2387, 7392, 97, 7803, -7090, 4000, -8972, 488, -9450, -492, 9530, -332, 408, 802, 7611, 8528, -3687, -2845, 4460, 5572, 9961, 9391, -1263, 3264, 4643, -7240, -2308, -3746, -2830, 6724, 6015, -5847, 6956, -5713, 1554, 6862, -7350, 9988, -6115, -1444, 68, 4569, 4368, 8647, -4420, 3485, 4070, -7827, -4858, 7773, 2273, 9449, 5903, -3906, -110, -1569, -9507, -9201, 3528, -4943, -8156, 3147, -1994, 8208, 6399, 7430, -1325, -1174, -3424, -7146, -1382, -1913, 2929, -4529, 1129, -1712, 7711, 9861, 7769, -8928, -5611, 2944, 6097, 6765, -702, 8014, 3381, 9091, -9039, -8958, -7402, -976, -5617, 8921, 9129, -6915, -8955, 6607, -7949, 9594, 8418, -1651, -2835, -2472, -4052, 1635, -1694, 8711, -5764, 7888, -9677, 3861, -3423, -3513, -9564, 581, -4499, -7935, 6401, -9868, 6018, 6879, 6222, -8062, -6713, 7471, -5567, -6329, 8612, 1093, 7311, 212, 8609, -180, -8419, 6068, 1290, -4085, 3643, -7300, -4672, -8396, 2704, 7722, -6778, -1198, -7841, -3596, -3719, 8002, -8928, -3664, -5677, 5212, -5939, 6543, 4328, 6982, 2831, 4997, -7483, 6799, 3340, -5588, -1288, 2718, -5187, -1756, 9354, 4387, 4005, -6826, 5623, -7830, 9526, -4436, 6774, 9276, 9469, 919, -8823, 9612, -1614, 9810, -5886, -2661, 6031, 8413, -5157, -1166, -4749, -9880, -3244, 1391, -6336, -5131, -6006, -8908, 3488, -5535, 5833, -2562, 689, 1102, -6124, 1743, -2989, 940, 124, -1331, -3713, -4087, 3017, 7576, 5690, 1704, -4937, 6896, 5796, -6027, 6808, 2943, -1108, -1246, -9897, -5957, 2218, 15, -4166, -3369, -4358, -9199, 9002, -9522, -9587, -3572, -6242, -179, 6622, -3201, -963, 8758, -5411, -8063, -5621, -5021, 520, -4032, 636, -2291, -4293, 8203, -5081, 6364, -3582, 2302, -7158, 5438, -9275, -7186, 7457, 3904, 4579, -5203, 5535, -1724, 5193, 6300, 9883, 6315, 963, -8704, -5538, 3071, 3807, -5003, -1317, -1657, 7919, 3269, 7371, 2627, -6992, -8693, 8948, 4703, -989, 7800, 7941, -9327, 1496, 2436, 5792, -9586, 4299, -9106, -5165, 8779, -2136, 8869, -2273, 5689, -1124, 6334, 8604, -4354, 4287, -6611, 1356, -3661, 859, 2720, 1851, 1013, -4573, 2046, 667, 7106, 9888, -9109, 6343, 334, 9638, -5370, -557, 1476, -8250, -5111, 227, -5261, -5426, 5057, -333, 9705, -140, 7412, 8847, -9630, -6467, 1010, -8809, -8100, 6488, -9409, 893, -8191, -7383, -6801, 2556, 4439, 5573, -145, 2529, 48, 8886, 241, -1009, -5521, 5045, -734, -7849, 4239, 7864, -5312, 7318, 5297, 8874, 6056, -5746, -861, -5847, -3503, -2939, -5960, 7846, 282, 5244, 2125, -7676, 6791, 4453, 4878, -1165, -6813, -2355, -9111, -2881, -1735, 7967, -276, -3491, -6849, 5353, -2200, -6022, -9355, 8256, 4141, 4098, -6180, 4645, -3588, 5545, 3272, -5038, 5798, 851, 6827, -498, 1507, -8464, 5921, 716, 4003, 837, 6893, -6111, 7890, 1608, 7258, -646, 8971, -8495, -311, -8382, -33, 4904, 8930, -4437, 9045, 6470, 6793, -2124, -3494, -6473, 6390, 8304, 4584, -9990, -9794, 1046, -6458, -4728, -6616, -728, -9156, 7916, 8296, -6042, -8207, -1421, 5578, -4192, -413, 4334, -2537, 8904, -3847, -3762, 7749, 8349, 4807, -2174, 8150, 4297, -8503, -4056, 4948, 3043, -3786, 9356, -1435, -2204, 6273, 6127, -1746, 3650, 5820, -6093, -9723, -3776, 4651, 8877, -8977, 381, -8592, -413, 7679, -4246, -6332, -559, -149, -1957, 910, -9303, 8697, 6774, 4416, -5677, 697, -9484, -3866, -7609, -9924, -1738, -8427, 8156, 1815, -8274, -567, 3049, -2352, -6551, 83, 1693, -8615, 4340, -9770, -6232, 4985, 6142, 9344, -5093, -3703, 4985, -9722, -3144, -3273, -7120, 3933, -8070, -8848, -7182, -4662, 1963, -2010, 81, -3431, 885, 9905, 1008, 7446, -3619, -6654, 6830, 8940, 8967, 7322, -2763, -6877, 6918, -1499, 3829, -9272, -4721, 7660, -486, 5824, -7046, -4076, 9707, 5022, 4867, -3622, -6985, 2248, 9775, 9571, -4686, -9028, 2039, -1563, -7537, 3840, -6589, -63, 731, 2274, -6030, 1942, 6245, 7639, 595, -7479, 2450, 365, -9851, 2870, 9255, 1790, -3290, -6154, 2724, -8981, 831, 9420, 2658, 5042, 3090, 1478, 3625, 6008, 8569, 7102, 9474, 1615, -1878, 4630, 7156, -8019, -6076, -7684, 6633, 1628, 213, 471, -9733, 4227, -348, 5791, -7783, -2471, 8888, -8888, 549, -3858, -5277, 4200, -7014, -7613, -6808, -4367, 6310, 6937, -5573, 1468, -9312, 8861, -2111, 6841, 925, 3498, -2907, -7258, 7362, 5537, -8276, -2917, 2352, 5937, 3356, 3929, -4680, -5340, -2421, -5853, -853, 2936, 7245, -9000, 5410, -134, 3175, -5401, -4253, -1810, -266, 8814, -3255, 8464, 1613, 5296, 5187, 4950, -8721, -191, 6286, 1557, -434, 5335, 6432, -6357, 3072, 4740, 2893, 294, 8104, -8300, -1637, -5557, -7255, -9946, 4069, 5990, 1283, -3399, -6943, 1855, 3334, 6670, 1912, 48, -1568, 8076, -6650, -4155, -9529, -3280, 1865, 4092, 8502, 2746, -2845, -6740, 6554, -9903, 5049, 8674, 8199, -4634, -6191, -5121, -4452, -4040, -2196, 2685, 6418, -9928, 8440, 4991, 7579, -2863, -9540, -3551, -3255, -3785, 1049, 5113, 3672, 7435, 8029, -7395, 5224, -4325, 1862, 2668, -8533, 8034, -6808, -6821, 6217, 1673, 1861, 2699, -5165, -6656, -2408, -1463, -8578, 9216, -2212, 4477, 8846, -27, 7405, 916, 9575, 6996, 7897, -841, -4861, -8423, 7772, -6308, 3290, 2522, 8702, -6564, -9109, -3572, -6181, 9028, -2097, -8429, 387, 5485, 6939, -4330, -3166, 7563, 6579, -3181, -4335, 9857, 945, -991, -4987, 3466, 3230, -7776, -5129, 8565, 7295, 745, -2749, 8904, -7015, -3258, -2281, -9073, 3297, 8151, -6952, 8768, 210, -2278, 7670, 6438, 8102, 8765, 9835, -4215, 4948, -2135, 377, 3882, -9518, 6419, -4460, -8929, 4834, 4026, -8648, 2736, 6982, -322, -8734, 420, 8730, -1143, 7986, -3802, -513, -2802, -2863, 9881, 5999, -3308, 9289, -1696, -5028, -3720, 7472, -8800, 2728, -1857, 8994, -943, -8865, 9499, -8415, -6875, 6512, 7377, 6570, 6164, -82, -7090, -9513, 5719, 1973, -6202, 5183, -5178, 3321, 1293, 1892, 5407, -5828, 2858, 7335, -6515, 3320, -7933, 3852, -9799, -4184, 666, 4778, 3087, 8314, 9234, -9122, 8273, -5394, 3121, 3506, -2392, 9398, -9366, 1982, -4630, -8366, 6850, -2303, 6215, 6457, -6291, 4584, -9260, -7893, -1907, 1639, 668, 8189, 9151, -2221, -5591, 1064, -6460, 5240, -8912, 845, -1088, -7435, -2227, 5595, 9719, -8491, 5871, 738, -9806, -1775, -2161, 1107, -5260, -7332, 4684, -7181, 9320, -303, -8793, -7686, -9296, 990, 8816, 7105, 5584, -6592, 428, -3621, -4580, -6113, 8986, -3169, 7558, 2327, -6241, 3204, 114, 579, -3433, -7191, -1454, -9689, -4717, 921, -8028, 7757, -3798, -7097, 8077, 8877, 6140, -8428, 9952, 5107, 1391, 2317, 8383, -6697, 9808, 8056, 1300, -9017, -9612, -1370, -5718, -1633, -49, -7995, -2927, 7269, 9416, -8524, -5745, -7177, -9355, 4733, 7927, -4779, -4789, -6449, -8494, -876, -7727, 300, -6068, -6625, 6837, 2245, 6416, -8883, 6169, 2750, 8743, -1411, -8229, 150, 650, -6946, 7515, -8949, 1890, 8258, 5831, 413, -714, 7675, 1088, 7275, 5011, -7484, 8458, 3316, -3811, -1483, -3016, 6788, -5604, 281, 349, -3295, 4098, -7378, 904, 427, -1439, 4319, 8806, 9250, -8501, -2060, -5019, -1569, -5113, -9791, -4461, 3380, 7773, -1595, -7852, -5859, -3377, -6559, 8878, 1416, 6241, 4368, -9034, -2271, -6931, 9051, 6080, -7398, 4668, 3412, -5464, 7827, -7989, -7545, -8752, -9212, 1950, -2280, -5919, 7613, -341, 4064, -496, -2965, -222, -4534, -6407, -4074, -6177, 1115, 3849, -7132, 3430, -4458, 7328, 4544, 9575, -8496, 3264, 9967, 4109, 3342, 9544, -2873, 1390, 2455, 4104, 7905, -270, 6455, -5168, 2822, -1536, 9149, 1550, -7605, 1119, 8218, -3471, -3109, -222, -1716, -7288, 1590, -4559, 5831, -6329, -679, 1625, -7158, -3795, -321, -6575, -29, 3521, 4566, -8878, 1137, -1338, 4794, 9775, 2265, 209, -1556, 7161, -8233, 285, 2767, -4909, -220, 4704, 9767, -6137, -6278, 566, 3134, 6421, -8438, -7255, 8194, 7718, -7359, -4753, -6193, 7358, 1057, 6320, 5788, -2061, 7010, 471, -8802, 4475, -6058, 6140, -3319, -8896, -4625, -9655, -7158, -5640, 9990, 1424, -2104, -5963, 5272, 1690, 5942, 2801, -8807, -7251, -4192, 160, 318, -7897, -9650, 3100, 227, 1305, 2331, -5705, 1408, -5822, -9958, -681, -7015, -7581, 4869, 1594, 9739, -7269, -6201, 9553, -1817, -7793, -9314, -5961, 2582, -9720, -3713, -314, 176, -2336, 7264, 2907, 432, -521, 9275, -9210, -6044, -6817, -5224, 3725, -6442, -1172, -5209, 7481, -1415, 2764, -646, -6067, 446, 2594, -6086, 7555, 2994, -8129, -5601, 4134, 6464, -727, -2305, -4718, 5991, -1759, -2339, -6180, -7640, -2583, -1278, 1345, 3696, 2812, 8790, 927, 8878, 1777, -6823, 4197, -5953, -540, 9527, 6891, 1029, -1887, 6146, 3370, 1347, 409, -6753, -511, 2057, 30, 5404, -8058, -4052, -8975, 6278, -460, -1282, 9148, 6440, 4724, -6314, -3957, -1422, -8143, -1653, 8099, -533, 9391, 1922, 9319, 3174, 8866, -5005, 8860, 8499, 9963, -6472, -6898, 6449, -5921, 6126, -6070, -5023, -7918, -1150, 2531, -4426, 510, -4222, -2543, -7176, -6656, 8757, -4672, 6081, -4239, -9107, -4749, 8417, 1215, -4104, -3157, -5751, -5136, 2469, 4675, 590, -1642, 9627, 8516, 5032, -4363, -2598, -5747, -1643, 5070, 913, -1184, -8309, 2704, -7085, 5503, -5967, -2637, -200, -1482, 2233, 9633, 3547, -77, -5671, -9082, 9063, 3654, -8720, 2889, -5298, -893, 8885, -1290, -8007, 1604, -9155, -7648, -9463, 4799, -8707, 8581, -6231, -7323, -4240, 47, -3274, 1685, -5564, 8019, -9980, 6151, -7384, -2422, -5343, -1724, -4673, 8154, -2720, -5200, 4134, 2539, -6768, 8554, -3616, 1625, 8927, -1969, -7490, 2382, -7666, 1209, 7520, 623, 6050, 4058, 4006, 2241, -2024, -6934, 5656, 6940, 2337, 6772, -6141, 2970, 5561, -5170, 9307, -3530, -7343, -8938, 6865, 8297, 2383, -7018, 7524, -4997, 1821, 5190, 5400, 594, 8677, 703, 9623, 7125, -4655, -3430, 6242, -4464, -6800, -6568, -3454, 692, 4820, -6707, -2275, 5587, 9757, 9865, -2640, 1477, 4905, 6304, 5404, -8447, 5782, -5680, -1207, -9066, -3055, -8903, -4071, -7419, -5952, 4006, -7009, -2980, 4160, 4155, 3396, -2376, -5314, -4289, 2333, -769, -4290, -9368, 9329, 6301, 3259, 7927, -1793, 7500, 9605, 3702, -6631, 1009, -8421, 6975, -6340, -8721, -9378, 7508, 1587, 7248, -6928, -514, 2285, -8207, -6659, 3072, -688, -6032, 8394, -6469, -9794, -2683, -8699, 8429, -2135, -6250, 2175, 7074, 3837, -7851, -5625, 8757, 7827, -8657, -519, -7171, 5315, 8546, 4685, 3173, 3432, 1175, 2665, -2969, 3289, 1717, -8768, -5095, -1863, 60, -9341, -9267, 8360, -6672, 3494, -5971, 5708, 3635, -9305, -9662, -612, -9663, -696, 4332, -5740, 7973, -9282, -809, 4278, -7016, 1134, 6130, 7307, 3230, -2605, 8665, -9859, 9218, 2043, -9714, 1780, 7113, -2472, 382, -3772, 1826, -1201, 4921, 1854, 6607, 9149, 243, -2441, 1442, -4139, -5398, 7082, 2369, -8663, 3875, 6127, -4583, 2678, 6840, -4060, 7609, 1514, -7636, -4928, -1515, -280, -1147, -7923, 7343, 2592, -8355, -3953, -5414, -4154, 5763, 2476, 1866, 722, 6348, -42, 2038, -9713, 8872, 3465, 560, -2279, -363, 4095, 9200, 5015, 8882, 7225, -2690, -2669, 3297, -1673, 7799, 4571, -1766, -9558, 4801, 254, 757, -2928, 8431, 6421, 8684, -5148, -7708, -4203, 3366, 9857, -8973, -692, 3649, -5301, 6280, 3837, 43, -3368, 3748, 1962, -1812, -2272, -4343, 2274, 8005, 1599, -5246, 1545, -2680, -7679, -1582, -9773, -4220, -8245, -6609, -9347, 2682, -6954, -3482, -7207, -3826, -6698, -5495, -9734, 781, 9952, 235, 8758, 3484, 787, 533, -6107, -6809, -2459, 4693, -6687, -39, 9056, 3245, 2467, -668, 522, 1175, -9560, -5809, 560, 9464, 3312, 4692, 9711, 5291, 9064, 1819, 7981, -7059, -3056, -2977, 9115, 4405, -9482, -9946, 5361, -8950, -8637, -1762, 2897, -2398, 8533, -8752, -4710, -7663, -3813, -6644, 2363, -8794, -9991, 4632, -6657, -5151, -3392, -8576, -8016, -3457, 6773, -2849, -7956, -8614, 92, -9670, 3422, -8192, -3166, 6596, -8584, 5448, 141, 9138, 9938, 6429, -4460, 5212, 8171, 9512, 384, -2994, -2577, -222, 697, -8563, 2380, 9006, 9410, -5418, 341, 603, 6915, 2915, 2765, 3304, 2876, -5923, -6725, -7167, 9538, 1438, 4506, -4435, 3376, -4803, 1469, 9118, 36, -2572, 224, -170, -1684, 3970, -149, -8022, -257, -2175, -974, 2967, -3192, 4521, -9289, -6393, -3175, 2724, -1369, -2688, -113, 899, 6596, -2322, -4923, -391, -2430, 5731, 1555, 4672, 7264, -1536, -4728, 1958, -3631, 1384, 9161, -9655, -5866, 3799, -1411, -2237, -9756, 8758, 9963, 878, 9391, 4719, -4359, 6043, -5338, 483, 8146, 1447, 3863, 2186, -1394, -4409, -4764, 8743, -976, 2100, -6589, 6889, -2922, -9073, 8734, 8375, -2532, 861, 6580, -4135, 9564, 718, -9731, 9904, -9598, 7275, 7175, 987, -1582, 5002, 2717, -5479, -6264, -9206, -810, -9979, 804, -2758, -5160, -4542, -6246, -5261, 2381, 6151, -3164, -6933, 7192, -2761, 1063, 7281, -6375, 6956, 6969, -2047, 6690, 492, 1598, -9854, -2160, -5917, -6343, -5769, 6519, 5838, -5697, 4435, 4329, -3392, -9567, -9894, -6896, -7668, 1971, -3065, 893, -5916, 7079, -1870, 4358, -1058, -2428, 991, 9004, 4551, 4822, 1537, -9555, -9911, 753, -8863, 4451, 7801, 2012, -6945, 765, -4991, 6941, 4052, -1440, -9187, -4593, 4735, -2192, 591, -8411, 3541, 483, -3778, -2222, -242, 9409, -9714, -1150, 9188, 2473, 5769, 6830, 2520, -9413, -6502, 7543, 2168, -783, 169, -7100, 3791, -2195, 2519, 7230, 9474, -1752, -5227, -1733, -9900, 4979, -4120, 6209, -7662, 6898, -588, -1530, -5182, 394, 5470, 6837, -6417, 211, -6232, 6489, 3106, -3207, -8596, -6137, 1523, 6166, 1034, 8579, 4309, 8274, 2913, -2628, -7176, 3179, -8722, 5037, 1010, -5873, -2576, 28, 2822, 4872, -6806, -5342, -3682, 7924, 1137, -4775, -9207, -4109, -7963, 506, -2042, 6168, -9618, -7584, -4610, 7528, -2809, 6568, 7423, -2953, 638, 3798, 4752, -3646, 5254, -173, -8658, 2947, -8307, 2876, -811, 1110, -8703, -9610, -8763, 8168, 9489, -1592, 9530, -9246, 7649, -6380, 900, 3978, 4501, -58, 3400, 4591, 1180, 3725, 7745, 5243, 1961, 565, 2118, -4130, 6888, 8375, -1541, 2007, -8500, -8043, -1411, -3147, 4917, 1302, -65, -4356, 7932, 7218, -6046, -5888, 7224, 8679, -7630, -9134, 1254, -6489, 3540, 3111, 2504, 7173, -7626, -5751, -7159, -8058, 3433, -2104, 7548, -1658, 1069, 2985, 6056, 188, 210, 2750, 6005, 1873, 8367, -8222, -9692, 3141, -2834, -7400, 3332, -3868, 3459, 554, 198, -5653, 2380, -9283, 8969, 9449, -8623, 8097, -6526, -2944, 4897, -5490, 6810, 4384, -5324, -771, 9258, 5395, -7273, -7534, -6732, -2854, -497, 6675, 2810, -1608, -5767, -5804, 8946, -1621, -5582, 5869, -2047, -6967, 1197, 8347, 2220, -144, -5902, -4590, 1313, 2556, 9547, -6327, 6583, 6385, -7431, -5444, 8213, -5642, 7246, -3986, 1829, 292, -3359, -8228, -6654, -6271, 1689, 4352, -9919, -8380, 9697, 2339, -9421, -6734, 9220, 4855, 4928, -5040, 1100, -3621, -5851, -2023, 1701, 7369, 593, 763, 2065, 6568, -975, 8016, 4805, -9778, -8264, 6466, -426, -1307, -9750, -8429, 410, 4386, 461, -1959, -7930, 6007, 1161, -2741, -1547, 9472, -9610, 8024, 7036, 7851, 8101, -5671, 8508, 5648, -5990, -8452, 4743, -978, 2131, 3494, 8053, -2785, 8040, -7581, -300, 7934, -4623, -5897, -72, 7156, 1508, -6893, -4536, -5867, -6040, 1288, 8856, 3630, -3720, 447, -9222, -9673, 5948, -4162, -3312, -4170, 3304, 9047, -6551, 1716, -3141, 7167, 3632, -5578, -6877, 7389, 3013, -701, -6207, -6950, -6823, -5261, -2061, -1946, -5481, -3374, -9150, -83, 2915, -7041, 8912, -186, -1553, 3517, -104, 7795, -347, -7533, -3326, 3273, -9339, -1347, 884, 1645, -5503, 463, -9756, -2482, 5612, -2112, 6789, -6257, 8608, 3298, -4211, -6622, -8304, -4729, -8163, 3806, -1472, 7221, -2560, 1385, 8124, 9188, 7379, 1078, 7255, -1712, -4164, 4779, 3262, -6006, 2050, -6564, -6372, -9394, 611, -3483, -4895, 1603, 5675, -1675, 8355, 3711, -7540, -5667, 9743, 9337, 4785, 9017, 585, -2257, -2891, 5442, -1118, 3410, -3044, 434, 8825, -1251, -1304, 8061, 36, -8733, -2396, 5649, -9303, 1849, -7986, -6323, -497, -9139, -7161, 1942, 7195, 3406, 9377, -5402, 4872, -5608, 5266, -769, 6417, 1307, 5454, -9005, -4119, -2792, -4584, 2367, -3464, 5080, 1394, -3054, -6772, -6959, -4768, -4789, -9537, -645, 1011, -4373, -4336, -3063, 8298, -9107, 9361, -5995, -232, -9804, -7352, 884, -617, 2184, -5206, -9241, -904, -2300, -3928, -211, -5021, 7011, -1711, 1935, 1539, 2311, -3588, -867, 6122, 632, 522, 9394, -1417, -3773, 9441, 3076, -867, 7811, 1704, -1913, -2851, 7626, 9937, 2227, -2872, 267, 8808, 8195, 1160, 8026, -107, 2682, 434, -4249, -3858, -208, 4199, 8232, -7325, 4027, -7074, -4021, -5566, 2243, -1635, -6246, 3394, 9162, 2099, -9015, 7492, 592, -1417, -8891, -5498, 8232, 3146, -2652, -2822, -8808, -4106, 5242, 9267, -3454, -9866, -5653, 2883, -4188, 6968, 5504, 679, -8115, 804, -1542, -9466, -4533, -6847, -7571, 4609, -4687, -3571, -6534, 855, -6561, 4404, -9950, -1424, -1370, 7900, 9561, 1720, -3637, 9724, -1123, -6993, -3476, -6733, -4958, -8222, -3729, 1095, 6963, -1157, -6973, -2641, -5427, -2048, 3358, 483, 8695, 6705, -1582, -3005, -321, 4665, 2198, 6119, 5375, -1551, -6116, 5926, -5937, -7546, -8692, 8915, -8285, -6585, 7967, -7274, -4744, 1406, 5766, -5266, 7682, 5971, 5263, -8128, -1390, -4526, 2216, -4534, 2679, 1027, -8007, 6219, -1139, 7789, 9698, 2237, 3270, 4128, -5669, 3848, -785, 5182, -7303, -2676, 3039, 7231, -8973, -4162, 519, -7838, -8186, -9130, 9360, 8005, -335, 1276, -2739, 8755, 8447, -2381, -1369, 5954, -1747, -4912, -5848, -8838, 8889, 5294, -6936, -3866, 6248, -1818, 7481, -816, 1053, 6214, -7017, -7002, -5480, 2806, 6285, -8306, -8461, -10, 5887, -4709, -1359, 4399, 7223, 2033, 8362, 437, 9176, 6427, -1868, -6488, 636, 9530, 2015, -4490, -6899, 5000, -3316, 9127, 9241, -3987, 4425, 4764, -9823, -5787, 2936, 6883, 5973, -3479, 8243, 9593, 2522, -6797, 8351, 6442, 7790, -5716, 4104, -8164, -3114, -3654, 232, -735, -7839, -5922, -4493, -7789, 7293, -2655, 7067, 3078, -6328, 3795, 4449, -4397, 3749, -7932, 9860, 3275, -8218, 5518, 1341, -2258, 8293, -8666, 7168, -8365, -6303, -9434, 815, 2296, -2569, -7165, 601, 558, -5827, 7837, 7366, 1778, -8873, -8935, -3266, 5086, -6929, 9245, -701, 4727, 5274, -5114, -6103, -8483, 4492, 6097, -1940, -9505, -3912, -8311, 5615, 3644, 8633, -2691, -7609, -4986, 1516, 3560, 7259, -2624, -9787, -4584, -3374, 3955, -8718, -2215, 5545, -2038, 3853, 8470, 5714, -1551, -2752, -3615, -7697, 2139, 9158, 1539, 722, -5960, 2557, 3798, -7881, -66, 6897, -5225, -3146, 9231, 9271, -2387, -8270, -3555, 6817, 3334, 998, 1033, 7295, 1956, 1254, -7121, 6, 365, -8565, 6301, 8010, 9063, 9609, 1137, 2617, 8665, 8087, 6582, 7466, -110, -9997, -7741, 1451, 2821, 5309, -6277, 4709, 4214, -5149, -8211, 5075, 5591, 906, 4893, 1166, -3924, -2093, -8479, 2451, -4402, 3316, 9245, 9171, -4489, 1882, 7463, 2801, -2335, -4460, 7304, -9105, -6096, -7306, -6394, 8365, -2215, 4873, 8236, 565, 1644, -8695, -9911, 5408, -2313, 5119, 7705, -2073, -1785, 6991, 4613, -7296, 7772, 8525, 7344, 8704, -5799, -9289, 2968, -7343, -1111, 8800, -8743, -1109, 6400, -2633, 182, -4894, -7644, -7714, -5498, 9226, -1295, 9999, -1152, -6986, 6810, 8803, 8378, 1793, -2822, 2772, 7280, 5571, 2694, 3864, 1257, -861, 8988, -7210, 6026, 2895, -7456, -5575, -9641, 3015, 8808, 794, -6488, -729, 9682, -5509, 7119, -2317, -5303, 0, -1459, -3771, 3754, 7944, -2663, 8038, 7786, -9822, -4921, -368, 1518, -1122, -5755, 3694, 3034, 4610, 3532, 7343, 14, 6137, -2274, 1319, 9380, 5392, 2404, 4790, 9307, 5825, 8252, 1422, -6592, 6442, 949, -7187, 6357, 5740, 854, -586, 425, 2256, -5669, 4310, -7572, -8221, 1889, -6134, 7812, -3590, -3175, -5393, 8294, 5976, 4008, -3543, -6317, -2612, -3861, -2077, 8604, 2976, 3251, -8220, -1344, 7666, 3879, -9620, 6139, -8330, 1004, -1225, -2590, 3530, 7280, 3007, 9167, 1417, 7543, 9188, -7415, 4393, -9611, 1655, -2261, 2105, -6862, -1463, -2842, 878, -888, -9813, 17, -5744, -4023, 7066, 8163, -4643, 9600, 3464, 4645, 94, -2573, 1636, -9582, 7702, -2813, 7001, 8658, 3673, -3876, -9597, -8917, -7880, 3376, 340, 1214, -2204, 3482, -9444, 8118, -7011, -2651, -4515, 6653, 4900, 4044, -8889, 9031, -7366, -8761, 311, 8268, 6991, 5074, -632, 6093, 9114, -7961, 6430, 5991, 827, -839, 2994, 5140, 4602, 3901, -8910, 9481, -2160, -3857, 4789, -9292, -4506, -7874, -1961, 9472, 3107, 9933, 4052, 5770, -2038, 7715, 5170, 7, -720, -7163, -5313, 3382, 4688, -8883, -5072, -8150, -2523, 7384, 8948, 2423, -3131, -1501, -2166, -6484, 8151, 3487, -1845, 917, 4614, -9769, -6956, -1638, -3770, -5112, 6110, 9575, -3748, -6899, 1887, -4720, 5981, -3558, 3776, 3447, -6934, 2087, -4457, -3793, 7697, -2847, 324, 2668, 1675, -3499, 949, -3669, 9251, 296, 4128, -9644, 7395, 3913, 6319, -1935, 4306, -9476, -356, 6132, 3011, 4281, 8183, -4720, -7688, 2279, -5589, -3491, -6644, -8670, -8790, -9541, 6995, -6410, 1310, -8378, 6391, 3225, -9701, -4078, 7348, 5300, -61, -9300, -1856, 2677, -2542, -9689, -8177, 915, 8518, -4923, -1251, -4212, -6139, -9076, -7866, 5436, -9806, 9224, 5013, 2639, -322, 9820, -5761, -3186, -3460, 7560, -9207, -1645, 2616, -6114, 2655, -768, 5091, 3209, 1575, 1802, -7252, 7615, 5581, -1170, 1171, 6734, 499, -4996, -1880, 5540, 9065, -1901, 9296, -7445, 190, 2188, -1789, -5128, 208, 9270, -2821, 6939, -677, 3206, 5424, 7012, 7681, -2445, 6983, -7442, -3011, -6878, 3923, 8626, 8478, -4273, -8366, -1571, -1845, -251, -3421, 6672, 6039, 2298, -4786, 6071, -6370, 7450, -4352, 5260, 6601, 8021, -5389, -7501, -743, 8141, 5067, 3477, 4525, -9999, 6051, 8405, 3486, -1698, 6710, -8888, -3159, 5306, 5155, -6235, 9198, 6236, 6219, 4122, 6082, -8533, 3468, -4331, 9788, -2292, 2574, -7145, 4021, -6337, 7380, 9724, -8226, 8725, -3211, 9863, -3840, 1842, -4321, -7264, -7264, -2330, -7933, 3616, 4952, -488, -8754, 9965, -8120, -8955, -5696, 1835, -6381, -7575, -5499, -2919, 8698, -9230, 9570, 2710, 3921, 1128, 4003, -9032, 9790, -9860, 6189, -4301, -6730, -5920, 867, 4313, -1968, -4013, 8202, -758, 90, 1239, -541, 1234, -54, -6271, -3629, 1994, 4789, 6686, 1815, 5064, 3957, -9995, -2670, -6904, -5829, 8595, 1916, 4804, 3735, 8139, -4293, -9588, 3634, 2412, 3898, 1380, 9752, 5806, 3818, 1318, 432, 1783, 2875, -4952, 9049, -1278, 5657, 8760, -7589, 1533, 7691, 392, -1255, -2494, 1464, 1101, -1572, -4477, -7304, 193, -5539, -8452, -4762, -3066, -9235, -4169, -3322, -6323, 1374, 8495, 3364, -7702, -218, 3580, 1096, 8743, -4142, 3965, 7572, -6406, 6714, -4274, 2889, 663, 4822, -454, 5244, -5735, -2882, 9426, 1224, 632, -9714, -4131, -7458, 7900, -7838, -9623, 8990, 8948, 583, -3253, -1507, 9991, -1903, -9623, -7638, 8236, 3764, -6586, -434, 5119, 1660, -4887, -4569, -2694, 8183, 7307, 3124, -8409, -8704, 9575, -8329, 4410, 7026, 8960, -2699, 5356, -3101, 1265, -2712, 9620, -4266, 1249, -8480, -5002, 6243, 1079, 6010, 7750, -3027, -9656, 8856, 3339, 9719, -9669, 5226, 5024, 9265, 7864, 34, -2114, 6768, 3794, 6972, 743, -7221, 3147, -6058, -1283, 5803, -1489, -7833, 2488, 538, 6764, 3078, -298, 9966, -5645, 6706, 3228, 2977, 2291, 3555, -7560, 3790, 269, -7549, 137, -437, -3008, -973, 8330, -8934, 9054, 6392, -9213, -9251, 1851, -4044, 6048, -1517, -548, -2007, 2089, -7, -7532, 405, 1552, -4992, -6700, 6938, -2027, 4097, -3159, -7689, -7538, 1072, 6312, -5559, 1026, -343, 5732, 8008, -9533, -2945, -3361, 5218, -1734, -109, 8783, 7623, 6605, 5561, -8864, -7424, 6815, 3568, 7438, -4679, 4833, 5562, 3708, 5179, 900, -1828, -7536, -7448, 3808, -3008, 7122, 8934, -347, -912, 8658, 7804, -9815, 7461, 1215, -7941, 9396, 1342, 6539, -5519, -9644, -3868, 4308, 6160, -6908, 1741, 5047, -1330, -9534, 9471, 8216, -7438, -5761, -1738, -2184, -2477, -7039, -7639, 3347, -8417, -6376, -9955, -6351, 3172, -8028, 8814, 2731, -1311, -9229, -1733, -9802, -1693, 4661, -3854, 6662, 2027, 4469, 7179, -3849, -5432, 1397, -9918, -4306, 2959, 2491, -4108, 5961, 5934, -319, 6014, -8974, -7973, -4974, 9986, 7507, -9554, -5599, 5969, -896, 9319, 7674, -881, 9895, 1193, -4178, -5481, 6919, 6849, 840, -9960, -9411, 2224, 4171, -4662, -8329, 6653, -191, 8473, 3691, 7113, 840, 2123, -6558, 1728, 2679, -8036, 5793, 1571, -7519, 4366, -6779, 6904, -2985, -5379, -5638, 8537, -7761, 4901, -1780, -2746, -8838, 7373, -5641, -3437, -2061, 3239, -2325, 4285, 8050, 164, 770, 4818, 9402, -304, -8747, 5458, 5611, -3890, 9908, 6372, 1690, -8296, -2335, 858, 671, 5220, -7170, 3004, -3831, -501, 804, 3347, -1448, -7481, -818, -4270, -6015, 2607, 5935, 6355, 119, 5259, 18, -9798, 1135, 4618, -1486, -8687, 4877, 2196, -2093, -4143, 2620, 154, -2602, 8038, -5426, -1435, -6220, -2062, 3070, 5234, -3742, -2566, 6888, 3439, 4461, -1358, -3537, 7546, -4146, 4736, -1635, 2488, 1117, -7529, -7052, 559, -8504, -5291, 9572, -1490, 7948, 9186, 3126, 7804, -3912, -4474, 9415, -7096, -6689, 5297, 6710, -528, -4434, 7027, 6330, 667, 803, -5852, 4060, 9194, 8136, 6136, 6475, -4962, 5094, 3862, -5391, -3526, -5089, 4602, -2803, -5673, 1495, 1914, 5026, -445, -9256, -5828, 8649, -3406, -4059, -8830, -8700, 1970, 4966, 7579, 8865, 111, 6665, -697, 4400, 2244, 182, -3587, -4913, -7253, -9555, 7510, -6561, -5277, 8650, 7056, 4381, 120, -7312, -7124, -8038, -7707, -3484, -8502, -2871, -2726, -5905, -5140, -1959, -4412, -585, 6292, 8395, -5741, -3132, -4299, -5703, -2925, -6235, -4527, 5803, 3407, 1001, 2814, -4458, -7222, 2669, -7527, 5975, 9358, 3625, 1655, -2111, 8598, -5856, -343, 6741, 5850, 5947, -8369, 5616, 7212, -2652, 6211, 5333, -8237, 7996, 9801, 7883, -5911, 7377, 5628, -4489, 7044, 7470, -3846, -7445, 4194, 1790, -2790, -1607, 936, 8336, 2274, 622, -3087, 2703, -5906, 6647, 4013, -6171, -1219, -9563, -6204, -4291, -7697, 2544, 5905, 6413, 5128, -4823, -1238, 9231, -5017, 6808, 5417, 6824, -9175, -1560, -1028, -144, -6615, -1455, -3194, -5064, -54, -268, 8412, -6883, -9625, 9922, 8316, 6500, -324, -5251, -8102, -3649, 7885, 620, -6958, -984, 1831, -9033, -7924, 3977, -3609, -5115, -8097, -7517, -2559, -4186, 245, 9515, -374, 7114, 2887, -6855, -6997, -9915, -6241, -2620, 8229, 4734, 974, 6469, 3491, -8056, -2258, -5598, -8934, 3285, 3714, -6815, -5126, 2358, -5051, 9348, 5521, -9314, -2570, -724, 2872, 7903, -9872, -7717, 6704, -6758, 8206, -6900, -2925, 7641, 3841, -303, -1185, -6929, -2548, 9955, 8152, -9133, 5559, -202, -7789, 8578, -8925, 3298, 4231, -4065, 5254, 4164, 6687, -4918, -3687, -3972, -815, -7993, 9048, 2364, 4328, 4933, -7877, -4426, -5988, -9614, -1596, 3463, 1521, -1649, 8920, 2108, -3506, -3557, 6516, 6715, 4549, -6198, 7634, -5795, -9542, 1808, -4576, 6356, -5335, 752, 2210, -6657, 8436, -224, -4254, 2320, -5544, -8092, -3506, 2228, 281, 9902, -5158, -1550, -1252, -8527, -5036, -9902, -7257, -1079, 927, 2963, -8518, -3817, -9652, 5978, -555, 2405, 9312, -210, 5036, 8190, -4563, 7408, -9909, -7324, -7961, 9635, -5649, -1460, 1464, -4954, 3004, 584, 9087, -6887, -4078, -7504, 2812, -9510, -5472, 9066, -7231, -7909, -4571, -6394, -4109, -5586, -2847, -7892, -679, 210, 9944, -4800, 1668, 6522, -8748, -1536, 167, 6130, -309, 1006, -1614, -584, 6363, 8240, 8377, -4104, 808, -4527, -8133, 6779, 7444, 1318, 3777, -9708, -8064, 5176, -2400, 7564, -9591, -5025, -2931, -603, 9523, 1160, -9294, 4235, 5559, 2427, -6179, 273, 2707, -747, 3325, -3258, 8745, 4632, -5482, -311, -929, -316, -4299, 4255, 5092, 2416, -1857, -3708, -9924, -2036, 3464, -3753, 8939, 1202, 6267, -6247, 7175, -327, -3737, 3140, 1243, 3724, -3195, 3876, 1526, 4573, 3356, -9905, 1973, -3142, -6497, -4725, 8330, -4597, 8647, -689, -4654, 3905, 6772, -9479, 3007, -691, -8996, -6655, 2233, 1411, 8353, -4458, 6568, 6937, 3749, 3428, -5021, -6331, -9854, 3182, -3711, 442, -9780, 7645, 6789, -4816, 3593, 4932, 8211, -2699, 6157, 3473, 659, -9424, -3180, 6374, 5561, 1894, 8839, 9386, -4882, 8332, 2899, -877, -7441, 3408, -171, -6801, 6179, -1743, -7450, -5821, -7062, -8064, -2577, -3197, 3556, 4150, 1344, 5845, -2656, 2768, 8401, -8497, 7313, 5519, -7630, -5598, -7851, 6644, -3335, -7288, 2499, 5592, 9355, -3192, 6549, -7862, 6147, -7961, -6242, -8312, 6226, -2619, 8111, 1595, -285, -5260, -7533, 6337, -777, -6579, 629, 8790, -4294, -1594, -4810, 7013, 7773, 1807, -2640, -2308, 3756, -1306, 4047, 4901, 880, -8367, 6840, -4551, 6267, 8270, 7520, 9922, 118, -8846, 435, -1957, 1161, -5287, 1918, -3431, -7465, -5696, 2017, 8057, 1346, -1044, -3024, 876, -5778, -3588, -8217, 1443, -527, -7109, 8262, 8064, 2548, 8696, 8621, -5181, -6311, 8644, 4120, -553, -9650, 4222, 6212, 683, 4655, 3530, 611, 2869, 6081, 7016, -4102, 6430, 4615, 555, 4583, 4133, -3800, -4501, -6871, -1083, 7511, -2145, 134, 5364, 4777, 7004, -4539, 8922, 2746, -1259, -3325, 8337, 2235, 8129, 4031, -9701, 5587, -6587, 6804, -169, 9674, 3632, -9, -5246, -4281, -4680, -2268, 1262, -7181, 9298, -17, -6719, 8201, 8534, 4305, -9242, 2589, 2435, -7326, 996, -3770, 7452, -7048, 7279, -4036, 3012, -8553, -166, 4192, 7762, 3045, -628, -9957, -5735, -7304, -6453, 586, -8983, 9906, -694, -8261, 6955, 9391, 9949, 2024, -2402, -6701, -6800, -6779, 5589, -5059, -1356, -72, 2166, -5425, 1967, -6331, -1551, 1557, 3520, 8259, -3227, 9276, -7679, 954, 6058, 8976, 6992, -8475, -1732, 7715, -6412, -7866, -4616, -3885, -3705, 1621, 48, -6369, -3967, -6318, -240, 5982, 6838, -2949, 3122, -5308, -2434, -6836, 4672, -7638, 4468, -3227, -248, -543, -2015, 2736, 4505, -7211, -8304, 1897, -1632, -365, -9428, -3747, -457, 9619, -1530, 7698, 1459, 9682, -298, -2381, 3082, 239, -301, -4002, 3060, 3986, -2254, 4007, 3966, 250, -1864, -9776, 409, 524, -3021, -8781, 3471, 1036, -9048, -2359, -8990, -8899, 7549, 2140, 1794, 3402, 123, -329, -4419, 5480, -4339, -2988, 8040, 2381, -1302, -2585, -2393, 9242, 3989, 9570, 7071, -1914, -6394, 307, -5070, -7390, 7988, -8733, 3818, -1867, 643, 9396, 7398, -8336, -8056, 85, -2505, 8679, 9037, -7920, 461, -5799, 4075, -6687, -5902, 5059, 7901, -4414, -7401, 6277, -7823, -5042, -2713, -7771, -8028, -2440, 3179, 9250, -2893, 9559, 2418, 233, -9936, 2582, -9542, -8515, -1024, -9882, 1492, 2665, -2382, -6918, -6361, -6998, 2090, -8792, 9309, 5371, -7675, -9729, -7055, -4388, -1753, -8880, 3353, -9018, 6352, -9306, -7414, 7167, -9142, 9675, -3145, -695, -2259, -1866, -9411, 6945, 2734, -8967, -6180, -794, -4441, -4547, 1754, -46, -551, 1898, -660, 8765, -3263, 1720, 7440, 9017, -3456, -8599, -9181, 2480, 7614, -9559, 6462, -9655, 1278, -1560, -9882, -6530, 1888, 8348, -3107, -5555, -1441, -9872, 6600, -1961, 755, -5438, -2378, -7484, -6798, -7610, -1130, -3032, 3558, -5158, 7785, -5897, 473, 8300, 5808, -7429, -6896, 2594, -8169, 8036, 3570, -7966, -5908, 2380, 6852, -9307, -2134, -3916, 5862, -5560, 9241, -9963, -1944, 9561, -6306, 3577, 5768, 3757, -5977, -3457, -7807, -3049, 19, 1602, 2084, 3347, -3102, -1947, 1694, 4121, 4408, -7838, -1300, -2672, 3378, 2317, -6891, -7452, 8857, -4115, 5492, -2749, 2997, -8185, 7320, -8126, 2360, 7664, -9305, -6460, -8789, -3720, -9411, 7232, 6704, 6855, 2234, 1074, 4637, 2502, -5471, 5482, -3014, 7020, 6311, -3299, -575, -8139, -8471, 2253, 5006, 5594, 1324, 8150, 9744, -6915, -9281, 8401, -3662, 805, 6155, 3089, 4004, -2792, -744, 5619, -953, 6074, -3974, -2289, 2588, 6151, 5121, 7088, -7801, -4795, -3749, -4113, 2395, 7733, 4277, 167, 4020, 9757, -4521, -270, 5294, 5489, 2145, -1359, -557, -3537, -6426, 1891, 7428, 2526, -5460, 5035, 8701, 5494, 2947, -9342, -6689, -8903, 7137, 360, 9820, 4985, 7496, 9301, 7128, 7459, -8249, 1418, 2255, 5176, 2849, 4723, -6433, -7082, 6529, -4863, -5878, 8332, 175, 3874, -1899, -9626, 4785, 3181, 3851, -9393, 4236, -6265, 4972, 1753, -4603, 6807, -9838, 8303, -3427, 3734, 9230, -2238, -5241, 3397, 3804, 3254, -3087, 3095, 5045, -7741, 2455, -4762, -9101, -3450, -2815, -6803, -3516, -1694, 5923, -8149, 8506, 8055, 821, 9100, 5073, 6742, -1036, -1387, -1522, 7705, 8090, 7170, 7179, -3327, 5696, 4463, -626, 524, 7625, -5189, 8707, -7249, 5873, 4481, -8650, 8192, -3549, 1988, -2417, -6404, -1131, 1114, -4862, 3913, 98, -3329, -113, 8955, -7036, -9979, 8383, 1480, 8405, 2702, 3267, -1233, -5204, -4760, 1496, 230, 1272, -5847, -4443, -3972, -2195, -2161, -7841, -1355, 540, -2588, 4451, 9451, -2161, 1696, -2200, 4876, 5534, 9861, 8956, 1493, 3015, -461, -2311, -7694, 399, -4446, 3012, 5030, -6388, -28, 7090, -1266, -1883, -7034, 6043, -7819, 8879, -6110, 7640, -9936, 1073, -7920, 5810, -1550, -5242, -891, 5303, 3708, -9038, -5964, -4313, -6668, 8562, -578, -2799, -1579, -7699, -6659, -1243, 7929, 4190, -4880, -1094, 8173, -4560, -2726, 8456, 3969, -8643, -2800, 2128, 8046, -6624, 6042, 5546, 4267, -6988, -9692, -5932, 9966, -7092, 606, 844, 7700, 2644, -2583, 2586, -1326, 7308, 9421, -6983, -5271, -6631, -2467, 7353, 4864, -1929, 9888, 3826, -3622, 6375, 6355, 569, -1397, -9020, 402, -1531, -1918, -8790, -383, -4539, 3951, 3733, 293, -2309, 6886, 7237, -4155, -4686, 5603, -7430, 7091, -1181, 7688, -7875, -8285, -8703, 2041, -7301, 734, 2918, 84, -1034, 5590, 5905, -5912, -2183, 7420, 2932, 779, -2638, -3741, -9200, -958, -9930, 4258, -1723, 2737, 5493, 901, 4230, 1721, -8576, -9582, 3823, 3414, -2721, 1281, -8926, 7171, 3090, -9814, -5996, -4776, -1630, -2776, -2477, 4407, 5722, 2111, -9615, -4699, 7155, 3129, 653, -910, -256, -1537, 4341, -4594, -1987, 8149, 9124, 7618, 6735, 6844, -4902, 3389, -4280, -1019, 3895, 8344, 1243, -6012, -7726, -48, -4437, -6327, 9768, -9083, -8508, -7545, 9165, 1250, -2297, 8857, -6232, 6648, -2749, -9681, -9956, 4675, 3592, -4600, -7558, 509, 4438, 6397, -7955, 4005, 7797, -4217, -8548, 1667, -7462, -5588, -4464, -3284, 3355, -6553, 5908, 5831, -4424, -5349, -5561, -3271, -8535, 7673, -809, -7988, -7873, -5053, -6146, -8435, -7450, 6656, -1383, -8182, 6362, 7201, 4696, -1908, -493, -9496, -1649, 2890, -6682, 7738, 4711, 5824, -3342, -4221, -1555, -456, 4005, 1490, -5463, 5199, 5729, 3060, -1368, 2301, -605, 4628, -3811, -7810, -2120, -7448, 955, 990, 7220, -826, -802, -7607, 7987, -4211, -727, -1105, -8006, -369, 3283, 6545, 4663, -6361, 9905, -5608, -209, -4796, -498, -6692, -4520, 7829, 7545, 202, -8274, -3764, 3518, 8523, 1775, -5099, 6333, 9959, 3265, -3314, 5325, -5762, 7544, 9717, -2482, 3189, -4456, 4811, -4091, 8304, 7604, -500, 2797, 4290, -7104, 6966, 5146, 1091, -1551, -7708, 882, -2692, 9599, -1419, 1455, 662, -3738, 5888, 5674, -1974, -2740, -2162, -7152, -2366, 9225, 3645, -8652, 8409, -6287, 5278, -7839, 196, 2891, 848, -3108, -5585, 8472, 7020, -7358, 2043, 4308, -1207, -5745, 6634, -185, -5627, -6031, 66, -1840, 8232, 5533, -9748, -164, -1731, 19, -4088, 3481, 2531, 9861, -3861, -5488, 187, -5543, 7292, 6587, -9348, 2139, 9997, 9792, -5075, 1441, -6641, 8822, -8961, 7077, 1482, -4497, 5874, -2135, 9686, -5192, 6870, 358, 5643, -7677, 8200, -8659, 229, -7850, -2664, 2954, 6249, -2850, -1985, 4816, -1996, 8947, 5667, 8731, 1774, 2886, -7144, 4537, -2457, -6550, -5016, -3583, -3758, -8159, 5888, 3242, -3876, -9686, 7531, -3749, -7248, -9106, -6179, -9381, -9306, -20, 8524, 7637, 6493, -5037, 9997, 6314, -2568, 2691, -1310, -2426, 1325, -247, 9174, -7376, 8297, 1636, 6210, 2833, -3665, 7111, 4644, -9607, -5097, -4484, -3865, 1870, -6638, 439, -7265, -9244, -2516, -8535, 4395, 4869, 2926, 6466, 4761, -8036, 300, 6687, 8078, -6332, 5725, 1100, 4238, 8408, 7371, -3754, -6393, 263, -1596, -4642, -9074, 3753, 9238, 3111, 1836, -4705, 491, -1508, -2995, 8694, 698, -1604, 4569, -75, 6106, 4203, -5145, -4123, -768, 8643, -1439, 9779, -9945, -8769, -8597, -452, -1440, -9612, -4436, -410, 3700, -9138, 3386, 4781, -114, -3371, -7551, 356, -4670, 5562, 1237, -81, -7345, -5987, -4604, 5433, -8300, 3979, -6700, -3518, -6067, 7337, 6218, 5374, -1337, -2125, 4063, 154, -1859, -7781, 4971, 95, 5055, 7232, 799, 7046, -6644, -644, -9137, 1343, 6330, -2562, 1717, -3, -4880, -8732, -4643, -87, 4683, 6231, -4933, 3651, -2415, -64, -4423, 2486, -8379, 5495, -8644, -1364, -3721, 7958, 4346, 5217, -5163, 6968, 3319, 9688, -3062, -1061, 8975, 6661, -6123, 6353, 8212, 424, -3925, -7289, -4079, 6568, 7961, 1386, 9681, 5654, -1614, 1766, -6377, -9857, 2082, -9304, 3715, 8814, 6127, -7946, -4947, 4196, 4513, 2825, -9351, 3528, -9629, 3534, 7529, -7997, -1232, -6086, 9648, 5637, -9622, -3978, -3692, 1786, -6704, -7595, 5438, -20, 7585, -7211, 6870, 5839, 2308, -404, 908, 9743, -2861, -9696, 1032, -7944, 6064, -2982, -7470, -4577, -6697, 692, -1742, 3384, -3755, -5085, 7504, 1148, -1795, -3463, 3414, -5368, 1088, -2453, -8872, -4228, 8706, -4437, -7303, -2829, -5030, 8935, -8202, -391, -5185, 8591, -1941, -5577, 8897, 7072, -3495, -881, 4151, 9291, -786, 9357, -4975, -814, -4720, -4027, -4111, 8572, -4628, -7691, 1075, -6139, -3971, -8704, 5841, 9243, -35, 8310, 9363, 6623, -7551, 6438, 5912, 5846, 6087, 5676, -3546, 7721, -6341, -3288, -3934, 24, 1995, -4141, 4583, -3943, 8259, 2493, -8025, 8452, -2314, -7924, -8915, -9523, -618, 5405, -9626, 3009, 9700, 3017, 9515, -4398, 4911, 5722, 6052, 4894, 3883, -7452, -8563, -2721, 2949, -6126, 533, -5748, -7369, 4770, -9513, 8836, 4140, 6183, -9298, 6771, 6790, 2604, -9077, -5436, 7289, -8381, -7317, 2371, -7304, 570, -5993, -7077, 3650, 3818, 3044, -1242, -6433, -5714, -7799, 735, 561, -9600, -7512, 4609, -3441, -561, -4282, -5836, -7197, -1662, -1230, 6713, -2420, -1149, 9839, -8540, -6569, 8559, 5874, -1199, -5005, 7324, 7601, 6263, 5063, 284, -6482, -6098, 2974, 391, 8688, 7937, -2220, 9559, 8661, -4629, 5249, 5010, -5626, -9342, 6786, -3682, -516, -4802, -678, -42, -1734, 5448, 7452, -4066, -8794, -6315, -76, 5902, -615, 2712, 7322, 2841, -6239, -2828, 743, -1997, 2062, 1100, -9148, 6443, -4444, -5343, 919, 7158, 3348, -1961, 2328, 76, 8221, 2276, -3442, -7768, 8965, 6922, -1019, 7242, -5632, -8626, 6782, -5667, 284, 177, -3808, 8911, -4037, 7257, 6551, 5962, -8602, 9207, 4635, 2473, 1590, -8023, -413, -4077, -6850, 737, 4475, 1064, -2153, 7003, -5164, 3632, -7176, 1707, -7227, -275, -4290, 8090, 8518, 764, -5320, -7222, -3280, 8624, -7556, 4461, -9553, -4965, -6069, 2243, 1938, 6646, -2718, -4341, 1025, -1257, -510, 9298, -4406, 2647, 7096, -5415, -7755, -9720, -1920, 2345, -4345, 1716, -7096, -9837, -7752, -2033, 7083, 6369, 3825, -6908, 8911, 7032, 7939, -9132, 567, 7408, -5536, -8805, -2650, 736, 2469, -8294, -1903, 2316, 7931, -1317, 9054, -6177, 1008, -4789, -97, 4606, 4929, 7936, 4760, 831, 2053, 8564, -3529, 3927, -5251, -6732, 4776, 3194, -1723, 3750, 1115, -4894, -4117, -9510, -6758, -2260, 9583, 1232, -1121, 1390, 8683, -2440, 1045, -3515, -427, 7852, -1221, -6984, 4690, -8199, -3927, 2015, -9891, -9056, -4444, -6364, 1448, 7315, 3001, 8559, 5552, 1358, 8521, -5098, 5050, 9847, 5162, 8199, 7313, 4274, -6710, -9365, -480, 1561, -9370, -7664, -5033, -1861, 267, -5947, 639, 9632, 7038, 5916, -7116, -7726, -6913, 5122, 9378, -8154, -2082, -1524, -1982, 615, 9131, -8167, -4536, 4319, 6064, -4218, 6873, 8215, -13, -6807, 3606, 888, 4634, -1781, -3357, 8968, 1037, -8027, -9603, -1354, -642, -7432, 436, -3270, -7790, -5241, -2333, -3115, 1504, -4027, 3995, -419, 6190, 3787, -6790, -5886, -9596, -6672, 6780, -6535, -9298, 5933, 5323, 9444, 7991, 4200, 3424, 8722, -9641, -2573, -5486, -6764, -4028, 9037, -4979, -2582, -9210, -1580, -1261, 539, -8630, 5917, 4500, 6986, 4960, 802, 4771, 1732, -741, -2507, -4858, 8852, -6377, 7714, 6499, 3929, -2414, 78, 4470, -5015, -1426, 6173, 9993, -7483, -7126, -9149, -3058, -1969, -391, -3585, 9758, -245, 7907, 8494, -2964, 2114, 4030, -5291, -5659, 4000, 5221, 5380, -5203, 5792, 3921, 6092, -5894, -8704, -5248, 7589, -584, 5796, 4763, 3654, 8043, 3432, -8821, -401, 4085, -1498, 252, -3507, -7999, 9513, -8523, -1453, 8076, 7766, -4316, -2009, -96, -4930, 5608, -331, 5857, -840, -5238, 5811, -3705, 8379, 5862, -3375, 6498, -8872, 4197, -3390, -6932, -2141, 7645, -4594, -6217, 2174, 8473, -7123, 6704, 5214, -170, 8992, 3268, -6134, 3672, -6094, 2511, 7097, -6717, -7528, 8286, 1735, 8811, 4024, 1621, -8779, 4290, 1153, -9347, -6418, -5428, 601, -2602, -9200, 6395, -3074, 3491, 3956, -9481, 1669, 2278, -7153, -4124, -1941, -958, -1592, 6620, 3088, -1223, -2407, -6057, 2621, 1086, -9150, 4236, -4207, -9505, -6745, -3341, -1702, -1854, -6879, 8326, 9566, -8872, 2544, -9251, -4785, 9948, -8864, 4653, -1447, 2322, -4203, 3517, 7278, -4819, 3733, -8766, 9339, 9996, -8074, -1711, 3949, -625, 5513, -5505, -8780, 8645, 9691, -6036, 1936, -9154, 6388, 673, -9607, 690, -3587, 3905, -5564, -7195, -6981, -1612, -7317, -7512, -3255, -9628, 4891, -3424, 8915, 6862, -4431, 8754, -7844, -9358, 3322, 39, 787, -7457, 566, 9731, 2204, -1753, 1214, 3758, 4373, 8796, -2104, 9620, 7456, 1909, 1378, -7153, 2353, 4883, -5092, -9846, -787, 5025, -3801, 4020, -3505, 6291, -9995, -5249, -721, -1154, -3701, -4986, -6087, 3121, -4152, -923, -8897, 7712, -6401, -1031, -2091, -5575, -8293, -8638, -1826, 2971, 6990, -2481, -7429, 1173, 4244, -3492, -4203, 7599, -764, 6729, 9282, -2304, 5822, 2302, 2542, 5272, 1865, -5911, 3393, 2255, 6208, 5363, -5883, 8660, -6514, -2394, 635, -9514, -7904, -8663, -7820, -9889, -8730, -5439, 8190, -3030, -6640, -5691, 2322, -5516, -8205, 6459, -5471, -6626, -891, -1556, -2849, -7528, 7034, -1512, -2893, -205, 2810, -3151, 9900, 1078, 3583, 1635, 8477, 7523, -3132, 6301, 8972, 7555, 9290, -1299, 6757, 3614, -8987, 1004, 3663, 9757, 7251, 288, 7091, -912, 7304, 5723, -6075, -1847, -5553, 6633, 8516, 3289, -4209, 2637, -8751, -4389, 2118, 2483, 1357, 2445, 3910, -9902, -5985, 4532, -5617, -1455, 9083, 3579, 2220, -7177, -7190, 7885, -286, 1315, -5113, -714, -8404, 3510, 8266, -5306, -5340, 2021, -1640, 8697, 3511, -1357, -5790, 8571, 8698, 823, 9339, -9854, 2066, -705, 5069, -7395, -9515, 1330, -7336, -5756, 2647, -4665, -2566, 8016, -853, -931, 377, 281, 6761, -9792, -1572, -2102, -7639, 8671, -1811, -7262, -2734, -3962, 8051, 239, -2269, -8178, 1150, 4515, -8727, -6490, 6860, -1432, -8189, -7330, 215, -6485, -8900, 6265, 6053, -2729, -2356, 323, 6360, -3471, -4420, 6066, -3007, -6438, -1344, -332, -4344, 2603, -2642, 2073, -9763, 1072, 5052, -6321, -1139, 1355, 9851, -4180, -1762, -2125, -9618, -2505, -2515, 4354, 3541, -5870, -2568, -2534, -7011, 9373, 8368, 2322, -9030, -5889, 8694, 9022, -214, 2539, 6757, 472, -1435, 6083, 9914, -8580, -8347, 8695, 991, -4776, 8150, -835, -8061, -2094, -247, -5154, -3345, -1055, -4371, 8174, -1558, -5037, -8256, 6923, 1436, -1585, -2499, 3017, -6985, 1800, 8907, -9488, 689, 6005, 4753, 9614, 1822, -2967, 1060, -6380, -4765, -6599, 8094, 8226, 1721, -2444, 7164, 5837, 8290, -6702, 7506, -9648, -3850, 434, 188, 6540, -7084, -9397, 1914, 9440, 1566, 921, 6067, 8312, -1012, 2552, 9363, -5116, -9731, -6401, -7177, -8563, -8983, 187, 1334, -37, -3971, -2069, -4355, 2720, -5173, -7584, 9589, 1411, 1796, 1208, -6111, 862, 4154, 982, -2695, 5452, -1146, -9927, 6280, -5717, -9022, 2779, -2036, -2962, -2370, 5663, 588, -488, 9261, -474, -3194, -6088, 8317, -9715, -8251, -3007, 7153, 9949, 9556, -7296, -991, -6081, 2835, 1120, -256, -3840, 1669, -5704, 6225, 612, 7911, -4183, -5473, -6861, -7088, 8381, -34, -9330, 2518, -7879, 4341, 7427, -166, -2897, 4947, -7744, 7659, -9894, 4224, 125, -7658, -3397, 1526, 6999, -1114, -820, -8153, -1054, 142, 127, 6541, 1799, -8100, -3766, 6704, -531, -4028, -4620, -923, 5183, 5489, 7804, -3124, -9171, 4492, -543, -2940, 6736, 7684, -8920, -1908, -4074, -6474, 6058, -4520, 8737, 7263, 2683, -9606, 6673, 9225, 5805, -3563, 2986, 2050, -9119, 6526, 3702, 3585, 4076, 8316, -1098, -81, 6692, 2926, 8880, 7052, 3222, 3178, 9487, 9722, 7057, -8410, 4883, 3105, 8200, -5870, -6953, -7425, 426, 2785, -1468, 4587, 2881, -7573, 1085, 4220, -8685, 7302, 9188, -8674, -2289, -7086, 4175, -1351, -5277, -3068, 529, -6752, -652, 17, 5249, -8696, 9329, -4456, -5723, 8504, 6315, 4608, -8305, 6293, -6983, -6044, 7643, 5285, -9941, -8083, 7710, 8082, 3074, 5155, -9075, -4228, -1332, -5439, 2178, -599, -9891, -3357, -5788, 1525, -206, -9620, 4233, -4607, 219, 6204, 4142, 6657, -1810, -844, -1717, -9193, -5502, -5558, -4112, 6517, -4479, 4160, 5736, -4089, 8109, -9343, -1433, -4082, -136, -5869, -7395, 3469, 2590, 9270, -3685, 5786, -3560, 3299, 7021, -1995, 5787, 1913, -9741, -9782, -3515, -8359, -2587, 6421, 8273, -8267, 9557, 7311, -8633, -4591, 8468, 4831, 2442, -1930, -6226, 4470, -1773, 6075, 2083, 9173, -8616, 5241, -3620, -6714, 3656, -450, -694, -6703, -1555, 5962, 1756, -6711, -1547, 7516, 1577, 7980, -7293, 4315, -3115, 4167, 5129, -932, 8249, 2308, 1088, -3262, -5066, -9652, 4931, 7650, 9166, 2335, 4699, 4759, -8660, 2177, -8809, -7018, -5329, -4037, -6471, 1428, 1258, 7056, 516, 7615, 1497, -624, -6624, 3898, 4182, 6242, 5101, 7432, 875, 2516, 353, -2388, 3972, -938, 9165, 2856, -1024, 2270, 3409, -459, 9957, -3285, 7844, 5781, -5177, -9397, -2507, 8737, 8507, -1803, 46, 6342, 2381, 7460, 4444, 3631, -3854, 7812, -3778, -3621, -4158, -1580, 6632, -2894, -8146, 5299, -7878, -9786, -62, -2377, -9906, -918, -6983, -29, 1618, 6218, 3980, 3028, -7960, 8653, -2579, 7078, 173, -8306, -6322, 1036, -7778, 5242, -2356, 1534, -2275, -6816, 9299, 8470, -4003, -2905, 3127, -130, -5579, -7478, -9062, -1743, 2975, 707, 2826, -3601, 7192, 8202, 9806, 2173, -1849, 6710, -6859, -2909, 5425, 3613, -842, 495, 2993, 5919, -7167, 6609, 2700, 5047, -790, 7044, 1297, 9781, 9972, 5364, -4483, 305, 4975, -2288, 6122, 1312, -2514, 8982, 7486, -1273, 5656, 4088, -8212, 1316, 6633, -6853, -227, 7022, 4646, -7371, -1453, 5541, -488, 4620, 7081, -7596, -999, 8919, -3915, 5385, 2079, -4009, -646, 94, 1970, -9761, -4536, 8461, 4691, 1999, 9958, -4407, 9669, -7212, -4009, 9537, 7402, -7754, 5932, -4167, 7363, 694, -8122, -6613, 30, -8845, -4991, 1509, 3118, -8026, -2691, 6829, 1798, 1907, 6439, -1089, 6816, -4580, 7399, -149, 6286, -4232, 4801, -3505, -8800, 7986, -4645, -775, 8309, -5548, -711, -916, 5013, -4043, 6877, 544, 6012, 7871, 9269, -7031, -6805, 6879, -3281, -5273, -7976, -7181, -4653, 6911, 9967, -6769, 4750, -9685, 850, -9849, 6473, -252, 9834, -4012, 658, -5837, 3170, -298, -1204, 6664, 749, 7810, -6705, 6098, 2517, -602, -2332, -9796, -217, 4566, -8802, 1749, -5820, 9596, 2003, -1169, -4255, -7676, 6495, -5942, -6487, -9914, -6191, -4694, -1856, -9184, 3130, 1054, -2887, 1372, -4505, -2420, 7593, 6905, -9715, 2771, 2573, -3545, -5225, -2914, -4159, -3272, -7541, -9520, -4200, 2660, -367, 3980, 4000, 1018, -3550, -9237, 5513, -6022, 9896, -6012, 6850, 8143, 9255, 8173, 692, -3991, 5257, -3626, 3816, -6328, 3463, 7529, -6307, 5894, -5997, -124, 8963, 9920, -7367, 569, -1157, 7210, -2464, 7449, -2455, 3456, 2337, -4696, 915, 4015, -6238, -2866, -6792, 1109, 6062, 5661, -6337, 4886, 3439, 9615, 402, 4483, -9986, -2364, 2432, -5357, -6738, -3546, -1851, 5478, -7077, 8983, -4780, -586, 3357, -8972, 3002, -8459, -4598, 1486, -649, -1179, 4480, 3720, -2281, -4033, -6720, 9125, 4463, -7710, -8971, 7510, -1567, 1628, 4293, -1934, 2287, 8274, -4958, 2275, 156, 6368, -1620, 972, -3946, 6887, 7745, 7249, -3557, 234, -6010, 8457, -4332, 3249, 7081, 7999, 2023, 8765, -2566, 5738, -2366, -4560, -3952, -4591, 8916, 8318, 7645, -3029, -3693, -2743, -258, -8207, -31, -2901, -8612, 3299, 3634, -8440, -4494, -1198, -5335, 176, -2949, -387, 6317, 2458, -8375, -7471, 2915, 5217, 5387, 8612, 5141, 972, -7666, 688, -42, -8328, -6635, -1551, 73, -1357, -7952, -4014, 1521, -1154, -6119, -145, -5800, 7462, -3013, -133, -8231, -7180, 4534, -4178, 3482, -7705, 6469, -4, -7069, -3989, -8213, -5775, 2305, -201, -8192, 2830, -1815, 7282, 9779, -629, 8943, -1775, 9363, -43, -875, 9283, -9122, 3273, -4937, 3473, 7787, 7627, 7500, -5185, 5893, 1764, 3875, -1864, -9735, -3698, 3590, -4791, -961, 9652, -8485, 274, 9943, -6102, 8094, -6003, 7152, 4869, -5743, -6030, -4564, -1109, 6926, -7886, -8602, -6387, 3055, -7297, -4945, 7196, -9521, -6638, -2614, -5647, 2279, 5868, 3791, 263, 460, 252, -6891, -4410, -7909, -758, 9595, -6159, -2247, -6888, -7462, -2307, 3044, -3444, -2591, -7289, 2390, 9306, 5422, -7781, -3381, -5126, 5778, -7590, -446, 2444, -9045, -5804, 6838, 1712, 5530, 6293, -9959, -1276, -8704, -7940, -8600, 585, 7545, 1252, 6193, 9654, -5327, -8525, -7429, -3340, -7062, 8882, 7216, 1763, 4027, 5838, -2906, 5777, -5143, 9062, -1094, -833, 8562, 9613, 3357, 2108, 355, 586, -626, 4931, 3345, -3341, 7209, 9625, 5644, 1597, 7396, 4086, -5516, -6542, 4198, 6018, 4154, 3760, 3365, 7293, -3147, 35, -4673, -5925, 3069, -8937, -813, 5193, 3898, -3180, 1220, 6420, 18, -9353, 8817, -7077, 4746, -9151, -8782, 2456, 918, 7321, -2260, -1934, -9804, 2431, 22, -7430, -910, 9316, 5252, 6257, -4602, 1676, -935, 5637, 2532, 5435, 5784, -8588, 3137, 2536, -4717, 205, 8187, -5007, 4465, -916, -1308, -5165, 3966, 1467, 15, 6845, -2956, -4999, -912, 9089, 5028, 5871, -7653, 587, 3793, 6147, -6690, 7049, -3524, -8588, 3133, -5826, -3050, 1274, 4750, -6898, -8294, 5601, 1850, 8429, 2718, 6853, -5148, -2072, 7236, 446, -8071, 1670, -8892, -2587, 3856, -8820, -1182, 2883, 1786, -9416, -9004, 5944, -9766, -8167, 9044, -327, 6096, -1835, -1648, 5013, 5316, -9166, 5590, 7839, 9453, 9726, 5559, 1864, -1895, 5805, 6552, 1663, -356, 6271, 326, 7327, 1397, -1287, -2116, -9122, -484, -495, 5898, 8493, -7204, -7336, 2807, 4596, 755, 7519, 2628, 9947, 7963, 2931, 4928, -3643, -7955, 9713, 3392, 6143, -5652, -188, -8896, -9404, -5631, -8900, 2388, 7671, 4580, -8860, -2417, -1742, -2756, 1383, -1991, 4969, 4408, -7114, -7895, -3878, 9249, -3151, -8262, -6533, -5425, 3619, -7256, -4741, -7707, -409, -2147, -4020, -8397, -997, 7768, 151, -7943, 6560, 717, 3457, -74, 6846, -6410, 4138, -9287, -6757, 9112, 8093, 7821, -7283, -1255, -9428, -8405, -4936, -4692, 457, 8784, -4433, 3073, 1434, 1008, -851, -5641, -847, -5933, -6757, -9985, -3880, -2085, -4640, -8532, 4120, -4911, -1507, 259, -928, 5875, 1541, -5384, -1193, -684, -1646, 6708, 3045, -1827, 9002, -9691, -6619, -3414, 9198, -1561, 8744, 8141, 6989, -2787, -7929, -1278, 4480, -9051, -7707, -1138, -9604, 7993, -4305, 5458, 8582, 2270, -3635, 7848, 3848, 6378, 381, 7885, 1850, 2841, -4709, 2576, 5174, 8911, -3352, 7027, 9885, -1309, 9186, 2932, 6757, -146, 3365, 2858, 2045, 7063, -8743, -8463, 2845, -5406, 9405, 9320, -4127, 2194, 9636, -2002, -6051, 9649, 1102, -5102, -3610, 786, -8175, 7610, -9962, -799, 1466, 8243, 8259, 918, -8930, 822, 7053, -1139, -5961, -7646, 6375, -3724, 1846, 3610, -2144, 3998, 7588, 9646, -4546, -5988, -5543, -1071, 2571, 6255, -7490, -5979, -4186, 9412, -4822, -1851, 7998, 2905, 5235, 8238, -8828, -839, 4524, 5801, 2497, 8740, 8108, 5621, 8700, 3710, 6561, -3999, -2531, 9858, 7549, -5632, -2180, -6762, 769, -6978, -3261, 2469, -1940, -2403, -5815, -5188, -8217, 2092, -4959, 9193, -1214, -7901, -1396, -2226, 7913, -8927, -8510, -2060, 9746, 4368, -1038, 6914, 3815, -3041, 2171, 2518, -5657, 9070, -9319, 118, 847, -2802, 7782, -3095, 1783, -5575, -7643, -4761, -8520, -7476, 4167, 4095, -1456, -4320, 2066, 9448, 9309, 5069, -7793, 29, 4785, 1651, -457, -7288, -603, -3283, 8832, -9107, 7830, 5035, 4869, 6328, -1159, -5776, 7981, 3049, -8546, 6680, -7095, 8124, 2159, -9795, 7455, -2646, 4127, -1858, -7811, 2550, 2756, 9339, -3765, 826, -820, 9239, 3503, -6566, 5027, -2812, -9426, -422, -8110, -7047, 2436, -9852, -7844, 9518, -3529, 2222, 466, 9726, 8104, 8994, 3821, -5085, 5915, -9978, 7076, -3223, 2273, -2909, -5557, -1013, -1890, -773, 6574, -7871, -7263, -2972, -7077, -7165, -5433, -1337, 548, -8895, 2022, 43, 1425, 1191, -7350, 1851, -4655, -7604, -2949, -4740, 2504, -5471, 4157, -8212, -8040, -9909, -5184, 4546, -9131, 848, -3232, 4257, 8879, 250, 3730, -8615, -8420, 980, -4361, 8985, -8766, -5549, -8799, -124, -149, 1122, 370, -9230, 4379, -3666, -7351, -2635, -8317, 6361, 3594, -3789, 1674, 263, 1949, 5515, -6310, 4682, -1147, -9584, -5519, 1206, -6124, 38, 6443, -9922, -6203, 3250, 7119, -8410, 7575, 5429, -4560, 5541, -4733, -8512, 2977, -5134, 4449, -7625, 8593, 49, 1600, -4017, -9451, 3992, 154, 7726, 3776, -7707, -9673, 9659, -2264, -7029, 159, -7479, -8143, -6496, 7606, -4702, -7539, 9173, -9483, 24, -8555, 2110, 3545, 8359, -7133, 9316, 4660, 2649, 7050, 2088, -5836, 6121, 7661, 9886, 279, 8639, 5866, -9271, 3466, 6381, -6284, 4185, 6925, 708, 1458, -9121, -38, -6112, 847, -4547, -2712, -8355, 7622, 6682, -1659, -3046, -7998, 1430, -4126, -4028, -8906, 3976, -4179, 1717, -1942, 465, 1525, 2276, -458, -555, 8148, 4646, 3299, -3784, 8231, -3158, 9136, 3311, -8478, -1262, -1539, 4723, 3462, 3204, -2557, -6329, -5381, -2151, -3233, 1082, -1071, -4494, 9423, -8377, 6523, 536, 8146, 4675, -7484, -4706, 6724, 373, 3209, -1057, -356, 1266, -2755, 5414, 3503, -9064, -498, 1751, 7738, 7558, 2558, 5688, 8834, -5801, 2534, 6191, 5314, -5535, -1553, -6404, -5629, -1643, -7162, 8867, 2630, 4394, -9144, 2700, 3427, 8080, -1851, -9751, 4022, -8769, 2774, 2235, -2903, 2727, 593, 78, 9162, -9838, -5842, -4908, -8494, -8246, 8640, 975, 2729, 1215, 9182, -7550, -6589, 3302, 3214, -9802, -6971, -4638, -5323, 1700, 8899, -8384, -9135, 2641, -8306, -6355, 2658, 3264, 4732, -9459, 9522, 8732, 6614, -169, -2289, -2663, -6894, 2827, 9436, -4684, -2601, 2766, -3458, -139, 7445, 9996, 3840, -5909, 2435, 4297, 751, -8472, -5316, 4104, 4408, -4892, -3898, 4504, -8816, -5562, 5015, -6852, -3495, 1578, 9669, 4445, 7176, 980, 3889, 8505, -4620, -3668, 3272, -3355, -4245, -5981, 4433, -3532, 3540, -977, 3679, -4341, 3909, -639, -6747, 5299, -4269, 9927, 7002, -9399, 3986, -3637, -1955, 1843, -4771, 2764, 3423, 7457, 5412, -3829, -4218, 5239, -3155, 2770, -8714, 2621, 2441, -5627, -9428, -3732, -7165, 5128, -1926, -2719, -2175, 9715, 6669, 8212, -4912, -9245, -9285, 8227, -2479, 9326, 1285, -3579, -4332, -4719, 335, -6960, -4986, 2059, 2446, 7600, 8105, 424, 2600, 6462, 9718, -347, -328, 1914, -8397, 4584, 5494, 8866, 561, -9562, 2794, 2368, 8960, 5553, -5999, -8068, -257, -1995, 8092, 3327, 9416, -1790, 1249, 8180, 5779, -8631, 9540, -1102, -5886, 1861, 610, -6313, 2595, 3391, 8583, -2435, 7945, 5923, -1474, -5479, 3250, -3181, -9604, 4859, -9020, -7411, -607, 9588, -2869, 2638, -2822, 2782, -110, -2274, -3382, -1839, -9248, -6457, 833, 3186, 3134, 7130, -6201, 69, -3746, -7331, 9190, 7967, -1485, 5278, 63, -5071, 2823, 5612, 3313, -4834, 9503, 4443, 580, 602, 3445, 8624, -7173, 6368, -1981, -7487, -3383, 5599, 6016, 5077, -5916, -8758, -7095, -5614, -6820, -8454, 6625, -9359, -5435, 6910, 9235, 7198, 2647, 1690, 9986, 7072, 7479, -9827, -9925, -6139, -8805, -9144, 8745, 3622, 8367, 3558, 5466, -6082, -460, 4631, 7204, 4298, -1136, -9315, -6008, -3186, -6384, 6521, 527, -7963, 5297, -3104, -4777, -9940, 1397, -5438, 727, -2804, 3329, -9026, 7880, -9483, 6091, -387, 9124, 8020, 810, -8461, -1027, 312, 1406, -495, -6586, 8865, -3432, 2612, 7266, -9980, 4990, -2862, 4845, 5321, 9861, -7565, 1524, -1623, -8556, 9371, 1901, -7216, 9826, -9673, -9661, -3413, 184, -5011, 6432, 4599, -8637, 4375, 9811, -3958, -8773, 1336, 4313, -6410, 6003, -1740, -3126, 8690, 7390, 817, 2798, 1519, 7935, 4502, 3010, -6898, 2906, 6379, -5911, 3197, -2277, 5391, 7282, -7235, -7312, -1147, 9792, 2395, 9545, -5935, 3602, 3731, -4213, -6600, 8356, -124, -7339, -3959, -5523, 4313, 8338, 5144, -2580, 2676, -3771, 279, 7648, -2642, -3224, -2971, -1425, 8646, 5025, -4231, -8115, -9115, -7428, 2558, -6889, 3063, 4161, 4720, -2231, -4260, -9402, -4954, -7179, -8848, -5228, -4373, -4449, 2955, -5025, -8993, 6673, -2778, -6187, -5238, -2215, -3853, -4610, 397, -4787, 1229, 5542, 6348, 5561, -158, 4945, -7144, 8220, 6488, 7018, 3258, -8615, 51, -3295, 5011, 621, -7814, -9014, 8009, -3703, -8784, 4282, 9783, -7780, -6143, -6814, 962, 9445, 6639, 1377, -4757, -890, 5340, 3733, 6046, 1387, -1881, -7598, -9821, 4936, -9227, 2074, -8872, 4165, 5809, 2595, -1971, -8824, -1413, 5840, -3791, -9498, 7628, 5567, 5055, -9555, -1716, 8684, 3663, -5443, -6891, 3344, 5940, 6790, 952, 5476, -7069, -8826, -8105, 7525, 4015, 5204, 610, -2322, 6420, 7951, -8481, -1429, -1512, -5506, -2101, -3997, -5724, 5259, -7657, -9849, -3788, -9897, 8185, 3646, -5439, -2945, 2752, -1945, -748, -6677, 9276, 3609, 9080, -7269, -4600, -5350, 2011, -9982, 917, -4451, -7112, -4204, -1658, 6599, 2485, -6289, -7819, -1756, -4535, -7653, -5112, -2758, -6173, -8622, 7627, 6672, -6384, 9258, 476, 2868, -5421, -5900, -4106, 7182, 7488, -3243, -3825, -9708, -7499, 6264, 4276, -4434, -5795, 296, -7228, 9351, 7256, 894, 8441, 3730, -9303, 2872, -5030, -1897, 5770, -4864, -5272, 5398, 4316, -9800, 5199, -5958, -1212, 4554, -9889, 5107, 9188, 4298, -1867, 2599, 333, -4872, -8567, -4518, 5587, 3138, -1985, -3604, 8895, 7171, -3389, -5162, 8035, -9533, -2865, -1491, -4273, 1918, 9395, -7298, -2723, -9454, -1612, 4336, 4302, 822, 1230, -4715, -4802, -7857, -6823, -4612, 4021, 1284, -7095, -1769, -9021, 9002, 139, 5497, 749, -5910, -2208, -8929, 8662, -8580, -6461, -3895, -2534, -53, -7176, 9754, -7779, -5580, 6198, -5113, 1230, -8819, -2609, -5740, -9645, -8306, -6952, 3111, -1902, 1886, -8431, -3431, -3595, -3023, 5987, 3022, -2352, 6502, -5068, 3322, -9837, -9237, 6304, 9829, 4760, -721, -5189, 5815, -343, 7169, 1498, 4343, -1592, 1989, 1861, 1010, 5868, -5005, 9898, 9490, -7213, 9115, 4528, -1862, 9388, -5105, 6822, 5787, 5905, -1254, -4571, -1550, 9436, 2123, -819, -786, -1570, -4278, -33, 1511, 5104, 2035, -8491, -2414, -6780, 451, -3246, 8808, 5761, -6351, -7542, 5520, -5065, -3847, -6446, -707, -8430, 7469, -30, 7053, -4062, 2168, -7139, -7064, -7983, -8949, 9906, -557, -9688, 8307, 2754, -198, -4083, 9632, 9311, 4987, 784, -3150, 1378, -83, 2486, -4732, -3991, -1716, 6498, -2504, -7969, 8000, -498, -8879, 1004, 8299, 3407, -4076, 2740, 6261, -703, 1239, 9629, 2251, -7498, 975, -552, -4406, 614, 7970, 9205, 6994, 233, 819, -7178, -8166, -121, 8288, -7298, 2336, -6076, -6538, -4314, 5780, -2193, 520, -2781, -9169, 2226, -5162, 5363, -7369, -7961, 244, -4054, 6668, -193, -3005, 2081, 7345, -9194, 3916, 1587, -466, -6986, -2073, -7104, 1067, 5206, -8837, 669, 6870, 8353, -5091, 8954, 3148, -8142, 5765, 5809, -7540, 9389, 2736, 5633, 5878, 3807, -644, 973, 3349, -7766, -1485, -8356, 3917, -4016, 854, 2868, -9656, -1184, -3767, -7880, 6012, 8004, 8471, -9398, 929, -5486, -3861, 9490, -1558, 8474, -5700, -626, -9383, 4100, 9717, 6824, 9319, -8639, -3574, 1796, 8398, -9420, 3866, 4542, -4192, -1292, -7492, 8862, -6331, 9380, 6366, -5133, 817, 4922, -945, -9183, 8009, -8861, -4677, 1184, -9762, 6999, -3663, -7685, -9252, 9193, -146, -6224, 9510, -8372, 8387, 8154, -2310, -9895, -8030, -9168, 9653, -2656, -4388, -9993, -8428, 4042, -6630, 7359, -7645, -4402, 5873, -3519, -358, -4168, 5397, 1243, 1703, -8845, -7263, -8157, 2405, -7504, 7022, -4012, 1052, -1619, 5421, -2776, -9938, 5347, -591, 8295, -3584, -4647, -6375, -4766, 3690, 5598, 918, 6816, -1354, 8556, 359, 764, 7048, -3152, 9629, 4328, 2298, 1522, -126, -2329, 6003, 1919, -3515, -8660, -3882, 7405, 7004, -7869, 5446, -6901, 9617, 2643, 3770, -2127, -398, 2974, -3726, -5133, -799, 5801, 4846, -1300, 2040, -9358, 5492, -2262, 3872, -5196, 466, -2762, 114, -5567, -5162, -4063, 9448, 1498, 6859, 2152, 7594, -2259, 9333, -9580, 7566, 8818, 8030, 7126, -8041, -1414, -5020, 3579, 7504, -497, 1173, 3625, 2485, 4993, 9192, 520, -8260, 7928, -6303, 46, -5618, 3501, 7290, -883, -870, -3552, 561, -4248, -1687, -7418, -3183, -6357, -1254, -6039, 3564, -7726, 6038, -4634, 2412, 3752, -3618, -7143, -7892, -7192, -2189, 2702, -4110, -2166, -5538, 299, 4119, -8983, 1117, 2890, -2885, 4250, -9460, -7677, 9894, -38, 8076, 9800, -765, 3961, 8318, -3038, -4370, -5253, -4244, -4500, 5762, 9718, 3844, -6761, 7729, -9613, -8410, 6056, 2040, 6704, -6345, 9335, -990, 7063, -9288, -5359, 2976, -4931, -8231, 2137, 5317, -5346, 3774, 8055, -5276, 5226, 5633, 2288, 3624, -1552, 4737, 2475, 2571, -9202, 4265, 609, -5910, -648, 5314, -1409, -5555, 9797, 3099, 4223, -8865, -3068, 7910, 8314, -5240, -7965, 6065, -3306, -2276, 6247, -6013, 1389, -7702, 3493, 7602, -2229, -8782, 5077, -2586, 3108, 9088, 5932, 8427, 5900, -840, 3849, 7480, 3917, 7465, -7273, -5843, -3402, -1330, -7654, 6794, -2203, 2860, 9270, 5320, 107, 2666, 5783, -4264, 9915, -9103, -6109, 2954, 6649, 6712, 4271, 7047, -2302, 8668, -5855, -7741, 6660, 5825, -5539, 1978, -5447, 2098, -2678, 4529, -9977, -1759, -1042, 294, 4313, -564, 6356, 6584, -5984, -6525, -3917, -2257, -393, 4832, 2263, -6914, -6086, -8039, 8281, -7324, -7921, 570, 1429, -3281, -3784, 5182, -2169, 4378, -6323, 6066, 5432, -8677, -8357, 6237, 2643, -7279, -5247, 7931, 4850, -6202, 5081, 5474, 8190, 4705, -7282, -2426, 4028, -9405, 7373, -448, 1681, 7258, 3206, 3575, 4218, -5542, -9520, -7153, 1586, -9728, 7057, -638, -1657, 2694, -308, -438, -4890, -4421, 3742, 5916, -5296, 7176, -3923, 5117, -3797, 6983, 3882, 1058, -5621, -2401, -8674, -2752, 878, 9999, 2566, 3822, -2434, 3507, 1597, 2727, -580, 2239, -2806, -3294, -915, -1556, -6958, 4861, 742, 9215, -1949, -6810, -5010, -8724, 7782, -9488, -9334, 8809, -3329, 2957, 4114, 1533, 7161, 435, -9262, -2415, -6589, -6069, 7905, -1403, -2573, 5903, 5752, -1859, -8002, -1215, -308, -2968, 2004, 244, 8938, -1356, 5332, -731, 3501, 7481, -8279, -8854, 2638, 2902, 5453, 4171, 3673, 8505, -7626, -4824, -9798, -7046, -7084, 6293, 4346, -3912, 6110, 365, -9521, -7943, 5798, -9861, -24, -6208, 4983, -3748, 1797, 7417, -6795, -1498, 4844, -7030, 2949, -6999, -7540, -2632, -6447, -8707, 3357, 388, -6856, -6801, -5535, -3792, -311, 119, -1240, -7912, -5463, -1815, -7639, -8126, -3803, -4707, 5787, -4275, -3271, 7829, -8789, -5506, 9241, 779, -2243, 5139, -6298, -4407, -1875, -6539, -2521, -8857, -6171, 8942, -446, 9401, 1849, -5853, -5878, 8369, 2172, 982, -8527, -301, 8901, 407, -4424, 5059, -1581, 6133, 8611, -3291, -1528, -5637, -3842, -6332, 7345, 5964, 6989, -8467, -4436, -514, 3505, 2115, -5820, 2031, -1995, -2765, 3007, -5610, -1474, -7039, -1211, -1054, 531, 16, 5438, -6359, 7880, 8046, -265, 4832, -1104, 9737, 3904, -5095, 1279, -5368, 5972, 6275, 6699, -435, -9432, 9733, -9649, -9804, -1630, -8921, 8114, -7444, -9333, 5147, -353, -6951, 4416, 115, -1371, 1640, -5620, 3473, 9527, 9544, 8622, 3357, 1993, 3984, 5569, 3284, -7690, 7589, -9508, -965, 3553, -4072, 8055, 6813, 3573, 8053, -1647, -1518, -426, -2848, 6051, -1627, -1658, -1284, 5158, -4104, 2366, 1192, 8046, -3207, -4940, -8466, -1134, 9065, 2269, 1240, 9504, 7050, -1125, -5940, 1986, -3275, -2881, -1115, -9701, -8821, -6055, 8656, -9138, -304, 9545, -5658, -5038, -8955, -9723, 971, 4292, -3832, -6409, -8827, -5094, 5200, -5946, 7436, -9434, -484, -64, 6342, 7460, 5544, -6747, -1386, 5001, 7008, 666, -4988, -4017, 8683, 8136, -481, 5472, 7690, 5130, -3943, -4683, -4842, 1278, 7997, 2883, 5005, 7574, 3567, 8822, -4920, -4457, 8164, 258, -2856, 1473, -692, -5499, -1157, 8074, -8309, 5669, -1526, -5509, -3131, 8661, 6870, -411, -9102, 905, -9534, -603, -9093, -4205, 1497, -348, -7220, -5792, 3290, -2976, -9483, -465, 6590, -3920, 1996, -9364, 5138, -6696, -2377, -9468, -3669, -9112, -9673, -6493, 5074, -2466, 2066, 9161, 8986, 5600, 3793, -1839, -4293, -9045, 6768, 7184, 1860, -8405, 1861, 6604, 5142, -5106, 780, -3261, -5974, 31, -2524, -2621, 9324, 4036, 8648, -9932, 7099, 7840, -4935, 7735, 2011, -7071, 9826, -8624, 2268, -6607, -9544, -2493, -2653, -2350, -3996, 2509, 2383, -5627, -3160, 659, -6947, 62, 9591, 4084, -7744, 8061, 6800, -4937, 8276, -9522, 1437, -5258, 7948, -9514, 6433, 6864, 6500, 8591, 5599, 8140, -8396, 7933, 4404, -1394, -8681, -9407, -4353, 245, 8871, -8393, 1094, 5905, 5613, 4244, -6063, 227, 1254, -638, -9654, -4819, 7119, 7246, -1423, -7715, -9271, 3525, 7360, -9769, 6968, -4088, 2535, 897, -3843, 7492, -4471, 5984, 7580, -9629, -9307, 8595, -7887, 6217, -6076, 1531, -7407, 3921, 3146, 3334, -1719, -1058, 8094, -9823, -6137, 5910, 7180, 9147, 7095, -9841, 5834, -5125, 366, 4210, -2615, -742, 6508, 4777, 4129, -5599, 127, 2865, -5103, 2230, -5880, 1815, 9711, -6712, -804, 536, 4573, 7040, -7908, 2728, -9770, 2828, -609, 5556, -9225, 3099, -6506, 7744, 7544, 1728, -2757, -9437, 5305, -3631, 4117, 4463, -4477, 2124, -3653, 2657, 2991, 2610, -2952, -6085, -9724, -8525, 3734, -8206, 4422, 6631, 9249, -3815, -769, 5303, 4447, -6352, -3325, 9795, 2387, -7256, 4399, -185, -5039, -1794, 4695, -7881, -9085, -5777, 7852, 5791, 7642, -2171, -1968, 9501, 3944, 2447, -2336, -7645, -1027, -6396, 3880, 2280, 1121, 1683, 5196, 9388, 1356, -297, 804, -5968, 27, -2354, 7625, 7399, 7007, 5412, -9429, 2774, -5201, -1168, 5878, 6516, -6286, 1769, -5785, -6030, 9120, -1913, -648, 3703, 5627, 8645, 7778, -3861, -642, -5402, 4252, 7317, 7275, -627, 9697, 6011, -8262, -9253, 326, -8833, 4649, 1224, -1122, 5510, 3928, 5218, -992, -5265, 4673, -5600, 2304, -3811, 1015, -5750, 9777, 5926, -2763, -2410, 680, -7932, 1579, 2359, 4350, 8905, -9195, 4461, 455, 5074, -1440, 2456, 5125, 7232, -1043, 1552, 1142, -334, 5591, -1718, 9973, 9631, 4136, 2184, 7696, 5035, 2323, -5464, -7526, 2438, 8182, 4170, -167, 4606, 9390, -426, 4897, 1734, 2826, 4317, 7905, -834, 7905, 4768, 8409, 7808, 3709, -5445, 8052, 4175, -3784, 5540, 6838, -9436, 2296, 4798, 1158, 8780, -2072, 900, 3908, -6883, -2451, -4474, 5735, -236, -7006, 9692, -1614, -1538, -8043, 6964, -8274, -2589, 2223, 2577, -3125, 3556, -7407, 4116, -7498, -3428, 9471, 5935, -6880, -3033, -529, 6298, -1054, -5665, 9267, -8428, -8331, 2648, 7583, 7717, 5524, -8035, -7098, 767, -5019, -9956, -6358, -9537, 2778, -980, 1973, -5028, 5065, 1297, -8470, -8058, -6441, -4632, -6091, 4892, 5953, -4014, 3333, -7719, 963, 3213, -3417, 1736, 9260, -5551, 2588, 6716, -3277, 692, -8290, 3865, 1998, -4756, 2987, 1750, -5600, 2882, -2766, -6119, -3922, -1835, -4240, -4086, -2343, 4083, 177, -3066, 9526, -7755, -4173, 2252, -9124, -3242, 485, -7977, -7762, 8859, 9354, 7560, 6307, 6950, -9482, 8968, -3289, -4577, 7029, 5186, -6357, 1948, -7349, -3565, -8579, 6508, 8236, 4948, 5906, -5451, 7252, -9747, -78, -3851, -3818, 613, -614, 9860, -7141, 801, -7419, -5663, 9206, -1084, -1555, -5330, -9989, -1534, 6461, -2364, -4660, -4709, -9297, -2630, 9060, -9983, -395, -2245, -5256, 194, -8335, -6313, 4094, -1355, -4825, -3695, 7791, -5262, 1477, -1346, 2337, 5278, 2143, -1357, 8996, -7220, -3024, -9526, 4213, -9684, -2366, 6994, 1482, 3518, 1092, -6322, -9169, -9926, -2336, -1330, 2714, 9621, -4174, -5013, -1384, -7730, -4560, 6225, 2480, -6613, -3262, -7786, -918, 6640, -4802, -8596, -4766, -4362, -4018, -6556, -5137, -2220, -6954, 4320, -6431, 1653, -1296, 1611, 4035, -304, 5459, -1730, 9512, -9849, 4029, 7522, 6542, -8723, -5029, 5312, -6116, 6569, 9030, -8713, 3192, -1557, 167, 848, 5250, -8221, 2355, 4068, 8398, -826, -684, 8817, -2961, -8219, -1458, 6022, -9622, 9720, 982, 7528, -8410, -3008, 7531, 5052, -8107, -7230, -7987, -5165, -7111, -81, 4221, -4458, 243, -2547, 6152, 5420, 5545, -9248, 8809, -8269, 1693, -7401, -2719, 4175, 1949, -5965, 2231, 9613, 8042, 7347, 3963, -241, -5549, -2102, 5969, 4636, 8953, -1104, -5593, -997, 4546, -1635, -1085, -2289, 8008, 2236, 4063, 3312, -943, 2934, 5215, 6021, 7965, 6701, -281, -645, -6309, 9273, 5781, -2792, 7621, 1015, -7542, 1562, -493, 5374, 154, 5396, 8603, -5666, -4021, 4670, 1539, 8445, 7521, 675, -7500, 4076, -548, -6961, 9339, -5291, -8854, -1532, 7607, 7488, -1667, -382, -5466, -3277, -3734, 1203, 2397, -7177, -9786, 5385, -8151, -7811, -7810, -8822, -726, -2448, -9016, -9102, -4594, 3972, 8492, 7577, -1342, -846, 718, 8447, -5627, 1740, -2145, 8065, -9650, -4605, 9213, -2214, -6094, -1355, -1612, 4304, -3643, -7093, -2105, 4973, 7992, -827, 7779, -8086, -545, 4310, -485, -5540, 9111, -1363, 322, -4187, 9548, 784, -9640, -3643, 6589, 7340, 6672, -6968, -2606, 2197, -4440, -5960, -1886, 3381, 1134, -3256, -1294, -3880, 2189, -6730, 1203, 5626, -1825, 6862, -1399, -9012, 469, -5248, 4400, -4487, -493, 1235, 484, -9675, -649, -7611, 7881, 9119, 2741, -435, -6962, -3830, -5336, 7464, -2568, 3605, -7621, -284, 6637, 4777, -3829, 2884, -180, -5944, 3701, -4115, 6725, -792, -7477, 8876, 4524, -1198, -3714, -2640, -7883, -6866, 6359, -9322, -5607, -3980, 2046, 382, 1146, -2759, 3979, -8100, 7608, 3309, 3632, -4947, -8005, 9484, -4947, 3382, 7637, -2306, 1194, 329, 3616, 3069, 2136, 3983, -235, -9799, -7384, 3443, -5223, 5523, -2217, -9099, -8650, 7671, -2721, -427, 7320, 5555, 8370, 9336, -6109, -1309, -1055, -3913, 6395, -7455, -6772, -7777, 7138, -7164, 5582, -9791, -895, -5276, -3779, -2422, 2206, 5416, -9054, 4681, -9943, 2023, 9182, -238, -3225, -2331, 4452, 6435, -23, 6761, -7629, -2802, 5334, -8426, 916, 8361, 5665, -8937, 1262, 4874, 5701, -670, -4124, -3265, -3085, -4642, 877, -787, 3416, -8241, 8323, 3840, -8380, 1918, 2588, -986, -7972, -2648, 3816, -3924, 9497, -9721, 6294, -5603, 6152, -1139, -664, 6954, -5608, -227, 4347, 2014, 6962, -2042, 1544, 3104, -9005, 8425, -9924, -2096, 442, 1578, -8597, -3361, 9421, -9889, 1759, -5640, -8589, 3827, 9834, 4619, 5467, -8127, 4471, 881, -5766, -1732, 8941, 1892, 651, 8707, 4455, -8335, 5187, -8514, 8073, 6804, -9530, 4356, -5605, -2316, 2026, 788, 9826, 1246, -8605, 6826, 1531, -1014, 7143, 7978, -4566, -5794, 1383, -8230, 7975, -4035, -1098, 6688, 292, -1640, 160, -3199, -8253, -7594, -6585, 2425, -2137, 9382, 4271, 4795, 6385, -1957, -5347, -7004, -1970, 6586, 7634, 5468, -3307, -7519, -3960, 8841, 6456, -9607, 6671, -3898, 9782, 6254, -3701, -8230, 6278, -3, -6627, -7217, -8255, 5982, 5874, -4926, -1142, -8792, -1369, -6898, -2992, 519, 1557, 2592, 5445, -2332, 906, -6708, 3249, -4551, -3419, 2541, -7354, 9993, 699, 7410, 2315, -1480, 3533, -2546, -9519, -8940, 9957, 1144, 9413, -9683, -9402, 1062, -8246, 8073, 8744, 3453, -4074, -4469, -5150, 642, 5856, 627, -3556, 1065, -4843, 1081, -6883, -931, 6196, 1266, -8692, -6719, 8191, -8819, 9261, -9235, -9276, -494, -9903, 823, -6024, 3715, 4854, -8940, -187, -2806, 436, -4230, -5653, 750, 5881, 1383, 2055, -9894, 2780, 8307, 3601, -565, 8525, -2054, 2121, 6375, 6087, -224, 8937, -4339, -8448, -2991, 8344, 9281, 745, -7424, -6877, 3637, -2377, -1850, -7683, -7712, 1139, 4706, -2521, 4605, -975, 2419, -9779, -823, -9332, -2569, -3249, 3733, 3715, 5267, -7811, 5723, 7182, 3507, 6570, -2374, -2996, 4579, 9715, -6230, 255, -4313, -5550, -1473, -7550, 9227, 5512, -1200, -3055, -5111, -3003, 7344, 3419, -2912, 4952, 6406, 9448, -5948, -1528, 5204, 9106, -4211, -9750, 7878, -4792, -1337, -1737, 8776, -4881, 6739, -9807, 5837, 5283, 4565, 8657, 2334, 2677, 1162, -8243, 9202, 342, 6435, -8809, 5655, 8893, 4679, -8084, -4468, 9183, 4312, 3858, -4052, -3202, -5301, 9331, 5601, -9135, -2516, -2156, 5054, -2134, -6381, 2231, -6127, -554, 2616, 3040, 6406, -8740, -7770, 8443, -9016, -7296, -150, 7049, 7141, -2115, -812, -2038, 8610, -6146, -5415, 7079, 4767, 5305, 8666, 9721, -5048, 5156, 52, 1661, -2486, -3911, -666, -7764, -5099, -6560, -2140, -8850, -7048, 2782, -3722, -7278, 6878, -3271, -8567, -9758, -1796, -3461, -615, -8186, 891, 452, 5360, -3874, 6623, -7853, 1577, 9134, -4736, -9089, 832, 6226, 2731, 7536, -143, 8755, 304, 9356, -6406, 3526, -5639, -6788, -5629, -1561, 1709, 9871, -2561, 6189, 2192, -7791, 9387, -892, 848, -9072, 1285, -8904, -6114, 8588, 6223, -7355, -4458, 9596, 7676, 657, -3531, -6163, 7678, -239, -7324, -4351, 5019, -8901, 892, -9074, -8218, -6914, -6218, 3556, 3355, -6516, -3705, -6878, -4983, 5191, 8342, 2128, 7871, -2370, 3, -8080, 7083, 410, -3148, 478, 2228, 1987, 7751, 8852, 8082, 8522, -9691, -3832, -7815, -8816, -115, -5007, -9188, 6027, -265, 4047, 9417, -7266, -954, -2194, 5497, -2291, 2236, 3449, -3526, 6871, -9651, 7942, 7516, 3566, 2038, -4069, -9337, 5094, 7821, -4768, -345, 581, -7136, -6032, -5546, -3496, -8220, -5532, -1469, -5735, 6200, -8064, 5100, -7755, 5510, -462, 1532, -9284, -4312, -5391, 9968, 5433, 3497, 6923, -6798, -1628, 5476, -2024, 7314, 1278, 5391, 6546, 8008, 3692, 2854, 7383, -4702, -2343, 3762, -2090, -1961, 3995, 4053, -5996, 6547, 997, 8405, 32, -8465, 8045, 6727, -8533, -9493, 8072, -1937, 9456, -7755, -4455, 7354, 1297, -3554, -4882, -397, -1009, -7760, -472, 7903, -5024, 4704, -326, -1584, -3681, -1827, 9000, 5432, 6925, 7222, 556, -1986, 723, -4274, -6786, -4879, 4235, -2132, -9244, -4791, 1746, 6000, 5489, 5991, 4254, -8202, 6523, 4008, -2620, -2061, -2822, 622, 1668, -9923, -210, -9872, 460, -9956, -9223, -111, 2099, -8730, -7576, -617, 9771, 6765, 2729, -9100, 9221, -7183, -1413, 7145, 8292, 468, -7586, -4793, 8725, -4686, 2420, 4525, 8088, 4215, -2190, -7776, 1106, 9835, -7981, 5453, 450, -6092, 6603, 3837, -3476, 4037, 6668, -7432, 9316, 4926, -6178, 77, -1673, -3196, 1016, 4852, -5281, -6482, 6450, 4995, -1346, 5552, -6094, -3619, 6237, -5948, -9048, 3337, 1223, -1581, 3571, -2658, 1868, -6507, -8159, 1032, -3047, -7307, -8933, -1299, -2991, 6635, -9649, -8450, 7318, -5733, 8931, 541, -7128, 9681, -8015, 3728, -9704, 555, -7131, 6463, 5505, -7506, 1459, -1957, 470, -9800, -990, 2717, 6323, 8113, -2608, 3194, 3011, -3501, 2608, 5641, 5732, 3761, 7715, -41, 21, 809, -1196, -3795, 6351, 1511, 5170, 1410, -784, -9824, 8639, -24, -5274, -2975, -7871, 7095, 3465, 2413, -6084, -6934, -6455, 4267, 9543, -8359, 1767, -6932, -8202, -2541, -283, 3672, 3017, 1904, -7827, 1631, -7421, -1145, -5740, -1001, -7187, -3052, -1762, 222, 1837, 9794, -1479, -7490, -3645, 6216, 8985, -3681, 4145, 6512, 3844, 1994, -6790, -9029, -8830, 1011, -7791, -1663, -6055, -5330, -1949, -414, 2644, -7691, -5514, -4407, 3452, -3100, 7347, 6757, 1649, 4766, -7263, 2301, 9930, -6161, -82, -2280, -36, 9963, 5091, -5422, -2739, 9844, -9717, 4000, -4995, 4437, -1548, -4342, -5983, 4965, -6364, -472, -6287, -978, 6780, -1803, 9452, -4085, 5723, 6928, 8429, 6634, -2416, 3456, -8414, -4687, 1850, -3168, -702, -7032, 5617, 7447, 4421, -4591, 2470, -757, -4935, -43, 2179, -4521, -2099, -4918, 1962, -3090, -8120, 43, -581, -5396, -2588, -6435, 2762, 8541, 9492, 5846, 5364, 2467, 2289, -9547, -9026, 8777, -170, 6341, -3163, 9692, -5862, 3153, -6574, 7731, -5829, -1143, 7033, 9369, 343, 4874, 9286, 2842, 1447, 2305, 1210, 8963, -8354, 5188, -8039, 8461, 8008, -5064, -6193, -7316, 6405, 7128, 9213, -6092, -9138, 5303, -7145, 4775, -2265, 9433, 8483, -8299, -2818, 2156, -9835, -52, -4767, -1855, -7346, 3033, -553, 1567, -6118, -9535, -2611, 387, -1378, -5856, 6533, -3659, 6929, 2749, -9226, -1179, -2911, -6575, 7655, 5749, -3046, -1261, 8972, -8982, -6509, 5589, -1825, 8926, 4361, 4648, 9004, 2043, -7178, 6001, 3198, -1166, -269, -2723, -4968, 4022, -7605, 7427, -9589, 7106, 1513, 3617, -704, 1977, -5632, -3554, 3318, -2298, -8410, 164, 2552, -7967, -2870, -8434, 9530, -2187, 6499, 3182, -2319, 5962, -2689, 1492, -1546, -8827, -1413, -4360, 5989, -8275, -3102, 5758, -425, -1785, 4342, 5382, 2146, -9144, 7701, 5013, 2235, 8621, -547, 939, -8747, -4646, 9977, 8067, 9264, 3660, -3987, 4319, -308, 7680, 8349, -8456, 8224, 2205, -1689, 8502, 7050, 5732, 6254, -2699, -1422, 6893, -1242, -5301, 2367, -6841, 9121, 2876, 2319, -4424, 1855, -5279, 9071, 5337, 1446, -5225, -1077, 2540, -3140, -310, -4476, 8517, -9765, -7215, -1275, 5567, 5508, 1117, 5752, -6593, 7639, 3828, -4774, 4751, 2167, -6056, 7753, 64, -7616, 6854, 7297, -3327, 7693, -7975, 1395, -6775, 5138, 9115, -2973, 6019, -4395, 1107, -1901, 1417, 5714, 1100, 9450, 951, 7277, -7106, -7471, -7261, -6915, -4272, -5136, -6366, -4305, -8790, -537, 2730, 8651, 9122, 7275, 1467, 2773, 1914, -2170, 3359, 454, 1213, 531, -2626, -4587, -3460, 6008, -5203, 1053, 8366, 853, -7549, 6159, 7618, -7994, -7529, 7088, -766, -22, 8359, 3406, -5216, 7506, 7049, -7425, 6315, 2021, 8753, 3396, 9070, 7552, 8723, -1348, 3344, 7958, -746, 1337, 6766, 6021, -5264, -9948, 5880, 831, -552, -1588, -3914, -6366, -9177, -8290, -6298, 7272, -5336, 9134, 8756, 7070, -3448, -5550, -5819, 8061, 2699, 3575, 6925, 8187, 5604, -5712, -1669, 9024, -7206, -2057, 1139, 3836, -7427, -6348, 2961, -5633, -5175, -7151, -1510, -6194, -8123, -7188, -6953, 884, 5173, 7877, 3135, 5649, 2755, -8562, -1532, 4665, -6532, -9813, 8808, -3956, 2809, 5265, -3133, -5377, 6962, -1012, -6057, -6246, 8172, 1572, -6627, 9997, -9410, -6944, -7030, 676, -8194, 4168, -9001, 5260, 5859, -5232, -8991, -5726, 2337, 8515, 4219, 8310, -3573, -1207, -3668, -1446, 754, 239, -3952, -259, 3943, 5116, -5695, -6456, 245, -8794, -3911, 1710, 4221, 8400, 7564, -9101, -5489, 8573, 5354, 4041, -1380, -9944, -8348, -7478, -2200, 1647, 8572, -251, 5456, 3569, 4522, 8797, 5501, -8428, 1426, -7187, 4743, 618, -1844, -2568, -9532, -9055, -7108, 8923, 5413, -5713, 763, 2621, -8750, -1372, 8356, -5256, 1493, 4539, -8296, -2669, -5081, 2906, -4482, 4912, -8620, -3375, -251, 7491, -7685, 2894, 9481, 5405, -8249, 3496, -6270, -7787, -9692, -9037, 3162, -1258, -3418, -8168, -135, 4630, 3042, -6826, -2650, 9501, -8926, -8627, -537, 9138, -8332, 8374, -4526, 5838, -8807, -3051, 6809, -9449, 6135, -4600, 9461, 6482, -3991, -2934, -3195, 5976, -1150, -2055, 8088, 9920, -6648, 8549, -8606, 1332, -4433, -6933, -2632, -6982, -5631, 1029, 5132, 8586, 4446, -822, 3918, 3513, 2194, 1320, -3554, 9953, 3791, -7613, 4681, 4230, 6971, 7406, 1155, 5010, -9120, 3965, -9924, 4099, -5508, -8096, 8277, 220, 9577, 9767, 4818, 7865, -9875, -4762, 2950, 944, 1846, -3139, 1581, 4796, 570, 8487, 4641, 2630, -1342, 1813, 7520, 1993, -1878, -633, -1984, -7051, -3572, 6341, -9428, 9001, 7955, 4474, 8557, -7701, 8310, -1221, 2176, -6215, 7834, 7921, -676, -3091, -8336, -7005, 2129, -6955, 514, -1024, -9225, -9436, 1411, -5505, -873, -3302, 3944, -5937, 4862, -6820, 8536, -6540, 2885, -4410, 634, -4029, -5648, -7569, 3771, 1275, -7376, -7090, 6899, 8289, -9224, 5133, -7051, 7651, 8144, -3275, 4286, -5449, -4258, 7403, -2820, 976, 7855, 3307, 5927, -9580, -7164, 7164, 519, -334, 1862, -1704, -2390, 3374, 1120, 1850, 3571, 5582, 6159, -660, -4478, -2407, 5079, 1501, -2958, 4936, -9976, -8479, 2870, -1445, 3672, -4942, 6821, -2249, 3509, 5809, -8223, -9151, 9505, 5915, 379, 6860, 1608, -346, -4651, 7092, 246, 5005, -466, 4571, -6969, 2200, -3601, -1499, 2990, 7218, 7612, 9975, -6426, 6940, 2207, -2891, 6980, 7276, 3357, 6355, 3107, -2318, -4008, 9340, 6454, 345, 9641, -7910, 7079, 5748, 2963, 6486, -7340, -6597, -3090, 7961, -6789, 2227, 7255, -9941, 3837, 5141, -2107, 2360, -6313, -3586, 4542, 1059, -8247, -1551, 6514, -9448, 7186, -3796, -2141, 3351, 5210, 9689, 7144, -2062, -1539, 6505, 3810, 4004, -2196, -5622, 6548, -4917, -6799, -7429, 4483, 2500, 5492, -6574, -3316, 1166, 5930, -9528, -2039, 1984, 4552, -2109, -5013, 5060, 2108, 4221, 905, 5464, -5023, 1156, -9288, 2874, -2671, -7413, 2057, -7738, -9180, -6033, 7602, 6600, -6174, -3404, -4124, -857, -9290, 8363, 5176, 7502, -7469, -3812, -7029, 8864, 1324, 3783, 5196, -8918, 2615, -5130, 8109, -2850, -4254, -1150, -8674, 9371, 2028, 9261, 9598, -2372, 7112, -2236, -2179, 6147, 4107, 5443, 3369, 3276, 4426, -3494, 8314, -4976, 819, 1045, -3271, 6784, 2169, -9034, -6518, -5672, -28, 829, 3253, -7628, -4299, 5822, 3502, -3365, -7232, 2631, 9054, -4912, 7195, 3619, 9954, -3012, -9872, -6033, 3743, 6437, 7656, 3136, -6914, 6261, -3269, -516, -290, -3452, 6541, 6025, -5507, 1502, 802, 2222, -390, 4696, -6191, -3931, -7712, 8412, -1423, -1509, -1169, -7565, 5678, 7328, -9640, 6860, -1631, -9918, -6918, 6021, 8025, -6630, -5612, -3109, -1746, 6119, -6930, -1023, -7066, 5867, -4582, -7029, 2259, -3288, -7843, 2451, 8648, 3456, 4159, 1349, -7142, -2350, -9558, 4550, -3762, -8817, 7277, 312, -9913, 7364, -2689, 7080, 9232, -3534, -9110, -7975, -8948, -6319, 8629, 6240, -9652, 9429, 2017, 4930, -5041, 952, 3602, -4155, 3751, 8870, 8294, -2605, -7770, 7405, -3377, 4041, 3553, -8104, 1766, -8757, -4433, -5711, -566, 7241, -5799, 9581, 9250, -8773, -4348, -2443, -4310, 170, -9069, -3304, 2253, 4696, -4144, 5052, 9503, -8577, 5137, 1787, 901, -8262, -6736, 6608, 9737, 1412, 9981, 3311, 6789, 8459, 4547, 1079, 7972, 8173, 7166, -6342, 2033, -8906, 6038, -8170, 4650, 44, -4605, 5686, -8313, -5618, 6545, 7506, 8146, 9011, 1676, 7170, -5892, -3262, 3324, 1132, -8216, -7549, -1043, -920, -3305, -4175, 3174, 8148, 1095, 999, -4801, 5076, -3637, 5655, 1706, 8041, 6594, -8601, -2642, -2073, 5372, 561, 8476, 2866, -2073, 4482, -6413, -8892, -3529, 70, 5234, 1240, 6120, 9048, -3541, -6727, -5745, 7841, -1925, 9806, 6335, 1465, 1839, -9167, -4513, 1754, 5566, -570, 4021, -3574, -8974, -5352, 9398, 5381, 709, -1690, 1993, -2396, 4991, -3828, 5010, -3750, -6647, -4463, -9297, 9865, 1217, -7485, 188, 3167, 3195, 2732, 4425, -8424, 1632, -2312, 159, -751, 78, -3553, 8518, -5203, -4769, -4777, 3055, -3291, 9752, 7889, -9796, 7670, -8222, -340, 5055, -5333, 4620, -5165, -1109, -6131, 6284, 8939, -8372, -8018, -763, 9896, 2508, -6932, 5914, -4810, 170, 198, 9686, 4715, 5652, 2563, 1457, -1813, -7075, -8777, 5102, 86, -3422, -6434, 4326, 4431, 1900, -7935, -3049, -3481, -1977, 4232, -814, 3474, -3910, -2837, 2132, 5102, 4336, -5724, -6209, 3410, -787, -573, 3820, 2899, 613, -1963, -6559, -744, -4539, 7258, 3328, 762, 2509, 3170, 9461, -1182, -6770, 4949, -4757, 3086, -5155, 2575, -9993, -9624, 7797, -6368, -5985, 3257, 1721, -9962, -1732, -4391, 8978, 197, 1657, -1904, -5884, 4442, 2722, 453, 2558, 283, 7801, 3141, -7307, -2495, -2576, 5040, -5187, 1258, 5110, -9916, -3529, 2885, -9861, 2294, -26, 5899, -5516, 7502, 3544, -2770, -1998, 2929, -9951, -9814, -5934, 3855, -1986, 2473, -3117, -2995, -6770, -9095, -456, -4479, 997, 3406, 3323, 7483, -9416, 9812, -8672, -5141, 7907, 4096, 4225, 9897, -6668, 1496, 1880, 1110, -9189, 3959, 955, 3564, -4024, -7374, -8888, -2146, -6967, -8718, 1832, 1610, 3129, 2579, -5679, -4952, -5394, 4990, -585, -4075, 7907, 9721, 516, -1790, 4244, 1055, 657, -8238, 2512, -2326, -5257, -4577, 6786, 1541, -6559, 6368, -8594, 7530, -4381, -7714, -3292, -1511, 1502, 4810, -5071, -169, 5923, -811, 6813, 1962, -7035, 8514, -8977, -3741, 1648, -5967, -3911, -6677, 6623, 8654, -5197, 6539, -7642, 5140, 1918, -8803, -6473, -7796, -3667, 6474, 9115, 2171, 3559, -1994, -7927, 6799, -595, -4239, -2584, 9896, -7497, -565, -4680, -8768, 3631, -9017, 228, -2910, 809, -7093, -7612, -8027, -4341, -7885, 8706, 690, -9159, -6664, -217, -4309, 9159, 4899, -4687, 4298, -2284, -3740, -2368, 3726, 7361, -5718, 8691, -8425, -4178, -2863, -285, 8601, -4067, -5095, -2551, -6524, 5041, -8657, 8794, 7869, -4395, -2170, -1779, 1391, -7758, -882, -468, -1963, -7343, 2097, 3479, 9149, 1725, 2666, 5750, 4720, 9990, -2915, 5569, -3770, -5126, -100, -5907, -5155, -3412, -4148, -914, -5854, -5660, 9112, 1531, 8426, -1835, -7712, 9536, 3304, -4084, -2641, -4750, 7501, -3769, -861, 5766, 6261, 6348, -8376, 2267, -230, 6323, -1553, -1230, 255, 504, 732, -1972, -3177, 1307, 4286, 6971, 339, 1570, -5588, 8517, 47, 2671, 1056, 6048, 9851, 4705, 4992, -1198, -413, -2914, 9989, -2339, -2062, 4272, -4787, 2890, 1556, 9276, 4494, -1713, 8226, -2234, -8934, -939, -2756, -180, -7296, -8028, -8864, 7552, 8452, -70, -3612, -1823, 111, 2792, 292, -4572, 1369, -340, -863, -6927, 8144, 2167, 449, -564, -8907, -8509, 8760, 326, -7424, -6187, -4665, 6111, -877, 5469, -7716, 8389, 2457, 9453, -5951, -4210, 5053, 7387, 3659, 6653, 4539, 6594, 4354, 1735, 9686, -5645, 2464, -966, 8843, 9560, -5570, 5563, -6101, 7002, 2110, 2546, 7870, -7569, -5267, -8805, -63, -9455, -7706, -132, 5718, -1985, -1782, -317, 9297, -3685, -7267, 5238, 1670, 8243, -8418, 9713, 5852, -9516, -2919, -5335, 733, -5947, 7884, -2947, 4246, 4229, -1798, -6561, 6329, -9895, 6301, 7882, -9457, -1023, -4564, -9732, -7531, 2600, -4714, -7992, -4165, 3821, 1370, -2833, -9933, 839, 8411, -7613, -2687, 5474, -2251, 9214, -2363, 4133, 5459, 3375, -3964, 3236, 9776, -2956, -4873, -1134, 1674, 7231, 4640, -6783, 9509, -1298, -9604, -3967, 7329, -7784, -8883, -1014, 740, 4251, -2337, 4596, -8843, -8738, 2580, 3467, 1199, 1774, -2038, 8450, -1001, 5855, -4379, 891, -4237, -5258, -8007, 7809, 9906, 9386, -1229, 2599, 5191, 3142, 7072, -9777, 4270, 4020, 9364, -5173, 7947, -2915, 9797, 4195, -4003, 5845, 6699, 123, 7489, 8472, -9600, 7558, -8293, 9121, 5525, 7025, 7542, -458, -732, -3484, -529, 6317, 7962, -6437, -1853, 3373, -1139, -48, 4144, -1911, 6140, -2368, 6058, -247, -9320, 4683, -9854, 1631, -6291, -5303, -7565, 7676, 7705, 3154, -7560, 5295, 6934, -1223, 6401, -8588, -638, -1260, 2717, -4806, -6600, 9373, 708, 3657, 5461, 4247, 9237, 5630, 9860, -1267, -1150, 6509, 9956, 9455, -7250, -1265, 2927, 2595, 8655, 7424, 2768, 7914, 2534, 419, 7326, 5717, 8739, 8670, -8220, 1260, 2642, -3315, -6218, -4294, 2282, 7882, 5197, 3171, 8971, -9557, 9127, 1462, -7385, -3493, -121, -5338, 9527, -5916, 1420, -4848, 6436, 8491, -5879, -318, 5170, -5288, -3576, 1851, -7937, 7093, 7135, -9961, -8685, 7953, 3794, 1012, 9108, 3161, 1082, -7892, 4362, 3245, -5542, -8287, 3194, -2352, -6598, 5476, -7570, 442, -2929, -7028, 2138, -5879, 1294, -1947, -1726, -4950, 4385, 1308, -4001, -9651, 3989, 4511, 3693, -635, 2256, -1171, -2055, 9673, 6054, -8013, -6265, -3528, 7021, 786, -3357, 4325, -9418, 4396, 6690, 8176, -7547, 1837, 3745, 4430, -6441, 6088, -7496, 671, -5243, -8015, 7340, 7938, 4909, 309, 7001, 3518, -5742, -8005, 2502, 876, 5558, 7454, 4723, -5808, 7542, -5198, 8537, 3698, -5428, 3174, 6235, -6216, 5349, 6813, 4157, 7080, 2544, -5326, -2391, -911, -9966, 9019, 4220, 1952, -6540, -6505, -7639, -7306, -2442, -5693, 6101, -5272, 8847, 4926, 7750, 3133, 407, 1602, 8343, -4953, -924, -7977, -5512, 2162, -3196, 1903, 1792, -772, 5390, -2335, -7917, 5227, -2563, -667, -1760, -6424, -9115, 8496, 1715, 2313, 4016, -6577, -86, -9238, -2229, -6547, -1779, 6074, 9985, -6227, 1418, 3259, -6655, 7655, 3964, 6039, -6671, 847, 6093, 9765, -7516, -7165, 5879, -6496, -4358, -7201, 494, -5369, 7441, 4575, -392, -3515, -3152, 1675, -2474, -3323, -8356, -7885, -3136, -8633, -7205, -3861, -3019, 2694, -3362, 3880, -4985, -8368, -1940, -3448, -5735, -7308, -3205, -636, 9664, 5254, 7628, -9193, -7562, 691, -3086, -6734, -8333, 1276, 4360, 2056, 7352, 5110, -7137, -5149, 9747, -1466, 4105, -9646, -1439, 8214, -859, 9612, -9582, -583, -4492, -9670, -675, -6575, -3295, 9305, -3412, -3328, -8544, 2193, 4999, 6896, -1063, -9965, -9185, 217, -741, 1677, -2325, -3845, -7055, 7069, -5349, -565, -3099, 3747, -2237, 6954, 3084, -9325, 1442, -8672, -9286, 6113, -4405, -1874, -2906, -8084, -219, -9323, 3102, -3320, -6063, -4767, -1403, 4503, 2922, -2721, 7537, -7793, 8386, -7057, -4885, -3399, -8251, -4442, 8758, -5880, 4407, 4581, 1339, -8613, 5951, 6117, -1199, -823, -6761, 896, 7527, -9160, 3749, 7940, -9919, -140, -241, -7775, -3947, -6524, -8368, 5497, 507, 5621, 1522, 2382, 5423, -2892, 1933, 7736, 7437, -984, -2384, 1931, 3287, -7513, 5943, -6732, 315, 5053, -771, 6352, -5815, 6093, -6877, -5316, 5589, 5557, 1787, 9849, 7833, 5392, -2473, -1895, -2366, -8161, -3531, 4950, -5713, -3644, 34, 2474, -7736, -6088, -5453, 723, -5792, -474, 8909, -1531, 2572, 4110, -527, 7801, -4238, 9007, -8893, -5088, -4624, 3108, 8563, 7389, 2293, -9992, 8901, -1123, -1569, -6924, 2464, 6611, -1578, 966, -4497, 7725, -3950, 3587, -891, -9135, 8534, 4035, 5088, 2820, -5895, -8465, 2758, -1529, 9461, 305, -1345, -2700, -1585, -1300, 4553, -924, -49, 7079, 6762, -5875, -7360, 2571, -1115, 487, -8193, -1458, -316, 7736, 181, -9688, -415, -702, -8177, 494, 1780, -8846, 5994, 7064, 983, -6904, 6054, -9104, -2100, -5067, -3135, 9938, -9095, -8284, 2658, -5111, 3593, -4450, 7972, 8524, 9882, -8485, -2741, -257, 2788, 3439, -7565, 7017, 2747, 8919, -7793, -8173, -1838, 2596, 2255, 1046, -5158, -6164, 9555, 7262, 2694, -9334, -4641, 7570, 6218, -6709, -2798, 244, -3707, -2625, -669, -6710, 7293, -1443, 9656, 1865, -8282, 480, -5001, 3833, 5178, 3767, -9512, -9283, 5545, -7383, -2548, 1898, -9013, 9464, 64, 2429, 9404, -391, -1880, 1422, 800, 303, 5128, -7656, -1743, 506, 8714, 7573, -8680, -19, 7648, -6108, 9903, 9962, 2979, -880, 1158, 7216, -6801, -4107, 3806, -6580, -5622, 4176, -4893, -9259, -6269, -5119, 4677, 2630, 4337, 5763, 1674, 353, -9837, 2035, 3343, -9083, -3234, 1782, -1163, -6650, 8609, 9606, 9143, -4997, -1212, -5353, 3730, -2545, -9190, -5291, -9639, 6865, -9788, 837, 1232, -6803, 5777, 3383, 9014, 3571, -7767, -6151, -7350, -1328, 9290, 6903, -4001, 8813, -7423, -9764, 7366, 8977, 5252, -5002, 4155, -150, 9376, 2862, 2593, 867, -3693, -1639, -2875, 8163, 8856, 7671, -7359, 7913, -4528, 1601, -1872, 9128, -8058, -3345, 9481, -9965, 4305, -7405, -4941, 2959, 1698, 6042, -414, 6861, -645, 2803, 8585, 4972, -5131, 3302, -4036, -2648, 5931, 1653, -7142, 3877, -1792, 6830, -1760, 8725, 5278, 8419, 8148, 8769, 7998, -7065, -5507, 8688, -6985, 3307, -3067, 1029, 7433, 9000, 1597, 2231, 4026, 3449, 2313, -7457, 161, 5622, 4650, -151, 1296, 5912, -5572, 6174, -8268, -7457, -2143, -3235, 6928, -5528, -3028, 4818, -2434, -7998, 885, 2625, -3897, -8431, 2693, -7715, 7514, -5835, -5397, -8418, 3051, -4881, 3976, -815, -8120, 961, 9128, 3096, 9375, -2914, 6110, 3888, 4569, -3105, 2614, -2416, -9404, 4732, 2731, 6488, 6256, -2577, -9207, -14, -7590, -6187, -9900, 9924, 2806, 381, 558, -8671, 5021, 7609, -4535, 3312, -2006, -4691, 3502, 2204, 6504, -9823, -5872, -6483, 9739, -7574, -9486, -2973, 7009, -7179, 248, 9864, -192, -9697, -8949, 3890, -1013, -6217, -3872, -1423, -6287, -4825, -639, 4102, -3142, -5151, -7026, 7201, 6423, -6519, 2848, -1620, -7785, 5696, -3800, -1199, 3652, 9496, 4227, -2362, 9347, -5465, 2890, -9352, 254, -5227, -2306, -7111, 9715, -6398, -4457, 4513, -3361, 8994, -7742, -2914, -1250, -5743, 2919, 6707, 2245, 4736, -5159, 2213, -3980, -1581, 8716, -9942, -8152, -8650, -5100, 9966, -8804, -1491, 9365, -572, 2010, -9023, -3910, -5422, -4687, 1402, -389, 239, -1657, 650, -9839, -4402, 4768, -4337, 6892, 5101, -1892, 9659, 9603, 4185, -9442, -8121, 3479, 8843, 2236, 8705, -8039, 4894, 4107, 1406, -7981, 5833, -3962, 1301, 3550, 2309, 9361, 3206, -8246, 6664, 1704, 9734, -9514, -1727, 3948, 5410, 2432, -2669, -929, -1114, 426, 7990, -5730, 5589, -1726, 49, 6865, 5752, -4747, 7266, -5233, -4360, -6538, -2242, -5106, -1353, -1087, 7657, -7004, 6899, 6996, 9512, 9971, 8697, 4665, -6695, -3625, 8932, -5600, 9853, -9508, 6193, -339, -3597, -900, -6245, 3058, 3004, 2721, -2106, -3335, 6164, 7723, -2119, -4247, -6494, -3062, 153, -2827, -5930, -364, -2165, -1839, 8873, 4529, -8097, -8083, 8539, -5761, -4303, 6494, 5000, -8273, 6838, -3150, 2651, 704, -126, -5906, -8384, 273, 2566, -9806, 6895, 2655, -4547, -5438, -2109, 4237, 6016, -8986, 5722, 4449, -9823, 3416, -696, -8651, 9204, -3551, -8738, 9198, 2265, -1377, -7095, 1339, -2164, -2797, -3229, -2490, -45, 674, 6788, -7890, 4900, 6259, -1697, -1491, -3039, 4822, 6892, -1553, -7511, 2706, -8009, 9489, 8002, 645, 3044, 6707, -1836, 592, -1808, -6790, 8783, -8759, -259, 5537, 3427, 7699, -4521, 2829, 5231, -311, -394, -4420, 526, 140, -9137, 6670, 6122, 3659, -6316, 7817, -9056, 4451, 8257, 7994, 8022, -4326, 4418, 1443, 6566, -2799, 6809, -9154, 4705, 202, -9703, -8425, -1770, -2201, 3295, 762, 5439, 4446, 5532, -7591, 6349, -1270, -4361, 4196, -9694, -7782, -3328, -1680, 678, 8643, 2142, -4853, 8884, 4710, -6448, 6163, 8761, 6852, -2593, 9126, 3850, 5286, -5088, 7791, 1252, -4663, 795, -5454, 4690, 3508, -7073, 9436, 492, -3746, 2568, -1153, 2600, 6325, 7993, 406, 8792, -6728, 6219, -2945, -1398, 85, -2481, -8253, -6189, 7350, -8725, 1950, 1988, 3495, -905, -4708, -7361, -4656, 6417, 6830, 7654, -2359, 7783, 6378, -3427, -1651, -8707, 6975, -5380, -344, 7912, 7156, -5341, -9230, 2593, -5014, 9843, 8418, 6320, -2171, -1887, 6052, -3720, 3213, 7610, 2358, -4388, -2075, -8447, -3513, -3233, 152, 3639, -2796, 5478, 227, 8993, 6054, -5559, -3553, 4233, 7510, -2879, 7997, -7466, 9603, 132, 8121, -7502, 673, 8483, -5341, -4578, 8959, 7531, 2376, -4819, 6060, -9398, -5953, -8325, -6082, 4393, 7220, -7047, 7290, 1046, -1444, 7892, -4558, 613, -5092, 3569, -3331, -3833, 7432, -8609, 4835, 5246, -3945, -2096, -80, -8522, 5840, 3071, 3378, 6297, -6900, 6468, -6458, -8040, -5006, 6721, 6273, -8596, -4272, -1444, 2869, -7250, 6659, -3121, -1652, -8598, -1549, -8305, 6145, -1000, 8500, -1549, 8069, 4912, 8955, 3929, 2613, -478, 3293, -1986, 4800, 3118, 8641, 2771, -9533, 9721, 2446, 5213, 1467, 263, -201, 3550, -1804, 1778, 8764, 9619, 5386, 2995, -2672, -3889, -5182, 9958, -1245, 6309, -9131, 1864, -2319, 1138, 8986, -5554, 7921, -3692, 5762, 8936, -344, 2559, 7678, -1285, 1828, -9619, 6726, -5984, 5847, -6247, -6021, -853, -3098, -7783, 7950, -3460, -4261, 5513, 8551, 158, -5963, 6888, -3932, -6385, 5202, -7700, -188, -2534, 2572, -5174, 4963, 2358, 3213, 354, -211, 9637, 4805, -8496, 5946, 413, -2287, -4469, -25, 5519, 9029, -9993, -6657, 8214, -8453, 4483, 3191, -9995, -7313, -9464, 7378, -7879, -8550, 2824, 3966, 7009, -7741, -9778, 9903, -1594, -8064, 2389, 1269, 7059, -9386, 6218, -4576, 8161, -3962, -4534, 6735, 9288, 9448, 7073, 2224, 1023, 7032, 9464, 333, 8180, -1295, -5021, 3685, -1073, 2543, 7760, 8065, 8811, -3432, -8130, -8788, -5558, 4982, 5122, -7512, -8030, 6405, -5812, -112, -9967, -8091, 1572, 2136, 1984, -7398, -3636, 9861, -274, -6881, 7452, -3769, 1520, -835, -2014, 854, 6104, -8527, 512, -8667, -3522, -3511, -3593, -6798, -5400, 2761, -3052, -2071, -8074, -4359, 2314, -581, -5344, 7958, -354, 9669, 9881, 3834, -5068, 3022, -4346, 9143, 5857, 7018, 5455, 1090, -2120, -957, 1152, -1750, 9257, 5776, -6879, 1126, -1859, -4423, 2792, -2023, -9969, -6993, -3968, 6235, 7291, -7819, -8102, 7709, 6900, 3934, 4857, 4319, 8871, -7410, 1505, 9789, -3877, -8665, -4523, -2971, -738, -6576, -7450, -7375, 819, -916, -8755, 7756, -6473, 7204, 2561, 7218, -4811, -7808, -7700, 1315, 5588, -6028, 2700, 4517, 8739, -7891, -6486, 511, 2376, -8813, 4306, 8752, -3351, 4023, -1004, -914, -817, 623, -7596, -3309, 8947, -2387, 9859, 2031, 6867, 840, 9628, 1285, -770, 2096, 6552, -2658, -967, -279, 6477, -4317, 7861, -7969, 8030, 2075, 6828, 8845, 8758, 5326, 7309, -9750, 9307, -2750, 1454, 4711, 9690, -9890, -5059, 629, -3862, -2368, -6499, -8397, -495, -4200, 6593, -8353, 2798, -6615, 6097, -8639, -2730, 6720, -2062, -7706, -8753, 5624, -8324, 7255, 9150, -1571, 8153, 825, -7300, 9314, -5071, 9742, -1162, -3528, -296, 7618, -2486, -3364, -4664, 5463, 1997, 3363, 7535, -2107, 3303, 1640, 906, -1334, -6600, -6204, -4095, -6021, 4450, 3844, 5255, -8513, -1516, 6009, -7026, -4759, 676, 6383, 9659, -3691, 8444, -5209, 5092, 804, -9997, 2913, 1166, -3355, 4006, -8133, -9594, -890, -3947, 5523, 2921, 8936, 2818, 9713, -7313, -3903, 7542, 4543, 7120, -3185, -9428, -6913, 4027, 9924, 5804, 2194, 1759, -3471, 3420, -7595, -6305, 8943, -5692, 1933, 7813, 369, 3202, 9167, 6008, 1186, 4540, 1462, -4043, 7451, 6961, 1695, -9861, -6993, 1764, 2774, 2033, -7799, 855, -262, 9163, -302, -7482, 6178, 6716, -7735, 4859, 8471, -4369, 7869, 5937, -3545, 5571, -8687, 7835, -7954, -1826, 3507, 3833, -9685, -2860, 1305, -6270, 4164, -1379, -2868, 9318, 4522, 1804, -7471, 4981, -9917, 8758, 8365, 4719, -8114, -4638, 7190, -9447, 1957, 7036, -3771, -6696, -4981, 1327, 7564, -7074, 4409, -2819, -6323, -2931, 731, -5226, 3293, -1765, -23, -5512, 6319, 9218, 5734, 6964, -2600, -3700, -1849, 555, -8878, 379, -3554, -4001, -1075, 3839, 4762, -7491, -6299, 8571, -272, 4413, -5146, -4825, -8476, 1463, -2162, -630, 3970, -3693, 6483, -2370, -7722, 6200, 9829, 8352, -6175, -1106, 7200, -8397, -6816, 9935, -6501, 6501, -4414, 6219, 5699, 8712, -8695, 5948, -4910, 8474, 1063, -2884, -1235, 2809, 6174, -2468, 5075, -2039, 7433, 4882, 7066, -1709, -4418, 5672, 1232, -1831, -766, 7244, 187, 520, -797, -5516, -3743, -8851, -1794, -625, -2114, -5496, -2700, 3960, -6384, 4462, -7534, 7570, 6799, 8618, 7358, -3466, 7680, 7941, -5256, 5668, -2103, -9474, 4573, -6953, 4600, 7753, 7514, 9497, 6505, -7564, 769, -520, 3301, -2693, 2009, -5336, 9530, -4569, 48, 4270, -1923, -2875, 7798, -7965, -2805, -3931, 2747, 7762, -9870, -700, -8799, -8535, -7081, 6811, 4416, -1970, -1024, -1351, 7601, -4261, -2865, -2622, -7692, -2036, 38, 7176, -4122, -8353, -7290, 4095, -6544, -4581, 2145, 1363, 2948, -2463, 9382, -9277, 242, -3174, -4510, -357, -3336, -3003, -5090, -26, 3766, -3342, 1133, -7138, 6653, 7619, 6764, -369, -9765, 8663, -9939, 6549, -3660, -99, 8494, -9207, 7272, 7829, -6697, 6704, 6705, 5227, -6860, -4345, -8298, 5484, -5007, 1257, 5137, -681, -9650, -9792, -4295, -5031, 2924, 6033, -5006, 4053, 7808, -6326, 6739, 4640, -1555, -3214, 546, -2185, 110, 2618, 3818, -616, 6095, 7377, 460, -8744, -3778, -6060, -3360, -9535, -3766, -4708, -4586, -7039, 4040, 2505, 9753, 5530, 3538, -4480, 1372, 1676, -1447, -4491, -1843, -217, -7181, 6028, -6606, 6247, 7100, 356, -4868, 5876, -197, -3169, 2741, -8698, 7722, -7238, -9534, -4335, 223, 8381, -3640, 6267, 2953, 9329, -471, -8270, -4797, 8818, -5248, 3995, -2262, -2870, -2542, 560, -6732, 6610, 4602, -7914, 2527, 732, 1803, 5857, -1133, -5798, 537, 608, -9289, -7, -3388, -1862, -6167, 5664, -7035, 2759, 6461, 2940, -1514, 6117, -3145, -7540, -5225, 8982, -2929, 5046, 690, 5768, -2387, -2829, 9774, -3894, 427, 8373, 7207, -9879, -9247, -7879, 3406, 5684, -2214, 5685, -1737, -6431, -7933, -4678, -8627, 5890, 807, -1516, -9561, 233, -5716, -8796, -7582, 2586, 7142, 8819, -5817, 536, 4169, -1893, 2117, -8779, 4671, -4072, -2711, 8787, 6110, -1156, -3262, 8163, 130, 8195, -1688, 5839, -7230, -9260, -5952, -4499, -2554, 4967, -2800, 528, 4755, -8712, -9235, 5947, 3664, -8078, -4207, -916, -2491, -9348, 5987, -5284, 8211, -7444, -3132, -8660, 8226, -7851, 2508, -7200, 7245, 5916, 6644, 7703, 2512, -9542, -6673, 4470, 5979, 4829, -8416, -2968, 9439, 1446, -9417, -97, 2279, -4458, -1595, -2298, 2480, -3262, 2497, -2272, 8872, 2330, -6478, 9264, 1065, -3653, -4303, 1530, -1951, 5647, -607, 1837, 4562, 9749, 2908, 1514, -81, 8584, -8035, 327, 2492, 1190, 9517, -4925, -3185, -376, 3111, 6052, 5773, -4291, 1149, 6672, 405, -3532, 3109, 8065, -5863, -4951, 2103, 6775, 9194, 4761, 6300, 3507, -4014, -3838, -7499, 1618, -8356, -6438, -3903, 6823, 210, -5431, 2693, -7459, 7469, 557, 4009, -6228, -7178, -2458, -8042, -7264, -9639, 6632, 6539, -1824, 9921, 1383, -8345, 6497, 6395, 1729, -2208, 7583, -8308, -2766, -2916, 9343, -5816, -5435, -5614, 195, -147, 8976, 7748, -5365, 999, 7347, 3374, 1442, 8779, 3117, -9858, -8860, -4670, -8231, -960, -5000, -2907, 1746, 973, -3441, -2257, 9087, 3142, -9996, 9014, 1411, 277, -7656, 98, -8148, -5501, -257, -2627, 9301, 7117, 4416, -1447, 6672, -8833, -9125, -7578, -8294, -6470, 9617, 608, 6930, -8051, -9776, -1866, 8186, -7560, 7563, 5323, 3323, 5297, 770, 7286, 2572, 7370, 9441, -1690, -8428, 7828, -28, -6938, -3754, -3471, -8318, -7969, 2596, -1612, -9469, 4123, -4813, -103, 2515, 9640, 7280, 5840, 9672, -9858, -9835, 8423, 6682, -8764, -6434, 2556, 6808, -8001, 3544, -2038, 9159, 8598, 5859, -9621, 4623, -6047, -8026, -7188, 5019, -8884, -4455, 6979, -4487, -6185, 4622, 140, 1786, 6365, 5208, -420, -2609, -3267, -4251, 7153, 4149, -8747, -7792, 6548, -4037, -4045, 4320, 3023, 3628, 5032, -458, -9139, -6302, -4585, 2061, -1345, 5126, 5906, 8953, 5225, -3004, -2178, 3907, 8804, -2843, 3627, -6024, 1885, 4941, 5009, -7895, -203, -4796, -5099, 1521, 348, -4546, 6626, -6529, -8478, -3679, -8626, 9247, -9034, -165, -5359, -108, 5805, 6934, 4, -3252, -5564, 7438, 6303, -6894, 6610, 4976, -2049, -5310, -1732, 2045, 2329, -7627, -2012, -850, 6036, -1671, 7038, -6021, 5543, -7054, -214, -2924, 2836, -1688, 1777, 261, -4184, -5446, -2369, -3392, 5988, 4317, 9911, -6352, 7574, -3360, 996, -3412, -1417, -470, -5956, -5302, -6976, 4385, -4277, 3281, -3566, 911, 923, 9764, 2675, -5409, -9586, 2200, 9774, 2898, 6662, -5168, -3418, -6107, -1426, 3239, -3656, -6257, -3721, -765, -9543, -3805, -1408, 2325, 356, -5328, 2046, -2922, -5500, -8888, -4665, -3897, -1430, -9735, 4209, 7632, 1445, 7771, -2482, 3036, -9906, -7341, 956, -6712, -2599, -7732, 4119, -4725, -8345, -3927, -5452, -2685, -2263, -5391, -9976, -5678, -559, 7382, -2027, -959, 4525, 305, 7712, 5023, 5986, 4586, -6529, -1412, 5473, -2718, -9212, -1907, 2434, -1986, 7164, 1864, 7960, -6454, 571, 8087, 6604, -4561, -9736, 9069, 9493, -9773, 4169, -896, 3864, -6390, -597, -4187, 1028, 8727, -483, 1931, -4191, -8093, -6998, 7859, 753, -2514, -7245, 9388, -8296, -6701, 2859, 3297, 5719, -8854, 1822, -6450, 4772, -859, 8107, -1722, 8036, -7880, -1595, 8194, -6659, 6674, -399, -369, -4159, -1443, 5290, -3496, 5311, -9366, -5466, -2637, -1953, -7071, 5888, 567, 7721, 4255, 5149, -7748, 2058, 5435, 9208, 3865, 2190, -9614, -4738, 5176, -2825, -7536, 8153, 6936, 8555, 4628, -1064, 1597, 2971, 1888, -6333, 3312, -4043, 2583, -1558, -6883, -5759, 23, 2815, 9706, -7451, -6099, -4628, 7377, 4688, 1840, -158, 7808, -9541, 7597, -173, 2989, -5840, 1948, -8553, -1277, -4225, -3473, -5304, -2498, -3620, -8017, -5970, 311, 4716, -3982, -4611, 5817, -9882, 4300, -6083, 1310, 6845, 5273, 6763, 2312, 2239, -8282, -3525, -9265, 4183, 6275, 9897, 902, 1433, 294, 9812, 2816, 9012, -492, -7624, -4545, 1467, 5173, 5357, 6286, -1178, 1875, -1537, 2238, 7515, 31, -700, 1434, 4442, 2820, -3600, -2517, -9151, -3956, -6229, -9558, 1291, 6189, -5211, 4427, 712, 5602, 1583, 6975, 3369, 1917, 8530, 3820, -2459, -5813, -3758, -5457, -5808, 4829, 5816, -9442, -6728, 8186, 7883, -2012, -6564, -6644, 59, -9261, -1177, 5324, 6587, 3542, 7919, -1714, -7005, 9607, 8882, 3272, -1684, 9122, -8009, 3359, 3131, 9750, -9843, 6698, -9514, -2128, 7668, -9209, 9724, -373, 3479, -4750, 8190, -7247, 3695, -152, 1407, -4602, -5322, -5835, -6108, 6592, 6016, -512, -9615, -3503, -9733, 9795, 1859, 5758, -5953, -2048, -3161, -4277, -4744, -1671, 2429, -9789, 7575, -7575, -3145, -7297, 8934, -241, 4160, 7508, -6332, 5224, 6120, 4983, -9731, 9137, 3885, 3329, 1430, 1406, 349, 8852, 5333, 2497, -8790, -7579, 9542, 4175, 6417, 8448, 4785, -4975, 1402, -228, -6578, 8900, -3484, -9168, -1482, -1617, -6229, 8881, 1358, 5400, -2989, 8946, -1301, -9005, -6096, 8731, 2002, 8001, -7445, -4392, -5927, -8660, -1925, -6482, -1334, -1209, 3343, -6806, 3103, -2168, -7372, 3863, -4492, 7300, -5927, -5569, 2020, -6782, -3386, -8636, -382, 7198, 8036, -1002, 8139, -7160, -6562, -6756, 3444, 5709, 3299, 1918, 8463, 2021, 5591, 9375, -2867, 5955, 1792, 417, 2943, -5479, -5402, 7967, 8091, 5172, 3201, -79, -8862, 8206, 4405, 8244, -3097, -3582, 5652, 9928, 2284, -9881, -3345, 4448, 5247, 5124, 8504, -3242, 4976, 9585, 9098, 228, -2046, 3354, -2337, -4261, 4223, -22, -6355, -658, 8122, 7312, 4617, -7995, 4486, -9720, -3610, 6829, 9030, 5419, -7990, 834, 7464, -7328, -1832, 9473, -5169, -7530, 839, -8762, -6613, -1248, -828, 7339, 5494, -5345, 6335, 8311, 8371, -9547, -1682, -4420, -8010, 711, 7115, -8182, 8968, 7054, 7605, -9142, -2284, 1541, -6750, 5374, -7146, -4729, -7937, -7202, 2677, 2717, -2044, 3097, 7342, 8626, 6026, 2271, 8848, 9300, -3383, 7794, -7764, -5324, -5851, -3765, 1774, 3072, -9451, -2762, 3493, 507, -7900, 8480, 1815, 4111, 775, -5492, 1359, 5245, 3485, 396, 7529, -6922, -5606, 4813, 4615, -8746, 895, 4045, 4338, 9840, 3379, 1076, -2200, -1054, 1215, 2486, 801, 1333, 4013, -3461, 2180, 6756, -1607, 389, -3951, 5374, 3871, 9418, -1684, -8137, -7593, 4445, 7758, 6740, -8559, 5617, 3558, -8296, -5292, -9805, -7647, -7819, 1036, -5398, -3402, -1491, 1287, -3371, 3724, 2191, -3011, -108, 7956, 5103, -3067, 2267, 1318, 7327, 6902, -1554, -6422, 2503, -7621, 517, 1965, -1276, 6895, 2712, -8940, 2516, 5180, 3371, 6203, 5221, -6888, -8449, 1558, -8206, -3230, 8100, 3992, 8317, 1704, -8419, 5052, 4024, -966, 3536, -1496, -8863, 6681, 8522, -234, 9205, 5228, -4285, -7860, 5654, -6150, 7844, 3442, 8846, 2229, 8108, 83, 75, -7870, 6564, -5182, 372, -2503, 110, -1836, 2977, 4992, 8788, 9311, 3005, 2503, -1326, -3086, -3073, -2618, -8994, 3284, -2642, -7828, -3201, 2872, -6053, -110, 7880, -5062, -8332, -6704, 9033, -2240, 6616, 1202, -9499, 1955, -9544, -7109, 4669, 5209, 1461, 8282, -114, -5363, 1063, 4273, -5541, 7064, 1776, 4042, -1035, -315, -559, -8422, 7479, -1973, 2920, 1382, 1711, -700, 2570, -3858, -8072, -5999, 2020, -1481, 3718, -181, -2765, -5653, 9546, 1721, -2592, -9709, -2177, 6250, 9925, 9099, -3936, -8198, 4115, -7694, 515, -6764, -5983, 1171, -699, -8227, 5262, -7564, 8254, -7383, 7068, -3262, 5573, 7649, 1838, 5972, -2349, 6552, -4456, -297, -7448, -8241, 9817, 4547, 3267, 8138, 2210, 6057, -6488, 6038, 5272, 6750, -6750, 7, 6870, 4861, 4339, 7794, 2857, 2880, 5943, 2284, -5968, 4477, -3541, 2508, 4043, 5277, 1765, 3769, 5321, 5352, 2624, 1656, -2782, 7828, -8020, -9885, -206, 2112, -4621, -6633, -6154, -3391, 6802, -5522, 6482, 7366, -982, 4915, -2321, -8911, -2488, 5021, -6971, 304, -3193, -8346, -5205, 8184, -4684, -8610, 9392, 9431, 933, 7194, -6730, 4914, 1753, -9581, 5751, 7816, -488, 5043, -8962, -1587, 9753, 7578, -1425, -4741, -9497, 2519, 7681, 1404, 4022, -355, 5016, 4045, 6884, 900, -8773, -1840, -8556, -1149, -6656, -6403, -5755, -3393, -5418, -3004, -6679, -5169, 6325, -2067, 1945, -5173, 4298, 1694, -7234, 3322, 6138, 8641, -1329, -6577, -2570, -7656, 1231, -7015, -1003, 9580, -2308, 1527, -1080, -2089, -2565, -5575, -7488, 1318, 9304, -6745, -8532, 6151, 6482, -8788, -3996, -2033, 9451, -4636, 5455, 8725, -2490, 7463, 5350, -4956, 3567, 1272, 8753, 681, 9732, -1938, 4515, -99, 2514, 7672, -9153, 9993, 1662, -5789, 2647, -1240, 815, 6570, -6051, -2950, -8631, 6150, -9123, -9810, 641, 2017, 2913, -9928, -948, 6639, -6611, -4618, -943, -9456, -7924, -151, 7126, 2045, 7742, 407, -7421, -4798, -2131, -38, 7001, 2433, 784, 3947, 1412, 2183, -9103, -357, -1659, -2769, -7152, -1831, 6149, 5973, 2957, -7033, 2466, 565, 8098, -3628, -2831, 1935, -7466, -1001, 2866, 6994, -165, -5990, -3723, -2826, 8825, -1647, -3236, 1619, -5260, 5208, 4447, -5915, 2395, 87, 5852, 6627, 8662, 5526, 8146, -2525, -7794, -8898, 2067, 8822, 7023, -3382, -5614, 4337, -1520, -4807, -9167, 2013, -9319, 6942, -594, -9909, -8453, -2856, -569, -1995, 1656, -3157, 521, 9741, 1412, 333, -8101, 1338, 8651, 6302, 6431, -52, 5661, 7345, 2721, -8326, 724, -162, 5041, 5480, -3098, -8259, 1491, 7446, -949, 5078, 1199, -2991, -7260, 1958, 9395, -3668, -4738, 5460, 4312, 1179, 8174, 3373, 6688, 3114, 7728, -8218, -370, -5042, 7940, -6879, -1783, 9366, 3317, 4001, 7807, 8555, -1700, -7449, 4213, 2578, 8242, -9130, 8900, 4412, 9707, -8836, -9424, -9453, -8185, 8917, 5495, 3253, -215, -1288, 9149, -1145, -8590, 1717, 7643, 5155, -1728, 999, 6576, 5876, 313, 3474, 2533, -5319, 5892, -6967, -4529, -5467, 5339, 269, -3997, 5271, -3824, -1595, -3781, 9923, 9195, -7894, 1822, 377, -8226, -7395, -2157, 2733, 6842, -7557, -9347, -3070, 278, 8433, -9067, -158, -6652, 2569, 7407, 9391, -4680, -3630, 2694, -46, -2006, -5068, 1832, -3210, -3701, 3291, -8790, -1244, 3246, 7391, -7720, -9036, -8943, 2593, 17, 9259, -4609, -1436, 6277, -9191, 9286, -8311, 5790, 9904, 1600, -1203, -9208, 8042, -1374, 7788, -8719, 2333, 8048, -2520, -1123, 1878, -4324, -3161, -604, -2638, 3781, -1428, -9116, -7472, -1634, -9910, 7759, -4705, -6334, 2138, -5964, -9885, 4887, -9346, 454, -6657, -782, 6765, -1598, 4154, 1224, -5599, 1025, 9478, 3118, 8138, 9049, -6835, -8269, 6985, -6121, -9023, 8690, -4761, 7209, 6573, 4562, 1091, 817, -5580, -4416, 8726, -3019, -5061, -4788, -5800, 2978, 9363, -5017, -6207, -9344, -8637, -6039, -6807, 6280, 5725, 225, 7641, 1421, -4718, -5873, 9699, 3794, -5007, 8599, 8950, 9001, 1283, 6902, -6067, 2272, -5123, 151, -5319, 3109, -5814, -5293, -5560, 95, 9276, 1544, -2734, -7100, 745, -6924, -8596, -4295, -9336, 2027, 4371, -6149, 3723, 8278, -9280, -5047, -9363, -5750, 1158, -1929, -9966, 6336, 1217, 6913, 2303, 2624, 8524, -2802, -770, -1705, 8545, 3755, 5112, 7912, 829, -3053, -1593, -7404, 9542, -7974, -9477, -3586, -8045, -673, -7809, 4086, 74, -6639, 4025, -8729, 5538, -2180, 657, -9353, -38, -8639, 5670, -6239, -3054, -8981, -4665, -8136, -1924, -9834, -8620, 5155, 2292, 6450, 2519, 7969, 7417, -5649, 2812, 6107, 8142, 1783, -6187, -9109, -1092, -9258, 1457, -9051, 8575, -8231, 8673, -13, -2390, 6165, 7205, -1086, 5741, -4730, 5007, 2971, -6163, -5516, -6766, 8473, -1492, 4091, -6899, -8135, 622, -5489, 7021, 2855, 7828, 7875, -9134, 8188, 8123, -3887, -7700, -4862, 872, 5173, -9682, -5187, 9154, 8728, 9266, -5267, 876, 2860, -1974, 4750, 7646, -8048, -4975, -6985, 9041, 604, 4001, -6627, 9105, -1261, 9232, -2789, -5823, 8486, 3800, -6404, 415, -3491, -1382, -477, 2271, -729, -8685, 4419, -5740, -7930, 110, 8353, -8086, 6593, -4802, -673, -5990, 7459, -4515, 6465, -7156, -8185, 3659, -9098, -4481, -1544, -7039, -1493, 7695, -9048, 3351, 71, 1221, -5367, -4926, 6262, 5175, 7565, 3844, -6940, 706, 4534, -2618, 7943, 6521, -3666, 1192, -5174, -8688, 3300, -4233, -299, 1252, -9910, -4125, -5829, -9897, 5118, 2900, -2630, 5493, 3497, 3578, -8309, 6680, -5674, -6854, -3406, 5063, 5216, 3038, -1005, -5054, 6309, 3644, 5929, 3633, -5051, 9828, -4934, -3821, -6261, -2937, 2691, 5541, 777, -3718, 544, 3226, -4894, 2879, 9781, 12, 5967, -2148, 8129, -313, 4652, -2894, 7680, 8509, 6399, -3046, -6337, 2187, 1691, 974, 7107, -6603, -7449, 9244, 6136, -6327, 5597, -1441, 1970, -6201, -7954, -3841, -2638, -20, -6797, 8264, 8262, 4129, -4493, 4511, 6083, -5059, 5093, 6437, 6284, 7215, 1530, -5036, -3832, 5106, 5722, 4140, 3273, 2740, -1085, 9741, 3015, 7688, 3333, 4237, 9958, 9089, 8419, 2802, -2294, 4422, -821, -7634, 984, -1483, 8840, -7165, -1501, 3100, -2766, -190, -3533, 3408, -412, 1793, 5113, -1261, -1272, -9013, 6022, 9887, 2607, 7346, 891, 2791, 7986, 3484, -2970, -6567, -8113, 6664, 2706, 4932, -6010, 3103, 2842, 4781, -7981, 5151, 7931, 9113, 1007, -6399, 867, 1481, 7960, 8305, 2755, 445, 7152, -9581, 1101, 2680, -2930, 5521, -3436, -902, 5001, 7706, -9626, -287, -2841, 3763, -9190, 1819, -8552, 1857, -5129, 3725, -2136, 3291, 6921, 1584, -5511, 6674, -1676, -7394, 7806, 2593, 6248, 6910, 8318, -5943, 7254, 3709, -1682, 9982, -4036, -4057, -8767, 3564, -3949, -4794, 140, -730, 4745, 3642, 1846, 9070, 6467, -4988, -5810, -9965, 4321, 6834, -5108, -8753, 5642, -8713, -704, 9285, -4127, -4962, 5767, -3341, 6260, -6223, 7330, 8833, 4195, -5561, -4617, 3984, 393, -1342, -7884, 3491, 4104, 9516, -9925, 9758, -7425, 4922, 4181, -7006, -847, 5556, -9770, -2788, 2658, 2211, 8689, 3480, 7552, -866, -4914, 1336, 2442, 7566, 8942, 1835, 6719, -1271, 7498, 5638, -3693, 5317, -5528, -1178, 8559, 2407, 4114, -6971, -1171, 2497, -6909, -6821, -850, -36, -9129, 2190, -9085, -4700, -932, 489, 4943, -5410, 5090, -5053, 6540, 4716, -6400, -502, 7585, -243, 2931, -4267, 6131, -2657, 6137, -9660, 1192, -7905, -7008, 3794, -2798, -4209, 5390, -1726, 363, -5549, 9782, 4969, 4160, 4180, -9970, -3487, 6563, 2099, -9185, 9513, -7221, 5339, -4634, 7745, -103, -4359, 3525, -9216, -8689, 5324, 2050, 8673, 767, 8305, -4866, 1787, 5573, -924, 2041, 3403, 5090, -1136, 600, -8979, 7999, -1795, -4390, 2042, 1956, 3885, -2263, 3517, 5698, -6406, -3604, -1432, 4485, 6079, 8174, 6513, -5377, -4051, -167, -7110, 5616, -1848, -8304, 1350, 1338, -3801, 7752, 7543, 5369, 2862, -451, -81, -7594, -9869, 5742, -1067, -5345, -5364, -8172, -9678, -9362, -118, -356, -7703, 2878, 5436, 3285, -383, -3311, 2370, -5359, -4821, -2901, -6317, 4208, 9852, 2042, 5969, -2726, 1312, -5929, 2717, 7562, -8301, -1757, -5381, -6491, 8236, 1840, 5916, -6724, -1518, -4190, 1142, 9572, -2176, -4142, -2816, 1, 1814, 1030, -3113, -9605, 4127, -2415, -2194, 423, -3704, 5117, 2779, 7845, 3733, -4441, -9287, 5785, -1505, 2351, -24, 2964, -45, -2568, 6649, 5233, -3935, 6173, -8776, -2508, -3272, 2134, 5508, 9459, -3524, -7096, 2610, -4282, -9841, -4099, -8632, -9788, 7977, -6718, -9568, 4346, -8408, -2648, -6777, 5568, 6954, 2707, 5853, 1780, -41, 5039, 8057, -1955, -2894, -4935, -5444, 2641, 8532, -871, 4969, -816, -3410, -8909, 8263, 6175, -1048, 5587, -9887, -4613, -1037, 4176, 8842, 564, -1877, 4064, 1025, 7624, 5777, -30, -6899, -1195, -1013, -1199, -7633, 8785, 4269, 5108, 7118, 708, 7869, -4528, -7143, 196, -8136, 4605, 5412, 9288, 2015, -3777, -7910, -6454, 6097, -1270, -2532, 6018, 7297, -8616, 2218, 7933, 9549, 6579, -5237, 6028, 1138, -608, -1412, -7258, 7534, 3650, 3104, -6378, 9169, 3332, 7892, -832, -8940, 164, -2214, -5746, 5556, 2994, 3214, -1638, -436, 4602, -9949, 8543, -6281, 8920, 7174, 4039, 3101, 9466, -5373, -6482, 7106, 5758, -3251, -247, 3162, 8920, -7010, -9190, 3038, 8422, 172, -8643, -8218, -4701, -2274, 1218, -6019, -2784, -4478, 9903, 8584, 6821, 5406, 2588, -9147, 2030, -3765, -540, -1615, -6256, 6428, 3655, 9492, 827, 8178, 4288, 3049, 6159, -5662, -7506, -6638, 3440, -3275, -9969, 4379, -3486, -5152, -9203, -7218, 2480, -6294, 7130, -10, -3729, -1178, 2121, -9449, 3011, 9066, 9139, 9363, -6021, 1613, -2167, 755, 6429, 1969, -366, 4698, 2795, -6217, 7944, 6327, -1942, 9470, 2713, -4570, 9888, 5730, 4131, -2943, 5915, 5195, -1302, -7951, -8162, -8069, -9753, 66, -6491, -1566, 6087, -8299, 8573, 8434, -8922, 825, -9935, -8154, -7699, 9186, -6762, -439, 9072, 6799, -4650, 1085, -1101, 7381, 4720, 6145, -6849, -296, 9556, 8603, -7736, -896, 6613, 6502, 5430, -8931, 4888, -7485, -3361, -886, -1880, -3055, -3685, 5853, -619, 1474, -7429, 8998, 6045, -7746, 5363, 9943, 5735, -646, -5167, 2945, -6402, -469, -6446, 57, 2777, 269, -8118, -2446, 5129, -2079, -6989, 1544, 6258, 3549, 8101, -4964, -3254, -2186, 8288, 4134, -7894, -297, -1220, 2063, 8709, 3785, -2404, 6020, 2672, 6082, -7941, 4457, 1520, 5100, 1396, 2942, -8613, 8140, -8264, -5422, 602, 7372, 2161, -5432, 2553, -7760, -5894, -3040, -5599, -2799, 7827, 9473, 5777, 4436, -714, -9760, -3943, -202, 9892, 1686, -5045, -7752, 9109, -7665, 186, -1656, 9742, 6529, -7114, 8499, -7786, -4564, -3460, 4490, -1015, -5551, 807, 5555, -8481, -5058, 4069, -6305, 4935, -5266, -5216, -4923, -2581, -7968, -514, -9183, -1388, -2028, -946, -6975, 110, -1954, -2201, -2260, -4640, -6233, -1075, 3618, -6167, 5570, -391, 2128, 9386, -5515, -4539, 5646, -6699, -3889, 4025, 6793, -2877, -2409, -5770, 5496, 2730, -8491, 6043, 6945, 9126, 3458, 9592, 2764, -367, 2275, -6568, 6509, -9877, -4656, -3655, 9655, -5546, 8194, -3400, -8020, 9459, -3588, -2127, -6745, 2898, 5806, -6483, -5033, -5040, 9469, 2024, 9871, 1970, 7324, -6550, -8588, -6042, 3397, 6166, 3753, 290, 149, -867, 161, -9094, -1803, -4860, 4898, 9900, -1468, 4133, 7251, 8971, -3689, 7176, 5999, -6820, 2253, -8966, 622, -5017, 1966, 3015, -8285, 2437, 1189, -4211, 5057, -9146, -2912, 7390, -6348, -3248, 9068, 7144, -8876, 902, 8656, -3609, -723, 3085, -6625, -6225, -4267, 603, 7099, 3457, 2413, -2045, -1221, -9484, -2196, -7827, 9110, -1151, 6303, -9225, -5863, -954, -8728, -8307, -3161, -2158, -2119, 1896, 2049, 3056, -2789, -3092, -8451, 9842, -5417, 5540, -1489, -5040, -3931, -6151, 2574, -1026, -5524, -5690, 3809, 4804, 9050, 7825, 820, -5185, 7302, -7185, 5611, -5057, -1064, -5630, 7503, 8704, 4122, -6916, -9136, 1955, 3700, 7706, -9006, 4169, 9006, -655, 6317, 5261, 8825, 7028, 2994, -2162, -1409, 8225, 6198, 8756, -9826, 3674, 7268, -6183, 2796, 266, 4207, -97, -8693, 3526, 9512, -5995, 358, -7557, -3195, -8, 9491, 7226, -3239, -1509, 9607, 3566, -2599, -3409, 7483, -8464, -6925, -3643, -5777, -3463, -9598, -3592, -2603, -8299, -1320, 4853, -4678, -7574, -1714, 7868, 4101, 37, 8114, -7221, 6473, 2189, 6750, 8134, -8464, -9230, -9563, 4944, -5305, -6986, 4194, -4155, -8791, 8973, 7108, -2622, 766, 5769, 9756, -5298, -6547, -6544, -8930, -4916, -8991, -3167, 1448, -9895, -3471, 4188, 5107, -5747, 8277, -9324, -3764, -7329, -283, -7632, -575, -5226, 276, 2663, -3341, -5661, -5096, 7443, -1620, 91, -1144, 6078, -2984, -5730, 1284, 2120, 4923, 8144, 6637, 5510, -7515, 7662, -6505, -7751, 9640, 7383, -2274, 1415, 3966, -227, -3863, 5534, 5463, -9330, 4221, -8454, 6250, -3533, 5398, -9655, 8557, -3441, 8599, 6336, 9805, 2858, -4355, -8716, -8048, 1768, 2918, -5971, 3542, -5702, 2296, 3361, 2925, 8368, -9494, -2018, 1125, 5847, 6891, -2544, 478, 9369, -9196, 4853, 6511, 9429, 2455, -6812, 1863, 1653, -5728, -11, -1830, 4019, 8352, -7362, -2041, -8354, 3499, 375, 3968, -7609, 8323, -329, -3756, -1932, 9710, -5820, 2437, -5930, -8207, 434, -5256, -9461, 3448, -7021, -9675, -7253, -6169, -1221, -7506, -9057, 6238, 2505, -381, 9942, 2285, 7664, -586, 9581, -3878, 5103, -6988, 5083, -7385, 6196, 3537, 3179, 7565, 149, 5896, -9992, -5377, -6680, -8407, 7034, -1659, 5924, 6288, -4477, -7301, 6512, -5360, -4274, -548, 3806, -4326, -7177, -616, -3128, 8446, -8879, -1142, 8925, -1440, 6511, -7561, -3284, -8873, 6907, 9144, -2677, 1008, -6933, -6204, 45, -5404, 7492, 1780, 5208, 4817, -6288, 264, 4132, 6306, -8153, 4890, 5433, -9304, 7551, -307, 5935, 6397, 7541, -7743, 5139, -3972, 1909, -4354, -6311, 1467, 587, 8385, 2624, 4258, -5816, -2541, -9767, 8610, -5946, 4623, 3671, -2010, 7584, -3084, 5787, -4318, 5741, 6716, -6870, -6906, 4087, -2618, -4305, 5275, 5835, -7302, -7119, 7517, -2497, 2384, -1676, -2756, 9189, 280, -1794, 879, 2212, 7339, 7789, 307, -4558, 4862, -3598, -1288, -6862, -5844, 5867, -8358, 7, -1080, 2469, 7957, -5717, -5190, 917, 4567, -2252, 4173, -7141, -7390, 8269, 9961, 8315, -7817, 9217, 6371, 9141, -2493, -8650, 2495, 6023, 5218, -8774, 6541, -7463, 7191, -1074, -6008, -8227, 5838, -8176, -2055, -7057, 4384, -9780, -117, 5939, 4103, 6025, -9850, -710, -7652, -1667, -9262, -2052, -7778, 4879, -672, -7309, 8785, -3346, 2803, -1423, 4419, -335, -4773, -8440, -3660, 4249, 5122, -3543, -7435, 7127, 4127, 4875, 1587, -9048, 8184, 5335, -9262, -6637, 8666, -7226, 1229, -1936, -1099, 9339, -9782, 7813, 1216, -5555, -1114, -8100, -3572, -3859, -1408, -5798, 6799, 3370, -396, 563, -1518, -3515, -689, -6569, 6974, -1870, 3055, -7769, 3461, -8301, 5570, 2521, 5185, -4711, -8615, -7723, 7854, 4744, -8097, -3105, -8281, -3023, 6281, -1974, -8018, -1975, 4451, 4810, -5120, 6719, -8620, 4261, -5445, 3490, -243, -5953, 2031, 8744, -6764, 4550, -1857, 3894, -6053, -77, 4134, 3504, -9615, 397, 6759, -5081, 3582, 9891, -9794, 3145, -7957, 1065, -8775, 4738, 3983, -4589, 334, 2163, 9855, 3400, -7617, 2479, -9879, -8639, 482, 2162, 5069, 7848, 7225, -7149, 5827, 6727, 4366, -5547, -544, 3001, 6584, -2791, -8489, -2550, 1995, 2569, -4920, -9063, -5037, 6900, 4387, 6407, 6732, -7177, 9196, 2015, 4241, 1886, 1986, 2726, -6726, -9128, -7471, 6992, -1620, 4853, -5592, -8625, 7111, -4142, -3351, 4105, -3011, 1498, -5237, 6141, -3852, -9782, -9449, -9430, 8816, -3087, 3461, 1053, 1978, -2715, -6530, 4015, 8192, -8063, -9237, -511, -7945, -2541, 1525, -1769, 4838, -1531, -6451, -6412, 4792, -7037, 8306, -7547, 2421, -6515, 2317, -926, -9728, -2149, -2297, -8799, -1697, -5663, -9477, -9390, 7414, 8184, 6239, 6342, 1942, -717, 3425, 6323, 9902, -712, 5787, -9304, 2427, 7373, 4197, -368, 7435, 553, 253, 7049, 2255, -1690, 6354, -2096, -6828, 5636, 7681, -3125, -990, 3937, 1388, -8159, 2420, 3102, -3935, -715, 8148, 5819, 8005, -4888, -6804, -6868, 6210, 5429, 9053, 2920, 4257, -1972, 1559, 29, 2418, -3414, 45, -2241, 5079, -8521, -235, 7322, -7320, -6966, 8103, 672, -2626, 271, 6809, -3873, 2447, 2242, -2417, -7434, -7759, -5497, -246, -9232, -8913, -3588, -4304, -1744, -1196, -193, 4753, -451, 3043, -8622, 1420, 8869, -1465, 4820, -7257, -2709, 3453, 8057, -9214, -2660, -6034, 5323, 2101, -120, 1884, 4829, -9701, 8777, -1737, 1064, -7957, 7205, 6582, 3749, 9001, -6638, 6868, 3806, 2620, -4564, 1143, 858, 5236, -4834, -5911, 7186, 4634, -3041, -8862, 3914, -3334, -5123, 8422, -1756, -4503, -9194, -6622, 8439, 5356, -990, 4251, 6355, -6626, 7860, 2430, 9977, 983, -231, 7192, -365, 887, 7587, -127, 6155, -7885, -4926, 8029, -3521, -2713, -8166, 4188, 1194, 9050, 2410, -5956, -1470, -8601, 7402, 8652, -5232, -2258, 7920, -4830, 1176, 219, 4091, -3721, 7487, 5072, -1612, 324, 7976, -7549, 7141, -8670, -7650, 1530, -178, 467, 8065, -9882, 4042, -1133, -6776, -2412, 9503, -5628, 3324, 1517, 6958, -3338, -838, -8377, 6616, -7550, -1299, -5770, 5399, 4578, 6660, 2997, -9373, 8590, 2597, 3089, -717, -5366, -8103, -4250, -8566, -489, -3282, 5930, 7960, -2164, 8176, -5760, 8143, 6186, -6572, 8343, -9335, -6363, -7289, 4216, -1030, 9162, -1284, -832, -1959, -6606, 7639, 2106, 519, 4764, -29, -4634, -8182, 2811, -7204, 8110, -1477, -968, -4405, 7708, -5030, 2732, -4510, 8639, -2201, 3078, -6053, 8755, 5699, 2522, 2680, 1865, -6388, -2253, -2435, -8738, -4491, 5006, 8987, -3735, -6963, -4919, -457, -6710, -3205, 9583, -5693, -719, -5983, 3712, 4936, 9731, 6950, 9579, -8499, -9056, -1624, -1873, -680, 4427, -2430, 3668, -2763, 1088, 6249, -9608, 591, -7995, -6392, -7284, -5336, 6276, -3422, -6064, -5038, -8667, -3710, -8700, -8894, -6388, -2312, 9719, 4172, 4864, -5198, 6734, -3181, -2879, -8733, 7513, 3173, -9051, -2486, -8542, -2625, -4050, 8474, 8337, 6163, 7356, 279, -2410, -4616, -9975, -5003, 5124, -1358, 9593, 9556, 1277, -3081, 8863, 2175, -6250, -1839, 7995, 5755, 6842, 1780, -230, 4674, -4823, -8436, 2188, 7430, -2416, 3723, -6444, 1047, -9722, -8677, 6165, -1490, 1693, 8467, 1778, -2959, 4297, -1476, 6117, 8076, -1860, 5378, -7661, -8700, -3511, -690, 9339, 6502, 490, -4957, 2014, -3185, -7568, -1076, -9552, 8098, -8972, 7467, 2550, -742, 8018, 2342, 5400, -6815, 8553, 7007, -7570, -7936, -474, 299, 686, -9088, 9544, -2480, -4093, 2182, -847, 4888, -3772, 1290, 5108, -9980, -8845, 5264, -5199, -8271, 9262, 8903, 4905, 7886, 753, 1281, 11, -9758, -5863, 8091, -8710, -7291, -7403, 2530, 8925, -3551, 4020, 5477, -908, -3435, -9412, -8455, -3189, -9512, -4878, -3612, 4362, 6590, 6483, -9667, 2206, 3695, 4548, 1698, 6890, 3881, 3767, 6698, -7598, 493, -5536, -6990, -3924, -9506, -9366, -484, 2038, 8543, -9065, -360, 5367, 3124, 1066, -2686, 4145, 4341, -2164, -1742, -8080, -5718, -6857, -1088, 4455, 3145, 6627, 5379, -6495, 9867, 1777, 3007, 8655, 6546, -7015, -6180, 2052, -7131, -7527, 685, -2302, 2921, -7427, -1897, 1989, 2349, 1804, -675, 1101, -9164, -8376, -8075, 3792, 282, 8532, 7665, 4398, -715, -4911, 8245, 4868, -4099, -7736, -1185, -6977, -5959, -2064, -656, 9245, 9272, 6768, 7953, -8138, 124, -6474, -2667, 4677, 3076, -8062, 8183, 866, -4537, -2763, -5336, -3112, -7909, 4043, -3600, 4868, 5286, -1178, -8582, -2877, -6140, -8145, -4914, 269, -1440, 6736, 6255, -3067, -268, -6936, -7384, -4008, -5407, -6138, 2167, 8795, 45, 1237, -47, 4085, -9842, 1004, -9460, 4019, 5334, -3881, 3023, 2813, -2277, -6632, 584, -6292, 3854, 9220, -7910, -8888, 4357, 7201, 4888, -1727, -6345, 4668, -5027, 4137, 3873, -4213, 7896, 6418, -5742, -4150, 2397, -88, 5134, -4610, -24, -7878, -2694, -7213, 1271, -7317, 2014, -1135, 1963, -4074, -7803, 2517, -9443, -1035, 1947, 1477, 3086, -8679, 176, 8165, -5469, 3563, 8845, -1114, -9706, 9917, -9560, 4046, 6177, -556, 4296, -8350, 4593, -252, 1202, 5489, 8441, -1891, 2768, 9245, -658, -1666, -7469, -2757, -3490, -7828, 4228, -411, 9003, -9710, 9023, 5848, 6293, -7629, -827, 3895, 9365, -1173, 6206, 6694, -1979, -4188, -8103, 5354, 930, 701, -8507, 4773, -8567, 1935, -1666, 1550, 4146, -606, -2205, 5815, 392, -6897, 2915, -2902, 6458, -5507, 5906, 9107, -6292, -8488, -7506, -3079, 103, -6045, 9742, 6786, -4621, -8497, -1144, -5587, 9839, -8026, 8931, 8789, 7020, -2028, 2019, 5946, 5425, -8773, 7878, -9902, 943, -5104, 9487, 9094, -9143, 9674, -6626, 6883, 4875, -5605, -1625, 6044, 1591, 4842, 9894, -7276, -9407, 921, -5685, 5079, -6577, -6785, -7206, 5514, -6367, 3685, 7537, 2450, 3459, 4424, -5587, 2748, -2716, 7023, 5087, -5280, -3322, -7110, 6286, 6517, -9207, 4049, 4601, 6438, -9665, -1576, -6088, -3258, -5162, 8238, 3177, -6094, -4090, 2220, -2960, -9630, 3130, 568, 4052, -3071, -4752, -8559, -4503, 3467, -7763, -1064, -1399, 6530, -7105, 314, -8881, -7199, 5938, -5654, -6416, -5770, -7529, 6605, -7013, -2181, -5227, 2692, -5578, 6188, 7819, -5548, -9941, 8091, 3306, 8373, -6858, 8255, -9093, 1741, 1282, 7484, -9945, 2031, 7726, 84, 8533, -1673, -1372, 4043, 6114, -2221, -1164, 9366, 5084, 1935, -8310, 8541, 5701, -4294, -6251, -3599, 3562, 8160, -5178, -9866, -1067, 1979, -134, 3325, -3156, -4031, -4276, 9313, 8861, -3735, -5653, -1640, -209, -8382, 2938, 550, 8459, 9177, 9986, -2929, 9502, -5735, 5048, -2888, 3871, -5843, -5397, -4379, 8764, -4263, -2487, 1815, -7004, -5604, 2363, 2073, 2406, -3585, -6290, -7436, -496, 2564, -1136, 6901, -9857, -2825, 943, -5770, 857, 9027, 5154, 7336, 1127, -6101, -6341, 7418, -836, -9630, -1285, 1494, 2318, 8569, 2521, -2073, 7843, -4693, 9793, 2810, -2575, -8825, -1606, 9836, -2344, -1514, 2327, -728, 5927, 7677, 7048, -6130, 4045, -7177, -4709, -9502, 3003, 9114, 7467, 5687, 2634, -6951, 7989, 7236, -7875, 6344, -6696, -3055, 9691, -7707, 1207, 3841, -2883, 7935, 2542, 8691, -6364, 9788, 5563, 6948, 7955, 5075, -3534, -4202, 6440, 1188, 667, -1129, 3660, 2254, 4066, -5801, -7118, 2406, -6487, 8006, 1705, 2597, 4907, -4062, -1965, -5476, -9794, -3893, 2474, 8386, -2265, 8133, 3052, -7751, 4977, 9089, 3342, 5584, 7336, -8917, 3906, 2782, 4939, 2228, 687, -5042, -3280, 1885, -3326, -1802, 939, 4530, 1185, -3773, -2808, 5131, 1447, -9428, 1755, -6015, 2637, 1641, -4866, 98, 4501, 1511, -2067, -5640, -8620, 2431, 6548, 6240, 7002, -8286, -7463, -8835, -6664, -8860, -470, 4894, 9823, 2610, 392, -8504, -164, 1945, -2070, -9032, -7707, 9848, 1260, 3335, 6807, 1665, -6376, -356, 9825, -6312, -4941, -5201, 8268, -1976, 5565, 1023, 9860, 2784, 6793, -7726, -3503, -7408, 9926, 2768, 8750, 1248, 7515, 2687, 7441, 2818, -973, 7646, -3145, 6139, -6347, 6071, -2006, -951, -336, -2810, -355, 3064, -8016, 4415, 7924, 8696, -5025, -1433, 9989, -889, 871, -2590, -1117, 7968, -1361, 7693, -6407, -9762, 8487, -3062, -7755, 811, -834, 4155, -5527, 5307, 2536, -3473, -2157, -1769, -3545, -6024, -8622, 2180, 5084, 2488, 5805, 3534, -1723, -2220, -6777, -6667, -3753, -5641, 9392, 778, -9962, -9618, -1170, -2088, -5934, -2039, 7878, -2693, 5554, -9408, -7314, -1847, 9665, 5861, 2556, 8993, 5609, 4635, 4103, -3792, 7972, 141, -7744, -81, -6287, -2395, -7261, 945, -5846, -843, 1078, 1158, 185, 435, 2118, -8429, 4468, 143, -5837, -7465, 5288, -4833, -792, -8022, 370, -8809, 5346, -3782, 3611, 6087, -9725, -7645, 3717, 2678, 6561, -2916, 7695, 9737, -5062, 3210, 5127, -8702, -1133, -6019, -7773, -3834, 895, -8884, 5418, 4549, 9517, 7214, -9683, -8906, 8857, 8744, 4037, 9182, -670, 9249, -1325, -6493, -7224, -2514, 2517, -9014, -4440, 8314, -3062, -6180, -8798, -132, 3279, 6471, 1927, -2995, 2543, -9312, -7277, 7227, -7899, -8589, 306, -9893, -691, -9847, -8147, -9797, -9339, -1737, 5523, -3815, -8905, -9802, -1686, 8828, 4083, -5385, -7892, -3547, 1193, 5047, 1479, 4256, 9775, -4432, 983, 4234, 7192, -9189, -2926, -8203, -2483, -9652, -2810, 5072, 5935, 1360, -3485, 2429, 3910, -2061, -7326, 6255, -3487, 6046, -7319, 8606, 5272, 2022, 8396, -7664, -5025, 5872, 2319, -5034, 2647, 2349, -6579, 5346, 9505, 7290, -6867, 5523, 1298, 5648, 672, 5996, 284, 7263, 3697, 8554, -6157, -1886, -8639, -5511, -4631, 9774, 9488, -4142, -9443, -5717, 6742, 7081, -1564, 2886, -631, -8685, -343, 7462, 1326, 7451, 3040, -2851, 8756, 1531, -382, 2219, -4845, -9911, -8565, 7025, 4271, 6768, -8025, -1821, -5494, -2737, -5628, 5166, -4599, 5708, 6068, 2002, 824, -8854, 4938, 8140, -4975, 333, 5775, -9799, 8297, 4193, -7582, -552, 932, 8192, -6075, 5634, 3470, 145, -3751, 3420, 4516, 9501, -8838, 1577, -7192, -2711, -9522, 9935, 2473, 8417, 5846, -21, -6479, -1178, -31, 8452, 7597, -780, 7976, 1575, 8683, -4332, -632, -2282, -5522, -7688, 1375, 8293, -2645, -1476, 3109, 2043, -5033, 8676, 858, 3149, -7972, 6515, -5513, 5752, -546, -9413, -9821, 8206, 8608, -5445, -5207, -3932, -1971, 6976, 9710, -512, -7815, 7207, 6450, -5981, -1123, 2757, -5136, -5527, 4976, 2916, 2588, -7681, -1826, -4650, -4306, 4973, -8427, 4541, -9745, 5393, 5251, -5428, 9716, -9076, -9046, 8173, 472, -9763, 1074, 3794, -4627, -1288, -5065, 5177, -3968, -9022, 9084, -8286, 7044, -3719, 8933, 442, -7966, -4557, -6601, -3603, -2952, -795, 1204, -6340, -1886, 9072, -4296, 2581, -3861, 9883, -5649, -6279, -3560, -6149, -1220, -1429, -5013, 8377, -212, -1747, 4065, -232, -896, 2551, -236, -5455, -6632, -9333, 2316, -6859, -518, -8071, -6755, -9929, 6836, 6556, -1724, -1192, 1689, 2048, -5643, 6970, -3542, -3202, 9059, -3603, 4410, 1371, 974, 8226, 2185, 9511, 8161, -8767, -1588, -6603, -3103, 8890, 6128, -6812, -96, -2470, -565, -8173, -7152, 7572, -5854, -4328, 7963, -6047, 8384, 5267, -4922, 888, -7757, 8381, 6931, 6678, -1623, -3958, -913, 1285, 8214, 2888, -8536, -7051, 2365, -1526, 5884, 8756, -1804, -4540, 5571, 351, 771, 3644, -2966, 4269, 5514, 3052, 1178, -9467, 9675, -9179, 2153, -9348, 7471, 6460, -5003, 4852, 5840, 5241, -1047, 8990, -6432, 6366, -9590, -1964, 5223, -4081, -8955, -991, 9675, -4607, -8477, -6688, -2816, 3791, -8993, 1027, -2084, -5239, 1188, -5940, 992, -958, 4076, -8225, 8475, 5977, -1450, 3588, 4836, 5245, 3650, -8810, -6923, 4147, -7659, -8561, -4005, -577, 24, -8789, -6225, 3880, -1981, 7911, 4674, 7232, 8462, 3640, -7492, 7329, -3034, 1820, -7008, 9908, 472, -5827, -356, -4442, 1155, -7051, -1876, -8420, -3926, 4709, 3432, -9832, -4643, -1106, -3645, 2635, 8360, 379, -3265, 1931, 3369, -34, 1427, 986, 4616, 8371, -7251, -2682, -9142, -8986, 3025, 1206, 6816, -5894, 501, -8547, -1257, 3756, -1917, -2243, -7498, 7033, -5075, 4568, 478, 1698, -885, -5384, 7280, 2537, 326, -4369, -9795, 6987, 6523, -2131, 9600, 1060, -7160, -3048, 6680, -3222, 7630, -6764, -6437, 9188, -508, 6999, -8135, 7120, -5868, 5682, 6776, -351, 7642, 5440, 6204, 2964, 3156, 7212, -1915, 5412, 3389, -49, 2256, -7483, 8122, -718, 850, 7219, 9806, 6646, -4630, -6990, 6026, 9386, -4640, -5818, -6975, 7183, 1879, 2219, -4904, -9862, 8776, 3613, -2282, 9440, 3491, -2176, -456, 47, 2842, -3170, 3937, -5738, -333, 1843, 7672, 4375, -160, -2514, -6869, 6633, -7119, 341, 1547, -6546, -5075, -8439, 8707, 1753, 742, -2544, -2482, -9737, 5562, 5843, -9499, -131, 2494, -7104, -1139, 686, -5098, -8073, -7996, 7676, 3969, 3249, -3429, -5329, -2512, -8616, -4423, -1376, 8786, -854, -2635, 7985, -7122, -7283, 2122, -2955, -2959, -6094, -5823, -7931, 9302, -4008, 836, 9732, -4129, 5925, 6469, 3031, 3608, 1248, -1779, 3166, -4710, -1091, -198, 9880, -3325, 8971, 1655, -9989, -1544, -1406, -7983, 7485, -1438, 7348, -8861, -1751, -8521, 5479, 1030, 8453, 5057, 2046, 5390, -8959, 1438, 8534, 5102, 1371, 1850, -5772, -7288, 6698, 8284, -905, 3596, -3296, -2918, 8987, -7394, 4304, -8977, -9956, 6626, 7475, -8694, 1152, 5108, 7505, 3615, 1094, -2111, 199, 155, 4189, -6886, -234, 14, -2407, -1046, 2055, -3963, 3798, 2869, -3146, -6048, 7834, -6692, -6121, 5351, 6214, 6667, -939, -9773, 5904, 1731, 7152, -84, -5936, -9879, -3271, -6253, -7927, -3902, 7207, -3287, -6784, 4485, 187, 8390, -3495, -6571, 1901, 8475, -7823, 7443, 4196, -4748, 5329, -8897, 3776, 1896, -5867, -11, -2242, -7409, 4966, 8770, 5524, 3031, 398, -5670, 1193, 623, 7954, -7893, -9975, 2823, -4766, -4961, -1812, 8689, -1704, 6439, 5982, -7361, -3335, -394, 7204, 8590, -3911, -6523, -5389, -2582, 6992, 1989, 2442, 1080, 2560, -2646, 2500, 9843, -3627, 2527, -9363, -6911, -8185, 1041, -9789, -7017, -223, 8092, 3075, 1834, -5479, -1506, 4487, 5065, -2320, 1465, 329, -8255, -1745, 4763, -2858, 9574, 1969, -3804, -9954, 3571, -4107, 6512, 2487, 8851, -9765, -8584, -1027, -1074, -1753, 2258, 4986, -6998, 8009, 8769, -5547, -7918, 2616, 6037, 4653, -1654, 8117, -8732, -4953, -1435, -5053, -6347, 1608, 5490, 473, -2883, -812, -5720, -416, -9246, -729, 7685, 1691, -3543, 77, 2052, -5306, 1190, -3546, -4489, 3815, -9492, 6042, 5408, 6270, -350, 2821, 6205, 202, 9363, 3180, 2353, -7499, 7247, -8446, -9975, -3776, 8604, -5262, 9807, -451, -8760, 4681, 6154, -5531, -6304, 7792, -9140, 1204, 6177, -6795, -3576, 4615, -8620, 1701, -5867, 2265, -8957, 5101, -2694, 1279, -7884, -7990, -7274, 332, 6304, 7468, 1324, -8894, -5778, 1144, 6522, 3408, 8506, -4685, 1534, 1817, 2990, 1643, -3505, -4465, -5925, -1132, -3868, -9689, -4951, 6320, 9810, 9494, -9967, -7240, -5690, -387, -4702, -9936, 9430, -257, 816, -6975, -3769, -5945, -3514, 3523, -2177, 6266, -4226, 2396, -1717, -1822, -427, 3843, -1742, -5751, 3270, -9157, -329, -7343, -539, 2445, -7594, 4562, 1492, 4459, -1928, -1363, 8858, -2139, -560, 3559, 8727, -7890, -7346, -624, 7332, -8483, 2441, -2621, 9109, 6841, -7750, 6754, -9701, 4676, 3128, -287, -6601, -8628, 2481, -9515, -8814, -277, -5700, -1555, -2135, 2410, 3008, -7785, -8330, 5587, 8332, -9290, 7847, 9542, 7352, 8090, 5943, -1044, -5959, -4234, -3098, -376, 4589, 5653, -9341, 5779, -743, 8044, 2172, -7908, -4590, -3885, -4272, 3018, -7698, 2963, -1271, -6776, 4195, 3371, -5380, -6218, -1252, 1869, 7592, 9300, -239, 80, -8260, -5655, 7281, -3054, 5399, -9760, -6167, 706, 7110, -3747, 3786, -2, -7137, -2335, 7371, -3560, -5346, -7150, -9435, 5562, 1785, -9999, 3278, 1931, -1537, -86, -5534, -4498, 1799, 7303, 5210, 4458, -186, -1588, -5213, -4220, -6497, 185, -8731, -4358, 8644, 1842, -6809, 185, -6176, 125, 9404, -5408, 4419, 7826, 3833, 7157, -6395, 4446, 9210, 672, -3437, 1324, -4679, 4781, 5894, -3781, -6146, -4163, -4931, -640, -4397, 5045, -6120, 1162, 6265, 2627, -7057, -8881, 484, -9629, -3342, -6319, 6493, -1620, -5233, 7871, 8445, -1260, 9373, -2569, 1655, 5043, 8982, -9100, 1426, -8535, -5491, 3627, -9948, 8896, -660, -8791, 2096, -5563, 7936, -6513, 8616, -2967, 9667, -391, 5308, 3018, 2110, 840, 6374, -1579, 5101, 1421, 4748, 9341, -9793, 5074, 4754, 7596, -5984, 1491, 5745, 2616, 9294, -8051, 8676, -251, 2228, -2960, -8194, 2472, 4368, -5762, 3060, 1797, -9997, 7838, 748, 9762, 2585, 7702, -908, 7607, -241, 6870, -9825, -8532, -8528, 4645, 9755, -2746, -7507, -6828, 3241, -8577, -9316, 5722, -3335, 622, 7089, 7968, -3175, -7467, -5697, -4967, -3749, -2623, -1613, 4694, -8434, -1, 5525, 2129, -5671, 4344, 9562, 7994, -2741, -2299, 990, 8489, -8401, 1900, -418, 157, -7760, 2582, -1890, -9984, 4233, 1633, -981, -3613, 660, 2706, -5073, -739, 5973, -3538, 7027, -3336, 7520, -728, -7588, -7984, -2978, -311, -2645, 8929, -9448, -323, 4742, 8560, -8928, 8400, 2389, 5096, 1896, -8634, -3028, 9115, 9338, 9739, -5976, -194, 7604, 8504, -3592, -4688, 2562, 2617, 5936, 5158, 4107, 9774, 5839, -5008, -7882, -3714, -6840, -2531, 6390, 542, 3734, -1071, -7362, 9911, 8268, 6672, 5560, 6537, 8133, 9562, -7292, 3265, 2570, 5470, 9746, 9225, 3046, 421, -2124, 5008, -8493, 9170, -7831, 272, -4915, -4219, 6374, -9402, 3434, 5595, -1523, -5576, -2007, -7703, -9416, 4264, 5350, 4588, 7917, -8952, 4199, -5041, 5720, -609, 6947, -2528, 6258, 4097, 5050, -7413, -7474, 2098, 6842, 6907, -5733, 4486, -10, -1680, -451, -3633, -5565, -570, -6787, -763, -4634, -8376, -2979, -9974, 9880, -9249, -5261, 1174, -7366, 7939, 6770, 7008, 3347, -9718, 2008, -6019, 4208, -6682, 8915, 9389, -7130, -9162, -5709, -8730, 6248, 6691, -9344, 3757, -7758, 7123, -5459, -3752, -2686, -659, 1731, -6442, -1944, 3213, 8956, -7542, 5389, 2734, -7338, 624, 302, 3214, -7439, -4429, 9599, 6484, -4989, -8180, 4270, -9685, 1018, 3253, 536, 3628, 582, 5967, 972, 9253, 3218, 357, -9159, -3620, -6136, -7336, 9527, -4935, 470, -1508, -3534, 789, 1187, 3580, -4639, -6220, 5368, 3353, -5595, -382, -9496, 5825, -2273, -5930, -1508, -1598, 7001, -9641, -5663, -8484, -2140, 3061, -9175, 7625, 3091, 1314, 3179, 3457, -1772, -5889, -4512, -8857, 5381, -8377, 1599, -5994, -1431, -8806, -5315, 6915, -9411, 9269, -1065, -6543, 8838, -4227, -6424, -7043, -7133, -3981, -5363, -6761, 2154, -3586, -1761, -9938, 7811, 7369, -4469, -3163, 7204, -7167, -6665, -1555, -7560, 4547, -2177, 595, -8042, 4847, -1728, 4420, -9778, 9047, -5528, 4177, -820, -9049, -9657, -8551, -632, -963, 6801, -9400, 667, -6841, -2916, -8936, -2037, -4949, -6723, 771, 9767, 9348, 691, 732, -941, -5621, -8934, 2975, -1882, -982, -4937, 5248, 8216, 4921, 3494, 9092, -9237, -7701, 6974, -8434, -8579, -1457, -2705, 3240, -4787, -5513, -1290, -2983, 9453, 7555, -4460, -9889, 5448, -5944, 1359, 7236, -1828, 9292, -345, -6428, -6452, 8328, 2056, -7844, -4918, 6174, 5398, 882, 9148, 5655, -12, -428, -2746, 2694, 2215, -6756, 9044, 4852, 2148, -3262, 417, 8243, 2307, -6631, -143, -330, -9126, 870, -2136, 4084, -1105, -8002, -4645, 7002, -4179, 6881, 818, 3529, 3986, -4077, 167, -5347, -4552, 5884, 9067, 491, -6833, 6833, -4535, 4164, 433, -4690, -711, -9698, 5402, 237, 1780, 9346, -8565, 5243, 4418, -8280, -5903, -1317, 7439, 887, 3411, 330, -5632, 7637, 3806, 6454, -817, -7279, -7072, 6576, -1886, 6330, -679, -3526, 1116, 1044, 8087, -1715, -1918, -3336, 6895, -5866, -2789, 7107, 2582, -3330, -9298, -7717, -1415, -9334, -2266, -6135, 5210, -7338, -5335, 8586, 4064, -191, 9311, -376, -3705, -2155, 1076, -2867, 7778, 889, 3194, 1676, 5558, 5968, 3359, -5586, -187, -4027, 8071, 424, -3012, 6481, 7595, -1080, 1058, 7650, 7572, -9710, -2551, 7100, 9969, 3653, 283, -9837, -9256, -588, -5360, -2327, 7480, 1905, 3992, -7300, -2424, -8064, -7501, 4007, -8518, 6393, 2300, 1965, 4870, 234, -6952, -9558, -9886, 3817, -9680, 5900, -268, -3001, 6524, 7105, 1346, -5043, -8811, -5344, 8519, 9753, -838, -1677, -9726, -786, -6726, 695, -1045, 3610, -6535, -5873, 2684, -6637, 8603, 9699, -7475, 626, 7690, -2525, -5641, 4129, 4276, -1674, 2541, -5926, -3437, 574, 9592, 8944, -2960, -3924, -8276, 433, -4209, 6430, 2750, 7770, -7689, -3997, -1876, 7854, -9630, 5989, -4716, -6202, -4245, -4848, 5566, 966, -2976, -658, -4095, 3815, -2596, -1757, 1006, 6644, 6174, 6809, 8551, 8539, 520, 5884, 5974, 5898, -675, -5919, -9399, 472, 6574, 4711, -78, 5292, -8881, -949, 9986, 4004, -6319, 5795, -2938, -3398, -3589, -9067, 9500, -8401, -3864, -4301, -1716, -7529, 3317, 7447, 2098, 2279, -7944, -5298, -3492, 5105, -3814, 5780, 8540, 7530, -1596, 9875, -4481, -5214, -3321, -8871, 9600, 576, 408, -1819, -4142, -723, 7387, 6857, 8722, -9724, -4496, 9970, -6671, -3827, -2606, -3203, -4791, 6336, -2400, 9981, 115, -3405, -9806, 7626, -8527, 7631, -5403, 6229, -5750, -3199, -5477, 3075, -1775, 3198, -2034, -2422, -7612, -9814, 4610, 3138, -6361, 4691, -3523, 7677, 3062, -9273, -808, -4433, 9571, 3179, 3708, 4863, 9829, 9559, 475, 863, 5635, -9430, 6650, -2938, 8134, 9861, 4298, -5110, 4665, 3380, -4874, -4225, -1691, 7682, 5622, -9957, -9738, 671, -6967, 1392, 4570, 6033, -1092, -1339, 862, -8243, -1198, 6219, -525, -3249, 2276, -3385, 4192, -3081, -5789, -1885, 7429, 4928, -1236, -2469, 5714, 8662, -2357, -7057, 3699, 4184, -5948, 8934, 8749, -3201, -4664, -9521, -506, -4476, 1887, -9401, -4266, 1681, -4470, 8080, -8049, -2758, 2550, 524, -9887, -3152, -2956, 6081, 7319, -6921, 8229, 5593, 7580, -9240, 7231, -9877, 4977, -1524, -8739, 2378, -8084, 5695, 8652, -8572, 3780, -9843, -2438, 9202, -3204, 806, 5511, 460, 4993, 903, 3612, -6545, -4551, 2442, -1690, -3129, 8532, -4931, 3108, -5245, -8100, 1865, -1511, 7051, -8265, -6829, 8505, -3021, 800, 1428, 5031, -2652, 3676, 1327, 1483, 672, 5952, -6908, 4715, 3001, -9639, -6756, 3213, 2170, 4205, -6098, 7723, -1463, 1259, -1863, -2792, -9376, -4148, 8996, 4722, -1868, 6228, -4668, -7569, 6974, 6150, 2107, 9293, 4156, 1040, -1627, 9251, -8095, 9778, 9388, -6719, -1452, 8841, -9947, 6353, 8980, -2131, -478, 1761, 9857, -9342, 4639, 5415, -9295, -1106, -8985, -4180, 2253, 5263, 5468, 1831, -9484, 7508, -9117, 1864, 6917, 3246, -4910, 8532, -6267, -3373, 2519, -8640, -2519, -7218, -1215, 276, -6065, 5803, -5357, 6971, 6295, -2519, 311, 8600, 3310, 6200, 9990, 4154, -9474, -964, 9084, -9976, 3138, 3120, 6567, -1667, -7082, 6133, 3423, -4376, -6477, -946, 9265, -1272, 2397, -3190, -3741, -5133, -2292, 9756, -9797, -8491, -793, -2234, 4550, -3027, 7631, -4378, 929, -4293, 3941, 8657, 5815, -6644, 7019, -5181, 9528, 1001, -1325, -8871, -8829, 7348, 9313, 8090, 5634, 146, 917, 424, -3417, 1344, -7462, 8445, -1294, 5305, -9681, 2745, 2364, 1951, -9590, -1635, 4214, 4186, 7144, -5925, 2574, -7031, 4721, -4651, -5904, 6775, 8075, 9838, -2716, 7783, -7820, -6382, -3108, -42, -1489, 473, -3332, 1020, 7250, -3725, -8574, -3247, 2832, -4407, 3725, 3850, 1775, 8527, 9508, -1789, -4231, -5004, -5425, -8473, -8770, -9445, 9567, -692, 1346, -2202, 9209, -6158, 8861, 9705, -8925, -642, 8909, 2949, -1881, 5275, -8180, 7747, -1893, -6403, 8155, 4795, -8843, 4081, 8066, -8425, -6087, 807, 4895, -225, -3983, 6661, 3436, -8643, 2530, 9466, -1283, 6706, 1795, -4275, 2709, -4307, -5828, -1601, -1550, 3059, 9988, -3713, -2115, 7660, 3388, 506, -8390, 6173, 5799, 1753, -8065, -7693, 9445, 3005, 5544, -7429, 5764, 6277, -1604, -9257, -3269, 6085, 3855, -2081, -6059, 8673, -6961, 6769, -3066, -5123, 6114, 3149, 3481, 8291, 1972, -3907, -8784, 959, 988, -2562, 4195, 6805, 8805, 2751, 1293, -132, -5534, -3290, 8368, 3387, -3395, -1001, -5987, -2092, -7338, -7503, -5628, 5562, 7115, 4206, 877, 3123, -4567, -9481, -7085, -7532, 1947, 5670, -1326, 8328, -8171, -2344, -1389, -6568, 6836, 1381, -1756, 6585, 6275, -2685, -6283, -6831, 2098, 3596, 7397, 3671, -9978, -2314, 4062, 1261, -9813, -4385, -6917, 9074, 1084, -4840, 1005, -6049, -782, 6382, -154, -2402, 2724, -4564, -4209, -6063, -9273, 8839, -1330, -1982, 311, 1517, 8991, -6336, -3144, -4919, -3339, -4575, -7257, 8447, -837, -1860, 6651, -9493, 6191, -5314, -6296, -7820, 9878, -5536, 2475, 1094, 6373, 3311, -2570, -6012, -7427, -8610, 5173, 3643, -5431, -9296, -1830, 7972, 9233, -7881, -1161, 5319, 4694, -4310, -7488, -2857, 4937, 5400, -6119, 9242, -500, -1888, -2709, -3087, 1014, 5127, 4651, -7328, 4782, 146, -9664, 536, 5723, -7155, -2534, 5322, -8716, 5105, 6927, 2647, 8522, -7073, -2646, 473, -7654, 5965, -8360, -6116, 7931, 7598, 4152, -5504, 3494, -4737, -7741, 1132, -3560, 5550, 7901, 3940, 4457, 8032, -5917, -6566, 5787, 2157, 9080, -2001, 2603, -9108, 911, 6419, 8348, 1113, 5191, 4558, 3022, -5232, 7379, 8927, 2037, -7754, 4948, -850, 9364, -6423, 5566, 7467, 7212, 2613, 7590, 3501, 8968, 1394, -8425, 9396, -1480, 86, 7987, 9178, 6766, 9800, -5744, 6337, -3480, -2164, -7753, -4207, 1107, 6644, 6374, 1590, -8321, 7831, 4014, 3921, -9787, -8691, -4585, 3727, 9580, 7369, 5079, -965, 1091, -652, 9833, -7749, 6521, 2530, -4048, 356, 4651, -55, 7043, 4047, -9369, -4839, 2416, 1772, -6670, -6628, -4796, 2299, -8760, -4540, -8797, -8182, 9701, -3499, 5455, 318, -5308, 3407, 1094, 2090, -1223, -7322, -5667, -9308, -9577, -8260, -9667, 2565, -1608, 5241, 1423, -2449, -1727, 9074, 9474, 1201, 1322, 9036, 7794, 9693, 3669, 3983, -947, -1575, -1815, 6990, 4395, 1676, 147, -1437, -5560, 7787, -3382, 9840, 5167, 5177, -9683, -4536, -4782, -2843, -1178, -5697, 6050, 8274, 3238, -7988, -1931, -992, -2033, -436, 1802, 3913, 759, 240, 1728, -9371, -2492, -3577, -3754, 6611, 1467, -6261, 5967, 3868, -1929, 9288, 8969, 66, -6657, 5597, 6032, 5879, -8597, 3142, -8152, -7482, -3200, 5116, 8688, -489, 8929, 436, -5185, 6889, -419, 4132, 8541, -484, 8495, -4814, -8342, -9371, 6860, -5911, -7037, -4955, -3387, -8427, 9062, -9610, 1607, 4461, 3631, -8924, -9982, 7253, 8571, 5788, 7386, 75, -7674, -4893, 3652, 6173, -2444, 6895, 1478, 4509, 5019, -565, 4512, -5957, -8382, 7959, 7851, 5382, 9980, 5348, 5134, -810, 3302, 235, 6819, -8552, -2812, 8081, -5265, -193, -4203, 5702, 4040, -5833, 7391, 719, 7498, 3420, 8330, 3285, -812, -3917, 7986, 7813, 9385, -9948, -1984, -4624, -228, -6028, -6655, -9686, 3899, -7343, -7137, -2743, 2181, 7348, 7924, 7497, -6798, -261, -4028, -358, 6862, 5340, 6758, 6732, 613, -5095, 4612, 9170, 5981, 1999, 9807, 791, 8012, -7361, 9619, -9602, 25, -4413, -1235, 7738, 1776, -6648, -9695, -7707, 8823, -1534, -5674, -5380, 2595, 6622, 7803, -4717, 180, -5077, 1943, -3055, -3577, 3896, -467, 1749, -2459, -7656, -5456, 894, -8113, 2764, -9043, 2697, -5311, 185, -3530, 7761, 1496, 8020, -1409, -6139, -5583, -1092, 8031, -6900, -1358, 3927, -2332, 8882, 5152, -2237, -905, 7729, -5255, 4475, 3875, 4904, 299, 1268, 1910, 9384, 3184, 6511, 8556, -380, 4945, -8663, -9398, -1856, -3075, -2672, 9546, -382, 2155, -371, -1371, -3868, 1461, 672, 9509, -5103, -6982, -1273, 4132, -5458, -1710, 6626, 1692, 8684, -9435, -4251, -7288, 8427, 4539, 4522, -6884, 503, 9500, 3915, 9660, 2527, 7081, 985, 6036, 7788, 7076, -2483, 4878, -6155, 1511, 3616, 6672, 220, -278, 2026, -5554, 1441, 3165, 9717, -6988, -798, -6250, 1700, -1057, 217, -3778, -5618, -8864, -8337, -9574, 9397, -9190, 4658, 3068, -5021, 7642, -4963, 3638, -8214, 2745, -4030, 8296, 4535, 5443, -4855, -3061, -8908, -8542, -5522, -2569, -120, -2748, 6302, 4561, -7985, -2340, 3109, 4417, -6315, 9183, -4098, 6331, 8824, 5942, -3684, 4235, 278, 2066, 9284, 8711, 4150, 9741, -6071, 5206, -8693, 3409, -3841, -6684, -5157, -8816, 9196, -3234, 4463, -3769, 3414, -4300, 2033, -6419, -9720, -9909, 6813, 5918, -6101, 2030, 8158, -3420, -5786, -2082, 2901, 9702, -5496, -8585, -1143, -1527, 6377, 6069, 8622, 3754, -6374, -3403, -6662, -21, 7652, -669, 5356, -3280, 2632, -2029, 1481, -1164, -4483, -7030, 7216, 3592, -3637, 9757, 4122, -7460, 8543, 1587, 7480, 8072, -6521, 6847, -8242, 6130, 7088, 292, 8808, -5657, -8883, -3983, -2667, 3858, 3758, -8922, 2622, 1086, -1636, -8363, -1737, 2385, 489, 1043, 1449, 982, 8555, 9775, 3032, 591, -9666, -254, -7361, -6745, -5245, 9153, 1944, 6470, 9853, 1453, -9577, -3886, 2932, 8919, -9685, -9850, -605, -6146, 3068, 3642, -6234, 6501, -7369, -9349, 2722, -4476, -6909, -7128, -4269, 3871, -7245, -8270, -6514, -8723, 8560, -1405, 597, 2115, -1989, 7849, -413, 7549, 1348, 253, -5969, 79, -3866, 2890, -9125, 174, 2070, -3505, -8593, -2552, -6652, -2021, -4193, 4899, 1196, 4029, 651, 2498, -5824, 4681, 5174, -8614, -976, 8158, 6764, -4529, -4851, 4653, 4358, 4163, -1442, -5494, 6677, -1455, -2512, 7291, -3435, 5861, -2876, -9817, -585, 9277, -4473, 7557, 7552, -4407, 4004, 6954, 6120, 3890, 6938, -5002, -3558, 2626, -234, 8084, 4933, -5509, 4569, -4358, 9666, 3356, 3894, -1068, 2527, -8080, -4028, -9217, 5929, -1102, 4041, 6595, 1997, -7949, 3208, 9727, 9471, 4385, 6744, -3696, 4301, 3122, -4693, -2544, 8890, 6002, 7639, 3280, -6466, 664, 318, 8445, 3059, 9118, 1895, -9787, 3105, -7094, 5536, 8936, 8280, -1542, 7268, 5994, -4642, 4048, -3857, 4984, 4807, -4732, 5738, 6247, 4285, -1930, -3425, -3702, 518, -5693, 4851, -2910, 5765, 8410, 3736, 4513, 6236, 8552, 5119, -1532, 7510, -8540, -2477, -1635, -1782, 6826, -9114, 7951, 3342, -1122, -2694, 6458, 8900, 5455, -6860, 9717, 2269, 2201, -9799, -6992, 8919, 2628, 6841, -4531, -5742, 9798, -8231, 9191, 3849, -7474, 6242, 9813, 7829, -2878, -717, -9540, -4161, 4899, -3477, -609, 256, -2981, 815, -1650, -5122, 8219, -817, -5464, 9035, -6163, 2003, -3426, 7761, -9991, 9043, -9078, 8825, -2454, 1931, 386, -4908, -7912, -4610, -4200, -59, -5448, 2301, 8784, -3604, 9230, -9590, 2347, -6776, -9686, 7323, -3644, -6284, 7671, 9532, -4634, -7236, -9784, 6892, -5097, -2492, 7521, -2047, -5767, -4710, -4157, 2986, -8826, 5793, 350, -1276, 7147, -7582, -4859, 7064, 8875, -5276, -8274, -747, -7540, 1327, 6281, -9957, 5441, 8646, 1343, 6011, -9734, 869, -5683, 508, 4771, -136, 6421, 7539, 7110, 4128, -5393, -3615, -1050, -3572, 2262, -1333, -8512, 2308, 8307, 7092, -7790, -391, 7070, -4473, -3897, -7276, 921, 8565, 835, -4559, 4366, 5104, 2338, 6071, -3112, 9922, 9910, 7430, -4433, 960, -4305, 5877, 7824, -6407, -8167, 6717, 800, 7140, -1031, -2429, 124, 2903, 6376, 6819, 5809, -5313, 4286, 2416, 3735, 7338, 2503, -3143, -5297, 2586, 6944, -7952, -5770, 2746, -8253, -3578, -1402, -8766, 9719, -5662, -2481, -7328, -1304, -7188, 5183, -5667, -1039, -6279, 2826, 8895, 7658, 7704, 5771, 8021, -2878, -6563, -2531, 195, -1898, -9481, -4014, 7842, -32, -3208, 2579, 197, 4841, 9212, 1482, 5876, -9390, -3775, 4678, 3159, 4527, -1977, 760, 5941, -1454, 61, -6872, 22, -577, -2316, 1304, 3269, -6185, -1269, 7933, -9737, 9498, 5441, -232, 1375, -835, -3063, -7428, 9298, 1201, -5653, 418, 3427, -8152, -1727, -3017, -84, -2852, 1272, 9189, 5454, -4300, -7157, 4037, -1068, 8548, 5400, 7838, 922, -9668, 4692, -1916, 1917, 5616, 5372, -5393, 61, -6786, -4869, -5279, 5984, 2887, 9504, 602, 4923, -8623, -8039, -9692, -5084, 9023, -2474, -4250, -8336, 4959, 3105, 5404, -3151, 5701, 510, -5765, -3360, 1971, -3135, 9509, -4662, -795, 2921, 2942, 8211, -481, -2219, 9152, -4057, 9668, -2607, 968, 3937, 1224, 6005, -3493, -7916, 7539, 5057, -7580, 3178, -7059, 1528, -8758, 3066, 7751, 9569, 4919, 4826, 7144, 7991, 1877, -6059, -1692, 2171, -1130, -3985, 7243, 6273, 3323, -8884, -6140, 7402, 5706, -2871, -2201, 7895, -5542, 7864, 4632, -5400, 3521, -5445, 3755, 2, 3926, -5035, -6618, -5184, 1213, -3669, 4486, 1044, 4796, 7653, 2806, -2823, -7883, 7731, -675, -8869, 1981, -301, 3342, 2996, 9218, 4254, -6932, 2019, -4927, -8601, 8119, 9500, -911, 5944, -5197, -4325, -3739, 8713, -6944, -2310, -1708, -470, -2367, 5544, -575, 9515, -2626, -2075, -7346, -8201, -6258, -7906, -9999, -3490, -3855, -2072, 5180, 295, -3718, -7745, 9470, -8931, 4270, -2685, -6586, -8755, -6237, -7098, -843, 7707, -4896, 2729, 2726, -7068, -9667, 5784, 8515, 7237, -3561, 4827, -2090, 893, -579, -4035, -5759, 5012, 7210, 4234, -2892, -7202, 4247, -3563, 3321, 7908, -886, 9188, 7194, -411, -364, -1400, 1925, 3108, 2703, -803, 200, -4204, 5165, -5275, -5665, 301, -4371, -8914, -625, 474, 6851, 3737, 9358, -5623, 3689, -8539, -9466, 4184, -1158, 5829, -350, 1724, 8741, -9015, 4843, -4609, 4512, -9061, 8744, -4075, -8199, -9349, -6309, -7969, -8636, 4922, 3841, 3598, 8655, 719, 5295, 4724, -9857, -3762, -9028, 1063, 1097, 6333, -4786, -4814, 4241, 6123, -7872, -8686, -4032, 984, 2496, -3726, 5320, 569, 2382, -8709, -2224, 8967, -7423, -5604, 9319, -85, -1183, -1399, 6010, 8899, 1831, -8606, 2766, 6385, 6764, -1810, -9493, 493, -4783, 1411, -7908, 3526, -5009, -7835, 2351, 1166, 6571, -665, 6067, 7319, 1719, 2082, -2304, -6508, -5691, -8569, 3762, -1873, 9597, 3568, -7484, -1771, 9710, -6346, -1796, 9782, 1148, -9048, -5247, -6360, -3884, -6158, -2649, 2836, 2564, 5943, -770, -7184, -3243, 6585, 1768, -5495, 4344, 3167, -9227, -2731, -8181, -7300, 509, -4116, -7604, 1776, 8815, -9307, 7605, -7227, 3037, -3281, 6785, -6282, -1511, -898, 872, 721, -1762, 3291, -2923, 4248, 6348, 8988, 1233, 6674, 9160, -6374, -2521, -6393, 9151, 8858, -9680, 33, 6345, -6887, 4988, -1852, 711, 9539, 948, -8011, 5710, 1053, -8642, -9972, -6706, 6650, -6058, -589, 883, 6029, -6217, -3435, -1474, 3613, 494, 9595, -2274, -9265, -1570, 7817, 7305, 4289, 5878, 5321, -6327, 3466, 3486, 1665, -5665, -6244, 8268, -9910, -7627, 2601, -6101, 4834, -1590, -5381, -609, 8543, -3588, 1677, -8190, -3728, -1535, -6815, -3152, 3124, 2664, -3908, 7794, -2026, 3853, -7104, 964, 2806, -7508, -1017, -308, -3434, -3950, 442, 9760, -3843, -3953, -2161, -3096, -1848, 4264, -6962, 7816, 2311, 3604, -3310, 7565, 2664, 8930, 409, -2315, -7774, 8672, -1438, -9059, 9251, -7200, 5824, -7001, -8575, 9259, 4747, -151, -8856, -7895, 3007, 5274, 9824, 5228, -9880, 1041, -808, -1298, 9957, 5732, 2271, -6885, 4871, -177, -1529, 8964, 3469, 2495, -8748, -4967, 9682, -8804, -6599, 6008, 2883, 4223, -3976, 207, -1574, 4627, -3154, 7977, 5186, 5166, -4892, 2529, -1703, 2039, 1943, 9776, -7767, 8350, 6651, 9872, 8443, 7330, -1482, 7090, -6283, -2946, -9663, 2378, 254, -7932, 1480, 2482, 7293, -9635, 8281, 2285, -5638, 683, -1702, -9156, -2012, 7595, 7550, 3033, 6117, -5738, -1597, -8872, 2180, 2237, -1904, 8758, 2759, 7463, 7624, -4938, -562, -1821, -8595, 708, 2517, 9647, 4884, -62, -2340, 1577, 9262, -1473, -8274, 8955, -210, 5067, 4843, -7813, -3929, 7341, 3048, -553, -1683, 7038, -961, 3365, -2920, 3325, -9595, -8207, 9600, -1399, 7322, 1798, -1282, 5155, -1234, 3917, -6537, 8829, -7891, 2573, -8518, 1763, -9226, 3075, -735, -8078, 6157, -976, -1387, 8266, -4381, 8536, -5197, 1923, 2299, -1157, 8004, -1817, 2884, 8807, -720, 1167, 2449, -787, -8047, -3655, -8820, 204, 6774, -2448, -6286, -6986, -6523, -78, -4391, 4347, -1030, 3890, -9982, -4832, -8281, -8874, 2390, 7437, 9018, -8203, -726, -9071, 6029, 5065, -2176, 5233, 7121, -3921, -3024, -8508, 6851, -595, 9588, -5310, -9040, -6327, -3191, 2062, 1461, 407, -7584, -734, -2399, -7122, -2310, -2979, -9018, -619, 6427, -8759, -6761, -9808, -2600, -5498, 1440, -4117, -2967, -7921, 8633, 3574, -9537, -4947, -4081, 7216, 7190, 3251, -4786, 9194, 282, -9929, 8379, -6987, -4567, 5964, -1565, 2275, 8184, -5988, 7616, 77, 3674, 4435, -5956, -7410, -7654, 7480, 4780, 309, -5041, -4965, -5435, 804, -6302, 7221, -7047, -9939, 1911, -5043, 2214, -3464, -3176, -6622, -5453, -7232, -9894, 5934, -9653, -7298, -7114, 9797, -5108, 5177, 1174, 5270, -331, -7374, 5893, 3268, 5582, 4827, -3522, -8564, 9915, -730, 7898, -6638, 2279, -4228, -4110, -9375, 4376, 3195, 149, -1765, -6452, 2685, -4064, -3436, -6452, 7880, 1188, 9450, -3190, 4854, 8112, -7941, -1493, -1152, 7637, -6745, -8242, -9826, -14, -472, 4520, -5231, 8717, -9946, 6257, 1176, -3983, -2796, -9601, -1686, -5648, -2789, 8056, 3668, 3625, -7982, -2090, -766, -1202, -6285, -3819, 73, 4780, 3705, 962, -9716, -9851, -7309, -7072, 8697, 577, -5520, 4861, 467, -4135, -6088, 3849, -5156, -8308, -4143, -8641, -6975, -3564, -1360, 4376, -7087, 326, -65, -6693, -1427, -3501, -1020, -964, 3479, 3696, 2024, 6125, -6830, -8020, -753, 6450, 2358, 2681, -2356, 2071, 2145, 8112, -1805, 2883, -2971, -6905, 6556, 900, 6366, 7838, 3643, -8701, 5507, -6991, -6038, -5098, -5389, 9946, -2473, -6555, 3535, -2758, -624, -7468, 1830, -2800, 4670, 3877, -390, -3667, -5464, -1792, 8728, -6349, -5614, 4798, -2751, -7270, 7836, 6704, 3482, 7770, -9686, 1079, 3423, 4808, -2277, -9956, 2452, 4419, 3086, 3616, 1267, -3432, 2520, 7909, 4547, -1950, 1048, -8994, -6758, -1987, -8210, -7229, -5420, -8319, -7659, -7581, -7294, 9272, -5721, -8346, 297, -7888, -2241, 6699, -4389, 7644, -4255, -1635, -2700, 2156, 7914, 3079, -6332, -9158, 1595, 6375, -7794, 2336, 8800, -7849, 6628, -6993, 129, -5896, -3155, 1610, 5509, -3941, -4422, -3406, 9865, -6537, 4141, -5952, -3772, -4717, -38, -5972, 4216, -2624, -3378, 9361, -974, -4541, -6345, -480, -6186, -6414, 45, 4865, -5843, -8215, 9140, -9580, 3774, -9631, 3058, 2031, -3557, 2126, -1714, -6632, 5039, 2, -14, 5414, 788, 9474, 5973, -4250, 8985, -3911, 6791, 7677, -6936, -1959, 9607, -4653, 1896, 1875, 4610, -857, -8034, -7627, 6119, 4451, 7407, -6949, 4268, -6473, 2528, -1529, -2781, -2959, -4166, 3170, 6117, 7008, 7709, -3728, 1049, -1802, 2455, -7391, 2422, -5720, -2309, -3975, -3036, -2508, 8147, -4901, 6639, -1532, 4344, 6458, 1109, -2792, -9716, -8801, 5034, 5954, -1941, 3527, -9010, -8861, -2532, -9638, 6809, 7809, -5209, -8834, 1529, -8510, 1410, 1266, -6559, -503, 1096, 4285, -2594, -5418, -4938, -5395, 1906, -7145, -8367, -9856, -2153, 7965, -6672, 3313, -9612, 5847, 7722, -996, -9868, 7934, -5262, -1988, 2709, -1582, -7013, 6972, 4444, -2676, 4386, -5369, -7578, 9331, -364, 1531, -4974, 5742, 560, 6601, -9548, 8204, 2441, 6888, 7949, -571, 1266, -866, 9162, 8627, 1569, 8396, -1577, -4275, 2582, 4987, -5208, -7109, 1679, -3832, 4185, -5267, 989, -5777, 8852, 360, 5474, -6766, 8288, -2981, -3528, 9766, -2046, -1049, -6048, -7776, 1005, -9812, -588, -5015, 1912, 8260, 284, 1783, -7515, 3021, 3669, -9656, -4642, 9198, 2674, -8610, 482, -3957, -2530, 6112, 7291, 3928, 1811, 2600, 902, 3998, -7356, -162, 3421, -3031, -9496, 8616, -6702, 4206, -7200, 6873, 5423, -9664, 5661, 8476, -4374, 5053, 2219, 7794, 6014, 725, 8295, -7949, 1948, 7869, 4417, 14, 9648, -5671, 3899, -739, -6455, 6751, 599, -852, -8110, 9864, -643, 8887, -7795, 6880, 9808, 264, -2123, 4195, 4979, 3389, -205, 2029, 970, -3776, -3591, 8700, 2393, -3009, 3777, 5748, -6368, -6041, -9279, 5777, 9579, -2856, -5060, 9929, -2132, 1259, -5649, -6479, 6402, -2741, 1910, -3708, 6737, -7689, 2705, -5979, -2599, 8299, 3524, -5447, 1716, -5267, 2994, -1728, 7478, -8590, 1270, -2792, 7139, 822, -3530, -5904, -6924, -9156, 2661, -3625, 6403, -9005, -2958, 1808, 9557, -3879, 8674, -3267, -7958, -8507, 2160, 4062, -9927, -5010, -6814, -9703, 1944, -3272, 1865, 4796, 5678, -8156, 4833, 417, -6389, 1343, 3672, -7321, -969, -8999, -6931, 6126, 1970, -6027, -8480, -8270, -1972, 1590, 1425, 5704, -8682, 2157, -2082, 660, 1195, 4310, -5550, 2091, -9968, 2578, 6769, 1437, 440, -7840, 9315, -3763, -1922, -4240, 7074, 5376, 8852, -8674, -6462, 7289, -5633, 913, -4959, 5276, 7641, 7275, 9503, -7088, 914, 9296, -2145, -283, 4864, -4852, -8613, -6181, 7296, 5843, -7107, -1802, -4958, -8278, -4160, 720, 6565, -2285, 7433, -9005, -6964, 8702, 1152, 4948, -6855, -3010, -7881, -4776, -8128, -9264, -8066, 8977, -7316, 1918, -4196, 6647, -323, 2050, 761, 2424, -3782, -3554, -2578, -4673, 7288, 9126, 5069, 9423, -4508, 9514, 6805, -1277, 6700, -2912, -7779, -8580, -9978, -9489, -9119, 5357, -3757, -8337, 6594, -2111, -1632, -409, 9151, -5903, 640, -8572, 8510, 8278, -1775, 177, 5642, 9391, -9114, -2234, -8843, -1501, -7751, -6200, -6780, 1431, 7199, -7487, -4260, -3474, 7126, -2587, -402, 7509, -7437, -6688, 6291, 3813, -7636, 99, -7842, -5496, 89, -1699, -6351, 5759, -7705, 5567, 2402, 3880, -9457, -7792, 1836, -8813, 1189, -7149, 6761, 2780, 8260, -8882, -9436, 3022, -5674, -4498, -9774, 8474, -646, 5053, 145, 5413, 8535, 3252, -8353, 3321, 1081, -1385, -5365, -6047, -2356, -5910, 9603, -6109, 8869, -3551, 1601, -4363, -2448, -3502, -5068, 2604, -9508, -1137, -8789, 4940, -6691, -2, 464, -1002, -7282, 3521, 8771, -1938, 4699, 4090, 2162, 2323, 6750, 2785, 2544, -6449, 2219, -3829, 6259, 1780, 1063, 899, 4819, 8725, -8673, -9396, -5349, 4083, 6646, 9158, -3588, -4784, 5195, 7399, -1739, 9470, -9868, -9712, -3869, -1360, -6509, -3343, -7929, -7800, -1856, 3867, 9455, -7420, 6527, -847, -8856, 9267, -5047, 6267, -9987, -6863, 5079, 4915, -9745, -2327, 8307, 4577, 8385, 8119, 503, 2778, -4069, -7478, 8888, -265, -1347, 9303, -9165, 2158, -3441, 4916, 6877, 5662, 6722, 3107, 3402, 3434, -7993, 5608, 3931, -2110, 3748, -383, 1096, 4957, -230, 5190, -8457, -9767, 4978, 183, -5780, -5609, -6276, -2001, 5479, 8156, -8859, -7438, 8985, -5350, -9088, -4246, -8140, 2758, -2362, -315, -468, 8101, 1003, 6709, 5944, -6538, -6381, -4776, -71, 9661, 5712, 4898, -8652, 8198, 632, -865, 4192, 6829, -3307, -1998, -502, -2560, -1493, 5983, -7498, -8128, -8201, -3060, 8193, -7775, 2166, -3173, -4101, -9562, 2217, 2033, 7651, 190, 2324, -6782, 2109, 4258, 3549, 3045, 6604, 1154, 5283, -2817, -2080, -2762, 1285, 1129, -9441, -4247, -1687, 2220, 4610, -2094, -9362, -4519, -218, 1382, -6700, 1854, 1331, -4027, -2029, 8172, 4328, 2833, 2161, -8765, -375, 9342, -7687, -3716, 3388, -8416, -3407, 1994, -1054, -8376, -186, -7271, 4599, 7332, -1995, 4709, 996, -6963, 6797, 9088, -1249, -8211, -6384, 6434, 2880, 3774, -239, -1474, 9107, -5435, 9594, -9671, -8703, 9640, 765, 5795, -9081, 697, -6034, 7177, 7337, 8832, 5065, -807, 7968, 4276, -4664, -7892, -5075, -4077, 4183, -6134, -2903, -5514, -7264, 116, -3832, -1973, 1652, -6085, -5003, -7838, 3468, -4264, -3023, 314, 8063, 3852, -7149, -8681, 1692, -5346, -4162, -8622, 8072, 6992, -7112, 5145, -2416, 3921, -6026, 1000, 8068, 3745, 7850, -1618, 5327, 3419, 3368, -3189, 412, 5938, 8502, -369, -4383, 6547, 6813, -9567, -6567, 4422, -7625, -5221, 4323, -986, -2809, -3454, -3588, 5577, -8783, 6524, 9133, -8837, -5967, -7528, 6905, -4990, 6840, -2310, -2607, 4608, 8761, 4254, 4060, 7645, -5188, 9013, -3597, 6219, 1762, -4302, -9519, 7977, 9002, -6687, 6309, 2184, 900, 8915, 5217, -8330, 8914, 3365, -214, -9000, -642, -9239, -7938, -8479, -7119, -9830, 5591, 9733, -3346, -931, -1727, 366, 2742, -4368, -4832, -1086, 3392, -9452, -284, 6502, -529, -8542, 4954, -3823, -9625, 4214, 616, 571, 366, -3392, 1904, 7482, 8260, -1265, -7530, 3379, 669, -6443, -2175, -478, -8506, -7683, -4813, -6343, 8458, 5993, 478, 7359, 2722, -1790, -9197, -5374, -2865, -3476, -3102, 308, 6193, 5500, -4060, 503, -1725, 5130, -2165, -5830, 8200, -2136, 6484, 9700, -6866, 4810, -9149, -1641, 9453, -9283, 7437, 5303, 8899, -2662, 5308, -4197, 5652, 9522, 5618, -8870, -711, 7737, 8309, 7322, -1247, 174, -5701, 830, 1894, -2877, 3488, 9657, -6662, -1134, 6996, 8960, -2702, 3780, 3256, 6803, -9358, 8888, -1002, -7415, 672, -8862, -9672, -6583, 1023, 7399, -4095, -8905, 3832, -9919, -7099, -8650, -5149, 6103, -3042, 3500, -9054, 2771, -9839, -3265, 7396, 7919, 7798, 7640, -4554, -9433, 2296, 3625, -5672, 9492, -5183, 4608, -7452, -4482, -5939, -9435, -7468, 8989, -3858, 741, 9533, -4029, -2423, 9658, 2147, -8574, -6730, -6589, -8311, 812, -6409, 5790, -3925, 477, 5133, -7665, 4460, -4042, 493, 7013, -307, -8657, 2740, 9745, -7716, -742, 5502, -8269, -7133, 5796, -987, -1618, -7442, -1141, 4702, -8076, -7138, 6782, -6284, -472, 2365, -3135, -3595, 7539, -1550, 3295, 9466, -7801, 1865, 2471, 309, -1353, -6711, -5563, -6954, 6464, 1800, -9747, -5523, 2057, -2346, -9200, -7701, -2452, -3227, 7416, -8773, 1069, -6047, 4967, -8494, 2554, 220, -5025, 6633, 7939, 2459, -7623, -8856, -1639, -7965, 7273, 3263, -6192, -2358, 8528, -4412, -4559, 5015, 4319, -1360, -3405, -2724, 5978, -4659, -9631, -8179, -1239, 4991, -8497, -7255, 2205, -1828, 4320, 723, 4656, 1604, 7656, 6325, 3856, -522, -8232, 224, -2146, 4283, -384, -5973, -152, 2243, -4792, 8310, -4405, -3169, -9542, 4546, 35, -2162, 6623, -5255, -2952, -5346, 2448, 7186, 9980, 3279, -8175, -8399, -2422, -3559, -5253, 8030, 4090, -9646, 6554, 5241, -3364, 4851, 3512, 2887, 3928, -3072, 9048, -7090, 7096, 8788, -3386, 9963, 9797, 8603, -4353, 9825, 9528, 3984, -1194, -9715, -9302, -5051, -8397, -1886, -4030, -5991, -7447, 6709, -7585, -4694, -2856, -2725, -846, 1373, 5644, -1647, 6628, -8463, -129, 4734, 8913, 7942, -9286, -3851, -1650, 9056, -6151, 761, 1609, 8909, -9728, 7145, -4663, -7920, -7362, -1496, -4591, -6057, 7762, 2821, 0, 2533, -2151, -1610, 2047, -6085, -7678, -3108, 6834, 5492, -4592, 9194, -1050, 4954, 9813, 8779, 7118, 7673, -5609, -1741, 4640, -7488, 6148, -7008, 5482, -7911, -3256, -4091, -9470, -5808, -1471, 1870, 5314, 1072, 7572, 3661, -4887, -5627, 1609, -8376, -8266, 674, 1338, -5520, 4107, -5729, 9773, 9713, 273, -4252, -9156, 5951, 6792, -7773, -3500, -3680, 1953, 3611, -8750, 715, 6751, -4218, 8123, 5853, -578, 9709, 3973, -4139, -1195, -8731, -133, 5547, -7397, -7464, 7362, 2443, -2056, 7645, -8658, -8346, -5742, -6454, 8797, -4346, -6312, 9851, -7258, 3440, -1547, -9918, 8371, 3419, 5501, 4123, -147, 5111, -2138, 9588, -8225, 6327, -5873, 1800, 2247, -1479, -4125, 7960, -805, 8815, -6538, 4985, 4591, 7792, 3018, -5764, 3803, -7954, -7866, -9503, 5811, 7081, -281, -8460, 2376, 1643, 8346, 9159, 3625, -9040, -3558, 2283, 1432, -5090, -4711, 4129, 9708, 1879, -4729, -4499, 2707, -988, -5462, -1827, -2850, 8376, 6827, 5465, -7901, -7700, 1819, -8370, -58, 7439, 8180, -2241, -4531, -9627, -7927, 3562, 4196, -3331, 1494, 5100, 867, -1344, -6936, -2407, -747, 6863, -8514, -1875, -8603, -6026, -8037, -1797, 4157, -533, -4370, 3164, 2193, 744, -5713, -4250, 7064, 4396, -6248, -6291, -9789, -5837, 8650, -5075, -4007, 8205, 3579, -5374, -5439, -7489, 6335, -5537, 1947, 6594, 4973, 2625, -8935, -3023, -1250, -1904, -9796, 4197, -7157, 3872, -2080, -4944, 7491, 1306, -7899, 6761, -7449, 1521, -4489, -5534, -6610, 9112, -4288, -1017, -1951, -7199, -223, -1407, 6331, -5527, -8433, -806, -1660, -8136, 4093, -9459, -1623, -9706, 5690, -3081, -8017, 8774, -1311, -1716, 9100, 3216, -3785, 3689, -9375, 2334, 7663, -8657, 1675, 5997, -8626, 3513, -8326, -4618, 3403, 2295, -8716, -4222, 1429, -3909, -2108, 2874, 4529, 8154, -287, -5503, 1156, 366, 5401, 7177, -4750, -7621, -1413, 496, 9126, -2739, 2275, -4201, 3679, 5190, 6711, -9341, 9968, -3794, -7443, 2898, 5520, 7638, 8205, -4912, -548, 4918, -8827, 438, 9577, -8188, -7560, 889, 9009, -9348, -4581, -6243, 9685, 2159, 3954, 8231, 117, 786, -7782, -3348, 8357, -6521, -5274, -7667, -924, -4450, -7308, 902, 8965, -3572, -6975, -741, -2587, -462, 4487, -2106, -4224, 1728, 6967, 8722, -5317, -9508, 9652, -2871, 6892, -5456, -5579, 292, 8512, -1290, -8889, -1284, -4471, -9483, 1982, -5379, 8976, 3023, -680, 4082, 3845, 9816, 6720, 4514, -9463, 8601, -504, 6797, -3707, -20, -7898, -181, -1785, 49, -9638, 5631, -930, -8475, 5190, 6232, 9423, 1479, 2573, -8441, 9952, 8424, -7796, 5774, 34, -9963, -6491, 9444, -3933, -9983, -3962, 8083, 822, -3387, 4812, -7319, 6433, 3378, -850, 7397, 8224, -8765, -5113, -1255, 3062, -5320, 7672, 495, -6985, 577, 4303, -323, 3621, 666, 4401, -1735, 5005, -2135, 1556, 6570, 703, 2101, 1477, 3895, -7742, 8489, -1991, 3659, -6766, 8207, -6162, 9975, -6643, -4618, 2634, 8646, 2755, 4113, -9300, -3444, -6643, 8069, -6915, -2333, 4281, -5264, 3871, -7198, -6301, -8218, 8918, -9037, 1141, -6908, 5443, 2149, 1344, -6230, -31, 7189, 1544, -5621, 1220, 267, -5878, 5838, -8234, -2324, 7985, -1934, -5703, 9176, -8810, 5145, -6292, -17, 6696, 6685, -2821, -8738, -3084, -4572, 5051, 7294, -3283, 1296, -9956, 9931, -6692, -6969, 4407, -6029, -1698, -6817, -3559, 8596, 9630, -4472, -9410, -2485, -2888, 4566, -4578, -4914, -9174, -4019, -6394, 3070, 1195, 31, 9888, 5489, 3239, 2919, -2094, -7410, -4068, 8787, 427, -6601, -5174, 2381, 5869, 7709, 7527, 8673, -8126, -718, 9515, 6610, -2091, -1634, 4092, -448, 1774, -5076, -7023, 9544, -2903, -1183, 3101, -3737, 3799, -5836, -4930, 7322, -224, 511, -7419, 6468, 4956, 3511, -5748, -4652, -3653, 2760, -7247, -9686, 9079, -126, 3704, -5154, 8357, 8119, -1320, -6101, 4745, 629, -858, 6913, -4548, 3305, 9981, -7727, -9878, 9925, 8950, 4747, -1625, 9886, -2918, 6489, -9403, -5221, -2929, -4480, -402, -8320, 2302, 337, 671, -281, 1954, 4101, 5958, -8716, 6545, -2750, 3630, 1915, 61, 1295, 8133, 5953, 7520, -5207, 5174, 7855, 3704, 179, 5530, 4881, 1425, -4991, 7747, -8197, 3413, -7736, -4543, 7108, 8607, 7913, -7720, 9750, -6627, 2112, 5767, 2641, 9515, -6231, 8461, 7901, 7326, -1655, -6769, -1774, 8588, -3263, -9913, -9668, -1325, 9319, 4737, -5592, 4406, -5649, -1223, -8625, 8354, 7806, -7814, -9186, -9076, 2420, 6754, -3748, -3113, -8655, -106, 6367, 3530, -802, -9677, 234, -860, 5007, 3250, 7950, 7862, -1058, 4684, 583, -7077, -3158, -3654, -4264, 2117, -1517, 3098, -7164, -667, -1519, -5532, 5023, -3495, -2033, 5079, 7278, 1187, -1554, 6183, -897, 1093, 8946, 4634, -6987, -9528, 6923, 4822, 7560, -189, 1091, -9399, 2963, 8664, -9270, 9507, 9739, -8912, 8147, 5655, 6760, -2633, -7525, -7288, -177, -5645, 7700, -3832, -6869, 3332, 517, 7855, 7365, 4303, 6619, -4747, 1595, 5180, -5948, -8342, 3998, -5096, 8370, 5523, -6841, -2148, 9537, -1087, -175, 6000, 5713, -9894, -1700, -5717, -1603, 5295, -7776, 1315, 2050, -3924, -4575, -3412, -9506, 1050, -4760, 1511, 1616, 686, -1196, -9924, 303, -1805, -4473, -4240, 2484, -8348, 3968, -4591, 1195, -9040, 1430, 3844, -3283, 1211, 1120, -5108, 2244, 4588, 1700, -4108, -5926, 6881, 2475, -5468, 9597, -5097, -1104, -9482, -217, 4934, 5948, 5868, 5214, 1408, -9555, -9315, 3237, 1099, -9168, -6291, 3845, 692, 435, -6360, -6197, -8802, -1300, 6779, -1698, 9947, -7840, 9005, -2987, 8734, 4975, 9970, 1501, 3574, -1889, 5222, 6490, -200, 1280, -8565, 1850, 8982, 1081, 2630, -8986, 6, -8365, 8340, 3822, -3279, 4139, 2997, -7529, 188, 9185, -3378, -3084, -5338, 9331, -7592, -486, -2787, 7594, 2132, -9186, -7565, 1427, -1899, 6203, 4532, 9228, -2038, -2869, -5780, 2432, 534, -7664, -5095, 2878, 6462, -6839, 5006, 7073, -9137, 8327, -5494, 6773, -7965, 5333, -8692, -3906, -4123, -6204, 1471, -2319, -863, 1689, -2958, -1776, 1517, -6534, -8983, 6723, -8739, -3617, -7649, -487, -6305, -8585, -3864, 3277, 5954, -7063, -3421, 1460, -132, 7215, 298, 7155, -3282, -4435, 443, -9047, 6235, -6026, 2777, -8750, 7103, -4938, 1299, -3664, 2847, 9278, -410, -3747, -1455, 4306, 5613, 3653, 6000, 1563, 7393, -6322, 6638, 5364, -751, 6246, 9500, -3220, -1708, 1352, -9455, 8062, 922, 7412, 8863, 4609, -6634, 9298, -1218, -2855, 6198, -6985, -2589, 4044, 9799, 4886, -9700, -5086, -3061, 7689, 793, 9187, 7764, 348, 1176, 9954, -4726, -2321, -9023, 5336, 9791, -4323, -8105, -2716, -1438, 6715, 4039, 1388, 7450, -7320, -7714, -9102, -5062, 1531, 8770, 1457, 5097, -769, 3087, 6822, 3127, -4827, -579, 7353, -9811, -2271, -7458, -3855, -6876, -4792, 6984, 5735, 4767, -4791, 9519, -5364, -3696, 4374, 7692, 1160, -6277, -6823, -1637, 6933, 8963, -4266, 1394, -2669, -7350, 1683, -4624, 7758, 5309, 6942, -4254, 6914, -1571, -897, -2731, -6934, -4972, -7555, 2214, 7330, -4835, 2589, 6509, 7413, 1067, 5081, 5829, -2856, -6791, -2212, 9658, 8383, -518, -4610, -1059, -8321, -5731, -415, -5943, 874, -220, -9810, 7351, -2743, 7976, 2845, 1027, 8838, -4130, 9109, -8338, -8363, -8876, -328, -5886, -6420, 8816, -8690, -9600, -7069, 4063, -6255, -2671, 2891, 5605, -3495, 2894, -4691, -9110, -5430, 2925, 5933, 3808, 6818, 8494, 5034, -2796, -1265, 7705, 1508, -4807, 2931, -7977, -2715, 8247, -9447, -873, 2336, 755, 3351, 2428, -6324, 1976, -7544, 6283, 918, 5875, -4430, -5833, -4401, -2935, 5430, 3766, -5421, 4205, 7732, -3340, -9253, -3738, -2703, 5534, 1250, 9862, -9802, -7955, 4518, 9750, 6721, 5017, 5270, -5607, -186, 7991, 5248, 3528, 2612, -3635, 9307, 5311, -9372, 800, -9531, 7269, -361, -1290, -1071, -3650, 6985, 8483, -5755, 9136, 3940, -3969, 1305, 8999, 1381, -2693, 7128, 3683, 1770, -7760, 3655, -6398, -2652, -4095, -3824, -1102, 8209, -6338, 8843, 5900, -8747, 8798, -7789, 4840, -2829, -4082, -175, -5876, -7678, 8595, 2193, 5155, -1631, -7716, 4497, 1060, -4586, 3584, -260, 6899, -6911, 7643, -2778, 2143, -2219, 7080, -2657, -597, -9491, -8253, 5845, 205, 8132, 8642, 5133, -4139, -5645, -5953, -6375, 1102, -3241, 7185, 3195, -9950, 5902, -8644, -8003, 9723, -6741, 8131, -3541, 2933, -8752, -545, 6218, -7721, -4835, 4977, 2979, 4186, 1104, 4780, -2768, 3727, 9728, -9128, 3607, 365, 737, 4069, 9141, 5394, -7941, -4307, -3519, -452, -7010, -8306, -7970, -5535, -7797, -4080, 2574, 2340, -4218, 6166, 6112, -3770, -7925, 6082, -5023, -8238, 1245, 640, 9072, -7246, 4475, 5941, -5487, -9794, 3443, -944, -9915, -1968, -2306, -8991, -7113, 6126, 4081, -9307, -5557, 7561, -7412, 7270, 3543, -3388, -2588, -7305, -2144, -2412, -2925, -2206, 9968, -3861, -9889, 6828, 2453, -5791, 9162, 5605, 6174, -8154, -9909, 5295, 9611, 5190, -8012, -4704, -9214, 2139, -7457, 5231, -2355, 9580, 9511, 7701, -987, 9151, 4997, 4053, 3136, 6336, -5685, 7845, 3092, -666, 9927, -9895, 2547, -7128, -7245, -1579, 8464, -8335, 9889, 5044, -3796, -8560, 9929, 3727, -3601, 9257, 2635, -7866, 1104, -2709, -3535, 146, 6068, -9974, -8648, -8857, -8677, -305, -9918, -6628, 2881, -4383, -7198, 5408, -8611, 6050, -7436, -4909, 6243, 3194, 9759, 6056, -6553, 2182, -7472, -8591, 3939, -5656, -5920, 8224, -4717, -8833, -6636, -583, -3969, 3264, 484, 5251, -4186, -5122, 5418, 488, 5317, -7059, 9852, -9987, -1461, -9693, 2722, 3780, 3362, 9751, 9100, -7041, -9612, -3042, -7394, 3391, 1996, 4906, 343, -4407, -5171, -8566, 5153, 300, 1263, -7742, -657, 8858, -3036, -1483, 9845, -2428, -7243, -3677, 5206, -9874, -5383, -4789, 7438, -3455, -2254, -2952, -195, 7307, -4581, -6290, -2798, -6712, -1549, 4611, -2855, 4637, -9042, -7417, 4473, -5684, -7766, 3859, -6016, -5731, -1085, -6946, -5543, 7426, 9355, -1247, 2833, -6794, -6408, 2057, -5254, -1276, 3579, 208, 4607, 6247, 5235, -6163, 9004, 2022, -707, 4687, -3679, -6549, 2927, 7927, -7520, 1662, -607, -5259, -296, -8585, 1388, -8502, -4173, -6167, -8532, -6141, -2451, -7587, 4331, -3382, 195, -2545, -2592, -9589, 7514, -8059, 4997, -1775, 9756, 2733, -8700, -4362, 6572, -8138, -3165, 3028, 1977, -4221, -628, -3827, 6498, -7376, -3221, 3023, 7630, -6509, 5941, -2905, 4571, -1445, -9910, 1199, 1509, 7736, 3394, 7797, 1223, 9605, 3377, 2740, 7428, -9236, -4654, 6299, 6462, 7260, 2313, -8919, 5512, -5735, -4162, -8166, 4932, 551, 7026, -9169, 3325, 2417, -6676, -4933, 7249, 9512, -9087, 7740, 4075, 4814, -8077, -8117, 352, 6542, -9677, 5604, 9556, 4324, 5578, -1537, -1973, 6841, 5928, 3094, -9224, 4716, 344, 2453, -3887, -8560, -8768, 4963, 9102, -2222, 2346, 5403, -3277, 7895, 9949, 1700, 4764, 9794, -95, 811, -8031, 7012, -3788, 3664, -3590, -7925, 1285, -8209, 8885, 6574, 799, 2296, -2090, -1186, -4374, 3907, -3614, -9430, -7365, 1893, 1769, 9101, -9988, -4894, 1660, -6461, 216, 7830, -7697, 9668, 1097, 1318, -5220, -1771, 6354, 2834, -9183, -390, 7664, 6783, -4581, 1724, 5139, 5953, -3709, 1875, -1729, 6068, 1384, -2766, 1385, 7353, 6215, 4842, -4998, 6595, -5934, 6570, 6838, 5035, -6771, -1814, 9987, 7537, -6334, -9419, -7935, -9764, 1693, -3858, -8152, 3140, 4191, 8859, 9348, 1188, -7907, 6517, -344, -9275, -415, 3810, 9049, 7077, -6814, 199, 44, -4421, -8001, -4626, 7179, 2450, -5416, 4509, 1602, 5674, -4365, -6625, -5650, 2564, -7494, 1163, 3980, 450, 4408, 9940, 2361, 610, 9163, -3859, 6998, 5195, 4292, -3934, -1727, -7471, -4936, -8908, -7402, -7584, 833, -5381, 5224, 5224, 5866, -1705, 516, -4836, 7905, 123, -4799, -9480, -7569, -4555, -4992, 4551, -6933, 3891, -6498, -8473, -4966, 6986, 2549, -6179, 1012, 5393, 1892, -2567, 2150, -3426, 9609, -2868, -3771, 8623, 7391, 9063, 3594, -1228, -4852, 5028, -4239, -7535, -1278, 4509, -7425, 29, -1981, -7153, 4680, 3756, -2489, 9350, -6, 9914, 2259, -7875, -8381, 8321, 4771, -3456, 2422, -3025, 3419, 1963, 3608, -9919, -3447, 2051, -619, -2838, -7750, 5935, -629, 6705, 3636, 1148, 6087, -6055, -9239, -8513, 2566, 586, -9334, 5066, 4704, -5280, -1696, 2002, 6506, 8282, 6199, -5107, -7503, -9701, -3982, -4201, 3978, -3135, -7265, -3505, -287, -6248, -5778, 3905, -7347, 7262, 3464, 2110, -6036, -9598, -7481, 2920, 2272, -7668, -3332, 8965, -6001, -5177, 6348, 7008, 9589, 7329, 8133, 4220, -3727, 519, 8374, 4346, 3081, 3825, -6508, 4244, -6320, -8037, -5789, -9073, 7720, -8757, 4154, 7696, -189, -1335, 842, -5730, 4230, 4997, -6493, 368, 6128, 6177, -2897, 7307, 4764, -7316, 7390, -5460, 7239, -778, 6597, -2093, -6841, 4802, 9998, 4783, -8767, 5010, -5130, -5558, -7888, 915, 4862, 262, 317, 9384, -4864, 7876, -1735, -9126, 8729, -3134, 14, -491, -549, 4812, -7184, -9852, -550, 8528, 8614, -2744, -1956, 3649, 6388, -1952, -5838, 5873, 4422, -4886, -5434, -263, -5784, 5543, 5977, -1728, 5442, -3521, 6286, -4359, -9552, 762, 5692, -8785, 4817, -6578, 3108, 1203, 5852, 4548, 4867, 9821, -9709, 3033, 1278, 8761, 6728, 5125, 9141, -4713, -3628, 9968, -4065, -6165, -4058, -7908, -7858, 6013, 8614, 5212, -4802, 4428, -9952, 1819, 4155, -7452, 5252, -5189, 7275, -4800, 8815, 2739, 71, 8633, -1543, 2805, 3357, 3237, -5245, 8685, -3116, 604, 3104, 6679, -5001, 5791, 9877, -9759, 4526, 1676, 2474, -4688, -6996, 5190, -2603, 443, -4254, 3678, 4662, -9913, -7486, 3842, 9839, 8554, -2759, 2622, -8110, -2295, -4108, -9782, -7458, -5621, 2767, -6543, 7678, 8968, 9511, -9630, -3699, -5540, 877, -4025, 2153, 6496, 6281, 4761, -9495, -2686, 6476, 9452, -8914, -5408, -3864, -9577, -3519, 7249, -4358, 527, 5171, 3031, 9685, -6965, 6423, -8090, 819, -3989, 8375, -2865, 5118, -2909, -5234, -6507, -3859, 8114, 2189, -335, -6319, -4567, -5659, -6063, 441, -5117, 5292, 700, -7240, 9550, 3167, -3935, -8813, -3518, -9483, -8629, 7417, -4127, 614, 2574, 5606, -3135, 1072, 8424, 3481, -7383, 9531, 8494, -8626, 5455, -6751, 6088, 4248, 6884, -8034, 6085, 4403, 4354, 4564, -3409, -3894, 1250, -3947, 6400, -5387, -6030, -5242, 5412, -4432, 9131, 3693, 2511, 6781, -2531, -2677, -9772, 8831, 851, 9429, -7179, -8862, 9789, 4416, -5024, -9839, 9129, -4837, -8726, 8197, -3054, 8462, -1387, 8321, 5617, 7868, -1974, 6260, 3087, 9184, 8369, 1439, 2544, 2656, 4231, -5647, 182, -7756, 8101, -4567, 7283, 3891, 9988, -386, -2339, 3272, 8963, 7890, 4806, -1682, -3942, 7536, -5307, 9642, -2760, 6895, -3700, 6548, -4494, 9559, -6867, -7329, 1081, 4935, 3801, -140, 1339, 3298, -3538, -1720, -1084, 8988, 4252, 3112, -5436, -8228, 2476, 4994, 8551, -5572, 4499, 2641, -9148, -3051, 7884, -6461, 9714, 4359, -6141, 2287, 4897, 1207, 4149, -9031, 5071, 1480, -4058, -482, 6187, 890, -1517, -1506, 2883, -2209, -4222, 5606, 3448, 8225, 251, -4978, -4613, 340, 5158, -1376, -7757, -3520, 4287, -3513, 6117, 2960, -8571, 1653, 8341, -3153, 6863, 8973, -9814, -957, 7116, -9716, 97, -7159, -393, 3512, 1631, -2206, -4407, -9694, -1968, -4053, 2010, 2347, 4333, 4055, -4537, -5420, 4480, -2650, 6400, -6, 926, 4715, 5991, -5341, 3984, -5660, -148, -5168, -2060, -9398, -4336, -4246, 4115, -3439, 2610, 5848, 9126, -9980, 8987, -6331, 2443, -336, 1723, 5657, 4425, 551, -2372, 8991, -1612, 3862, -5006, 1134, -7806, -6702, -7830, 3822, -1887, 3156, -8823, 9823, 9343, -4439, 3937, 7057, -2057, -575, -1014, -4970, -5838, -9960, 1788, -564, 2295, -2854, 7455, 8105, -3777, -3577, -7861, -2990, -1650, 9615, -5664, -6385, -9029, 1845, 7989, 1203, 3282, -3741, 1670, 9605, 8263, -5047, 7582, 477, -7997, 5582, -7499, -2294, 8780, -2461, -4760, 972, -518, 1567, 9428, 870, 4797, 9291, 7928, -3911, -6368, 394, -3742, 6701, -6989, -1027, -2445, 4132, -7558, -3777, 6718, -9109, 3997, -6954, -4115, -6213, -9488, -5680, 2995, 9431, -64, -5799, 9081, 3971, -1857, 3633, 9959, 5514, -9930, -1340, -4379, 3808, -2385, 21, 4999, 1206, 2458, 9611, 5571, -1695, 2716, 9577, -6319, -7936, 8239, -2137, -7833, -759, 1109, 2053, -4346, 1824, -8672, 3698, 9992, -5302, 4473, 1446, 8522, -1662, 4759, 9392, -1027, 5513, 1420, -3212, 3178, -2084, -8478, 6873, 5257, -6148, 5606, 2654, -1544, -239, 2989, -513, -7251, -9652, 4682, 2376, -2361, -7609, -9840, 3751, -9768, -2624, -2812, -8604, -6327, -7855, -4541, -9927, 9194, -1003, 56, 4828, 2205, -8049, 9793, 3232, -9051, -3859, -8029, -4649, -8307, -7663, -6774, -194, 4414, 7301, 2148, 3100, 1384, -333, 5825, -9089, -9889, -5421, -235, -6366, 9501, 4441, -7715, 9161, -575, -1366, 4500, 5053, 1915, -2439, -2972, -8998, 3266, 1920, 6488, 1344, -2134, -3454, 5578, -3699, -5072, -7264, -3911, -9934, 59, -3157, -2127, 1734, -7014, 7744, 5356, -5800, -9474, -2320, 3023, 7076, 5427, 3993, -5187, 8210, -8029, 5665, -903, -9465, 5435, -830, 244, 5527, -8013, 9188, -6264, 3250, 2950, 5700, -6850, 4689, 9389, -8123, 8331, 8827, -8949, 2430, -271, -543, 4542, 421, -1800, -2158, -4988, 2465, 4579, -6108, 3014, -5434, 2638, 1, 5934, 9924, 4002, 3988, -7024, -4982, -3453, 3127, -7835, 1283, 4964, 720, 6252, 9316, -1001, -463, 5561, 1048, 3286, -5826, 7626, -7687, 9964, 9843, -9195, 1843, 6631, 9466, -8610, -1196, -9881, 8672, -6836, 9332, -406, 8490, 2357, -9047, 7841, -2866, 7462, -9535, -8593, 4295, 8189, -1750, 1314, -839, -1860, 9499, -4687, -4189, -9096, 1303, -8422, 1266, 4366, -3094, 4373, 686, -7741, -2028, -7229, 5124, 86, -6979, -4592, -3977, 8705, 8129, -9233, 5521, -7937, 536, 9450, -3656, 6262, 4637, 3226, -5199, -8796, -9281, -878, 5366, -2373, -4219, -7768, -7183, 8888, 4374, -7644, -7989, 9542, 7976, -8214, -8932, 9051, 9262, -7662, 3020, -1013, 7625, -6439, -1569, 2349, -298, 2895, -1982, -1467, 3159, -1946, -439, -420, 9479, 9390, 3662, 8465, 3431, -2640, 420, -6367, -2371, -3359, 4595, -8063, 3059, -597, 8037, -7661, 3877, 4879, -8155, -8571, -3863, 5565, -8398, 492, 8669, -8774, -7451, 221, -205, 3101, -1333, -6520, -3086, 7671, -7822, 8278, -504, 2954, -145, 6715, -932, -244, 6505, 5773, 9622, 399, 9962, -2263, -7005, -3488, 8735, -4963, -3790, -3125, 9429, 8104, 1432, -9115, 34, 3424, -2483, -4234, -7591, 1928, 4162, -9011, -5368, -653, 8053, 7874, 2903, 1972, -1850, 6306, -8582, 1428, -7862, 7373, -8047, -614, 2589, -3002, 2964, -143, 8352, -1348, 2529, 5392, -4031, -2871, 7991, 4247, 6421, 4521, 3724, 2264, 1559, 9392, 7870, 9390, -7059, 1561, 7001, 9527, 7632, -894, -2217, 1127, 2284, -9973, -2939, 2561, 6088, 4090, 553, -6661, 7638, 5522, -8410, -1339, -8722, 2738, -8261, -6777, 8011, 9775, -1188, 9113, 9463, -7850, -1684, 1294, -5615, -9285, 896, 762, 859, -3535, 5875, 2554, 7086, -4883, -6548, 8334, 9741, 7578, 9406, -6527, 77, 6060, -8351, 5842, 4914, -2752, -516, 7497, 9803, 550, -2737, -7982, 9544, -4835, -3281, 7831, -2796, -8923, 7050, -826, -9522, -5454, 4983, 8079, 6218, 137, -3133, -7064, -8738, -6369, 1682, -9083, -3898, -5982, -2791, 9291, -3794, 3324, 8205, 6131, 3799, 7822, -8572, -4223, 520, 7912, 39, 9405, 4194, 6334, -1630, 4874, 4310, -7367, -3636, 9779, 3256, -9685, -9921, -9289, 7137, -9843, 6778, 3471, -3907, 2768, 1516, -1006, 250, 913, -6787, -6113, 8046, 8931, 748, 4458, 8066, -3821, -8772, -4971, -2057, 7418, 2880, -547, -2737, 4351, 9331, 7863, 3178, 3153, 976, -171, 5778, -4565, 9231, -7363, -2604, 7625, 541, -1755, 645, -5264, 4794, 8668, -9400, -9804, -4905, 9943, 9782, -1065, 2265, -9445, -6702, -5184, 4828, -7603, 4588, 9076, 5897, 7613, -1483, 5497, -9570, -3482, 3168, 1044, -4771, -7208, 6044, 5429, 7465, 6733, 7940, -7060, 9833, 2969, 4982, -5258, 1461, 5281, -1980, -599, -392, 2873, -1708, -3383, -2123, -3899, -2573, -1658, -1494, -5880, 4260, 5503, 7048, -3133, 1797, -2196, 5048, -7888, 2204, -5883, -2909, 5638, -7420, 8084, -8180, 3783, -5817, 4565, -7471, 34, -4339, 1986, 2858, -9567, -6002, 5510, -949, 2781, -8234, 4335, 9247, 2853, -9502, 5311, 8952, 6019, -5729, -8392, -8465, -9698, 9968, 7246, -1757, 9993, -7036, -6177, -558, -1393, 7424, -7485, 9222, 9865, 499, 8101, 1752, 2005, 738, 1494, -3029, -7452, 2957, 4120, -7352, -8442, 3290, 2815, 9262, -4937, 6578, -5904, -312, 5394, 2616, 8866, -1080, -7223, -2752, 7068, 1539, 8130, -5755, 5785, -8322, -8394, -4980, 7152, 6150, -8232, 730, 8477, 1372, -3656, -6300, 7415, 2161, -5831, -8850, 7724, -9238, 7898, -4413, 1586, -8347, -8342, -2706, 2884, 6920, -4334, -390, -1242, -2254, 2272, 8206, 7537, -308, -6867, -9826, -7876, 2597, -5053, -307, -5382, -135, 3707, -5462, -7400, 9281, 5752, 1029, 3597, -6622, -4747, 1819, 6676, 5601, -6486, -1694, -2957, -9478, -6178, 6952, 1378, -6746, 9215, -7852, 877, 2359, 6845, 3863, 1632, 3180, 4610, 2609, -6399, 7152, 9516, -4802, -3375, -3780, 9153, -6345, 7438, -8824, 3534, -834, 92, 9940, 699, -2685, 6734, -2450, -5802, -4107, -1146, 6575, -3300, -8411, 599, 1720, 1540, -2748, 3700, 5708, -8888, -6520, 9556, -6370, -9320, 424, -6082, -9706, 1523, 2409, -9781, -2460, 7437, -5342, -2136, -6463, -3792, -2325, -2096, -2961, -4344, -3705, -6959, 7457, 3822, -6416, -6599, 8933, -3521, -4076, 4957, -5681, -9139, 7210, -3206, 2091, -8920, 8817, 9068, 603, 7382, 8056, -5484, 740, -1571, 8768, -8771, -999, -1494, -8577, -7373, -4394, -8331, -3151, -7886, 2817, -8679, 7075, 7781, 491, -5816, 798, -4114, 9369, -7367, -192, -3426, 733, 6784, 2375, -9273, 5304, -9391, -3166, 5444, 4982, -9245, -8231, -4201, -7839, 1834, 5242, -1105, -1641, -3597, 4311, 6379, 8849, 6692, 6069, 5990, -1023, -4000, -1696, -4395, 5504, 4725, -2978, -824, -2474, 2030, -1220, -3924, -2827, -1361, 8221, 7941, -6730, -2952, -6159, 3663, -8885, -7286, -8069, -2958, 3134, 7075, -3798, -4710, -9533, 4607, 5355, -1873, -7098, -4333, -3262, 9407, -3861, -48, 464, -3441, -713, -395, -3481, -7397, -1102, 3586, 3684, -4607, 7619, -8191, -1610, 2115, -9195, -2548, 8501, -7886, -3493, -3111, -6285, 3450, 5582, 7459, 2647, -5418, -8348, -1856, -5962, -4075, 3229, -7340, -7138, 2004, -5692, 7238, 4375, 6399, 5819, 968, 2843, 4841, 8326, 7613, 9730, 5568, 3020, -2892, -8256, -5944, -700, 6537, 2738, 1139, -6195, -9005, 4534, -9278, -1628, 1083, 1104, -8085, -9809, 9896, -8831, 2574, -277, 9772, 5457, -8159, 8156, 6374, 7206, -4721, 1751, -6038, 4757, 228, -74, -1489, 403, -1583, -8301, -8486, -3798, 5937, 2368, -9506, -3901, 6190, -9738, -3451, 5397, -123, -5227, 3604, 2502, 3355, 5418, 8326, 1304, -1868, 7804, -6700, -4735, -9575, -7571, -9727, 8459, 7130, 5382, -1625, -2454, -6140, 726, -9113, -822, 3936, 8599, -710, 2226, -6034, 5599, -7249, -8010, 3556, 2983, -1238, 379, 9226, 983, 4645, -4451, 5834, -5888, 6222, 6614, -1709, 1821, 4790, -9933, -6054, 4653, -2640, 2910, 3166, -1434, 3611, -4104, -3539, 651, 4296, 373, -1222, 7754, -1531, 6411, -3071, 4588, 7973, 9956, 4966, 797, 6850, -729, 8792, 8613, -3713, -2643, 1702, 6892, -5633, -4078, 4542, 3668, -7114, -3983, -3633, -872, -6570, -3624, -4953, -5115, 1807, -6664, 6003, -2834, 3110, -2845, 2772, 3323, -25, -7826, 7700, 4147, -3947, 914, -5590, 4432, -3772, 5567, -1473, -9625, 4308, -4932, 7738, 3510, 6278, -8931, -7521, -7152, 6942, 6202, -6922, -772, 7872, 4208, 1151, -6294, 4298, -2720, -2378, 3720, 2105, 6548, -3297, -9534, -6478, 5044, 6968, 9332, 3825, 9641, -9413, 8938, 5878, 9710, 6926, -9285, 1711, -1415, -6039, -5396, 3435, -1812, 7228, -7680, 3750, 3919, -3527, 5281, -9238, 1441, -3398, -7473, -7969, 5225, -2858, 6962, -740, 4985, 5731, -8004, -7436, -6749, -7012, 6469, 9778, -8146, -3901, -5289, -3106, -9460, 3128, 8659, -3773, -9492, -5680, 8996, 843, 5387, -9638, 3679, 2266, 2520, 6145, 6133, 98, 4076, 9030, 1821, -9403, 432, 3163, 8653, 5401, 3052, 4942, 7038, 4612, -6147, 5848, -6601, 9839, 6015, -661, -9969, -3939, -8286, 2683, -8762, -5784, 4909, -8205, 5031, -431, 6963, -5495, -9400, 8844, 8109, -8270, -3322, 5747, -4188, 9887, 7985, -8370, -5684, 2839, -784, 7818, -879, -2571, -2768, 293, 8913, 7742, 5419, 6093, 7549, 7992, -4614, -3485, 5560, 182, 6590, 3186, -5279, -5411, -5487, -3598, 8717, -8486, -8714, -4106, 9396, -4638, -3449, -8872, 4658, 3274, -8611, -1552, 8693, -3221, 1926, -4063, 8313, 6861, 9308, 1016, -7526, 1191, 9996, -5661, -367, 7236, 863, 6658, -9093, 4932, 2700, -8804, -3751, -4938, -3066, 4027, 3627, 4530, -2871, -9910, 129, 9717, -1914, -8376, -98, -7285, 6353, 8643, -8123, 7722, -7843, -4709, 7663, -7122, 3750, 5765, -3405, -1675, -9988, 7716, -2945, -5665, 4751, 7080, -3064, 2027, -3684, -2084, -7577, -7853, 5640, 8924, -6664, -6290, -6943, -3268, -1409, 7064, 3121, -5777, -1726, -8466, 1329, -8665, -1355, -9445, 2132, 7592, 9126, 8677, -5308, -6504, -6050, 5199, 4032, 3377, 4070, -1949, -7933, 3315, 1038, -5648, 5701, -7203, 8621, 4141, 1124, -2010, -6654, -8220, -4548, 3604, -1200, -614, -4184, -2840, -6764, -4914, -4832, 2522, -7624, -2415, -9150, 7324, -9932, 9000, -8306, 1297, 9579, -4943, -1331, 9283, -6969, -3700, -2728, -7023, 6952, 8730, 9677, -8948, -2995, -8132, -6271, -7801, -8692, 9793, -25, -8452, -9758, 278, 4395, -1728, 6411, 4909, -2057, -9755, 4671, -1666, -5534, 7784, -7649, -6445, 1166, 7861, 1642, -9917, -2262, -728, -4663, -5998, -4741, -5309, 9035, -8849, 1308, 659, -9286, 1083, 2233, -6323, 7323, -5896, 9270, 7621, -5745, -6327, 4560, -5369, -9297, -5323, -6316, -763, 6743, -2469, -4476, 5281, 6982, -5656, -1815, 1207, 745, -2767, -8583, 1336, -1600, 3924, -5319, -9811, -2557, 528, -2673, 3739, 6482, 8426, -772, -87, -8751, 7121, -2623, -2470, 841, 9692, 3830, -4014, 1033, 2795, 6960, -182, -5523, -8532, 6022, 9871, 7873, 1761, -9880, -2364, -2121, 2916, 5486, -6091, -4869, -3088, -2788, 7286, -5005, 5885, 9234, 2284, -9301, 9939, 97, -9569, 935, 404, 4321, 863, 9700, 1503, -2678, -2852, -9786, -3707, 871, 1910, 9136, 2868, -9028, -4984, -1851, -18, 2234, -8671, 8206, 5274, 7460, -6651, -2271, -1872, -9695, 762, 5806, 6144, 9758, 1992, 157, 3218, -4869, -4561, 2008, -5213, 4597, 5491, 21, -9342, 6308, 772, 5502, -7329, -6100, 2031, 9020, 6505, 2804, 4091, -386, -5736, 4217, -6179, -9145, -5085, -1219, 2220, 3663, -6799, 4924, -3643, 6436, 7661, -5088, 3519, 2380, 6280, 162, 4966, -6475, -6806, 7711, 8548, 7056, -714, 4082, 7719, -740, -3265, 6211, 7850, -9317, 1180, -3781, -5788, -6766, 7791, -9599, -8097, -9442, -490, 8481, -7214, 6874, 4496, 5232, -1414, -8821, 334, 5276, -2970, 6938, 3328, -5282, 5385, -5107, -5471, 1611, 2852, 4504, -1553, 4821, -1073, -5779, -659, 7750, 37, -4220, -1452, 1068, 3541, -423, -7610, -5567, 799, -9958, 6645, -9546, 6699, -8900, -5652, -8706, -8714, 2536, -348, 7743, -7332, 2887, 7933, -8789, -6479, -2808, 4619, -6062, -7659, 379, 2233, -7246, 4590, -6078, 2994, 3874, -1504, 5025, 8079, 1525, 8261, -5904, -1081, -4608, -2266, -2799, 8191, -6758, -7287, -4090, 6448, -2357, -1187, 2377, -5607, 4042, 6965, -8243, -182, -6001, 3284, 4720, -2311, 1232, -7231, 1739, -1549, -3832, 9630, -8556, 5661, 8312, -2848, 8173, -2101, -1951, -2926, 2215, 1765, -76, 3274, 3465, 5524, -9557, 6109, -6667, 4215, -6257, -8329, -6205, -5684, 82, -7544, 7683, 9792, -7148, -725, 8979, 8978, 6244, 4605, 4447, -9257, 8299, -1243, 1302, 7226, -5399, 2393, 5997, 8531, -6983, -3062, -535, -8916, -9443, 2729, -4167, -6083, 1842, -6197, 7422, -5806, 2656, 473, 2096, -933, 4205, -4734, 290, 2913, 6345, -4447, 1490, -9573, -207, 9361, 2620, 4004, 9249, -8960, -1875, 8787, -3126, 5727, -2046, 3732, -4463, 2588, 690, 7304, -4345, 5607, 5391, 2767, 98, 871, 6145, 9163, -7288, -7897, -5940, -3311, -4141, 6605, -5241, 2938, -2726, 8500, 3266, 9474, -5690, 317, 3720, 962, -328, -9375, -9080, 123, 102, 4415, -5370, -9169, -1184, 9987, -9041, 5985, 2420, -3380, 2788, -327, 8929, -2062, 8558, -3420, -1604, 9604, -3412, -3594, 283, 5519, 5113, 4256, 4042, -7156, 5982, 6978, 1448, 2548, 9750, -5931, 9067, 757, -7401, -4045, 6657, 1868, 7419, 8497, -6599, -9467, -4973, -4877, -9482, -7297, 1677, 127, 2981, 1570, 4202, -84, 8723, -3776, 6948, -3213, 7736, 5667, -9485, 9123, -5819, -8332, -6112, 536, -9461, -6563, 6107, -5058, 4141, 5213, 7248, 5903, 1067, 7131, 4351, 1466, 8260, 9779, 3899, 2663, 2768, -1681, -901, 3108, -2128, 9324, 7714, -5708, -2215, 5417, 6309, -3498, -5722, -9065, 2679, -8232, -4273, -3224, 8473, 7956, 722, -4576, 5087, -4627, -8878, -9194, -44, -8441, -7187, 5893, -8339, -4182, 8578, -6052, 7871, 9459, -3832, 1671, -8382, 4703, 4121, 9167, 7661, 9962, 7521, -1682, 6311, 2877, 6346, -9416, -7256, -7262, -5503, 4793, 8289, 4997, 6494, 2176, -1126, -9367, 4454, 4645, 57, -4570, 4528, -2954, -1623, 9424, -6578, -6714, 9221, -1706, -1995, 7444, -2836, 301, -7216, -4782, -7569, 9063, 4679, -9641, -782, -3456, -604, -9401, 9686, -9576, -7825, -3929, -8695, -9313, 1159, 4288, -9420, 3470, 9972, 2046, 7404, 8877, 4640, 6617, 836, 3894, 7262, 7670, 5254, -1039, -1555, 6680, -5440, -5386, 5684, -8812, -7398, 3607, -3584, -1505, -6565, -812, 7784, 2716, 1804, 2921, 4504, 8063, -5577, 4872, 207, 2336, -3356, -4394, -7431, -2539, -8154, -1140, 2262, 3964, 1840, 5894, 4571, 9589, 2042, 1400, 1822, 631, -8900, -6675, 1471, -68, 8407, -5840, 1781, 1911, -4872, 4855, 1353, 9097, 9557, -8176, 1645, 9179, 4972, -8120, -3201, -4700, 1039, 3821, 1120, 9355, 1928, -9988, -2915, -8502, 7497, -1291, 2957, -3751, -6460, -9127, -4888, -8790, -5616, -2221, 9267, 6277, -2797, -1802, -8972, -7357, -2242, -940, -9677, 8728, 633, -8818, 5119, -1939, 3267, -6014, -4170, 7373, 9734, 680, 530, -130, 7155, -7400, 9919, 8791, -8939, 7237, 3895, -2566, 477, 7236, -1898, -1597, 3014, 8181, -4097, 8127, -7817, -4071, -5122, -9157, 9360, 3182, -5094, 8345, -3959, 9464, -3645, 3284, 2832, -9855, 1190, 4751, 9638, -2776, -315, 1735, -565, 6510, 3411, -4621, -453, -4004, -3054, -8837, 5970, 5584, 6625, -9896, -1465, -1482, 6408, -6000, -8598, -6137, -8762, -2100, 2163, 2367, -7666, -8674, 4939, -89, 1291, -8562, -690, -3382, 5353, -6157, 2611, -7561, -9701, -9324, 6726, -9735, -6986, -9608, -8529, -4822, 3956, -2665, -1734, -2842, -2508, -9538, -6371, 8073, -5766, -3971, 8167, -2935, -6808, 6740, 9699, 3566, -1131, -3780, -8819, -6310, -5542, -701, -2926, 558, 9370, 8793, -2797, -9111, -5653, 6472, 8418, 5744, 1060, -9081, -9998, -3470, 8879, 5992, -4502, 779, 9800, 5447, -3689, -1656, -2433, 7234, 5743, -8004, -3010, 8501, -2655, 1713, -7624, -4404, -9474, 1603, -556, -422, 8892, 2149, -6085, -4707, -7462, 6363, -894, -3651, 3442, 1659, 1343, -1645, -7636, 8246, -2827, 763, 3870, 3198, 7203, -9881, -465, 1447, -9140, 8263, 6419, 8287, 296, 446, -4145, 2887, 7828, 3202, 6228, -8560, 2644, 6399, 9616, 4842, 1352, 1685, 343, -2916, 3873, -4418, -2460, -1864, 3075, -6655, -1911, -9455, 3000, -7252, 8120, -8458, -1085, -4777, 9618, -6101, -5620, 1866, -9541, 6055, 7814, -5108, -807, -2947, 6839, -3106, -24, -6850, 7316, 5390, -1403, -7750, 9985, -5607, -629, -3805, -7568, -2995, -5854, -7910, -2067, -5527, -4124, 6329, -5704, -9855, 4211, 1323, -1391, 2312, -7891, 3760, -4964, 7108, -493, -2903, 4380, 9369, -588, 6018, 4163, 5937, -5523, 395, -5641, 8371, -6292, 4748, 3824, 5625, -1911, -4151, -6987, -5851, 2134, -8504, 5056, 1489, -2278, -2526, -9285, -7580, -9874, -7828, -2705, -6714, -6566, 130, 9531, 6316, -2317, -8483, -4878, 5469, 8286, -699, 3424, 8848, 5394, 3476, -9352, -9210, 7055, 9786, 8896, 9809, -9469, -8810, -485, -6616, 7300, -8874, 4472, -2944, 2797, 7700, 366, 7514, -7579, -8077, 1221, 1789, 2954, -4382, 584, 2144, 8429, -8547, 6619, -7332, 3530, -3967, 3116, -2457, 9150, -7355, -5930, -1014, 6795, -1057, -9564, -6497, -4271, -9173, 5787, 1283, -6075, 4735, 1681, 7992, 7097, -9515, 2220, 3285, -8388, 9042, -6808, 329, 363, 4135, -9861, 3988, -7758, -6254, -7866, 9401, 4551, -701, 1461, 4191, -20, -8415, 619, 3950, 2648, 4543, 7236, -8122, -920, 4952, -7850, 2577, -8716, 819, 817, 605, -5595, 4400, -4345, 6657, 6725, 6760, -4532, 1393, -2633, -5424, 9955, 3663, -7953, 6746, 8280, -2809, 8110, -69, 7249, 1735, 9904, -5778, -959, 6526, 6210, 3243, 7629, 6930, 5217, -8246, -7120, 2416, -5554, 6375, 9667, -3165, -9238, -6242, -3088, -3249, -1997, -5823, 1020, 3326, 5410, 6217, 1099, -7175, 4519, 8291, -7960, -6704, -7381, 1191, 6302, -268, -9630, -60, -6406, 9400, 2924, -2285, -2148, -95, -384, -6185, -3666, -4943, 6417, -1848, 8238, -5685, -4461, -2070, 3877, -4885, -4116, 8045, -7674, 4061, 4727, -5711, -1830, 8631, 7925, 3857, -8851, 1221, -5682, -821, 5228, -3570, -6118, 3911, -5519, 2440, -5839, -4962, 7912, 7724, 6809, 4161, 842, 4211, -8177, -2797, -5700, -9874, 7671, -5278, 9718, -5648, 958, 3642, 55, -1350, -6229, -5684, -4027, 8492, 1391, 519, -6204, -7330, -2488, 3405, 4126, -5961, -8470, -5730, -7154, -9456, 8548, 2458, -3563, -4727, 3798, 7415, -3976, 7255, -3643, 5030, 674, -1125, -5666, -8230, 6749, -2853, -190, 5503, -9265, 2564, -3413, -8922, 4668, -8754, -8353, 7757, 6817, -5265, 2010, -5507, 3889, -8913, 2383, 2411, 7608, 5331, -6056, 3196, 707, 8441, 3727, 6997, 5190, -4932, -8545, -5601, 2919, -3540, -6229, -6951, -3185, -1218, -3071, -5441, 3910, 1908, -4607, -8984, 8245, -8965, -1479, -517, 1488, 6000, 4557, -9433, 6032, 911, 7700, -4160, 6697, -7147, 7033, -6771, -6442, -5416, 7844, 3636, -1717, 4999, 6338, -8498, 721, 6551, 1638, 5496, -3666, 5979, 5402, 2034, 4047, 1311, -6665, -1955, -3388, -7853, 7669, -312, 383, 30, -333, 8455, -8292, -2598, 4469, 8786, -5513, 3185, 7503, -7608, -480, -5955, -9742, 8564, 5019, -2673, -8804, 9818, -8980, 3768, -8336, 4018, 3380, -7969, -8933, 5723, -6633, -6663, -8723, 6645, -8850, 1380, 6787, 6018, -8425, 275, 9136, 8999, -6362, -4013, 2203, -790, 6, -5416, -3987, -3376, -8731, 7846, 8469, -6168, 2782, 3939, -8694, 2855, -447, 6464, -5633, 4146, 7232, 649, -2066, 9853, -6040, 67, -2777, 6803, -2730, 8686, -8534, 3709, -439, 3453, -8891, 4506, 5166, -9800, -3792, -4287, 100, -7010, -3382, 684, 4434, 7477, -6247, 1592, -9691, -7157, -5134, 6299, 859, -3677, -1770, -8688, 1974, 8763, 6548, 2498, -2427, 3736, 316, 8421, 8915, 7695, 602, 2991, -3359, 9371, 5241, 4622, 7197, 6142, -1336, 5724, 7988, -3847, -7320, -2729, -8622, -1315, -7794, 9166, -3650, 5402, -4898, 1674, -8479, 7502, -7166, 8626, 8234, 3773, -9219, 3820, 121, 324, 235, -2765, 9296, -2947, 2947, 4488, -6743, -7660, -7515, 1948, 3434, -3524, -2383, -6560, -6992, -8605, 2799, -1952, 3515, 6610, 9502, -5975, 4075, -2911, 8822, -2874, -7430, 4728, 7887, 6915, -5751, 836, -1411, -2433, 8568, -9501, 8377, 2036, 6679, 1412, 4747, 2300, -1949, 6615, 7180, -5035, -5268, 8535, 2787, -9645, 7817, 932, -7083, -4130, -3288, 2615, 9976, -6362, -3648, 5891, 261, 4440, -2680, 7796, -8251, 4382, 2057, 2982, -1099, -829, -7809, 1382, -2593, 8599, 9184, 3602, 3445, 1518, 102, 2415, 8644, 9415, 9170, 8264, -5292, 71, -1338, 9387, 1428, 792, -9412, -2567, 575, -2207, 6018, -8061, 3016, 2216, 4749, 2013, 2538, -2349, -3134, -8061, 8868, -5584, -7658, 359, 1908, -3005, -5064, -3995, -7910, 6758, -4414, -363, -2631, 4464, 6655, -2463, -1126, -347, -334, 8180, 4709, -2547, -4149, 1688, 2570, 2714, 5524, 5702, -9047, -7369, -3847, -1245, -2817, 8461, -913, 5372, -8632, 4718, -9445, -7733, 2498, 63, 2260, -7780, 5046, 1435, 6931, -2671, 8126, 9687, -2747, -8432, 7039, -7125, -679, 3977, -6631, -5253, 8817, -1918, 9443, -6944, -7725, 2030, -6180, 9592, 1348, 2892, -7006, 6818, 7759, -9290, 9251, -89, 2087, -783, 2975, -9847, -7675, -2214, -1859, -8195, 1304, -2427, -2169, 7374, -9882, -4941, -4862, -131, -1554, -765, 6739, -7746, -1589, -4517, 4707, -5836, -189, 8249, -2777, -5987, 7571, -4758, 9418, 8344, 9741, 3843, -3218, 752, 9111, -5384, -6076, -8101, -1229, -681, -5749, -3599, 3353, 2812, -2307, 8024, 365, 9466, 9073, 2560, 4494, 8779, 6686, -3204, -9266, 4689, 7260, -4454, 9720, 9214, 6341, -6488, 9022, 4590, 5149, -8965, 674, -9081, -4944, -9401, 1296, -6626, -822, 6994, 301, 2088, -7176, -1855, -8686, 1924, -9972, -2180, 8495, 7366, 25, 7662, -4888, 5814, -7091, 8042, -4454, -6116, 2148, -5538, 6637, -6372, 7484, -351, -7950, -6520, -6927, -8138, 8232, 4669, -8282, 6744, 2638, 2829, -1241, -5721, 1966, 4216, 5727, 4048, -5085, 6328, 1132, 8163, 1386, 5338, -7673, -5074, 6429, 7177, -1956, 5879, 2366, 9266, 4243, -7232, -1012, -7860, -4866, 3169, 4618, 6068, -4876, -4167, -6964, 7263, -862, 969, 2428, 2998, -1302, 8106, 6186, -9251, 7567, -1744, 957, -6138, 4465, 4777, 995, -6667, 7258, -9971, -1432, 2996, 8910, -4855, 6115, 9150, 9037, -4696, -107, 2501, 3059, -1613, 4505, 9314, -506, 9470, -7847, -6186, 6878, -9823, -4343, -3122, -337, 8581, 9524, 7935, -1808, 4931, -258, -1425, -8530, 7364, 1399, -7228, -8679, -7207, 6995, -7961, 1349, -7979, -7118, 822, 3069, 231, -8728, -3258, -5763, -1431, -9050, 5350, -7794, 256, -7552, -4989, 9805, -1875, -9891, 3906, -9270, 7493, -5973, -8075, -8143, -6430, -5124, 3789, 4990, 8243, -5033, 5311, -4777, -2670, -4548, -9341, 7270, -7451, -7024, -1855, 6285, 2147, 4517, -9965, 9910, -3201, -4219, -5486, -8554, 1756, 1423, -3060, -4006, 4692, 6994, 3200, 9863, -9960, 590, 5972, 4118, 2158, 4775, 4616, 8342, -7781, -7529, -2230, 6131, -7522, -2329, -4203, 8843, 5759, 9061, -704, 5986, -7235, -8826, 2945, -1342, -6294, -2312, 3596, -3894, 9226, 3170, -5622, -7810, 3903, -2825, 5421, 4769, 1478, -2753, -9693, -4793, -7187, 6777, 4901, 2645, 4195, -1485, 3588, 3815, 9785, -8176, 2210, -4990, -1816, 9598, 5457, -9592, 1268, -5191, -9289, -7929, 7173, 4471, 9083, 8219, -9997, 221, -7661, -9506, -3900, 7137, -1767, 5233, 3405, 5109, -9197, -939, 4780, 3584, -7916, 8997, 466, -9501, -2523, 1242, 1115, 7862, -3724, -8172, 7828, 8000, -2277, -8679, -9, 3029, 6912, 228, -5066, -2785, 9735, -4880, 8590, 6406, -5933, 3563, -1980, -8213, 5117, 9231, -5098, 7638, -9416, -1865, -334, 9289, 9641, 458, -1720, -7590, -8880, -3389, 6769, -4129, -1138, -2135, -3090, 4452, 1980, 8208, 9221, -8241, 1130, -1789, 5940, -6924, -1312, -4762, 3724, -4725, -9918, -9441, 7596, 7555, -5619, -5233, -71, 8015, 371, -5018, -8610, 1770, -1784, -2081, -6764, -2583, 6545, -1731, -7014, -633, 2492, 3354, -6518, -8734, 4651, 9881, 3440, 5971, -5765, -7937, -3338, -2613, 185, 289, 6111, 5643, 425, -9648, 5368, -6060, 3284, -9164, 3712, 9325, -1759, 7663, 5382, 7206, 783, -8949, 8613, -4440, -7347, -221, -1445, 9462, 2875, -799, -5566, 7170, -6167, 9654, 5329, -7580, -7149, 8368, 3371, -4996, 6486, -4620, -4817, 5776, 8275, -7810, 9754, -3677, 9474, 1664, 6491, -7363, -3985, 3024, 2609, -2649, -3581, -1238, 4746, 5020, 4786, -8334, -7195, -2890, -2734, 1879, -2792, -1719, 1331, 104, 2546, -7842, -1325, 342, -7746, -462, 2882, 2311, -3130, 6840, -6703, -8965, -3863, 5536, 6329, -7244, 5865, 5128, -4138, -7840, 7742, 2333, 943, -69, 6150, -606, -9416, 338, 6257, 8260, 274, 8678, -1775, 6614, -8192, -4013, 96, 8233, 5625, 6114, 7324, -2666, 9218, -8303, -8215, -2228, -5948, 5416, 2208, 2243, -7938, -4110, -3829, -9780, 9899, -3156, 3856, 2936, 1422, -7833, -3248, 9371, -8808, -149, 9742, -3102, 6641, 6029, 7646, -863, -6842, -6943, 9189, 146, -7181, 6634, -2758, 2917, 4841, -3164, -9293, -7935, 9717, -497, -601, -8990, 3737, 1898, -5860, 4955, 4536, -5429, -1171, 9508, 3787, 4186, -2593, -889, -2393, 6184, -2970, -3045, 2542, -4878, -2322, -693, -2074, -9796, 9105, -9827, -4511, 6557, 2790, 5937, -2633, -4225, 8872, 7227, 5922, 58, 9029, -2065, -332, 6670, 945, -9640, 8469, -6677, -3646, -6401, 1909, -3462, 5540, 5291, -7, -8763, 2297, 4626, -8413, 2304, -417, 4760, 1893, -3374, 6737, -4810, 7384, -9034, -9856, -5635, -6280, -6644, 7291, -5512, 808, 3113, -7821, -5094, -6325, -6602, 4958, -4936, 2690, 8575, 7736, -7317, 1893, 3484, 1765, 6101, 7495, -9841, -8224, -4711, 4479, -7791, 6637, -3485, 8650, -6801, 3142, 1476, -7061, 1196, -8364, -5310, -2764, -7336, 6007, 9418, -3797, 4000, -6763, -8673, 6990, 8781, 8610, 7217, -3743, 3499, -3106, -7443, -8338, -9441, -6810, -3389, 6336, -1191, 2240, -9294, -904, 3958, -3156, -1181, 5644, 5450, 6662, -2550, 4635, 9109, 7598, 6889, -2936, -3178, 8821, 7029, 951, 6254, 3071, 1175, -5037, 7593, 9766, 7754, 2228, -7383, 4999, -1314, -4905, -4144, -3171, 4113, 6060, 8773, 5959, 8540, -9524, 1281, -1541, 7743, -5723, 993, -7900, 9245, 8198, 843, -422, 3015, 8790, 8841, 9488, -9497, -9055, -1581, -5495, 7442, 3185, -9142, 5417, -6816, -6950, 8170, 2639, 3818, 4666, -3111, -7043, -7156, -9749, -5813, 3006, 9348, -2456, 3, -1016, 8739, -6282, 1106, -2250, -7452, 8294, 7531, 3704, -8559, 4188, -6578, 1361, 3213, 4345, -6900, 7269, -121, -5060, -3937, 8977, 2801, 729, -3035, 5332, -2743, -7334, -5226, 4795, 7703, -8821, -2655, -5970, 9830, 909, 3650, 4398, -9938, 8263, -5525, 8884, 8600, -8041, 1371, -564, 9318, -4690, -4103, 7918, 1950, -6132, -7885, 2220, -5455, 2774, 4983, -7806, -1322, 4682, 4719, 6503, -9604, -5942, 8472, -8206, -4169, 4862, -5814, 9477, 9153, -4924, -3292, 6395, 1978, -3069, -9299, -7877, -3289, -3360, 2642, -690, 6002, -1141, -741, -2034, -2099, 264, -5944, -2146, 2480, -5139, 615, 9734, -124, -1117, 5744, -4100, 4911, 8160, -931, -3763, 692, 1222, 7996, 909, 9067, 794, 2986, -401, 6098, -7592, 1322, -6104, -6546, 490, -7808, -4969, 9121, -1834, 4675, 5393, -7341, -3099, -6664, -3050, -9557, -6102, -7439, -5137, 414, 2700, -7951, 1226, -8325, 9957, 8912, -1241, -349, 6745, 6708, -6796, 5633, 1432, -5053, 4153, -8188, -9058, -7579, 3460, -7266, -7636, 9748, 2757, -4449, -5492, 5965, -6297, -9693, -9269, 5027, -4133, 7899, 9779, 2463, -4278, -2832, 5569, 5596, -2841, -189, -4618, 4078, 9455, 2463, -5417, 1273, -3326, 9257, 4763, -4092, -1720, -5711, 4382, -6381, 5271, -5883, 325, 3282, 6046, -2736, 5030, 5500, -2152, -8125, -2893, -6095, 1036, -9984, -6145, -5545, 4930, 5289, -7919, -1648, 8480, -6864, 5872, 6223, -4627, -1448, -2044, -3945, -866, 7100, 1609, 5956, -3935, 4712, 6276, -7694, -5964, 8402, -5024, 4276, -717, -6516, 3473, 4232, 8211, -2756, -1060, 5702, -4208, 9010, 6420, 3987, 2852, 6650, -3207, 8518, -7384, 2088, 8034, -7741, 9446, 7344, -4596, 2602, 4011, -37, 337, 2232, -3995, 2743, 1879, 1439, -6703, -2495, 8576, 5521, 3282, -6023, -2730, -3655, -6932, -9633, -7040, 9846, 7028, 9742, 2652, 2894, -1303, -8221, -9736, -1775, -6622, 9469, -5397, -8235, 3911, 713, -2114, 811, 1698, 4461, 7071, -3624, 3384, -53, -1353, 4339, 95, 1329, 1824, 1063, -80, -6735, 1632, -5063, 4514, 3511, -2749, 3947, -5056, 8782, 7943, 3563, 6979, 3030, 9659, 7663, -5646, -3117, -8093, 5102, 5365, -43, -7569, -9890, -5650, -441, 4402, -8367, 5387, -8032, -6658, 2215, -1770, -3394, 8626, -5333, 6557, 1994, -3966, -6690, -3200, 1898, 8998, -5089, -5265, -6402, -16, -8907, -9194, -4638, 1815, 5226, -322, 821, 6785, -9446, -6675, -9082, 1988, 7582, 7724, 2177, -7499, -1615, -4689, -3916, 3321, 6681, -9109, 6556, 911, -7972, 3533, 2665, -47, -1044, -225, 2056, 2371, 9840, 5907, -1704, -6001, -5959, -2583, 9792, 3469, -1911, 8584, 372, -3722, -4027, -6063, 1254, -561, 5402, -241, 463, -5028, 7115, -181, -4148, 6993, 5550, 9609, 8189, 8178, 5235, -7503, -5238, -469, 7055, -3130, -6352, 3388, 840, -2946, 1833, -6982, 8460, 2344, -5360, 3722, 9665, 8408, -694, 7701, -6201, -6145, -973, -3813, -4426, 7721, -9259, 4664, 6158, -1585, -6896, -93, 3322, -6425, 9977, 3883, -2023, 1804, -745, -7664, -5413, 8026, -5718, -2123, 5890, 1909, -8113, -3526, 717, -3856, 2482, -8564, -8470, -8890, -1041, 2174, -9754, 2954, 8840, 7426, -1905, 8323, 5411, 8906, 1398, 359, -9993, -6677, -9111, -8332, -3426, -5399, -7707, -5159, -1240, -4562, -7281, 1567, -2176, -6717, 2975, 4809, 7366, 1290, -5419, 2039, -5786, 7799, -4138, -2578, 3830, -2175, -6659, 7902, -1743, -341, 6593, 2909, 9846, 8462, 8455, 780, 3655, -3810, 2828, -4586, -6288, 2089, 3363, 9003, -2478, 7001, 7441, 2289, 1608, -3749, -2183, -5127, 6364, 4256, 3032, 4569, -6894, -8420, -4320, -4252, -5355, 6577, 6523, -9576, 9522, 1116, -6619, 8142, -1883, -8869, 6840, 4649, -1065, -4946, -9151, 3202, -8234, 2189, -1737, 8189, 7154, -3775, 5547, 2375, 973, -666, -6764, 6318, -2976, -4252, 2617, 1685, -3832, -7283, -3982, -2005, 4278, 7507, -6378, -5368, -2965, -1539, -7237, -8056, -4286, 2461, -8402, 3856, -4890, -3945, 2306, -5774, -8547, 3960, -1496, -2696, 1939, 6708, -7906, 3401, -9544, 3834, -930, -1970, -6279, -634, -8987, 4596, 464, 8320, 1545, -5260, 1441, 7889, 1466, -539, -6609, -8266, -7958, -9161, 2263, 8053, -6131, -684, -9741, -4410, 3883, 277, 9166, 9219, -9709, -3309, 7655, 2039, -7149, -7468, -543, 1020, 775, 2422, -6785, -582, 170, -1657, 7387, 559, 9653, -4865, -7332, -2002, 5885, 1319, -9254, -1175, 8866, -8480, 305, -3928, 3748, 7707, 5761, 373, -9807, 9738, -4877, -7787, 147, -2897, 4668, 6533, -2830, -9533, 9228, 7837, -4670, -2457, 5256, -5133, -1210, 2682, -1536, -6139, -9959, -3806, 2933, 3261, 2665, -5472, 7960, -2129, 1945, 3876, -6827, 3148, 6291, 9011, 3935, -6836, -402, -4308, -6115, 2172, 8691, 2459, -4985, -9812, -5368, -9300, -481, 906, -2471, -9228, 1540, -5001, -8560, 7112, 2936, -7438, -7953, -5286, 8396, 1636, -4171, -9636, -430, 7559, -9362, 19, -9145, 969, 5896, -3492, 8243, 7139, -7057, -5133, 3647, 8890, -3990, 6068, -1908, -184, 5066, -9309, -5354, 9986, 1093, 5800, 3824, 3406, -2208, 8796, 7333, -439, 228, 385, 5041, -4843, -7936, 8967, 2249, -8209, 4672, -3356, 1721, 2024, -7545, 3336, -7069, 2935, -8830, -7854, -6706, 570, -3136, 4886, 7332, -8574, -9339, -9609, 6496, 5032, 9957, 3019, -944, -825, -3762, -6561, -2534, -7449, -7275, -5072, -4448, -915, 180, 6102, 9192, 7870, 667, -6482, 4914, 4829, 9826, 8142, 3596, 2424, 1905, 5510, -8869, -9676, 3771, 5363, 4087, -4609, 5019, 4396, 1429, 7264, 4540, -3670, -6220, 6916, -2510, -4068, -8568, 6257, 3751, 5720, 2274, -9003, 1783, 4630, -7619, 2425, 8661, -8658, 1632, 9619, 6899, 4755, -1614, -6367, 7456, -9667, 9363, -116, -5445, -217, -5389, -1014, 2702, 4179, 2104, 5427, 6770, -2198, 1688, -507, -2383, 8626, -4831, -7535, 4397, -6169, 9546, -1171, 9057, -5524, 4556, -3189, 9928, 2810, -3029, 3440, -631, -7705, 616, -1905, -833, -8455, -5513, -7304, 4057, 9171, 9016, -2429, -52, 9066, -8723, 1004, -3226, 6466, 2479, -2417, 8216, -3129, 7145, -5390, -2110, 1290, -1971, 8074, 4969, 2428, 7872, -9432, -9262, -3884, -1690, 3670, -2162, 2929, -1986, 7393, 1918, -1176, -4358, 6516, 2811, 2551, 9256, 3475, -3561, 2021, -8465, -9049, -1331, -3380, 905, 9164, 8594, -4400, 8096, -1423, -8811, -3169, 4776, -4200, 1663, -6493, 6719, -5434, -5022, -9910, -1375, -2504, 820, -2451, 3551, 5964, -1362, 5499, -8117, -725, -8321, -728, 8303, -7205, -6381, -3515, -4133, -4242, -9095, -1215, -5518, 4623, 5665, 6891, 1090, -3194, -9501, -9720, 5127, -3290, 1943, -7416, 8228, -5069, 6483, -926, 8344, -4275, -9417, 1357, 7803, 2974, -787, 2568, -9185, -3920, -4525, 5742, 8132, 1633, -9440, -3243, 8026, 7267, 3067, 981, 8307, 9307, 9125, -5074, -4246, 9190, 17, -5764, 1271, -3022, 8177, 7980, 7389, -1690, -8407, -4414, -2753, -1854, 1627, -953, 5551, 4979, 4177, 336, 5747, -3925, -1248, -7503, -1295, 4155, -5619, 3826, -6976, -8559, 1751, -6247, -7458, -7476, 9954, -1274, 3817, 3970, 296, 6527, -1377, -8081, -3868, -6619, 722, 1817, -5188, -884, -283, 5594, -7990, 2914, -5175, -1155, 5336, 3136, -9861, -9173, 6712, 4645, 3426, -3211, -5934, -4396, -7196, 3820, 249, 9358, 1442, 7151, 6509, -6325, -7912, 6422, 1352, -9569, 3647, 6556, 6704, -6969, 3020, -8612, -7308, -2075, 2568, 519, 4276, 1301, 2301, 7505, 1875, 4948, 806, -6550, 7704, -7392, 6901, 4222, 2395, -6287, 9522, -3016, 3649, 1400, 2258, -731, 1365, -1407, -1363, -6136, 6632, -4140, -3794, -7920, 5511, 9631, 7645, -6585, -1932, -578, -3024, -6404, 3051, -5609, -2534, -831, 4656, 1777, -1179, 1245, 9140, 4299, 5823, -2878, 9342, -2904, 3241, 5960, 3116, 3562, -123, -5200, -7009, -8898, 2803, 446, 2947, 4126, -1988, -4493, -9625, 3428, -7191, 1741, 7453, 6361, -7256, 7221, 4144, -2584, -2409, -9588, -7659, -9015, -8626, 8647, 5595, -3405, -9900, -5730, 7195, -9444, 6142, 3204, -2574, -3106, -1826, -4028, -7611, -397, -9489, 7471, -7884, -8344, -1052, -593, 4478, -6610, 7915, -9938, -7942, -393, 5699, 5593, -9604, -8131, -6633, 6592, 2268, -4407, -6653, 2443, -8275, -778, 4578, 4496, 1156, -1525, -1100, -4011, -2954, -8802, -4224, -31, 7644, 1836, -5161, 1376, -9547, 9958, 502, -9731, -464, 213, 213, 4703, 9213, 2533, 8804, -600, -5113, -2472, -9932, 6700, -411, 4797, 5555, -8262, -7100, -6647, 9729, -2200, 389, -3666, 7687, -3482, -3670, -7241, 4589, -1264, -4557, -4205, -3779, -2646, 5453, 403, -1194, -4765, -5942, -8309, 1917, -2173, -122, 1462, 9114, 4619, 1376, -4881, -686, -2046, -7300, 1595, 5562, 5526, 1834, 4301, 799, 6920, -6422, 6771, 8660, -1073, 5460, 8516, 3161, -4252, -7603, -9012, 3264, 208, 8830, -335, -8587, 7262, 1386, 1881, -7626, 5507, -9996, -7275, -7700, 3880, 2681, 6887, 5727, -9958, 9522, -7125, -3962, -4892, -7015, -3434, -1593, -9227, -3289, -4607, -3934, 2943, -267, 274, 945, -3123, 2877, -8396, 7301, 870, -4140, 250, 9721, -8717, -8588, 9286, 802, 278, -6973, 6152, -7516, -3313, -2868, 5201, -8567, -8413, -4748, 2776, -904, -5081, 4721, -4728, -7827, -8378, 3222, 4542, 4673, 242, -7228, -6134, 4666, -753, 7218, 1927, -9833, 8487, -3991, -3126, -3376, -8172, 8333, 1323, 3194, -635, 1656, 5364, 7153, 3732, -857, 9115, -3000, 8242, 469, -4410, 1856, 656, -5281, -3461, -6464, 894, -8396, -4454, -7290, -4364, -2527, 2785, 6706, 9503, 4668, 3518, 2227, -7179, -1496, 5060, 5408, 7248, -1116, 4893, 8552, -5557, -830, -6505, -234, -287, -9968, -9853, -3490, -4778, 4186, -7682, 3674, -6246, -8792, -3690, -5170, -8126, -3684, -4717, 5307, 4677, -4781, -4311, 1964, -8530, -3350, -2434, 1390, 5372, 8871, -8982, -1966, -6303, 1116, 7202, -9346, -5216, 9446, -7863, 3361, 3865, -817, 2444, -2498, 663, -6102, -3715, -4310, 9031, 1484, -3870, 4050, -8957, 4684, 453, 8003, -3501, 6721, -8900, -218, 456, 8991, 8029, 593, 5580, 1480, -859, -4464, -1121, -494, -5261, 845, 510, 6600, -5114, -6841, -5479, -7287, 3095, 8228, -2315, -4686, 5566, 3211, -2697, -4898, -418, -926, -6376, -3833, 4652, 3606, -9214, -8247, -3760, 3570, -1457, 1499, -2872, -4765, -1536, 1721, 5611, 1576, -2471, 2567, -1620, 4174, 3134, 6294, -7039, -7419, -900, -9057, 825, 2129, -6955, 5257, 5512, -3156, -8221, -8802, 2668, 3808, 8556, 6926, 1573, -174, 40, 8459, -5183, 6567, 1909, -1004, 7128, -1687, -19, -7774, -385, 5601, -5271, -8834, 1456, -8337, 2093, 7421, -9631, -6724, 9975, 3283, 7064, -3166, 5559, 5364, -6211, 2873, 7127, -1270, 329, 3487, 3107, -2219, -9238, -8156, 3911, 6594, -5136, -5965, -3282, -7635, 1887, 1649, -5970, -2716, 545, 8562, -3670, -1188, 6014, -1483, 8917, -9490, 5574, 4367, 6173, 7496, -3307, 3608, -5865, -4442, -9116, -7478, 9375, 6085, 3050, 3220, -3725, -7680, 7346, 4703, -5150, 157, 2496, -1292, 2317, 5562, 8893, -2179, 1631, 7510, -4859, 6334, -7987, 3197, -5633, 7704, -7033, -8916, -5660, -5742, 6804, -159, 2284, -4213, -2438, 9051, 8861, 6077, -6776, -8342, 9927, -6467, 1238, -1075, -2257, 1835, 4687, -2069, -8761, 4795, 6702, 1434, -8066, -4619, 1568, 2445, 3279, -8774, -8080, 8845, -9700, 8348, 4439, -4438, -7017, 7146, 7795, 268, -7643, -7600, 4603, 5938, 1112, -6002, 2557, -4642, -9077, -9433, -1516, -5282, -3595, 7997, -8072, -9138, 1662, 7816, -7707, 5698, 6442, -503, 6395, 4989, -3850, 4628, -6429, -6377, -4264, -4334, -3912, -156, 487, 8700, -4880, 7863, 9301, -2409, -2210, -3160, 1787, -725, 5245, -4459, 9119, -8103, 2422, -5223, -5473, 8538, 3012, 874, -2835, 4809, -7686, -7589, 8540, 8997, 2857, 9641, -5571, 1810, 5028, 6137, 7768, -1955, 3618, 8791, -6870, -8445, 517, 9856, 3052, 8243, 2385, 7612, 31, -8728, 8019, -5186, 1922, 8778, 5649, -7284, 4746, -6558, -4211, -6969, -771, 3372, 5116, -6348, 3843, 8865, 3605, -7738, 9477, -9450, 6021, -6311, -4362, -9856, 9578, 3144, 2634, 6544, 8944, 7174, 2673, 5657, -5013, 177, 3813, 9693, -2066, 393, 5613, -475, -1141, 9115, 5025, 1604, -3938, -3414, 2830, -2528, 4282, 9569, -1409, 7927, 2832, 3383, 7014, -9282, 3889, -3659, -6643, -1652, -278, 8860, -7518, -6583, -7637, 1823, 4228, -2807, 9179, 9984, -872, 6649, 4794, -4750, -5813, 7908, -6324, -2861, -3789, -417, -1019, 7633, 4372, -7338, 187, -2266, 7170, 6613, 4124, 474, -85, -3647, 5898, -7762, -1238, -7588, 2399, -3508, -9404, 8064, -8421, -8010, 3580, 1308, 647, 9580, -1175, -4702, -9262, -2597, -4674, 1363, 3450, -4945, 5249, 2451, -4737, 9068, -5524, 440, -9196, -3909, 3730, 7836, 2784, 9369, 3783, -340, -4276, 4027, -6047, -3217, 6618, -421, 4860, 755, -398, 5481, -6902, 2824, -443, -950, -8725, 8364, 9253, -3431, 5934, -5830, 8663, -7976, -2595, 7576, 4037, -1538, 3354, -386, -9500, -1279, 4270, -5479, -6352, 1322, 717, 219, 8149, -5345, 3686, 7489, 9172, 4985, 2362, 2247, -3510, 4977, -2654, 2286, -8869, -5252, -7980, 2872, 4527, 9345, 1344, 2615, 6302, -8387, 432, -3212, 4014, -4893, -990, 4027, 6983, 2217, -8556, -8229, -1523, -5688, -2573, -5822, 7163, -5360, -3960, -324, -4844, -8602, -9064, 3509, -5956, -4614, -3011, 7506, 7190, 5281, 6392, -7539, 2045, -6534, 9344, 3004, 1778, 3675, 9676, 7764, -5831, 337, 1958, -9036, -4392, 7920, -1918, 4233, 6122, 991, -6905, 9695, -3485, -9467, 9547, -3451, -7695, -7816, -4874, -4811, -6822, -7236, -766, -4000, 7711, -4389, -6464, 5015, 1445, -9368, -5013, -1154, 138, 1203, 4867, 7650, -2584, -7631, -4227, -9145, 4628, -628, 7102, 3811, -4982, -7950, 1317, 1483, -8726, 5504, -6325, -6749, -1482, -8260, -2175, 5640, -5865, -1935, -4930, -6841, 76, 1762, -5233, -6251, -401, -6110, 6345, -1315, 7167, 6265, -8244, -7816, 4650, 1186, 9219, 8224, 6135, 9777, 7533, -2303, 6599, 4932, -5620, -3956, 8926, 1721, 6003, 917, 9677, -9275, 1004, 6079, -2580, -9283, 2307, -3903, 2372, 5925, -6877, 1783, 3633, -5355, -8300, 5296, 6517, 8706, 5654, -84, -192, 5374, -9294, -2113, 7688, 1883, 4951, -3180, 1772, 6988, 5719, -4358, -4677, 7228, -7895, 1812, -2662, -9263, -9087, -5836, 1331, 3899, -7943, 7492, -5983, 1287, 5373, 9314, 7472, -660, 6195, -1038, 5451, -1267, 8206, -7463, 1359, -6827, 7686, -1540, -4866, 2600, 5386, -7350, -4969, -6462, 4372, -5361, -327, 181, -8119, 9375, 2531, -7693, -2015, -9204, 3978, 6354, 6355, 8420, 881, -2462, 6801, -952, 110, -4847, -5728, -2007, -8445, 9969, 4335, -6867, 8887, -644, 9335, -7079, 7571, -7692, -7878, 9570, -6854, -7773, -9703, -4731, 8094, 526, 4522, 6816, -7090, 4256, -8285, 5399, -5342, 7853, 1922, -5711, 3000, 8071, -632, 892, 1738, 2076, 6321, -1841, 585, -9612, 5245, 2018, 9076, 835, 9953, 6153, -7609, 5953, -1199, 3495, -9554, 8748, 2496, 8468, 7792, -6132, -8020, -467, -7671, 9120, 2553, 7661, -4470, 843, 9644, -4058, 4521, -352, 6164, -2703, -3586, -5300, -5629, -9540, -8080, 1714, -4316, 8395, -7097, 7291, 3194, 7702, 4099, -3940, -6120, -3389, 7805, -8510, 9478, -2042, -1517, 7207, -7943, -911, 1685, -4044, 1841, -201, -1141, -4661, -5138, 6594, -45, 7482, 3746, -7510, -6321, 126, 5418, -2133, 4078, -9382, 8788, 932, 6874, -1335, 2550, 643, 8343, 7640, -4430, -4018, -8294, 4326, 5672, -5990, 3019, 1387, -4341, -9815, -8515, -4477, 4793, -7097, 9447, 7816, 8377, -2909, -1770, 7632, -1680, -2129, 3081, -7043, -3863, -3527, 9615, 1180, -4730, 4135, -4688, -6827, 7202, -3458, 6938, -360, 9102, -3520, -7694, 2982, 1744, 4687, 3977, -989, 3567, -7209, -9068, -6624, -7966, 4030, -1089, -7657, 4886, 2195, 968, 1325, -7324, 1722, 54, -1921, 4636, -8197, -6441, -2920, 6520, 3299, -1560, -2163, -9801, -1216, -4303, 7845, 2285, 3769, -6178, -4245, 3272, 1194, -1375, 1357, 9211, -5162, 5353, 4764, 6420, -7611, 2864, 3313, -6424, 6184, 3543, -6781, -5919, -2375, 6450, 9915, 4717, 1227, -7958, -5574, -5997, 4743, 7781, -4026, -1236, 2917, -8040, 100, -3189, 9826, -9889, 824, 7874, 6474, 6340, 1935, -208, 1702, 8761, -238, 7311, -4811, 8832, -9006, -1150, -1661, -2951, 2354, -3057, -2617, -136, 8165, 733, 8046, 9605, 7989, 5169, -2442, 449, -8922, -9622, -7055, 9087, 4496, -2677, -6085, -729, 2177, 8772, -9991, 59, 3758, -910, -3440, -1407, 6069, -7111, -6150, -4576, 2856, 6343, -3636, -8020, -1296, -3314, 3425, 9076, -8844, 3648, -3673, -2007, -4600, -2746, -5137, -1215, 2526, 5114, -7139, 7193, -1193, 7934, 519, -596, -454, 2060, -862, 1735, -3375, -1724, -5953, -4162, -725, -5971, -335, 6925, -1362, 7202, -1034, 9201, -4267, -104, -7642, -7620, -8360, -788, 1803, -1491, 588, -893, 4124, 4469, 6331, -4793, 2325, -6968, 4204, 1908, 2004, -9610, 621, -3365, -6968, -9681, -2502, 9054, -4960, 9754, -1240, 4400, 300, 7464, 9460, 602, -1697, -6163, -3510, 2973, 1606, 4311, -890, -7906, -1434, 4563, 8645, -5438, -7847, -8291, 6253, 7307, 969, 554, 9559, -5078, 5081, -9398, 4129, -5407, 5051, -98, -8517, 67, 5154, 4190, 6088, 8331, 6419, 5646, -4039, -2148, -7617, 5214, 4165, -1767, 4676, 8290, 8110, -6712, -7669, -7650, -5242, 5674, 5246, -1953, -3843, -716, -3436, 1908, 6605, 2909, -5728, 3712, -7188, -3976, 9900, 3579, 902, 2306, -3738, -8873, 5480, 2493, -8437, -8214, 5659, -344, -129, -9140, -9103, 9206, 5441, 91, -7102, 6682, -9819, -8086, 6873, -6522, -8749, -3736, -752, 3218, -5019, 6327, -1495, 9391, -6199, 1371, -3153, -5863, -6723, -7759, -5909, 964, -1651, -6773, 5196, 5440, -5710, -3352, -2258, -5572, 5277, 7953, -82, -9793, -5098, -2663, 8976, -4448, -6579, 9653, -2102, 1552, -8437, -7939, -3246, 9315, -50, 4853, 2146, 767, 3235, 8440, -598, -4510, -4144, 1753, -9357, -8010, 2254, 7437, -3081, 6611, 8738, 2733, -3124, 5083, 6486, -4231, 1702, -1166, -4240, 4443, -8473, -5626, -15, -9840, 2482, 8321, 5249, -2139, -3204, 1295, -7510, -9471, 8335, -1372, 5627, 1301, -7910, -4662, -3564, 1894, -8876, 3456, -6780, 6683, -933, 2386, 283, -8585, -4122, -4906, 4796, -8458, 2026, -3211, 4534, 778, -8178, -2269, -9875, -6923, 3417, 954, 4967, -4240, -4756, 9292, 4986, -2522, -6329, -4360, -6691, 4946, -6291, -6651, 1469, -7834, 1252, -7890, -1309, 9714, 5175, -5780, -7999, 272, -4446, -336, -3828, 7885, -7433, -4388, -7004, -6668, -5477, 7261, 8254, 3101, -9072, 3141, -7181, 6405, -4936, 8014, 952, -9111, -1950, 637, 2649, 1866, 4622, 1012, 3674, 2990, -9303, -3730, 4640, 120, 1134, -652, -3699, -8577, 9867, 403, 7316, 5196, -326, 55, -383, 4191, -849, 5428, 7996, 6258, 8573, 8588, -934, 9589, -1393, 7296, -1262, 5126, -6120, 5718, 1692, 4438, -4610, -2862, 421, -1701, -3377, 5533, -8711, 5888, 9928, 934, -5724, -5311, 8631, 1914, -5403, 4902, -1063, 6644, -3260, -6231, -4434, 8801, 4716, -193, 2044, 9202, -6089, -8121, -8903, 5430, 8067, 3103, 3164, 2224, -1507, -2427, -5871, 5529, 908, 5686, 7072, -8891, -2356, -1939, -8077, 6821, -5698, 7642, -5205, -88, 2052, 125, -9701, 124, 1102, -1892, -3026, 2637, -253, -5048, -3949, -4579, -2665, -685, 8059, -3205, 5010, -2640, -5511, 3624, -4253, -6267, 5212, -2084, 8562, 3756, 8351, 7122, -5464, 5928, 6167, 5751, -5447, 5167, -7438, -1017, -2150, -8928, 3836, 752, 4488, -3257, -310, -9968, -677, 339, -6807, 6805, -5218, -3066, -4958, -7103, 680, 3183, -8822, 1432, -4855, 9314, -1699, -5199, 6058, -5068, 779, -2968, -3442, 7329, 2831, 5587, 3886, -3989, 3219, 8243, -7485, 8703, -1811, -9316, -4875, 3566, 3785, 6026, 7739, -3397, 9062, -3398, 7691, 8343, -40, -2180, 5513, 7154, -8663, 1095, -562, 7904, -4746, 8240, -8607, -5595, -3784, -4490, -9782, -2566, -9892, 1871, 3461, 9040, -3119, 1350, -8138, 2604, -5303, -9454, 1309, -3563, -8108, 2333, -6878, 6255, 8676, -7107, 3086, 5265, -4949, -9298, 8449, 2200, 8121, -344, -8694, 7550, 1286, -1359, -317, -5183, -8900, -7336, 6893, 5411, -7259, -9165, 6444, -6817, -6114, 8856, -4594, 8847, -3070, -339, -1077, -1432, 3079, 8214, -2808, -1901, 7736, 8826, 836, 7025, -477, -8049, 752, -7633, 5708, 2819, 4563, -7062, 2220, -4012, -6850, 3648, 1842, -3893, -6, 4596, -1622, 7225, 8478, -3206, -1865, 5403, 5118, 5864, -933, 5029, -7649, 3023, -7497, 7566, -1503, -7789, -3440, 8211, 9137, 2241, 377, 9561, 1299, 7501, 1762, 4329, 7928, 9360, 3605, -642, -3541, -7077, 9334, 3707, -5984, -3271, -321, -9609, -5016, 9144, 9238, -2243, -476, 7402, -6709, -3139, -9367, -7304, 2203, 2543, -5456, -9054, -8263, -3915, -7505, 5290, -4978, 9453, 6631, 5594, -4186, 9859, -8912, 1220, 2572, -8843, 6476, 5056, -3715, 1903, -9413, 8282, 372, 5079, 3009, 244, 3032, 3659, -7478, -9340, -4632, -8271, -674, -2751, 9517, -9649, 5066, -6825, -7431, 8068, -922, 3813, -4544, -1512, 4565, 8288, 8160, 6548, -3996, 5582, 1242, -3023, -6118, -4288, -6318, 1459, -5079, 6375, 9014, 5247, -4596, -3417, -1973, 4344, 7485, 7684, 9784, 5766, -2304, -973, -6004, 335, -1048, -6117, 4321, -1672, -4820, 4295, 2136, 8435, -5003, 3265, 2210, -9641, -3520, -5943, -8047, 434, -4305, 6225, 6345, 8016, 4674, 7507, 1317, 5674, -634, 6264, -8312, -3466, 6672, 896, 8324, 3281, 4418, 504, -6158, -2030, -4169, -488, 8626, -841, 6953, -5550, 2014, -5024, -2000, 9778, -2642, 8255, 8273, 4192, 9122, -3759, 29, 9500, -6890, 2505, -274, -5067, -4750, 2841, 6380, 8973, -7741, -6153, -1184, -217, 6111, 6378, -7870, -4192, -84, -1655, 6427, -9638, 1413, 1591, 2165, 7729, 7921, 6648, 9501, -7806, 6215, 2594, -5825, -8952, -4702, 7099, -5615, -5964, 2195, 2944, 9587, -7009, 2106, 8535, 4940, -8228, -3438, -3495, 9650, 3113, -9291, -114, -7054, -9629, -6284, -5082, -8189, -2146, -1291, 4170, 9271, -6630, 9741, 4941, 6566, -1363, 380, -9393, -6902, 9978, 2560, -1697, -2680, -9346, -6587, 6941, -4215, 4882, -3634, -7722, 9499, -4939, -7392, 2087, -6750, -9607, -474, 2875, 3441, -478, -4760, -4654, -9861, 9714, -6582, 3307, 2066, 6277, -22, 8948, 1723, -3191, 2287, 2876, 6071, 3585, 9148, 4120, 7399, -3477, 4171, 6665, -196, -6391, 1107, -3472, -5355, -3191, -8809, -326, 82, -6253, -673, 4668, 1671, -7596, -2747, 9487, 4502, 2770, 5262, -4045, 3468, -4159, 738, 3176, -6124, 2176, -4535, 6729, -1979, -9670, 8446, -2962, 7400, 1861, 9777, -9404, 6565, 3192, -7674, -3651, -968, -7742, -9717, 5330, -3858, -3685, -5717, -8386, 2333, 3413, -5242, -6648, 2731, -9139, 2651, -1305, 101, 8102, -5063, 2259, 7766, -5928, 9851, 2878, 6160, 4289, 3858, 5124, 627, -7330, -8050, 8065, 4125, -4875, 3180, 9424, 6219, -7077, 7347, -6265, 3453, -113, 2622, -6872, 4228, 5186, 8825, -6377, -1357, -2691, 3765, -3638, -7785, 5063, 5089, -6077, 8256, -2010, 8317, 7755, 9157, 8586, -3436, 3894, 610, 8451, 718, 8920, 8742, -9737, -7781, 637, 191, -1846, 7561, -4416, 6330, -4967, -5759, 7171, -8648, 7193, -3239, 393, -6620, 3913, -7795, 5211, -6044, 3362, 1617, 3746, -6094, -589, -6920, 1687, -5811, -1797, -7801, 9111, 5563, 7201, 6334, -2176, -8590, -8658, 2773, 3228, -3662, 5221, 1928, 8363, -6185, 7939, -6923, 5635, 3456, 1944, -6080, -9387, 1104, -4438, 210, -5281, -3452, 2580, -1066, -7432, -5731, 1472, -5976, 2431, 3471, -8343, -2968, 9043, -8381, 3357, -5332, -3784, -2540, -474, -9808, -8134, -7885, 507, 9377, -715, 6234, -6450, -3633, 6675, -791, 7804, -8906, -7131, -1797, 5480, -4591, 2068, -8329, 9281, -7423, -8587, 3053, 5056, 6130, -7709, 5915, -8934, -8180, -8682, -9630, 1813, 8874, -4917, 3415, 2667, 3565, 16, 1820, 4152, 4257, -598, 4612, -3832, -365, -8056, 3183, 132, -5092, -2186, -8989, -41, -1799, 5907, -4186, -3790, -474, 6374, -794, 2475, 2855, -6808, 6444, -5439, 9312, 7698, -726, 2702, -7228, 7121, 8659, 7179, -5916, -731, 630, 9507, 2972, 7239, -207, 4571, 9372, 9456, 4324, -2365, -2381, 3438, -2993, 6749, -2386, -9278, 1293, 6317, -1881, 1917, 866, -3207, 993, 9039, -3005, 9260, -8493, -8813, 1067, -9811, 3415, -6703, 9203, -7261, -5098, -7299, 564, -4480, -479, 9262, 8691, -6999, -5836, 2246, -6445, 2867, -447, -9110, 9617, 8503, 2476, 6894, 5323, 4280, 4499, 2617, 6261, 8920, -9240, 2727, -3749, 1927, -6719, 1716, 2071, -2414, 3146, 9277, -7957, 5579, 2213, -9935, -3048, -8123, 2164, 4083, 3440, -2036, 6750, -1698, 77, 7776, 2108, -7993, 8470, 4966, -8538, 3105, 3735, 5740, -4665, -9681, -4999, -4646, 6166, -5169, 1025, -8461, 8694, -9972, -9787, -8696, -5267, 4020, 3900, -7917, 1855, -6594, 5969, -4496, -8712, 5970, -3240, -4055, 7386, 8317, 1494, -8943, -3303, -4977, 2460, -3570, 3515, 8166, 126, -7843, -2115, -5228, 65, -703, -1397, 3852, -1028, 9270, -5061, 8370, 773, 4580, 4238, -1485, 2694, -6046, 2786, -9084, 4639, -1015, -2904, 6771, -526, -6998, 5909, -3812, 7617, -6581, 359, 8092, 1135, 5136, -9729, 7013, 8540, 4808, -8253, -5447, 5841, -9188, 8727, -5960, -5877, 5327, -3034, -4399, -2298, -3784, 9099, 3501, 4147, 2007, 7205, 3670, 5435, 9634, 2531, -8733, -8615, -650, 4325, 131, -1086, 9245, 5087, -2847, 623, -5657, 7699, -1310, -9306, -9770, -9084, 4950, -8739, 9980, 2654, -9065, 831, -98, 5652, -6994, -7661, 6652, 7239, -3996, -6827, -549, 3241, -823, -9332, 1679, 1182, 7319, 3719, -9808, -496, 4931, -3966, 1968, 1510, -4864, 1256, -4018, 8867, -8322, -1306, 5515, -7080, -8886, -144, 9438, 8178, -2709, -6675, -1654, 5289, 7434, 1659, 6609, -4062, -4386, 8066, 7364, -2018, -6760, -1595, -9668, 9725, -7642, -9477, 1845, 2456, 8517, 4873, -4127, -1795, 9592, -2995, -5692, -283, -2450, 2528, 8398, 1221, -91, -504, -2372, -2874, -7949, -81, 5000, 1801, -2720, 3042, -8822, -1903, 4484, -2598, -9351, -674, 1360, -6385, 210, -2366, 3305, 6165, -7911, -7615, 7848, 5132, 8084, 6928, 3311, 8543, 8492, 9952, 2334, -4351, -4187, 4697, 3567, -3181, 8859, -5740, 4756, -5643, -7708, -9175, -3224, -7076, 7716, 3576, 752, -2282, 605, 3997, 5163, -7213, -4420, -1096, -2734, 4937, 2043, -9496, 4067, 3066, 4188, -2323, 6075, -1678, 3730, 3162, 8809, -1886, 7198, -9277, -1979, 5881, -5705, -841, -6568, 4027, 7591, -2060, -767, -2146, 7320, 4787, 174, 2547, 4760, 2241, -3330, 768, 6238, -8181, -2803, -6803, 5158, -2013, -2487, 3452, -3971, 2759, -1897, 4844, 6084, -9280, -1409, -3645, 7569, 7995, 4216, 6623, -4730, 6105, -8637, 6202, -1338, -4622, -7665, -6696, 9239, 1377, 803, 4098, -6170, -7488, 2031, -2953, -3646, -1683, -7971, -8338, 5365, -6372, -1768, -2173, 1058, -9665, -4053, -2463, 4100, 4177, -4554, 8028, 146, -9750, -1440, 3497, -3317, -5334, -1339, 843, 981, 7199, 6913, 2732, 6086, 5466, 521, -8819, -1398, 8244, -6846, -6664, -3335, -2384, -5677, 1434, 2850, 7262, -7283, 6766, -3095, 8158, -936, -996, 2928, 8748, 8932, 6562, 6504, -3732, -5751, -2407, -1964, -1883, 7937, -6372, -4297, 7792, -5914, 2361, 6685, 7683, 7704, -3338, 4514, -8818, 8445, 8432, -8308, -5537, 8857, 262, -6836, -5310, -7113, 860, 9117, -2766, -1101, -1417, -9130, -9408, 3413, 4562, -6619, 6265, 4989, 1337, -4720, -2523, 8110, 5838, 9447, 8660, 7826, 1576, -827, -8710, -9359, 9721, -4016, 4023, 9446, -3246, 1729, -9187, 8479, 8600, -1335, 8249, 6166, 6807, -6152, -3795, 1287, 5841, 9631, 6802, 3667, -3893, 8246, 7601, 7107, -4974, -8303, -1324, -1223, -632, -7752, -4124, -4713, 5125, -5180, -9153, 3990, 2259, -9592, 6716, -6904, 9026, 622, -1528, 9369, -1756, 5345, 9637, 9277, -2806, 4667, 6407, -7998, -9127, 2701, -6467, -2135, -282, -3200, -7792, 8040, -1577, -8340, -6828, -9724, -3440, -9191, 4608, 4114, 7561, 5836, -9553, 4554, -9144, 8394, 5649, -2954, -1952, -1968, 6955, -249, 1527, -7911, -72, 3141, 1482, 7054, 1473, -117, -3906, -1542, -2767, -6565, 9057, -7751, -855, -6150, 3964, 3780, -2151, 2285, -4999, 421, -4483, -418, 6509, -6270, -5077, -158, -4309, 919, -1516, 5427, 2471, -2494, -6152, -3010, 8147, -3300, -6140, 2688, 3948, -3309, -3095, 2995, 1757, -2320, -646, 5733, 9214, 570, -6148, 5748, -710, -2982, -1825, 6915, -2544, -5927, 6405, 9234, -7091, 5141, -7620, -363, 2477, 9945, 7239, -6530, -6171, 9822, -8701, 8470, -9956, -7153, 8374, -6380, 238, -5776, 6048, -2615, 2069, -5339, 4609, -3370, -6106, 6474, -7485, 9669, 2919, 1012, 9827, 7494, 9986, 6230, 6040, 8863, -5719, -2372, -4774, 3373, -2381, -9953, 3114, 2486, 7781, -4624, -1730, 9780, 4077, 6767, -5021, -2178, 8664, 1055, -7044, 119, -935, -5122, 1311, 6876, -5817, -2550, 3161, -7406, -889, -3560, 3602, -8436, 8837, -2890, -8634, 1724, -2172, 6519, 4056, 1317, -3685, -9334, -5318, 3792, -6263, -3572, 3780, -5026, 3844, -7650, 1169, 9239, 638, 4984, 8387, 7584, 1027, -5372, -7923, 8025, -1939, -3487, 8935, 5722, -6725, 8353, 2381, 3162, -1772, -3166, -9182, -4867, 5455, -8907, 7237, 3965, -4315, 6151, 2972, -5472, 3892, -2601, 519, -4603, 5662, 5771, 1155, -1992, 474, -7202, -4653, 3753, -435, 5797, -7632, 5606, -1634, -5664, -1809, 3723, 3535, -5302, -4188, 931, 2033, -6589, 691, 8814, -4009, 4096, -3071, 5515, 5334, -9032, -5551, 5547, 3648, -2983, -9814, 5993, 710, 9226, 3643, -9366, 4691, 7841, 4805, 8142, 8022, 293, -7594, 5030, -5058, -6786, -5463, -9021, -5931, -3643, -2824, -5905, -6149, 4447, 4539, 8425, -2681, -8674, 2492, 9756, 8003, 8618, -5729, 3388, 2978, -7954, 1997, -3151, -3173, -6353, 8611, 4664, -5321, 7171, 7487, -569, -7894, -7038, -7638, 5305, 8044, 8283, -2567, 149, -4093, 6366, 3078, -2289, 2641, -1861, 1278, 5162, 9111, -359, 2225, -5525, 4513, -4310, -1083, -2017, 6115, -4449, -4751, -4348, 6837, -781, 8430, 273, 699, -7468, 4520, 1514, 8735, -9861, -9242, -1106, -3633, 8512, -2846, -6617, 889, 1638, -9680, -7446, -2544, 3168, 550, 5444, -1023, -1124, -3937, 7940, -5870, 6573, -8226, 5755, -9452, -6126, 7644, -1095, 1738, 2389, -7404, 6201, 6603, -2139, 2260, 4403, 6599, 5575, 7223, -5515, -983, -8753, 3816, 9222, 1003, 8999, -2951, -8729, -2350, 3031, 6416, 3014, 8841, 4791, 8141, -6904, 3678, -8451, -8279, -9536, 5045, -2358, -9228, -6967, -8042, -9566, 2865, -6899, 1642, 4998, 3301, -2575, 5300, -5709, -6616, -4083, 7601, 6308, -1196, -8892, -830, 3942, -5733, -3672, -2792, 797, 332, -827, -3996, 2913, -2913, 4740, -9860, 6220, -5761, -2355, -7683, -6825, 7565, 2287, 5136, -5397, 5687, 5338, -8532, 8320, -1509, -210, 3512, -1356, -1708, 9071, 5150, 4566, 6478, -1638, 2417, -5662, 7456, -5973, -3392, 114, -8494, 4624, 6581, 6104, -6192, 9737, -1515, 4510, 491, 9514, -1663, -2788, 1938, 9569, -866, -9671, 9085, 2996, -8926, 3853, 5195, 6702, -3138, 5420, 7985, -8003, 7624, -3118, 2730, 4058, -2531, 8748, 7441, 3784, 8500, 8157, -2374, -8718, 9641, -5552, -2671, 3167, -8488, 2079, -1503, -7752, 9566, -2001, -7146, -6006, 9781, 7652, -7218, 4890, -5498, -5723, -907, -6796, -999, 6405, 865, 9883, -8084, -7337, -4773, -8103, -4535, 9121, 3217, 9806, -4866, -1119, -8493, -6469, 9850, -1338, 273, -2761, -8502, 3563, -6890, 8848, 8525, 9234, -7629, -6560, -5696, 7436, -6291, -755, -5642, 3396, 3799, 7163, 5374, -8641, -2135, -6771, 5127, 5051, -986, 4110, -5485, -7540, -3816, -9453, 5735, 9715, -9422, 7909, 8827, 4951, -8859, -6426, 8403, 3773, 3597, 7730, -5414, -5071, -7378, 7083, 3885, 5407, -7095, 8443, 7808, -8555, -1425, -8032, 1263, 9116, -4715, 4120, 9284, 1395, 4034, -5848, 7691, 7964, 9034, 7431, 3948, 5471, 9020, 2468, 9567, -977, -1995, 7648, 6573, 612, -4717, -285, -8493, 2400, -4997, -9277, -5508, 6811, -2884, 9001, 8972, 3182, -4486, 3573, 1725, -6936, 3987, 8897, 1074, -6959, -7866, 7831, -2302, 4509, 4512, 9989, 3491, -5929, -6061, 9446, 3737, 4215, 5641, 4044, -1640, -166, 5225, -9805, 2907, 7425, 9832, 9722, -5790, -3202, -2293, -2017, -5082, 8196, 6531, 5143, 2207, 5738, -450, -1444, 4949, 111, -7168, 4047, -7989, 6710, -6839, -2432, -448, 9039, 6537, -6655, -19, -7778, 1767, -2157, 3660, 8946, -9, 2558, 2895, 6863, -5648, -2992, 9080, 753, 1948, 4312, 3640, 5567, 9646, 2992, -9253, -8508, -5988, 1832, -9758, 6266, 2116, 3677, -3331, -225, 3579, 4340, 4853, -83, -7397, 6916, -5095, -8268, -9771, 2161, -9073, 5773, -7794, -2978, 9033, -2494, -4815, 4913, -9541, 8407, -2537, -1511, 9102, 5433, 5072, -9553, 6976, 9634, -3743, -5830, -1871, 9362, 1952, -9351, -6200, 8098, 254, 1708, 3035, 5489, 748, 2953, -4074, 4798, 7840, -543, 6812, 7334, -7151, 9749, -3341, 7167, -3568, -5393, -5317, 1568, -7327, 7975, -7674, -6735, 2901, 495, 288, -809, -8149, -3675, -1754, -6288, 9681, -5871, -3298, 1785, 2397, -5470, 9869, 4782, 4015, -7405, 8286, 5245, -7638, 3064, 6404, 1370, 4683, 4600, 7892, 6221, -5584, 6810, -6637, 1653, 7715, 7047, -8989, 8682, 2604, 1857, -79, -1577, 2915, 5044, 8383, 620, 7719, -2632, -4942, 7444, 2755, -4857, 3776, -8504, 6877, -43, -8820, -9571, -3239, -8063, 4812, -2006, 3366, 2427, -4018, 2425, -7512, 408, 1992, -1420, 588, -4707, -7041, 2587, -3899, 1242, -9051, -9536, -4483, -6203, -9380, -9849, 6484, -4808, -6062, 8170, 4374, 9064, -5902, -8882, 8831, 2653, -5003, -5716, 2994, -910, -1018, -2629, -6068, 1410, -5067, 6738, 6776, -9963, -9587, 2198, 6869, -1534, 3319, -6917, 4850, 1481, -7630, 6697, 1135, -522, 8240, -2938, 3126, -4282, -4330, 6749, -8541, 4692, 9270, 5971, -8202, -9129, -8118, 8491, 5796, 201, -5507, -662, -9793, -8130, -7516, -1834, -9187, 7483, -1322, 2839, 1430, 566, -461, -6786, -1036, -1451, 7094, 1997, -4730, 2817, 4795, -3375, -9556, 5955, 4469, -37, 1602, -9457, 8452, -5413, -5737, -2723, -8838, 6092, -5039, 2178, -4676, 5700, -8270, 2205, -4787, -5745, 7357, 3024, 8538, -8763, 2751, 6095, 8115, -4431, -4590, 9650, -7639, -7077, -9255, -1757, 335, -544, 3145, -2781, -9071, 60, -2612, 9150, -421, -804, -1513, 3846, -7206, 8208, 6309, 6083, -7270, -2965, -8442, -986, -9094, 8016, 3422, -5226, -3459, 9307, 5419, -7682, -2695, 7097, -7761, 2985, 7209, 7569, 2874, -194, -6295, 1235, -7361, 1180, -1097, 9535, -3059, 9344, -6879, -2927, 2571, 3124, -8505, -1166, -767, 3378, 7315, 1509, -404, -9064, -3989, -4889, 8710, -8406, 4214, -6235, -9994, -7271, 9119, 5424, -288, 9166, -5521, -622, -1959, 5249, 1366, -2459, -3411, 9800, 1743, -8782, -2682, -1860, 5670, 9629, 1443, 3885, -5964, -3647, 1962, 8629, 1478, -461, -9332, 1638, 4557, 9381, 7140, -136, -5651, 2254, -4251, -405, 5346, -8550, -9559, 1129, 830, -3820, 3415, 7715, 1281, -605, -2205, -6671, 7793, -4427, -5195, 4332, -9158, -6003, -7611, 4541, -4843, 1331, 1957, 4711, -7253, -8621, -3304, -9595, 3526, -1459, 6651, 8469, 7775, -4881, 9864, 4341, 4705, -1052, 2173, -6585, 4637, -6007, 8498, 1998, 9302, -46, -8507, 2125, -7583, 4476, -4600, -3774, -8280, -3243, -6938, 3117, 8963, -4681, -7963, 302, -5680, 6249, 9676, 7921, -9400, 9672, -3529, -9370, 3203, 4250, 1673, -2478, -7009, 3172, 7222, -6632, 2189, -195, -6665, 8323, 3330, 9225, 6557, 2795, 4495, -3318, -338, 4156, -188, -283, 5576, 4800, 5426, 8479, -2324, -4596, -4952, 932, 7526, 552, -969, -8302, -3408, -5785, 5910, -7734, -9334, 5322, -1607, -1385, 3434, 6626, 776, -5634, 2751, 1012, -950, 9577, -2780, -3855, -7874, -619, 7199, -9819, 4444, 6664, -6444, 57, 3576, -890, 2262, 1336, -2470, -1477, 1623, -8358, 8180, 270, -9859, 6088, -149, -969, -4263, -4562, 4258, -8415, -6321, 7709, -1250, 990, 1308, 6076, -2672, -1696, 409, -6115, 9395, 2664, -8468, -6509, -5451, -1740, 2223, -4937, 2112, 8406, -9642, -5546, 2830, -3797, 6877, 7564, -6886, 9598, 2482, -6733, -6019, -6227, 7550, -3642, -4641, 8884, -17, 8617, -7393, 9826, 3437, -233, 8667, -9473, -1888, -1806, 7559, -6395, -4307, 2234, 5368, 4615, -4995, 2719, -5274, -7146, -4972, -8963, -4907, 7726, -962, 4552, -13, 8197, 8177, -8505, 4260, -8329, -8289, -1407, -777, 6357, -5770, 8946, 5383, 3566, -4917, -7467, 6959, -4903, -8981, -9131, 5679, 6641, -3257, -9617, 3983, -6962, -4236, -8268, 5978, -4210, 5681, -60, 5575, -1838, 761, 9919, 4089, -3593, -3783, -6810, 407, 7672, -2941, 1848, 5936, -7667, 9393, 4894, -3803, -1060, -9965, 156, 315, 7833, -3864, 1809, 8449, 9631, -3782, 79, -8788, -155, 5881, -7387, -4613, -6771, 3657, 7412, -7859, -3193, 2060, -6834, 8966, 4749, 9276, 9274, -8437, -2061, -1578, -8607, 4784, 7324, -8269, 5643, -3353, 6008, -6985, -9809, 638, 8983, -9473, -9843, 6730, 1287, 4698, -8855, 7260, -9810, 3923, -6909, -6779, -2914, -6110, 3854, 5586, -7885, 9982, 4881, 6927, -5311, 5759, 3526, 9628, 1905, 2954, -929, 7432, -9659, -146, 1253, 685, -2618, 8698, 6065, 4950, -8489, 9038, -3760, 4326, -4118, 191, -5033, 5019, -6514, 3282, 6656, -9003, -4006, 379, -5884, -1298, 574, 4051, -229, 5673, -6306, 6329, 7103, -5998, -8397, 8266, -2861, -5537, 3798, 7398, 5018, 7555, -7374, 4301, 9834, 5938, -4097, 2536, -2164, -9311, -2564, -6039, 3328, -2461, -3338, -6192, 8816, -7029, 2767, -4141, -651, -7810, -3037, 7801, -7902, 9078, -6880, -8129, -4144, 2844, 1003, 8474, -8361, -5438, -6104, -4798, 1018, -9689, 3715, 3506, 7989, -7619, -5673, 5749, 6946, 2434, -1801, 6529, 9091, 5855, 7455, 7956, -3188, -8203, -7137, -3150, 62, -9734, 4377, -3493, -996, 9074, -4780, -7660, -1918, -3468, -5521, 1191, -9701, -3737, 7968, -2070, 5456, 6269, 2178, -111, 4036, -475, -7544, 4893, 1502, 5864, 8272, -5137, 224, 3148, 3573, -3513, 3691, 563, -4584, 3917, 5036, 9782, -1643, 124, -8847, 5401, 8860, 5207, 5547, -3807, -2538, -5194, 4748, 33, 7095, 3585, 3714, -6929, 2069, 1456, 8172, -7699, -5855, -3112, -3857, 6561, 5313, -8933, 6425, 5666, -7668, 5944, 1513, 1429, 4605, 9348, -3752, 315, 8074, 7542, 2875, 4320, 3453, -590, 6995, -6739, -5339, -4662, -4722, -3254, -9766, 6018, 7726, 1476, 9674, 6899, -2227, -6800, 3129, 575, -1419, -6574, 2585, 5983, 5961, -2384, -7975, -5062, -8675, -9049, 4195, -3251, 4841, -4472, 4410, -2511, -2833, 1998, 3932, -6083, 7048, -5501, 4412, -6715, 3346, 2298, -4367, 8533, 8234, -9220, -7913, 6285, 7370, -2753, 581, 4809, 652, 9298, 8296, 4266, 7587, -9725, 8802, 5531, 2525, 3697, 5535, 2148, -754, -716, -9293, -6231, 2194, 4952, -2816, -9069, 933, 1517, -5308, -679, -1113, 8231, 4723, 7721, -568, 5133, 5857, 6907, 3712, 6903, 1350, 7425, 6435, 1181, 2349, 3978, 9033, -1128, 4734, -5039, 4970, -5539, 9951, -1002, 9897, -5632, -9752, 5568, -6659, -5186, 9935, 7378, 3807, -4197, 6434, 3196, 9370, 6966, 9744, -2927, 330, -8002, -4843, -2407, 4901, -4211, -9176, 7132, 4979, -7953, 4505, -8380, -6772, -2705, -9459, 6780, 4750, -8587, 2244, 1500, -8327, -1829, -779, 2247, -3881, 4936, 3257, 6445, -6108, 286, 2565, 6958, 1967, 6238, 4581, -2905, 1821, -4038, 1539, 3470, 7013, -1884, 6094, 405, 2989, -8350, 5071, 5808, 2112, -4122, -4920, -3538, -4332, -1174, 274, 4213, 9103, 6736, -8797, 8054, -6309, 22, 4906, -5813, -6840, 1897, 7012, 754, -5528, -8620, 1667, 9570, -3305, 9650, 1211, 5413, -69, 3582, 1753, -2118, -5074, -7127, -5109, -9920, 2413, 5348, -3430, -3173, 1662, -1646, -4578, -4947, -4933, -6833, -6275, -6467, 4684, 3730, -1605, 3453, 8055, -9348, -6227, 7024, -5751, -4890, 32, -5850, 2912, -8811, 8451, 8726, -2153, -3774, -1985, -2598, 8808, 9008, -6922, -2831, 9297, -7187, -8856, 4211, -3993, 3747, 9620, 815, 2086, -9960, 1199, 3738, 6414, 6641, -7159, -9890, -4708, 726, 7851, 4935, 937, 9205, -5225, 4861, 2487, 7071, -4009, 1343, 5161, -5773, -9421, 6101, 1050, -7988, 2344, 1580, 4706, -3548, -1970, 6449, 3290, 3724, 1537, -1021, -8492, 1326, 4487, -9467, 8054, 7371, 8237, -7145, 9863, 2739, -4110, 6284, -723, -5499, -3199, 4152, -3651, 3040, 9708, -2960, -3727, -910, -3154, -7870, 848, 4128, -1073, -304, -6118, -2054, -4102, 6971, 2140, -3295, -5168, -449, -7443, -7569, 7428, 8647, -7509, -1718, 2417, -9571, -3851, -9529, 8481, -2744, -7701, 5562, -3185, -4350, 5481, -1671, -9846, -917, 2724, 2000, -1329, -6928, 3104, 4670, -7926, 5551, -7966, 9791, -5412, 6412, -9097, 2188, 8232, 3840, 818, 7585, -1123, 7539, -3336, -2064, -5004, -8115, 5089, 6773, 2500, -1583, 6108, 4034, -2110, -3643, 6703, 4888, -9537, 5878, -4344, 3360, -2469, 8, 2687, 3922, 5480, -4589, -4170, -9585, 7197, 3645, 2840, -3542, -8728, -1584, 5533, -9816, 4338, -1341, -4499, 4479, 4131, 686, 3981, 5994, -8338, -6682, -8670, 2113, 5171, -1815, -6118, -2280, -4340, -2625, 4080, 8817, -6280, 834, 7871, 7429, 6217, -972, 3393, 5027, -9361, -4121, -2248, 5328, -9570, 8445, 2147, -3705, -1808, -9808, 8016, -4479, 7322, -243, 7848, -6647, -2412, -3198, -3681, -3052, -4860, 5975, 2144, 7086, -2842, 6446, 528, 9065, -1658, 836, -2812, 3003, 7084, -7959, -2011, -6362, 9650, 2099, 9930, 6572, -219, 3602, -3484, 6196, 5363, -8185, -6198, 7275, 6011, 8676, -1422, 7273, 8871, 684, -1457, 7908, -6803, -1973, -8239, 4096, -2015, 8973, 2757, 3185, 5829, -6266, -4775, -9688, -5479, -19, -3716, 5886, 6231, -2915, 5601, -5311, -6636, -8903, 5622, 2477, -3141, -8112, -1156, 9101, -3706, -7426, 4288, -5905, 5816, 4741, 2935, -5866, -386, -3131, -5136, -3689, -16, 52, 5201, -5848, 1411, 3467, 4061, 1286, -8074, -6753, 3119, 4785, -2167, -2603, -10000, 9607, 1749, -3583, 2874, -5770, -7350, 7857, -5199, -6119, 9237, 7572, 9311, 9047, -2098, -5257, -1, 4281, 6120, 6219, 2192, -1158, 1477, 8394, -8417, 241, 7037, -7020, -8231, 4717, 940, -8281, -1396, 5589, 2875, -1297, 8925, 5989, -6258, -5577, -5022, 6193, 5537, -8930, -9317, 3785, -6762, 1525, 7742, 9287, -3968, 6631, -2882, 3054, -2867, -6228, 4982, 2813, 8720, -5067, -6506, -5420, 6209, -624, -7781, -8693, 4727, -8581, -3600, -2330, 2986, -2195, 8263, -7003, -5974, -9750, -8395, 4478, 7851, -629, 720, -899, 2850, 7869, 863, -7577, 2308, 6367, -2007, -6593, -9308, -5152, -5330, 9669, 7413, -7212, 7650, 2980, 2804, -6179, 9620, -9795, -456, -6059, -5828, 3781, -1542, 203, -7982, -6618, -6955, 7022, -3022, 1983, -9798, 7531, -5604, -61, -190, 9960, 5621, 1479, -3863, -9154, 5096, 1748, -7631, 8566, 1635, 9610, -2954, 7902, 5854, 6859, -6785, 9266, -3123, -6150, -5651, -132, 6411, -8948, -8771, 2899, 4151, -716, 1732, 6163, -2573, -7208, -6214, 5802, 4419, -1125, -8304, 5364, -2202, 8137, 8783, 2572, -1145, -6677, 4285, -3243, 7472, -2024, 495, 1514, 8227, -1113, 7997, -3612, 8056, 9092, 177, 1215, 5832, 5878, 3493, 8123, -3057, -2720, -4660, 5455, -2746, 866, -9174, -6351, 7053, -3903, -2659, 243, 3653, 5263, -2381, 2801, -4226, -5190, -3492, -9163, -3628, 522, 8560, -3513, 6786, -7862, -5070, 6094, -1462, 8974, 9275, 3930, -910, -2311, 8745, 9737, 6813, 9519, 6611, 5561, -5075, 7325, -2248, -7592, -3694, 410, 9310, -2495, -5454, -6587, 5055, -6387, -2952, 195, -9970, 9912, 3810, 6248, -9494, 4359, 5898, -3939, -2125, 5798, 7524, 7965, 4594, 847, 4190, 5282, 580, 5213, 4195, -4089, -874, -2063, -2108, 3474, -7976, -8214, 2400, -3526, -9595, -7775, 8867, 5147, 3314, 393, 5471, 9756, 7225, -1437, -1331, 414, -5255, -5186, -5438, 975, 5407, 3299, 8100, 9355, 405, 6156, -5553, 7914, 1779, 9265, 5246, 5114, -4258, 9882, -1073, 5826, -5990, 5952, 2716, 9412, 31, -8748, 675, -6241, 8634, 182, 8726, 8427, 7805, -8129, -9481, 7091, -2036, -42, 5801, -8879, -5875, 770, 5038, 4660, -3049, -4029, -7316, 145, 984, -2025, 7749, 2570, -4710, -7892, 144, -9491, -8366, 321, 2451, 4660, 5950, -222, -1056, -287, -6021, 6780, -1478, -2538, 3352, 5909, -2656, 289, -1241, 2222, 1219, 6517, 7009, 8095, 9393, -2649, -1952, 3679, -5430, -546, -9517, -7397, 1956, 3678, -6982, -7940, 9366, 3664, -4264, 4834, 574, 5946, 4615, 6724, 5450, 7294, -3759, 845, -5891, 7255, 1466, -5923, -4627, -8585, 4863, 6319, -9249, 4324, -1498, -3299, -36, -8624, 8151, 7353, 9352, 1952, -6111, 4319, -1664, 6362, -8063, -98, -9688, 238, -3639, -7764, -1560, 8035, 8675, 4177, 9201, -2205, 6914, 7971, 1064, 5351, 8661, -4274, 9460, 6978, -6625, 1527, 1354, -2968, 2820, 2729, 2691, -4188, -319, 5423, 4185, -101, 7234, 1870, -2767, 8802, -8788, -3173, 9362, -2323, -9799, 7453, -6997, 381, -6542, -4015, -8391, 5365, 5657, -604, -1350, -8275, -587, -199, 2656, -4731, 2029, 7749, -5461, 3214, -9682, 6435, -9914, 94, 9607, 7879, 3412, -5839, 8910, -3692, 9821, -6998, -9638, -3918, -2315, 2042, 417, 315, -9789, -9126, 5019, -9856, -8690, 5718, 8240, -3405, -3043, 6464, 2510, -7591, -7731, 3077, 1826, 8082, -2612, -7210, 4403, 2226, -3458, -5016, -4478, 9682, -2345, -118, -8686, -208, 511, -7898, -3158, -6422, 162, -7961, -7385, -8922, 2645, 2111, -7722, -7846, -6499, -4885, -9448, -8913, 8663, -8349, -883, -2455, 7181, 5207, -5219, -9195, 7319, -8395, 4352, 8073, 6786, -1483, -3315, 5627, -1053, 5954, 8174, -415, 6087, 2253, -9945, -5161, 8282, 7369, -3073, -8786, -8914, 8094, 6001, 4834, 1051, -9436, -4551, 5946, -7130, -4447, -7747, -6857, 9757, 8384, 2449, 9092, 8296, 6410, 2448, -6964, -3429, 2250, 2458, -179, -2477, -9355, -8212, 7406, 1064, 2716, -6683, -6510, 7050, 1380, -9250, 6376, 6492, 2941, -1677, 115, 3364, -5692, 9988, 6968, -1403, 3548, -4536, -7537, 5788, -4282, 5847, 7685, 7521, -48, -1223, 6630, -5678, 9137, -2669, -5181, 6825, -2974, 7404, -7937, -4643, 2152, -4029, 1633, -5965, 6835, -3586, -8311, -7467, -5964, 2549, 3589, 1087, 2679, -2599, 2370, 9638, 8221, -9264, 7717, 8798, -8705, -6859, -8146, 9104, 3729, 7128, -2305, -4705, -7718, -392, 2461, -8888, -2338, -2405, 3670, -2616, 6203, 8022, 197, 7306, 3721, -3006, -5100, -5692, 2480, 3815, 9949, -9702, -910, -670, 4282, -5109, 320, -3062, -2958, -6517, -4885, -9036, -3256, 8010, -6274, 7903, -8126, -1552, 5295, -3223, -4834, 6571, 414, -1555, -560, 1686, 8704, -109, 2447, -6791, -8168, 3233, -4444, -4816, -5443, -5014, -909, 1878, 2626, 5690, -8198, -5161, 4006, -6131, 3393, -8979, -3144, -4110, -9764, -5295, -7758, -4853, 7193, -3363, 5492, -9102, -8926, -1569, 4766, -6945, 868, -8857, 3115, 1462, 8172, 3452, 1574, -1830, 3764, 717, -8733, -4698, 1305, -8586, -1884, 1272, 2195, -5313, -967, -6020, 4410, 8813, -4239, 1847, -6810, -7106, -9296, -4368, -9220, -8154, -7245, 5912, -417, 6757, 7331, 1902, -2355, 9054, 7402, 7452, -6192, 3318, -708, 2635, 5408, -8375, -7605, -4596, -9420, -1376, 541, -8446, 2020, -5652, -182, 2219, -5007, 871, 3177, -5935, -7030, -2773, 978, -1326, 6225, -6043, 4497, -4859, 5748, 2871, 262, -3388, 2248, 3250, -6304, 1125, -3906, -9674, 3623, -4940, -4694, 1877, 5430, -7324, 6871, 1132, 2745, 1424, 1946, 8031, 4082, -1475, -1561, 9220, 4086, 9155, 9686, 7642, 5648, 3975, -2039, 939, -8501, 2699, 7861, 3549, -2843, 7583, 4032, 456, 2847, 9053, 9785, 7700, 1307, -3144, 9307, -2688, 2486, -1092, 8728, 2098, 8139, 8638, -316, 436, 6409, -6228, -1951, 5996, 9870, -3089, -3568, -9025, -6055, 590, -4469, -1155, 2315, -1826, -9015, -330, -6927, -3520, -348, -4843, 6112, -9096, 4108, -870, 7276, -1773, 1892, 3116, -7662, 5022, 5107, -7392, 6502, -8772, 9238, 1817, -1966, 7168, -997, -1281, 5932, 3283, 3380, -6191, -9052, -3732, 2030, -9389, 2593, -1565, -6691, 4879, 749, -9649, -5813, 1167, 1783, -4674, -5225, -3289, -9943, -5903, 1963, -8983, -5052, -3215, 5313, -4343, 2768, -6953, -7475, -500, -6657, 7058, -3249, -4896, -4112, -3124, -3334, -6818, 3597, 5315, -4768, 8467, -6721, -5134, -5232, -9025, -2927, 5800, -2533, -890, 1705, -409, 6370, 5365, -1380, -7102, -5666, 8712, -2829, -1612, -5348, 1326, -4341, 8872, 4506, -2407, 6563, 112, 7109, 1447, -3434, 9495, -6396, 9681, -8984, 2660, -8681, -57, 1074, -5821, -2404, -4317, -3138, -3533, -38, -8932, -7892, 9503, -3410, -2928, 3941, 4035, -597, -5979, -1949, -9051, -3794, 6921, -4826, -2527, -4089, -3657, -7777, -9800, -4481, 9210, 367, 4287, -2169, -9155, 529, 4391, -7091, -9162, -1203, 8390, 1114, 1940, 6405, -4935, -3677, -2044, 9918, -5161, -5196, 6838, -6566, 6190, -3849, 7890, -5166, -4900, 958, -6196, 6472, -1421, 8595, -9479, -7217, -9367, -7393, -1349, 6674, 3920, 1500, 4344, 8908, 3354, -5286, -4572, 4101, -4225, 8719, -842, -8727, 4476, 9153, -7769, -3903, -3438, 8145, -6456, 7536, -9837, -5803, 489, -1202, -9048, -2722, 1685, -6775, 8264, 6829, 6363, 8997, 8787, 6598, 9124, -380, 6407, -149, 9903, -2557, 9878, -7760, -9255, 7345, -5068, -9077, -9532, -1393, -2339, 1512, 1826, 6105, 8416, -2729, 7554, -2966, 2208, 6805, 8090, -8477, -6389, -9254, -2560, 6417, 5830, 8342, -6727, 6309, -5093, 2915, -5999, -4354, -5173, 1395, 814, -5672, 3171, 6828, -5231, 1962, -8049, 8962, -1667, 279, -1402, 669, 4070, 6958, -2692, 1544, -2829, 8747, 7407, 3812, -8902, 708, 1842, 7230, 241, 7202, -4616, -9219, 9717, 1319, -7795, -1836, -7276, -9854, 2887, 7205, 8712, 1522, 1335, 495, -9203, 2818, -9785, 7586, 5820, -5006, 1998, 214, -538, -61, -205, 7546, 8704, 1104, 2831, 2072, -2602, -4510, -3800, 302, 563, -4384, 4469, 8556, -5486, -1794, -7553, -9127, 416, 5729, 5907, 5643, 5492, 8093, -3345, 5493, 3043, 6852, 1699, -516, -286, -1957, -5701, -2906, -3287, 2967, 9907, 1420, 168, -2934, -9923, 611, -5182, -6472, -8901, 410, -1372, -3422, 5559, 3708, -2402, -6823, 3671, 2947, 8244, -5333, -3808, 612, 1207, 8176, -8404, -2016, 5518, 8261, 4020, 4992, 970, 3287, 5013, -1532, 1549, -3741, -6210, 3340, -2837, -8262, -4041, -6636, -6822, 3780, 1357, -3784, -2598, 9771, -4191, -6590, -2591, -9774, -1718, -5388, -9799, -2581, 8753, -8916, 688, -7871, -9005, -7642, 5737, -3278, 9827, 6657, 9961, 3146, -9372, -9576, 4157, -4734, -9815, 3713, 4187, 1949, 3204, 4017, 39, 854, 3379, -475, -1133, -6799, 3430, 2517, 1717, 9303, -7140, 549, 903, 4585, -5010, 4681, -1601, -6462, 9768, -1762, 6155, -658, 7923, -4931, 1748, 6302, -2221, -7876, 859, 3382, -6271, -142, -223, 7026, 1236, -4489, 6587, 9168, -7481, 7250, 2936, 5955, -1821, -1887, 1585, 4405, 918, -566, 5785, -579, -2826, 729, -8436, 7918, -5651, -4313, 8310, -3697, 2864, -3653, 9991, 8193, -8352, 7839, -7676, 4548, 60, -8099, -1486, 8998, 3491, 1851, 1999, -1985, 7651, -7128, 847, -2915, -4609, -7777, -9551, -5837, 6576, -1685, 2553, -3828, 6933, 182, 9175, -7139, 979, -544, -8940, 5110, -844, -2514, 5965, -724, 795, -501, -4899, -4699, 2121, -482, -6107, 4461, 7084, 9534, -8321, -7391, -2844, -9933, 3927, 562, -9500, -2394, -9890, -5256, -5709, -2772, 664, -2130, -5276, -4344, -6662, -2365, -3750, 4714, 4222, -4788, 1189, -5545, 8196, -8390, -8019, 9216, -1843, 4680, -1648, -6459, -6504, -9735, -9355, 5489, 7390, -2048, -5157, 6984, 814, -3834, 2922, -9500, 3451, 5010, 4975, 8899, 7734, 8450, 1933, 532, -8102, -6790, 6166, 5726, 1137, -6251, -3436, 3213, -2539, -7749, -1774, -6720, -7354, 4190, -2400, 2696, 7530, -3652, -4505, 3911, -9189, -4957, -4576, 5495, -3685, -4217, -6050, 8952, 350, 7674, -7502, -6832, -9874, 6582, 8143, -3199, -8091, 1260, -2697, 7665, -5896, 8110, 6587, -5771, -7121, -8357, 3711, -3929, -9336, -5538, 373, 8608, -5368, -9656, 2751, -6144, -4087, 3110, 7880, 2781, -2672, 9161, 5967, -6095, 1278, 6084, -4428, 9355, -4459, 2648, -461, -8031, 4864, -2268, -7247, -574, -6866, 5601, 7434, -8258, -6907, 1186, 4083, 7903, -1142, -8138, 6918, -4353, -719, -2592, -5729, 5472, 8187, 5977, 234, -7268, -6400, 1109, -9965, -1371, 735, 1476, 6923, -6127, 4532, -5351, -8158, 6149, 5258, 4862, -1643, 1835, 2322, -6268, -6173, -8514, 2319, 5808, -5087, -9493, 5700, 2830, -6468, -526, 3511, -4329, 6608, -7962, -3945, 3093, 4571, 6895, -5891, 5538, 2698, 7261, -7212, -9445, 2365, 6522, 5251, -2434, 1644, -2169, 2663, 6784, -5984, -8811, 2826, 7692, 2998, -755, -2759, 8838, 9983, -7374, 7505, 4046, 9928, 7039, -4631, 1959, 9783, 1692, 7053, 5019, -3394, 8853, -2261, -8251, -5335, -6659, -3819, -4312, 2508, -2714, -1350, -1986, -8438, -2706, 2674, 8520, -8650, -3951, 5833, 1279, -883, -8808, 1761, -5619, -7631, -5504, -913, 6410, -2661, -9508, 783, 9852, -5561, 2179, 4847, 4035, 7593, -3109, -7649, 673, 7367, 2408, 6585, -2587, 7939, 3057, 4481, 1392, 175, 7800, -682, 7094, -9426, 5553, 464, -5567, 7752, 1565, 2877, -7891, -8862, -3084, -8234, 3132, -7518, -4822, 9201, 6132, 6738, -3830, 3515, 664, 1822, 5409, 6861, 5615, -539, -9602, -1007, 731, -2612, 365, -3732, 9441, -6512, 6000, 6846, -2990, 6837, 9560, -1282, -8193, -4139, 4414, 1711, -5117, -5760, 5786, -4145, 6477, -7246, -9354, 8533, 9882, 5741, -1358, 5323, 3655, 9875, -5656, -7139, -656, 8167, -272, -9757, 6244, -1694, -312, 1095, -9290, 7562, -1161, 4522, -4891, -6068, -3557, -461, 87, -2993, 7161, -8834, -5500, 6954, -1246, -2627, 499, -3485, 3994, 3815, 6878, 3314, 3958, -9370, 1592, 9162, -8166, -9937, 6029, 4021, -4796, -8916, -2830, -8641, -3169, 7230, -8850, 391, 3651, 4273, -3015, -1325, 9353, -8576, 370, 3945, -1757, 1309, 4557, 6492, 2489, 1731, -2663, -537, 7123, 5523, 4521, -1638, -3038, -5497, 7348, -1848, -3559, -1267, 4670, -9968, -8885, 9764, -700, 8252, 8535, -9700, -1668, 1924, 2345, 5788, 736, 4705, 4037, 2353, 7451, 5682, -4072, 410, -2808, 9142, -2146, 8475, 6531, 2366, 4752, -115, -2045, 1662, -8053, -4029, 9916, -172, 723, 6038, -5486, 6239, 6248, 4871, 5972, -7642, -913, 1589, -2048, 6122, -4943, -3233, 7634, -6924, 6936, 8654, -2857, 6960, 9792, 6714, -1224, -8399, 7945, 576, -4362, 4382, 1581, 407, 2574, -5298, 347, 8142, 9245, -7208, 7672, 5684, 285, 2138, -6799, -290, -1558, -5678, -4481, 9784, 9403, 2467, 4375, -4204, 8472, -9691, 3140, -6248, -3147, 2497, -9980, 8319, 8057, -6943, 8183, -9253, -2090, -1971, -3872, 2081, 6149, -6535, -2358, 2174, 9221, -7003, -6081, -9998, 5176, -4630, -1547, 6119, -7244, -8609, 8904, -3849, -5123, 3558, 4222, -9986, 6999, -4754, -3520, -7144, 2684, 9006, 8038, -3099, 57, -1120, 5057, 1417, 9271, -4990, -3031, -6909, 1044, 9267, -3356, -2013, 1720, 8402, 2766, -6664, 6543, -6256, -8711, -1280, 182, -9199, -2770, -5337, -4848, 8898, 9814, 2526, 313, 3437, -6331, 6998, 4658, 2491, 7282, -2700, -4318, 9533, 7784, 4246, 2465, 816, 9343, 5081, -9437, -4268, 7774, 4108, 1937, -2419, -9699, 6971, -3258, -2780, 5567, 681, 9092, 1024, 2972, 2479, -368, -3695, 150, -2273, 9810, 7325, -4100, 3790, 5009, -4713, -7829, 4523, 131, -4171, 794, 1009, 409, 2635, 8719, -5161, 1014, 9437, -7267, -3401, -8253, -1728, 6350, 445, -6802, 1542, 889, -8606, -9711, 5085, 1754, 3953, -1030, 1974, -2880, -5789, -6143, -7852, 5209, -4738, -396, 8730, -1797, -7168, 6725, 9352, -908, 3997, 6994, -7381, -7901, -428, -9763, -9098, -8366, -9783, -5821, 5650, -4349, -506, -1445, -3160, -7883, -3461, 8404, 1336, 9919, -9649, 8160, -7570, 3415, -151, -2916, 1672, 9857, -2992, 5500, 8437, 3012, -4069, -4152, -741, -8193, 2777, 2330, 77, 5588, -9431, -2727, 7867, 7624, -8644, -7913, -6806, 3243, -8285, 5126, -8941, -9206, -870, 6504, 4030, -2777, -7287, -2603, 9254, 7497, -5938, -3222, 4953, 4204, 1832, -2619, 6855, 8099, 9347, 5653, -1192, -6453, -8420, 3043, -4291, 8138, 8663, 6166, 4604, 2514, 7051, 4585, -3594, -101, 4425, 1945, 8520, 8290, 7729, 5380, -3730, 5007, 4437, 7857, 9560, -8678, -4941, 2051, 6723, -9903, 3340, -2361, 3435, -6306, -1396, 8745, 5781, 9890, 4020, 843, 3947, -6828, -4232, 1011, -5831, -1869, -1534, -7794, 2590, -4512, 1904, 601, 5340, 3335, 333, 7731, 9528, -1084, 6384, -2829, -5263, -1605, 5856, -3211, 5431, -2310, -2481, -2481, -5582, -1405, 5259, 4442, 80, -6716, 4368, -5094, -2938, -2281, -8800, 29, -5365, 7978, -8311, -4893, 8563, 40, -2704, 6550, 2011, 8375, -5467, -3221, 6763, -5199, 1157, 9427, 8763, 7715, -9682, -8320, -3241, 4527, 6826, -6100, 2975, -9480, -3358, 4509, -8000, 3179, 4940, -6596, 4151, -9845, 8262, -6976, -1482, -2407, -7141, 8829, -9261, -9499, -3827, 7753, 66, 4623, 5990, -9347, 7743, -6594, 911, -6432, 6290, 2140, -9146, 9054, 4984, 3666, -8855, 5724, -2081, -474, -733, 6562, 8054, 520, -5480, -9030, 1210, 7470, -7018, 8048, 494, 4415, -4016, 1302, 1612, 5364, -4705, -2449, -4880, -646, 9041, -5501, -4454, -6577, 1255, -3467, 7841, -7418, 397, 3340, 7891, -1934, -9857, 3081, -8815, 504, 3397, -408, -5234, -8442, -9657, -1191, 6875, 295, -4804, -9033, 2876, -2920, 9999, -6651, -1375, -4879, 1887, 7590, -5475, -9071, -4416, -1407, 9207, 8585, 3902, 7820, -3459, 6616, -9328, 7210, -9779, -842, 4872, 5131, -620, -6851, -3051, -8383, 6266, -8365, 602, 2820, 7133, -6021, 1459, 7226, 5847, -6389, 8953, -5951, -3934, 7076, -3453, 7043, -1352, 5456, -4964, 5146, 2731, 6904, -2581, -7455, -4696, 9699, -983, 6513, -543, -5735, -3463, -3282, -2788, 4971, 112, 4451, 9821, -9401, 8990, -6017, 4113, -9672, 2435, 1872, -8594, -945, 2046, -1197, -5997, -4683, 3502, -1103, -690, 7908, 6746, -6492, 4468, 9954, 7350, -8674, 671, 1506, 2489, 6150, -6837, 6066, -3164, 2902, 9713, 6948, -361, 5647, -3491, 8086, -2273, 5257, 341, -1693, 3667, 3206, -3492, 4633, 3860, -8236, 8929, -5819, 7229, -5034, -2691, -8876, 7466, 8917, 9382, 3731, -2297, -8722, 8616, -3974, 1445, -2879, 9812, 3898, 6681, -2048, -2850, 7504, 6035, -8271, -7321, -6822, 710, -3584, -4409, 7102, 2657, 5099, -7226, 7025, -3004, -7969, -7476, 5956, 3867, 745, 5219, -8361, 3574, -7616, 9365, -4898, 5460, 7609, 6277, 8863, 8340, -6383, -684, -550, -9908, -8781, -1776, 0, 3344, -1080, -6083, -4814, -9530, -7369, -7545, -6833, 7090, -2042, -9185, 7075, -5713, -7412, -2214, -4652, -5423, 7993, 1763, -1712, -3753, 5774, 5605, -2960, -5953, -6402, 706, -874, -2608, 5739, -9859, 2215, -8282, 5004, -5909, 7546, -1779, 1906, -6568, -6145, -4569, -7216, -5300, 8569, -2206, 8249, 7751, -9697, 5163, -8264, 8793, -3182, -638, -9864, 9914, -4125, -7824, 4435, -4608, 7567, -7774, 5300, -5469, 1224, 2868, -9488, 909, 1263, 2008, -5668, 9048, 7920, 2287, 1687, 259, -5719, -8320, 2669, 4654, 7394, 9931, -6672, 7737, -1858, -2396, 9326, -4606, 1595, 1648, -2215, -4292, -5920, 6306, 9414, -7445, 1091, 3214, -5837, 6358, 7934, -4599, 818, 8349, 6073, -7678, -2791, -6623, 668, -4690, -8479, -6406, 3388, -9902, -2647, 2608, -6990, -3556, 9987, 4936, -3381, -4935, 6772, -9038, -8187, 6000, -9557, 8227, 2610, 5258, -9110, -6120, 1246, -1379, 8214, -6328, 4062, 580, 8265, 39, 7776, -2847, 8824, 2143, -825, -7266, 8001, 192, 9601, 2021, -2960, 5941, 7827, -1471, -7447, 4036, -3512, 4797, -9663, 5643, 8285, -213, -1306, -4624, 7528, 1452, 5549, 4824, 451, 1566, -7847, 7428, -567, 3099, 9088, -6534, -581, -9254, -412, 886, 5077, -2394, -2846, -8820, 6936, 9987, 9342, 1906, -7777, 4452, 7398, 6778, 7093, 8964, -1873, -9197, -8605, 6906, 9773, -9329, -7473, 1135, -5083, 6656, -9511, -5772, 7043, 3748, 9174, 728, 7067, -6682, -2021, 7545, -5478, -7400, -5497, -1733, -1524, 8161, 5378, 2512, -8231, -1351, -775, -4587, 6548, -2430, -966, -527, -8449, 7153, -1355, -1199, 3309, 1875, 1102, -2683, 5134, -563, 5823, 5698, 9192, -2653, -7943, 296, -6922, 4461, 6345, -19, 2112, -9361, -9469, 1812, -8351, 3663, -2324, -5982, -2577, -8690, 8672, -253, 6999, -1651, 7893, 9991, 2188, 4512, -4978, 6887, 7732, -5414, -9747, 6408, -6955, 3944, 9047, 9489, 1411, -376, 756, 1093, -8970, -8033, 7336, 6855, 2808, -7766, 6856, -9161, -518, 4064, -1615, 6855, -6349, 5811, -8998, -478, 6387, 7783, 2574, 2761, -8489, 1056, 8522, 2526, -335, -602, 4946, 3365, -1371, -2619, 6047, -3433, 927, 907, -8669, 536, 933, 4340, -6899, 6466, -4553, -9261, -6602, 3814, 1402, -4431, 8207, -2454, 6582, 3317, 7610, 2777, 9083, 5209, 5228, -4646, -567, 5433, -7377, 2464, -7302, -828, -2324, -1829, 7551, 2647, 2917, -1791, 9704, -2591, -2266, 9825, 9796, 9520, -4992, -8327, 763, 6196, 9514, 4724, 6700, -124, -9109, 1769, 5592, -5230, -6930, -4094, 179, -9657, 4925, -9721, 2244, -3608, -4322, -9341, -8221, 9722, 5524, 9649, -7671, 4826, 4462, 2783, -3086, -9696, 6656, 8428, 262, -1383, -3485, 9820, -8414, 1859, -1384, 5015, -8602, 8845, -1124, -4935, -8382, -7760, 8536, -2466, -7652, -2626, -9825, -8090, 5473, 7385, 5111, -9852, 3027, 7658, -3204, -2756, 405, 2659, -689, -9367, 3589, -3590, 794, -2674, -6945, -2292, 5350, 2297, 8207, -6396, 117, -5916, 3188, -5967, -1710, -2839, -470, -8284, -5617, 345, -5978, -630, 8298, -6514, -4477, -3132, 6631, 8673, -8974, 5501, 3184, 7129, 5880, 6884, -1746, -4304, 5284, -9217, 4675, 6031, -655, 6933, -3024, 7467, -7376, -7444, -6879, 7364, 8950, 7408, -460, 6607, 5980, -4812, 7110, 383, 1279, 7604, 9013, 5751, 7133, -2743, 2779, 975, -9514, 7518, 8965, -2502, 7969, 6804, -9653, 6222, 8133, 288, 5037, 5938, 2152, -8888, 931, 2873, -5284, -1566, 1313, 1150, -6926, 8211, 5260, -3008, -2889, -4805, -2658, -1027, -1177, -4153, 3587, 3300, 5824, -131, 6263, 9588, -4953, 6915, -3681, -3317, 2790, -3120, 9149, 2452, 503, -2902, 1548, -6322, 9706, -9097, -8821, 7953, 8048, -4174, -1428, -5035, -8071, 8479, -6976, -6219, 9039, -1885, 1978, -9401, -5569, 445, 2827, -441, -7760, 7620, -9422, -6433, -3371, -4706, -1652, 2666, -4881, 3411, 7583, -1724, 1450, 468, -6298, -6335, -3506, -7907, -6235, 1854, 3681, -1510, -3015, -4192, -2087, -7694, -9024, 814, -6958, -6031, 7681, 2056, 9061, -9626, -2765, -8917, 8597, -5971, 2038, -9533, 7919, -998, 4142, -8596, -9036, -481, -7011, 4029, -6077, -3599, -8375, 5562, 2030, -745, 8455, 5224, -5495, 8218, -9513, -3251, -2827, -7223, 4143, 6712, -4298, 5889, 3976, 7034, 2520, 9167, -4243, -3744, -9385, -542, 4153, -1440, -6368, -3668, -7861, -1687, -414, 2318, 2857, -6674, 4240, -2536, -5054, -5313, 1189, -1398, 5085, 1805, -2069, -4401, -5101, 2960, 2295, -889, 8959, 6189, -4837, -4965, -4236, 9207, 6687, -6766, 8695, 6737, -999, -4123, -7331, -3804, 469, -1776, -8985, 2633, -107, 5350, -73, -9016, 855, -7505, -6878, -8496, 3711, -4341, -764, -5277, -3495, -180, 6017, 7243, -3294, -8919, 498, 6388, 316, -3438, 2083, -5315, 3179, -6697, -456, -7413, 1577, 593, 1288, -4135, 9813, 8848, 707, -3657, -8902, -2104, -391, 6355, 6918, -9082, 4231, 1461, -6227, -5364, -4527, -7887, 4342, -6728, 7287, -3392, 9116, -7854, -2009, -1811, 8177, -6442, 9073, -2640, -4791, -8477, -1234, 815, 2464, 7914, 4401, 205, -9907, 8651, -894, 3732, -9253, -3793, -6930, -9759, -180, 5527, 3696, -6462, -6243, -9080, -1885, 8638, -926, -6667, 9606, -5464, -1946, 5483, -8577, 8266, -9437, -3048, 4963, 8228, 8758, -2509, -2359, 5238, -8114, 1824, -4753, 9502, -6672, 6323, -6961, 6675, -1165, 4063, 2970, -2874, -5684, -353, -2616, 7886, -2516, -8740, 2429, 3123, -2188, 5176, 6593, -9230, -800, -450, -796, 598, 2298, -9604, 8861, 7334, -228, 9213, 465, 5196, -2018, -1070, 4370, 181, 7107, -3300, 6640, -9709, -7835, -7323, -4457, 99, 6970, -9200, 8996, -1284, 9190, -4682, -8992, 1336, 4240, -4714, 2647, 2622, -9847, -8170, 4435, -9789, 8030, -7283, -2685, -7141, -880, -758, -1176, 6011, -4472, -2548, 8267, -4611, -9694, -3167, 7544, 9021, -3666, 8591, 7379, -1627, -6340, -9622, -2310, -892, -4852, 4458, -5150, -7443, -4757, -1949, -3942, -8117, -7866, -290, 4275, -6873, 9012, 4548, -5536, -8002, 9296, 8691, -8749, 5112, -5976, -8140, -3807, -7048, 2457, 8268, 1638, 6692, 9960, 3220, -5530, -6434, -1224, 4454, 118, -1707, -8462, -9046, -5307, 4914, 1377, 6544, -5940, 5131, 8198, -7781, -3188, -7150, -6351, -5906, -8252, -2568, 5692, 3175, -1046, 7056, 2366, 4489, -4237, 3626, 4303, -324, -9431, -6521, -697, 261, 4698, 5503, 5307, -6856, -1065, -9625, -1684, 8985, -8706, -2032, 7975, -105, 5625, -817, 4870, -1988, -7302, 2060, 4532, 6228, -2957, 6219, 1449, -4607, 9599, 1566, 6463, -6432, 5153, 2131, 4742, -7576, 3658, 5476, 6576, 8108, 5911, -8669, -8306, 7786, 7023, 2606, 9488, 3598, 815, -7507, 968, 3416, -567, -2554, 1968, 2830, -2108, 587, -3904, 6877, -3787, -9096, 5188, -8078, -4383, 9990, 5299, -9094, -5480, -71, -1776, -7596, -3146, 3855, 378, 1219, -7445, -8573, 3334, 9271, 3702, 5747, 9794, 8565, -6983, -2193, -7090, 7138, -9925, -4797, 1924, -3086, 7925, 2513, 1678, -6531, -1066, 6708, -7056, 9954, 7674, -88, -255, -8785, -5105, -5155, -9870, 9290, 7895, -9803, -2727, -6233, -2735, 3808, -4013, -7101, 1604, -2061, -7667, -1232, -6692, -7333, -4105, 499, 759, 4806, -749, -2333, 6017, -831, 5252, 1642, 2639, 3350, -6165, -9369, 768, 1715, 541, 8383, 347, 6244, 4348, -5941, -339, -1929, -3871, 3078, -7573, 5447, 2848, 1323, -2473, 8654, 1682, 6706, -1953, 6059, 5279, -154, -4372, -2813, 9335, -7807, -3682, 9403, -1171, 9171, 9405, 5850, 5381, 6977, 4825, 6250, -5669, -3490, -2739, -5078, 553, -2090, -6558, -2286, 8343, 418, 6555, 6544, -6719, 9386, -1911, -5508, 9356, 599, -5283, -1918, 2545, 3873, -8995, -7769, -4989, -2602, 1943, 1348, -6733, 483, -5111, -6031, -8652, -6953, -9884, 3662, 5831, 8006, 9239, -9030, 3609, -4263, -6141, -3380, 7556, -9281, -6505, 5590, 4731, -1051, -2122, 5280, 9851, -1101, -4426, -2444, 624, 7053, -4921, -4438, 4923, 5549, -6162, 3816, 5832, -752, -8781, 3752, -9756, 6024, 3200, 801, 8502, -7054, -1379, -2516, -152, -6369, 9929, 403, 7389, 5023, 2638, -9747, 2689, 6765, -9728, 6430, 1985, 4006, -1924, 1476, -9170, -429, 3113, -4937, -3744, 5957, 454, -8750, 1318, -5206, -799, 9548, -4311, 8551, -5357, -8820, -7889, 6304, 8885, -9378, 1822, 3451, -3313, 1380, 1005, -7931, -6697, -5982, 6192, -4856, 9953, 4259, -1525, 4403, -9650, 532, 7329, -4861, 2988, -1521, -5821, 8510, 8458, -1700, 9570, 4948, 1861, -9641, -3149, -131, 6434, 1715, -7064, -8674, -8719, -6466, 8013, 1668, -8097, 9949, -380, 5793, -9843, -1194, 4716, 2801, 475, -4980, 2115, -7000, 1548, -2292, -9614, 1498, -4881, 6920, 7568, 3201, -3080, -4957, -6638, 1096, 3199, -8625, 1452, -4996, 2766, -8200, 9893, -5525, 2763, 5906, -7824, 1586, -6714, 6329, -9029, -307, 6254, -9841, 4803, -6197, 1048, -3363, -7611, -1725, -5749, 9343, 9296, 7989, -8943, 5333, -4229, -6659, 989, 2412, -2959, 9186, -9210, 3953, -5956, 1445, -3968, -9405, 3661, -5417, -2965, -3901, -374, 1247, 9005, 3507, 8984, -3730, -8091, -8896, -7485, -4586, -6356, 1552, -1767, -2606, -7758, -4014, 5960, -2232, -2892, -730, 171, 1005, 2547, 277, -721, 8790, 1821, 4510, 9168, -475, -1877, 4358, -3971, 1958, -6347, 9489, -5411, 7169, -1410, 3877, 6799, -1420, 8480, -9946, -4650, 3082, 9444, 3563, -6936, -69, -6231, -3309, 7972, 7554, -9476, -3290, 2997, -8835, -4452, -2216, 9221, -9517, -4453, -3395, 1143, -6747, -4459, -1220, -8276, -6721, 1178, -9934, 2774, 3048, -5709, -9843, 9307, 9000, 9296, -6755, -1846, -3180, 3460, 9237, -648, -7141, 6360, 6345, 4342, -6342, 620, 2559, 8208, -1032, 8819, 7677, -8942, 3064, 7833, 992, 8098, 8233, 8878, 5048, -679, -2671, -3259, 8152, -4086, -8186, 4278, -8360, -1470, 8953, -215, -8024, 9184, 5843, -1782, 1503, 1343, 394, -3767, 9553, 3547, 2881, 9950, -8423, -1494, 8516, -4273, -5535, 1220, 2689, 5538, 4668, 6295, -22, -5989, -2526, -1767, 3935, 2851, -4240, 7962, -5364, -1618, -2759, -3398, -4290, -6190, -1916, 3823, -4373, 13, -745, -7884, -5414, 7882, 3495, 5789, 1569, -7491, -861, 3239, -3145, 8695, -6925, 1643, 1769, 6986, 7786, -7239, 7040, 1699, 2258, 4279, -7874, 4876, 1189, 4295, -9993, 2800, 9146, 5374, -7400, -4550, 9238, 6687, 7914, -63, -4619, 2849, -4292, 9255, 681, -5603, -6662, 4571, 1510, 7782, 8215, -3510, 5952, 7109, 6284, -4570, 2339, -3830, 9136, -5932, 504, 1808, -9647, -8738, 1372, 4260, -2205, 724, 2202, -461, 368, 4091, 1841, -5068, 4289, -9931, 8670, 7897, -8237, 5554, 3679, -4682, -4073, -2302, -2609, -9051, 6409, 8203, 4313, -1550, 1149, -1518, -2465, 2168, -6615, 551, -1350, 3638, 2163, -8900, 2480, -7497, -4892, -1149, -2604, 858, 9694, 8875, -3064, -6373, 2408, -8020, 5836, -9511, -1026, 2517, -7507, -4804, -4058, -2387, -4400, -1741, 665, 802, 1029, -3449, -7935, -7495, -7598, 9356, 1140, 7994, -6552, 9208, -713, -1210, 1848, -6251, -1724, 8462, 6835, -1960, 1296, -5142, -1716, 2813, -705, 9841, -7987, 9737, 7913, -7726, 2151, -5112, 4494, 7539, -3085, -4314, 2073, -2596, -8005, 7606, 7481, 881, 3888, -1329, -1603, 2762, -9029, 746, 2297, 867, 6773, -246, -7424, -4868, 7658, 4399, -1585, -9665, -7987, 4576, 958, 2461, -7641, 4238, -6038, -6414, -7678, 4517, 9111, -4779, -4034, -9524, -6022, 1520, 8919, 9727, 2266, -1587, -105, 4032, -5048, -3834, -3101, 6905, -7159, -3045, 9504, -4675, 286, -9441, 6028, -6031, -7417, -6749, -4416, -5556, -949, 5398, 2519, -8420, 9587, -7858, 5986, -7640, 9684, 7888, 8183, -4999, -4657, 9990, -7792, -5859, 1668, -5181, 3255, 7834, 7551, 1821, -568, 9412, -7457, 7548, -7702, 6564, 852, -4600, 4194, 8980, 2291, 4286, -8960, 4377, -6536, -4149, -4751, -1843, 176, 2593, -781, 2374, 5662, -6189, -9335, 727, 1216, 9512, 5436, 4675, 3776, -411, -4263, -8487, 341, 2836, -3426, 3345, 7369, -6806, -6751, 524, 3299, -1739, 5339, 2938, 158, -6139, 3565, 7897, -4338, -2616, -8201, 5306, 3271, 3337, -1605, -5415, -9817, 5386, 2859, -8193, 1946, 2906, -5538, 8816, 6637, -5247, 7150, 8239, 5250, 5710, 8083, 6327, -6273, -5638, -3617, -1917, -7464, 2525, 6198, -8756, 9355, -6900, 456, -7247, -1866, 9466, -2606, -7112, -604, 9011, -4377, -5501, 8880, 4347, 4884, 8732, 5272, 6483, 5509, -6119, -3351, -5985, 3186, -9582, -2823, -2566, -2468, 3182, -6550, -6930, 1529, -1540, -872, -5665, 2413, 8316, 2677, 5602, 6883, -6136, -1275, 5552, 3826, 2719, 79, 2609, 1486, -4378, 3578, 2300, 3532, -4082, 5420, -2672, -5647, 157, 76, -2060, -2371, 2190, -1118, 2859, 2078, 9240, -1417, 7819, 3057, 8552, 1917, -717, -7466, -6523, -4158, 4280, -5176, -801, 6066, 9035, -3726, -2043, -7287, 4458, 1972, -1871, -6491, 8623, 9253, -7090, -6810, -1552, -8556, -9700, -2728, -3029, 811, 2502, 3489, -4947, 3355, 9860, 2558, -4323, 4086, -792, -7926, -2425, 4422, -292, -5472, -1085, 7271, 1321, 9911, 8487, 2743, 7219, -7003, 5940, -1310, -2589, -4592, 4308, 7469, 3319, -5198, -1723, 6809, -2521, -5092, 5510, -8542, 9477, 7455, -811, 711, -470, -2143, 173, 399, -138, -9535, -362, -8848, 9460, -8749, 1584, 8738, -3260, 5835, 9004, 893, 4398, -5517, -25, -3433, 1953, -6598, 2864, -1157, -9361, -6473, 5120, 7570, 4878, 7439, 1247, -5925, 6044, 4489, -6206, -4116, 8278, 7245, 8522, 2249, 9828, -8308, 5016, -4208, 8407, 9255, -4487, -2991, 9444, 2550, 3213, 9765, 3909, 4277, 440, -640, -5632, 3318, 8804, 8526, -6953, -479, -4856, 113, -4715, 1373, 2001, -3304, -3509, 1442, -345, -8939, -1124, -7387, -5722, -3313, 3541, 7405, 2634, -1956, -3664, -3398, -5698, -178, -8448, 3481, -3627, 9418, -3615, 4941, 4716, -7723, -6958, -5781, 3388, -5374, -173, 1704, 3049, -8139, -5430, -5974, 9817, -6849, -1311, -6399, 7710, -7792, -5671, -1533, -6416, -3224, 7178, -5547, 6424, -8537, -8969, -1532, 5533, -368, 3093, 5021, -9484, -768, -8619, -3695, -9314, -9496, 4007, 5815, -6442, 1934, 1514, -4238, -8078, 7350, -8431, 4316, 1708, 1716, 7395, 2068, 1868, -8942, -9217, -181, -5001, 9378, -2030, 6931, 2085, -1666, -5374, -1008, 1081, 5013, 5042, 1050, -8081, 7811, 4881, -5478, 6003, -6755, -9826, 5589, 5227, -38, 5986, -6524, 907, 2869, 358, -7841, 6516, -2971, -8047, -8000, -157, -2784, 6288, 34, 5263, -3909, 7017, 7982, 2982, 8132, -4878, 9886, 5370, 4210, -6714, -7353, -4740, -4503, 585, -9275, -5116, -6182, -4967, -4599, -7052, -2086, -5944, -9107, 1100, -3815, -7393, -362, 7057, -399, 4872, -6343, 6199, 4742, 1524, -5526, -6624, 6957, 2069, 5652, -7065, 8398, 2824, -3250, 355, -7335, -2784, -5588, 1900, -6431, -6085, 321, -5480, 1798, -629, -638, 268, 5368, -6439, -6910, -4681, 496, -6203, -5102, 8528, 6474, 4749, -3190, -2848, 8160, -8392, -3318, 8627, 4923, -3537, -3901, -1674, -8927, 3017, 5233, -1651, 3307, 9372, -1474, 9948, 3684, 5883, -4273, -7966, 4276, -8269, -4718, 1139, -2542, -9995, 9715, 810, -9547, -2315, -6282, 3770, 5234, 9448, -2277, 444, 7089, 1954, 5388, 8212, -2382, 709, 8155, 5514, 6004, 3560, -9950, -9177, -43, 4284, 5095, 8091, -4222, -495, 1726, 4674, 4728, -2538, -6243, -6153, 730, -7453, -146, -2702, 3681, 2351, -4110, 25, 7577, -7677, 2831, -8642, -1692, -9356, -33, -2405, -5627, -6569, 1498, 9509, -8184, -4452, -4521, 3301, -2443, -6295, -783, 8929, -8299, -4951, 9073, -9668, -4665, 5871, -6098, 1015, -6351, -8646, -4501, -8755, -2389, 1763, 3328, 7821, -4611, 5572, 2988, -485, -7583, 1623, 9180, 4733, -8527, -8636, 7819, -9745, -7024, 6973, 4231, -6985, 3950, 9281, -2635, -4959, -2685, -5452, 9678, 9879, -4930, -2516, 8812, 6584, -8319, 239, 914, 1991, 545, 912, 1153, 403, 9303, 9634, 6259, -9030, 1616, 1280, -6940, 8919, -5305, 8093, 6107, -9514, 3699, -995, -113, 1633, -7389, 4980, -5128, 8438, 9341, -2593, 2765, 2802, 7977, 4354, 2176, -4622, -7203, -4856, -4861, 8857, -143, -3583, 9816, 8481, -3924, -3441, -8199, -3965, -8291, -7650, -1457, 9442, -4647, 3680, 4022, 3334, -4410, 8938, 9522, 9162, 7139, 8330, -2501, 2352, 4533, 7216, 5155, -1247, 1273, -7904, 382, -8830, -6376, -8696, -3185, -5265, 4215, -5945, 4162, -222, 4181, 3396, 6543, 9155, 103, -9552, 1501, -4755, 782, -9566, 47, -8637, 9834, 2751, -2628, 8455, 6155, -2628, 5005, -4888, 2426, 6249, -8044, -5194, 6437, 5848, -8804, -6238, -6028, -3432, 3028, 189, 1164, -73, 8146, -5075, -2473, -4896, -4232, -7044, -8633, -97, -5527, 5909, -3865, -9114, -9274, -3784, 9965, 6912, 7342, -9351, -6963, -6046, 2658, 2614, 1951, 3771, 6038, 9123, 6178, 2031, 7885, -8627, 3148, -3175, 6930, -3501, 4022, 8620, -3731, 8812, -4603, 7945, -7291, -5326, -9561, 1847, 7301, -5314, -5661, -7453, -269, -2427, 8778, -3597, 122, -9474, 7225, -7965, -6144, -98, -4940, 1321, -6272, 5971, 8451, -2020, -5177, 7835, 9814, -557, 6225, 2534, -5220, -3565, -4614, -9649, -8617, -6466, 6241, -5575, 5363, 2348, -9226, -8132, 8185, 3337, -9724, 2523, 6639, -5987, -7789, -2746, -7223, 6080, -7652, -6763, 7123, -5506, -5721, -4629, -1947, -672, -8988, 5561, -3762, 1171, 2610, -5147, -9686, 3174, -667, -46, -1574, -7925, 7986, -5364, -5108, -4242, 330, 1258, -6534, 1543, -9958, -5155, -5181, -1852, 3300, -5062, -9075, 2485, 4475, 587, 2715, 9241, 7752, -5005, 4139, 5688, 9837, -8560, 545, 2127, -216, 8855, -4116, -5740, -2633, -4558, -8758, -9887, -4751, -7424, 7925, -1067, -8514, -1428, 8699, -1082, -2725, 3949, -2937, -4079, -616, 261, 2387, -9645, -744, -5626, 4528, 6658, -8041, -7334, -7243, -2216, 5411, -7577, -8760, 7728, 8183, -6445, -6095, 4610, -7687, -8717, 6050, 1860, -6646, -3559, 3307, -1083, 2795, -9808, -4263, -4265, -7552, 9379, -6405, 2659, -8817, -5730, 8541, -9858, -7362, 151, -2682, -3042, -1945, 1102, 8674, 3934, 9652, 9, -852, -6379, -8509, 639, -9746, 5857, 938, -6385, -4794, -7119, 6866, -7321, 1107, 3734, 2059, -6266, -9017, 8242, -2868, 9043, 7161, 9648, -8179, 9775, 3333, 4505, -4506, 5291, 9807, -7133, 5306, 2333, 5789, 6743, -9502, -3145, 3900, 6012, -5684, -974, -4135, 835, 5437, 2689, -8486, 1056, 6321, -1807, 4653, -6147, 4155, 4697, -3893, 1325, -6986, -6596, 6361, -1211, -5222, -6536, -6830, -4764, 5596, 8809, 8953, -720, -9242, -718, -5787, -7098, -5757, -6213, -2318, 6819, -808, -2305, 8602, -6181, -5363, -4788, -6485, -3058, -115, -5669, -1561, 1430, -7098, 1503, 3986, 3471, -3126, -7124, -5200, 1870, -4869, 5869, 9218, 8029, -5260, -9454, -5356, 6184, -547, -2224, 4912, -5251, -2016, 9287, 9696, 6742, -2710, -45, -2170, 3972, -7647, -1413, -1556, 5232, -2916, -8270, -6045, 7798, 9325, 1890, -9051, 6844, 8018, -9354, -3173, 3956, 1468, -8436, -5632, 6311, 1545, -163, 6498, 1419, 6929, 4541, 1113, 5632, -9255, 7216, -6422, 4695, -9844, 9545, -3941, -4146, -1747, -5345, 1833, -2571, 7799, 8695, 6004, -829, 1540, -9890, 4154, -7774, -4744, -4584, 5292, 3265, 9853, 2465, 9841, 5237, -8030, 895, -6959, 5968, -1974, 1252, -8932, 948, 5668, 8235, -1858, -7719, 4031, -7732, -5575, -7673, 5304, 3093, -9309, 4927, -3112, 6294, -7941, -270, -3415, -8456, -1186, -473, -9900, 7681, -1587, 7404, 3884, 7140, -5486, -5076, 3992, 3062, -5969, 8457, 4193, 2491, 189, 8247, -9401, 682, 4412, 6600, -7384, -621, 9297, -3409, 3878, -547, 2178, 3961, 1260, 2173, 2288, -6591, -9443, -8068, -6208, -9329, 9978, -7490, 1443, 6477, 8906, 4404, -9494, -5956, 6669, -5053, -2247, -4204, -4777, -9567, -1411, -5590, 7867, 5589, 2115, -8390, 6380, 5775, 6674, 2198, 4669, 318, 4919, 4774, 4493, 2694, -1444, -7395, 8891, 4047, 420, 8033, -1150, 5809, 1142, -4957, 356, -4985, -2239, 7755, -132, 261, -1658, -6473, -2112, -5665, 9821, -2488, -6775, 3137, 3942, 15, -1885, 4463, -550, -4339, 1735, -1158, -2788, 556, -2609, 2717, 5108, -5775, -877, 6329, -4047, -8469, 6309, -2951, -7596, 4554, -8432, 1693, -312, -8294, 8428, -8760, -7243, 2485, -5989, 919, -528, -5881, -4844, -93, -50, 4801, -6276, -4922, -1879, 7102, -654, 1885, 7802, 9345, -5157, -5944, 4965, -762, -463, -5352, -8161, -7684, -5969, -1786, -1394, 4082, -6971, 883, 4638, 9210, -2555, -6637, 6309, 6090, -5722, -1172, -4765, 4235, 7389, 5691, 888, -9394, 7472, 2607, -7269, -8039, -9703, 263, 6008, 8849, 5877, 3503, 6352, -8878, -2775, -3470, 7290, 3620, 2569, -1545, 1287, 4859, -9833, -9111, -1806, 7024, -8062, 6829, 909, 3242, 9722, -8455, 5748, 6692, -4406, 6431, 431, 3649, -2944, 1354, -2676, 1701, 1513, -5008, -7036, 1344, 6267, 6125, 1198, -7738, -4197, -4790, -8803, -1412, -569, 7654, -4889, -3199, -8565, 3562, -8921, -7440, -9098, -4705, 6446, -5516, 8789, -7830, 4740, -2455, -2513, 7497, 718, 6377, 2241, -9781, 8887, 9912, 3685, 7781, -6091, 2207, -3450, -8764, 958, -3583, -9013, -4823, -4556, -3625, -2981, -3243, -6893, 8104, 1587, 7182, 9449, -5358, 9756, -3630, 8891, -5862, -8113, -1252, -3513, 7584, -7844, -5916, 2985, -3420, -5017, -6309, -8178, 3160, 5235, 5236, -1127, -765, 2247, 3963, 1524, 2270, 3037, 8039, 969, 681, -4973, 2014, 2037, 8594, 18, -8635, 98, -1713, -2583, -5477, -3471, -6436, -5769, 2280, 1697, -5796, -8394, -1795, -2545, 9112, -8540, 1684, 6008, 3663, -4856, -9935, 4825, 241, -8529, -26, 6195, -7759, 7900, -1009, -4611, 5629, 186, -7537, 8258, 3183, 4016, 4324, 9017, -6670, -9650, -1287, 2860, -3161, 3815, 8445, 3848, 2216, 7048, 7933, -1221, 4439, 3398, 1474, -3910, -7893, -6300, 9510, -8195, 3171, -8776, -7811, -2852, 1776, -7684, 9943, -4859, 2661, -7969, -1863, -3630, 7033, 2300, 624, -9291, 3628, -8503, 3404, 3061, 1385, 1346, 5317, -6719, -8351, -1289, 812, 7870, -4190, -3204, 440, 1113, 4205, 4094, 1456, 4356, -6360, -3984, 9371, -7677, -4388, 2032, -5112, -2893, -3385, -5783, -5256, -8388, -5803, -6225, -7719, 9722, 3923, 2428, 4462, 5222, -9418, 6987, -8873, -4816, 175, -9539, 4130, 3815, -528, -9395, -3023, 37, -4393, -6351, -7708, 6180, -4031, -6164, -6126, 4388, 4661, -1405, -7792, -4007, 4515, 5724, 6585, 8618, 9894, 755, 7412, 9247, -9554, 6717, -8897, -4251, -7953, -1380, 8404, 5710, 5137, -798, 4359, -1448, 535, 8844, 603, 3672, 6077, -7652, -6982, 8433, -9662, 3543, -2, -6576, -1714, 3792, -3609, 2311, 1205, 824, 1241, 7881, -1857, -5295, 4322, 2690, -802, 1312, -3315, -211, 5595, 1163, 890, -5700, -8863, -488, 1713, -5571, -4306, -7862, -8884, -5566, -5449, -3739, -5901, 7692, -4336, 3529, -2372, -6168, -5996, -2072, -1087, 1594, -9429, -7926, -6471, -715, 1336, -893, 7419, 3958, -1602, 8814, -1896, 9835, -4293, -3160, 9747, -1712, 1799, -2010, -9830, -5832, -9470, 4876, -7078, 6970, -8087, -329, -6090, 2627, 4617, 9595, -1053, 244, -8782, 8681, -936, -7038, 662, 5751, 1632, -2009, -2660, 1048, 4703, 3472, -5305, -2105, 5507, 4701, 8402, 9053, 4619, 1950, -4235, -9042, -1571, -3564, -3896, 3307, -8924, -7110, 3022, -8957, 4509, 5803, 8500, 8942, 7105, 8285, -1854, -8453, -6889, 90, -4752, 8552, 8722, 1295, 9785, -210, -664, -3023, 6113, 2800, -9767, -7648, -6299, 4192, -915, 5672, -9447, -7448, 1393, -4981, -1837, 1668, 6223, -9177, -2113, 542, 110, 5192, -1160, -1020, 2285, -9596, 4654, 3088, -6142, 7734, -297, 3453, 3481, -1471, 1402, 9740, 8278, 9314, -8683, -737, 4043, -5731, -3338, -4444, 7220, -9302, 9658, -9758, -5114, 768, -60, -3535, -5264, 4353, -4386, -4169, 3524, 209, 4641, -5027, 837, -1278, 1063, 6674, -7159, -9668, -8089, 2172, -6951, -9399, 9945, 3336, -9341, 8526, 3921, 6599, 1792, -58, 4298, -3321, -5544, -9596, -9956, 5354, 7999, -6447, -1270, 2569, 5208, -4095, -2429, 7090, -4521, -8313, 4722, -7592, -8844, -3502, -7305, 2459, -5016, -2818, -2118, 2037, 2269, -9466, -6232, 5737, 8853, -9147, 9539, 5078, 2615, 6388, 9341, -1773, 9364, 6810, -3157, 2136, 8943, 1506, -510, 2294, -2545, 6113, 2861, -560, 3633, 8473, 5542, -4188, -1978, -5714, -4993, -6023, 1078, -986, -3303, -1629, -7586, 8693, 636, -2788, -7996, 5115, 1009, 5412, 6974, 8938, -7118, -4789, -3860, 5295, -4363, -7515, 280, 3253, 9258, -9269, 8561, 1390, 8824, 7410, 2211, -5923, 6708, 4662, 7727, 4254, 3248, -3859, -1758, -4948, 3057, 1104, -9824, 8193, -758, -766, 5695, -1764, 447, -9157, 3153, 5379, -175, 260, 8308, 240, 1582, 888, -2276, 303, 7668, -6683, -4436, 6812, 9068, 300, 5458, -279, 7469, -7500, 2465, -2503, -3728, -2335, -9823, -8431, 2796, -8962, 6185, -4149, -1866, -278, 1910, 3631, -2636, -8761, 1598, 5284, 4684, -9314, -8506, -6672, 4172, 5384, -3326, 1178, -1602, 7154, 181, -386, 8963, -9754, -1562, 1729, 6549, -6802, -4157, 109, 3113, 6503, 2018, 6236, 1638, -1654, -7298, -9555, 8633, 1962, 7066, 4563, -6922, 7112, -2481, -9309, -2285, 2607, 9403, 9351, -911, -7217, 1911, -6106, -6439, -2680, -2717, -6870, -6476, -131, -655, 9414, 9075, 7475, 8914, 7566, -904, -1605, 4204, -9153, 1693, -908, 231, 7499, -4796, 5630, 2293, 7239, 1007, 131, 5814, 5560, -9060, -7128, -9420, -8350, -4511, -2345, -2865, 4526, 845, -4549, 603, -6332, -2467, -8170, 227, 7466, -8783, -5380, -6697, -6135, -6620, -6041, 8302, 6815, 2984, -3694, -4439, -7773, 7531, -9657, 8511, 9462, 1146, 7689, -4457, 3719, 7602, 7023, -5073, 3694, -9867, -4708, -60, 6911, -6967, -467, -3472, 9688, 8254, -7344, 5683, -4852, -2997, -9448, -9359, 7929, -3841, 7787, 3652, 533, -3768, -4479, 5915, -5922, 1967, 8393, -1230, -3273, -6009, 1341, -9515, 5509, -4093, 3713, 3380, -3102, -9620, -6118, 1981, -6751, 9256, -3254, -4102, -9590, 3874, -4044, -5434, -5947, 6728, 9124, 5442, 4788, 6290, -5803, -7371, 609, -574, 2053, 1166, 2009, 5926, -7069, -9492, 2406, -6311, 4462, 2948, -7312, -4845, -340, -7466, -5566, -6843, -1498, 7872, 6842, -7071, 1827, -6252, 7304, -486, 6502, 6433, -9071, 9043, -7273, -4031, -7348, 8249, -3667, -9610, -5134, 7825, -4718, 3571, 9578, 4895, 6862, -1302, -6117, 8207, 3867, 1933, 5836, 8541, 8772, -4177, 1953, -6408, 5035, 9816, -346, 2724, -9985, -2595, -5203, 9486, -6215, 8819, -4667, -2650, 2223, 7399, 1952, -1214, -6928, -7223, -5674, -6475, 3873, -4548, 6781, 4721, 1312, 9926, -3820, -7475, -469, 4225, 3416, 3621, -7648, -8026, 8803, -4256, -2204, -3311, 9746, -1489, 309, -4425, -2004, 2301, 8964, 6985, 5551, 1225, -3966, 9476, -6947, 6673, 575, -6429, -8213, 1357, 4202, 2900, 6649, 9103, -2358, 3607, 120, -7421, -1866, 8270, 6920, -6458, 7845, -85, -3968, -5855, 271, 1093, 5437, 1002, 4441, -4998, -4005, 5162, -3518, -6894, -1838, 2914, -6540, -6774, -6227, -7048, -8928, 6238, -5453, 4455, 5988, -2877, -3532, 9422, 2186, -6485, 7915, 8600, -9589, 7234, -2091, -2496, -443, -2029, -75, -9901, 5010, 8734, 4591, 8876, -5314, 7838, 2359, 107, 5888, 9092, 4839, -6676, 3604, 4473, 3336, -3068, 466, -5579, -4925, 9812, -5958, 8012, 785, -8932, 6189, -9154, 4910, 7939, -3918, -1518, -478, 6606, -30, -8534, 649, 4374, -3928, -1762, 5532, -2015, -7358, 749, -1618, -9726, -9026, 3212, -9191, -6519, 3400, -5300, -6515, 7105, 2702, 4534, 3696, 1251, 5142, 7458, 755, 7515, 5876, 8725, 6610, -1721, -6750, -4575, -2472, 1013, -9643, -2615, 8417, 7789, 2264, 3078, -3868, -2378, 254, 2594, -7839, -4901, -279, -5057, -2644, 8040, -6370, 9969, -1947, 3027, -1061, 9145, -1131, 1298, 1451, 7600, -7105, 8929, -9388, 5318, 1908, -9628, -3152, 747, 2370, 1942, -5359, 8146, 6325, -6914, 6824, -6525, 7808, -8411, -3428, -5279, 3804, 7835, 7728, 1995, -6842, 4588, -8526, -2404, 2685, -3246, 706, -9524, -9803, 6609, -8044, 1861, -4598, 4763, -5514, -4042, -8117, 4487, -2150, -5645, -428, 8064, 6884, 8370, 7425, -9935, -8729, -7168, 9963, 5872, -2181, -30, 1211, 3947, 8530, -8997, 7941, 8862, 6884, 2933, 9325, 2390, 971, 6464, 6106, -4188, -6857, 2229, -5227, 4497, -4205, 902, -7656, -9603, -8929, -4612, 6137, 7076, 5718, 5811, -1607, 7693, -3941, -3792, -1102, 2508, -8479, -7276, 1634, -2960, 2453, -3008, -7248, 9258, -7664, -5342, 7224, -8426, -1350, 324, -3723, 5949, 4269, 6136, -9739, 2736, -2667, -6178, -3245, -7930, -1366, 8590, -8389, 2443, -8115, 6715, 628, 5609, 6251, 2729, 8312, -2394, 4899, 7891, 8892, -8314, -2682, 413, 7893, -8223, 3536, -9035, 3969, -7750, -3736, 67, 412, -4551, -9905, -1717, -4149, 7567, -8185, 6924, -5315, 2454, -7916, 5851, -9830, -8533, -7341, 685, 4052, 7164, -9022, 7406, -3696, 1527, -8805, 8544, -3466, 2367, -6868, -266, -5724, 2871, -3099, 7234, -4454, 9191, 1281, 4371, 2965, -3461, -3125, -320, 5435, 9910, 6678, 4466, -7085, -8653, -4119, 407, 9310, -5333, -6641, 1334, -6894, -908, -3920, 7723, -4622, 5167, -3804, -9927, -9661, -1651, 1684, -3698, 7971, -3282, -3781, -4403, 4073, -3630, 8433, -4083, -2216, 483, -345, -8984, 756, 6917, -6076, 492, -6726, -4377, 3151, -4887, 508, 295, -1055, 2439, -1601, -4539, -825, -4532, 4250, -6659, 6275, -7423, 9987, -9154, -7997, 9097, -3266, 3747, -757, -673, 2504, -6305, 8196, -5597, 7817, -2993, 6876, 6729, -2186, -1751, 4309, -5992, -1311, -4087, -4822, 6927, 3495, 4723, -1366, -350, 5136, -3655, -4900, -940, -6350, -5725, -3363, 2197, -3848, 5543, -9107, -9798, 6039, 1185, 892, -9123, -3454, -5496, -2939, -8704, -9403, 8442, -7776, -296, -3552, -691, 104, -7944, -9083, 5378, 2318, 208, -866, -798, 7639, -8384, 7668, -5644, 8016, 4535, 1201, -9049, 2805, -5286, 2333, 3954, 7540, 2966, 6911, 8711, 9221, 7454, -6634, 5974, -6712, -4824, -4915, 8630, 1300, 4988, -8149, 9951, -8598, -2859, -3194, -7567, -9612, -75, -2729, 2090, -3950, 4844, -6448, 9204, 6305, -7945, 6757, -9860, -60, 2380, 3639, -4173, 2372, -9947, -8126, 7695, -68, 5504, -3864, 5901, -1745, 1975, 4243, -383, 8192, -4414, 2143, 4856, 1561, -7742, -8437, -6069, 1467, 6741, -3676, 1940, -3938, 5007, -7114, -9134, 2022, -2177, -2438, 23, -5990, -8311, 9059, -7437, -9851, -782, 4689, 5669, -4836, -6384, 1741, 6756, 219, 2158, -995, 4638, 2769, 7660, -1383, 7884, -6599, 4059, 2980, -4995, -4283, 4320, 5334, -3348, -9309, 166, 5266, 3129, 9938, 8776, -7234, -2097, -9000, -2330, 7484, 4573, -1420, -590, 6554, 9693, -3389, 804, -825, 7371, -8229, -6365, 4252, -5699, -5577, -5148, 4291, 8913, 3427, -8088, -7724, -7518, -2988, 2714, -843, 1929, -4217, 5287, -324, -9009, -2483, -8803, 1700, -8570, 7260, 2812, -5319, -4247, 2898, -727, -5843, 3157, 2755, -7895, -582, -6600, 9567, 4374, 2388, 1898, -4336, -756, 4570, -4670, 4985, 7405, -2479, 140, 4846, 6295, -875, -1480, -6067, -2652, -2167, 7293, -3015, 4061, 5381, -5082, -7302, -463, 9754, 5758, -1946, -9755, 4487, 5154, 3851, 3478, -4771, 2404, 1149, 8279, 5007, 7582, -6509, -8412, -6536, -1173, 4947, 6879, 3827, 2547, -5976, 2045, 8378, -6669, 7514, -2721, -2682, -9284, -7746, 424, 999, -759, 1851, 49, 9439, 9765, 5816, -6889, -987, -9101, 782, -4364, 5751, 3153, -5649, 7378, -9008, 566, 637, -4660, 5185, -1549, 8025, -9642, -6273, -7112, 5639, 9988, 3880, 9065, -8421, 3094, 9040, 4294, 6980, 1333, 2233, 7038, -1357, -987, -8738, -1139, -8288, 2472, -5261, 6735, 2284, -6567, -9069, -218, 9527, -7294, 3138, 1046, 6073, -7874, 8666, 4543, -8780, -4749, 4736, 6633, 1856, -9929, 6706, 7407, 1972, 7329, 702, -6001, -8189, 2467, -3923, 9112, -6372, -1483, -9346, 4280, 5109, 7960, -9311, -5917, 9460, -233, 7898, 7392, 158, -5609, -1953, -5504, -8835, 1714, -9792, -8189, 4553, -3323, 74, 9091, 4259, 8278, -3877, -6767, -4369, 9226, 1204, -178, 6919, 3512, 559, 1489, 3857, -3108, -3445, -2173, -9641, 1622, 1482, -4390, -2869, -3026, 9139, 2987, 5410, -7166, 9464, 9206, -5172, -5123, -3380, -4523, 2363, -2914, -827, 2957, -8183, 3528, 2119, -6634, -7112, -4682, -5594, 6081, -6111, 7074, 8114, 825, -8159, -4159, 63, -1860, 3089, 5164, -4377, -3215, 2137, -5602, -2154, -2034, -9706, -9269, 9440, -6784, -2575, 2645, 5337, 3949, -410, -7806, 7612, 6672, 4228, -7571, 2247, -4327, 6821, -6091, -609, 1212, 9139, 6282, -872, 4509, 4925, 4983, -2655, -9101, 1948, -1206, 2854, -2853, 1773, -8384, -8212, 631, 5340, -6148, 8837, -7117, 5207, 5988, -9230, 4545, -315, -368, -5304, 6423, -7921, 6478, -4844, -8777, 1029, -9458, 8909, -3015, 3961, -5957, 2962, 1259, 1435, -1727, -19, 7568, 7774, -1390, 6371, 9824, -9724, 4189, 4067, -4833, 7879, -6507, 1903, -3889, 4926, -3922, 5523, 4030, -6627, 3718, 9659, 2198, -215, 4228, 315, -9364, 203, 3236, -3610, -178, 9848, 3346, 6554, 3913, 1404, 579, -3584, -1993, -3111, -9838, 5264, 1782, 4595, -8902, 9035, 1161, 2490, 1535, 4567, -9522, -1255, -7582, 708, -925, 4213, 7951, 3949, -3490, -318, 3999, 5892, 311, 3889, 6843, -7915, -6097, 4681, 3845, 2806, -7772, -5058, -8752, 2600, 8205, -5693, 954, 8745, 5875, 4342, -8813, 3987, -6383, 2337, 4412, 5599, 2376, -8137, 8165, 7118, 552, 1871, 3533, -834, -2679, 9075, 4298, 2993, 5640, 4986, -4272, 1268, 6402, -7364, -5449, 8986, -7268, -9567, 4151, 264, -1557, -7635, -8318, -3118, 308, -5966, 4159, 1310, 2633, 9423, 5081, -785, -2778, -2601, -6550, -9065, 4961, -5956, 8595, -9822, -6943, -8061, -4128, -5558, -3499, -5832, -7889, 2134, -6410, -7343, -4016, 3043, 2716, 2082, -1552, 6717, 2746, -2428, -9887, -3817, 9361, -9662, -9700, 5102, -2433, -7714, -8179, 6061, 8249, 1115, 6419, -5710, -3948, 4249, -8214, 8551, 3264, 9150, -1942, -4526, -584, 3557, -5497, 5755, 4799, -965, -3151, -4846, -7948, -480, -2604, -7994, 6061, 3442, -2043, 2354, -8297, 3133, 2690, 1673, -8713, -384, 2879, 6100, -9827, 2110, -2851, 5448, -1630, 8681, 794, -350, 7132, -3907, 2222, -4785, -8062, -726, 5273, 5723, -9031, 7904, 5996, -7316, -6493, 482, 5800, -2346, 8095, 5571, -3434, 6489, 2962, 3539, -4813, 1038, 4019, 3510, -5021, 9386, 9327, 7463, 6979, -2658, -5459, -4923, -6763, 6117, 6191, 5906, -7165, -1846, 4632, -3154, 6871, -1322, -2091, -6729, -336, -8306, 2089, -1518, -6539, 9058, -8399, -8990, -936, 8870, -9144, -6593, -6393, 2100, 360, 5307, 8441, 7582, -7832, 2513, -6046, -1818, -924, -4528, 6708, -6824, 8669, -9300, -5181, -5156, 5645, 3423, -8064, 7907, -9851, 3031, 3534, -1117, -1355, -4268, -4569, -3546, 3620, 3267, 5544, 3491, 1759, 7681, -5585, 7802, -5724, -1327, 4642, -1463, 5933, -4173, 1143, 1201, 3570, -8013, 3733, -6099, 3142, 7075, 5533, -4584, -2147, -5203, -3589, -8009, -5681, -6787, -7520, 1872, -2702, -3048, 4429, 6776, -7007, -6205, 3721, -3397, -7742, 5700, -580, -805, 8351, 8374, 7269, 7057, -1113, 3345, 8499, -450, -1648, -6584, -4239, 8540, -452, 2296, 1539, -3120, -3575, -3129, -2345, -9938, 2817, -3183, -112, 9474, 9298, 8326, 7334, 9046, 4246, -9528, -4041, -8879, -6967, -4766, 4622, 8384, -8180, 4401, -103, -1635, -8093, 1961, 130, -849, -8358, -9006, 3132, 335, 5189, 1126, 2037, 2116, 8236, -489, 8301, -7374, -565, 1449, 927, -9406, -3021, 5878, -6178, -667, -2422, -3045, 5504, 1724, 2814, -8043, 3791, -7957, 9779, -2043, 3345, 8838, -3611, 7415, -4362, 256, 5583, 559, -6154, -7517, 2050, 77, 3965, 8872, 3768, -3651, -2353, -2829, -2727, 7359, 8477, 4164, 5995, 4879, -1102, 7423, -6250, -9203, -3427, -2637, 8210, -735, -68, 8351, -500, 5445, 3306, -5998, -6453, -9530, -2836, 1086, -2975, 3580, -7616, 7383, 9459, -5396, -3553, 4506, -1529, 3213, 1134, -900, -9786, -4508, -5466, -1070, -8286, 612, -5556, 5691, -3088, -6794, 7846, -8587, 91, -6289, -1895, -2781, -1165, -560, 2973, -1147, -9344, 197, 7765, -5006, -1716, -1737, 9144, -645, 3930, -6635, -3568, -6701, -7235, 5511, -1528, -7022, -7814, -7983, 822, 3167, 3836, -5786, -1325, 1187, 12, 1027, -793, 2758, -289, -768, -2232, 3669, 4585, -9967, 6640, 6792, 3762, 2404, 1920, 1034, -400, 6818, -7140, -2738, -9461, -3557, 9387, -7949, 5978, 8054, 9260, 1471, -8949, -7366, -5937, -7864, 6970, 3663, 646, -8131, 3229, -6243, -5959, 3901, 9173, -6192, 7504, 8826, 4080, -4113, -376, -8389, -9235, -4439, 494, 3333, 8829, 9786, 4700, -5468, 7506, -8998, -1564, 5099, 1189, 8585, 8490, 7545, 9914, -7602, 7003, -5647, -5234, 4521, 3845, -5507, 3862, 1700, -6217, 2963, 8933, -9290, -5231, 3316, -3349, -8634, -8083, 4613, -4122, -2112, -2800, 8785, 4727, -9309, -6099, 9318, 7129, -2672, 5122, 7121, 3862, -4397, 4671, -5118, -4073, -3409, -45, 544, 1267, 7336, 8042, 1619, -8957, 9520, 3646, -8837, 9691, 6959, -1656, -7428, -381, -5104, 4521, 6020, -7126, -9699, 5994, 8969, -6306, -234, 2018, 8590, -9607, 8007, -6171, 7465, 9358, 8055, 524, -5957, -3296, 2611, 2797, 5754, 1544, -6519, 7740, 1471, 352, 9692, -10, -7998, 3, 7099, 9820, 79, -7081, 7395, 6062, 4718, -556, -2103, 5039, -3939, 2858, -9312, -8881, -47, -9801, -4448, 3425, 6587, 1305, 412, -9902, 969, -2503, -2956, -8633, -7259, -511, 173, 5591, 2937, 5277, 4852, 1476, -5626, 9809, -4591, 6679, -9703, 8882, 730, 2980, 4993, 4183, -8067, -5948, -8755, 8640, 9386, 175, 2454, -7002, -9946, 2887, 994, -1100, -8245, -2034, 9325, -318, -1067, -5664, 5636, 5925, 3871, -4050, 4624, 133, -486, 158, -9943, -5504, 7595, -8506, -3501, 5845, 9841, -4209, 9338, -9917, 9954, 3582, 6954, 9506, 5673, -3432, 2727, -4753, -1499, -1342, 8544, -9186, 3315, -2999, 3002, 8258, 4409, -2937, -623, 8409, 6532, -4493, 6805, -1021, 3139, -2432, -3610, -239, 1136, -2183, -5735, 3774, 5892, 5440, -5491, 2205, 8826, -6974, 1867, -3345, 967, -8288, 356, 8947, -6078, -8943, 6019, -7760, 215, -1184, -5443, -8927, 3670, 4420, -846, 3046, -4371, -9667, -35, -4575, 7121, -3048, -4932, 9552, -5045, -7112, -7718, 5934, 7121, -3712, 2561, 5035, 4827, 2917, -140, 470, -2785, 9441, 9107, 5223, 8185, 5767, -3213, 8791, -3927, 751, -3758, -8796, -4099, -3250, -2418, -2199, 5657, 9955, 8441, -8181, 7053, -2363, 2717, 9509, -1170, -7073, -1671, 750, -375, -1874, 448, 2107, -2151, -8576, -8405, 8996, -3788, -9705, 1004, 8761, -5267, -5104, 2358, 7745, -8810, -6429, -5040, 3659, -2022, 7795, 3447, -3712, -2499, 3862, -3970, -3247, 2810, -6609, -2610, -9776, 4543, -5963, 7288, -5980, 6407, -2851, -3144, 5673, -5883, -9219, -4783, 7237, -8527, -8029, 7871, 8360, -6668, -3401, -4749, -9292, 4640, 2373, -8730, -6095, 2684, -7661, -6910, 3838, 3643, -442, 6709, 2515, 6462, 6915, -8793, -274, -729, -4743, -8020, 6382, 2561, 6422, 5286, 8135, -6675, -9306, -8994, 2961, -2623, 9086, -9212, -113, -2350, -5072, -6169, 7944, -9339, -3134, 9835, -3685, -570, 4464, -4120, 8212, 8891, -7076, -3178, 4411, 2866, -1468, 1781, 1981, 3893, 1104, 4951, 6740, -4528, 9863, 8268, 4957, -7104, -9552, -4256, 1063, -5529, 8192, 5921, 5564, -3576, -6193, -7933, 5649, 9316, 9886, 5049, -1209, -3504, 9599, 8279, -1464, 5738, 9966, -6816, -2114, 7115, 5015, -2929, 8547, -792, -1568, -7201, 7304, -7782, -6507, -868, 2113, -4547, -6735, 5646, -8108, 2204, 3378, -6509, 2295, 6263, -4656, -8566, 3150, 3747, -1116, -1505, -6598, 810, -1174, -1441, -8946, -9091, -8136, 1493, 4955, -2240, 1547, -1653, -3656, 5172, -2889, 2338, 7321, -2855, -4499, -9298, 9010, 9688, -8530, 3822, -3467, -3669, -4801, 8605, 2063, -3005, 7507, 8774, -9894, 8496, -3127, 4654, -4557, -8896, 231, -2634, -5208, -8044, 4975, -360, -142, 1896, -2386, -1733, 9062, -1783, 9017, 8829, 910, 8217, -7731, 97, -2500, -1470, -9470, 8186, -7794, -6543, 5266, -8810, -3308, 5489, -9270, -1219, -466, -7546, -9642, 15, 9861, -4447, 6762, -875, 3793, -4317, 4888, 9253, 3918, 2541, -8908, 9484, 1311, -4837, -4300, -1296, -1586, -2363, 3643, 5923, -8129, -6274, 8718, -7684, -2306, 7710, -4450, 6301, -595, -3659, -3825, 6953, -1542, 2616, -372, 2813, -4838, 9380, 8891, -7966, -666, 115, 8609, 3105, -1540, 9627, 4620, -6250, 7304, 9799, -5232, 8951, -6506, 1018, -6678, -9704, -9887, -218, 8074, 2358, 3285, 7129, -787, -5263, -6480, -2582, 4705, 5166, -5371, -8489, -2248, -8401, 5883, -4622, 7328, 9594, 5301, -5942, 741, -9784, 3997, 3880, 1514, 6429, 4300, 7364, 4092, 3405, -4435, 3233, 2357, 1472, 8089, -4330, 367, 7396, -3356, -303, -163, -6807, 2635, 1611, -639, -9003, -7509, 995, 3013, 1872, 506, -4358, -9662, 977, 7966, 5697, 587, 1558, -1226, 8689, 1965, -8207, -9788, 7429, -3667, -7119, -7787, 7308, -1202, -9550, -240, 5818, -2150, -6757, -5413, 7377, 8200, -5353, 7504, 304, -8242, 7389, 7140, 3606, -9355, 807, -7815, 3556, -9807, -1451, 7, -684, 4401, -5601, -4068, -2778, -5926, -3869, 6050, 5935, -979, -340, -890, -4704, -6992, -3350, 4206, -4972, 98, 6930, -7154, 269, -3097, 112, -4484, -7345, 7158, -3827, 7773, 7905, -8570, -1886, -3187, 5881, -5094, 3325, 1792, -3919, 650, 5926, -1277, 9172, 8402, 6637, 1699, 1373, 3462, 3167, 4932, 5988, -6615, 7307, 8578, 3849, -7081, 1123, 1204, -5755, 9165, 2947, 1270, -167, -6116, 2567, -7324, 4104, -6628, 7566, -7898, 1857, 7145, -9809, 2064, -5313, 8316, -5658, -223, -9805, 5195, -576, 1577, 6182, -6623, -2854, -1480, -5346, 1815, -6049, 4710, 6090, 317, 5052, -7786, 6647, 4372, -5703, -134, -8045, 8106, 740, -6909, -5076, -870, -1704, 817, -7637, 4505, 9931, 7908, -4701, 2847, -2267, 2468, 7665, 8984, -4839, 8568, -3260, -5009, 7118, -3904, -3314, 7142, 5736, -8164, -9168, -6026, -7520, -2893, -562, -507, 5170, -3966, -2078, -771, -7972, -1317, -9357, -1186, 81, 1811, 9629, 695, 3765, -9532, -4788, -7031, 5898, 6293, 7286, 1584, 7800, 1811, -3320, 9193, 8778, -1587, -9672, -2439, 4726, -9513, 9815, -7953, -6724, 3445, -7139, -2070, 9436, -6378, 8778, -5081, -1108, -5685, 6438, 4280, 381, -3524, 8857, 6545, -5802, -8567, -9950, 2034, -9592, 235, -9748, 6810, -1422, 7382, -5272, -7912, -4636, 8456, 2201, 9857, 2849, 9182, 5071, 5841, 490, 8483, -763, 3565, 4033, -8534, 6844, -6319, -138, 9043, -8455, 8354, 4125, -873, -3618, 2359, -1478, 5011, 274, -6416, 7031, 8128, -8919, -4921, -1883, -3727, -3116, -5550, -3739, -6101, 9264, -7464, 590, -2219, -4205, 9765, -3836, 6470, -8052, -8408, 1530, -4976, 4961, -3480, 5051, 8705, -9242, 5510, -9712, -4167, -4726, 8118, 9850, -885, 5174, 668, -5572, 3890, -8258, -4516, 328, 1035, -7882, 2771, -9863, -5237, 5511, -4929, -1801, 9678, 5706, 8713, -7196, -3626, 747, 1051, -8494, 3978, -1572, 164, 9884, -8869, -912, -7839, -4422, -8529, -9640, 2498, -9745, 7002, -9281, 7772, -1905, 4382, -107, 7644, 5660, 9623, 8093, 4952, -2999, -9692, -8247, -174, -1990, 7262, 6727, -4222, -1492, 2467, -5659, 3156, -7361, -8530, 1716, 3351, -7974, -2104, -8159, -3486, -4521, -7712, 1021, -1204, -4645, -3525, -2674, -4282, -4509, 3373, 5606, 6837, -5196, -9548, 5075, 2055, -2627, 793, 2033, 5256, 3200, -2386, 5228, -4024, 4518, 2952, -2370, -9744, 4346, -6392, -3946, -5136, 5295, -3730, -5158, 6958, 1213, 4489, 6041, 1590, 3181, 1128, -2599, 638, -4546, 5504, 6754, -2728, -705, -4016, 7423, 6430, 9186, -3127, 3703, 8748, -826, -5151, 589, -8903, 3605, -1678, 9831, 9627, -1104, -3941, 2326, 5787, -7313, -5010, -7770, -5470, 124, -4050, 8587, 8090, 3525, 9035, 2969, 8659, -3180, -4937, -5714, 915, 105, -9865, 1263, -5315, -9951, 5522, -6271, -1389, 7430, 6620, -6826, 571, -6013, -3755, 7891, -5836, 5127, 7854, -3549, -8624, 6813, -9975, 3500, 8794, -4071, 4564, 820, 5018, 6697, -5671, -2986, 1476, 4548, 2835, -996, 9209, -3189, 8020, 5667, -2253, -7128, -7481, 5810, 3127, -8100, -3705, 8549, -1930, 399, -2051, -9484, -3332, -6889, -9031, -9097, -662, -2183, 5598, -3113, 5189, 80, -4068, 3384, -3988, -4, 3043, 3626, 2666, 3015, 7029, 6765, -7109, -559, -6811, 9434, -5206, -5406, 9561, 1288, -5028, -861, -2555, -5590, -4515, -3168, -3021, -7294, 11, 8393, 5572, -5524, 2422, -1429, -9014, 332, -6744, 7462, 9977, 5870, 4302, -3986, -256, 9393, 8203, 2429, 1887, -1710, -9976, 6437, -1667, 6000, 1216, -9085, -9885, 8550, 150, -2602, -4438, 5594, 2223, 9939, 1897, 5884, 3654, -5656, 6201, 4002, -1310, 2705, -2683, 7323, 1409, 1600, 7123, -322, 2259, -902, -7991, 1569, -5726, -6776, -9125, 3201, 6241, 3951, -5887, 4453, -2548, 3762, 4454, -4301, -1180, 4935, 1241, -2686, -278, -7209, 7514, 2916, -4030, -1736, 7286, 6641, -6468, 5374, -2105, -5341, -3423, 9311, 4203, -6157, -3541, -9891, -395, 6585, 7053, -5763, -3753, 4005, -8452, 1254, -1514, -6170, -2274, 2070, 9553, -9711, 3447, -3216, 3409, -6773, -1227, 2133, 2160, 6220, -3580, -8480, -1975, 2320, 983, 3669, -6534, -3185, -583, 6782, -2516, -843, 6994, -9724, 4758, 1175, 4546, 6179, -4214, -72, -3099, -1566, -7560, 5016, 3830, -5568, -8962, -6018, 2575, -5580, -4913, -9997, -3157, -683, -3228, 8655, -7226, -426, 3679, -9055, 4770, 5067, -3365, 1612, -1469, -7565, -6846, -7187, 1607, -7582, 704, 2323, 2991, 6598, -703, 4832, -4897, 9482, -2808, 7276, -4869, -2735, -3585, 5063, -6484, 5608, -7025, 6447, 6218, -1171, 6559, -9657, -9008, 1066, -4598, 1496, 9342, 4832, -2225, -3196, -8287, -2321, -9952, -7911, 5996, 6485, -3925, -5227, -8070, -6852, 9947, -3684, -6158, -4935, 9579, 8866, 9334, -6357, -8559, -7491, 2654, 6080, -6271, -7933, 1016, -2758, -5144, -2941, 6267, -6628, 8753, -6963, 1058, -8398, 3000, 8031, -4205, 6494, -9743, 6105, 1473, 7844, -3451, 5088, -9151, -9942, 1422, 892, 6527, 1176, 7146, -8083, -5229, 800, -6294, 4602, 2770, 72, 3727, 5919, 6892, 3990, -7690, 4568, 5530, 3528, -7604, 2289, -110, 3039, 1941, 99, -6347, 9786, -5116, 4179, -5080, 2470, -9340, 4545, -7142, 5268, 8552, 6342, 8843, 2384, -9679, 2864, -3350, 5048, 9600, -5520, -5138, 4215, -4188, 732, -3047, -7193, 3872, -397, 3786, 3682, 1727, 5348, 8605, 6982, 2843, -6270, -3425, -3702, -9996, 7379, -3011, -4127, 5339, 4756, -6902, 8229, -4153, 3572, -8600, -518, 3709, -5826, 4887, -8997, -5058, -3566, 2455, 3051, -3395, -795, 6907, 6449, 1081, 8142, -1439, 3637, -8787, 7637, 465, 6802, 6913, 360, 5378, 8134, 6979, -929, -1842, 9537, 9455, 8697, 3897, 5346, 6000, -7980, -753, 5337, -6193, 7879, 2353, -7226, -2156, 5982, -8205, 7730, 2049, -5877, 2502, 4194, 8928, -4007, 2636, 4072, 6688, 4047, -5920, -6848, -9825, -8217, 2129, 685, 6301, -7842, 1424, 6367, -8647, 3285, 5525, 5673, -9662, -6615, -676, 2037, -5530, 9811, 5971, -3410, 8395, -7898, 2841, 7839, -3793, -3099, -3941, 1402, -512, -1610, -599, -7091, -5609, -4540, 8935, 5771, -6509, 6238, -2610, 1581, -5115, 8683, 5060, 7474, -4239, 2255, 999, 8130, 5357, 2198, -3, -9639, 9998, 3703, -8426, 9741, -8402, -6535, 3896, -4921, -4330, -9139, 6485, -2301, 8790, -5897, -4938, -4580, -4519, 5173, 6186, 7700, 4909, -2855, -5030, 8406, 7512, -2361, 7915, -3892, 6697, 8954, 3297, -3692, -7736, -7824, 6467, -658, 1721, -5767, -2659, -6448, 6551, 4483, -9150, -3650, 9528, -9996, -5014, -5820, 8506, 4991, 8518, -7520, -4165, -8297, 5645, -8558, -1569, -113, -8124, -4421, 892, -5891, -5411, -199, -166, -5345, 8607, -1291, 7603, 4633, -5240, 9042, 8581, -1153, -1796, 3134, -7585, 8788, 5436, 7376, -4124, -259, 2814, -5036, -9363, 3267, 4576, 19, 9737, -6872, -6740, 2628, -2493, -8721, 1173, 8339, -995, -6857, 1534, 6641, -6561, -7272, -7532, -5951, 1186, 1053, 8422, 256, -1030, -9167, 6312, -6899, -8473, 3702, -437, -2241, -6066, -4835, 2880, 6203, -9399, -1243, 9167, 2086, 1464, -4422, 3095, -4195, -7697, -7760, -6270, 2867, -8881, -8677, -8425, -1899, -4287, -1452, 5748, -6131, 3870, 4029, -5774, 1181, -1129, -7249, -7220, 8479, -8948, 748, -9662, -2770, -5496, -6651, 3708, 3454, -6993, 7431, 1875, 8543, -6719, -8824, 3551, -8800, -6352, 9395, 3862, 9007, 6979, 904, -4000, -7122, -4728, -7260, 9861, 2951, 3843, 3270, 9887, -344, 6406, -4903, -7190, -9225, -8553, -525, -6257, -8500, 6906, 5068, 4223, -3727, 5481, 9953, -1782, -8615, 8985, 6294, 6320, -7242, 7762, -4352, 2210, 13, 1131, 4297, -3591, -9242, -7341, -8618, -5245, -1546, -8597, 6950, 7155, 7114, -5541, 5789, 2247, 8607, -3359, 6851, 2126, -4702, 2041, -8046, 6933, -4118, -526, 1481, 9731, -4371, -4820, -8561, -3223, 3611, -9296, -8738, 1800, -2711, -9953, 3225, -2638, -4960, 4044, -7682, 424, 1139, -4257, 493, -8479, 5938, 743, -5414, -2213, 5295, 2322, -8395, 9735, -7989, 6062, 4874, 8061, -7667, 3006, -3498, 4795, 3687, 8615, -5320, 6450, 2440, 6809, -7888, -9189, 2725, -6692, -469, 4162, -3801, 9165, -3922, 3199, -4276, -973, -7714, -3257, 9106, 9596, 8676, -6898, -6091, 7449, -2068, -7007, 9723, -2989, 883, -9712, -5246, -9209, -3219, -2884, -7503, 7904, -972, -8911, -4012, 7085, -7005, -4102, -9349, -2596, -2586, -7770, 7877, 4206, -8479, -2905, 4030, 3154, -2021, -7257, 6093, 5665, 1943, -545, -4081, -7968, 420, -788, -5511, 3270, -4898, 5880, -4476, 2621, -5273, -942, -8098, 9301, 3324, 3133, -2015, 3319, 2585, -7344, -796, 1924, -8574, 7588, 9408, -147, 6197, -5098, -8249, 2178, 594, 9521, 5627, -837, 5192, 2726, -2348, -1537, 4083, 9842, 2999, -8970, -7863, -5199, 7740, -8163, -5946, 9540, -4480, -7298, 1603, 7096, 1756, -5851, -9983, -8558, 5368, -4420, -4013, 394, 2608, 3678, 3266, -939, 9648, -8794, -2753, -3360, -2833, -1002, -8233, -2442, -8287, 3735, -9570, 3016, 9656, -9720, 7012, 5830, 6237, 10, -3881, -5789, 5372, -7168, 6499, 6495, 6999, 1661, 1540, -3994, 690, 3456, -2197, -5884, -8025, 4910, 2543, 1474, -680, -9984, -8217, 5341, 7737, 6896, 1643, -3813, 3807, 978, 1723, 8883, -1285, -5869, 4244, 2701, -9991, -6658, -6163, -838, 4325, 8967, 7813, -7260, -6705, 3271, 4868, 2622, 2948, 7982, -2022, 3379, -1317, -3600, -5612, -8088, 7497, 1249, -1841, -3189, -5594, -8775, -6447, 9867, 4520, 9527, 468, 1369, 5983, -635, 2048, -1561, -3196, 7614, 554, -8128, -5053, 1235, 2383, 7178, -4912, -571, -7717, 3795, 2343, -4446, -7484, -9450, -4474, -6347, 2965, -3986, -2696, -7604, -9828, -2783, 854, -8244, -2849, -2575, 1693, -5288, 9887, 9881, 4783, -3982, 9977, 617, -8877, 6506, -4850, 1436, 3406, -7257, -8839, -9808, -9374, -8170, -7531, -7146, -5570, -9457, 1956, -157, 5705, 2720, 7983, 6750, 8408, -3423, -2095, 4396, 4654, 3759, 4397, -8221, 8271, 4796, 6435, -6155, 4176, -2715, 184, 6885, 5775, 5292, -9980, 4605, -4903, -6294, -2746, -3899, -1559, 1245, -7788, 9078, -8969, -6658, 7604, -7081, 8615, -9882, -587, -5873, -4623, -7195, -2018, -7713, -8424, -8812, -6246, 8154, -163, -2707, 8069, -3209, -4468, -7453, 7113, -9566, -5734, -412, -3490, 6010, -3845, 7315, -1150, -2463, 279, -1669, -1301, -1664, -1470, 2817, -1676, -8371, 9024, 6387, -9898, 9466, 2528, 2561, -3822, -7878, 7594, 5485, -3657, 9852, 3145, 6158, 4352, 2618, 9075, -1199, 2603, 5462, -8148, -9048, 3186, 8545, -2846, 8862, -9028, -2985, -2050, 4786, -5356, -3735, 4576, 2480, 9254, -207, -4996, -9737, -4443, 8990, -7021, -8304, 4738, -392, 5749, 2423, -6225, -8118, -5692, -8610, 7717, 340, -758, 2639, 742, 7878, -3005, 1453, -8855, 8890, -3946, 5365, -3791, -6721, 3255, 6365, 2232, 3616, 2525, 8806, -5418, -3153, -8214, -6540, 5256, -2229, 9371, -3, 8382, 160, 9745, -6919, -5386, 7652, 9593, -9527, 1285, -760, -5294, -4940, -8734, -6670, -7137, 881, -8825, -1743, 9139, -7735, 3330, -7528, -5260, 2059, 5708, -9165, -4747, -4686, 7631, 1892, 415, -6028, -1274, -9893, -8409, 4352, -5542, 6159, -2289, 91, -2806, 2783, -5413, -4321, -7869, -7182, 3132, -1347, 1417, 4570, 1667, 1937, -1210, -2915, -7493, -1528, 7261, 9671, 7656, -4892, 6301, -9206, -8524, 4488, -5810, 3435, 391, -1438, 1474, 452, -6646, 6205, 7658, 8837, -7325, -7089, -7601, -6242, 3981, 3832, 1201, -9820, -5207, 6657, 9166, 5800, 8006, -2289, -6024, -3871, -9322, 9166, 2823, 1245, -6215, -6140, 3111, 8400, 4805, -9860, 2865, 9109, 8412, -3102, -4733, -1585, -9197, -6642, -2238, -2638, -5193, 6142, -6304, 7259, 9327, -446, -4559, -1494, 8522, -2644, 5790, -8063, -4017, -6006, 9974, -3126, 2453, 9138, -9579, 8665, 9682, 7431, -6175, -939, 9374, 6165, -6241, 2681, -6190, -6419, 384, -6380, -713, -9852, -3385, -7368, 6505, -6239, -7403, 8701, -8483, -8182, -445, 676, 5359, -9865, 2431, 8195, -1337, -3055, -6484, -7965, 265, -1609, -2772, -2567, -3969, -5491, -4502, 6772, -1283, -2513, 111, 9713, -5853, -9388, -4668, -9940, 4061, 2541, -268, 9108, -2547, 2868, -4396, -2810, 3887, 391, -810, 9284, -521, -1206, 4859, 1196, 399, 2666, 7584, 8252, 4191, -2345, 5188, -2787, 329, -5766, -521, 8696, 5083, 439, -7674, -6754, 3371, -9308, 7193, 9820, 4233, -986, 7400, 7056, -1324, 3482, 9827, 166, -750, -1042, 729, 6252, 6161, 6526, 9393, 7641, 2985, -2113, -4905, 8008, 3489, 3878, 4468, -6379, 9951, 9492, 4243, 8887, 9625, -1381, 5423, -574, -876, 1332, -3986, 3502, -969, 6821, 2545, -6491, -2068, -1945, 8906, 5336, -7804, -1590, -1815, -5017, 1422, -829, -4614, -8343, -2090, -7440, 3216, -437, 4652, -4432, 6153, -4510, 8509, -855, 5232, 4951, 5332, -1135, -2813, 3, 8137, 6800, -5114, -6508, 3761, 6938, 7699, -1858, 9002, 6143, 9067, -549, 2920, -966, -4188, -2059, -7008, -3865, 3495, 9917, -4423, -8115, -6984, -3470, -8496, -2520, 3779, 3054, -2624, -4470, 1206, -7534, -1446, 7311, -7551, 8335, 376, 7183, -3077, -3502, -7843, -9242, -6739, 4981, 9114, -8663, 9388, 460, 91, 3078, 2928, -6258, -2640, 5460, -4279, -5828, 8117, 8113, -2169, 1885, -9042, -1978, -9931, -5409, -179, 82, -6233, 1395, -8917, -7998, 2209, 4752, -847, -2688, 6568, 3999, -6652, -215, -6616, 1048, 8336, -3233, -2892, -2348, 7783, -6196, -5003, 8098, -7783, 2765, 1279, 8864, 8252, 4661, 7228, -6860, 5606, 5038, 2917, 1848, 4095, -4251, -1386, 4172, 7718, -1382, 5182, -522, -6846, 3620, 7868, 4930, 423, 345, -7675, 546, 3858, -561, 819, 7583, 1019, 2732, 3640, -27, 4237, 7390, 6705, 9181, -8679, 3107, 1266, -2835, 1420, -7398, 9138, 6349, 7193, 6250, -5987, 5305, 5081, -4009, 1437, -7254, -5534, 5861, -5446, 9789, -2998, -3706, -6497, 4402, 4818, 2870, 1778, 9519, 4420, 7466, 3598, -9675, -5101, 7671, 5392, 7387, -2958, 1935, 8158, -3715, -2232, -6492, 3858, 4611, 6569, 6163, 1971, 4466, 7195, -2962, 6286, -6609, 4507, 2345, -1953, -1842, -2173, -585, -5831, 5121, -5395, 8043, 1893, -9535, -9016, 7937, 7627, -9938, 2733, -282, 7765, 2676, -4860, -1557, 1267, 984, -1707, -3170, -5642, 8781, -8778, -8978, -2349, -6117, 9684, 5673, -6904, 1092, 7430, 50, -2945, -3353, -3038, 8335, -7829, 9498, -8847, 8963, -4938, 9544, -2702, 4031, 7272, 24, 437, 654, -4668, -9742, -1787, -3650, -6754, 317, -10, -9952, 4161, 1063, 9902, -9879, 2743, -1683, 9466, 4921, 3961, -2911, 1425, 6230, 8684, -4130, 701, 5278, -2887, -6092, -8856, 4944, 8970, 7509, 4317, -8360, 9903, -6999, 2513, 4795, 9006, -2842, -3416, 8620, -9244, 3965, 3795, -317, -949, -7201, 681, 3616, -8693, 1550, -273, -9565, 1767, 3964, 4230, 8337, -2017, 8706, -8402, -632, 952, -5754, 9817, -4005, 1505, -6642, -5977, -3624, 7400, -108, -4832, 7643, 6272, 3575, -2977, 3220, -175, -3708, 6536, 3450, -1466, -6093, -2884, 5456, 3262, 7970, -368, -4753, 2329, 776, -7556, 4873, -8639, 7846, 9937, -6539, 6480, 6093, -7311, 2150, 2404, -9515, -6665, -3469, 7038, -163, -6399, 7825, -9297, 9738, -3, 6207, 3171, 5179, -7769, -6725, 8586, 2043, -2194, -1828, 9941, -5873, 3338, -1997, -8852, -8028, 9414, -6660, -9430, 217, 2668, 3457, 3716, 8677, 4554, -8825, -7349, 5016, 1075, -4832, 3503, -1707, 7853, 8742, 4486, -2440, 199, -3830, -6998, 8912, 1539, -8917, 794, -3446, 9372, -33, 2809, 4926, -7135, -352, -497, -3003, -9051, -2374, 8377, -4506, 3018, -1902, -2353, -8925, -3530, -71, -5500, 6834, 1701, 2649, -4918, 8608, 5014, -7981, -65, -2955, -656, -5546, -5105, 2064, 484, 6566, -2844, -7664, 5150, 704, 7654, 30, -207, -1253, -5132, -8886, 4455, -9918, 7063, -7621, 4389, 9151, 1133, -143, -7404, 8218, -7482, -6383, -5053, 9293, 3114, -951, -6457, 8725, -4177, 1486, 5832, 7140, 8104, 2888, -7468, -631, -193, -1603, 3926, -4376, -7431, -7694, -347, -6556, -1028, 5391, -662, -6407, -7375, 8258, -9829, 7184, 2506, -9738, -1624, -5257, 1318, -1636, -3217, -4063, 8353, 2044, -2830, 2781, -9123, -101, -7862, -6306, -6925, 2297, 1666, 2995, 3990, -7918, 9967, -7241, 319, -2917, 4825, -3194, 223, -4199, 7185, 1178, 9906, -9462, 462, 7109, 7494, -4734, -8585, 6645, -6466, -1521, -3806, 426, 9056, -2795, 2095, -7723, 6943, 4973, -8260, -1675, -5366, -101, 429, 5096, 8623, -8959, 6497, -4868, -9249, 4230, 898, -8361, -4344, 2917, 201, -433, -9763, 398, 4678, 3543, 878, -1698, -9573, 4125, -2479, -7510, 2802, 4605, -5448, -7933, -9151, -5846, -410, 7922, 9836, 504, 9664, 417, 869, -6468, 6286, 1232, 5290, 8107, 1970, 5661, -4289, -77, 3178, -1254, 1245, 9964, -4091, 7148, 2294, 3193, 7632, 9944, -7198, 5958, 7080, 1648, 8195, -7045, 123, 2532, 8307, -1599, -4011, 4341, -1483, 8570, -5968, 5658, 6279, -2688, 5645, 6009, -298, -422, 7205, -8571, 7861, 1320, -493, -1808, -5288, -9644, -1988, -988, -6756, -886, -4441, -3396, 4622, 532, -5139, -3984, 9090, 991, 3795, -5320, 5095, 5420, -4263, 4485, -5372, 4112, -428, 4762, -1020, 3935, -1368, 4201, 1206, 0, 3280, 9660, 8601, 4159, 2472, 4533, -8465, 7519, 9361, -5893, 7724, -193, 2145, -3789, -4449, -7308, 8416, -3634, -1102, 4915, 6493, 1797, -958, 7323, -3025, -2775, -4806, -3570, -3488, 7327, 7916, -3394, 4395, -8608, -2065, 2216, -5652, 6448, 1855, -7956, -2085, -6786, 7846, 2136, 5737, 8188, 5789, -60, -7665, -7796, -5126, -7627, -2455, -7903, -244, 1522, -3860, -509, 2342, 3142, -3108, 5587, -5827, -7495, -6791, 2797, -3147, 551, 9074, 6600, -2064, 6250, -3325, 3169, 5342, 9987, 846, 9566, 8785, 6318, 64, -2675, 2799, 4286, 5333, -8879, -3405, 5083, 8959, 135, -342, -5789, 5115, -2299, -7472, -9802, 6803, -6350, -1432, 2171, 5640, -4200, -711, -5211, 9859, -4125, -8204, -3059, -4449, -4333, -1310, -3275, -7928, 6859, 1780, 9957, 344, 139, 2541, 5994, -8130, 6346, 3438, -2526, -8110, 9365, 8158, -6801, 1008, -3927, 8085, 9834, 6778, -7239, -7273, 1641, -9088, -1495, 4886, -7648, -3811, -9028, -6042, 1424, 9567, -9063, -9908, -6546, -2497, -2931, 3381, -1333, -9145, -5820, -2410, 7339, -3023, 6366, 2207, 485, 3020, -8321, 1377, 8433, 9913, 5824, -4755, 1109, 8559, 9875, 9465, 7671, 2391, 2902, -4338, -7443, 5545, 970, -5371, -1821, 4838, -8855, -4814, 9019, 9779, 8849, -7110, 6438, -3505, -3618, 7442, 8979, 170, 1908, 936, -1069, -2341, 9621, -2504, -8214, 3489, -2786, -9750, 2530, 9782, -8088, -642, -776, 7416, -8479, -6051, 4248, -8950, 1023, 9756, -6897, 4974, 7283, -5036, -5216, -5814, 7611, -932, -5167, -106, -9689, 9101, -1858, 5963, -6029, -3928, 1948, -8241, 937, 7597, -9251, 5384, 5275, 9185, -2854, 2407, 903, 3772, 8310, 4179, -6100, 2186, -9940, -1191, -3556, -6102, 6358, 2566, 1108, -6596, -2051, 4280, 1982, -6817, 6149, -5360, 3214, 3156, -7817, 2869, 605, 4166, 5051, -8460, -3365, 2796, 8404, 1274, -5342, -3454, -9085, 444, 4503, 7321, 3142, 3294, 8003, -4044, 5711, -9633, 7260, -9757, 7521, 2712, 5226, -7217, 8539, 5302, 3140, 6022, -5043, -7989, 9779, 6277, -8370, 5586, 1868, 776, -1678, -9448, 5103, 4047, 7468, -9788, 3684, -6777, -7956, 9116, -2078, 2217, -2416, -8624, -1728, 2916, -4618, 5637, -3879, 6613, -6719, 6897, -7878, 4023, -1007, 8519, -5427, -7141, 8186, 1247, -3277, 5060, 1948, -3272, 4855, 4351, 9882, -6745, 1300, 1257, 9476, -459, 8941, 4192, -2501, -8257, -1805, -3801, 743, -4411, -7591, -2838, -6018, 4862, 8624, -3244, -4704, 438, 9571, 697, 8043, -5613, -3068, -1025, -9880, -2537, 3494, -2579, -4013, -8482, -2997, 7, -8561, -1811, 981, -7381, -7753, -1668, 5876, 2516, -9935, 5151, 5314, -5888, -7022, 9675, -8426, 1068, -6404, 8365, -7966, 8072, 8819, -7252, -2839, 9037, -5330, 7835, 9652, -1021, 8873, 3363, -3893, 7494, 1243, 5598, -3253, 6563, 4693, 2667, 3236, -9252, -608, 8763, -3435, 3836, -1631, -2178, 4732, 6104, 9342, 3552, 7050, -8913, -506, -8011, 9083, 6526, -5202, 3931, -8846, -1988, 6213, 6111, 4741, 2831, 4948, -100, -5639, -9105, -282, 5102, -9632, 2172, -7742, 6360, 8545, -8024, 1483, 1067, 5346, 2922, 9128, -7727, 3637, -3900, 9551, 3143, 2635, -5794, 5475, -8405, 5142, 9369, -1554, 6526, -1171, -213, 5681, 8843, 3186, 224, -3688, 7032, 2878, -9492, -3826, 2986, 4348, 2357, -2646, 1948, -1734, -2008, 7187, 5350, -6504, -56, -9680, 1154, 6145, -7768, -9924, -9404, 3680, 9522, -4977, -1103, 4538, 4671, 1662, -6935, 7328, 8464, -1055, 8955, 7849, -5893, -6282, 9671, 5803, 5048, 4928, -5496, -4882, 1412, -760, 8568, -9154, 4855, 5599, 4767, 6626, -9755, -7016, 1957, 1172, -820, -5580, 3040, -3410, -6988, -4787, -7322, 6851, 3578, 7851, -8182, -1649, -1779, 3177, -5915, 7676, -9038, 964, 6051, 8436, -889, 1196, 3296, 9575, 2714, 5406, 5267, -9763, 5272, -1823, 4462, 6471, 8301, -399, -5317, -23, -5585, -7737, 6646, -2671, 247, 5465, 660, 3106, -9008, 781, 4699, 2313, -3639, -7179, -4738, 2302, -3966, -2216, -2415, 2313, -5157, 2780, 9050, -3464, 4385, -8862, 5496, 8368, 4605, 2651, -5834, 7194, -8390, 4076, -9949, 2024, 5312, -872, -1202, 4403, -3683, -2638, 2387, 8175, -6310, 5670, -1941, -6324, 4744, 8525, 2113, 5622, -894, 6976, -7753, 7275, 6032, 1473, 1373, -552, 8099, -5133, -7952, -4150, 5486, -2247, -2246, 6789, 3835, 4136, 6586, 8869, -8062, 8305, 7539, -9527, 1734, -7572, 2433, 81, 8082, -8051, -5444, -3192, 4984, -900, 4136, -7092, -2728, -2003, -9313, -8462, -2480, 7193, 688, 9187, -7768, -5971, 5211, -2983, 6126, 7483, 944, 3514, 1785, -4665, -2249, 6664, 9892, -4615, 2700, -10000, -2120, -5068, 1797, -5073, -889, -6786, 9312, 1631, 5868, 3400, -255, -4109, -1015, 8491, 8278, 5648, -2975, 1748, -4147, 3677, 3693, 3629, 6528, 2553, 6156, -3972, 429, -7736, 5236, 8102, -4948, -3625, 8303, 900, -34, -9701, -8037, -8896, 638, 8971, 4946, 3994, -5252, -6588, -9484, -7990, 8632, -6989, -7110, 424, -4428, -1011, -1381, -7775, -3853, -6301, -809, -5919, -9821, 9072, 5647, -3426, -5591, -1753, -7251, 1403, -6122, -7076, -4826, 9450, -3689, 3141, 5496, -7000, -9870, -6948, 9906, -8482, 4040, 3886, 9611, -3074, 2010, -5116, 7559, -6449, 1167, 5818, -9942, -5437, -9312, 8037, -5826, 9947, -661, 9651, 7835, -6646, -3919, -8453, 6667, 6605, 6467, -9775, 5738, -295, 5367, 1292, -4456, 2641, 5077, -4206, -6795, 817, -637, 6387, 7348, 9626, 7003, 996, -534, 5962, 8504, -1897, 2756, -2012, 7158, 5841, 758, -5981, -4561, 4953, -2255, -934, 4755, 291, -9408, -1443, -6470, 3414, -2698, -5056, -7692, 990, -453, -6019, -445, 5510, 6294, 9336, -9465, 1004, 4630, -1947, -8306, 7950, -9760, 6022, -3770, -9089, -458, -4471, -7821, 7401, 1089, 9360, -4175, 2908, 1354, 2940, -538, -1286, -7198, -4983, -4568, -1462, 3506, -5842, -8512, 5113, -4636, -9003, 3588, 3384, 951, 6321, 3393, -6794, -3763, 1020, -5477, 5403, 5902, 4910, -1326, -7288, -7341, -6001, 9756, -9870, -4671, -3757, 7568, 1478, -5601, -3490, -5380, 6940, -2041, -9209, -4231, -2302, -273, -1134, 3405, 8363, 170, -7899, 5597, 7898, -6655, 5502, -4320, -9634, 9894, 2897, -5223, -4239, 4575, -6419, -1243, -5193, -7104, 2263, 3279, -4044, -5216, -309, 7344, -5238, 2811, -7243, -5030, 5168, 8373, -3585, 7666, 678, -132, -3816, 436, -8828, 2376, -9467, -2458, 5815, -7417, 7132, -4357, 4372, -9516, 7888, 5669, -3866, -159, 3474, 8880, 6367, 9824, 8314, 2137, -4413, 8896, -7736, -3686, -9039, 6834, 7729, -1828, -9154, 4907, -504, -8136, -3842, -8341, 5448, -420, 8517, -6483, 6478, -8470, -3143, -2088, -6167, -5712, 5217, 5996, -6260, 117, -7847, -4466, 7823, -8580, -3710, 4388, 4171, -7571, 6543, -5607, -2629, 1950, -1432, -7594, -6288, 3748, 8729, -2211, 1651, -6845, -342, -9009, 2435, -9377, -5103, 9776, 8074, -9673, -2155, -3013, -879, -2979, -2187, -4760, 329, 2391, 2342, -4249, -8660, 2059, -9976, -2606, -7390, -4521, 262, -1482, -9865, -2501, -3449, -6822, 3180, -8757, 6440, -9561, 2934, 7412, 5212, -1134, 4824, -1710, -3442, 7108, 9778, -1744, 8001, 9566, -1409, 6481, 7165, -1605, -7441, 7862, 2557, -1137, 9413, -8619, 6532, 3426, -5400, 6323, -565, 9586, -6668, 4698, 6836, 125, -5766, 9289, 9326, 2439, -6793, 1872, -2565, 2392, -7291, -8352, 4063, 4467, -591, 3253, 329, 7299, -7986, -4296, -7743, -5267, -2343, 1127, 2566, -4197, 1585, -1545, 7732, -8604, 6135, -9333, -4465, 8921, 8718, 1576, 1774, -4301, -1163, 5359, -2314, 2838, 4100, -5730, -4068, 7304, 9630, -1769, -8786, -8583, -2800, -3341, 2496, 3538, -5110, -9923, 1713, 7193, 5707, -4141, 3483, 5407, -8065, 9580, -1245, -334, 7561, 5943, -3534, -8625, 1757, -2206, 9540, 491, -8285, 51, 6080, 3207, 9147, 2301, 159, -6289, 8597, 2309, 8379, -8896, 4874, -4817, -8598, 2361, -5774, -8167, -8546, -1655, 9204, -4525, -4717, -9094, -5881, -8456, 4389, 3282, -4493, -6812, 7704, 8191, 6170, -687, -7467, 9914, -344, 4963, -2641, 1465, -5571, -4763, -4772, -6999, -133, 7140, -4262, -5296, 2406, 7021, -2624, 7049, 3385, 1514, -2791, -6721, 6845, 777, 2748, 1706, 7044, 7830, 3171, -1731, -5685, 7947, -7065, -3430, 8998, 490, -8405, 227, -9509, 1389, 6918, 2227, 8505, -8055, 7703, 5243, -1736, 2258, -4827, 8672, -7029, -3901, -1741, -7125, -8622, 5752, 4924, 9189, 7204, 8103, 2081, 1063, 9327, -6729, 9664, -4773, 8937, -5033, -5288, 763, -51, -6091, -716, -7426, -3826, -8895, -7838, 5117, 5141, -7408, 7894, -3801, 8452, -5035, -5739, 4518, 9422, 9773, 7610, 277, -1450, 5581, -1735, -7812, -4373, 5264, 7557, 7874, 2522, 3550, -8486, -8899, -120, 3185, -3555, -9990, -1782, 4470, 4121, -8532, 713, -5567, 229, -4002, -2688, -1516, -2189, -4991, 9856, -3953, -5567, -3833, -5997, -7037, 6852, 4805, -5403, -9116, -2551, 100, -4551, -4731, 8910, 887, -2117, 2191, 7257, -229, -3082, 8213, 5745, 7523, -953, 1434, -3367, -3151, 9128, 4757, -5125, 9439, -6466, 1116, 7424, 1798, -5178, -6127, -9567, -39, -6022, -4224, 533, 2678, 5988, 368, -1817, 9942, 5483, 247, -7921, 6353, -692, 770, 3153, 2407, -415, -7240, 6010, 1291, 2102, -1498, 5878, 9483, 3854, 5523, 9040, 1278, -8910, -4284, 2825, 9900, 3489, -7312, 5786, 6609, 4892, 7567, -5226, -9146, -4005, -9074, -9912, 818, 5935, 5906, 1456, -7523, -9421, 9663, -6245, -4740, -9410, -4425, 7776, -7004, -2067, -2080, -4143, -5440, 2805, 7872, -4828, -8300, 6691, -8642, -8687, 1054, -4279, -8159, 387, 8719, 868, 103, -684, 1992, 6655, -4028, 3217, -8502, -3741, -9519, -8130, 9222, -5069, 3665, -3760, -10000, 7409, 7086, 2600, 485, 5021, -1024, -1219, -1870, 4704, -1880, -6836, -5958, -143, 1170, -4171, 9986, -923, -1787, -6458, -4873, -6656, 2012, -2803, -6379, -2813, 8233, -8572, -2397, -5073, -1812, -7395, 9522, -2852, 2123, -1720, 2931, 5367, -9695, -6544, 8814, 9638, 5306, -3383, -4901, 5298, 9050, -5726, 7329, -1067, -3705, -7622, 3259, -7338, 2485, -3428, 7031, -8443, -2414, -322, -349, -1088, 2412, 4047, 3706, -7788, -9620, -2177, -3015, 3936, -2868, 6265, -2614, -1226, 3477, 1032, 8998, 3848, 7292, -5930, -6326, 7162, 7865, 5935, -7164, -3303, 1331, -5861, 79, 275, -5565, -2935, 1415, -3622, -507, 3516, -5299, -2847, 3405, 4881, -8541, -246, -851, 7056, 4833, -2260, -7031, -7499, -2066, 1257, 6271, -3344, -3717, 2660, 9170, 2640, 2584, -9696, 649, 5406, 8309, -6529, 8629, 2280, 1052, 1227, 9598, 4819, -3145, 4308, 3307, -1023, 5817, 4861, -6727, 298, 9294, -3332, -4636, 7972, -7612, -6417, -3697, 9247, 3976, -7846, -9274, -2997, -9176, -7071, 5353, -1100, -1143, 6577, -624, -2641, -1402, 4817, 1174, 6605, -6407, 3427, 3818, -9141, -4156, 5236, -6486, 252, -4630, 2450, -333, -3807, -6262, 4493, 6706, 9111, 584, 1187, 8000, -7371, 6812, 2601, -3696, 9691, 8739, -9536, 2253, -8362, 3240, 1550, 6696, 7206, 6865, 4252, -4959, 4686, -377, 2726, 8602, 2802, 5233, -4505, 519, 8427, 7944, -7298, -4170, 9189, -1107, -8519, 9517, 4471, -4098, -5150, -5905, -4258, -6433, -1776, -9095, -3934, -6715, -4289, 2668, -9140, 7982, -4913, -7177, -7056, -9347, 7815, 2009, 1743, 3086, -1272, -3185, -6369, 9451, 3829, -449, 6772, -1571, -8526, 9782, 2899, -6944, 6515, -3214, -2853, 750, 1793, 434, 6502, 3495, 7890, -1920, 1477, 2129, -3870, 6346, -3090, 6890, -9014, -8466, -8984, 1726, -9477, 9513, -1660, 1856, 7788, -232, -6112, 5894, 5956, 6199, -5340, 1225, 5838, 2143, 3907, -9544, 480, 4868, 8298, 1607, -4187, -8469, 9993, -8316, 479, 7581, 2112, 521, -7598, -9033, 3590, 6866, 7605, -5049, 79, -4680, 3890, 1705, 8926, -1131, -4546, -892, 2451, 902, -4784, -1729, -7241, 6876, 4550, -9424, -4981, 2248, -2179, 1205, -4890, 4914, 9611, 1844, -451, 4103, -3669, 2954, -7845, -7865, -9175, -6575, 6137, 8346, -1938, 2628, -5105, -916, -3110, -8114, 8647, -6113, -8073, 7780, 7341, 6307, -5761, 1247, -4043, -146, -2363, -8966, -1658, 2327, 5737, 5046, -3012, 8666, -1414, -6133, -724, -2867, 7798, -8120, 4249, -8683, -5846, 2891, 4001, -8645, 5031, 1243, -5989, 1689, 402, 2149, 6479, 8638, 9153, 6217, -8473, -831, -4325, 437, 3513, 1954, -7548, -8008, -8682, 6756, 5429, 6473, 4145, -7056, 5681, 5236, -3260, 9799, 2414, 2997, -7838, -1973, -4955, -1345, -7124, -1740, 8743, 5513, 6411, -8465, 9217, -7390, 2784, 4934, 6356, 4191, 7378, 3923, -1772, 3975, 5406, -7373, 4907, -3911, -4544, 2325, -9016, 3692, -2886, -7898, 9471, 2732, -9631, 7876, 4881, 9043, 481, -1428, -5247, -2958, 596, -1126, 9782, 5581, -1241, -2770, 2906, -9990, -645, -2388, 6952, 236, 822, -7803, 1144, -671, -6964, -7357, 404, 66, 8254, 6513, -1468, -9097, 7036, 4664, 4827, -2641, -9242, -8884, -8208, 4526, -840, -4126, -9204, -3018, -9290, -415, -3454, -7388, -9359, 5993, -567, -8313, 5033, 6808, 8079, -5826, 4158, 927, 811, -3750, 7697, 8364, 6032, 8436, 763, -5995, -2391, -3965, 1219, -501, 8135, -2513, 4522, -6937, 8515, -6657, 5841, -7252, -1909, -2540, 2397, 271, -169, -3917, -299, 2783, 8051, 1741, 4047, 5129, 6490, -6563, -3253, 9543, 9019, -7781, -9906, -4648, -8411, -2468, 7841, 7630, -6448, 2015, -8343, 7205, 1122, -9110, -2797, 3174, 6475, 6367, 2483, -4207, 3620, -674, 5264, -9130, -5427, -4276, 3503, 4815, -5811, -4176, 7324, 3842, 4068, 5010, -2447, 5103, -1185, 7086, 7146, -5088, 8141, 1430, -7065, -4862, 6408, -2088, 6409, -4899, -733, 3759, -7259, -6354, 4015, -2946, 8180, 4704, -7608, -6022, 1306, 4776, -3131, 4050, -383, -2962, 4457, 3179, 980, -2890, -5067, -1242, 783, 445, 9690, 7611, -1512, 2044, 9997, -8743, -5499, 7457, -4389, 7204, 2610, -2289, 3588, 2844, 6992, 639, 6104, -2991, 4260, -3543, 7111, 5383, -6921, -2458, -6729, 5190, 9699, 1986, 3053, 9275, 308, 8838, 4827, 7381, -1451, -4342, -3002, 4230, -5647, -2589, 7732, 8053, 4758, 6189, 7680, -4835, 4989, 5209, 9333, -9539, 8934, -6708, -9322, -1331, -9452, 9436, -3701, -5837, -9703, -9301, 6952, 8985, 6798, 1848, 8153, 4639, 1741, -9196, 3366, 3705, -425, 3559, -1213, -8697, 1491, -595, -5635, -5220, -6434, 5936, -8295, -3160, 2978, 4688, 2718, 6710, 421, 5643, 1302, -2699, 3866, -3836, -2232, 3930, 8573, 4442, 704, 7989, -1474, -1007, -8936, -4068, -1817, -4497, 608, -9204, -2989, 2850, -2104, 7212, -3720, -8599, 1355, 5771, 8739, -3207, 5337, -7438, 1437, -6800, 9800, -9667, -1694, 6907, 440, -1047, 6478, 2197, 7111, -2466, 4563, -6625, 4788, -7968, -902, -4905, -9033, 2676, -1348, 6871, -5943, -1222, -2916, -2352, 5826, -1565, -2364, -3395, -8304, -9298, -5739, 2733, -6236, 1474, -6584, -5863, 5037, 7950, 8244, -2717, 1788, 8748, 35, -535, 8055, 3633, 9209, 5556, -1728, -8401, -873, -7683, -5955, -9653, -7152, -593, 2653, 6797, -3815, -5958, 7519, -4042, -6759, -4575, -9099, 1430, 7341, 243, 1381, 5379, 5315, -3517, 549, 5421, -76, 6986, 1567, 3242, -6260, 8472, -8412, -579, -5944, 530, -3664, 2660, 5531, 8012, 7830, -2795, 1358, 9725, 7871, -2193, -6241, 1134, 5858, 8666, 5806, -5346, -2405, 7216, -9625, 916, 7209, 2878, 17, -7110, -4818, -8429, -5265, 1931, -5935, 7103, 2249, 4804, 8586, -5158, 2320, -9952, -1607, 4570, -7441, 7835, 37, -2809, -2173, -6077, -7663, 4258, 5358, -1449, -3604, 6403, 4667, -1981, -6543, 3156, 1256, -5188, 6833, -7039, 1554, -2914, -929, -6095, -4330, 7962, 9224, -8942, -5793, -2985, -3059, 3297, -836, 1689, -754, 486, -9828, -6527, -1546, 6123, -9481, 1103, -3768, 6536, 7823, -2452, -2324, -9300, -5215, 1290, 9108, -7625, 6827, 1302, -6687, -9392, -3786, 2596, 7142, 9827, 8095, -6116, -1935, -7197, -7435, 5606, -9614, 981, -4081, -7345, -9833, -2618, -1475, 7851, 917, 858, 5470, 4529, 7610, -6750, -7976, 2453, -479, 3024, -7356, -7471, 8864, -5163, 2957, -8345, -1679, -1769, 8126, -3353, -3410, -4263, -3353, 7395, -2879, -944, -2250, 2854, 4318, 3677, -8647, 5150, 7301, 8912, 8123, -7320, 1681, 284, 7640, 2748, 7824, 3419, 7434, 2425, -3442, 6785, 7377, -7853, -4472, -6053, 6130, -5425, 5905, 8125, 9349, -9305, 5180, -6486, -7938, 5138, -883, -402, 9898, -7037, -7783, 7008, -2207, -4687, -8621, 9282, -915, -4099, 4159, -4913, 5353, 2221, 3803, 5112, -8249, -2468, -5171, -5555, -4619, 3260, -3297, -8402, 3727, 6394, -2853, -8351, 6383, 4563, -2563, -8474, 2917, -3201, -7037, 1166, 6174, 1931, -6938, -2243, 5022, -7546, -9527, 5377, -7802, 4945, 602, 9593, 9095, 7746, -8330, 412, -90, -4402, 6364, 3594, -6722, 2469, -9451, -9751, 9397, -823, 7550, 9079, 7754, 978, -5021, 1917, 2170, -8919, 883, 2658, -8937, 2836, -4159, -5607, -8376, 9864, -6163, -127, -7334, 7018, 6069, -9948, -9542, -2932, 2074, -8959, 5490, 43, 4982, -2775, 4208, -4216, 9114, -592, -3353, -9888, -4435, -1743, 9529, -9755, -8713, -3497, 6218, 9490, 7090, 7441, 2311, 5570, -1392, 5516, -2953, 7974, 9629, 6756, -7077, 8654, 9599, -3014, 7968, 3336, 7436, 5298, -2944, 6583, -2842, 8627, -8453, -4138, -9008, 4450, 5310, 7, 5082, -8726, -1250, -862, -9929, 8635, -8424, -48, -8074, 6778, 381, 7324, 7000, -9541, 780, -1432, -8895, -9514, -9427, 5357, 9150, 4734, 1903, -9169, -5002, -7416, -5217, 2264, 8667, -9655, -705, -9246, 8402, 7555, 894, 2937, 3916, 5471, 4207, -2319, -2176, 2072, -8208, -6014, 7507, -3584, 9160, -5330, 2955, 6667, -8050, -3912, 3725, -8026, 6260, 6624, -4745, 9081, 1455, 6504, -2756, 6846, -2813, 593, -4873, -2507, -3584, 4815, -1410, -6569, 112, -3136, 1497, -1371, 833, -1185, 7368, -8470, -6084, -3056, 9227, 4929, 4731, -7611, -4899, -7970, 1452, -8559, -8030, -508, -6895, 8629, 7721, 4658, 1030, -1313, 8467, -7010, -7846, -2748, -937, 1625, 1396, -9794, -6405, 4968, -2925, -4654, -402, 9752, -3044, 6289, 6612, 8002, 6518, -7216, 3631, -5246, 405, 1506, -7042, 5472, 2257, -8856, 5931, -3285, -7777, -1966, 3381, -7518, -1099, 9081, 6778, -6809, 5890, -2540, 355, 4739, -8975, -4124, -4203, -4881, -725, -6796, 5881, -2995, 3950, -8376, -4285, 6118, -6649, -1948, -9186, 1033, -7894, 3990, 6933, -1388, -2421, 7888, -5686, 7686, 1137, -577, -8558, -8876, 4072, 7791, 7883, 1158, -5542, 8826, -8486, -2830, -9199, -90, 8491, -8899, 3029, 6667, 6231, 3221, 4206, -7522, -2971, 4396, 478, 6835, -4893, -4776, -5431, 2369, 1831, 4356, -7298, -8399, -9209, 374, 4714, 120, -1966, 5331, 1058, -6860, -3863, -5486, 7616, -9618, 8901, -4832, -6849, -5552, -160, -9028, 299, 1995, -7251, -284, 4681, 6478, 3055, -3583, -2499, 3266, -6454, 6791, -3571, 7483, -8090, -1060, 4873, -4594, 426, 6874, 9914, 1183, -9686, 4108, -3882, 2367, 9793, 5691, 6981, -1777, 4148, 8191, 4630, 5310, 3964, -3551, 5833, -3293, -3750, 7443, 8543, -440, 5972, 6000, -8775, -4966, -4213, 9585, 128, 2790, 9613, 9012, 8334, -2945, -6669, 1820, -5438, 6247, 8334, 6907, -7371, -8379, -4985, 8991, -3656, -3545, -8578, -7415, -7720, 7087, 3189, -6817, 1375, 7736, -8638, 2933, 6365, -964, 7651, 7515, -9908, 1327, 5867, 2729, 2919, 1422, 5132, -4670, 1926, 2651, -8817, 4444, 2663, 273, 1237, 4233, 2596, -6285, -2497, 839, 3639, 8968, -9956, -8829, 3079, -9506, -6932, 4903, -6433, 1123, -1320, -6718, -7917, -9631, 4862, 2990, -5063, -5145, -6201, 5138, -891, -9527, 5973, 1312, 9068, 2626, -8226, -2758, 4050, -3512, 8262, 8406, 9954, 2369, -5040, 7179, 2529, 5526, -8130, -1698, 823, 9886, 9795, -8354, 6391, -8655, 2280, -6419, -8319, -4831, -1128, -7762, 5504, -5499, 8841, 6531, 5315, 7682, 6151, -9919, 7801, 3985, 9159, 7829, 2724, -7575, 4467, 4441, -7135, -8069, 4499, -7691, -906, -3473, 5824, -837, -2788, 9371, -973, 7395, -9285, -7474, 2065, -2050, 7374, 5895, -3834, 7552, 5673, -1306, -9385, -6713, -4210, -848, -49, 1131, -4767, 9262, -1861, 1242, -6402, -6954, 7091, 9691, -8482, 6957, 1490, -9541, 6346, -4597, 2511, -3178, 4790, 8021, 8529, 1553, 7309, -8076, 4250, 8865, -5158, 1667, -7015, -5667, 6624, 6304, -9686, 5501, -4946, 4662, 105, -9580, -5910, -2093, 4823, 1182, 6881, 4947, -7287, -9059, -6956, 7817, 3895, -7924, -1206, -8023, 5273, -5446, 5816, -1441, 4167, -3858, 842, 8152, -5278, -7679, 5762, -351, -5916, -9599, 4882, 8280, -6149, 8377, -1871, 7848, 9759, -7805, -4544, -1331, -8574, -4923, 8288, -4191, -3364, 690, -8874, -5447, 9541, -3466, 2387, 7838, 9822, -5738, -9752, -888, -7612, 8598, -6301, 5108, 7861, -8244, 5194, 4864, 1702, 9916, -6753, -2945, 2441, 8893, -5383, 4199, -3408, -3857, 8637, 170, 449, -4174, -1672, -2760, -9316, -7923, -8659, 70, -2786, -926, 4456, -2053, 4694, 107, -6025, 571, 9501, -8293, 2721, -2892, -3247, 8118, -1604, 3991, -4835, 560, -7970, -8667, 755, -310, 8818, -4132, 3113, -2850, -3462, -5380, -3342, 7622, 5563, 7666, -7631, 3511, -2372, -1745, 7678, 8642, -305, 1193, -2772, 9765, -8243, -9013, 6514, 1662, -6753, -6988, 2832, 5964, -7425, 3074, -2942, 9455, 5099, 6990, -1287, -4451, 7919, -1492, -4251, -538, 7240, 353, 5129, -8938, 7550, 9590, 5325, 7565, 1663, -5558, 8462, -7033, 6396, 421, -6335, -6912, -4396, -9600, -5861, -2501, -3243, -7061, -1351, 3877, 3302, -1299, 4663, 5795, 4927, 8843, -6517, -158, 9032, 3372, 2730, -4076, 5117, 8679, -2514, -3815, -541, -9582, 4146, 1072, -6795, 7685, 5478, 9845, 6823, 6765, -7584, -8195, -8326, -363, -3988, 1835, -9392, 5844, 7894, 1641, 9358, 6386, -7966, 6174, -9211, 943, -2988, 2641, -5413, -7571, -4131, 1000, 7315, -4656, -8166, 8997, 970, -9230, -592, 4188, 4776, 6243, 7802, -4564, -5036, -1868, -6738, 1282, -3040, -8751, 4180, -9066, 7869, -9289, 5483, 1462, -6671, -2698, -9835, 6609, 9762, -429, 7935, 9025, 3774, -5012, -8459, 7723, 7302, -650, 7229, -9288, -124, -837, -8614, -3143, -6137, 2377, -7910, 5618, -9714, 7202, -7715, -2289, 2445, -8280, -3517, -2493, -7299, 7142, -9255, 2547, 8912, -7550, -2461, 2053, 9126, -9856, 3888, -3849, -4718, -6562, 2168, -9795, 6919, -8975, -1741, 1435, 9627, -6510, -4893, -2847, 6633, 4780, 6367, -9564, 4009, -1924, -5468, 3608, -1510, 0, 8132, 9415, -1246, -7460, -4765, -9355, 8911, -9777, -3281, 7260, -2894, -780, 8960, 3297, -7798, 6229, -473, -4697, 3285, -4152, -498, 432, 2282, -3802, -8872, 2039, 9531, 1666, -1682, 9467, -2607, -9003, -390, 6383, 9509, 1426, 902, 7602, -5706, 6045, -5005, -3212, 3709, -2940, -4875, 1405, 6376, 6428, -7347, -4496, -6636, -3421, 1711, 1760, -3603, -8053, -4728, 2413, 1404, 8380, -4614, 1746, -243, 6636, 1773, -8519, 9101, 2062, -8314, -8578, 657, 9735, 5015, 7266, 209, -1330, 5275, -2676, -3448, -9347, 1917, 3016, -214, -9657, 7667, 9518, -9091, 4263, -5679, 6536, -1459, 8647, 855, 5864, -2718, 9371, 4052, 8829, 2716, -5947, 866, 2190, 5302, -9275, 8974, -5281, 8321, -8553, 1382, 5196, -3783, 2721, -7305, -7958, 5912, -4815, -1311, -1997, -2727, 5647, -9973, -7315, -2277, 4245, -7542, 4574, 9654, 2449, 8554, 310, -2633, 4239, 3049, -8841, -6119, -7858, 9081, -6579, -1377, 2203, 468, -9880, 2607, 3158, 3861, -7607, -3427, 5630, 5535, -2641, 3061, 3587, 2247, -6572, 9161, 4293, -10, 7166, 8420, -2597, -9441, -1374, -4112, 6432, 5865, -9398, -30, 4960, 2781, -3267, -271, -455, -7378, -6626, 4558, -3350, 5934, -6017, -2446, 7766, -1363, 3808, -4199, 1709, -8539, 8484, 7786, -5088, -7429, 7661, -1878, 9942, -6344, 6340, 6287, -9447, 6091, -5222, 6146, -5688, -2047, 3154, -2013, -8646, 3090, 9454, 228, 3411, -1716, -535, -8535, -2030, -3315, 693, 6674, -5203, 8631, 8674, 8639, -4632, 1391, -3547, -3923, 955, 3898, 4534, -3465, 1443, 3338, 158, 7165, -3341, 2355, 6089, 8277, 9921, 7206, -9017, -2740, -627, -5077, -2598, -5554, -1543, -6667, 4390, -8988, -2682, 9275, -8502, -1602, -9569, -8331, -6896, -3634, -8987, -1431, -3530, -3512, -7863, 1911, -3811, -491, 9058, -5144, -6207, 7308, 6152, 8234, 5788, -5198, 3634, -4560, 9198, 2639, -4998, 8349, 6737, 2942, -2252, -6069, -4419, -8795, 138, 3033, -7126, 1253, -6182, -5298, 5267, 8434, 4528, -7218, 5715, -836, -858, 9363, 5059, 9844, 5470, 3956, 6151, -2798, 9807, -8217, 6771, 5450, -629, 1112, -7329, -1747, -5766, 3485, 42, 1113, 1186, 2913, 9516, 141, -8925, -5158, 5445, 2072, -6778, -3558, 6518, 1954, 6859, -1097, 8031, -6890, -1386, 5115, 1937, 5607, 3582, -6968, -9302, -2585, -9590, 2613, -7344, -4211, -1485, -5940, 702, 4505, 3644, -4403, 2389, 5102, -4370, -1740, 7750, -1242, -3484, 7730, 5395, -2924, 4270, 6511, 9330, -3193, -4261, 5177, -873, -2190, 2062, 7822, -669, 5686, 8031, -433, 8113, 4749, 9421, 9610, 7807, -7392, -7357, 4380, -1675, -1998, -8443, 7443, 1181, -7586, 3252, -3207, 995, 7049, -3477, -9238, -1068, 7539, 1787, 8319, 4553, 5000, 8412, 3521, 5993, 4640, -6175, -4254, -6581, 4341, 6093, -5417, -5030, -6229, -7133, -1270, 9861, -7526, -5336, -8268, -1544, 8551, 5590, 7076, -7310, 5986, 2189, 3214, -2761, -1597, 1292, 836, 4949, -3724, -3967, 5282, -1510, -8987, 9906, -7035, -8832, 3773, 612, -4640, -7353, -4307, -6049, 2727, 8622, 2898, 2330, -3842, -216, 7233, 2001, -699, 2395, -2604, 6362, -5017, -4491, -3628, -9008, -8552, -304, 4824, -6840, -6877, 4620, -3489, 6073, 4620, -619, -6682, 636, 5091, 3093, -7717, -2667, 2676, -506, 7214, 8831, 8503, 8455, 3830, 1489, 4880, -647, -4771, 5258, -6577, 2296, 4080, 7258, 919, 4628, -3987, 3096, 9317, -7194, 9488, 6203, -6414, -4625, -4463, 8555, 139, 4859, 9320, -2478, 8083, 2986, 341, 4589, 8298, -8254, 9595, 4082, 6722, -6134, -2007, -8521, 7965, 6944, 1240, -835, -8875, -5837, 4047, -2471, -4114, 8117, -8490, -7857, -5862, 5571, 6698, -181, 256, 2660, 9716, -8144, -5232, -3422, -6219, -6078, -971, 2650, 1493, 715, 5310, 797, 6489, -2922, -4501, -4752, -4229, -9314, -7965, -7023, 714, -2574, -4259, -8179, 7164, -8943, -9310, -7999, -9493, -8716, -4371, -3021, 9592, -8475, 8867, -5424, 9169, -5240, 3034, 7764, -6829, 1891, -324, -5219, -2281, 3874, 9427, -9070, 9609, 152, -2375, -9985, 5588, 3481, -8974, 6530, 9893, 3272, 8304, 4455, 6532, -61, -1059, 8125, 3031, -4108, 2448, -5032, -6074, -9016, -4552, -8672, -1503, -1632, 3951, -2869, -6390, -292, 2250, -8551, -2582, 1339, 7507, 2773, 9208, 9973, 8574, 7626, 1221, -9619, -4349, -6171, -5756, 2930, 4677, -3927, -3130, 12, 7108, 1975, 8143, 8015, 5764, -7738, -2307, -3247, 6397, -3259, 7544, -1933, 3633, -7245, 2795, -2584, -5628, 1812, 9953, -5824, 7743, -5276, -1705, 691, -5443, -6056, -6251, -7615, 3122, 5887, 9035, -901, -4528, 5922, -4117, -3233, 9296, -1505, -6596, 9038, 9945, -9452, 7064, 5809, 8422, 3595, -120, 5056, 2469, 7071, -5357, -1728, -5918, -8640, -4108, -2723, -7216, 9461, 2992, 9288, -7847, -6199, 3206, -8693, -8514, -2848, 9862, 1095, -347, 2975, -8740, 9761, 8191, 6930, 9025, -6072, -5596, 7937, -3664, 360, 6064, -1168, -6539, -4895, -9699, -7070, 8458, -7277, -1681, 6941, -933, -8787, 138, -6777, 6769, 2489, 279, -2214, -7683, 5395, 6867, 2751, -8433, 4727, -4231, -6838, -9452, -4779, 8177, 9725, 7369, -3358, -2108, 5759, -9163, -3576, 6820, -6143, -192, 8122, 1555, -6946, 8191, 6520, -6280, 6092, 6111, -8484, 7546, 4779, -2318, -3813, 7450, -5755, 5324, -7091, -5931, -1983, 2461, -6651, -9060, 9822, -2511, 9496, 8680, -4592, -4624, -2282, 6593, -7768, -2504, 7260, -8337, 8613, -4985, -596, -3737, 2677, -8174, -661, 4998, -9459, -2339, 3890, -9571, 906, 598, 3888, -4398, -2614, 4269, 1351, 7913, 3133, 8575, 641, -1979, 8188, -9433, 3596, 9056, -2573, 5344, 7884, -8569, 3265, 3415, 1123, -687, 9136, 3497, -8166, -2417, 8687, -3360, -3921, -8394, -5554, -6627, 1434, -845, -1789, -5606, 7281, 1432, -1072, 2730, -3103, -9701, 1558, 9796, -8587, -7644, 124, -4768, -2694, 2726, -5499, 5361, -2342, -2449, 7154, 104, 6682, 4777, -1305, -5639, 3830, -1857, -2321, -6846, 9326, 1205, 7684, -1769, 7414, 5967, 8695, 1706, 3833, -6665, 4011, -8601, -7302, 5952, -5186, 8908, -3940, 4695, -9395, -1278, -4231, 9508, -3638, 2461, -8802, 3285, -179, -168, 184, -7136, -1537, 674, 7034, -545, 2784, 1741, 6196, -1215, 1333, -2688, -6306, -180, -6075, -1138, 1427, 493, 2645, 9455, -1520, 3530, 638, 5389, 7753, -1398, -6644, -3022, 3783, 6150, -7645, -9181, -617, 4164, -192, -7704, 4480, -9167, -5389, 7857, -9392, 2304, 3894, 7286, -5120, -4220, 361, 6823, -4356, 9684, 6622, -6313, 5942, 6263, 819, -7158, 3921, -8883, 7645, 2513, 9046, 2739, -4916, -7238, -5968, -8743, -6898, -8860, -9931, 6114, 4591, 3424, 5913, -9202, -3269, 5369, 4813, -8309, 8273, -7578, -6559, 4547, -4206, -7017, 2806, -7603, -5902, -8518, -7400, 9589, 6471, 5093, -9603, 1846, 1561, 1872, -3712, 9773, -5558, 1094, 8440, -6865, -3211, -9855, 1572, -1651, 206, -680, 5582, -5429, -676, -6627, -1355, -4177, 7211, 3442, 6233, 8723, 4190, 7226, -3351, -6818, 1305, -5924, -9791, 1141, -5602, -2259, 1567, -3535, -4737, 9897, -4110, 6527, 2427, -7402, 9898, 7833, 392, -2016, -1543, 1915, 1858, -9076, 9131, -1802, 2750, -7725, -6240, 1185, 3999, 8033, 2223, 8663, -1562, 4670, 650, -9124, -2660, -622, -7353, 5135, -533, 653, -4658, 9219, -9089, -5356, 591, 1126, 5848, 2714, 5508, 1977, -5599, -7795, 9779, -2417, -2520, -3325, -1147, 7173, -834, 6750, 1797, 5593, 8418, 4865, -8642, 2710, -3440, -6594, 9646, 8591, -2430, 5320, -1564, 1937, -1222, 6465, 824, 3046, 1892, 177, 8885, -2161, -1586, 1983, 8032, -7549, -2545, 5601, -2524, 1601, 403, -3277, 9945, -9405, 6414, 1608, 1787, 8432, 6925, 4859, -5633, 3777, -7437, 2415, -9707, 2467, 4785, -4156, 2844, 4671, 776, -7671, 4254, -4440, 4523, -2688, -7220, -7002, -1289, 1412, -2380, -6078, -3961, 8570, 2344, -3572, -7434, 8718, -5358, -9123, -7841, -7, 7859, 9275, -9096, 4142, -8412, -2723, -441, 9433, -66, 1001, -7236, 569, 9560, -9505, 2169, 6067, 4464, 5879, -2403, 2016, -1758, 2501, 6022, -9321, 4148, -3283, 935, 9975, 677, 1736, 8870, -7978, 3432, -2362, 3956, 8999, -6382, -3462, 1932, -6845, -4939, 2586, 2686, -4756, -5097, 1037, -2531, -9923, 3620, -3973, 4280, -8955, 9897, 1372, 5235, 4207, 59, 7523, 3409, -9957, -1829, 5342, 4941, -451, 1074, 9407, 5787, 1878, -3510, 582, -4898, -5321, 8197, 5753, -4997, -7794, 2670, -518, -7195, 2399, -1507, -2107, 5468, 1276, 6781, -4124, -773, 9933, 878, -6233, 1662, -6947, -5911, -9363, -4404, 6020, 2854, 6291, 5019, -9295, 1417, -8436, -3318, -612, 1239, -3284, 7551, 5868, -9978, 9524, -9502, 9676, -7129, -1297, -7050, -7146, -8442, 545, -124, 2915, 7290, 8581, 6848, 4356, 4554, -4342, 1384, -6712, 1815, -3102, -3207, -4907, -7469, -6032, -2502, -9415, 8753, -5164, 8697, -578, -3253, -9314, -6724, -4200, 5364, 7538, -2617, 7393, 7466, -6808, -7551, -3778, -2926, 8717, -7633, -4232, 6755, -64, 2683, -9532, 9038, 8801, -6761, -3704, 5441, -1768, -6153, 1304, 9182, 3481, 6406, -7643, 9694, -2340, -4496, -3154, -2309, 8611, 5644, -6486, -2588, 7182, 3523, -7086, -6972, 1706, -7834, -9965, 6165, 3471, 3353, 8224, 769, 6627, -3634, 8231, -5339, -9482, -8933, -45, 7752, 7292, -8133, -3431, -2384, 9370, -3502, -7535, 2486, -9995, 2126, -7284, 2933, -6047, 6183, 7200, 2563, -381, -7335, 8861, -2899, 2052, -7224, 4807, -5439, -4624, -9958, 8214, 5482, -1427, 136, -5735, -8688, -6859, 9403, -2546, 2683, -3114, 6789, 600, 802, -1387, 1289, -495, 7922, -329, 3025, 3020, -4654, -765, 63, 7416, -4363, -7406, 5405, -215, -8623, 8771, -1409, -6272, 8949, -1222, -7626, 4252, 74, -7175, -3827, 5795, -9376, -4740, 2236, -1521, 1027, 2538, 678, -1582, -3102, 7298, 3855, 945, -6550, -4522, -3284, -3015, 6871, -3529, 3193, 8734, 5966, 4981, -5635, 6506, -7926, -95, -3024, -8477, 8247, 2691, 4320, -6845, 7472, 9254, 6525, 2960, 8589, 9818, 5767, -6259, 3698, -3059, -2141, 2372, 4170, 9524, 2263, 9418, 7747, 3327, -8251, 6074, 563, -7915, -1365, -6351, 1771, -9954, -5350, -6072, 9520, -2915, -3274, -5226, 553, -9624, 326, -7524, 6122, 3379, -5328, 1021, -6012, 6507, -2097, 3633, 3576, -1087, 5410, -8149, -7300, 5322, 671, 4139, -4920, -1325, 4355, 3024, -3031, 5513, -1883, -4663, 9451, -2105, -6263, -8361, -795, 9094, 5668, -9206, -1651, 407, 2410, 9361, -7108, -393, 1930, 3426, 9155, 4869, -8126, 9428, 7464, 7098, 6238, 6982, -6410, -2536, -3477, 964, 1027, 1082, 4506, 4138, 1286, -9859, 4456, 1748, -1547, 9263, 4310, -7624, -9776, 3128, -7033, 7691, -3220, 3227, -2809, -4183, 565, -8946, 9240, -8236, 3807, 5109, -2693, 1902, 3407, -9579, -4022, -8131, 9377, 5601, 9657, 7268, 5407, -6341, -322, -580, 2735, -4002, 6376, 1747, -2337, -5793, 2696, -3, 7162, 8742, 2370, 4406, -5294, -5402, 6807, 4933, 1107, 614, 3355, 9579, -1907, 7141, 8243, -9368, 4460, 3998, -7378, -4124, 1064, 9270, 8330, 7006, 7925, 482, 799, 7678, -8949, -7323, 9794, -215, -8880, 6278, -4413, 7086, 9972, 5479, 4890, -5694, 5734, -9741, 3756, -1058, -380, -6096, 8351, -8111, 2016, -3621, 859, 6350, -3811, -7720, 1351, 2536, 4539, 9657, -4654, -1280, -1127, -3674, -262, -2153, 3362, -6687, -2938, 6024, -7435, 9738, 5889, 8115, -2782, 8166, 6281, -1081, 4780, 9820, -7459, 9317, 3432, 6764, 4240, -8616, 487, 6306, -8086, 5212, 1111, -8030, -2669, 4450, 3584, 6424, -166, 5932, -835, -6408, 52, 8573, -3941, -9264, 4410, 6347, -8085, -8172, 9413, -6380, 9430, 8645, 3852, 8292, 8477, -6834, 5607, 4390, -5218, 7062, 9001, 2825, -5828, -4506, -5183, -1443, -1381, -8016, -2126, 2058, -4542, -958, 2914, -1498, 9313, 5474, -5437, -9398, -2329, 5412, 7716, -4676, -6797, 9221, 8196, 4495, -7169, -6633, 8447, 4072, -6003, 6863, 8660, -4348, 2200, 6145, 7747, -9009, 3519, 9158, -8220, -7453, -6295, 8154, 94, -9700, 1648, -8705, -4274, 9919, -1749, -5335, 4890, -3810, -2446, 6582, 6145, 8825, -6332, -6884, 3005, 7766, -7443, 2482, 492, 925, -3146, 4084, -5703, 3674, 5138, 2310, -3394, -2591, -2156, -3608, -3596, 2806, 8620, -8203, -6364, 3604, 6166, 3284, -242, -8633, 4471, -5251, -989, -4138, 2272, 5871, 3787, -2405, 5872, 9895, 4131, -2167, -8374, 2445, 1902, -7137, 8282, 1442, 9509, 3626, 1669, -7854, 6879, -8738, 9283, 3294, -7122, 383, 9778, -735, -8386, 5438, -5040, -4540, -202, -7042, 8483, 8838, -5896, -2276, -3945, -3393, -4430, -83, 5614, -4561, -3928, -8542, -7959, 3601, 4133, -1299, -9306, 9761, 4909, -8887, -7819, 845, 1998, 4582, -7681, -1054, 3281, -2134, -8900, 8948, 1161, -8474, 8862, 7543, 5272, 4710, -1653, -3127, -3912, 9595, 8528, -3923, -8826, 9637, 1450, 8993, -790, 9270, 1018, 1882, 1183, 5171, -380, -2743, -7148, -903, -8343, 5649, -5491, 775, -5240, 551, 6045, 171, 6521, -1544, -1406, -5859, 7877, 355, 7350, 1778, -9917, 1083, 9905, 8437, 5923, 3812, -5828, 9268, 6970, -9608, 5930, -9984, 606, 6320, 7931, -9006, 8031, 5706, -2715, 7148, -2797, 7127, -1893, 5543, 3444, -4373, 3000, 9635, -6417, -1029, -6022, 4203, 9066, 5855, 575, 3127, -9528, -8151, -7287, -2319, -6228, -9932, -6546, -7998, 7585, -6355, -8100, -3513, -8414, -8871, 7411, -2919, 5023, -1451, 5374, 8651, -4173, 1101, 4657, -8483, 6095, -5816, 4366, 8096, -97, -6550, 3448, -2539, 7102, 4015, -4458, 3720, -4736, 3554, -652, 6163, -6155, -4896, 2642, 1927, 6774, 7280, 4978, 7261, 8953, -9891, 3600, -6999, 5907, -9336, -2959, 5800, -2603, 859, -3969, 3855, 6297, -124, 7882, 560, -8807, -5176, 3583, -6838, 7875, -8305, -7779, 354, -4341, 6373, 454, -6439, -2142, 6365, 5062, 1449, -8929, -5456, 5956, 4072, -9927, -355, 7098, 3645, -7807, -8576, -7687, -4466, -4960, -6750, 3463, 4285, -2332, -119, 5459, -7633, -182, 3664, 227, 4816, -3190, -3517, 9496, -1318, 103, -353, -3131, -2295, -4573, 5706, 6463, 6368, -4148, -9381, 6783, -346, 9113, 4016, -2571, -5884, 5703, -1340, 6550, 7751, -9477, -9065, -4081, -2274, 5261, -5678, -4983, 8619, -6464, -9261, 9354, 3554, 9486, 5865, 9000, -5342, 8033, -5365, -8998, -8039, 1760, 9906, -888, -9308, -9279, -2770, -255, -8835, -8650, -741, -1973, -3406, 4617, 966, 1634, -1219, 3557, -6290, -448, -1576, -38, -3477, 8296, -6742, -7535, -2761, 5322, -5482, -6005, 9487, -1962, 5457, -2309, -3129, 9384, -368, 7148, -7665, 0, -8598, 383, 4307, 8343, 5921, -1208, 9638, 7619, -7606, 6822, -6507, -7905, 9844, -9255, 8459, 3402, -3807, -1281, 3478, 2695, -3989, -1240, -5385, -4447, 9302, 4841, -8717, 4411, 6457, -8233, 6229, 6962, -876, -8926, 1383, -5985, -4147, -6475, 1496, 5672, -441, -570, 9054, -3529, 24, -7330, 6484, -6423, 1002, -9427, 9493, 3529, -6518, 6348, 7349, -2924, 6887, 5159, 1006, 733, 1385, 5590, -6509, -2250, -7217, 195, -2336, 7004, -8029, -6790, -9487, 2435, -3667, 9729, 288, -3049, 3263, -3401, -5759, -3833, 5620, 2699, 4289, -8396, -8760, 9770, -533, -2070, 6879, -7843, -1301, -3609, -4621, -5179, -630, 394, 7610, -656, -5183, 6708, -8004, -7071, -3061, -5961, -9097, -7486, -10, -8776, 4177, -8895, 5053, -8156, 5048, 4479, 584, -7481, -1958, -84, -9025, 9661, -2035, -1928, -9352, 4936, 1588, 9201, -8530, -3065, 4622, 573, -1647, 882, -6469, -7464, 9376, 1011, -335, 4077, 9584, -6849, -838, 1598, -8590, 1193, 6895, 9097, -6453, 9959, -4655, -2638, 6755, 7404, -2480, -1818, 4738, -8364, -4616, -4333, 1198, 2954, 7216, -7687, -6967, 340, -1277, 3601, 1894, 4557, 28, -5702, -4432, -976, -7251, 3322, -4049, 9324, -9938, 2446, -6376, 7904, -7531, -6730, -1274, -1263, -1395, 8339, 8239, -8234, 3055, -5193, 8103, 7934, 840, -948, -4056, 3315, 6117, 8057, -4009, -6922, -5032, 716, -6630, 5660, 3416, -7586, -6090, -5644, 2577, -3185, 534, 1571, 6580, 7939, -274, 5829, 519, 7807, -4238, 158, -1464, -3806, 2997, 911, -5483, -8376, -6474, -4244, -8962, -9442, -887, 9981, -3108, -8757, 6584, 5817, 3190, 2030, 6365, -7839, 6360, 3440, -7681, -1027, 3182, -6606, -1590, 5193, -145, -5645, -620, 7883, -7264, -2147, -9197, 5171, -2931, 1136, 6778, 1631, 219, 3546, -2741, 3579, 7401, -2897, 2502, 1939, -4693, -6794, -9667, -7992, -1648, -7773, 3979, -4277, -1174, -4089, 5171, 6522, 1068, -5048, 8456, -8531, 2529, 3012, -9121, 2408, -7486, -9797, 7735, 7968, -9718, 5801, 2626, -2544, 7016, -3722, -3457, 6953, -3269, -668, -8144, -8649, 1294, 1649, -4953, 1763, -8459, -2018, 6100, 3736, 3306, 3233, 6583, 6684, -499, -5956, 4333, 2888, -4995, 9506, -1845, -4433, -6717, -479, 5404, -2266, 7250, 4341, 8881, -2004, -8145, -4590, -1659, -9294, 8583, -2556, 5202, -6955, 2003, 7391, 9251, -6474, 6962, 807, 3155, -1410, -6586, -7179, 9844, 8111, -6966, -4719, 6743, 8695, 1586, -87, 8036, -5621, 2275, -6708, 6855, -2767, 6355, 1057, -108, -9811, 8991, -3029, 5078, -5147, -233, -2132, -1134, -5420, 7250, -3591, 1061, -8218, -8612, 3666, 7951, -1717, 9365, -9824, -1699, -7640, 7018, 2394, 8108, 423, -4995, -9747, -2672, -5189, 9387, 6859, 6550, -6758, 4046, -8853, 9784, -5924, 238, -2158, -6587, -3031, 8920, 9782, 2634, 4057, 2083, 6416, -3766, -1430, 982, 7494, -7735, 63, -5310, -59, -5351, 453, 9986, -558, 1901, -5407, -439, -3920, -6205, -7678, 3087, -8602, 2746, 144, -6008, 3179, 8264, 7157, -1432, -132, 7711, 1226, -2783, 4820, -1567, -4106, 4269, -4906, -3653, 4522, -2100, -4726, 6971, -8007, 8560, 6068, 2619, -7906, -4114, -7873, -1784, 3875, 9147, 3316, 2981, 1532, -3485, 8301, 598, 7676, 7813, -4642, -4127, -6568, -6879, 2549, -5093, 7043, -1477, 5597, -7081, -4097, -4111, -9586, 4513, 4523, 765, -867, -910, 7653, -6352, 6824, -2222, -4266, -4404, -343, -2174, -1353, 6058, -7269, 5568, 3428, -8216, -6092, -3429, 7083, -173, 4907, -7333, 7880, -6625, -3855, 8708, 8921, -8865, 4415, -7181, 962, 5849, 3236, 4150, -7556, 6163, 8925, 5326, -2066, -6554, 9948, -5774, 9775, -5679, -2974, -5989, 1552, -2914, -5739, 7047, -2430, 9120, 6010, -5519, -4476, 5130, 1666, 5759, -9826, -6382, 1180, 6202, 4120, -2115, -7378, 9710, -2733, 9630, -7452, -7965, -7998, -4306, -36, -6136, 1798, 9154, -3387, 5830, -7300, -6975, -8944, 5633, -2273, 2040, -8125, -4161, -6200, 7002, -6777, -6466, -663, -5509, 7713, -2586, -1405, -4837, -4191, -4430, 2498, 4803, 2972, 215, 4566, 4572, -6906, -5688, -7888, -3384, 2442, -7970, -7626, 3559, 8924, -8914, 2531, 5999, 4182, 3108, 5425, 83, -4870, 3681, -7216, 8391, -4057, 6174, 2805, 3474, 4485, -125, -2214, -1853, -6862, -6355, -9085, -5154, -2401, 7608, -8322, -1237, 3847, 8126, -6110, 708, -9453, 6547, -905, 6756, 77, 2171, -4220, -9107, -8525, -8431, 5109, 9434, 4052, 4675, 2392, -5560, -1982, 8234, -4450, -9371, 3451, -7691, 3275, -1124, 5004, 7467, 7415, 8456, -2567, -6960, -5756, -263, 8896, 9186, 5527, -444, 1548, -9982, -8728, 4321, -793, 7006, -7104, -2629, -4792, 4869, -7500, -6611, -9756, -1102, -4963, 8241, 4439, -37, 9989, -850, -2769, -2307, -2596, 8365, -893, 9286, 7095, 5828, -9274, -7808, 3019, 6858, -2173, 1325, 695, 4570, -7966, -194, -2816, 9161, 8785, -6976, 6942, 7702, 288, 2824, 1994, 4490, 486, -2801, 3646, -2065, 4867, -317, 7733, 8967, 5229, 860, 760, 9571, 3893, 4008, 9397, -4354, -7770, 4747, 2873, -5965, 1255, 8626, 1912, -1279, 5738, 416, -157, 7752, 3313, -1338, 3692, -6247, -6267, -8198, -982, -3265, -3590, -3736, -8633, 9376, 1749, -1215, -7399, -9268, 6632, 2611, -5794, 7137, 8023, 643, 98, -9971, -4598, -3034, -1360, -5383, -4253, 9024, -4123, 8384, -1605, -922, 3136, 2924, -8546, -9734, -5056, 2290, 9951, -3773, -6225, 856, 1995, 2042, 6917, 8164, -9, 6784, -7150, 1164, -1205, 2189, -5641, -9066, -7209, -1865, 5349, 3725, 2520, -4105, -6240, 7608, 4687, -2334, 8375, 8772, 5845, -4417, -8794, 2379, 8303, -5579, 607, -3687, -4161, -5191, -8890, -8800, -7614, 5880, -4367, -513, -1189, 2577, -5122, 7240, -3981, -6021, -1286, 5315, -2997, -2085, 6096, 7653, 4395, -5194, 2157, 3986, 6623, -1715, -3695, 2473, -9143, 224, -4487, -9190, -1358, -8998, 4986, -2115, 8428, 3689, 2864, -2845, 6266, 3149, 6110, -3469, -8525, 3368, -203, 6367, 6815, 6077, -6118, -83, 622, 8668, 349, 7390, 1562, 9732, -2020, 683, 4417, -5204, -276, 7529, -3527, -9304, -6109, -5921, 8858, -2060, 5343, 5400, -1664, 9765, -8407, -7487, 9634, 989, 7149, 7719, 7969, -566, 448, 4400, -7346, -9438, -8119, 3451, -5930, -4135, 2401, 281, 2793, -6821, 8174, 919, 6595, -2150, -4838, -6987, 7424, 3088, 7643, 4817, 8714, -1269, 7659, -4264, -7062, -656, -2372, -3616, -8564, 3637, -7217, 6100, 8719, 1329, -3747, -9291, -5281, -9867, -6728, -8647, -9024, -185, -9571, -6859, -8695, 8589, -7937, 2035, -2163, -7639, -2860, 3266, -8385, -2255, 4699, -6478, -2808, -7867, 1339, -5189, -7731, -6886, -2862, 4094, -3242, 2075, 5323, -8038, -5512, -4131, -5831, 9332, 1050, 2538, -108, 1125, 4996, 2247, -8822, -4142, -3157, 5647, -8319, -3858, 2531, 1862, -393, 6084, -7623, -8282, 2358, 8342, 6726, -9280, 6439, 3924, 3765, 7794, -634, 8345, 2484, 2734, -4316, 7629, -9684, 5477, 1776, 2402, 1223, 4607, -2899, -269, -9211, -9360, 13, 6674, 7469, 2934, 3275, -9581, -9749, 4936, 4987, 5238, -4069, 3395, 3151, -4390, 9250, 5256, -54, 885, -492, 4814, -2224, -3597, -3387, -4716, 5189, 6281, 496, -6324, -7845, -4350, 6304, 1266, 2811, -6141, 8373, -8797, 8857, -1725, -1056, -304, -7541, 3799, -9424, -4484, -7914, 5671, 8275, -5229, -4190, -9216, 5126, -7658, -552, 2137, -3168, -3634, 2567, -4988, -2741, 2656, -3722, -9615, -9843, -3023, 9761, -3615, 9171, -9309, -7072, 4136, -5750, 1371, 9894, -5776, -5844, 4517, 8437, 5361, -7242, 355, -7858, 6169, -2769, -352, -6835, 4869, -8015, -5182, 1651, 7275, -2144, 1699, -9341, 725, -5326, 5052, 3281, -9158, 5122, 1960, 4802, -9779, -1535, 4236, -9680, 9953, -9077, -5420, 3119, 3740, 9523, 8741, 8943, 9311, 6585, 7952, 7035, 1080, 3502, 3264, 8838, 9214, -6862, 9551, 243, 416, 7930, -5512, 5086, -9024, 5241, -3399, -1023, -1693, 5090, 6970, 8990, -4305, -4799, -7226, 362, 3725, 8911, -2608, 6384, 7861, 7112, -5180, 6819, 8682, 3872, 1962, -6045, -4784, 6883, 162, -1178, -1804, 2386, -6522, 8375, 4771, -6943, 2515, -5654, 7076, 4618, 7799, 9389, 4435, 4058, 2134, 7908, 264, -2388, -1288, 6459, 1903, -3622, 5800, -4724, 7882, 953, -9547, 1087, 4701, -8640, -5840, -1650, 7178, -590, -5021, 9998, 8669, 3402, 5924, 8407, -2880, -1237, -9592, 8698, 863, 274, -6423, 245, -5953, -4795, 7432, -9368, 9567, 9264, 107, 1474, 4166, -6819, -3869, 6449, 4700, -7430, -6651, 6953, -602, -6865, 8935, 4958, 2068, -4447, -9531, -7339, 5390, 1081, -9332, 6525, -8786, -1405, 9042, 8372, 9251, -3356, -2562, -6157, 7703, 7328, -8950, -1679, -5910, -1164, 4937, 8258, 3935, 1984, -703, 1227, 4543, -1807, 3003, -9336, 6481, -2683, 7459, -2556, 6340, 3250, -3609, -7376, 2424, 2410, -3786, -5895, -5908, 4223, 8550, 7989, 3818, -1851, 4368, 2301, 7824, -1962, -6884, -3859, 5634, 5266, 7236, 4282, 2076, 2959, -9251, 7393, -7057, -4039, -2589, 425, 628, 7209, 915, 9161, 1654, -8954, 1603, 2174, 9589, 7747, 1685, 1036, -3012, 3812, -9693, -6389, 1531, -4272, 8555, 6483, 9130, -9816, 2556, 5673, 3734, -9184, 2205, -5955, 7612, 5671, 3110, 6321, 1886, -6430, -7744, 345, -7257, -1654, 1458, 3740, 4116, -1912, 1246, 4037, 4395, -6503, 5063, 350, 5531, 8929, 5219, -2971, 2925, 5044, 9886, -3363, 7197, 5181, -5384, -1250, -1034, 7511, 9703, 6343, 8199, -9731, 1885, -5579, -1424, -6690, 6009, 1486, -6570, -2589, 6736, 3369, -2427, 4275, 859, -1101, -6614, -1878, 5106, 2864, -2267, -9469, 2943, -4200, 2489, 1735, 5699, -2204, 2407, 8960, -9941, -8879, 6798, -5008, -2599, -600, -8945, 249, -6413, -160, 5670, -6724, 3072, 2172, -3007, -1209, 7739, 3252, 5609, -2688, -2799, -9815, 5858, -4748, 6145, -6412, -2471, -936, 8482, 6143, -1558, 5009, 479, -506, 119, 8931, 6157, 6986, 6882, 9795, 5781, 737, -3527, -9750, -4224, 4731, 1804, 1613, -2157, 9117, -5847, -7693, -6073, -7085, 9884, -9933, -6865, 4974, 7353, -6785, 4105, 9922, 4037, 4557, 9218, -7768, -6548, 2989, -8270, -8811, -2899, 5703, 9033, 3924, -6569, -4830, 505, 8288, -5474, 620, 8650, -9547, 3436, -1091, 9723, 2767, -4854, 6131, 4036, 9343, -1800, -5490, -9093, 3499, 9677, -8778, 6594, -6855, 904, -9626, 8278, 9955, 2799, -6487, -9993, -7351, 1, -5396, -9271, -9669, -7557, -508, 3758, -6231, -5250, 50, 4819, -1637, -9871, 7547, -3112, 4384, 8152, 2778, 3432, 994, -976, -2830, 7376, -9974, -8974, -196, 2538, 2415, 9036, -5109, -6626, 9582, 8747, 3428, -2890, -2868, -6660, 8715, -4206, -5527, 5665, -67, 9249, 8884, -5469, 5615, -394, 2344, 8701, 9331, -9606, 3565, 1636, -915, 5103, 4818, 223, 8452, 2920, -7408, -2282, 6191, -4462, 6657, -891, -6473, 3779, 1799, -5192, 8310, -4576, -6838, 8138, -3451, 5069, -3938, -9562, -9251, 9089, 8696, 8391, 981, -2881, 6748, -2337, 1795, 9843, -454, 5857, -1731, -6292, 7400, 9078, -2622, -1839, 4608, -3309, -5936, -9254, 6610, -4516, 3051, 8106, 3660, -8461, -5457, 8310, -7820, 2908, 6164, 9798, -2438, -2444, -3984, -8406, -5885, -5930, -1219, -8304, 478, -3169, 9783, 1634, -6713, -1028, 6783, -913, -3355, 8326, -79, -4479, -9656, 7257, 3614, -1901, 8014, 4848, 8801, 544, 3883, -1593, -2414, 1218, 6055, -1613, 9666, -5855, 8915, -2531, 2801, -6293, 8997, 5166, 6047, 2433, 4376, 5855, -5472, -6087, 1624, -9054, 7643, 6145, -3416, -9677, 4410, 1835, -5311, 1598, 1654, 4575, -4620, 3777, -7164, 6014, 7352, 5259, 8923, -5186, -6308, -8674, 7918, 1166, 2480, 4172, 9551, 9297, -1036, 58, 3348, 3633, -9644, -1825, -3461, 2153, -5250, 3649, -3335, -192, -8395, 4710, -5539, 6828, -787, -5796, 5852, 3712, -2838, -4595, 5275, 90, -7513, 2916, 3326, 8820, 4201, -7778, 7962, -7079, -8511, 1544, 6006, 7213, 9859, -5155, -3222, 147, -5902, 1307, -108, 8293, 3200, 7949, 4638, -4120, 5397, -8652, 903, -6687, 6341, -9871, 8815, -7750, -4471, -1124, 1884, 8763, 3260, -4434, -3668, 4151, -4167, -6611, 3246, 9987, -8372, 934, 6313, 2854, -7013, 2439, 3175, 2499, -1759, 2633, 1142, 7812, -4570, 609, 6863, -3478, 3097, -1299, 9083, -584, 1606, 9723, 6434, -4872, -1847, 3332, -842, 9835, -5713, -8263, 5780, -3610, -388, -3314, -8682, 7898, 9767, -4061, 6455, 9556, -9258, -919, 5831, -1643, 9154, -3149, -2948, 5643, -2122, -4274, 3405, 7543, -5934, -9802, -786, -8673, -8560, 232, -1893, 9869, -7901, -5436, 9227, -4654, -722, -4206, -4935, -213, -4791, 346, -4998, -5527, 493, 9440, 4133, 5510, -4771, -7727, -5632, -4386, -9158, 4900, 9720, 1875, -3922, 2601, -2151, 8387, 5032, 976, 6422, -6069, 2306, 765, -5434, -8866, 6866, 4317, 7287, 3966, 1033, -6748, 4052, 3765, 6551, -3591, 3775, 5430, 6371, -6879, -8874, 447, 1801, 2092, -4491, -5747, 4652, -1152, 9731, -6802, -1764, 4128, -9832, 82, -1780, 4936, -2496, 625, 2267, -3025, 8229, 3050, 1316, 7130, -235, 695, 6230, 3090, 3099, -3934, -9109, -3979, 5929, 7907, -3790, 579, -8097, -661, -3417, 7589, 647, 1285, -4455, -848, -2174, -4714, -8300, 6726, -6779, 8501, -8869, -2381, 8966, -2235, -7290, -6597, -4824, -4965, -3906, -1606, 3230, 1583, -8039, 6683, 1260, 1110, -2718, -5807, -8984, -8316, -7231, -2228, -783, -4010, 171, 1250, 4934, 2173, -5290, 3615, 9699, -3123, -223, -1385, 9561, -1689, 4564, -4787, -2671, 1392, -6793, 8824, 4296, -2027, -4665, -702, -641, 1343, -321, 2881, -2168, 8985, 2206, 5371, -2795, -2496, 758, -8017, -2111, 3825, -8098, 4809, 5547, -3179, -7366, 2866, 533, -2532, 5921, -3988, -6537, 7034, -7168, 9439, -6981, 2545, -1169, -9825, -2359, 7331, -3347, 723, 2310, 5325, 2936, -4173, -6294, -2390, 231, -5476, -3147, -1345, -8327, 4114, 7212, 8492, 8445, 4822, -7940, -3069, 2919, -9204, -8497, -2707, 4128, -2461, 414, 1085, -2410, 7668, -1413, 777, 6229, -6883, 7773, 5594, 9652, -2098, 2400, -4799, 4875, 8301, 325, 8325, -4815, -3848, -8261, 3034, 9588, -4898, -3383, -3333, -1116, -9152, -4210, -8326, 8857, 1262, 1962, -7870, 6499, -8886, 2349, -5719, 261, -5553, -9941, 4454, 3278, 9561, -2589, 4980, 9337, -2990, -7115, -2830, 5720, 3816, -3493, -9031, 6749, -8881, -6227, 2811, 2927, -9488, -7331, -5152, -7878, -7495, -7515, 9155, 5292, 2461, 9598, -6141, 5677, -4958, 5523, 7162, -6789, 6484, 9484, 6238, 2241, 5064, -9077, 457, 2249, 9206, -2879, -5575, -1353, 5479, -1967, 4731, -7084, 9035, -1158, -1783, -8441, -2207, -3431, -9354, 42, 9965, -6791, -8765, -1789, 8982, -8433, -4491, -6728, 2746, 244, 3206, -5257, -2090, 8904, -4237, -9836, 8593, -729, -7960, 6133, -9942, -6677, -8569, -2498, -6625, 3879, 4500, 1718, -119, 4952, 7664, 1686, 6114, 7410, -7557, 869, -3, -2985, 360, 839, -4745, 5652, 3941, -7128, 5365, 5426, 6371, -9869, -8429, -110, 390, 5625, -5331, -1612, 6454, -6421, 1865, -5270, -2506, -998, 95, 1309, -3396, 400, 8425, 4671, -1005, 9776, -2823, 2719, 3035, -4523, -3919, -2506, -3847, 608, 6215, -4022, -3994, 2943, 9945, -7640, -3148, 9244, 8133, 5978, -5846, -6318, -2949, -5757, -7814, -9539, -1048, -412, 7462, -3547, 4385, 3164, 8731, -4452, 810, 5083, -2712, -776, 6821, -8920, -1506, -7380, 4365, -5791, 9969, 9924, 479, 3632, 1994, -6074, 7889, -6641, 4886, 8458, 3649, 4333, 6206, -4481, -866, -427, 2937, -831, 1087, -9620, 9044, 5817, -7566, -1994, 2930, 3967, -7111, 1575, 3268, 4149, 194, -607, 7850, 9213, -6973, 2418, -953, -438, 4424, 3176, -4533, -2185, 5033, 4486, -7256, -6164, 7137, 1589, -3947, 7145, -4719, 7934, 3230, 4114, -8147, 3061, -3114, 5173, -8540, 1770, 1117, -4686, 3444, -4705, 7413, 7028, -2337, 3418, 7893, -6949, -2625, -8107, -8460, 2214, -5087, 8497, 5712, 7386, 2871, 9607, 2453, -636, -7636, 54, -191, 9200, -2119, 2108, -2332, 9458, 2416, 7062, -2372, 5043, -6059, 6951, -3792, 2743, -7055, -3703, -4107, -5938, -5711, 5121, 5791, 982, 363, 8163, -9315, -8984, 9240, 5286, 1016, 575, -8482, -2540, 3784, -3518, 4265, 5962, -6432, -8351, -5744, 7156, 2705, 5624, 9317, -3210, -1368, -4102, -2940, 8730, -6330, -542, -7316, 6793, 7098, -2358, -3563, 7621, -3618, 9870, -396, 7302, -8367, -3396, 4543, 2723, 572, -8546, -7276, 6314, 3721, 6746, 8631, -7455, -6713, 4812, -2883, 9550, -6646, 2249, 6724, -4679, -8107, -3763, -167, 6226, -6478, 7453, -6680, 3772, -9261, -5277, -4853, 4871, 1302, 5154, -6123, -8264, -6327, -692, 639, 438, 8341, -1207, -7448, -6419, 2729, 7065, 8411, 6035, -6301, -7259, -2129, 3537, -9688, -8199, -6906, -3488, 8136, -9023, 8958, -3968, -3125, 5539, -2941, -1096, -9445, -3651, -5544, -4742, 7809, -1318, -2118, -6259, -2453, 4312, 9689, 4401, -4867, -672, 2027, 9593, 3747, -1327, 4919, 1507, 6711, 7895, -8854, 7350, -8311, 8236, 7007, -8652, -3481, 1474, -2682, 8639, -6664, -5178, -3845, 9794, 7701, 5729, 6515, 3490, -6327, -4031, 5500, 6607, -2376, 4825, 3722, -5508, -2186, -2110, -6140, -2254, 2550, 1609, 5711, 8935, -73, 1129, -7381, 18, 4215, 3333, 9681, 4863, -7979, -26, -3177, 4612, 3984, -3360, -5919, -8276, 3757, -6077, -4281, 7165, -6626, -513, -9453, -39, 3499, -5642, 6212, 7891, 3813, -2514, 1276, 1296, 3982, -9660, -5921, -2178, 7246, 7095, 5236, -8139, 6733, 9500, 3172, 8098, -5373, 6180, -9077, -7757, 6398, 3988, 7851, -5116, -90, 3463, -3519, 274, 394, 1999, 8036, 1362, 18, 1584, -3649, 5778, -2263, 6213, -760, -5569, -414, 6735, -1451, -7285, -9342, 3769, -6853, 7072, -7515, 4182, -4864, -4940, -6673, -1094, -1600, 2045, 9314, -2388, 2527, 5933, 4436, 5637, -3446, 819, -5361, -2439, 3779, -8743, -7197, -2659, 6, -1515, -5156, -4761, -7586, 187, 5172, -5309, -7916, 5914, 1635, 6707, -3834, 9464, 2056, -8804, -657, -3796, -4595, 5382, 338, -1084, -7976, -7276, 2072, -7835, -3715, 5555, 4639, -2525, -9591, 4092, 3566, 853, 9359, -5363, 2139, 6084, 5617, 2914, 533, -5560, -7951, 6032, 8284, 5746, 2896, 3478, -5837, -531, 8290, -9371, -1887, 9474, 3856, -7837, 6096, -6305, 158, -1108, -7038, -9953, 2527, 2179, 5284, 2097, -6485, -7364, -8826, -2230, 750, 5522, 4067, -1135, 6105, -9094, -1586, 1955, -2065, -7706, 1542, -2491, -8265, -5911, -5797, -2546, 1396, 9889, -2434, 367, 5035, -9411, 9027, -5028, 7309, -9648, 1511, -8708, -5463, 42, -9217, 7933, 7771, -4912, -9246, -213, 1432, -2137, -8676, -8383, -460, 9023, 2634, 9621, -1216, -9392, -725, 1713, -3345, -9328, -2297, -3742, -3327, 8867, 1393, 942, -6428, -5430, 356, 3334, -4882, 1355, 2042, -4380, -8642, 6942, -9237, 5176, -7271, 4632, -9547, -6501, -5065, 5667, -5164, 7610, -3308, 77, 5530, -214, 5387, -6313, -5869, -3424, -6316, 3598, -5358, -8841, 7552, -8323, -9346, -4147, 8186, -1753, -9967, -6244, 3737, 9687, -366, -1900, -4517, 3166, -7416, -7243, -9543, 9119, -5789, 1765, 615, -7965, 7157, 2962, -3341, -2775, 5273, 8286, 930, -3764, 6750, 7061, 9177, -3490, -9097, 9184, -5610, -2462, -3093, -6576, 6200, -7999, 6917, -1004, -848, 4329, 8722, 6787, -7888, 2758, -8934, 7680, -3528, 7720, 5670, 8923, -7608, 3668, -1984, -3167, -3642, 4472, 4400, 609, 8893, 5081, 220, -165, -8735, 1233, -4742, 991, -3287, -5559, -6398, -6892, 7102, -4574, 7040, 8943, 3989, 6980, -4314, -5382, 7335, 9687, -1501, 5587, -1676, 176, -2534, 6846, 2954, 2642, 7057, 7590, 7271, -6031, -1769, -3167, 3955, -6204, -8586, 8759, -4074, 4755, 1595, 4880, -5200, 3502, -2518, -9122, -722, -839, -8453, -778, 4403, 6131, 8973, -3074, 503, 7886, -9102, 2531, -3041, -6527, 4749, -2718, -3160, -5144, 1564, -7541, 5090, 697, -5569, 2698, 2449, 7188, -9239, 3672, 9728, -6652, -4976, -6628, -8653, 146, 8245, 7149, -4162, -4712, 89, 5752, -2520, -2580, 7911, 7673, -6293, 6667, 7869, 7696, 2645, -615, -981, -5479, -4682, 8999, 4182, 745, -764, 2199, 6970, -9630, 9863, -4263, -3507, 4150, -8754, -7064, 3515, 2557, -3449, 7716, -2946, -841, -2828, 3836, -1070, -8919, -7880, -4290, -5429, 2972, -3556, 7008, 6300, 5221, 2960, -2256, -9863, -9106, -5138, 785, -7392, -7407, -6032, -4189, -1963, 1291, 92, 8296, 2490, 8335, 9049, 5917, -3105, 3062, 5319, 230, -9134, 8915, 4200, -4914, 2349, -2469, -9980, 7354, 9381, 5253, -399, -6192, -2459, -1997, 8036, 5744, -9800, 7432, -7308, -9815, -1187, 1220, 6545, 7476, -5436, 4271, 3490, 4283, 3254, 4488, -297, -1204, -1079, -5314, -3651, 2753, 6766, -4926, 3583, -3991, -7733, 8742, -7004, -8832, 5815, 4534, -9298, -4724, 812, -4951, 3323, -1807, -3431, -6564, 6208, -3439, -1228, 5349, -7321, 244, 2995, -957, 9270, -6988, 4830, 8465, 9408, 6800, 3986, 593, 2628, -3026, -768, 8948, 9741, 3089, 4815, -5732, 3119, -8962, 542, -783, -739, -9674, 2399, -1005, 59255 });
            maxSubsetSum(new int[] { 7 ,-9, -7, 8, 4, -7, 8, 7, 8 });
            int allOnes = ~0;

            int left = allOnes << (allOnes - 3);


            singleNumber(new int[] {8,14,8});
            paintHouses(new int[][] { new int[] { 1, 3, 4 }, new int[] { 2, 3, 3 }, new int[] { 3, 1, 4 } });
            regularExpressionMatching("caab", "d*c*x*a*b");
            LargestRectangleArea(new int[]{2, 1, 5, 6, 2, 3});
            longestPath("dir\f    file.txt");
            string message = "ABC";
            string str2 = message.Substring(0, message.Length - 1);
            string str1 = message.Substring(message.Length - 1, 1);            
            mapDecoding("2871221111122261");
            ways(10, new int[] { 2, 5, 3, 6 });
            int n = 4 & 1;
         ;

            long[] fib = new long[3];
            fib[0] = 0;
            fib[1] = 1;
            for (int k = 0;k < 3; k++)
            {
                fib[2] = fib[0] + fib[1];
                fib[0] = fib[1];
                fib[1] = fib[2];
            }

            mapDecoding("2871221111122261");
            double fl = 2.0 / 16.0;
            sumSubsets(new int[] { 1, 1, 2, 2, 2, 5, 5, 6, 8, 8 }, 9);
            nQueens(6);
            List<int> path = new List<int>();           
            climbingStaircase(4, 2);
            List<List<int>> paths = new List<List<int>>();
            
            paths.Add(new List<int>() { 1, 1 });
            paths.Add(new List<int>() { 1, 2 });
            List<int>[] arr = paths.ToArray();
            CurrencyArbitage.currencyArbitrage1(new double[][] { new double[] { 1, 0.5, 0.2 }, new double[] { 1.8, 1, 0.5 } , new double[] { 3.95, 1.2, 1 } });
            double ij = -Math.Log(1.8);

            minimumSwaps(new int[] { 4, 3, 1, 2 });
            minSubstringWithAllChars("tvdsxcqsnoeccaurocnk", "acqt");
            productExceptSelf(new int[] { 27, 37, 47, 30, 17, 6, 20, 17, 21, 43, 5, 49, 49, 50, 20, 42, 45, 1, 22, 44 }, 40);
            findLongestSubarrayBySum(0, new int[] {1,0,2 });
            Paths.GetLongestPath("user\f\tpictures\f\t\tphoto.png\f\t\tcamera\f\tdocuments\f\t\tlectures\f\t\t\tnotes.txt");
            //   FlightPaths.flightPlan(new string[][] { new string[]{"Chicago", "Denver","03:00", "06:00"},
            //new string[] {"Chicago", "Denver","03:30", "07:00"},
            //new string[] {"Chicago", "Los Angeles", "01:00", "05:00"},
            //new string[] {"Denver", "Austin", "06:30", "08:30"},
            //new string[] {"Denver", "Austin", "07:30", "09:30"},
            //new string[] {"Austin", "Denver", "06:30", "08:30"},
            //new string[] {"Los Angeles", "Phoenix", "06:00", "07:00"},
            //new string[] {"Los Angeles", "Phoenix", "05:30", "06:50"},
            //new string[] {"Phoenix", "Austin", "08:00", "08:40"}}, "Chicago", "Austin");
            FlightPaths.flightPlan(new string[][] { new string[]{"Lubbock","Norfolk","01:34","04:00"},
         new string[] {"Norfolk","Toledo","04:59","07:58"},
         new string[] {"Toledo","Modesto","09:00","12:00"},
         new string[] {"Orlando","Laredo","05:04","12:56"}}, "Lubbock", "Modesto");
            NetworkFailures.singlePointOfFailure(new int[7][] { new int[] { 0, 1, 1, 0, 0, 0, 0 },new int[] { 1, 0, 1, 0, 0, 0, 0 },
            new int[] { 1,1,0,0,0,0,1 },new int[] { 0,0,0,0,1,1,1 },
            new int[] { 0,0,0,1,0,1,0 },new int[] { 0,0,0,1,1,0,0 }, new int[]{0,0,1,1,0,0,0} });
            Connections.hasDeadlock(new int[][] { new int[] { 1 }, new int[] { 2 }, new int[] { 0 } });
            //IndexedMinHeap<int> mheap = new IndexedMinHeap<int>();
            //mheap.Insert(9);
            //mheap.Insert(4);
            //mheap.Insert(1);
            //mheap.Insert(5);
            //mheap.Insert(6);
            //mheap.Insert(3);
            //mheap.Insert(7);
            //mheap.Insert(8);
            //mheap.Insert(2);
            //int j = mheap.DeleteMin();
            ////mheap.DecreaseKey(9, 0);

            NetworkWires nw = new NetworkWires();
            int length = nw.GetMinLenWire(4, new int[][] { new int[] { 0,1,5 },
            new int[] {0,2,3},new int[]{0,3,1 }, new int[] {2,3,2}, new int[] {2,1,3}, new int[] {1,3,4}});
            Console.WriteLine(length);

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
