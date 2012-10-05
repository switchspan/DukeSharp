using System;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class TransformTest
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
            _transform = null;
        }

        #endregion

        private Transform _transform;

        [Test]
        public void TransformValue_WithNumbersToXs_ReturnsTransformedString()
        {
            _transform = new Transform(@"(\d)", @"X");
            const string valueToTransform = "abc123xyz";
            string actual = _transform.TransformValue(valueToTransform);
            Console.WriteLine(String.Format("TransformValue result = {0}", actual));
            Assert.AreEqual("abcXXXxyz", actual);
        }
    }
}