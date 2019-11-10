using System;
using System.Collections.Generic;
using System.Linq;
using NTierMvcCustomerSystem.BusinessLogic.Common;
using NTierMvcCustomerSystem.BusinessLogic.Interface;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.DataAccess.Implementation;
using NTierMvcCustomerSystem.DataAccess.Models;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.BusinessLogic.Services
{
    public class CustomersService : IModelService<Customer>
    {
        private readonly string _customersFilePath;

        private readonly CallNotesService _callNotesService;
        private readonly CustomersRepository _customersRepository;
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public CustomersService()
        {
            _customersFilePath = ConfigurationHandler.GetDataSourcePath();
            _callNotesService = new CallNotesService();
            _customersRepository = new CustomersRepository();
        }

        public CustomersService(string customersFilePath, string customersFileName)
        {
            _customersFilePath = customersFilePath;
            _callNotesService = new CallNotesService(customersFilePath, customersFileName);
            _customersRepository = new CustomersRepository(customersFilePath, customersFileName);
        }

        public bool Insert(Customer customer, out int id)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersService::Insert] Starting insert customer. Customer: {}", customer);
            }

            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException(
                        nameof(customer), "[CustomersService::Insert] Customer can not be null.");
                }

                if (string.IsNullOrEmpty(customer.UserName))
                {
                    throw new ArgumentNullException(
                        nameof(customer.UserName), "[CustomersService::Insert] UserName can not be null or empty.");
                }

                if (string.IsNullOrEmpty(customer.FirstName))
                {
                    throw new ArgumentNullException(
                        nameof(customer.FirstName), "[CustomersService::Insert] FirstName can not be null or empty.");
                }

                if (string.IsNullOrEmpty(customer.LastName))
                {
                    throw new ArgumentNullException(
                        nameof(customer.LastName), "[CustomersService::Insert] LastName can not be null or empty.");
                }

                if (string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    throw new ArgumentNullException(
                        nameof(customer.PhoneNumber), "[CustomersService::Insert] PhoneNumber can not be null or empty.");
                }

                if (customer.DateOfBirth.AddYears(Constants.AgeLimit) < DateTime.Today)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(customer.DateOfBirth), $"[CustomersService::Insert] Age can not larger than {Constants.AgeLimit}.");
                }

                // Generate a unique Id for the new customer
                customer.Id = IdSeqGenerator.GetIdSeq();
                var isInserted = _customersRepository.Insert(ToCustomerEntity(customer));

                // Can't insert when there is a same userName
                if (!isInserted)
                {
                    _logger.Warn("[CustomersService::Insert] Can't insert when there is a same userName.");
                    id = Constants.NotExistCustomerId;
                    return false;
                }

                // After creating customer successfully, create Call Note file for the user
                _callNotesService.CreateCallNotesFile(customer.UserName);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersService::Insert] Insert customer Successfully. Inserted Customer: {}", customer);
                }

                id = customer.Id;
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersService::Insert] Insert customer failed. Customer: {0}", customer);
                throw new BusinessLogicException("[CustomersService::Insert] Insert customer failed.", e);
            }
        }

        public bool Update(Customer customer)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::Update] Starting update customer. Customer: {}", customer);
            }

            try
            {
                if (customer == null)
                {
                    throw new ArgumentNullException(
                        nameof(customer), "[CustomersRepository::Update] customer can not be null.");
                }

                if (string.IsNullOrEmpty(customer.UserName))
                {
                    throw new ArgumentNullException(
                        nameof(customer.UserName), "[CustomersService::Update] UserName can not be null or empty.");
                }

                if (string.IsNullOrEmpty(customer.FirstName))
                {
                    throw new ArgumentNullException(
                        nameof(customer.FirstName), "[CustomersService::Update] FirstName can not be null or empty.");
                }

                if (string.IsNullOrEmpty(customer.LastName))
                {
                    throw new ArgumentNullException(
                        nameof(customer.LastName), "[CustomersService::Update] LastName can not be null or empty.");
                }

                if (string.IsNullOrEmpty(customer.PhoneNumber))
                {
                    throw new ArgumentNullException(
                        nameof(customer.PhoneNumber), "[CustomersService::Update] PhoneNumber can not be null or empty.");
                }

                if (customer.DateOfBirth.AddYears(Constants.AgeLimit) < DateTime.Today)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(customer.DateOfBirth), $"[CustomersService::Update] Age can not larger than {Constants.AgeLimit}.");
                }

                var isUpdated = _customersRepository.Update(ToCustomerEntity(customer));

                // Can't update if id and userName are not matched
                if (!isUpdated)
                {
                    _logger.Warn("[CustomersService::Update] Can't update if id and userName are not matched. Customer: {}", customer);
                    return false;
                }

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersService::Update] Update customer Successfully. Updated customer: {0}", customer);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersService::Update] Update customer failed. Entity: {0}", customer);
                throw new BusinessLogicException("[CustomersService::Update] Update customer failed.", e);
            }
        }

        public bool DeleteById(int id)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersService::DeleteById] Starting delete customer. Id: {}", id);
            }

            try
            {
                if (id < Constants.MinValidCustomerId)
                {
                    throw new ArgumentException(
                        "[CustomersService::DeleteById] Id is not valid.", nameof(id));
                }

                // Delete Call Notes file before user deleted
                var customer = ToCustomer(_customersRepository.SelectById(id));
                _callNotesService.DeleteCallNotes(customer.UserName);
                
                var isDeleted = _customersRepository.DeleteById(id);

                // Can't delete when there is no such id
                if (!isDeleted)
                {
                    _logger.Warn("[CustomersService::DeleteById] Can't delete when there is no such id.");
                    return false;
                }


                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersService::DeleteById] Delete customer with id:{} Successfully.", id);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersService::DeleteById] Delete customer failed. Id: {}", id);
                throw new BusinessLogicException("[CustomersService::DeleteById] Delete customer failed.", e);
            }
        }

        public Customer SelectById(int id)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersService::SelectById] Starting select customerEntity by Id. Id : {}", id);
            }

            try
            {
                if (id < Constants.MinValidCustomerId)
                {
                    throw new ArgumentException(
                        "[CustomersService::SelectById] Customer id is valid.", nameof(id));
                }

                var customer = ToCustomer(_customersRepository.SelectById(id));

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersService::SelectById] Select Customer by Id Successfully. customer: {}", customer);
                }
                return customer;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersService::SelectById] Select customer by Id failed. Id : {0}", id);
                throw new BusinessLogicException("[CustomersService::SelectById] Select customer by Id failed.", e);
            }
        }

        public IList<Customer> SelectByFirstOrLastName(string name)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersService::SelectByFirstOrLastName] Starting select customer by Name. Name : {0}", name);
            }

            try
            {
                if (name == null)
                {
                    throw new ArgumentNullException(
                        nameof(name), "[CustomersService::SelectByFirstOrLastName] Name can not be null.");
                }

                var customers = _customersRepository.SelectByFirstOrLastName(name).Select(ToCustomer).ToList();

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRCustomersServiceepository::SelectByFirstOrLastName] Select customer by Name Successfully. Entities: {0}", customers);
                }
                return customers;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersService::SelectByFirstOrLastName] Select customer by Name failed. Name : {0}", name);
                throw new BusinessLogicException("[CustomersService::SelectByFirstOrLastName] Select customer by Name failed.", e);
            }
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
                return customers;
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
