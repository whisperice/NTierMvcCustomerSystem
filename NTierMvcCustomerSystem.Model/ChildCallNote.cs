using System;

namespace NTierMvcCustomerSystem.Model
{
    public class ChildCallNote
    {
        public DateTime NoteTime { get; set; }
        public string NoteContent { get; set; }

        public override string ToString()
        {
            return $"DateTime: {NoteTime}, NoteContent: {NoteContent}";
        }
    }
}