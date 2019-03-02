using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
   public class Paths
    {
        public class Result
        {
            public int MaxLength { get; set; }
            public int Index { get; set; }            
        }
        public static int GetLongestPath(string fileSystem)
        {
            string[] filesPath = fileSystem.Split(new string[] { "\f" },StringSplitOptions.None);
            Result state = new Result() { MaxLength = Int32.MinValue, Index = 0 };
            GetLongestPathUtil(filesPath,state,string.Empty,0);
            return state.MaxLength-1;
        }
        public static void GetLongestPathUtil(string[] filesPath,Result state,string path,int level)
        {
            if (path.Contains('.'))
            {
                if (state.MaxLength < path.Length)
                    state.MaxLength = path.Length;
                return;
            }
            if (state.Index == filesPath.Length)
                return;           
            
            while(state.Index < filesPath.Length && 
                filesPath[state.Index].Length - filesPath[state.Index].Replace("\t", string.Empty).Length == level)
            {                
                string filePath = filesPath[state.Index].Replace("\t", string.Empty);
                state.Index++;
                GetLongestPathUtil(filesPath, state, path + @"/" + filePath, level + 1);                                
            }           
        }
    }
}

