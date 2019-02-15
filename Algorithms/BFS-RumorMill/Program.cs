using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFS_RumorMill
{
    public class Program
    {
        /// <summary>
        /// Handles the IO portion of the algorithm, BFS actually does the computation
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Student[] students;
            Dictionary<string, int> index = new Dictionary<string, int>();  //  name -> obj

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
            RumorMill mill = new RumorMill();
            for (int i = 0; i < num_reports; i++)
            {
                foreach (int stud in mill.BFS(students, reports[i])) //  Each report
                    Console.Write(students[stud].name + " ");       //  prints each name
                Console.WriteLine();                                //  then endl
            }

            Console.ReadLine();
        }
    }

    public class RumorMill
    {
        private Student[] students;
        public IEnumerable<int> BFS(Student[] students, int start)
        {
            int size = students.Count();

            //  Structures to track which students are placed at which distance
            List<List<int>> table = new List<List<int>>();
            int[] dist = new int[size];
            for (int i = 0; i < size; i++)
                dist[i] = -1;

            //  Initiate rumor-maker
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            dist[start] = 0;
            table.Add(new List<int>());
            table[dist[start]].Add(start);

            //  Record all connections
            int distance;
            while (q.Count > 0)
            {
                int current = q.Dequeue();
                foreach (int i in students[current].friends)
                {
                    if (dist[i] < 0)
                    {
                        q.Enqueue(i);

                        distance = dist[current] + 1;
                        dist[i] = distance;

                        //  Add new list at this index if necessary
                        if (table.Count == distance)
                            table.Add(new List<int>());
                        table[distance].Add(i);  //  Add the student in the index that equals its distance
                    }
                }
            }

            //  Find all outsiders
            List<int> outsiders = new List<int>();
            for (int i = 0; i < size; i++)
                if (dist[i] < 0)
                    outsiders.Add(i);
            table.Add(outsiders);

            foreach (List<int> dist_group in table)
            {
                dist_group.Sort();
                foreach (int i in dist_group)
                    yield return i;
            }
        }
    }

    public class Student
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
