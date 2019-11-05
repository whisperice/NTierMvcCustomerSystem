using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTierMvcCustomerSystem.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.Common
{
    [TestClass]
    public class ConfigurationHandlerTest
    {
        private const string ExpectedPath = @"E:\文档\tuts\C#\NTierMvcCustomerSystem\DataSource";

        [TestMethod]
        public void GetAppSettingsValueByKey_GivenExistKey_GetCorrectValue()
        {
            var key = "DataSourcePath";
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
    }
}
