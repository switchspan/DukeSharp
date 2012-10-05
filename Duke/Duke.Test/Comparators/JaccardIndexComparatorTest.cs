using System;
using System.IO;
using NUnit.Framework;
using Duke.Comparators;

namespace Duke.Test
{
    [TestFixture]
    public class JaccardIndexComparatorTest
    {
        private JaccardIndexComparator comparator;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            comparator = new JaccardIndexComparator();
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
        public void Compare_DifferentWithNumericComparator_ReturnsDouble()
        {
            string val1 = "5011";
            string val2 = "5012";
            comparator.SetComparator(new NumericComparator());
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.Less(actual, 1.0);
            
        }

        [Test]
        public void Compare_DifferentWithSoundexComparator_ReturnsDouble()
        {
            string val1 = "This is a string";
            string val2 = "This is yet another string";
            comparator.SetComparator(new SoundexComparator());
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.Less(actual, 1.0);
        }

    }
}