using System;
using System.IO;
using NUnit.Framework;
using Duke.Comparators;

namespace Duke.Test
{
    [TestFixture]
    public class ExactComparatorTest
    {
        private ExactComparator comparator;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            comparator = new ExactComparator();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            comparator = null;
        }

        [Test]
        public void Compare_SameValues_ReturnsOneAsDouble()
        {
            string val1 = "ABC123";
            string val2 = "ABC123";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(1.0, actual);
        }

        [Test]
        public void Compare_DifferentValues_ReturnsZeroAsDouble()
        {
            string val1 = "ABC123";
            string val2 = "ABD123";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(0.0, actual);
            
        }
    }
}