using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTierMvcCustomerSystem.BusinessLogic.Common;
using NTierMvcCustomerSystem.BusinessLogic.Interface;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Implementation;
using NTierMvcCustomerSystem.DataAccess.Interface;
using NTierMvcCustomerSystem.DataAccess.Models;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.BusinessLogic.Implementation
{
    public class CustomersService : IModelService<Customer>
    {
        private string _customersFileName;
        private string _customersFilePath;
        private IRepository<CustomerEntity> _customersRepository = new CustomersRepository();
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public CustomersService()
        {

        }

        public CustomersService(string customersFileName, string customersFilePath)
        {
            _customersFileName = customersFileName;
            _customersFilePath = customersFilePath;
        }

        public bool Insert(Customer entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(Customer entity)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Customer SelectById(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> SelectAll()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersService::SelectAll] Starting select all customers.");
            }

            try
            {
                var customerEntities = _customersRepository.SelectAll();
                var customers = customerEntities.Select(ToCustomer).ToList();

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersService::SelectAll] Select all customers successfully. Customers: {0}", customers);
                }
                return customers != null ? customers : new List<Customer>();
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersService::SelectAll] Select all customers failed.");
                throw new BusinessLogicException("[CustomersService::SelectAll] Select all customers failed.", e);
            }
        }

        private CustomerEntity ToCustomerEntity(Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            var customerEntity = new CustomerEntity
            {
                Id = customer.Id,
                UserName = customer.UserName,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth.ToString(Constants.DateOfBirthTimeFormat),
                PhoneNumber = customer.PhoneNumber,
                CallNoteName = customer.UserName + Constants.PostfixOfNoteFile
            };

            return customerEntity;
        }

        private Customer ToCustomer(CustomerEntity customerEntity)
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
    }
}
