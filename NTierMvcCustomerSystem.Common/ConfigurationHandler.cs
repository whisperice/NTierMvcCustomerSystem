using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Common
{
    /// <summary>
    /// A common handler for reading application configuration from Web.config
    /// </summary>
    public static class ConfigurationHandler
    {
        private const string DataSourcePath = "DataSourcePath";

        private static string _dataSourcePath;
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static string GetAppSettingsValueByKey(string key)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[ConfigurationHandler::GetAppSettingsValueByKey] Getting key: {0}", key);
            }

            try
            {
                if (string.IsNullOrEmpty(key))
                    throw new ArgumentNullException(nameof(key), "The AppSettings key name can't be Null or Empty.");

                var appSettingsValue = ConfigurationManager.AppSettings[key];
                if (appSettingsValue == null)
                    throw new ConfigurationErrorsException(
                        $"Failed to find the AppSettings Key named '{key}' in App/Web.config.");

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("[ConfigurationHandler::GetAppSettingsValueByKey] Key: {0}, Value: {1}", key, appSettingsValue);
                }
                return appSettingsValue;
            }
            catch (Exception e)
            {
                Logger.Error(e, "[ConfigurationHandler::GetAppSettingsValueByKey] Error occured.");
                throw new CommonException("ConfigurationHandler::GetAppSettingsValueByKey: Error occured.", e);
            }
        }

        public static string GetDataSourcePath()
        {
            try
            {
                if (!string.IsNullOrEmpty(_dataSourcePath))
                {
                    return _dataSourcePath;
                }

                _dataSourcePath = GetAppSettingsValueByKey(DataSourcePath);
                return _dataSourcePath;
            }
            catch (Exception e)
            {
                throw new CommonException("ConfigurationHandler::GetDataSourcePath:Error occured.", e);
            }
        }
    }
}
