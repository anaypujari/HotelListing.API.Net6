using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Core.Models.Country;
using AutoMapper;
using HotelListing.API.Core.Contracts;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Core.Exceptions;
using HotelListing.API.Core.Models;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiversion}/countries")]
    [ApiController]
    [ApiVersion("1.0", Deprecated =true)]
    public class CountriesController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly ICountriesRepositories _countriesRepositories;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController( IMapper mapper,ICountriesRepositories countriesRepositories, ILogger<CountriesController> logger)
        {
           
            this._mapper = mapper;
            this._countriesRepositories = countriesRepositories;
            this._logger = logger;
        }

        // GET: api/Countries/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepositories.GetAllAsync<GetCountryDto>();
            return Ok(countries);
        }
        
        // GET: api/Countries/?StartIndex=0&pageSize=25&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PageResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            var pagedCountriesResult = await _countriesRepositories.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCountriesResult);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepositories.GetDetails(id);
            return Ok(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updatecountryDto)
        {
            if (id != updatecountryDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }

            //_context.Entry(country).State = EntityState.Modified;

            try
            {
                await _countriesRepositories.UpdateAsync(id,updatecountryDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if ( await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CountryDto>> PostCountry(CreatecountryDto createCountryDto)
        {

            var country = await _countriesRepositories.AddAsync<CreatecountryDto, GetCountryDto>(createCountryDto);

            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")] 
        public async Task<IActionResult> DeleteCountry(int id)
        {
            await _countriesRepositories.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepositories.Exists(id);
        }
    }
}
