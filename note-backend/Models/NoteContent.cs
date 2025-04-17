using System.ComponentModel.DataAnnotations;

namespace note_backend.Models
{
    public class NoteContent
    {
        [Key] public Guid NoteId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
