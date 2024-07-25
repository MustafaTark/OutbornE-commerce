using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Tickets;
using OutbornE_commerce.BAL.Repositories.Tickets;
using OutbornE_commerce.DAL.Models;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketsController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            var tickets = await _ticketRepository.FindAllAsync(null, false);
            var ticketEntites = tickets.Adapt<List<TicketDto>>();
            return Ok(new Response<List<TicketDto>>
            {
                Data = ticketEntites,
                IsError = false,
                Status = 0,
                Message = ""
            });
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetTicketById(Guid Id)
        {
            var ticket = await _ticketRepository.Find(t => t.Id == Id , false);
            if(ticket ==  null)
            return Ok(new Response<TicketDto>
            {
                Data = null,
                IsError = true,
                //Status = (StatusCode)2,
                Message = $"Ticket with Id : {Id} doesn't exist in the database"
            });
            var ticketEntity = ticket.Adapt<TicketDto>();
            return Ok(new Response<TicketDto>
            {
                Data = ticketEntity,
                IsError = false,
                Status = 0,
                Message = ""
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketForCreationDto model , CancellationToken cancellationToken)
        {
            var ticket = model.Adapt<Ticket>();
            ticket.CreatedBy = "user"; // !!!
            var result = await _ticketRepository.Create(ticket);
            await _ticketRepository.SaveAsync(cancellationToken);
            return Ok(result.Id);
        }
        [HttpPut("Id")]
        public async Task<IActionResult> UpdateTicket(Guid Id , [FromBody] TicketForCreationDto model,  CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.Find(t => t.Id == Id, true);
            if(ticket == null)
                return Ok(new Response<TicketDto>
                {
                    Data = null,
                    IsError = true,
                    //Status = (StatusCode)2,
                    Message = $"Ticket with Id : {Id} doesn't exist in the database"
                });
            ticket = model.Adapt<Ticket>();
            ticket.CreatedBy = "user";
            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveAsync(cancellationToken);
            return Ok(ticket.Id);
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteTicket(Guid Id , CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.Find(t => t.Id == Id, true);
            if (ticket == null)
                return Ok(new Response<TicketDto>
                {
                    Data = null,
                    IsError = true,
                    //Status = (StatusCode)2,
                    Message = $"Ticket with Id : {Id} doesn't exist in the database"
                });
            _ticketRepository.Delete(ticket);
            await _ticketRepository.SaveAsync(cancellationToken);
            return Ok(ticket.Id);
        }
    }
}
