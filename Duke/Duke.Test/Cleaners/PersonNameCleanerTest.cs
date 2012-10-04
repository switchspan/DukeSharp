using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class PersonNameCleanerTest
    {
        private PersonNameCleaner cleaner;

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            cleaner = new PersonNameCleaner();
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            cleaner = null;
        }

        [Test]
        public void Clean_KnownNickname_ReturnsFullname()
        {
            var valueToTest = "Al Jones";
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("KnownNickname result = {0}", actual));
            Assert.AreEqual("albert jones", actual);
        }
    }
}