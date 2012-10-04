using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class LowerCaseNormalizeCleanerTest
    {
        private LowerCaseNormalizeCleaner cleaner;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            cleaner = new LowerCaseNormalizeCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            cleaner = null;
        }

        [Test]
        public void Clean_WithAccentsInString_StripsAccentsOut()
        {
            string valToTest = "AKéSJDGHASDé";
            string expected = "akesjdghasde";
            var actual = cleaner.Clean(valToTest);
            Console.WriteLine(String.Format("Accented LowerCaseNormalize clean result = {0}", actual));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Clean_WithDigits_LowercasesLetters()
        {
            string valToTest = "AHe45Bg";
            string expected = "ahe45bg";
            var actual = cleaner.Clean(valToTest);
            Console.WriteLine(String.Format("Digits LowerCaseNormalize clean result = {0}", actual));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Clean_WithAccentStripOff_LeavesInAccentCharacters()
        {
            string valToTest = "AKéSJDGHASDé";
            string expected = "akésjdghasdé";
            cleaner.SetStripAccents(false);
            var actual = cleaner.Clean(valToTest);
            Console.WriteLine(String.Format("With Accented LowerCaseNormalize clean result = {0}", actual));
            Assert.AreEqual(expected, actual);
            
        }
    }
}