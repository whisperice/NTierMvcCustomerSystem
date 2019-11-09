using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Model
{
    public class CallNote
    {
        public DateTime NoteTime { get; set; }
        public string NoteContent { get; set; }
        public IList<ChildCallNote> ChildCallNotes { get; set; }

        public override string ToString()
        {
            return $"NoteTime: {NoteTime}, NoteContent: {NoteContent}, ChildCallNotes: {ChildCallNotes}";
        }
    }
}
