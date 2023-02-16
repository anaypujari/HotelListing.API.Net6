using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repository
{
    public class CountriesReopository : GenericRepository<Country>, ICountriesRepositories
    {
        private readonly HotelListingDbContext _context;

        public CountriesReopository(HotelListingDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(q=>q.Hotels)
                .FirstOrDefaultAsync(q=>q.Id== id);
        }
    }
}
