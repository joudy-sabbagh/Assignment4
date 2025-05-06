using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UpdateEventValidator : AbstractValidator<UpdateEventDTO>
{
    public UpdateEventValidator()
    {
        RuleFor(e => e.EventId).GreaterThan(0);
        RuleFor(e => e.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(e => e.Date).GreaterThanOrEqualTo(DateTime.Today);
        RuleFor(e => e.NormalPrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.VipPrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.BackstagePrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.VenueId).GreaterThan(0);
    }
}
