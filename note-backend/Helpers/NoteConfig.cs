using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using note_backend.Models;

namespace note_backend.Helpers
{
    public class NoteConfig : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("T_Note");

            builder.Property(a=>a.Title).HasMaxLength(100).IsUnicode(true).IsRequired();
            builder.Property(a=>a.NoteId).IsRequired(); 
        }
    }
}
