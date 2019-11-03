using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTierMvcCustomerSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

    }
}