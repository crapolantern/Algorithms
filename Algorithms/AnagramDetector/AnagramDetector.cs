/*
 *  Author:     Wesley Lancaster
 *  Date:       1/18/2019
 *  Program:    Anagram Detector
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramDetector
{
    class AnagramDetector
    {
        /// <summary>
        /// Counts the number of "unique words" from a list, or words not part of an anagram from
        /// other words in the list. First, 2 ints must be submitted: the number of words, and the
        /// length that all the words share. Then the list of words must be submitted. The program
        /// will then print how many of the words were unique.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            int wordCount, length;
            int uniqueWords = 0;
            List<string> words = new List<string>();

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
                //  If the dictionary already has the word, it's not unique
                if (words.Contains(word))
                    uniqueWords--;

                //  If it doesn't, add it
                else
                {
                    words.Add(word);
                    uniqueWords++;
                }
            }

            //  Report and close
            Console.WriteLine(uniqueWords);
            Console.Read();
        }
    }
}
