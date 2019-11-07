using System;

namespace NTierMvcCustomerSystem.Model
{
    public class ChildCallNote
    {
        public DateTime DateTime { get; set; }
        public string NoteContent { get; set; }

        public override string ToString()
        {
            return $"DateTime: {DateTime}, NoteContent: {NoteContent}";
        }
    }
}