using HotelListing.API.Data;

namespace HotelListing.API.Contracts
{
    public interface ICountriesRepositories : IGenericRepository<Country>
    {
        Task<Country> GetDetails(int id);
    }
}
