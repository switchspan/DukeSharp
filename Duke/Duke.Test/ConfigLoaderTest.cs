using System;
using System.IO;
using NUnit.Framework;
using Duke;
using Duke.Comparators;

namespace Duke.Test
{
    [TestFixture]
    public class ConfigLoaderTest
    {
        private const string TestConfigPath = "test_config.xml";

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
        public void Load_WithValidConfigurationPath_ReturnsConfigurationObject()
        {
            var actual = ConfigLoader.Load(TestConfigPath);
            
            Assert.IsInstanceOf<Configuration>(actual);
        }

        [Test]
        public void Load_WithValidConfigurationPath_HasAffliationProperty()
        {
            var config = ConfigLoader.Load(TestConfigPath);
            var actual = config.GetPropertyByName("AFFILIATION");
            Console.WriteLine(String.Format("{0}", actual));
            Assert.AreEqual("AFFILIATION", actual.Name);
        }

        [Test]
        public void Load_WithAffiliationProperty_HasLowProbabilityOfPointFourEight()
        {
            var config = ConfigLoader.Load(TestConfigPath);
            var actual = config.GetPropertyByName("AFFILIATION");
            var expected = 0.48;
            Console.WriteLine(String.Format("Low Propbablility = {0}", actual.LowProbability));
            Assert.AreEqual(expected, actual.LowProbability);
        }

        [Test]
        public void Load_WithAffiliationProperty_HasExactComparator()
        {
            var config = ConfigLoader.Load(TestConfigPath);
            var actual = config.GetPropertyByName("AFFILIATION");
            Console.WriteLine(String.Format("Comparator = {0}", actual.Comparator));
            
            Assert.IsInstanceOf<ExactComparator>(actual.Comparator);
            
        }
    }
}