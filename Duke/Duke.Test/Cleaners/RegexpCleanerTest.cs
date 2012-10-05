using System;
using Duke.Cleaners;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class RegexpCleanerTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            //cleaner = new RegexpCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            //cleaner = null;
        }

        #endregion

        [Test]
        public void Clean_SelectingAlphaCharacters_ReturnsAlphaCharacters()
        {
            const string valueToClean = "asdfCD324asdf11";
            const string regexString = @"([a-zA-Z]+)";
            var cleaner = new RegexpCleaner(regexString);
            string actual = cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Clean result = {0}", actual));
            Assert.AreEqual("asdfCD", actual);
        }

        [Test]
        public void Clean_SelectingDigitsOfSecondGroup_ReturnsSecondGroup()
        {
            const string valueToClean = "123-4567";
            const string regexString = @"(\d{3})-(\d{4})";
            var cleaner = new RegexpCleaner(regexString);
            cleaner.SetGroup(2);
            string actual = cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Clean result = {0}", actual));
            Assert.AreEqual("4567", actual);
        }

        [Test]
        public void Clean_SelectingDigits_OnlyReturnsDigits()
        {
            const string valueToClean = "asdfcd324asdf11";
            const string regexString = @"(\d+)";
            var cleaner = new RegexpCleaner(regexString);
            string actual = cleaner.Clean(valueToClean);
            Console.WriteLine(String.Format("Clean result = {0}", actual));
            Assert.AreEqual("324", actual);
        }
    }
}