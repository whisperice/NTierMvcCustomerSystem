using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class JsonFileHandlerTest
    {
        private const string CustomerFileName= "CustomersTest.json";

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

            var path = ConfigurationHandler.GetDataSourcePath();
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
			                                            'CallNoteFilePathAndName': 'E:\\文档\\whisper.json'
		                                            },
		                                            {
			                                            'Id': 2,
			                                            'UserName': 'wing',
			                                            'FirstName': 'AAA',
			                                            'LastName': 'BBBCD',
			                                            'PhoneNumber': '0444321321',
			                                            'DateOfBirth': '01/01/2000',
			                                            'CallNoteFilePathAndName': 'E:\\文档\\wing.json'
		                                            }
	                                            ]
                                            }");

            var dataSourcePath = ConfigurationHandler.GetDataSourcePath();
            var jObjectRead = JsonFileHandler.ReadJsonFile(dataSourcePath, CustomerFileName);

            Console.WriteLine(jObject["Customers"][0].GetType());
            Assert.IsTrue(JObject.DeepEquals(jObject, jObjectRead));
        }
    }
}
