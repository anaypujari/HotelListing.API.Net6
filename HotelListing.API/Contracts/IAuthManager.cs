using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
        Task<AuthResponceDto> Login(LoginDto loginDto);

        Task<string> CreateRefreshToken();
        Task<AuthResponceDto> VerifyRefreshToken(AuthResponceDto request);
    }
}
