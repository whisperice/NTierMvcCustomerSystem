using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.BusinessLogic.Common;
using NTierMvcCustomerSystem.BusinessLogic.Services;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Models;
using NTierMvcCustomerSystem.Model;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomersSystem.BusinessLogic
{
    [TestClass]
    public class CustomersServiceTest
    {
        private int _notExistingId;
        private int _notValidId;

        private string _filePath;
        private string _fileName;

        private string _callNoteFilePath;

        private CustomersService _customersService;

        private Customer _customer1;
        private Customer _customer2;
        private Customer _customer3;
        private List<Customer> _customers;

        private JObject _jObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _notExistingId = 10000000;
            _notValidId = Constants.MinValidCustomerId - 1;

            _filePath = Path.Combine(TestConstants.DataSourcePath, TestConstants.DataSourcePathSegment);
            _fileName = TestConstants.CustomerFileName;

            _callNoteFilePath = Path.Combine(_filePath, TestConstants.CallNoteContentFolderName);

            _customersService = new CustomersService(_filePath, _fileName);

            _customer1 = new Customer
            {
                Id = 1,
                UserName = "whisper",
                FirstName = "CC",
                LastName = "BC",
                PhoneNumber = "0444444444",
                DateOfBirth = DateTime.ParseExact("09/11/2016", Constants.DateOfBirthTimeFormat, null)
            };

            _customer2 = new Customer
            {
                Id = 2,
                UserName = "wing",
                FirstName = "AAA",
                LastName = "BBBCD",
                PhoneNumber = "0444321321",
                DateOfBirth = DateTime.ParseExact("01/01/2000", Constants.DateOfBirthTimeFormat, null)
            };

            _customer3 = new Customer
            {
                Id = 3,
                UserName = "water",
                FirstName = "CCDDD",
                LastName = "BCDDD",
                PhoneNumber = "0422159753",
                DateOfBirth = DateTime.ParseExact("01/01/1995", Constants.DateOfBirthTimeFormat, null)
            };

            _customers = new List<Customer> { _customer1, _customer2 };

            _jObject = JObject.Parse(@"{
                                          'Customers': [
                                            {
                                              'Id': 1,
                                              'UserName': 'whisper',
                                              'FirstName': 'CC',
                                              'LastName': 'BC',
                                              'PhoneNumber': '0444444444',
                                              'DateOfBirth': '09/11/2016',
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

            Directory.CreateDirectory(_callNoteFilePath);
            Directory.Delete(_callNoteFilePath, true);

            File.Delete(Path.Combine(_filePath, _fileName));
            CreateAndWriteCustomersFile();

            Directory.CreateDirectory(_callNoteFilePath);
            CreateCallNotesFile(_customer1.UserName);
            CreateCallNotesFile(_customer2.UserName);
        }



        [TestMethod]
        public void Insert_ValidCustomer_InsertCorrectlyAndReturnTrueAndCallNotesFileCreated()
        {
            var isInserted = _customersService.Insert(_customer3, out var id);
            Assert.IsTrue(isInserted);

            _customer3.Id = id;
            _customers.Add(_customer3);

            var customers = ReadFromCustomersFile();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));

            Assert.IsTrue(CheckCallNotesFileExist(_customer3.UserName));
        }


        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Insert_NullCustomer_ThrowBusinessLogicException()
        {
            _customersService.Insert(null, out var id);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Insert_NullUserName_ThrowBusinessLogicException()
        {
            _customer3.UserName = null;
            _customersService.Insert(_customer3, out var id);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Insert_TooLargeAge_ThrowBusinessLogicException()
        {
            _customer3.DateOfBirth = DateTime.ParseExact("01/01/1800", Constants.DateOfBirthTimeFormat, null);
            _customersService.Insert(_customer3, out var id);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Insert_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _customersService.Insert(_customer3, out var id);
        }

        [TestMethod]
        public void Insert_ExistingCustomerName_NotInsertAndReturnFalse()
        {
            var isInserted = _customersService.Insert(_customer2, out var id);
            Assert.IsFalse(isInserted);

            var customers = ReadFromCustomersFile();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));
        }



        [TestMethod]
        public void Update_ValidCustomer_UpdateCorrectlyAndReturnTrue()
        {
            _customer2.FirstName = "gfd";
            _customer2.DateOfBirth = DateTime.ParseExact("01/01/1990", Constants.DateOfBirthTimeFormat, null);
            var isUpdated = _customersService.Update(_customer2);
            Assert.IsTrue(isUpdated);

            var customers = ReadFromCustomersFile();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));
        }


        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Update_NullCustomer_ThrowBusinessLogicException()
        {
            _customersService.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Update_NullUserName_ThrowBusinessLogicException()
        {
            _customer2.UserName = null;
            _customersService.Update(_customer2);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Update_TooLargeAge_ThrowBusinessLogicException()
        {
            _customer2.DateOfBirth = DateTime.ParseExact("01/01/1800", Constants.DateOfBirthTimeFormat, null);
            _customersService.Update(_customer2);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void Update_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _customersService.Update(_customer2);
        }

        [TestMethod]
        public void Update_IdAndUserNameNotMatched_NotUpdatedAndReturnFalse()
        {
            var tempId = _customer2.Id;
            _customer2.Id = _notExistingId;
            var isUpdated = _customersService.Update(_customer2);
            Assert.IsFalse(isUpdated);

            _customer2.Id = tempId;

            var customers = ReadFromCustomersFile();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));
        }



        [TestMethod]
        public void DeleteById_ExistingId_DeleteCorrectlyAndReturnTrueAndCallNotesFileDeleted()
        {
            var isDeleted = _customersService.DeleteById(_customer2.Id);
            Assert.IsTrue(isDeleted);

            _customers.RemoveAt(_customers.Count - 1);

            var customers = ReadFromCustomersFile();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));

            Assert.IsFalse(CheckCallNotesFileExist(_customer2.UserName));
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void DeleteById_SmallerThanMinValidId_ThrowBusinessLogicException
            ()
        {
            _customersService.DeleteById(_notValidId);
        }

        [TestMethod]
        public void DeleteById_NotExistingId_NotDeleteAndReturnFalseAndNoCallNotesFileDeleted()
        {
            var isDeleted = _customersService.DeleteById(_notExistingId);
            Assert.IsFalse(isDeleted);

            var customers = ReadFromCustomersFile();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));

            Assert.IsTrue(CheckCallNotesFileExist(_customer1.UserName));
            Assert.IsTrue(CheckCallNotesFileExist(_customer2.UserName));
        }



        [TestMethod]
        public void SelectById_ExistingId_SelectCorrectly()
        {
            var customer = _customersService.SelectById(_customer2.Id);
            Assert.IsNotNull(customer);
            Assert.AreEqual(_customer2, customer);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void SelectById_SmallerThanMinValidId_ThrowBusinessLogicException()
        {
            _customersService.SelectById(_notValidId);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void SelectById_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _customersService.SelectById(_customer2.Id);
        }

        [TestMethod]
        public void SelectById_NotExistingId_ReturnNull()
        {
            var customer = _customersService.SelectById(_notExistingId);
            Assert.IsNull(customer);
        }




        [TestMethod]
        public void SelectByFirstOrLastName_NotNullName_SelectCorrectly()
        {
            var customers1 = _customersService.SelectByFirstOrLastName("D");
            var customers2 = _customersService.SelectByFirstOrLastName("C");
            var customers3 = _customersService.SelectByFirstOrLastName("BC");
            var customers4 = _customersService.SelectByFirstOrLastName("CC");
            var customers5 = _customersService.SelectByFirstOrLastName("");
            var customers6 = _customersService.SelectByFirstOrLastName("AAAAAAAAA");

            var expectedCustomers1 = new List<Customer> { _customer2 };
            var expectedCustomers2 = new List<Customer> { _customer1, _customer2 };
            var expectedCustomers3 = new List<Customer> { _customer1, _customer2 };
            var expectedCustomers4 = new List<Customer> { _customer1 };
            var expectedCustomers5 = new List<Customer> { _customer1, _customer2 };
            var expectedCustomers6 = new List<Customer>();

            Assert.IsTrue(Utilities.EqualsAll(expectedCustomers1, customers1));
            Assert.IsTrue(Utilities.EqualsAll(expectedCustomers2, customers2));
            Assert.IsTrue(Utilities.EqualsAll(expectedCustomers3, customers3));
            Assert.IsTrue(Utilities.EqualsAll(expectedCustomers4, customers4));
            Assert.IsTrue(Utilities.EqualsAll(expectedCustomers5, customers5));
            Assert.IsTrue(Utilities.EqualsAll(expectedCustomers6, customers6));
        }
        
        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void SelectByFirstOrLastName_NullName_ThrowBusinessLogicException()
        {
            _customersService.SelectByFirstOrLastName(null);
        }

        [TestMethod]
        [ExpectedException(typeof(BusinessLogicException))]
        public void SelectByFirstOrLastName_CustomersFileNotFound_ThrowBusinessLogicException()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _customersService.SelectByFirstOrLastName("A");
        }



        [TestMethod]
        public void SelectAll_CustomersFileFound_GetAllCustomers()
        {
            var customers = _customersService.SelectAll();
            Assert.IsTrue(Utilities.EqualsAll(_customers, customers));
        }

        [TestMethod]
        public void SelectAll_CustomersFileNotFound_CreateCustomersFileAndGetEmptyList()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            var customers = _customersService.SelectAll();

            Assert.IsNotNull(customers);
            Assert.IsTrue(customers.Count == 0);
        }

        [TestMethod]
        public void SelectAll_NoUserInCustomersFile_GetEmptyList()
        {
            File.Delete(Path.Combine(_filePath, _fileName));
            _jObject = JObject.Parse(@"{
                                          'Customers': [
                                          ]
                                        }");
            CreateAndWriteCustomersFile();

            var customers = _customersService.SelectAll();

            Assert.IsNotNull(customers);
            Assert.IsTrue(customers.Count == 0);
        }



        private IList<Customer> ReadFromCustomersFile()
        {
            JObject jObject;

            using (StreamReader file = File.OpenText(Path.Combine(_filePath, _fileName)))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObject = (JObject)JToken.ReadFrom(reader);
                }
            }

            var customerEntities = jObject["Customers"].Select(JTokenToCustomerEntity).ToList();
            var customers = customerEntities.Select(ToCustomer).ToList();
            return customers;
        }

        private void CreateAndWriteCustomersFile()
        {
            using (StreamWriter file = File.CreateText(Path.Combine(_filePath, _fileName)))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    _jObject.WriteTo(writer);
                }
            }
        }

        private static Customer ToCustomer(CustomerEntity customerEntity)
        {
            if (customerEntity == null)
            {
                return null;
            }

            var customer = new Customer
            {
                Id = customerEntity.Id,
                UserName = customerEntity.UserName,
                FirstName = customerEntity.FirstName,
                LastName = customerEntity.LastName,
                DateOfBirth = DateTime.ParseExact(customerEntity.DateOfBirth, Constants.DateOfBirthTimeFormat, null),
                PhoneNumber = customerEntity.PhoneNumber
            };

            return customer;
        }

        private static CustomerEntity JTokenToCustomerEntity(JToken customerEntityJObject)
        {
            if (customerEntityJObject == null)
            {
                return null;
            }

            var customerEntity = new CustomerEntity
            {
                Id = (int)customerEntityJObject["Id"],
                UserName = (string)customerEntityJObject["UserName"],
                FirstName = (string)customerEntityJObject["FirstName"],
                LastName = (string)customerEntityJObject["LastName"],
                PhoneNumber = (string)customerEntityJObject["PhoneNumber"],
                DateOfBirth = ((string)customerEntityJObject["DateOfBirth"]),
                CallNoteName = (string)customerEntityJObject["CallNoteName"]
            };
            return customerEntity;
        }

        private bool CheckCallNotesFileExist(string userName)
        {
            return File.Exists(Path.Combine(_callNoteFilePath, userName + Constants.PostfixOfNoteFile));
        }


        private void CreateCallNotesFile(string userName)
        {
            var jArray = new JArray();
            var jObject = new JObject { ["CallNotes"] = jArray };

            Directory.CreateDirectory(_callNoteFilePath);

            var fullFileName = Path.Combine(_callNoteFilePath, userName + Constants.PostfixOfNoteFile);
            using (StreamWriter file = File.CreateText(fullFileName))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    writer.Formatting = Formatting.Indented;
                    jObject.WriteTo(writer);
                }
            }
        }

    }
}
