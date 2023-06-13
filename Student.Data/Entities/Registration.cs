namespace StudentRegistration.Data.Entities
{
    public class Registration
    {
        public string? Id { get; set; }
        public string? StudentId { get; set; }
        public string? CourseId { get; set; }
        public DateTime CreateAt { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
