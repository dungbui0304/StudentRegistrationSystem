namespace StudentRegistration.Data.Entities
{
    public class Course
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Registration>? Registrations { get; set; }
    }
}
