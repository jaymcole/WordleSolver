using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WordleSolver
{
    public class TrendsService
    {

        private HttpClient client;

        public TrendsService() {
            client = new HttpClient();
        }

        
        public async Task<decimal> GetWordTrendScore(string word)
        {
            Console.WriteLine("Starting");

            int startYear = 2021;
            int endYear = 2022;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://books.google.com/ngrams/graph?content={word}&year_start={startYear}&year_end={endYear}&corpus=en&smoothing=3"),
                Content = new StringContent("", Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return ExtractScore(responseBody, word);
        }


        private decimal ExtractScore(string messageContent, string word)
        {
            string start = "<script id=\"ngrams-data\" type=\"application/json\">[{\"ngram\": \"" + word + "\", \"parent\": \"\", \"type\": \"NGRAM\", \"timeseries\": [";
            string end = "]}]</script>";
            string score = messageContent.Substring(messageContent.IndexOf(start) + start.Length);
            score = score.Substring(0, score.IndexOf(","));

            decimal decimalScore;
            if (Decimal.TryParse(score, out decimalScore))
            {
                return decimalScore;
            } else
            {
                return 0;
            }

            //return Decimal.Parse(score, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint);
        }

    }
}
