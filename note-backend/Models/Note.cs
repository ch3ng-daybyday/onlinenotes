using System.ComponentModel.DataAnnotations;

namespace note_backend.Models
{
    public class Note
    {
        [Key] public int NoteId { get; set; }

        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Content { get; set; }
        public List<NoteContent> ContentList { get; set; }
    }
}
