using Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace Application.UseCases.Venues
{
    public class GetAllVenuesQuery : IRequest<List<Venue>> { }
}
