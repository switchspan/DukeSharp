using System;
using Duke.Comparators;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class DiceCoefficientComparatorTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            comparator = new DiceCoefficientComparator();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            comparator = null;
        }

        #endregion

        private DiceCoefficientComparator comparator;

        [Test]
        public void Compare_DifferentStrings_ReturnsDoubleValue()
        {
            string val1 = "A String";
            string val2 = "Another String";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);
        }

        [Test]
        public void Compare_NumberStrings_ReturnsDoubleValue()
        {
            string val1 = "5011";
            string val2 = "5211";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);
        }

        [Test]
        public void Compare_SameStrings_ReturnsOne()
        {
            string valueToTest = "A String";
            double actual = comparator.Compare(valueToTest, valueToTest);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.AreEqual(1.0, actual);
        }

        [Test]
        public void Compare_StringVsEmptyString_ReturnsDoubleValue()
        {
            string val1 = "A String";
            string val2 = String.Empty;
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);
        }

        [Test]
        public void Compare_StringVsReversedString_ReturnsDoubleValue()
        {
            string val1 = "A String";
            string val2 = "gnirtS A";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);
            
        }

        [Test]
        public void Compare_OffByOneLetterString_ReturnsDoubleValue()
        {
            string val1 = "A String";
            string val2 = "A Strinn";
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);

        }

        [Test]
        public void Compare_NumbersWithNumericComparator_ReturnsDoubleGreaterThanZero()
        {
            string val1 = "50";
            string val2 = "52";
            comparator.SetComparator(new NumericComparator());
            double actual = comparator.Compare(val1, val2);
            Console.WriteLine(String.Format("Compare result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreNotEqual(1.0, actual);
            Assert.Less(actual, 1.0);
            Assert.Greater(actual, 0.5);
            
        }
    }
}