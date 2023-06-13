using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.ViewModel.Students
{
    public class UpdateStudentRequest
    {
        [Display(Name = "Mã Sinh Viên")]
        public string? Id { get; set; }

        [Display(Name = "Tên")]
        public string? FirstName { get; set; }

        [Display(Name = "Họ")]
        public string? LastName { get; set; }

        [Display(Name = "Địa chỉ Email")]
        public string? Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
    }
}
