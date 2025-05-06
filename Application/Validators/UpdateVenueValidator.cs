using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UpdateVenueValidator : AbstractValidator<UpdateVenueDTO>
{
    public UpdateVenueValidator()
    {
        RuleFor(v => v.VenueId).GreaterThanOrEqualTo(0);
        RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(v => v.Capacity).GreaterThan(0).WithMessage("Capacity must be positive");
    }
}
