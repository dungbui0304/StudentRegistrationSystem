using Microsoft.AspNetCore.Identity;

namespace StudentRegistration.Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Student? Student { get; set; }
        public bool FirstLogin { get; set; }
    }
}
