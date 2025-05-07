// Application/UseCases/Attendees/CreateAttendeeHandler.cs
using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Attendees
{
    public class CreateAttendeeHandler : IRequestHandler<CreateAttendeeCommand, int>
    {
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAttendeeHandler> _logger;

        public CreateAttendeeHandler(
            IAttendeeRepository attendeeRepo,
            IMapper mapper,
            ILogger<CreateAttendeeHandler> logger)
        {
            _attendeeRepo = attendeeRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateAttendeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting CreateAttendee for {@Dto}", request.Dto);

            var attendee = _mapper.Map<Attendee>(request.Dto);
            await _attendeeRepo.AddAsync(attendee);

            _logger.LogInformation("Created Attendee with Id {AttendeeId}", attendee.Id);
            return attendee.Id;
        }
    }
}
