// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using WordleSolver;


Console.WriteLine("Hello, World!");
//string dictionaryFilePath = Path.Combine(Environment.CurrentDirectory, "resources");
WordleManager manager = new WordleManager();
Driver driver = new Driver();

string COMMAND_STOP = "stop";

while (true)
{
    Console.WriteLine(manager.GetInstructions());
    string input = Console.ReadLine();

    if (input.ToLower() == COMMAND_STOP)
    {
        break;
    } else
    {
        try
        {
            manager.ProcessGuess(input.Split(" "));
            Console.WriteLine("Searching words...");
            List<string> topChoices = driver.SearchWithFilters(manager.GetAllowLists(), manager.mustUseCharacters).Select(o => o.word).ToList();
            Console.WriteLine($"Top Options: {string.Join(",", topChoices)}");
        } catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine("Invalid input");
        }
    }
}



//driver.LoadWordsFromList();
//await driver.GetWordScores();

//TrendsService trends = new TrendsService();
//decimal wordScore= trends.GetWordTrendScore("Something").Result;


/*
Console.WriteLine("Done constructing word tree");

HashSet<char>[] ignoreLists = {
//  new HashSet<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()),
    new HashSet<char>("ABCDE__HIJKLMNOPQRSTUV_XYZ".ToLower().ToCharArray()),
    new HashSet<char>("_BCDEFGHIJKLMN_PQRSTUVWXYZ".ToLower().ToCharArray()),
    new HashSet<char>("ABCDEFGHIJKL_NOPQRST_VWXYZ".ToLower().ToCharArray()),
    new HashSet<char>("ABCD_FGHIJKLM_OPQRSTUVWXYZ".ToLower().ToCharArray()),
    new HashSet<char>("ABC_EFGHIJKLMNOPQ_STUVWXYZ".ToLower().ToCharArray()),
} ;

List<string> possible = new List<string>();
root.FindWords(possible, "*****", ignoreLists, 0, "");
*/





