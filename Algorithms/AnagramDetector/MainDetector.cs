/*
 *  Author:     Wesley Lancaster
 *  Date:       1/18/2019
 *  Program:    Anagram Detector
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AnagramDetector
{
    class MainDetector
    {
        static string[] words;

        /// <summary>
        /// Handles options regarding the AnagramDetector
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            int selection = 0;
            do
            {
                Console.WriteLine("//*** Anagram Detector ***//Make a selection:\n" +
                    "\t1: Run IO version of Anagram Detector\n" +
                    "\t2: Run time complexity analysis for Anagram Detector\n" +
                    "\t0: Quit" +
                    "Your Selection: ");
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    selection = input;
                    switch (selection)
                    {
                        case 0:
                            break;
                        case 1:
                            IOAnagramDetector();
                            break;
                        case 2:
                            Console.WriteLine("How many words? (Must be greater than 1024)");
                            int wordCount = int.Parse(Console.ReadLine());
                            Console.WriteLine("How many letters in each word?");
                            int wordLen = int.Parse(Console.ReadLine());
                            AnalyzeAnagramDetector(wordCount, wordLen);
                            break;
                        default:
                            continue;
                    }
                }
            }
            while (selection != 0);
        }

        /// <summary>
        /// Uses the console window to count the number of "unique words" from a list, or words not 
        /// part of an anagram from other words in the list. 
        /// 
        /// First, 2 ints must be submitted: the number of words, and the length that all the words 
        /// share. Then the list of words must be submitted. The program will then print how many of
        /// the words were unique.
        /// </summary>
        /// <param name="args"></param>
        public static void IOAnagramDetector()
        {
            int wordCount, length;
            int uniqueWords = 0;

            //  Create a dictionary: word -> unique?
            Dictionary<string, bool> words = new Dictionary<string, bool>();

            //  Get parameters
            string[] parameters = Console.ReadLine().Split();

            //  Convert both params to int
            wordCount = int.Parse(parameters[0]);
            length = int.Parse(parameters[1]);

            //  Receive words, but don't store duplicates
            for (int i = 0; i < wordCount; i++)
            {
                //  Alphabetize the word ("asdf" => "adfs") and make sure it's lowercase
                string word = String.Concat(Console.ReadLine().OrderBy(s => s)).ToLower();
                //  If the dictionary already has the word, it's not unique.
                if (words.ContainsKey(word))
                {
                    //  If the word is marked as unique, undo it
                    if (words[word])
                    {
                        words[word] = false;
                        uniqueWords--;
                    }
                }
                //  New word: add the word and assume it's unique for now
                else
                {
                    words.Add(word, true);
                    uniqueWords++;
                }
            }

            //  Report and close
            Console.WriteLine(uniqueWords + "\n");
        }

        /// <summary>
        /// Counts the number of "unique words" in the array of words, or words not part of an 
        /// anagram from other words in the list.
        /// </summary>
        /// <param name="count">The number of words to evaluate</param>
        /// <returns>The number of unique words in the list</returns>
        public static int AnagramDetector(int count)
        {
            int uniqueWords = 0;

            //  Create a dictionary: word -> unique?
            Dictionary<string, bool> wordDictionary = new Dictionary<string, bool>();

            //  Read and store words, but don't store duplicates
            for (int i = 0; i < count; i++)
            {
                //  Alphabetize the word ("asdf" => "adfs") and make sure it's lowercase
                string word = String.Concat(words[i].OrderBy(s => s)).ToLower();

                //  If the dictionary already has the word, it's not unique.
                if (wordDictionary.ContainsKey(word))
                {
                    //  Decrement uniqueWords if the word is marked from true to false
                    if (wordDictionary[word])
                    {
                        wordDictionary[word] = false;
                        uniqueWords--;
                    }
                }
                //  New word: add the word and assume it's unique for now
                else
                {
                    wordDictionary.Add(word, true);
                    uniqueWords++;
                }
            }
            return uniqueWords;
        }


        public static void AnalyzeAnagramDetector(int count, int length)
        {
            //  Create random words
            words = new string[count];
            SetRandomWords(length);

            //  Analyze workload by incrementing workload and reporting time. 
            int workload = count / 512;
            double[] times = new double[10];
            Stopwatch sw = new Stopwatch();

            //  Get average with each workload
            const int averageAmt = 20;
            double[] singleTests = new double[averageAmt];
            int uniques = 0;
            Console.WriteLine("Workload\tTime\t\tRatio"); // Print head of table

            //  i: Increment workload
            for (int i = 0; i < 10; i++)
            {
                sw.Restart();
                //  j: Test each workload by the specified amount of times
                for (int j = 0; j < averageAmt; j++)
                    uniques = AnagramDetector(workload * (int)Math.Pow(2, i));
                sw.Stop();

                //  Record average
                times[i] = sw.Elapsed.TotalMilliseconds / averageAmt;

                //  Print result
                Console.Write(workload * (int)Math.Pow(2, i) + "\t\t" +
                    Math.Round(times[i], 3) + "\t\t");
                //  Print ratio when able (current time / last time)
                if (i > 0)
                    Console.WriteLine(Math.Round(times[i] / times[i - 1], 3));
                else Console.WriteLine();
            }

            //  Now analyze with 2000 entries but a growing word size
            GrowingWordSize();

            Console.WriteLine("By the way, out of " + workload * (int)Math.Pow(2, 10) + " results, there were " +
                uniques + " unique words!");


        }

        public static void GrowingWordSize()
        {
            int count = 2000;
            int length = 5;
            double[] times = new double[10];
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("WordSize\tTime\t\tRatio"); // Print head of table

            //  Increment length
            for (int i = 0; i < 10; i++)
            {
                //  Create random words
                words = new string[count];
                SetRandomWords(length * (int)Math.Pow(2, i));

                sw.Restart();
                //  Take 10 samples and store average
                for (int j = 0; j < 10; j++)
                {
                    AnagramDetector(count);
                }
                sw.Stop();
                times[i] = sw.Elapsed.TotalMilliseconds / 10;

                //  Print result
                Console.Write(length * (int)Math.Pow(2, i) + "\t\t" +
                    Math.Round(times[i], 3) + "\t\t");
                //  Print ratio when able (current time / last time)
                if (i > 0)
                    Console.WriteLine(Math.Round(times[i] / times[i - 1], 3));
                else Console.WriteLine();
            }
        }

        /// <summary>
        /// Populates the char[] called words, a property of this class, with words filled with
        /// random letters.
        /// </summary>
        /// <param name="length"></param>
        public static void SetRandomWords(int length)
        {
            Random rand = new Random();
            char[] letters = new char[length];
            for (int word = 0; word < words.Length; word++)
            {
                for (int letter = 0; letter < length; letter++)
                    letters[letter] = (char)('a' + rand.Next(26));
                words[word] = new string(letters);
                //  DEBUG
                //Console.WriteLine(words[word]);
            }
        }
    }
}
