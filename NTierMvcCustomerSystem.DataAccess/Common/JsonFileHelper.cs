using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;


namespace NTierMvcCustomerSystem.DataAccess.Common
{
    /// <summary>
    /// The Helper Class to manipulate a json file which act as the data source of the customers.
    /// </summary>
    public static class JsonFileHelper
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static JObject ReadJsonFile(string filePath, string fileName)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[JsonFileHandler::ReadJsonFile] Reading file... FilePath: {0}, FileName: {1}", filePath, fileName);
            }

            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath), "The file path can't be Null or Empty.");
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName), "The file path can't be Null or Empty.");
                }

                var fullFileName = filePath + Path.DirectorySeparatorChar + fileName;
                using (StreamReader file = File.OpenText(fullFileName))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject jObject = (JObject)JToken.ReadFrom(reader);

                        if (_logger.IsDebugEnabled)
                        {
                            _logger.Debug("[JsonFileHandler::ReadJsonFile] Read file successfully... File: {0}",
                                fullFileName);
                        }

                        return jObject;
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                _logger.Error(e, "[JsonFileHandler::ReadJsonFile] Directory: {0} not found.", filePath);
                throw new DataAccessException($"JsonFileHandler::ReadJsonFile: Directory: {filePath} not found", e);
            }
            catch (FileNotFoundException e)
            {
                var fullFileName = filePath + Path.DirectorySeparatorChar + fileName;
                _logger.Error(e, "[JsonFileHandler::ReadJsonFile] File: {0} not found.", fullFileName);
                throw new DataAccessException($"JsonFileHandler::ReadJsonFile: File: {fullFileName} not found.", e);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[JsonFileHandler::ReadJsonFile] Read file error.");
                throw new DataAccessException("JsonFileHandler::ReadJsonFile: Read file error.", e);
            }
        }

        public static void WriteJsonFile(string filePath, string fileName, JObject jObject)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[JsonFileHandler::WriteJsonFile] Writing file... FilePath: {0}, FileName: {1}", filePath, fileName);
            }

            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new ArgumentNullException(nameof(filePath), "The file path can't be Null or Empty.");
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName), "The file path can't be Null or Empty.");
                }

                // Create directory if it is not exist
                Directory.CreateDirectory(filePath);

                var fullFileName = filePath + Path.DirectorySeparatorChar + fileName;
                using (StreamWriter file = File.CreateText(fullFileName))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(file))
                    {
                        writer.Formatting = Formatting.Indented;
                        jObject.WriteTo(writer);

                        if (_logger.IsDebugEnabled)
                        {
                            _logger.Debug("[JsonFileHandler::WriteJsonFile] Read file successfully... File: {0}",
                                fullFileName);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                _logger.Error(e, "[JsonFileHandler::WriteJsonFile] Directory: {0} not found.", filePath);
                throw new DataAccessException($"JsonFileHandler::WriteJsonFile: Directory: {filePath} not found", e);
            }
            catch (Exception e)
            {
                _logger.Error(e, "[JsonFileHandler::WriteJsonFile] Read file error.");
                throw new DataAccessException("JsonFileHandler::WriteJsonFile: Read file error.", e);
            }
        }
    }
}
