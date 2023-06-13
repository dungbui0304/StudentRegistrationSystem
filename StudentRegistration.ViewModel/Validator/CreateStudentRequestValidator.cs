using FluentValidation;
using StudentRegistration.ViewModel.Students;

namespace StudentRegistration.ViewModel.Validator
{
    public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
    {
        public CreateStudentRequestValidator()
        {
            RuleFor(s => s.Id).NotEmpty()
                .WithMessage("Id is required.")
                .MinimumLength(10).WithMessage("Id is required min 10 character.");
            RuleFor(s => s.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(s => s.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(s => s.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required.");
            RuleFor(s => s.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
        }
    }
}
