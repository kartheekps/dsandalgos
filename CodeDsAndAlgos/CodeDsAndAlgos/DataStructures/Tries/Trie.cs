using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDsAndAlgos.DataStructures.Tries
{
    public class Trie
    {
        private readonly TrieNode root;

        private class TrieNode
        {
            public bool isEndofWord;
            public Dictionary<char, TrieNode> Children;
            public TrieNode()
            {
                isEndofWord = false;
                Children = new Dictionary<char, TrieNode>();
            }
        }

        public Trie()
        {
            root = new TrieNode();
        }

        public int LongestKeyPrefixOf(string key)
        {
            TrieNode current = root;
            int i = 0;
            int lkey = 0;
            while (i < key.Length && current != null)
            {
                if (!current.Children.ContainsKey(key[i]))
                    break;
                else
                {
                    if (current.isEndofWord) lkey = i;
                    current = current.Children[key[i]];
                }
                
                i++;
            }
            return current.isEndofWord ? i : lkey;
        }

        public void Insert(string key)
        {
            Insert(root, key, 0);
        }

        private TrieNode Insert(TrieNode root, string key, int i)
        {
            if (root == null) root = new TrieNode();
            if (i == key.Length) { root.isEndofWord = true; return root; }
            char c = key[i];

            if (root.Children.ContainsKey(c))
                root.Children[c] = Insert(root.Children[c], key, i + 1);
            else
                root.Children[c] = Insert(null, key, i + 1);
            return root;
        }
    }
}
