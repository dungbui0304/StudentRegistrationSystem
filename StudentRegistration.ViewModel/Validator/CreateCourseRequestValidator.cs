using FluentValidation;
using StudentRegistration.ViewModel.Courses;

namespace StudentRegistration.ViewModel.Validator
{
    public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
    {
        public CreateCourseRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Course id is required.");
            RuleFor(c => c.Name).NotEmpty()
                .WithMessage("Course name is required.")
                .MinimumLength(10).MaximumLength(50);
            RuleFor(c => c.Description).NotEmpty()
                .WithMessage("Course description is required.")
                .MinimumLength(10).MaximumLength(50);
        }
    }
}
