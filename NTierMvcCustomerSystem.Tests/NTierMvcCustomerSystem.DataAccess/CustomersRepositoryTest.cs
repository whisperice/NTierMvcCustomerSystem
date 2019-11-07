using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess;
using NTierMvcCustomerSystem.DataAccess.Models;
using NTierMvcCustomerSystem.Tests.Common;

namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomerSystem.DataAccess
{
    [TestClass]
    public class CustomersRepositoryTest
    {
        private const string CustomerFileName = TestConstants.CustomerFileName;

        [TestMethod]
        public void SelectByFirstOrLastName_GivenExistLastName_GetCustomerEntity()
        {
            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
            var entities = customersRepository.SelectByFirstOrLastName("BBCD");
            foreach (var customerEntity in entities)
            {

                Console.WriteLine(customerEntity);
            }
        }

        [TestMethod]
        public void Insert_GivenNotExistUserName_InsertSuccessfully()
        {
            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
            var entity = new CustomerEntity
            {
                Id = 3,
                UserName = "water",
                FirstName = "CC",
                LastName = "BC",
                PhoneNumber = "0422159753",
                DateOfBirth = new DateTime(1995, 1, 1),
                CallNoteName = "water.json"
            };

            Assert.IsTrue(customersRepository.Insert(entity));
            
        }


        [TestMethod]
        public void Update_GivenEntityWithMatchedUserNameAndId_UpdateSuccessfully()
        {
            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);
            var entity = new CustomerEntity
            {
                Id = 3,
                UserName = "water",
                FirstName = "ZZZ",
                LastName = "QQQ",
                PhoneNumber = "0499999999",
                DateOfBirth = new DateTime(1991, 10, 10),
                CallNoteName = "water.json"
            };

            Assert.IsTrue(customersRepository.Update(entity));

        }

        [TestMethod]
        public void DeleteById_GivenEntityWithExistId_DeleteSuccessfully()
        {
            var customersRepository = new CustomersRepository(TestConstants.DataSourcePath, CustomerFileName);

            Assert.IsTrue(customersRepository.DeleteById(3));

        }
    }
}
