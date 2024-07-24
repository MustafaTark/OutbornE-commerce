using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Countries;
using OutbornE_commerce.BAL.Dto.Currencies;
using OutbornE_commerce.BAL.Repositories.Countries;
using OutbornE_commerce.DAL.Models;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryRepository.FindAllAsync(null, false);
            var countryEntites = countries.Adapt<List<CountryDto>>();
                return Ok(new Response<List<CountryDto>>
                {
                    Data = countryEntites,
                    IsError = true,
                    Message = "",
                    Status = 0,
                });
        }
        [HttpGet("Id")]
        public async Task<IActionResult>GetCountryById(Guid Id)
        {
            var country = await _countryRepository.Find(c => c.Id == Id, false);
            if (country == null)
                return Ok(new Response<CountryDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Country with Id: {country!.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            var countryEntity = country.Adapt<CountryDto>();
            return Ok(new Response<CountryDto>
            {
                Data = countryEntity,
                IsError = true,
                Message = "",
                Status = 0,
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCountry([FromForm] CountryForCreationDto model , CancellationToken cancellationToken)
        {
            var country = model.Adapt<Country>();
            country.CreatedBy = "admin";
            var result = await _countryRepository.Create(country);
            await _countryRepository.SaveAsync(cancellationToken);
            return Ok(result.Id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCountry([FromForm] CountryDto model, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.Find(c => c.Id == model.Id, false);
            if (country == null)
                return Ok(new Response<CountryDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Country with Id: {country!.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            country = model.Adapt<Country>();
            country.CreatedBy = "admin";
            _countryRepository.Update(country);
            await _countryRepository.SaveAsync(cancellationToken);
            return Ok(country.Id);
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteCountry(Guid Id , CancellationToken cancellationToken)
        {
            var country = await _countryRepository.Find(c => c.Id == Id, false);
            if (country == null)
                return Ok(new Response<CountryDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Country with Id: {country!.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            _countryRepository.Delete(country);
            await _countryRepository.SaveAsync(cancellationToken);
            return Ok(country.Id);
        }
    }
}
