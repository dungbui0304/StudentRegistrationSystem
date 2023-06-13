using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.ViewModel.Students
{
    public class ChangePassword
    {
        [Display(Name = "Mật khẩu cũ")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        public string? ComfirmPassword { get; set; }
    }
}
