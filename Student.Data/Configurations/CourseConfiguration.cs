using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Data.Entities;

namespace StudentRegistration.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.HasMany(c => c.Registrations)
                .WithOne(r => r.Course)
                .HasForeignKey(c => c.CourseId);
        }
    }
}
