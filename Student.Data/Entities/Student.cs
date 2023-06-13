namespace StudentRegistration.Data.Entities
{
    public class Student
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public List<Registration>? Registrations { get; set; }
    }
}
