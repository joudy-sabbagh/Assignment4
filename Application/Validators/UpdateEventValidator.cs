using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UpdateEventValidator : AbstractValidator<UpdateEventDTO>
{
    public UpdateEventValidator()
    {
        RuleFor(e => e.EventId).GreaterThanOrEqualTo(0);
        RuleFor(e => e.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(e => e.NormalPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EventDate).NotEmpty().WithMessage("Date is required");
        RuleFor(x => x.VIPPrice).GreaterThan(0).WithMessage("VIP price must be positive");
        RuleFor(e => e.BackstagePrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.VenueId).GreaterThan(0);
    }
}
