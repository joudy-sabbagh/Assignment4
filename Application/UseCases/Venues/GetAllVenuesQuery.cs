using System.Collections.Generic;
using Application.Common;          
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Venues
{
    public class GetAllVenuesQuery : IRequest<Result<List<VenueListDTO>>> { }
}
