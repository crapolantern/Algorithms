using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAG
{
    public static class MainClass
    {
        /// <summary>
        /// Handles the IO portion of the algorithm
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //  User will give a max of 2 arguments per line
            string[] input = new string[2];

            //  City Declarations
            input[0] = Console.ReadLine();  //  Number of cities
            int num_cities = int.Parse(input[0]);
            WeightedDAG dag = new WeightedDAG(num_cities);
            for (int i = 0; i < num_cities; i++)
            {
                input = Console.ReadLine().Split();
                dag.AddCity(input[0], int.Parse(input[1]), i);
            }

            //  Highway Declarations
            input[0] = Console.ReadLine();
            int num_hwy = int.Parse(input[0]);
            for (int i = 0; i < num_hwy; i++)
            {
                input = Console.ReadLine().Split();
                dag.AddHighway(input[0], input[1]);
            }

            //  Requested Computations
            string[][] comp;   //  An array containing arrays of computation requests
            input[0] = Console.ReadLine();
            int num_comp = int.Parse(input[0]);
            comp = new string[num_comp][];

            for (int i = 0; i < num_comp; i++)
            {
                input = Console.ReadLine().Split();
                comp[i] = new string[2] { input[0], input[1] };
            }

            //  Work the computations and report back
            int fee;
            for (int i = 0; i < comp.Length; i++)
            {
                fee = dag.LowestToll(comp[i][0], comp[i][1]);
                if (fee < 0)    //  Unreachable requests are flagged with a negative number
                    Console.WriteLine("NO");
                else
                    Console.WriteLine(fee);
            }

            Console.Read();
        }
    }
    public class WeightedDAG
    {
        //  City Name -> City Index
        private Dictionary<string, int> indexes;
        //  Index = City Index, Node chain are highways leaving that city
        private City[] cities;
        //  For storing the reverse topological order
        private List<int> reverseTop = new List<int>(); //DEBUG

        public WeightedDAG(int number_of_cities)
        {
            indexes = new Dictionary<string, int>();
            cities = new City[number_of_cities];
        }

        public void AddCity(string name, int toll, int index)
        {
            indexes.Add(name, index);    //  Add index to dictionary
            cities[index] = new City(name, toll); //  Add city to array
        }
        
        public void AddHighway(int city, int to_city)
        {
            cities[city].hwy_out.Add(to_city);
            cities[to_city].hwy_in.Add(city);
        }

        public void AddHighway(string city, string to_city)
        {
            AddHighway(indexes[city], indexes[to_city]);
        }

        public int RequestComputation(string city, string to_city)
        {
            return LowestToll(indexes[city], indexes[to_city]);
        }

        public int LowestToll(string city, string to_city)
        {
            return LowestToll(indexes[city], indexes[to_city]);
        }

        /// <summary>
        /// Computes the cheapest way to navigate from one city to another, if the cities are
        /// vertexes of a directed acyclic graph.
        /// </summary>
        /// <returns></returns>
        public int LowestToll(int city, int to_city)
        {
            // If they are the same, return zero
            if (city == to_city)
                return 0;

            //  Get the minimum toll and work from end to start. Check for a -1 flag
            City current = new City("", 0);

            //  Get reverse topological order of the parameters (start from destination and work up)
            List<int> dfs = new List<int>();
            foreach (int i in DFS(city, to_city))
                dfs.Add(i);

            foreach (int i in dfs)
            {
                if (i == -1)
                    return -1;
                current = cities[i];
                //  If this is not the destination, find the cheapest path from here
                if (current != cities[to_city])
                {
                    //  Find a city that leads to the same destination
                    City between;
                    int min = -1;
                    foreach (int j in current.hwy_out)
                    {
                        if (dfs.Contains(j)) //(between start and finish in dfs)
                        {
                            between = cities[j];
                            if (min < 0)
                                min = between.pathcost;
                            min = Math.Min(min, cities[j].pathcost);
                        }
                    }
                    current.pathcost += min;
                }
                current.pathcost += current.toll;
            }

            //  Return the pathcost of the last city in topol
            return current.pathcost - current.toll; //  Subtract the toll of the starting place
        }

        /// <summary>
        /// Conducts a "land-locked" depth-first search of the graph, or a search that finds all
        /// vertices that are connected to the given vertex.
        /// </summary>
        /// <returns>An array that contains reverse topological order of the graph, starting and 
        /// ending at the parameters</returns>
        private IEnumerable<int> DFS(int start, int end)
        {
            //  Sanitize temporary variables
            reverseTop = new List<int>();
            foreach (City c in cities)
            {
                c.pathcost = 0;
                c.visited = false;
            }

            //  Get reversed topological order (stored in reverseTop property)
            Explore(start);
            if (reverseTop.Contains(end))
            {
                //  Return the range from 'end' to 'start' (the given parameters)
                bool range = false;
                foreach (int i in reverseTop)
                {
                    //  Start returning numbers when you find start
                    if (i == end)
                        range = true;

                    //  Return when able
                    if (range)
                        yield return i;

                    //  You're finished when you find 'start'
                    if (i == start)
                        break;
                }
            }
            else
                yield return -1;
        }

        /// <summary>
        /// Explore all cities connected to start
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private void Explore(int current)
        {
            //  Visit this city
            cities[current].visited = true;

            //  Explore all connected cities
            foreach (int i in cities[current].hwy_out)
                if (!cities[i].visited)
                    Explore(i);
            reverseTop.Add(current);
        }
    }

    public class City
    {
        public string name { get; }
        public int toll { get; }
        public List<int> hwy_out { get; set; }
        public List<int> hwy_in { get; set; }
        
        //  Temporary variables to facilitate traversal
        public int pathcost { get; set; }
        public bool visited { get; set; }

        public City (string name, int toll)
        {
            this.name = name;
            this.toll = toll;
            pathcost = 0;
            hwy_out = new List<int>();
            hwy_in = new List<int>();
        }
    }
}
