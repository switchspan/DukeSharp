using System;
using System.IO;
using Duke.Cleaners;
using NUnit.Framework;
using Duke;

namespace Duke.Test
{
    [TestFixture]
    public class ColumnTest
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
        public void Column_GetName_ReturnsIntializedValue()
        {
            var actual = new Column("TestCol", "TestProperty", "TestPrefix", new LowerCaseNormalizeCleaner());

            Assert.AreEqual("TestCol", actual.GetName());
            Assert.AreEqual("TestProperty", actual.GetProperty());
            Assert.AreEqual("TestPrefix", actual.GetPrefix());
            Assert.IsInstanceOf<LowerCaseNormalizeCleaner>(actual.GetCleaner());
            
        }
    }
}