using FluentValidation;
using StudentRegistration.ViewModel.Users;

namespace StudentRegistration.ViewModel.Validator
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MinimumLength(4).WithMessage("UserName min 4 characters.")
                .MaximumLength(8).WithMessage("UserName max 8 characters.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.ComfirmPassword)
                .NotEmpty().WithMessage("ComfirmPassword is required.")
                .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }
    }
}
