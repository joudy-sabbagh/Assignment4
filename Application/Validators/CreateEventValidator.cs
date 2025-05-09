using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateEventValidator : AbstractValidator<CreateEventDTO>
{
    public CreateEventValidator()
    {
        RuleFor(e => e.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(e => e.NormalPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EventDate).NotEmpty().WithMessage("Date is required");
        RuleFor(x => x.VIPPrice).GreaterThanOrEqualTo(0).WithMessage("VIP price must be positive");
        RuleFor(e => e.BackstagePrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.VenueId).GreaterThanOrEqualTo(0).WithMessage("Valid venue is required");
    }
}
