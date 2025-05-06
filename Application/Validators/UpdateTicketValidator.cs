using Application.DTOs;
using FluentValidation;

namespace Application.Validators;

public class UpdateTicketValidator : AbstractValidator<UpdateTicketDTO>
{
    public UpdateTicketValidator()
    {
        RuleFor(t => t.TicketId).GreaterThan(0).WithMessage("Ticket ID must be positive");
        RuleFor(t => t.EventId).GreaterThan(0).WithMessage("Event ID must be valid");
        RuleFor(t => t.AttendeeId).GreaterThan(0).WithMessage("Attendee ID must be valid");
        RuleFor(t => t.TicketType)
            .NotEmpty().WithMessage("Ticket type is required")
            .Must(type => Enum.IsDefined(typeof(TicketCategory), type))
            .WithMessage("Invalid ticket type");
    }
}
