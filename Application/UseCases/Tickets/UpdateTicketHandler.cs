using System;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Tickets
{
    public class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand, Unit>
    {
        private readonly ITicketRepository _ticketRepo;
        private readonly IEventRepository _eventRepo;
        private readonly ITicketPricingService _pricingService;
        private readonly ILogger<UpdateTicketHandler> _logger;

        public UpdateTicketHandler(
            ITicketRepository ticketRepo,
            IEventRepository eventRepo,
            ITicketPricingService pricingService,
            ILogger<UpdateTicketHandler> logger)
        {
            _ticketRepo = ticketRepo;
            _eventRepo = eventRepo;
            _pricingService = pricingService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            _logger.LogInformation("Updating ticket {Id} with {@Dto}", dto.Id, dto);
            var ticket = await _ticketRepo.GetByIdAsync(dto.Id)
                        ?? throw new KeyNotFoundException($"Ticket {dto.Id} not found");
            var ev = await _eventRepo.GetByIdAsync(dto.EventId)
                     ?? throw new KeyNotFoundException($"Event {dto.EventId} not found");
            var price = _pricingService.GetPrice(ev, dto.TicketType);
            var category = Enum.TryParse<TicketCategory>(dto.TicketType, true, out var cat)
                         ? cat
                         : TicketCategory.Normal;
            ticket.UpdateTypeAndPrice(dto.TicketType, price, category, dto.EventId, dto.AttendeeId);
            await _ticketRepo.UpdateAsync(ticket);
            _logger.LogInformation("Updated ticket {Id}", dto.Id);
            return Unit.Value;
        }
    }
}
