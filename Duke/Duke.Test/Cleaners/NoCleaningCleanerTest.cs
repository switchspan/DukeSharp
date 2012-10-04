using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class NoCleaningCleanerTest
    {
        private NoCleaningCleaner cleaner;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            cleaner = new NoCleaningCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            cleaner = null;
        }

        [Test]
        public void Clean_WithAString_ReturnsSameString()
        {
            var valueToTest = "This is a test";
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("No Cleaning result = {0}", actual));
            Assert.AreEqual(valueToTest, actual);
        }
    }
}