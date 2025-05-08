using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Tickets
{
    public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, int>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ITicketPricingService _pricingService;
        private readonly ILogger<CreateTicketHandler> _logger;
        private readonly IMediator _mediator;

        public CreateTicketHandler(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo,
            ITicketPricingService pricingService,
            ILogger<CreateTicketHandler> logger,
            IMediator mediator)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
            _pricingService = pricingService;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var ev = await _eventRepo.GetByIdAsync(dto.EventId)
                      ?? throw new KeyNotFoundException($"Event {dto.EventId} not found");

            var price = _pricingService.GetPrice(ev, dto.TicketType);
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

            await _mediator.Publish(new TicketPurchasedEvent(ticket.Id), cancellationToken);
            return ticket.Id;
        }
    }
}
