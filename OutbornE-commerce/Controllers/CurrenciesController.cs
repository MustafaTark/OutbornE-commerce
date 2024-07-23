using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Currencies;
using OutbornE_commerce.BAL.Repositories.Currencies;

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
            var currencyEntites = currencies.Adapt<CurrencyDto>();
            return Ok(new {data =  currencyEntites , message = ""});
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetCurrencyById(Guid Id)
        {
            var currency = await _currencyRepository.Find(c => c.Id == Id , false);
            if (currency is null)
                return Ok(new { message = $"Currency with Id : {currency!.Id} doesn't exist in the database");
            var currencyEntity = currency.Adapt<CurrencyDto>();
            return Ok(new { data = currencyEntity, message = "" });
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateCurrency([FromForm] CurrencyDto model , CancellationToken cancellationToken)
        //{
        //    var currency = model.Adapt<Currency>();
        //}
    }
}
