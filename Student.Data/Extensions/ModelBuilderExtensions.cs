using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data.Entities;

namespace StudentRegistration.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var adminRoleId = Guid.NewGuid();
            var studentRoleId = Guid.NewGuid();

            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleId,
                    Name = "admin",
                    NormalizedName = "admin",
                    Description = "Day la role admin"
                },
                new Role
                {
                    Id = studentRoleId,
                    Name = "student",
                    NormalizedName = "student",
                    Description = "Day la role student"
                }
                );

            var adminId = Guid.NewGuid();
            var studentId = Guid.NewGuid();

            var passwordHash = new PasswordHasher<User>();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    UserName = "admin",
                    NormalizedUserName = "admin",
                    Email = "admin@example.com",
                    NormalizedEmail = "admin@example.com",
                    EmailConfirmed = true,
                    PasswordHash = passwordHash.HashPassword(null, "123456"),
                    SecurityStamp = string.Empty,
                    FirstLogin = true
                },
                new User
                {
                    Id = studentId,
                    UserName = "student",
                    NormalizedUserName = "student",
                    Email = "student@example.com",
                    NormalizedEmail = "student@example.com",
                    PhoneNumber = "0947771450",
                    EmailConfirmed = true,
                    PasswordHash = passwordHash.HashPassword(null, "123456"),
                    SecurityStamp = string.Empty,
                    FirstLogin = true
                }
                );

            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = "1951060632",
                    FirstName = "Dũng",
                    LastName = "Bùi",
                    PhoneNumber = "0947771450",
                    UserId = studentId,
                    Email = "dungbt@gmail.com"
                }
                );
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(
                new IdentityUserRole<Guid>
                {
                    RoleId = adminRoleId,
                    UserId = adminId
                },
                new IdentityUserRole<Guid>
                {
                    RoleId = studentRoleId,
                    UserId = studentId
                }
                );
        }
    }
}
