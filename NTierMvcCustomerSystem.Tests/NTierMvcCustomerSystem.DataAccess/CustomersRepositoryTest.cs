//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NTierMvcCustomerSystem.Common;
//using NTierMvcCustomerSystem.DataAccess.Implementation;
//using NTierMvcCustomerSystem.DataAccess.Models;
//using NTierMvcCustomerSystem.Tests.Common;
//
//namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
//{
//    [TestClass]
//    public class CustomersRepositoryTest
//    {
//        private const string CustomerFileName = TestConstants.CustomerFileName;
//
//        [TestMethod]
//        public void SelectByFirstOrLastName_GivenExistLastName_GetCustomerEntity()
//        {
//            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
//            var entities = customersRepository.SelectByFirstOrLastName("BB");
//            foreach (var customerEntity in entities)
//            {
//                Console.WriteLine(customerEntity);
//            }
//        }
//
//        [TestMethod]
//        public void SelectByUserName_GivenExistUserName_GetCustomerEntity()
//        {
//            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
//            var entity = customersRepository.SelectByUserName("whisper");
//            Console.WriteLine(entity);
//        }
//
//        [TestMethod]
//        public void Insert_GivenNotExistUserName_InsertSuccessfully()
//        {
//            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
//            var entity = new CustomerEntity
//            {
//                Id = 3,
//                UserName = "water",
//                FirstName = "CC",
//                LastName = "BC",
//                PhoneNumber = "0422159753",
//                DateOfBirth = "01/01/1995",
//                CallNoteName = "water.json"
//            };
//
//            Assert.IsTrue(customersRepository.Insert(entity));
//            
//        }
//
//
//        [TestMethod]
//        public void Update_GivenEntityWithMatchedUserNameAndId_UpdateSuccessfully()
//        {
//            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
//            var entity = new CustomerEntity
//            {
//                Id = 3,
//                UserName = "water",
//                FirstName = "ZZZ",
//                LastName = "QQQ",
//                PhoneNumber = "0499999999",
//                DateOfBirth = "10/10/1990",
//                CallNoteName = "water.json"
//            };
//
//            Assert.IsTrue(customersRepository.Update(entity));
//
//        }
//
//        [TestMethod]
//        public void DeleteById_GivenEntityWithExistId_DeleteSuccessfully()
//        {
//            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
//
//            Assert.IsTrue(customersRepository.DeleteById(3));
//
//        }
//    }
//}
