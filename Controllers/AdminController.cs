using AutoMapper;
using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.DbContext;
using be_artwork_sharing_platform.Core.Dtos.Artwork;
using be_artwork_sharing_platform.Core.Dtos.Category;
using be_artwork_sharing_platform.Core.Dtos.General;
using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Entities;
using be_artwork_sharing_platform.Core.Interfaces;
using be_artwork_sharing_platform.Core.Services;
using be_project_swp.Core.Dtos.Artwork;
using be_project_swp.Core.Dtos.RequestOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;

namespace be_project_swp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IArtworkService _artworkService;
        private readonly IAuthService _authService;
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdminController(IArtworkService artworkService, IAuthService authService, ILogService logService, IMapper mapper, IUserService userService)
        {
            _artworkService = artworkService;
            _authService = authService;
            _logService = logService;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        [Route("get-artwork-for-admin")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> GetArtworkForAdmin(string? getBy)
        {
            var artworks = await _artworkService.GetArtworkForAdmin(getBy);
            return Ok(_mapper.Map<List<GetArtworkByUserId>>(artworks));
        }

        [HttpPatch]
        [Route("accept-artwork")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<GeneralServiceResponseDto>> AcceptArtwork(long id)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                await _logService.SaveNewLog(userId, "Accept Artwork");
                var result = await _artworkService.AcceptArtwork(id);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("refuse-artwork")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<ActionResult<GeneralServiceResponseDto>> RefuseArtwork(long id, RefuseArtwork refuseArtwork)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                await _logService.SaveNewLog(userName, "Refuse Artwork");
                var result = await _artworkService.RefuseArtwork(id, refuseArtwork);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("update-status-user")]
        [Authorize(Roles = StaticUserRole.ADMIN)]
        public async Task<IActionResult> UpdateStatusUser(UpdateStatusUser updateUser, string nickName)
        {
            try
            {
                string userName = HttpContext.User.Identity.Name;
                string userId = await _authService.GetCurrentUserId(userName);
                await _logService.SaveNewLog(userName, "Update Status User");
                var result = await _userService.UpdateUser(updateUser, nickName);
                return StatusCode(result.StatusCode, result.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
