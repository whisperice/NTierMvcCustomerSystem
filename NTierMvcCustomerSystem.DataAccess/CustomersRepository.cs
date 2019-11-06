using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.DataAccess.Common;
using NTierMvcCustomerSystem.DataAccess.Models;

namespace NTierMvcCustomerSystem.DataAccess
{
    public class CustomersRepository : IRepository<CustomerEntity>
    {
        private string _customersFileName;
        private string _customersFilePath;
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public CustomersRepository()
        {
            _customersFileName = Constants.CustomersFileName;
            _customersFilePath = ConfigurationHandler.GetDataSourcePath();

        }

        public CustomersRepository(string customersFilePath, string customersFileName)
        {
            _customersFilePath = customersFilePath;
            _customersFileName = customersFileName;
        }

        public bool Insert(CustomerEntity entity)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::Insert] Starting insert costomerEntity.");
            }

            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(
                        nameof(entity), "[CustomersRepository::Insert] CustomerEntity can not be null.");
                }

                var userName = entity.UserName;
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentNullException(
                        nameof(userName), "[CustomersRepository::Insert] UserName can not be null or empty.");
                }

                var jObjectFromFile = JsonFileHandler.ReadJsonFile(_customersFilePath, _customersFileName);

                var customerEntityJObject = jObjectFromFile["Customers"].FirstOrDefault(c => userName.Equals((string)c["UserName"]));

                // Can't insert when there is a same userName
                if (customerEntityJObject != null)
                {
                    _logger.Debug("[CustomersRepository::Insert] Can't insert when there is a same userName.");
                    return false;
                }

                var jObjectFromEntity = CustomerEntityToJToken(entity);
                jObjectFromFile.Merge(jObjectFromEntity, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
                JsonFileHandler.WriteJsonFile(_customersFilePath, _customersFileName, jObjectFromFile);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRepository::Insert] Insert customerEntity Successfully. InsertedEntity: {0}, IdSeq: {1}",
                                            entity, jObjectFromEntity["IdSeq"]);
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersRepository::Insert] Insert customerEntity failed. Entity: {0}", entity);
                throw new DataAccessException("[CustomersRepository::Insert] Insert customerEntity failed.", e);
            }
        }

        public bool Update(CustomerEntity entity)
        {
            return false;
        }

        public bool DeleteById(int id)
        {
            return false;
        }

        public CustomerEntity SelectById(int id)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::SelectById] Starting select customerEntity by Id. Id : {0}", id);
            }

            try
            {
                var jObject = JsonFileHandler.ReadJsonFile(_customersFilePath, _customersFileName);

                var customerEntityJObject = jObject["Customers"].FirstOrDefault(c => (int)c["Id"] == id);
                var customerEntity = JTokenToCustomerEntity(customerEntityJObject);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRepository::SelectById] Select customerEntity by Id Successfully. Entity: {0}", customerEntity);
                }
                return customerEntity;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersRepository::SelectById] Select customerEntity by Id failed. Id : {0}", id);
                throw new DataAccessException("[CustomersRepository::SelectById] Select customerEntity by Id failed.", e);
            }
        }

        public CustomerEntity SelectByUserName(string userName)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::SelectByUserName] Starting select customerEntity by userName. UserName : {0}", userName);
            }

            try
            {
                if (string.IsNullOrEmpty(userName))
                {
                    throw new ArgumentNullException(
                        nameof(userName), "[CustomersRepository::SelectByUserName] UserName can not be null or empty.");
                }

                var jObject = JsonFileHandler.ReadJsonFile(_customersFilePath, _customersFileName);

                var customerEntityJObject = jObject["Customers"].FirstOrDefault(c => userName.Equals((string)c["UserName"]));
                var customerEntity = JTokenToCustomerEntity(customerEntityJObject);

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRepository::SelectByUserName] Select customerEntity by userName Successfully. Entity: {0}", customerEntity);
                }
                return customerEntity;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersRepository::SelectByUserName] Select customerEntity by userName failed. UserName : {0}", userName);
                throw new DataAccessException("[CustomersRepository::SelectByUserName] Select customerEntity by userName failed.", e);
            }
        }

        public IList<CustomerEntity> SelectAll()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::SelectAll] Starting select all customerEntity.");
            }

            try
            {
                var jObject = JsonFileHandler.ReadJsonFile(_customersFilePath, _customersFileName);

                var customerEntities = jObject["Customers"].Select(JTokenToCustomerEntity).ToList();

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRepository::SelectAll] Select all customerEntity Successfully. Entities: {0}", customerEntities);
                }
                return customerEntities != null ? customerEntities : new List<CustomerEntity>();
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersRepository::SelectAll] Select all customerEntity from file failed.");
                throw new DataAccessException("[CustomersRepository::SelectAll] Select all customerEntity from file failed.", e);
            }
        }

        public IList<CustomerEntity> SelectByFirstOrLastName(string name)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::SelectByFirstOrLastName] Starting select customerEntity by Name. Name : {0}", name);
            }

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException(
                        nameof(name), "[CustomersRepository::SelectByFirstOrLastName] Name can not be null or empty.");
                }

                var jObject = JsonFileHandler.ReadJsonFile(_customersFilePath, _customersFileName);

                var customerEntities = jObject["Customers"]
                                        .Where(c => ((string)c["FirstName"]).Contains(name) || ((string)c["LastName"]).Contains(name))
                                        .Select(JTokenToCustomerEntity).ToList();

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRepository::SelectByFirstOrLastName] Select customerEntity by Name Successfully. Entities: {0}", customerEntities);
                }
                return customerEntities != null ? customerEntities : new List<CustomerEntity>();
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersRepository::SelectByFirstOrLastName] Select customerEntity by Name failed. Name : {0}", name);
                throw new DataAccessException("[CustomersRepository::SelectByFirstOrLastName] Select customerEntity by Name failed.", e);
            }
        }

        /// <summary>
        ///     Convert a JToken of CustomerEntity Class to a CustomerEntity
        /// </summary>
        /// <param name="customerEntityJObject">Must be a JToken of CustomerEntity Class</param>
        /// <returns>If input parameter is not null, return A CustomerEntity Object converted from JToken
        ///         or a CustomerEntity with NotExistCustomerId if input parameter is null</returns>
        private static CustomerEntity JTokenToCustomerEntity(JToken customerEntityJObject)
        {
            if (customerEntityJObject == null)
            {
                return new CustomerEntity { Id = Constants.NotExistCustomerId };
            }

            var customerEntity = new CustomerEntity();
            customerEntity.Id = (int)customerEntityJObject["Id"];
            customerEntity.UserName = (string)customerEntityJObject["UserName"];
            customerEntity.FirstName = (string)customerEntityJObject["FirstName"];
            customerEntity.LastName = (string)customerEntityJObject["LastName"];
            customerEntity.PhoneNumber = (string)customerEntityJObject["PhoneNumber"];
            var dateStrings = ((string)customerEntityJObject["DateOfBirth"]).Split('/');
            customerEntity.DateOfBirth = new DateTime(int.Parse(dateStrings[2]), int.Parse(dateStrings[1]), int.Parse(dateStrings[0]));
            customerEntity.CallNoteName = (string)customerEntityJObject["CallNoteName"];
            return customerEntity;
        }

        private static JToken CustomerEntityToJToken(CustomerEntity customerEntity)
        {
            var jArray = new JArray();
            var jObject = new JObject { ["Customers"] = jArray };

            if (customerEntity == null)
            {
                return jObject;
            }

            var o = new JObject
            {
                ["Id"] = customerEntity.Id,
                ["UserName"] = customerEntity.UserName,
                ["FirstName"] = customerEntity.FirstName,
                ["LastName"] = customerEntity.LastName,
                ["PhoneNumber"] = customerEntity.PhoneNumber,
                ["DateOfBirth"] = customerEntity.DateOfBirth.ToString(Constants.DateOfBirthTimeFormat),
                ["CallNoteName"] = customerEntity.CallNoteName
            };
            jArray.Add(o);
            return jObject;
        }
    }
}
