using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class CreateAttendeeValidator : AbstractValidator<CreateAttendeeDTO>
    {
        public CreateAttendeeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("A valid email is required.");
        }
    }
}
