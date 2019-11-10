//using System;
//using System.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NTierMvcCustomerSystem.BusinessLogic.Services;
//using NTierMvcCustomerSystem.Common;
//using NTierMvcCustomerSystem.Model;
//using NTierMvcCustomerSystem.Tests.Common;
//
//namespace NTierMvcCustomerSystem.Tests.NTierMvcCustomersSystem.BusinessLogic
//{
//    [TestClass]
//    public class CustomersServiceTest
//    {
//        private const string CustomerFileName = TestConstants.CustomerFileName;
//
//        [TestMethod]
//        public void SelectAll_GivenAllGood_GetAllCustomers()
//        {
//            var customersService = new CustomersService( TestConstants.DataSourcePath, CustomerFileName);
//            var customers = customersService.SelectAll();
//            foreach (var customer in customers)
//            {
//                Console.WriteLine(customer);
//            }
//
//            var customer0 = new Customer
//            {
//                Id = 1,
//                UserName = "whisper",
//                FirstName = "A",
//                LastName = "BCD",
//                PhoneNumber = "0412123123",
//                DateOfBirth = DateTime.ParseExact("01/01/1990", Constants.DateOfBirthTimeFormat, null)
//            };
//
//            var customer1 = new Customer
//            {
//                Id = 2,
//                UserName = "wing",
//                FirstName = "AAA",
//                LastName = "BBBCD",
//                PhoneNumber = "0444321321",
//                DateOfBirth = DateTime.ParseExact("01/01/2000", Constants.DateOfBirthTimeFormat, null)
//            };
//
//            Assert.AreEqual(2, customers.Count);
//            Assert.AreEqual(customer0, customers[0]);
//            Assert.AreEqual(customer1, customers[1]);
//        }
//
//    }
//}
