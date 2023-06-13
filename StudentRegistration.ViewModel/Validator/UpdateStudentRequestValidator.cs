using FluentValidation;
using StudentRegistration.ViewModel.Students;

namespace StudentRegistration.ViewModel.Validator
{
    public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
    {
        public UpdateStudentRequestValidator()
        {
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(s => s.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
        }
    }
}
