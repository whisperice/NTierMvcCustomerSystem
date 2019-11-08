using System;
using System.ComponentModel.DataAnnotations;
using NTierMvcCustomerSystem.Common;
using NTierMvcCustomerSystem.Model.Validators;

namespace NTierMvcCustomerSystem.Model
{
    public class Customer
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9]{5,20}$", ErrorMessage = "Must be 5-20 characters")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "No numbers")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage ="No numbers")]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^(?:\+?61|0)[2-478](?:[ -]?[0-9]){8}$", ErrorMessage = "Only valid Australia landline or mobile number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Date of Birth is required")]
        [AgeLimit(ErrorMessage = "Age can not larger than 110")]
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, UserName: {UserName}, FirstName: {FirstName}, LastName: {LastName}, PhoneNumber: {PhoneNumber}, DateOfBirth: {DateOfBirth.ToString(Constants.DateOfBirthTimeFormat)}";
        }

        protected bool Equals(Customer other)
        {
            return Id == other.Id && UserName == other.UserName && FirstName == other.FirstName && LastName == other.LastName && PhoneNumber == other.PhoneNumber && DateOfBirth.Equals(other.DateOfBirth);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Customer) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (UserName != null ? UserName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (PhoneNumber != null ? PhoneNumber.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ DateOfBirth.GetHashCode();
                return hashCode;
            }
        }
    }
}