using deha_exam_quanlykhoahoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace deha_exam_quanlykhoahoc.DBConfiguration
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            // auto = DeleteBehavior.Cascade  => auto delete child when parent get deleted
            builder.HasMany(e => e.listFile)
            .WithOne(e => e.Lesson)
            .HasForeignKey(e => e.LessonID)
            .HasPrincipalKey(e => e.Id)
            ;

            builder.HasMany(e => e.Comments)
             .WithOne(e => e.Lesson)
             .HasForeignKey(e => e.LessonID)
             .HasPrincipalKey(e => e.Id);
        }
    }
}
