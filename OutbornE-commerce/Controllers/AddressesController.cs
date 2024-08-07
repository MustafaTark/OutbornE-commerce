using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Address;
using OutbornE_commerce.BAL.Repositories.Address;
using OutbornE_commerce.DAL.Models;
using System.Threading;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        public AddressesController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAddresses()
        {
            var addresses = await _addressRepository.FindAllAsync(null, false);
            var addressEntities = addresses.Adapt<List<AddressDto>>();

            //var response = new Response<List<AddressDto>>
            //{
            //    Data = addressEntities,
            //    IsError = false,
            //    Message = "",
            //    MessageAr = "",
            //    Status = 200
            //};
            //return Ok(response);
            return Ok(new Response<List<AddressDto>>
            {
                Data = addressEntities,
                IsError = false,
                Message = "",
                MessageAr = "",
                Status = 200
            });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAddressById(Guid Id)
        {
            var address = await _addressRepository.Find(a => a.Id == Id , false);
            var addressEntity = address.Adapt<AddressDto>();
            if (address is null)
                return Ok(new Response<AddressDto>
                {
                    Data = null,
                    IsError = true,
                    Status = (int) StatusCodeEnum.NotFound
                });
            return Ok(new Response<AddressDto>
            {
                Data = addressEntity,
                IsError = false,
                Status = (int) StatusCodeEnum.Ok,
            });
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAddress(Guid Id , CancellationToken cancellationToken)
        {
            var address = await _addressRepository.Find(a => a.Id == Id, false);
            _addressRepository.Delete(address!);
            await _addressRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] AddressForCreationDto model , CancellationToken cancellationToken)
        {
            var address = model.Adapt<Address>();
            address.CreatedBy = "admin";
            var result = await _addressRepository.Create(address);
            await _addressRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressDto model , CancellationToken cancellationToken)
        {
            var address = await _addressRepository.Find(a => a.Id == model.Id);
            address = model.Adapt<Address>();
            address.CreatedBy = "admin";
            _addressRepository.Update(address);
            await _addressRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = address.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
