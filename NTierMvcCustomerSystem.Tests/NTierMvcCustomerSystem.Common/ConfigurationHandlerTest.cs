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

        [TestMethod]
        public void GetAppSettingsValueByKey_GivenExistKey_GetCorrectValue()
        {
            var key = Constants.DataSourcePathKey;
            var expectedValue = TestConstants.DataSourcePath;

            var value = ConfigurationHandler.GetAppSettingsValueByKey(key);

            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void GetDataSourcePath_RunOnce_GetCorrectValue()
        {
            var expectedValue = GetDirectoryName();

            var value = ConfigurationHandler.GetDataSourcePath();
            value = Directory.GetParent(value).FullName;

            Assert.AreEqual(expectedValue, value);

        }

        [TestMethod]
        public void GetDataSourcePath_RunTwice_GetCorrectValue()
        {
            var expectedValue = GetDirectoryName();

            ConfigurationHandler.GetDataSourcePath();
            var value = ConfigurationHandler.GetDataSourcePath();
            value = Directory.GetParent(value).FullName;

            Assert.AreEqual(expectedValue, value);

        }

        private static string GetDirectoryName()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (string.IsNullOrEmpty(directoryName))
            {
                directoryName = ConfigurationHandler.GetAppSettingsValueByKey(Constants.DataSourcePathKey);
            }

            return directoryName;
        }
    }
}
