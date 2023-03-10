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
using Microsoft.AspNetCore.OData.Query;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("2.0")]
    public class CountriesV2Controller : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly ICountriesRepositories _countriesRepositories;
        private readonly ILogger<CountriesController> _logger;

        public CountriesV2Controller( IMapper mapper,ICountriesRepositories countriesRepositories, ILogger<CountriesController> logger)
        {
           
            this._mapper = mapper;
            this._countriesRepositories = countriesRepositories;
            this._logger = logger;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepositories.GetAllAsync();
            var records= _mapper.Map<List<GetCountryDto>>(countries);
            return Ok(countries);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepositories.GetDetails(id);

            if (country == null)
            {
                //_logger.LogWarning($"No Record found in {nameof(GetCountries)} with record id: {id} ");
                //return NotFound();
                throw new NotFoundException(nameof(GetCountry),id);
            }

            var countryDto= _mapper.Map<CountryDto>(country);

            return Ok(countryDto);
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

            var country = await _countriesRepositories.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _mapper.Map(updatecountryDto, country);

            try
            {
                await _countriesRepositories.UpdateAsync(country);
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
        public async Task<ActionResult<Country>> PostCountry(CreatecountryDto createCountryDto)
        {
            
            var country= _mapper.Map<Country>(createCountryDto);

            await _countriesRepositories.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepositories.GetAsync(id);
            if (country == null)
            {
                //return NotFound();
                throw new NotFoundException(nameof(GetCountry), id);
            }

            await _countriesRepositories.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepositories.Exists(id);
        }
    }
}
