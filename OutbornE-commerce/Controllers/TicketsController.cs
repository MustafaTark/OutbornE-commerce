using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.ProductColors;
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
        public async Task<IActionResult> GetAllTickets(int pageNumber , int pageSize)
        {
            var items = await _ticketRepository.FindAllAsyncByPagination(null, pageNumber, pageSize);

            var data = items.Data.Adapt<List<TicketDto>>();

            return Ok(new PaginationResponse<List<TicketDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = items.TotalCount
            });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetTicketById(Guid Id)
        {
            var ticket = await _ticketRepository.Find(t => t.Id == Id , false);
            if(ticket ==  null)
            return Ok(new Response<TicketDto>
            {
                Data = null,
                IsError = true,
                Status = (int)StatusCodeEnum.NotFound,
                Message = $"Ticket with Id : {Id} doesn't exist in the database"
            });
            var ticketEntity = ticket.Adapt<TicketDto>();
            return Ok(new Response<TicketDto>
            {
                Data = ticketEntity,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
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
            return Ok(new Response<Guid>()
            {
                Data = result.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpPut("Id")]
        public async Task<IActionResult> UpdateTicket( [FromBody] TicketDto model,  CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.Find(t => t.Id == model.Id, false);
            if(ticket == null)
                return Ok(new Response<TicketDto>
                {
                    Data = null,
                    IsError = true,
                    Status = (int)StatusCodeEnum.NotFound,
                    Message = $"Ticket with Id : {model.Id} doesn't exist in the database"
                });
            ticket = model.Adapt<Ticket>();
            ticket.CreatedBy = "user";
            _ticketRepository.Update(ticket);
            await _ticketRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>()
            {
                Data = ticket.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteTicket(Guid Id , CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.Find(t => t.Id == Id, false);
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
            return Ok(new Response<Guid>()
            {
                Data = ticket.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
    }
}
