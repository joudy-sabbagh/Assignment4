using Application.DTOs;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class UpdateTicketCommand : IRequest
    {
        public UpdateTicketDTO Dto { get; }

        public UpdateTicketCommand(UpdateTicketDTO dto)
        {
            Dto = dto;
        }
    }
}
