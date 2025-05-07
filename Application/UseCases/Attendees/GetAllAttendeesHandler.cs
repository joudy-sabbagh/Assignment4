// Application/UseCases/Attendees/GetAllAttendeesHandler.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesHandler
        : IRequestHandler<GetAllAttendeesQuery, List<AttendeeListDTO>>
    {
        private readonly IAttendeeRepository _repo;
        private readonly IMapper _mapper;

        public GetAllAttendeesHandler(
            IAttendeeRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<AttendeeListDTO>> Handle(
            GetAllAttendeesQuery request,
            CancellationToken cancellationToken)
        {
            var entities = await _repo.GetAllAsync();
            return _mapper.Map<List<AttendeeListDTO>>(entities);
        }
    }
}
