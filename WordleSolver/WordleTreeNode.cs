using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
{
    public class WordleTreeNode
    {
        private char WILD_CARD = '*';

        public char nodeCharacter;
        public Dictionary<char, WordleTreeNode> children;
        public bool hasStop;

        public WordleTreeNode(char nodeCharacter)
        {
            this.nodeCharacter = nodeCharacter;
            children = new Dictionary<char, WordleTreeNode>();
        }

        public void FindWords(List<string> output, string search, HashSet<char>[] validChars, int index, string currentWord)
        {
            if (search.Length != validChars.Length)
            {
                throw new Exception("Bad request - search length must be the same as ignore list");
            }


            if (index >= search.Length)
            {
                if (hasStop)
                {
                    output.Add(currentWord + nodeCharacter);
                }
            } else {

                if (search[index] == WILD_CARD) 
                { 
                    foreach (char nextChar in children.Keys)
                    {
                        if (validChars[index].Contains(nextChar))
                        {
                            children[nextChar].FindWords(output, search, validChars, index + 1, currentWord + nodeCharacter);
                        }
                    }
                } else
                {
                    if (children.ContainsKey(search[index])) {
                        children[search[index]].FindWords(output, search, validChars, index + 1, currentWord + nodeCharacter);
                    }
                }

            }
        }

        public void AddChild(string word, int index) 
        {
            if (index >= word.Length)
            {
                hasStop = true;
            } else
            {
                if (!children.ContainsKey(word[index]))
                {
                    children.Add(word[index], new WordleTreeNode(word[index]));
                }
                children[word[index]].AddChild(word, index+1);
            }
        }
    }
}
