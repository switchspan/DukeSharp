using System;
using System.IO;
using NUnit.Framework;
using Duke.Comparators;

namespace Duke.Test
{
    [TestFixture]
    public class MetaphoneComparatorTest
    {
        private MetaphoneComparator _comparator;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            _comparator = new MetaphoneComparator();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            _comparator = null;
        }

        [Test]
        public void Compare_DifferentStrings_ReturnsDoubleValue()
        {
            const string val1 = "A String";
            const string val2 = "Another String";
            double actual = _comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);
        }

        [Test]
        public void Compare_SameString_ReturnsOneAsDouble()
        {
            const string val1 = "A String";
            const string val2 = "A String";
            double actual = _comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(1.0, actual);
        }

        [Test]
        public void IsTokenized_Called_ReturnsTrue()
        {
            bool actual = _comparator.IsTokenized();
            Console.WriteLine(String.Format("IsTokenized result = {0}", actual));
            Assert.IsTrue(actual);
        }

        [Test]
        public void Metaphone_WithString_ReturnsMetaphoneString()
        {
            var valueToTest = "This is a test";
            var expected = "0SSTST";
            var actual = MetaphoneComparator.Metaphone(valueToTest);
            Console.WriteLine(String.Format("Metaphone result = {0}", actual));
            Assert.IsInstanceOf<string>(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}