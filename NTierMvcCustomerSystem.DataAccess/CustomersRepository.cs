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
        private const string CustomersFileName = "Customers.json";
        private static string _customersFilePath = ConfigurationHandler.GetDataSourcePath();
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public bool Insert(CustomerEntity entity)
        {
            return false;
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
            return null;
        }

        public IList<CustomerEntity> SelectAll()
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("[CustomersRepository::SelectAll] Starting select all costomerEntity.");
            }

            try
            {
                var jObject = JsonFileHandler.ReadJsonFile(_customersFilePath, CustomersFileName);

                var customerEntities = jObject["Customers"].Select(convertToCustomerEntity).ToList();

                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("[CustomersRepository::SelectAll] Select all costomerEntity Successfully.");
                }
                return customerEntities;
            }
            catch (Exception e)
            {
                _logger.Error(e, "[CustomersRepository::SelectAll] Select all customerEntity from file failed.");
                throw new DataAccessException("[CustomersRepository::SelectAll] Select all customerEntity from file failed.", e);
            }

        }

        public List<CustomerEntity> SelectByKey(string key)
        {
            return null;
        }

        /// <summary>
        /// Convert a JToken of CustomerEntity Class to a CustomerEntity
        /// </summary>
        /// <param name="customerEntityJObject">Must be a JToken of CustomerEntity Class</param>
        /// <returns>A CustomerEntity Object converted from JToken</returns>
        private CustomerEntity convertToCustomerEntity(JToken customerEntityJObject)
        {
            return null;
        }
    }
}
