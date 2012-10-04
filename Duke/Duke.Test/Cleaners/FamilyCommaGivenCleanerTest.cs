using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class FamilyCommaGivenCleanerTest
    {
        private FamilyCommaGivenCleaner cleaner;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            cleaner = new FamilyCommaGivenCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            cleaner = null;
        }

        [Test]
        public void Clean_LastnameCommaFirst_FormattedString()
        {
            var valueToTest = "Smith, John";
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("LastnameCommaFirst result = {0}", actual));
            Assert.AreEqual("john smith", actual);
        }

        [Test]
        public void Clean_AbbreviatedName_AddsSpacesToInitials()
        {
            var valueToTest = "J.R. Ackerley";
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("LastnameCommaFirst result = {0}", actual));
            Assert.AreEqual("j. r. ackerley", actual); 
        }

        [Test]
        public void Clean_AbreviatedNameThreeLetters_AddsSpacesToInitials()
        {
            var valueToTest = "J.R.R. Tolkien";
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("LastnameCommaFirst result = {0}", actual));
            Assert.AreEqual("j. r. r. tolkien", actual); 
            
        }
    }
}