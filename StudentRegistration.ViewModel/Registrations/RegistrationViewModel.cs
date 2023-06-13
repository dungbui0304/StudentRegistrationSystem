using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.ViewModel.Registrations
{
    public class RegistrationViewModel
    {
        [Display(Name = "Mã đăng ký học")]
        public string? Id { get; set; }

        [Display(Name = "Mã sinh viên")]
        public string? StudentId { get; set; }

        [Display(Name = "Tên sinh viên")]
        public string? StudentName { get; set; }

        [Display(Name = "Mã môn học")]
        public string? CourseId { get; set; }

        [Display(Name = "Tên môn học")]
        public string? CourseName { get; set; }
        [Display(Name = "Tên môn học")]
        public DateTime? CreateAt { get; set; }

    }
}
