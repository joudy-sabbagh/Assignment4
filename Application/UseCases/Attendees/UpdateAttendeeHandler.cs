using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.DTOs;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Application.UseCases.Attendees
{
    public class UpdateAttendeeHandler : IRequestHandler<UpdateAttendeeCommand, Unit>
    {
        private readonly IAttendeeRepository _repo;
        private readonly ILogger<UpdateAttendeeHandler> _logger;

        public UpdateAttendeeHandler(
            IAttendeeRepository repo,
            ILogger<UpdateAttendeeHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAttendeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            _logger.LogInformation("Updating attendee {Id}", dto.Id);

            var attendee = await _repo.GetByIdAsync(dto.Id);
            if (attendee == null)
            {
                _logger.LogWarning("Attendee {Id} not found", dto.Id);
                throw new KeyNotFoundException($"Attendee with ID {dto.Id} not found.");
            }

            attendee.Update(
                dto.Name,
                new EmailAddress(dto.Email)
            );

            await _repo.UpdateAsync(attendee);
            _logger.LogInformation("Updated attendee {Id}", dto.Id);
            return Unit.Value;
        }
    }
}
