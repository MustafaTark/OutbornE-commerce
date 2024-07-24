using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Cities;
using OutbornE_commerce.BAL.Dto.Countries;
using OutbornE_commerce.BAL.Repositories.Cities;
using OutbornE_commerce.BAL.Repositories.Countries;
using OutbornE_commerce.DAL.Models;
using System.Threading;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICountryRepository _countryRepository;
        public CitiesController(ICityRepository cityRepository , ICountryRepository countryRepository)
        {
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _cityRepository.FindAllAsync(null, false);
            var cityEntites = cities.Adapt<List<CityDto>>();
            return Ok(new Response<List<CityDto>>
            {
                Data = cityEntites,
                IsError = false,
                Status = 0,
                Message = ""
            });
        }
        [HttpGet("countryId")]
        public async Task<IActionResult> GetAllCitiesForCountry(Guid countryId)
        {
            var country = await _countryRepository.Find(c => c.Id == countryId, false);
            if (country == null)
                return Ok(new Response<CountryDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Contry with Id: {countryId} doesn't exist in the database",
                    Status = (StatusCode) 2,
                });
            var cities = await _cityRepository.FindByCondition(c => c.CountryId == countryId, null); // null for the includes !!!
            var cityEntites = cities.Adapt<List<CityDto>>();
            return Ok(new Response<List<CityDto>>
            {
                Data = cityEntites,
                IsError = false,
                Message = "",
                Status =0,
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateCity([FromForm] CityForCreationDto model ,Guid countryId , CancellationToken cancellationToken)
        {
            var country = await _countryRepository.Find(c => c.Id == countryId, false);
            if (country == null)
                return Ok(new Response<CountryDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Contry with Id: {countryId} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            var city = model.Adapt<City>();
            city.CreatedBy = "admin";
            var result = await _cityRepository.Create(city);
            await _cityRepository.SaveAsync(cancellationToken);
            return Ok(result.Id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCity([FromForm] CityDto model, Guid countryId, CancellationToken cancellationToken)
        {
            var country = await _countryRepository.Find(c => c.Id == countryId, true);
            if (country == null)
                return Ok(new Response<CountryDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"Contry with Id: {countryId} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            var city = await _cityRepository.Find(c => c.Id == model.Id, true);
            if(city == null)
            {
                return Ok(new Response<CityDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"City with Id: {model.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            }
            city = model.Adapt<City>();
            city.CreatedBy = "admin";
            _cityRepository.Update(city);
            await _cityRepository.SaveAsync(cancellationToken);
            return Ok(city.Id);
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteCity(Guid Id, CancellationToken cancellationToken)
        {
            var city = await _cityRepository.Find(c => c.Id == Id, false);
            if (city == null)
                return Ok(new Response<CityDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"City with Id: {city!.Id} doesn't exist in the database",
                    Status = (StatusCode)2,
                });
            _cityRepository.Delete(city);
            await _cityRepository.SaveAsync(cancellationToken);
            return Ok(city.Id);
        }

    }
}
