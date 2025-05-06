using System;                 // for Enum
using Domain.Entities;       // for TicketCategory
using FluentValidation;
using Application.DTOs;

namespace Application.Validators;

public class UpdateTicketValidator : AbstractValidator<UpdateTicketDTO>
{
    public UpdateTicketValidator()
    {
        RuleFor(t => t.TicketId).GreaterThanOrEqualTo(0).WithMessage("Ticket ID must be positive");
        RuleFor(t => t.EventId).GreaterThanOrEqualTo(0).WithMessage("Event ID must be valid");
        RuleFor(t => t.AttendeeId).GreaterThanOrEqualTo(0).WithMessage("Attendee ID must be valid");
        RuleFor(t => t.TicketType)
            .NotEmpty().WithMessage("Ticket type is required")
            .Must(type => Enum.TryParse<TicketCategory>(type, out _))
            .WithMessage("Invalid ticket type");
    }
}
