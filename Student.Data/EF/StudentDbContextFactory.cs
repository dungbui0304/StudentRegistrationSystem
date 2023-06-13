using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StudentRegistration.Data.EF
{
    public class StudentDbContextFactory : IDesignTimeDbContextFactory<StudentDbContext>
    {
        private readonly string _connectionString;
        public StudentDbContextFactory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();
            var connectionString = config.GetConnectionString("StudentDb");
            _connectionString = connectionString;
        }

        public StudentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentDbContext>();

            optionsBuilder.UseSqlServer(_connectionString);
            return new StudentDbContext(optionsBuilder.Options);
        }
    }
}
