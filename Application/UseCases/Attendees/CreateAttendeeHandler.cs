// Application/UseCases/Attendees/CreateAttendeeHandler.cs
using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class CreateAttendeeHandler : IRequestHandler<CreateAttendeeCommand, int>
    {
        private readonly IAttendeeRepository _attendeeRepo;
        private readonly IMapper _mapper;

        public CreateAttendeeHandler(
            IAttendeeRepository attendeeRepo,
            IMapper mapper)
        {
            _attendeeRepo = attendeeRepo;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateAttendeeCommand request, CancellationToken cancellationToken)
        {
            var attendee = _mapper.Map<Attendee>(request.Dto);

            await _attendeeRepo.AddAsync(attendee);
            return attendee.Id;
        }
    }
}
