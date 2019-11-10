using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTierMvcCustomerSystem.Common;

namespace NTierMvcCustomerSystem.Model
{
    public class CallNote
    {
        [Display(Name = "Note Time")]
        public DateTime NoteTime { get; set; }

        [Required]
        [Display(Name = "Note Content")]
        public string NoteContent { get; set; }

        public IList<ChildCallNote> ChildCallNotes { get; set; }

        public override string ToString()
        {
            return $"NoteTime: {NoteTime}, NoteContent: {NoteContent}, ChildCallNotes: {ChildCallNotes}";
        }

        protected bool Equals(CallNote other)
        {
            return NoteTime.Equals(other.NoteTime) && NoteContent == other.NoteContent && Utilities.EqualsAll(ChildCallNotes, other.ChildCallNotes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CallNote) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = NoteTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (NoteContent != null ? NoteContent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ChildCallNotes != null ? Utilities.GetHashCode(ChildCallNotes) : 0);
                return hashCode;
            }
        }
    }
}
