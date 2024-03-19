using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.RequestOrder;
using Microsoft.EntityFrameworkCore;

namespace be_artwork_sharing_platform.Core.Services
{
    public class RequestOrderService : IRequestOrderService
    {
        private readonly ApplicationDbContext _context;

        public RequestOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RequestOrderDto> GetRequestById(long id)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(o => o.Id == id);
            if (request == null)
            {
                return null;
            }
            var requestDto = new RequestOrderDto()
            {
                Id = request.Id,
                NickName_Sender = request.NickName_Sender,
                NickName_Receivier = request.NickName_Receivier,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Text = request.Text,
                CreatedAt = request.CreatedAt,
                StatusRequest = request.StatusRequest,
                IsActive = request.IsActive,
                IsDeleted = request.IsDeleted,
            };
            return requestDto;
        }

        public async Task SendRequesrOrder(SendRequest sendRequest, string userName_Request, string userId_Receivier, string fullName_Sender, string fullName_Receivier)
        {
            var request = new RequestOrder
            {
                NickName_Sender = fullName_Sender,
                NickName_Receivier = fullName_Receivier,
                UserName_Sender = userName_Request,
                UserId_Receivier = userId_Receivier,
                Email = sendRequest.Email,
                PhoneNumber = sendRequest.PhoneNumber,
                Text = sendRequest.Text
            };
            await _context.RequestOrders.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<ReceiveRequestDto> GetMineOrderByUserId(string user_Id)
        {
            var receivier = _context.RequestOrders.Where(f => f.UserId_Receivier == user_Id)
                .Select(f => new ReceiveRequestDto
                {
                    Id =f.Id,
                    NickName_Sender = f.NickName_Sender,
                    Email_Sender = f.Email,
                    PhoneNo_Sender = f.PhoneNumber,
                    Text = f.Text,
                    CreatedAt = f.CreatedAt,
                    IsActive = f.IsActive,
                    IsDeleted = f.IsDeleted,
                    StatusRequest = f.StatusRequest.ToString(),
                }).ToList();
            return receivier;
        }

        public IEnumerable<RequestOrderDto> GetMineRequestByUserName(string user_Name)
        {
            var request = _context.RequestOrders.Where(f => f.UserName_Sender == user_Name)
                .Select(f => new RequestOrderDto
                {
                    Id = f.Id,
                    NickName_Sender = f.NickName_Sender,
                    NickName_Receivier = f.NickName_Receivier,
                    Email = f.Email,
                    PhoneNumber = f.PhoneNumber,
                    Text = f.Text,
                    CreatedAt= f.CreatedAt,
                    IsActive = f.IsActive,
                    IsDeleted = f.IsDeleted,
                    StatusRequest = f.StatusRequest,
                }).ToList();
            return request;
        }

        public async Task UpdateRquest(long id, UpdateRequest updateRequest, string user_Id)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(f => f.Id == id && f.UserId_Receivier == user_Id);
            if(request is not null)
            {
                request.IsActive = updateRequest.IsActive;
            }
            _context.Update(request);
            _context.SaveChanges();
        }

        public async Task CancelRequestByReceivier(long id, CancelRequest cancelRequest, string user_Id)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id && r.UserId_Receivier == user_Id);
            if(request is not null)
            {
                request.IsDeleted = cancelRequest.IsDelete;
            }
            _context.Update(request);
            _context.SaveChanges();
        }

        public async Task UpdateStatusRequest(long id, string user_Id, UpdateStatusRequest updateStatusRequest)
        {
            var request = await _context.RequestOrders.FirstOrDefaultAsync(r => r.Id == id && r.UserId_Receivier == user_Id);
            if (request is not null)
            {
                request.StatusRequest = updateStatusRequest.StatusRequest;
            }
            _context.Update(request);
            await _context.SaveChangesAsync();
        }

        public StatusRequest GetStatusRequestByUserNameRequest(long id, string userName)
        {
            var checkStatusRequest = _context.RequestOrders.FirstOrDefault(r => r.Id == id && r.UserName_Sender == userName);
            return checkStatusRequest.StatusRequest;
        }

        public bool GetActiveRequestByUserNameRequest(long id, string userName)
        {
            var checkStatusRequest = _context.RequestOrders.FirstOrDefault(r => r.Id == id && r.UserName_Sender == userName);
            return checkStatusRequest.IsActive;
        }

        public int DeleteRequestBySender(long id, string user_Name)
        {
            var request = _context.RequestOrders.FirstOrDefault(o => o.Id == id && o.UserName_Sender == user_Name);
            if (request is not null)
            {
                _context.Remove(request);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
