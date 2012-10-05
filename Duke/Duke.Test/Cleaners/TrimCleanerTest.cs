using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class TrimCleanerTest
    {
        private TrimCleaner cleaner;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            cleaner = new TrimCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            cleaner = null;
        }

        [Test]
        public void Clean_StringWithLeadingAndTrailingSpaces_ReturnsTrimmedString()
        {
            var valueToTest = "  thistest  ";
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("Clean result = {0}", actual));
            Assert.AreEqual(8, actual.Length);
        }
    }
}