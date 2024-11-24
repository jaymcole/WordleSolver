using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
{
    public class WordleSpace
    {
        private HashSet<char> remaining;

        public WordleSpace() {
            remaining = new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray());
        }

        public void RemoveChar(char c)
        {
            remaining.Remove(c);
        }

        public void SetCorrect(char c)
        {
            remaining = [c];
        }

        public HashSet<char> GetRemaining()
        {
            return remaining;
        }

    }
}
