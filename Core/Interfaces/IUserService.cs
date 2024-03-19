using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Entities;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoResult>> GetUserListAsync();
        Task<UserInfoResult?> GetUserDetailsByUserNameAsyncs(string userName);
        Task<IEnumerable<string>> GetUsernameListAsync();
        Task UpdateInformation(UpdateInformation updateUser, string userId);
        void ChangePassword(ChangePassword changePassword, string userID);
        Task UpdateUser(UpdateStatusUser updateStatusUser, string userId);
    }
}
