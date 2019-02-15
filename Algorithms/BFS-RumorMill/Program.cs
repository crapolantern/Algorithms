using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFS_RumorMill
{
    class Program
    {
        static Student[] students;
        static Dictionary<string, int> index = new Dictionary<string, int>();//name -> obj
        /// <summary>
        /// Handles the IO portion of the algorithm, BFS actually does the computation
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            //  Read number of students at school
            int num_students = int.Parse(Console.ReadLine());
            students = new Student[num_students];

            //  Read each student name
            for (int i = 0; i < num_students; i++)
            {
                students[i] = new Student(Console.ReadLine());
                index.Add(students[i].name, i);
            }

            //  Read number of friend pairs
            int num_friends = int.Parse(Console.ReadLine());

            //  Read each friend pair: "name1 name2"
            string[] input = new string[2];
            for (int i = 0; i < num_friends; i++)
            {
                input = Console.ReadLine().Split();
                //  The student that has the index matching the name adds the other friend
                students[index[input[0]]].friends.Add(index[input[1]]);
                students[index[input[1]]].friends.Add(index[input[0]]);
            }

            //  Read number of rumor reports requested
            int num_reports = int.Parse(Console.ReadLine());

            //  Read the students that started rumors for each report
            int[] reports = new int[num_reports];
            for (int i = 0; i < num_reports; i++)
            {
                reports[i] = index[Console.ReadLine()];
            }

            //  Print each report
            for (int i = 0; i < num_reports; i++)
            {
                foreach (Student s in BFS(reports[i]))  //  Each report
                    Console.Write(s.name + " ");        //  prints each name
                Console.WriteLine();                    //  then endl
            }

            Console.ReadLine();
        }

        /// <summary>
        /// For "Rumor Mill" problem, find out what order students discover school rumors shared by
        /// a map (or graph) of friends
        /// </summary>
        /// <returns></returns>
        static IEnumerable<Student> BFS (int start)
        {
            
        }
    }

    class Student
    {
        public string name { get; }
        public List<int> friends { get; set; }

        public Student(string name)
        {
            this.name = name;
            friends = new List<int>();
        }
    }
}
