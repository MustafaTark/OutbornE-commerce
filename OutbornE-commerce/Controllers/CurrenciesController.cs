using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Countries;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Currencies;
using OutbornE_commerce.BAL.Repositories.Currencies;
using OutbornE_commerce.DAL.Models;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository;
        public CurrenciesController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var currencies = await _currencyRepository.FindAllAsync(null, false);
            var currencyEntites = currencies.Adapt<List<CurrencyDto>>();
            return Ok(new Response<List<CurrencyDto>>
            {
                Data = currencyEntites,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.Ok)
            });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCurrencyById(Guid Id)
        {
            var currency = await _currencyRepository.Find(c => c.Id == Id, true);
            if (currency is null)
                return Ok(new Response<CurrencyDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Currency with Id: {Id} doesn't exist in the database",
                   Status = (int)(StatusCodeEnum.NotFound)    
                });
            var currencyEntity = currency.Adapt<CurrencyDto>();
            return Ok(new Response<CurrencyDto>
            {
                Data = currencyEntity,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.Ok)
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCurrency([FromBody] CurrencyDto model, CancellationToken cancellationToken)
        {
            var currency = model.Adapt<Currency>();
            currency.CreatedBy = "admin";
           var result =  await _currencyRepository.Create(currency);
            await _currencyRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.Ok)
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCurrency([FromBody] CurrencyDto model, CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.Find(c => c.Id != model.Id, false);
            if (currency is null)
            {
                return Ok(new Response<CurrencyDto>
                {
                    Data = null,
                    IsError = false,
                    Message = "",
                    Status = (int)(StatusCodeEnum.NotFound)
                });
            }
            currency = model.Adapt<Currency>();
            currency.CreatedBy = "admin";
           _currencyRepository.Update(currency);
            await _currencyRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = currency.Id,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.NotFound)
            });
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteCurrency(Guid Id , CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.Find(c => c.Id != Id, false);
            if (currency is null)
            {
                return Ok(new Response<CurrencyDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Currency with Id: {Id} doesn't exist in the database",
                    Status = (int)StatusCodeEnum.NotFound
                });
            }
            _currencyRepository.Delete(currency);
            await _currencyRepository.SaveAsync(cancellationToken) ;
            return Ok(new Response<Guid>
            {
                Data = currency.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }

    }
}
