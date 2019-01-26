using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnagramDetector

{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AllSame()
        {
            MainDetector td = new TestableDetector();
        }
    }
}
