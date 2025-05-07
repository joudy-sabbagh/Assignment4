// Application/UseCases/Tickets/CreateTicketHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Tickets
{
    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, int>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ILogger<CreateTicketHandler> _logger;

        public CreateTicketHandler(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo,
            ILogger<CreateTicketHandler> logger)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
            _logger = logger;
        }

        public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating ticket {@Dto}", request.Dto);

            var dto = request.Dto;
            var ev = await _eventRepo.GetByIdAsync(dto.EventId)
                      ?? throw new KeyNotFoundException($"Event {dto.EventId} not found");

            var price = dto.TicketType switch
            {
                "VIP" => ev.VIPPrice,
                "Backstage" => ev.BackstagePrice,
                _ => ev.NormalPrice
            };
            var category = Enum.TryParse<TicketCategory>(dto.TicketType, true, out var cat)
                ? cat
                : TicketCategory.Normal;

            var ticket = new Ticket(
                dto.TicketType,
                price,
                category,
                dto.EventId,
                dto.AttendeeId
            );

            await _ticketRepo.AddAsync(ticket);

            _logger.LogInformation("Created Ticket with Id {TicketId}", ticket.Id);
            return ticket.Id;
        }
    }
}
