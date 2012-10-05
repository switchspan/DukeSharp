using System;
using System.IO;
using Duke.Utils;
using NUnit.Framework;

namespace Duke.Test
{
    [TestFixture]
    public class CsvReaderTest
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
        {
            // Setup code goes here...
            _reader = new CsvReader(new StreamReader(TestFileName));
        }

        [TearDown]
        public void Cleanup()
        {
            // TearDown code goes here...
            _reader.Close();
        }

        #endregion

        private const string TestFileName = "mapping-cleaner-test.txt";
        private CsvReader _reader;

        [Test]
        public void CsvReader_GetFirstRow_ReturnsStringArray()
        {
            String[] row = _reader.Next();
            Console.WriteLine(String.Format("Result = {0}", String.Join(",", row)));
            Assert.IsInstanceOf<String[]>(row);
        }
    }
}