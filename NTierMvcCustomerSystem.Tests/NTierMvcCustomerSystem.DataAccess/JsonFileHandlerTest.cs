using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class JsonFileHandlerTest
    {
        private const string CustomerFileName= TestConstants.CustomerFileName;

        [TestMethod]
        public void WriteJsonFile_GivenExistPathAndName_FileWriteSuccessfully()
        {
            JObject jObject = JObject.Parse(@"{
                                          'Stores': [
                                            'Lambton Quay',
                                            'Willis Street'
                                          ],
                                          'Manufacturers': [
                                            {
                                              'Name': 'Acme Co',
                                              'Products': [
                                                {
                                                  'Name': 'Anvil',
                                                  'Price': 50
                                                }
                                              ]
                                            },
                                            {
                                              'Name': 'Contoso',
                                              'Products': [
                                                {
                                                  'Name': 'Elbow Grease',
                                                  'Price': 99.95
                                                },
                                                {
                                                  'Name': 'Headlight Fluid',
                                                  'Price': 4
                                                }
                                              ]
                                            }
                                          ]
                                        }");

            var path = TestConstants.DataSourcePath;
            var fileName = CustomerFileName;

            JsonFileHandler.WriteJsonFile(path, fileName, jObject);
        }

        [TestMethod]
        public void ReadJsonFile_GivenExistPathAndName_ReadCorrectly()
        {
            JObject jObject = JObject.Parse(@"{
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

            var dataSourcePath = TestConstants.DataSourcePath;
            var jObjectRead = JsonFileHandler.ReadJsonFile(dataSourcePath, CustomerFileName);

            Assert.IsTrue(JObject.DeepEquals(jObject, jObjectRead));
        }
    }
}
