using System;
using Duke.Utils;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class StandardUtilsTest
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
        }

        #endregion

        [Test]
        public void ComputeBayes_BothValuesZero_ReturnsZero()
        {
            const double val1 = 0.0;
            const double val2 = 0.0;
            double actual = StandardUtils.ComputeBayes(val1, val2);
            Console.WriteLine(String.Format("ComputeBayes result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(0.0, actual);
        }

        [Test]
        public void ComputeBayes_OneValueZero_ReturnsZero()
        {
            const double val1 = 0.0;
            const double val2 = 19.22;
            double actual = StandardUtils.ComputeBayes(val1, val2);
            Console.WriteLine(String.Format("ComputeBayes result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.AreEqual(0.0, actual);
        }

        [Test]
        public void ComputeBayes_TwoDoubles_ReturnsProbabilityAsDouble()
        {
            const double val1 = 52.3;
            const double val2 = 19.22;
            double actual = StandardUtils.ComputeBayes(val1, val2);
            Console.WriteLine(String.Format("ComputeBayes result = {0}", actual));
            Assert.IsInstanceOf<double>(actual);
            Assert.Greater(actual, 0.0);
        }
    }
}