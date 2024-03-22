using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Entities;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoResult>> GetUserListAsync();
        Task<UserInfoResult?> GetUserDetailsByUserNameAsyncs(string userName);
        Task<IEnumerable<string>> GetUsernameListAsync();
        Task<GeneralServiceResponseDto> UpdateInformation(UpdateInformation updateUser, string userId);
        void ChangePassword(ChangePassword changePassword, string userID);
        Task<GeneralServiceResponseDto> UpdateUser(UpdateStatusUser updateStatusUser, string nickName);
    }
}
