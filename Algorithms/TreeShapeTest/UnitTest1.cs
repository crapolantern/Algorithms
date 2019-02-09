using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeShapeCompare;

namespace TreeShapeTest
{
    [TestClass]
    public class UnitTest1
    {
        Tree tree = new Tree();
        [TestMethod]
        public void RightSlant()
        {
            tree.Add(1);
            tree.Add(2);
            tree.Add(3);
            tree.Add(4);
            tree.Add(5);
            tree.Add(6);
            tree.Add(7);
            tree.Add(8);
            tree.Add(9);
            tree.Add(10);
            tree.Add(11);
            tree.Add(12);
            tree.Add(13);
            tree.Add(14);
            tree.Add(15);

            Assert.AreEqual("1.3.7.15.31.63.127.255.511.1023.2047.4095.8191.16383.32767.", tree.Positions());
        }

        [TestMethod]
        public void LeftSlant()
        {
            tree.Add(15);
            tree.Add(14);
            tree.Add(13);
            tree.Add(12);
            tree.Add(11);
            tree.Add(10);
            tree.Add(9);
            tree.Add(8);
            tree.Add(7);
            tree.Add(6);
            tree.Add(5);
            tree.Add(4);
            tree.Add(3);
            tree.Add(2);
            tree.Add(1);

            Assert.AreEqual("1.2.4.8.16.32.64.128.256.512.1024.2048.4096.8192.16384.", tree.Positions());
        }

        [TestMethod]
        public void Balanced()
        {
            tree.Add(8);
            tree.Add(4);
            tree.Add(12);
            tree.Add(2);
            tree.Add(6);
            tree.Add(10);
            tree.Add(14);
            tree.Add(1);
            tree.Add(3);
            tree.Add(5);
            tree.Add(7);
            tree.Add(9);
            tree.Add(11);
            tree.Add(13);
            tree.Add(15);

            Assert.AreEqual("1.2.3.4.5.6.7.8.9.10.11.12.13.14.15.", tree.Positions());
        }

        [TestMethod]
        public void Clear()
        {
            RightSlant();
            tree.Clear();
            LeftSlant();
            tree.Clear();
            Balanced();
            tree.Clear();

            Assert.AreEqual("", tree.Positions());
        }
    }
}
