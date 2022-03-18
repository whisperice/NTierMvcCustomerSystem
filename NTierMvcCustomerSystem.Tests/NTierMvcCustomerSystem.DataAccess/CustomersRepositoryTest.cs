using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.DataAccess.Implementation;
using NTierMvcCustomerSystem.DataAccess.Models;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class CustomersRepositoryTest
    {
        private string _filePath;
        private string _fileName;
        private JObject _jObject;
        private CustomersRepository _customersRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _filePath = Path.Combine(TestConstants.DataSourcePath, TestConstants.DataSourcePathSegment);
            _fileName = TestConstants.CustomerFileName;
            _customersRepository = new CustomersRepository(_filePath, _fileName);

            _jObject = JObject.Parse(@"{
                                          'Customers': [
                                            {
                                              'Id': 1,
                                              'UserName': 'whisper',
                                              'FirstName': 'sdfdsf',
                                              'LastName': 'sdfsdfsd',
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
                                            },
                                            {
                                              'Id': 3,
                                              'UserName': 'water',
                                              'FirstName': 'CC',
                                              'LastName': 'BC',
                                              'PhoneNumber': '0422159753',
                                              'DateOfBirth': '01/01/1995',
                                              'CallNoteName': 'water.json'
                                            }
                                          ]
                                        }");

            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            CreateAndWriteCustomersFile();
        }

       

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void Insert_EntityIsNull_ThrowDataAccessException()
        {
            _customersRepository.Insert(null);
        }

        [TestMethod]
        public void Insert_EntityHasExistingUserName_ReturnFalse()
        {
            Assert.IsFalse(_customersRepository.Insert(new CustomerEntity
            {
                Id = 1000,
                UserName = "whisper",
                FirstName = "AAA",
                LastName = "BBB",
                PhoneNumber = "0411222333",
                DateOfBirth = "02/12/1999",
                CallNoteName = "whisper.json"
            }));
        }

        [TestMethod]
        public void Insert_EntityHasNotExistingUserName_ReturnTrue()
        {
            Assert.IsTrue(_customersRepository.Insert(new CustomerEntity
            {
                Id = 1000,
                UserName = "iceberg",
                FirstName = "AAA",
                LastName = "BBB",
                PhoneNumber = "0411222333",
                DateOfBirth = "02/12/1999",
                CallNoteName = "iceberg.json"
            }));
        }
        
        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void Insert_CustomerFileNotFound_ThrowDataAccessException()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            _customersRepository.Insert(new CustomerEntity
            {
                Id = 1000,
                UserName = "whisper",
                FirstName = "AAA",
                LastName = "BBB",
                PhoneNumber = "0411222333",
                DateOfBirth = "02/12/1999",
                CallNoteName = "whisper.json"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void Update_EntityIsNull_ThrowDataAccessException()
        {
            _customersRepository.Update(null);
        }

        [TestMethod]
        public void Update_EntityHasUnmatchedIdAndUserName_ReturnFalse()
        {
            Assert.IsFalse(_customersRepository.Update(new CustomerEntity
            {
                Id = 1000,
                UserName = "whisper",
                FirstName = "AAA",
                LastName = "BBB",
                PhoneNumber = "0411222333",
                DateOfBirth = "02/12/1999",
                CallNoteName = "whisper.json"
            }));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void Update_CustomerFileNotFound_ThrowDataAccessException()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            _customersRepository.Update(new CustomerEntity
            {
                Id = 1,
                UserName = "whisper",
                FirstName = "AAA",
                LastName = "BBBAAA",
                PhoneNumber = "0411442333",
                DateOfBirth = "02/11/1999",
                CallNoteName = "whisper.json"
            });
        }

        [TestMethod]
        public void Update_EntityHasMatchedIdAndUserName_ReturnTrue()
        {
            Assert.IsTrue(_customersRepository.Update(new CustomerEntity
            {
                Id = 1,
                UserName = "whisper",
                FirstName = "AAA",
                LastName = "BBBAAA",
                PhoneNumber = "0411442333",
                DateOfBirth = "02/11/1999",
                CallNoteName = "whisper.json"
            }));
        }


        [TestMethod]
        public void DeleteById_EntityWithNotValidId_ReturnFalse()
        {
            Assert.IsFalse(_customersRepository.DeleteById(200));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void DeleteById_CustomerFileNotFound_ThrowFileNotFoundException()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            _customersRepository.DeleteById(1);
        }

        [TestMethod]
        public void DeleteById_EntityWithValidId_ReturnTrue()
        {
            Assert.IsTrue(_customersRepository.DeleteById(2));
        }

        [TestMethod]
        public void SelectById_ValidId_GetCorrectEntity()
        {
            Assert.AreEqual(new CustomerEntity()
            {
                Id = 2,
                UserName = "wing",
                FirstName = "AAA",
                LastName = "BBBCD",
                PhoneNumber = "0444321321",
                DateOfBirth = "01/01/2000",
                CallNoteName = "wing.json"
            }, _customersRepository.SelectById(2));
        }

        [TestMethod]
        public void SelectById_NotValidId_ReturnNull()
        {
            Assert.IsNull(_customersRepository.SelectById(200));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void SelectById_CustomerFileNotFound_ThrowDataAccessException()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            _customersRepository.SelectById(1);
        }



        [TestMethod]
        public void SelectByUserName_ExistingUserName_GetCorrectEntity()
        {
            Assert.AreEqual(new CustomerEntity()
            {
                Id = 2,
                UserName = "wing",
                FirstName = "AAA",
                LastName = "BBBCD",
                PhoneNumber = "0444321321",
                DateOfBirth = "01/01/2000",
                CallNoteName = "wing.json"
            }, _customersRepository.SelectByUserName("wing"));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void SelectByUserName_NullUserName_ThrowDataAccessException()
        {
            Assert.IsNull(_customersRepository.SelectByUserName(null));
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void SelectByUserName_CustomerFileNotFound_ThrowDataAccessException()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            _customersRepository.SelectByUserName("wing");
        }


        [TestMethod]
        public void SelectAll_AllGood_GetCorrectEntities()
        {
            IList<CustomerEntity> expectedEntities = new List<CustomerEntity>
            {
                new CustomerEntity
                {
                    Id = 1,
                    UserName = "whisper",
                    FirstName = "sdfdsf",
                    LastName = "sdfsdfsd",
                    PhoneNumber = "0444444444",
                    DateOfBirth = "09/11/2016",
                    CallNoteName = "whisper.json"
                },
                new CustomerEntity
                {
                    Id = 2,
                    UserName = "wing",
                    FirstName = "AAA",
                    LastName = "BBBCD",
                    PhoneNumber = "0444321321",
                    DateOfBirth = "01/01/2000",
                    CallNoteName = "wing.json"
                },
                new CustomerEntity
                {
                    Id = 3,
                    UserName = "water",
                    FirstName = "CC",
                    LastName = "BC",
                    PhoneNumber = "0422159753",
                    DateOfBirth = "01/01/1995",
                    CallNoteName = "water.json"
                }
            };

            var customerEntities = _customersRepository.SelectAll();

            Assert.IsTrue(Utilities.EqualsAll(expectedEntities, customerEntities));
        }

        [TestMethod]
        public void SelectAll_CustomerFileNotFound_FileCreatedAndReturnEmptyList()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            var customerEntities = _customersRepository.SelectAll();

            Assert.IsTrue(File.Exists(Path.Combine(_filePath, _fileName)));
            Assert.IsNotNull(customerEntities);
            Assert.IsTrue(customerEntities.Count == 0);
        }


        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void SelectByFirstOrLastName_NullName_ThrowDataAccessException()
        {
            _customersRepository.SelectByFirstOrLastName(null);
        }

        [TestMethod]
        [ExpectedException(typeof(DataAccessException))]
        public void SelectByFirstOrLastName_CustomerFileNotFound_ThrowDataAccessException()
        {
            Directory.CreateDirectory(_filePath);
            File.Delete(Path.Combine(_filePath, _fileName));

            _customersRepository.SelectByFirstOrLastName("BBBCD");
        }

        [TestMethod]
        public void SelectByFirstOrLastName_ExistingName_GetCorrectEntities()
        {
            var customerEntity1 = new CustomerEntity
            {
                Id = 1,
                UserName = "whisper",
                FirstName = "sdfdsf",
                LastName = "sdfsdfsd",
                PhoneNumber = "0444444444",
                DateOfBirth = "09/11/2016",
                CallNoteName = "whisper.json"
            };

            var customerEntity2 = new CustomerEntity
            {
                Id = 2,
                UserName = "wing",
                FirstName = "AAA",
                LastName = "BBBCD",
                PhoneNumber = "0444321321",
                DateOfBirth = "01/01/2000",
                CallNoteName = "wing.json"
            };

            var customerEntity3 = new CustomerEntity
            {
                Id = 3,
                UserName = "water",
                FirstName = "CC",
                LastName = "BC",
                PhoneNumber = "0422159753",
                DateOfBirth = "01/01/1995",
                CallNoteName = "water.json"
            };

            var entities1 = _customersRepository.SelectByFirstOrLastName("AAA");
            var entities2 = _customersRepository.SelectByFirstOrLastName("A");
            var entities3 = _customersRepository.SelectByFirstOrLastName("BBBCD");
            var entities4 = _customersRepository.SelectByFirstOrLastName("BC");
            var entities5 = _customersRepository.SelectByFirstOrLastName("C");
            var entities6 = _customersRepository.SelectByFirstOrLastName("");

            var expectedEntities1 = new List<CustomerEntity> {customerEntity2};
            var expectedEntities2 = new List<CustomerEntity> { customerEntity2 };
            var expectedEntities3 = new List<CustomerEntity> { customerEntity2 };
            var expectedEntities4 = new List<CustomerEntity> { customerEntity2, customerEntity3 };
            var expectedEntities5 = new List<CustomerEntity> { customerEntity2, customerEntity3 };
            var expectedEntities6 = new List<CustomerEntity> { customerEntity1, customerEntity2, customerEntity3 };

            Assert.IsTrue(Utilities.EqualsAll(expectedEntities1, entities1));
            Assert.IsTrue(Utilities.EqualsAll(expectedEntities2, entities2));
            Assert.IsTrue(Utilities.EqualsAll(expectedEntities3, entities3));
            Assert.IsTrue(Utilities.EqualsAll(expectedEntities4, entities4));
            Assert.IsTrue(Utilities.EqualsAll(expectedEntities5, entities5));
            Assert.IsTrue(Utilities.EqualsAll(expectedEntities6, entities6));
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
    }
}
