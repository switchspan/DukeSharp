using System;
using System.IO;
using NUnit.Framework;
using Duke.Comparators;

namespace Duke.Test
{
    [TestFixture]
    public class SoundexComparatorTest
    {
        private SoundexComparator comparator;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            comparator = new SoundexComparator();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            comparator = null;
        }

        [Test]
        public void Compare_DifferentValues_ReturnsDoubleValue()
        {
            string val1 = "5011";
            string val2 = "5211";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.Less(actual, 1.0);
        }

        [Test]
        public void Compare_SameValues_ReturnsOneAsDouble()
        {
            string val1 = "5011";
            string val2 = "5011";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(1.0, actual);
        }

        [Test]
        public void Compare_TestandText_ReturnsPointNineAsDouble()
        {
            string val1 = "test";
            string val2 = "text";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.GreaterOrEqual(actual, 0.9);
        }

    }
}