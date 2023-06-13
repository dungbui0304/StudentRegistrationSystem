using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Data.Entities;

namespace StudentRegistration.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.FirstName).HasMaxLength(50);
            builder.Property(x => x.LastName).HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired();
            builder.HasMany(s => s.Registrations) // 1 student có nhiều registration
                .WithOne(r => r.Student)          // 1 registration chỉ có thể thuộc về 1 student
                .HasForeignKey(r => r.StudentId);  // bảng registration có khóa ngoại là studentId
            builder.HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.UserId);
        }
    }
}
