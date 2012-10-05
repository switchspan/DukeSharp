using System;
using Duke.Utils;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class StringUtilsTest
    {
        #region Setup/Teardown

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

        #endregion

        [Test]
        public void Join_NameWithAbbrev_ReturnsString()
        {
            var valueToTest = new[] {"M.", "C.", "Hammer"};
            string actual = StringUtils.Join(valueToTest);
            Console.WriteLine(String.Format("Join result = {0}", actual));
            Assert.AreEqual("M. C. Hammer", actual);
        }

        [Test]
        public void NormalizeWs_StringWithWhitespaceOnInterior_RemovesWhitespace()
        {
            const string valueToTest = "  this is        a test    ";
            const string expected = "this is a test";
            string actual = StringUtils.NormalizeWs(valueToTest);
            Console.WriteLine(String.Format("NormalizeWs result = {0}", actual));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NormalizeWs_StringWithWhitespace_RemovesWhitespace()
        {
            const string valueToTest = "  this is a test    ";
            const string expected = "this is a test";
            string actual = StringUtils.NormalizeWs(valueToTest);
            Console.WriteLine(String.Format("NormalizeWs result = {0}", actual));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReplaceAnyOf_WithStringValue_ReturnsStringWithReplacements()
        {
            const string valsToReplace = "oO";
            const char replacementChar = '0';
            const string testString = "Woot!";
            const string expected = "W00t!";
            string actual = StringUtils.ReplaceAnyOf(testString, valsToReplace, replacementChar);
            Console.WriteLine(String.Format("ReplaceAnyOf result = {0}", actual));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Split_NameWithAbbrev_ReturnsAppropriatePieces()
        {
            const string valueToTest = "J. R. Ackerley";
            string[] actual = StringUtils.Split(valueToTest);
            Console.WriteLine(String.Format("Split result = {0}", actual.ToString()));
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("J.", actual[0]);
            Assert.AreEqual("R.", actual[1]);
            Assert.AreEqual("Ackerley", actual[2]);
        }
    }
}