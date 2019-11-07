using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTierMvcCustomerSystem.Model
{
    public class CallNote
    {
        public DateTime DateTime { get; set; }
        public string NoteContent { get; set; }
        public ChildCallNote ChildCallNote { get; set; }

        public override string ToString()
        {
            return $"DateTime: {DateTime}, NoteContent: {NoteContent}, ChildCallNote: {ChildCallNote}";
        }
    }
}
