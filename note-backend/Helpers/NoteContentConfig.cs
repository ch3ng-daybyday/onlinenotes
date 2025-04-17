using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using note_backend.Models;

namespace note_backend.Helpers
{
    public class NoteContentConfig : IEntityTypeConfiguration<NoteContent>
    {
        public void Configure(EntityTypeBuilder<NoteContent> builder)
        {
            builder.ToTable("T_NoteContent");
        }
    }
}
