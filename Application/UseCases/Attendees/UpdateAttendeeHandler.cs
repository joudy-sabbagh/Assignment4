// Application/UseCases/Attendees/UpdateAttendeeHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class UpdateAttendeeHandler : IRequestHandler<UpdateAttendeeCommand>
    {
        private readonly IAttendeeRepository _repo;

        public UpdateAttendeeHandler(IAttendeeRepository repo) => _repo = repo;

        public async Task Handle(UpdateAttendeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var attendee = await _repo.GetByIdAsync(dto.Id);
            if (attendee == null)
                throw new KeyNotFoundException($"Attendee with ID {dto.Id} not found.");

            // use domain Update method
            attendee.Update(dto.Name, dto.Email);
            await _repo.UpdateAsync(attendee);
        }
    }
}
