using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class CreateTicketValidator : AbstractValidator<CreateTicketDTO>
{
    public CreateTicketValidator()
    {
        RuleFor(t => t.EventId).GreaterThanOrEqualTo(0);
        RuleFor(t => t.AttendeeId).GreaterThanOrEqualTo(0);
        RuleFor(t => t.TicketType).NotEmpty().WithMessage("Ticket type is required");
    }
}
