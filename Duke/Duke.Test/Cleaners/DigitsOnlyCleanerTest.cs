using System;
using Duke.Cleaners;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class DigitsOnlyCleanerTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            _cleaner = new DigitsOnlyCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            _cleaner = null;
        }

        #endregion

        private DigitsOnlyCleaner _cleaner;

        [Test]
        public void Clean_AlphanumericString_ReturnsOnlyDigits()
        {
            const string valueToClean = "Abc12321hds";
            string actual = _cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Alphanumeric Digits clean result = {0}", actual));
            Assert.AreEqual("12321", actual);
        }

        [Test]
        public void Clean_NoDigitsInString_ReturnsEmptyString()
        {
            const string valueToClean = "alsdkfjLSKJFSHDFSD";
            string actual = _cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Only Alpha Digits clean result = {0}", actual));
            Assert.IsEmpty(actual);
        }
    }
}