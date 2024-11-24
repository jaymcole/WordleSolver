using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
{
    public class WordleWord
    {
        public string word;
        public decimal score;
        public int year;
        public int matchCount;
        public int volumeCount;

        //disqualifiers
        public bool usedInPreviousPuzzle;


        public WordleWord() 
        {
            score = -1;
            usedInPreviousPuzzle = false;
        }

        private static string ELEMENT_DELIMITER = "[̲̅$̲̅(̲̅5̲̅)̲̅$̲̅]";
        private static string KEY_VALUE_DELIMITER = "( ͡° ͜ʖ ͡°)";
        private static string WORD_KEY = "word_lowercase";
        private static string SCORE_KEY = "score";
        private static string PREVIOUSLY_USED_KEY = "usedInPreviousPuzzle";
        private static string MATCH_COUNT = "match_count";
        private static string YEAR = "year";
        private static string VOLUME_COUNT = "volume_count";
        public void Deserialize(string fileLine)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] elements = fileLine.Split(ELEMENT_DELIMITER);
            foreach(string element in elements)
            {
                string[] parts = element.Split(KEY_VALUE_DELIMITER);
                dictionary.Add(parts[0], parts[1]);
            }

            if (dictionary.ContainsKey(WORD_KEY))
            {
                this.word = dictionary[WORD_KEY];
            }

            if (dictionary.ContainsKey(SCORE_KEY))
            {
                this.score = decimal.Parse(dictionary[SCORE_KEY]);
            }

            if (dictionary.ContainsKey(PREVIOUSLY_USED_KEY))
            {
                this.usedInPreviousPuzzle = bool.Parse(dictionary[PREVIOUSLY_USED_KEY]);
            }

            if (dictionary.ContainsKey(MATCH_COUNT))
            {
                this.matchCount = int.Parse(dictionary[MATCH_COUNT]);
            }

            if (dictionary.ContainsKey(YEAR))
            {
                this.year = int.Parse(dictionary[YEAR]);
            }

            if (dictionary.ContainsKey(VOLUME_COUNT))
            {
                this.volumeCount = int.Parse(dictionary[VOLUME_COUNT]);
            }
        }

        public string Serialize()
        {
            List<string> serializedParts =
            [
                WORD_KEY + KEY_VALUE_DELIMITER + word,
                SCORE_KEY + KEY_VALUE_DELIMITER + score,
                PREVIOUSLY_USED_KEY + KEY_VALUE_DELIMITER + usedInPreviousPuzzle.ToString(),
                YEAR + KEY_VALUE_DELIMITER + year,
                MATCH_COUNT + KEY_VALUE_DELIMITER + matchCount,
                VOLUME_COUNT + KEY_VALUE_DELIMITER + volumeCount,
            ];
            return string.Join(ELEMENT_DELIMITER, serializedParts.ToArray());
        } 

    }
}
