﻿using Mapster;
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
            return Ok(currencyEntites);
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetCurrencyById(Guid Id)
        {
            var currency = await _currencyRepository.Find(c => c.Id == Id, false);
            if (currency is null)
                return Ok(new { message = $"Currency with Id : {currency!.Id} doesn't exist in the database" });
            var currencyEntity = currency.Adapt<CurrencyDto>();
            return Ok(currencyEntity);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCurrency([FromForm] CurrencyDto model, CancellationToken cancellationToken)
        {
            var currency = model.Adapt<Currency>();
            currency.CreatedBy = "admin";
           var result =  await _currencyRepository.Create(currency);
            await _currencyRepository.SaveAsync(cancellationToken);
            return Ok(result.Id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCurrency([FromForm] CurrencyDto model, CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.Find(c => c.Id != model.Id, true);
            if (currency is null)
            {
                return Ok(new Response<CurrencyDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Currency with Id: {model.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            }
            currency = model.Adapt<Currency>();
            currency.CreatedBy = "admin";
           _currencyRepository.Update(currency);
            await _currencyRepository.SaveAsync(cancellationToken);
            return Ok(currency.Id);
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteCurrency(Guid Id , CancellationToken cancellationToken)
        {
            var currency = await _currencyRepository.Find(c => c.Id != Id, true);
            if (currency is null)
            {
                return Ok(new Response<CurrencyDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Currency with Id: {currency!.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            }
            _currencyRepository.Delete(currency);
            await _currencyRepository.SaveAsync(cancellationToken) ;
            return Ok(currency.Id);
        }

    }
}
