using deha_exam_quanlykhoahoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace deha_exam_quanlykhoahoc.DBConfiguration
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            // auto = DeleteBehavior.Cascade  => auto delete child when parent get deleted
            builder.HasMany(e => e.Lessons)
            .WithOne(e => e.Class)
            .HasForeignKey(e => e.ClassID)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict)
            ;

            builder.HasMany(e => e.ClassDetail)
             .WithOne(e => e.Class)
             .HasForeignKey(e => e.ClassID)
             .HasPrincipalKey(e => e.Id)
             .OnDelete(DeleteBehavior.Restrict)
             ;
        }
    }
}
