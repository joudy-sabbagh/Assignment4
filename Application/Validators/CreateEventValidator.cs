using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateEventValidator : AbstractValidator<CreateEventDTO>
{
    public CreateEventValidator()
    {
        RuleFor(e => e.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(e => e.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Event date cannot be in the past");
        RuleFor(e => e.NormalPrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.VipPrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.BackstagePrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.VenueId).GreaterThan(0).WithMessage("Valid venue is required");
    }
}
