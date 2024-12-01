using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using note_backend.Models;
using System.Reflection.Emit;

namespace note_backend.Helpers
{

    public class MyDbContext : IdentityDbContext<User, Role, long>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //config table
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Note_User");
            });
            builder.Entity<Role>(entity =>
            {
                entity.ToTable("note_Role");
            });
            foreach (var model in builder.Model.GetEntityTypes())
            {
                foreach (var property in model.GetProperties())
                {
                    if (property.ClrType == typeof(string))
                    {
                        var type = property.GetColumnType();
                        if (string.IsNullOrEmpty(type) || type.Contains("nvarchar(max)"))
                        {
                            property.SetColumnType("nvarchar(225)");
                        }
                    }
                }
            }
        }
    }
}
