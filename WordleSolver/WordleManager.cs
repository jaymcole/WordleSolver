using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
{
    public class WordleManager
    {
        public static char CORRECT = 'c';
        public static char VALID = 'v';
        public static char INCORRECT = 'i';

        WordleSpace[] wordleSpaces;
        public Dictionary<char, int> mustUseCharacters;


        public WordleManager() 
        {
            mustUseCharacters = new Dictionary<char, int>();
            wordleSpaces = new WordleSpace[5];
            wordleSpaces[0] = new WordleSpace();
            wordleSpaces[1] = new WordleSpace();
            wordleSpaces[2] = new WordleSpace();
            wordleSpaces[3] = new WordleSpace();
            wordleSpaces[4] = new WordleSpace();
        }

        public string GetInstructions()
        {
            return $"correct character and space={CORRECT}, correct character wrong space={VALID}, invalid character={INCORRECT}";
        }

        public HashSet<char>[] GetAllowLists() {
            HashSet<char>[] ignoreLists = {
                wordleSpaces[0].GetRemaining(),
                wordleSpaces[1].GetRemaining(),
                wordleSpaces[2].GetRemaining(),
                wordleSpaces[3].GetRemaining(),
                wordleSpaces[4].GetRemaining(),
            };
            return ignoreLists;
        }

        public void ProcessGuess(string[] letters)
        {
            if (letters.Length != 5)
            {
                throw new Exception("Incorrect number of letters");
            }
            for (int i = 0; i < letters.Length; i++)
            {
                letters[i] = letters[i].ToLower().Trim();
                ValidateLetterPair(letters[i]);
            }

            for (int i = 0; i < letters.Length; i++)
            {
                ProcessLetterGuess(i, letters[i][0], letters[i][1]);
            }
        }

        private void ValidateLetterPair(string letterPair)
        {
            if (letterPair.Length != 2)
            {
                throw new Exception("Letter pair is too short");
            }

            if (letterPair[1] != CORRECT && letterPair[1] != VALID && letterPair[1] != INCORRECT)
            {
                Console.WriteLine($"Letter status cannot be: {letterPair[1]}");
                throw new Exception("Letter status is invalid");
            }
        }

        private void ProcessLetterGuess(int index, char letter, char status)
        {
            if (status == CORRECT)
            {
                wordleSpaces[index].SetCorrect(letter);
            } else if (status == VALID) 
            {
                wordleSpaces[index].RemoveChar(letter);
                if (!mustUseCharacters.ContainsKey(letter))
                {
                    mustUseCharacters.Add(letter, 0);
                } else
                {
                    mustUseCharacters[letter]++;
                }
            } else if (status == INCORRECT) 
            {
                foreach(WordleSpace space in wordleSpaces)
                {
                    space.RemoveChar(letter);
                }
            }
        }
    }
}
