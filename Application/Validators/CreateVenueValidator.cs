using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateVenueValidator : AbstractValidator<CreateVenueDTO>
{
    public CreateVenueValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty()
            .WithMessage("Venue name is required");

        RuleFor(v => v.Capacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than 0");
    }
}
