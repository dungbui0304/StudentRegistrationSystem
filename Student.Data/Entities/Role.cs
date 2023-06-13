using Microsoft.AspNetCore.Identity;

namespace StudentRegistration.Data.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
    }
}
