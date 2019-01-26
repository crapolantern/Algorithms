/*
 *      Program:    Tree Shape Compare
 *      Author:     Wess Lancaster
 *      Date:       1/25/2019
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeShapeCompare
{
    class Program
    {
        /// <summary>
        /// Main method only handles IO concerns
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            string[] input;
            int prototypes, length;
            string treeData;

            //  A list of int arrays. Each int[] contains node positions for a tree. If a tree 
            List<string> prototype_log = new List<string>();

            /*
             * INPUT:
             * 5 3      How many prototypes, how many digits per prototype
             * 2 7 1    Each line is a prototype, with a certain number of digits
             * 3 1 4    
             * 1 5 9
             * 2 6 5
             * 9 7 3
             * 
             * OUTPUT:
             * 4        How many shapes of trees there exist from the input
             */
            
            //  Read and translate first line
            input = Console.ReadLine().Split();
            prototypes = int.Parse(input[0]);
            length = int.Parse(input[1]);

            //  Nested loop: Create a tree from each line
            for (int i = 0; i < prototypes; i++)
            {
                input = Console.ReadLine().Split();

                //  Nested loop: Add all numbers to the tree
                for (int j = 0; j < length; j++)
                {
                    tree.Add(int.Parse(input[j]));
                }

                //  Store tree information in a string
                treeData = tree.Positions();
                //  Only add shapes we haven't seen yet
                if (!prototype_log.Contains(treeData))
                    prototype_log.Add(treeData);

                //  Start over with a new tree
                tree.Clear();
            }

            //  Report the length of the list, which is the number of tree shapes we've seen
            Console.WriteLine(prototype_log.Count);

            Console.Read();
        }
    }

    /// <summary>
    /// A structure that stores nodes according to their value. The first object received is the
    /// root (index 1), and all others "trickle down" underneath it. They can move downward to the 
    /// left or right depending on if they are less than or greater than the parent node,
    /// respectively. Every assignment to a position is permanent;
    /// 
    /// Indexes are as follows:         Not all positions have to be taken though:
    ///          1                                            1
    ///        /   \                                         / \
    ///       2     3                                       2   3
    ///      / \   / \                                     /   /
    ///     4   5 6   7                                   4   6
    /// This means that a node's parent will always be its index/2, and a node's child will be 2n if
    /// its less, or 2n+1 if it's greater. Because of this, there's no need to link nodes to each
    /// other; instead math is used to navigate the tree.
    /// 
    /// This tree is not balanced, or balancing.
    /// </summary>
    class Tree
    {
        private Node root;  //  The root node, starts every search
        private Dictionary<int, Node> nodes = new Dictionary<int, Node>(); //  index => value

        /// <summary>
        /// Finds where the given value should be located in the tree. It doesn't have to exist
        /// </summary>
        /// <param name="value">Value to search for</param>
        /// <returns>The index of where the node should be located</returns>
        public int Search(int value)
        {
            Node n = root;
            int index = 1;

            //  Continue looping until we reach a node that matches the value
            while (n != null && n.value != value)
            {
                // Not found, trickle down. Current = child
                index = ChildIndex(n, value);
                if (!nodes.TryGetValue(index, out Node found))
                    n = null;
                else 
                    n = found;
            }

            return index;
        }

        /// <summary>
        /// Adds the value to the tree by searching for a fitting index and creating the node
        /// </summary>
        /// <param name="value"></param>
        public void Add(int value)
        {
            int index = Search(value);
            nodes.Add(index, new Node(index, value));
            
            //  Now that we have at least 1 entry, set the root (never changes)
            root = nodes[1];
        }

        /// <summary>
        /// Returns a sorted, lettered version of positions contained in a string. For example, if
        /// there were tree positions stored on 1, 2, 3, and 7, it would return "ABCG" regardless of
        /// the order they were fed to the tree. This is done so that they can be easily compared in
        /// the future.
        /// </summary>
        /// <returns></returns>
        public string Positions()
        {
            string pos = "";
            //  Add the letter version of all indexes
            foreach (int i in nodes.Keys)
                pos += ('z' + i);

            //  Return the alphabetized string
            return String.Concat(pos.OrderBy(s => s));
        }

        public void PrintTreePositions()
        {
            foreach (int i in nodes.Keys)
            {
                Console.Write(i + " ");
            }
        }

        public void Clear()
        {
            nodes = new Dictionary<int, Node>();
            root = null;
        }

        /// <summary>
        /// Returns the index of the child based from a value comparison. The child doesn't have to
        /// exist.
        /// </summary>
        /// <param name="parent">The parent node</param>
        /// <param name="child_value">The child's value</param>
        /// <returns>The child's index, if it exists</returns>
        private int ChildIndex(Node parent, int child_value)
        {
            //  If the parent is greater, the child's index is 2(parent_index)
            if (parent.value > child_value)
                return parent.index * 2;

            //  If the child is greater, the child's index is 2(parent_index) + 1
            return (parent.index * 2) + 1;
        }
    }

    class Node
    {
        // Public properties
        public int index { get; }
        public int value { get; }
        /// <summary>
        /// Constructor for a new node. Must already have chosen placement
        /// </summary>
        /// <param name="value"></param>
        public Node(int index, int value)
        {
            this.value = value;
            this.index = index;
        }
    }
}
