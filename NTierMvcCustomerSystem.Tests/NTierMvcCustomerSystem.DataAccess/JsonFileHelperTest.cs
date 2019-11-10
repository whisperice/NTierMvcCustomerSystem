using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.Model;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class JsonFileHelperTest
    {
        private string _fileName;
        private string _filePath;
        private JObject _jObject;

        [TestInitialize]
        public void Init()
        {
            _fileName = TestConstants.CustomerFileName;
            _filePath = Path.Combine(TestConstants.DataSourcePath, TestConstants.DataSourcePathSegment);
            _jObject = JObject.Parse(@"{
	                                            'Customers': [
		                                            {
			                                            'Id': 1,
			                                            'UserName': 'whisper',
			                                            'FirstName': 'A',
			                                            'LastName': 'BCD',
			                                            'PhoneNumber': '0412123123',
			                                            'DateOfBirth': '01/01/1990',
			                                            'CallNoteName': 'whisper.json'
		                                            },
		                                            {
			                                            'Id': 2,
			                                            'UserName': 'wing',
			                                            'FirstName': 'AAA',
			                                            'LastName': 'BBBCD',
			                                            'PhoneNumber': '0444321321',
			                                            'DateOfBirth': '01/01/2000',
			                                            'CallNoteName': 'wing.json'
		                                            }
	                                            ]
                                            }");


            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

        }

        [TestMethod]
        public void WriteJsonFile_NotNullPathAndName_WriteFileSuccessfully()
        {
            JsonFileHelper.WriteJsonFile(_filePath, _fileName, _jObject);

            JObject jObjectRead;

            // read from the json file after write, in order to make comparison
            var fullFileName = Path.Combine(_filePath, _fileName);
            using (StreamReader file = File.OpenText(fullFileName))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObjectRead = (JObject)JToken.ReadFrom(reader);
                }
            }

            Assert.IsTrue(JToken.DeepEquals(_jObject, jObjectRead));
        }

        [TestMethod]
        public void ReadJsonFile_GivenExistPathAndName_ReadCorrectly()
        {
            // Write to the json file before read
            using (StreamWriter file = File.CreateText(Path.Combine(_filePath, _fileName)))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    _jObject.WriteTo(writer);
                }
            }

            var jObjectRead = JsonFileHelper.ReadJsonFile(_filePath, _fileName);

            Assert.IsTrue(JToken.DeepEquals(_jObject, jObjectRead));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void WriteJsonFile_NullPath_ThrowDataAccessException()
        {
            JsonFileHelper.WriteJsonFile(null, _fileName, _jObject);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void WriteJsonFile_NullName_ThrowDataAccessException()
        {
            JsonFileHelper.WriteJsonFile(_filePath, null, _jObject);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void WriteJsonFile_NullPathAndName_ThrowDataAccessException()
        {
            JsonFileHelper.WriteJsonFile(null, null, _jObject);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ReadJsonFile_NullPath_ThrowDataAccessException()
        {
            JsonFileHelper.ReadJsonFile(null, _fileName);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ReadJsonFile_NullName_ThrowDataAccessException()
        {
            JsonFileHelper.ReadJsonFile(_filePath, null);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void ReadJsonFile_NullPathAndName_ThrowDataAccessException()
        {
            JsonFileHelper.ReadJsonFile(null, null);
        }
    }
}
