using FluentValidation;
using Application.DTOs;

namespace Application.Validators
{
    public class UpdateAttendeeValidator : AbstractValidator<UpdateAttendeeDTO>
    {
        public UpdateAttendeeValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);

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
