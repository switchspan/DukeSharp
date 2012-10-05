using System;
using Duke.Comparators;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class MatchTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            _comparator = null;
        }

        #endregion

        private Match _comparator;

        [Test]
        public void CompareTo_DifferentValues_ReturnsIntValue()
        {
            var compareTo = new Match(.55, 22, 44);
            _comparator = new Match(.50, 23, 43);
            int actual = _comparator.CompareTo(compareTo);
            Console.WriteLine(String.Format("CompareTo result = {0}", actual));
            Assert.IsInstanceOf<int>(actual);
        }

        [Test]
        public void CompareTo_HigherScore_ReturnsNegativeOne()
        {
            var compareTo = new Match(.25, 22, 44);
            _comparator = new Match(.50, 22, 44);
            int actual = _comparator.CompareTo(compareTo);
            Console.WriteLine(String.Format("CompareTo result = {0}", actual));
            Assert.AreEqual(-1, actual);
        }

        [Test]
        public void CompareTo_LowerScore_ReturnsOne()
        {
            var compareTo = new Match(.50, 22, 44);
            _comparator = new Match(.45, 22, 44);
            int actual = _comparator.CompareTo(compareTo);
            Console.WriteLine(String.Format("CompareTo result = {0}", actual));
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void CompareTo_SameScore_ReturnsZero()
        {
            var compareTo = new Match(.50, 22, 44);
            _comparator = new Match(.50, 22, 44);
            int actual = _comparator.CompareTo(compareTo);
            Console.WriteLine(String.Format("CompareTo result = {0}", actual));
            Assert.AreEqual(0, actual);
        }

        [Test]
        public void CompareTo_SameValues_ReturnsIntValue()
        {
            var compareTo = new Match(.50, 22, 44);
            _comparator = new Match(.50, 22, 44);
            int actual = _comparator.CompareTo(compareTo);
            Console.WriteLine(String.Format("CompareTo result = {0}", actual));
            Assert.IsInstanceOf<int>(actual);
        }

        [Test]
        public void Match_NewObject_ReturnsInitValues()
        {
            _comparator = new Match(.50, 23, 43);
            Console.WriteLine(String.Format("Match: Score = {0}, Ix1 = {1}, Ix2 = {2}", _comparator.Score,
                                            _comparator.Ix1, _comparator.Ix2));
            Assert.AreEqual(0.5, _comparator.Score);
            Assert.AreEqual(23, _comparator.Ix1);
            Assert.AreEqual(43, _comparator.Ix2);
        }
    }
}