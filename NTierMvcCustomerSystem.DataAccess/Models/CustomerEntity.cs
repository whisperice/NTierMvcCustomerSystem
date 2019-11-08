using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.DataAccess.Models
{
    public class CustomerEntity
    {

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string CallNoteName { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, UserName: {UserName}, FirstName: {FirstName}, LastName: {LastName}, PhoneNumber: {PhoneNumber}, DateOfBirth: {DateOfBirth}, CallNoteName: {CallNoteName}";
        }

        protected bool Equals(CustomerEntity other)
        {
            return Id == other.Id && UserName == other.UserName && FirstName == other.FirstName && LastName == other.LastName && PhoneNumber == other.PhoneNumber && DateOfBirth == other.DateOfBirth && CallNoteName == other.CallNoteName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CustomerEntity) obj);
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
                hashCode = (hashCode * 397) ^ (DateOfBirth != null ? DateOfBirth.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CallNoteName != null ? CallNoteName.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
