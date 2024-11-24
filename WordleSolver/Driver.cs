using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace WordleSolver
{

    public class Driver
    {
        //https://storage.googleapis.com/books/ngrams/books/datasetsv3.html
        private WordDictionary dictionary;
        private TrendsService trendsService;
        private int processedWords = 0;
        private int filesProcessed = 1;
        public Driver() 
        {
            trendsService = new TrendsService();
            dictionary = new WordDictionary();
        }

        public List<WordleWord> SearchWithFilters(HashSet<char>[] allowList, Dictionary<char, int> mustUseCharacters)
        {
            List<WordleWord> results = dictionary.SearchAndRankWords("*****", allowList);
            List < WordleWord > finalResults = new List<WordleWord> ();

            foreach (WordleWord word in results)
            {
                bool validWord = true;
                foreach(char neededCharacter in mustUseCharacters.Keys)
                {
                    if (!word.word.Contains (neededCharacter))
                    {
                        validWord = false;
                    }
                }

                if (validWord) 
                {
                    finalResults.Add(word);
                }
            }

            return results.Slice(0, Math.Min(10, results.Count));
        }


        public void PrintMostFrequentWords(int n)
        {
            foreach (WordleWord word in dictionary.GetNMostFrequestWords(n))
            {
                Console.WriteLine($"{word.word}\t{word.volumeCount}\t{word.matchCount}");
            }
        }

        public void ProcessNgramData()
        {
            processedWords = 0;
            string[] files = Directory.GetFiles("C:\\Users\\jaymc\\Desktop\\ngams-data");
            List<string> filesToProcess = new List<string>();
            foreach(string file in files)
            {
                if (!file.EndsWith(".gz") && file.EndsWith("1-p"))
                {
                    Console.WriteLine(file);
                    ProcessFile(file);
                    filesProcessed++;
                }
                
            }
            dictionary.SaveExistingWordSet();
            Console.WriteLine("Done");
        }

        private void ProcessFile(string file)
        {
            using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
            {
                int nextCheckin = 0;
                while (!streamReader.EndOfStream)
                //for(int i = 0; i < 1000; i++)
                {
                    string line = streamReader.ReadLine();
                    try
                    {
                        string[] lineParts = line.Split("\t");
                        string ngram = lineParts[0];
                        if (ngram.Contains("."))
                        {
                            ngram = ngram.Split(".")[1];
                        }

                        if (ngram.Contains('_'))
                        {
                            ngram = ngram.Split(".")[0];
                        }
                        ngram = ngram.ToLower().Trim();

                        string year_string = lineParts[1];
                        string match_count = lineParts[2];
                        string volume_count = lineParts[3];
                        
                        /*
                        Console.WriteLine($"ngram:          {ngram}");
                        Console.WriteLine($"year:           {year_string}");
                        Console.WriteLine($"match_count:    {match_count}");
                        Console.WriteLine($"volume_count:   {volume_count}");
                        */

                        WordleWord word = dictionary.GetWord(ngram);
                        if (word != null)
                        {

                            int year;
                            if (int.TryParse(year_string, out year) )
                            {
                                word.year = year;
                            }

                            int volumeCount;
                            if (int.TryParse(volume_count, out volumeCount))
                            {
                                word.volumeCount = volumeCount;
                            }

                            int matchCount;
                            if (int.TryParse(volume_count, out matchCount))
                            {
                                word.matchCount = matchCount;
                            }
                            nextCheckin++;
                            processedWords++;
                        }

                        if (nextCheckin > 0 && nextCheckin % 123 == 0)
                        {
                            nextCheckin = 0;
                            Console.WriteLine($"Processed {processedWords} words from {filesProcessed} files");
                        }
                    } catch (Exception e) {
                        Console.Error.WriteLine($"Fuck all. This line borked: \"{line}\"");
                    }
                }
                
            }
        }

        public async Task GetWordScores()
        {
            List<WordleWord> wordsToRefresh = dictionary.GetWordList();
            int count = 0;
            Console.WriteLine($"Gathering scores for {wordsToRefresh.Count} words");
            foreach (WordleWord word in wordsToRefresh)
            {

                Console.WriteLine($"Getting score for {word.word}");
                if (word.score == -1)
                {
                    decimal score = await trendsService.GetWordTrendScore(word.word);
                    word.score = score;
                    Console.WriteLine($"{word.word} -> {word.score}");
                    dictionary.SaveExistingWordSet();
                    count++;

                    if (count > 2)
                    {
                        Thread.Sleep(1000);
                        count = 0;
                    }

                } else
                {
                    Console.WriteLine($"Skipping \"{word.word}\" as it already has a score");
                }
                
            }
        }

       public void LoadWordsFromList()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "resources", "possible_words.txt");
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    dictionary.InsertNewWord(line);
                }
            }
            dictionary.SaveExistingWordSet();
        }

    }
}
