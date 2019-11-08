using System;
using System.ComponentModel.DataAnnotations;
using NTierMvcCustomerSystem.Common;

namespace NTierMvcCustomerSystem.Model.Validators
{
    public class AgeLimit : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateTime = (DateTime) value;
            return dateTime.AddYears(Constants.AgeLimit) >= DateTime.Today;
        }
    }
}