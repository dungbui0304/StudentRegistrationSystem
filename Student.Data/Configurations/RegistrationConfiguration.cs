using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentRegistration.Data.Entities;

namespace StudentRegistration.Data.Configurations
{
    public class RegistrationConfiguration : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.ToTable("Registrations");
            builder.HasKey(x => x.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();
            builder.HasAlternateKey(r => new { r.StudentId, r.CourseId });
        }
    }
}
