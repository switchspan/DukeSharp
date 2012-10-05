using System;
using System.IO;
using NUnit.Framework;
using Duke.Cleaners;

namespace Duke.Test
{
    [TestFixture]
    public class MappingFileCleanerTest
    {
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

        [Test]
        public void Clean_MappedItem_ReturnsMapping()
        {
            var mappingFile = "mapping-cleaner-test.txt";
            var valueToTest = "shizzle";
            var cleaner = new MappingFileCleaner();
            cleaner.SetMappingFile(mappingFile);
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("MappedItem result = {0}", actual));
            Assert.AreEqual("sure", actual);
        }

        [Test]
        public void Clean_NonMappedItem_ReturnsOriginalValue()
        {
            var mappingFile = "mapping-cleaner-test.txt";
            var valueToTest = "kazam";
            var cleaner = new MappingFileCleaner();
            cleaner.SetMappingFile(mappingFile);
            var actual = cleaner.Clean(valueToTest);
            Console.WriteLine(String.Format("MappedItem result = {0}", actual));
            Assert.AreEqual("kazam", actual);
            
        }
    }
}