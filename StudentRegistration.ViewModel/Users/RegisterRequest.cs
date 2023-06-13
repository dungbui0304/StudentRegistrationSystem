using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.ViewModel.Users
{
    public class RegisterRequest
    {
        [Display(Name = "Tên đăng nhập")]
        public string? UserName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ Email")]
        public string? Email { get; set; }

        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        public string? ComfirmPassword { get; set; }
    }
}
