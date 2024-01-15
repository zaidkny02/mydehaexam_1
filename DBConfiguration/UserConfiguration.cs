using deha_exam_quanlykhoahoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace deha_exam_quanlykhoahoc.DBConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // auto = DeleteBehavior.Cascade  => auto delete child when parent get deleted
            builder.HasMany(e => e.ClassDetail)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserID)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Comments)
             .WithOne(e => e.User)
             .HasForeignKey(e => e.UserID)
             .HasPrincipalKey(e => e.Id)
             ;
        }
    }
}
