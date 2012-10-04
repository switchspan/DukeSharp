using System;
using System.IO;
using NUnit.Framework;
using Duke.Utils;

namespace Duke.Test
{
    [TestFixture]
    public class StringUtilsTest
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
        public void Split_NameWithAbbrev_ReturnsAppropriatePieces()
        {
            var valueToTest = "J. R. Ackerley";
            var actual = StringUtils.Split(valueToTest);
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("J.", actual[0]);
            Assert.AreEqual("R.", actual[1]);
            Assert.AreEqual("Ackerley", actual[2]);
        }
    }
}