using System;
using System.IO;
using NUnit.Framework;
using Duke.Comparators;

namespace Duke.Test
{
    [TestFixture]
    public class DifferentComparatorTest
    {
        private DifferentComparator comparator;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            comparator = new DifferentComparator();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            comparator = null;
        }

        [Test]
        public void Compare_DifferentValues_ReturnsOneAsDouble()
        {
            string val1 = "5011";
            string val2 = "5211";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(1.0, actual);
        }

        [Test]
        public void Compare_SameValues_ReturnsZeroAsDouble()
        {
            string val1 = "ABC123";
            string val2 = "ABC123";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(0.0, actual);
            
        }

        [Test]
        public void Compare_SameValuesDifferentCase_ReturnsOneAsDouble()
        {
            string val1 = "ABC123";
            string val2 = "abc123";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(1.0, actual);

        }
    }
}