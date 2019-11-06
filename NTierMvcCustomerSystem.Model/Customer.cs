using System;
using NTierMvcCustomerSystem.Common;

namespace NTierMvcCustomerSystem.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, UserName: {UserName}, FirstName: {FirstName}, LastName: {LastName}, PhoneNumber: {PhoneNumber}, DateOfBirth: {DateOfBirth.ToString(Constants.DateOfBirthTimeFormat)}";
        }
    }
}