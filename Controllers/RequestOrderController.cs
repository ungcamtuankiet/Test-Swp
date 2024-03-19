using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.RequestOrder;
using be_artwork_sharing_platform.Core.Interfaces;
using be_project_swp.Core.Dtos.RequestOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace be_artwork_sharing_platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestOrderController : ControllerBase
    {
        private readonly IRequestOrderService _requestOrderService;
        private readonly ILogService _logService;
        private readonly IAuthService _authService;

        public RequestOrderController(IRequestOrderService requestOrderService, ILogService logService, IAuthService authService)
        {
            _requestOrderService = requestOrderService;
            _logService = logService;
            _authService = authService;
        }

        [HttpGet]
        [Route("get-by-id")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetRequestOrderById(long id)
        {
            try
            {
                var result = await _requestOrderService.GetRequestById(id);
                if (result == null)
                    return NotFound("Not found Request");
                return Ok(result);
            }
            catch
            {
                return BadRequest("Get Request Failed");
            }
        }

        [HttpPost]
        [Route("send-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> SendRequestOrder(SendRequest sendRequest, string user_Id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                string fullNameResquest = await _authService.GetCurrentFullName(userName);
                string currentUserNameRequest = await _authService.GetCurrentUserName(userName);
                string fullNameReceivier = await _authService.GetCurrentFullNameByUserId(user_Id);
                if(userId == user_Id)
                {
                    return BadRequest(new GeneralServiceResponseDto()
                    {
                        IsSucceed = false,
                        StatusCode = 400,
                        Message = "You can not request you"
                    });
                }
                else
                {
                    await _logService.SaveNewLog(userId, "Send Request Order");
                    await _requestOrderService.SendRequesrOrder(sendRequest, currentUserNameRequest, user_Id, fullNameResquest, fullNameReceivier);
                    return Ok(new GeneralServiceResponseDto()
                    {
                        IsSucceed = true,
                        StatusCode = 200,
                        Message = "Send Request to Order Artwork Successfully"
                    });
                }
            }
            catch
            {
                return BadRequest("Send Request Failed");
            }
        }

        [HttpGet]
        [Route("get-mine-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetRequestOfMines()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var result = _requestOrderService.GetMineRequestByUserName(userName);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpGet]
        [Route("get-mine-order")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> GetOrderOfMines()
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var result = _requestOrderService.GetMineOrderByUserId(userId);
                return Ok(result);
            }
            catch
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpPatch]
        [Route("cancel-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> CancelRequest(long id)
        {
            try
            {
                var cancelRequest = new CancelRequest();
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                await _logService.SaveNewLog(userId, "Cancel Request");
                await _requestOrderService.CancelRequestByReceivier(id, cancelRequest, userId);
                return Ok("Cancel Request Successfully");
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPatch]
        [Route("update-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> UpdateRequest(long id)
        {
            try
            {
                var updateRequest = new UpdateRequest();
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                await _logService.SaveNewLog(userId, "Update Request");
                await _requestOrderService.UpdateRquest(id, updateRequest, userId);
                return Ok("Update Request Successfully");
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPatch]
        [Route("update-status-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> UpdateStatusRequest(long id, UpdateStatusRequest updateStatusRequest)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                await _requestOrderService.UpdateStatusRequest(id, userId, updateStatusRequest);
                await _logService.SaveNewLog(userId, "Update Status Request");
                return Ok("Update Request Successfully");
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpDelete]
        [Route("delete-request")]
        [Authorize(Roles = StaticUserRole.CREATOR)]
        public async Task<IActionResult> DeleteRequest(long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                var checkStatusRequest = _requestOrderService.GetStatusRequestByUserNameRequest(id, userName);
                bool checkActiveRequest = _requestOrderService.GetActiveRequestByUserNameRequest(id, userName);
                if(checkActiveRequest)
                {
                    return BadRequest("Your request has been confirmed by the receiver or is in progress so do not delete this request!!!!!");
                }
                else
                {
                    var result = _requestOrderService.DeleteRequestBySender(id, userName);
                    if (result == 0) return NotFound("Request Not Found");
                    await _logService.SaveNewLog(userId, "Delete Request");
                    return Ok("Delete Request Successfully");
                }
            }
            catch
            {
                return BadRequest("Delete Request Failed");
            }
        }
    }
}
