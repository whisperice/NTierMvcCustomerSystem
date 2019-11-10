using System;
using System.ComponentModel.DataAnnotations;

namespace NTierMvcCustomerSystem.Model
{
    public class ChildCallNote
    {
        [Display(Name = "Note Time")]
        public DateTime NoteTime { get; set; }

        [Required]
        [Display(Name = "Child Note Content")]
        public string NoteContent { get; set; }

        public override string ToString()
        {
            return $"DateTime: {NoteTime}, NoteContent: {NoteContent}";
        }

        protected bool Equals(ChildCallNote other)
        {
            return NoteTime.Equals(other.NoteTime) && NoteContent == other.NoteContent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChildCallNote) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (NoteTime.GetHashCode() * 397) ^ (NoteContent != null ? NoteContent.GetHashCode() : 0);
            }
        }
    }
}