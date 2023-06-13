using FluentValidation;
using StudentRegistration.ViewModel.Students;

namespace StudentRegistration.ViewModel.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password is required.");
            RuleFor(x => x.NewPassword).NotEmpty()
                .WithMessage("NewPassword is required.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$")
                .WithMessage("Password must contain at least 8 characters, uppercase, lowercase, digit and special character.");
            RuleFor(x => x.ComfirmPassword).NotEmpty()
                .WithMessage("ComfirmPassword is required.")
                .Equal(x => x.NewPassword)
                .WithMessage("The password and confirmation password do not match.");
        }
    }
}
