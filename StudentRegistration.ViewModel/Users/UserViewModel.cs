namespace StudentRegistration.ViewModel.Users
{
    public class UserViewModel
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool FirstLogin { get; set; }
    }
}
