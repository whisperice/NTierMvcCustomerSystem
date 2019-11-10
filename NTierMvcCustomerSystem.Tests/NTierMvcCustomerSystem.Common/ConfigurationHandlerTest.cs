using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.Common
{
    [TestClass]
    public class ConfigurationHandlerTest
    {
        private const string ExpectedPath = TestConstants.DataSourcePath;

        [TestMethod]
        public void GetAppSettingsValueByKey_GivenExistKey_GetCorrectValue()
        {
            var key = Constants.DataSourcePathKey;
            var expectedValue = ExpectedPath;

            var value = ConfigurationHandler.GetAppSettingsValueByKey(key);

            Assert.AreEqual(expectedValue, value);

        }

        [TestMethod]
        public void GetDataSourcePath_GivenNoKey_GetCorrectValue()
        {
            var expectedValue = ExpectedPath;

            var value = ConfigurationHandler.GetDataSourcePath();

            Assert.AreEqual(expectedValue, value);

        }

        [TestMethod]
        public void Test()
        {
            var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(location);
        }
    }
}
