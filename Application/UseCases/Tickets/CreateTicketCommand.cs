using Application.DTOs;
using MediatR;

namespace Application.UseCases.Tickets
{
    public class CreateTicketCommand : IRequest<int>
    {
        public CreateTicketDTO Dto { get; }

        public CreateTicketCommand(CreateTicketDTO dto)
        {
            Dto = dto;
        }
    }
}
