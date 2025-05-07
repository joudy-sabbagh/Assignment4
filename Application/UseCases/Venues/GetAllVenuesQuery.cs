// Application/UseCases/Venues/GetAllVenuesQuery.cs
using System.Collections.Generic;
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Venues
{
    public class GetAllVenuesQuery : IRequest<List<VenueListDTO>> { }
}
