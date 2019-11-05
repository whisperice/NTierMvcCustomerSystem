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
    /// The Handler Class to manipulate a json file which act as the data source of the customers.
    /// </summary>
    public static class JsonFileHandler
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static JObject ReadJsonFile(string filePath, string fileName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[JsonFileHandler::ReadJsonFile] Reading file... FilePath: {0}, FileName: {1}", filePath, fileName);
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
                        JObject jObject = (JObject) JToken.ReadFrom(reader);

                        if (Logger.IsDebugEnabled)
                        {
                            Logger.Debug("[JsonFileHandler::ReadJsonFile] Read file successfully... File: {0}",
                                fullFileName);
                        }

                        return jObject;
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.Error(e, "[JsonFileHandler::ReadJsonFile] Directory: {0} not found.", filePath);
                throw new DataAccessException($"JsonFileHandler::ReadJsonFile: Directory: {filePath} not found", e);
            }
            catch (FileNotFoundException e)
            {
                var fullFileName = filePath + Path.DirectorySeparatorChar + fileName;
                Logger.Error(e, "[JsonFileHandler::ReadJsonFile] File: {0} not found.", fullFileName);
                throw new DataAccessException($"JsonFileHandler::ReadJsonFile: File: {fullFileName} not found.", e);
            }
            catch (Exception e)
            {
                Logger.Error(e, "[JsonFileHandler::ReadJsonFile] Read file error.");
                throw new DataAccessException("JsonFileHandler::ReadJsonFile: Read file error.", e);
            }
        }

        public static void WriteJsonFile(string filePath, string fileName, JObject jObject)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("[JsonFileHandler::WriteJsonFile] Writing file... FilePath: {0}, FileName: {1}", filePath, fileName);
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
                using (StreamWriter file = File.CreateText(fullFileName))
                {
                    using (JsonTextWriter writer = new JsonTextWriter(file))
                    {
                        writer.Formatting = Formatting.Indented;
                        jObject.WriteTo(writer);

                        if (Logger.IsDebugEnabled)
                        {
                            Logger.Debug("[JsonFileHandler::WriteJsonFile] Read file successfully... File: {0}",
                                fullFileName);
                        }
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Logger.Error(e, "[JsonFileHandler::WriteJsonFile] Directory: {0} not found.", filePath);
                throw new DataAccessException($"JsonFileHandler::WriteJsonFile: Directory: {filePath} not found", e);
            }
            catch (Exception e)
            {
                Logger.Error(e, "[JsonFileHandler::WriteJsonFile] Read file error.");
                throw new DataAccessException("JsonFileHandler::WriteJsonFile: Read file error.", e);
            }
        }
    }
}
