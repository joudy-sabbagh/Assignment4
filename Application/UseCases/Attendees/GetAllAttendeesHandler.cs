// Application/UseCases/Attendees/GetAllAttendeesHandler.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Application.Common;            
using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Attendees
{
    public class GetAllAttendeesHandler
        : IRequestHandler<GetAllAttendeesQuery, Result<List<AttendeeListDTO>>>
    {
        private readonly IAttendeeRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllAttendeesHandler> _logger;

        public GetAllAttendeesHandler(
            IAttendeeRepository repo,
            IMapper mapper,
            ILogger<GetAllAttendeesHandler> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<AttendeeListDTO>>> Handle(
            GetAllAttendeesQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllAttendeesQuery");
            var entities = await _repo.GetAllAsync();

            if (entities == null || !entities.Any())
            {
                _logger.LogWarning("No attendees found");
                return Result<List<AttendeeListDTO>>.Failure("No attendees available");
            }

            var dtos = _mapper.Map<List<AttendeeListDTO>>(entities);
            _logger.LogInformation("Retrieved {Count} attendees", dtos.Count);
            return Result<List<AttendeeListDTO>>.Success(dtos);
        }
    }
}
