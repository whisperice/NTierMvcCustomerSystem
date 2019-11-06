using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTierMvcCustomerSystem.Model;

namespace NTierMvcCustomerSystem.DataAccess.Models
{
    public class CustomerEntity : Customer
    {
        public string CallNoteName { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, CallNoteName: {CallNoteName}";
        }
    }
}
