using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.Auth;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Entities;
using System.Security.Claims;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralServiceResponseDto> SeedRoleAsync();
        Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginServiceResponceDto?> LoginAsync(LoginDto loginDto);
        Task<LoginServiceResponceDto> MeAsync(MeDto meDto);
        Task<string> GetCurrentUserId(string username);
        Task<string> GetCurrentUserName(string username);
        Task<string> GetCurrentFullName(string username);
        Task<string> GetCurrentFullNameByUserId(string userId);
        Task<string> GetPasswordCurrentUserName(string username);
        bool GetStatusUser(string username);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
    }
}
