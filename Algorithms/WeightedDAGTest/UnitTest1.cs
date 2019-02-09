using System;
using DAG;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WeightedDAGTest
{
    [TestClass]
    public class UnitTest1
    {
        WeightedDAG dag = new WeightedDAG(0);

        public void BuildLayout1(out WeightedDAG dag)
        {
            dag = new WeightedDAG(12);
            dag.AddCity("A", 1 , 0);
            dag.AddCity("B", 5 , 1);
            dag.AddCity("C", 10, 2);
            dag.AddCity("D", 15, 3);
            dag.AddCity("E", 20, 4);
            dag.AddCity("F", 25, 5);
            dag.AddCity("G", 30, 6);
            dag.AddCity("H", 35, 7);
            dag.AddCity("I", 40, 8);
            dag.AddCity("J", 45, 9);
            dag.AddCity("K", 50, 10);
            dag.AddCity("L", 55, 11);
            dag.AddHighway("A", "B");
            dag.AddHighway("A", "C");
            dag.AddHighway("B", "D");
            dag.AddHighway("C", "B");
            dag.AddHighway("C", "D");
            dag.AddHighway("D", "E");
            dag.AddHighway("D", "F");
            dag.AddHighway("E", "F");
            dag.AddHighway("G", "H");
            dag.AddHighway("I", "J");
        }

        public void BuildLayout2(out WeightedDAG dag, int limit)
        {
            dag = new WeightedDAG(limit);
            dag.AddCity("0", 0, 0);
            for (int i = 1; i < limit; i++)
            {
                dag.AddCity(i.ToString(), i, i);
                dag.AddHighway((i-1).ToString(), i.ToString());
            }
        }

        [TestMethod]
        public void OneCity()
        {
            BuildLayout1(out dag);
            Assert.AreEqual(0, dag.RequestComputation("A", "A"));
            Assert.AreEqual(0, dag.RequestComputation("L", "L"));
            Assert.AreEqual(-1, dag.RequestComputation("A", "K"));
            Assert.AreEqual(-1, dag.RequestComputation("K", "A"));
            Assert.AreEqual(-1, dag.RequestComputation("L", "K"));
        }

        [TestMethod]
        public void TwoCities()
        {
            BuildLayout1(out dag);
            Assert.AreEqual(5, dag.RequestComputation("A", "B"));
            Assert.AreEqual(10, dag.RequestComputation("A", "C"));
            Assert.AreEqual(35, dag.RequestComputation("G", "H"));
            Assert.AreEqual(45, dag.RequestComputation("I", "J"));
            Assert.AreEqual(-1, dag.RequestComputation("G", "J"));
        }

        [TestMethod]
        public void BuildLayout1Tests()
        {
            BuildLayout1(out dag);
            Assert.AreEqual(45, dag.RequestComputation("A", "F"));
            Assert.AreEqual(40, dag.RequestComputation("A", "E"));
            Assert.AreEqual(20, dag.RequestComputation("A", "D"));
            Assert.AreEqual(35, dag.RequestComputation("B", "E"));
            Assert.AreEqual(40, dag.RequestComputation("B", "F"));
            Assert.AreEqual(35, dag.RequestComputation("C", "E"));
            Assert.AreEqual(40, dag.RequestComputation("C", "F"));
        }

        [TestMethod]
        public void BuildLayout2Tests()
        {
            const int limit = 100;
            BuildLayout2(out dag, limit);
            int sum = 0;
            for (int i = 0; i < limit; i++)
                sum += i;
            Assert.AreEqual(sum, dag.RequestComputation("0", (limit-1).ToString()));
        }
    }
}
