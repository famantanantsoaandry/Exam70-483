using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace   Exam70_483.Exercices.Parallel
{
    public class Course
    {
        private static string theEBook = string.Empty;
        private static CancellationTokenSource cancelToken = new CancellationTokenSource();
        public static void Launch()
        {
            // Example1();
            Example2();
        }

        private static void Example1()
        {
            GetBook();
        }

        private static void Example2()
        {
            do
            {
                Console.WriteLine("Start any key to start processing");
                Console.ReadKey();

                Console.WriteLine("Processing");

                Task.Factory.StartNew(() => ProcessIntData());

                Console.WriteLine("Enter Q to quit : ");
                string answer = Console.ReadLine();

                //Does user want to quit 

                if (answer.Equals("Q", StringComparison.OrdinalIgnoreCase))
                {

                    cancelToken.Cancel();
                    break;
                }

            } while (true);

            Console.ReadLine();
        }

        #region Private Methods

        static void ProcessIntData()
        {
            // Get a very large array of integers.
            int[] source = Enumerable.Range(1, 100_000_000).ToArray();
            // Find the numbers where num % 3 == 0 is true , returned
            //in descending order.

            int[] modThreeIsZero = null;

            try
            {
                modThreeIsZero = (from num in source.AsParallel().WithCancellation(cancelToken.Token)
                                  where num % 3 == 0
                                  orderby num descending
                                  select num).ToArray();

                Console.WriteLine();

                Console.WriteLine($"Found {modThreeIsZero.Count()} numbers that match query !");


            } catch (OperationCanceledException ex)
            {

                Console.WriteLine(ex.Message);
            }

            

        }

        private static void GetBook()
        {
            IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
            defaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            WebClient wc = new WebClient
            {
                Proxy = defaultWebProxy
            };

            wc.DownloadStringCompleted += (s, eArgs) =>
            {
                theEBook = eArgs.Result;
                Console.WriteLine("Download complete .");
                GetStats();
            };


            wc.DownloadStringAsync(new Uri("http://www.gutenberg.org/files/98/98-8.txt"));

           Console.ReadLine();
        }

        static void GetStats()
        {

            // Get words from e-book.

            string[] words = theEBook.Split(new char[]
            { ' ','\u000A',',','.',';',':','-','?','/'},
            StringSplitOptions.RemoveEmptyEntries);

            // Now , find the ten most common words .
            string[] tenMostCommon = null;
            string longestWord = string.Empty; 

            System.Threading.Tasks.Parallel.Invoke(
                () =>
                {
                    tenMostCommon = FindTenMostCommon(words);
                },

                () =>
                {
                    longestWord = FindLongestWord(words);
                }
                );

            //Get longest word.

            

            //Now that all tasks are complete , build a string to show all stats.

            StringBuilder bookStats = new StringBuilder("Ten Most Common Words are : \n");

            foreach (string s in tenMostCommon)
            {
                bookStats.AppendLine(s);
            }

            bookStats.AppendFormat("Longest word is : {0}", longestWord);
            bookStats.AppendLine();

            Console.WriteLine(bookStats.ToString(), "Book info");

        }

        private static string[] FindTenMostCommon(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            string[] commonWords = frequencyOrder.Take(10).ToArray();

            return commonWords;
        }

        private static string FindLongestWord(string[] words)
        {
            return (from w in words orderby w.Length descending select w).FirstOrDefault();
        }

        #endregion
    }
}
