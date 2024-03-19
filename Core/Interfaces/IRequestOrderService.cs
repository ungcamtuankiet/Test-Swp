using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Dtos.RequestOrder;

namespace be_artwork_sharing_platform.Core.Interfaces
{
    public interface IRequestOrderService
    {
        Task<RequestOrderDto> GetRequestById(long id);
        Task SendRequesrOrder(SendRequest sendRequest, string userName_Request, string userId_Receivier, string fullName_Sender, string fullName_Receivier);
        IEnumerable<ReceiveRequestDto> GetMineOrderByUserId(string user_Name);
        IEnumerable<RequestOrderDto> GetMineRequestByUserName(string user_Id);
        Task UpdateRquest(long id, UpdateRequest updateRequest, string user_Id);
        Task CancelRequestByReceivier(long id, CancelRequest cancelRequest, string user_Id);
        Task UpdateStatusRequest(long id, string user_Id, UpdateStatusRequest updateStatusRequest);
        int DeleteRequestBySender(long id, string user_Name);
        StatusRequest GetStatusRequestByUserNameRequest(long id, string userNames);
        bool GetActiveRequestByUserNameRequest(long id, string userNames);
    }
}
