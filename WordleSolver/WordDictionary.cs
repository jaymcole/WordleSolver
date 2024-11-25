using System.IO;
using System.Linq;
using System.Text;

namespace WordleSolver
{
    public class WordDictionary
    {
        private string EXISTING_WORDS_FILE_NAME = "wordset.wordle";
        public static string RESOURCE_FOLDER = "resources";

        private WordleTreeNode root;
        private Dictionary<string, WordleWord> Words;
        private string workingDirectory;

        public WordDictionary() {
            string dictionaryFilePath = Path.Combine(Environment.CurrentDirectory, RESOURCE_FOLDER, EXISTING_WORDS_FILE_NAME);
            if (!File.Exists(dictionaryFilePath))
            {
                File.Create(dictionaryFilePath);
            } 

            root = new WordleTreeNode(' ');
            Words = new Dictionary<string, WordleWord>();
            LoadExistingWordSet();
        }

        public void InsertNewWord(string newWord)
        {
            if (!Words.ContainsKey(newWord))
            {
                root.AddChild(newWord, 0);
                WordleWord word = new WordleWord();
                word.word = newWord;
                Words.Add(newWord, word);
            }
        }

        public void InsertNewWord(WordleWord newWord)
        {
            if (!Words.ContainsKey(newWord.word))
            {
                root.AddChild(newWord.word, 0);
                Words.Add(newWord.word, newWord);
            }
        }

        public WordleWord? GetWord(string word)
        {
            if (Words.ContainsKey(word))
            {
                return Words[word];
            }
            return null;
        }


        public List<WordleWord> GetNMostFrequestWords(int n)
        {
            List< WordleWord> mostFrequent = Words.Values.OrderByDescending(x => x.volumeCount).ToList();
            return mostFrequent.GetRange(0, Math.Min(mostFrequent.Count, n));
        }

        public List<WordleWord> GetWordList()
        {
            return Words.Values.ToList();
        }

        public List<WordleWord> SearchAndRankWords(string searchQuery, HashSet<char>[] allowList)
        {
            return SearchWords(searchQuery, allowList).OrderByDescending(x => x.volumeCount).ToList();
        }

        public List<WordleWord> SearchWords(string searchQuery, HashSet<char>[] allowList)
        {
            for(int i = 0; i < allowList.Length; i++)
            {
                Console.WriteLine($"{i}: {string.Join(" ", allowList[i].OrderDescending().ToList())}");
            }


            List<string> results = new List<string>();
            root.FindWords(results, searchQuery, allowList, 0, "");

            List<WordleWord> wordResults = new List<WordleWord>();
            foreach(string word in results) 
            {
                wordResults.Add(GetWord(word.Trim()));
            }
            Console.WriteLine($"Found {results.Count} matching words");
            return wordResults;
        }

        public void LoadExistingWordSet()
        {
            int foundWords = 0;
            string dictionaryFilePath = Path.Combine(Environment.CurrentDirectory, RESOURCE_FOLDER, EXISTING_WORDS_FILE_NAME);
            using (StreamReader streamReader = new StreamReader(dictionaryFilePath, Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    WordleWord word = new WordleWord();
                    word.Deserialize(line);
                    foundWords++;
                    InsertNewWord(word);
                }
                Console.WriteLine($"Loaded {foundWords} words from file");
            }
        }

        public void SaveExistingWordSet()
        {
            string dictionaryFilePath = Path.Combine(Environment.CurrentDirectory, RESOURCE_FOLDER, EXISTING_WORDS_FILE_NAME);
            List<string> serializedWords = new List<string>();
            foreach (WordleWord word in Words.Values) 
            {
                serializedWords.Add(word.Serialize());            
            }
            string serializedDictionary = string.Join(Environment.NewLine, serializedWords.ToArray());
            File.WriteAllText(dictionaryFilePath, serializedDictionary);

        }

    }
}
