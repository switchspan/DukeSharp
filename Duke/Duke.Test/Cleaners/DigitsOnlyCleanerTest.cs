using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class DigitsOnlyCleanerTest
    {
        private DigitsOnlyCleaner cleaner;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            cleaner = new DigitsOnlyCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            cleaner = null;
        }

        [Test]
        public void Clean_AlphanumericString_ReturnsOnlyDigits()
        {
            const string valueToClean = "Abc12321hds";
            var actual = cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Alphanumeric Digits clean result = {0}", actual));
            Assert.AreEqual("12321", actual);
        }

        [Test]
        public void Clean_NoDigitsInString_ReturnsEmptyString()
        {
            const string valueToClean = "alsdkfjLSKJFSHDFSD";
            var actual = cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Only Alpha Digits clean result = {0}", actual));
            Assert.IsEmpty(actual);
        }
    }
}